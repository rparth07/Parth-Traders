import { CommonModule } from '@angular/common';
import { ComponentFixture, fakeAsync, flushMicrotasks, TestBed, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticateComponent } from '../user/authenticate/authenticate.component';
import { AuthService } from '../user/services/auth.service';
import { normalizeRegistrationPhone, registrationFailure, validateRegistration } from './registration-validation';
import { AppSetting } from './app-settings';
// Generate disposable fixture values; no account credential is embedded in source.
const fixturePassword=(length:number,includeDigit=true)=>Array.from(crypto.getRandomValues(new Uint8Array(length)),(value,index)=>includeDigit&&index===length-1?'7':String.fromCharCode(97+value%26)).join('');
const shortPassword=fixturePassword(8);
const letterPassword=fixturePassword(10,false);
const valid={username:'signup-fixture',phone:'+91 98765 43210',address:'Test-only fixture address',email:'signup@example.invalid',password:fixturePassword(10)};

describe('Registration validation and response handling',()=>{
  it('uses the actual ten-character/digit policy without inventing other password requirements',()=>{
    expect(validateRegistration(valid)).toEqual({});
    expect(validateRegistration({...valid,password:shortPassword}).password?.join()).toContain('10 characters');
    expect(validateRegistration({...valid,password:letterPassword}).password?.join()).toContain('number');
  });
  it('accepts formatted local/international numbers and rejects incomplete numbers',()=>{
    expect(normalizeRegistrationPhone('+91 (98765)-43210')).toBe('+919876543210');
    expect(normalizeRegistrationPhone('987-654-3210')).toBe('9876543210');
    expect(normalizeRegistrationPhone('1234')).toBeNull();expect(normalizeRegistrationPhone('+91abc')).toBeNull();
  });
  it('reports each invalid field and explains username spaces',()=>{
    const errors=validateRegistration({...valid,username:'Full Name',phone:'123',address:' ',email:'not-an-email',password:shortPassword});
    expect(Object.keys(errors).sort()).toEqual(['address','email','password','phone','username']);
    expect(errors.username?.join()).toContain('Spaces are not allowed');
  });
  it('preserves structured field errors and supports legacy model-state responses',()=>{
    expect(registrationFailure({status:409,error:{errors:{phone:['Already used.'],email:['Use another email.']}}}).fieldErrors).toEqual({phone:['Already used.'],email:['Use another email.']});
    expect(registrationFailure({status:400,error:{passwordTooShort:['Use 10 characters.']}}).fieldErrors?.password).toEqual(['Use 10 characters.']);
    expect(registrationFailure({status:400,error:{invalidUserName:['Raw submitted name should not be echoed']}}).fieldErrors?.username?.join()).not.toContain('Raw submitted');
  });
  it('distinguishes outages and rate limiting from details the user must correct',()=>{
    expect(registrationFailure({status:503}).message).toContain('temporarily unavailable');
    expect(registrationFailure({status:503}).fieldErrors).toBeUndefined();
    expect(registrationFailure({status:0}).message).toContain('connection');
    expect(registrationFailure({status:429}).message).toContain('Wait');
  });
});

describe('Registration HTTP responses',()=>{
  let http:HttpTestingController;
  beforeEach(()=>{localStorage.clear();TestBed.configureTestingModule({imports:[HttpClientTestingModule],providers:[{provide:Router,useValue:{navigate:jasmine.createSpy().and.resolveTo(true)}},{provide:JwtHelperService,useValue:{isTokenExpired:()=>true}}]});http=TestBed.inject(HttpTestingController);});
  afterEach(()=>{http.verify();localStorage.clear();});
  it('normalizes formatted phones and returns actionable server errors to the form',fakeAsync(()=>{
    let result:any;TestBed.inject(AuthService).registerApi(valid.username,valid.phone,valid.address,valid.email,valid.password).then(r=>result=r);
    const request=http.expectOne(AppSetting.API_URL+'api/user/authentication/signup');expect(request.request.body.phoneNumber).toBe('+919876543210');expect(request.request.body.password).toBe(valid.password);
    request.flush({errors:{email:['This email is already in use.']}},{status:409,statusText:'Conflict'});flushMicrotasks();expect(result.success).toBeFalse();expect(result.fieldErrors.email[0]).toContain('already in use');
  }));
});

describe('Registration form corrections',()=>{
  let fixture:ComponentFixture<AuthenticateComponent>;let auth:any;
  beforeEach(fakeAsync(()=>{localStorage.clear();auth={registerApi:jasmine.createSpy().and.resolveTo({success:true}),loginApi:jasmine.createSpy()};TestBed.configureTestingModule({imports:[CommonModule,FormsModule,RouterTestingModule],declarations:[AuthenticateComponent],providers:[{provide:AuthService,useValue:auth}]});fixture=TestBed.createComponent(AuthenticateComponent);fixture.componentInstance.registering=true;fixture.detectChanges();tick();fixture.detectChanges();}));
  afterEach(()=>{fixture.destroy();localStorage.clear();});
  function settle(){tick();fixture.detectChanges();tick();fixture.detectChanges();}
  function fill(values:any){for(const [name,value] of Object.entries(values)){const input=fixture.nativeElement.querySelector('[name="'+name+'"]') as HTMLInputElement;input.value=String(value);input.dispatchEvent(new Event('input'));settle();}}
  function submit(){fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));settle();}
  it('shows specific corrections at invalid fields and keeps every entry',fakeAsync(()=>{
    fill({...valid,username:'Full Name',phone:'1234',password:shortPassword});submit();
    expect(auth.registerApi).not.toHaveBeenCalled();expect(fixture.nativeElement.querySelector('#username-errors').textContent).toContain('Spaces are not allowed');
    expect(fixture.nativeElement.querySelector('#password-errors').textContent).toContain('10 characters');expect(fixture.nativeElement.querySelector('#phone-errors').textContent).toContain('country code');
    expect(fixture.componentInstance.registration.email).toBe(valid.email);expect(fixture.componentInstance.registration.password).toBe(shortPassword);
  }));
  it('shows a duplicate field response without clearing entries and clears it when edited',fakeAsync(()=>{
    auth.registerApi.and.resolveTo({success:false,message:'Please correct the highlighted fields.',fieldErrors:{email:['This email is already in use.']}});
    fill(valid);submit();expect(fixture.nativeElement.querySelector('#email-errors').textContent).toContain('already in use');expect(fixture.nativeElement.querySelector('#register-email').getAttribute('aria-invalid')).toBe('true');expect(fixture.componentInstance.registration.password).toBe(valid.password);
    fill({email:'corrected@example.invalid'});expect(fixture.nativeElement.querySelector('#email-errors').textContent).toBe('');
  }));
  it('creates a valid account and displays a sign-in action while clearing the used password',fakeAsync(()=>{
    fill(valid);submit();expect(auth.registerApi).toHaveBeenCalledTimes(1);expect(fixture.componentInstance.registering).toBeFalse();expect(fixture.nativeElement.textContent).toContain('Your account has been created');expect(fixture.componentInstance.registration.password).toBe('');
  }));
  it('keeps entries and distinguishes a service outage from invalid fields',fakeAsync(()=>{
    auth.registerApi.and.resolveTo(registrationFailure({status:503}));fill(valid);submit();expect(fixture.nativeElement.textContent).toContain('temporarily unavailable');expect(fixture.componentInstance.registrationErrors).toEqual({});expect(fixture.componentInstance.registration.email).toBe(valid.email);
  }));
});
