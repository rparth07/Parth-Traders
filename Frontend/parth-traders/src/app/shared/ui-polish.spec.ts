import { Router } from '@angular/router';
import { UserRoutingModule } from '../user/user-routing.module';
import { AuthGuardService } from '../user/services/auth-guard.service';
import { OrderService } from '../user/services/order.service';
import { CommonModule } from '@angular/common';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticateComponent } from '../user/authenticate/authenticate.component';
import { HeaderComponent } from '../user/header/header.component';
import { CheckoutComponent } from '../user/checkout/checkout.component';
import { CartService } from '../user/services/cart.service';
import { ProductService } from '../user/services/product.service';
import { AuthService } from '../user/services/auth.service';
import { Product } from '../user/core/models/Product';
import { AppSetting } from './app-settings';
const product={productId:1,productSku:'UI-ONLY',productName:'UI test fixture',unitPrice:20,discount:10,unitsInStock:5} as Product;

describe('Session cart recovery',()=>{
  beforeEach(()=>sessionStorage.clear());afterEach(()=>sessionStorage.clear());
  it('restores a removed item, its quantity, position, total and persisted cart counter',()=>{
    const cart=new CartService();cart.addToCart(product);cart.addToCart(product);cart.addToCart({...product,productSku:'SECOND'});
    const removed=cart.removeFromCartAt(0)!;expect(cart.getAllProductCountEvent().value).toBe(1);
    expect(cart.restoreCartItem(removed,0)).toBeTrue();expect(cart.getCartItems()[0].quantity).toBe(2);expect(cart.getTotalOrderPrice()).toBe(54);
    const restored=new CartService();expect(restored.getAllProductCountEvent().value).toBe(3);expect(restored.getCartItems()[0].productSku).toBe('UI-ONLY');
    expect(cart.restoreCartItem(removed,0)).toBeFalse();expect(cart.getAllProductCountEvent().value).toBe(3);
  });
  it('offers undo when decreasing the last item to zero and safely ignores stale item callbacks',()=>{
    const cart=new CartService();cart.addToCart(product);
    const view=new CheckoutComponent({initializeOrder:()=>{}} as any,cart,{} as any);view.sync();const item=view.orderItems[0];
    view.changeQuantity(item,false);expect(view.orderItems.length).toBe(0);expect(view.lastRemoved?.item.productSku).toBe('UI-ONLY');
    cart.decrementProductCountFrom(item);expect(cart.getCartItems().length).toBe(0);
    view.undoRemoval();expect(view.orderItems.length).toBe(1);expect(view.totalOrderPrice).toBe(18);expect(view.lastRemoved).toBeUndefined();
  });
});

describe('Linked account error summary',()=>{
  let fixture:ComponentFixture<AuthenticateComponent>;
  beforeEach(fakeAsync(()=>{localStorage.clear();TestBed.configureTestingModule({imports:[CommonModule,FormsModule,RouterTestingModule],declarations:[AuthenticateComponent],providers:[{provide:AuthService,useValue:{registerApi:jasmine.createSpy(),loginApi:jasmine.createSpy()}}]});fixture=TestBed.createComponent(AuthenticateComponent);fixture.detectChanges();tick();fixture.detectChanges();}));
  afterEach(()=>{fixture.destroy();localStorage.clear();});
  function settle(){fixture.detectChanges();tick();fixture.detectChanges();tick();fixture.detectChanges();}
  it('focuses a linked registration summary and moves to a field without navigating',fakeAsync(()=>{
    fixture.componentInstance.registering=true;settle();fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));settle();
    const summary=fixture.nativeElement.querySelector('.auth-error');expect(document.activeElement).toBe(summary);expect(summary.querySelectorAll('a').length).toBe(5);
    const before=location.href;summary.querySelector('a[href="#register-email"]').click();settle();expect(document.activeElement?.id).toBe('register-email');expect(location.href).toBe(before);
    expect(fixture.nativeElement.querySelector('#email-errors').textContent).toContain('valid email');
  }));
  it('connects both missing sign-in fields to summary links and inline errors',fakeAsync(()=>{
    fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));settle();
    const summary=fixture.nativeElement.querySelector('.auth-error');expect(document.activeElement).toBe(summary);expect(summary.querySelectorAll('a').length).toBe(2);
    expect(fixture.nativeElement.querySelector('#login-password-errors').textContent).toContain('Enter your password');
    summary.querySelector('a[href="#login-password"]').click();expect(document.activeElement?.id).toBe('login-password');
  }));
});

describe('Header filter consistency',()=>{
  let fixture:ComponentFixture<HeaderComponent>;let service:ProductService;let http:HttpTestingController;
  beforeEach(fakeAsync(()=>{sessionStorage.clear();TestBed.configureTestingModule({imports:[CommonModule,FormsModule,HttpClientTestingModule,RouterTestingModule],declarations:[HeaderComponent],schemas:[NO_ERRORS_SCHEMA],providers:[{provide:AuthService,useValue:{getCustomerName:()=>'',isAuthenticated:()=>false,logout:()=>{}}}]});http=TestBed.inject(HttpTestingController);service=TestBed.inject(ProductService);fixture=TestBed.createComponent(HeaderComponent);fixture.componentInstance.shouldDisplaySearchBox=true;fixture.detectChanges();http.expectOne(AppSetting.API_URL+'API/admin/categories').flush([{categoryName:'Hand Tools'},{categoryName:'Power Tools'}]);tick();fixture.detectChanges();}));
  afterEach(()=>{http.verify();fixture.destroy();sessionStorage.clear();});
  function settle(){fixture.detectChanges();tick();fixture.detectChanges();tick();fixture.detectChanges();}
  it('shows multiple selected categories and preserves them when searching or clearing text',fakeAsync(()=>{
    service.productFilter$.next({...service.productFilter$.value,categories:['Hand Tools','Power Tools']});settle();
    const select=fixture.nativeElement.querySelector('select') as HTMLSelectElement;expect(select.selectedOptions[0].textContent).toContain('2 selected');
    const input=fixture.nativeElement.querySelector('input[type="search"]') as HTMLInputElement;input.value='drill';input.dispatchEvent(new Event('input'));settle();fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));settle();
    expect(service.productFilter$.value.categories.length).toBe(2);expect(service.productFilter$.value.name).toBe('drill');
    input.value='';input.dispatchEvent(new Event('input'));settle();expect(service.productFilter$.value.name).toBe('');expect(service.productFilter$.value.categories.length).toBe(2);
  }));
  it('applies a native category selection and labels a singular cart item correctly',fakeAsync(()=>{
    const select=fixture.nativeElement.querySelector('select') as HTMLSelectElement;select.value='Hand Tools';select.dispatchEvent(new Event('change'));settle();expect(service.productFilter$.value.categories).toEqual(['Hand Tools']);
    TestBed.inject(CartService).addToCart(product);settle();expect(fixture.nativeElement.querySelector('.cart-link').getAttribute('aria-label')).toBe('Shopping cart, 1 item');
  }));
});

describe('Approved guest cart boundary',()=>{
  afterEach(()=>sessionStorage.clear());
  it('allows only cart review while keeping payment and order routes guarded',()=>{
    TestBed.configureTestingModule({imports:[RouterTestingModule,UserRoutingModule]});
    const routes=TestBed.inject(Router).config;
    expect(routes.find(r=>r.path==='checkout')?.canActivate?.includes(AuthGuardService)).not.toBeTrue();
    for(const path of ['payment','confirm','orders','order/order-details']) expect(routes.find(r=>r.path===path)?.canActivate).toContain(AuthGuardService);
  });
  it('can construct and edit the session cart without a customer account',()=>{
    sessionStorage.clear();const cart=new CartService();cart.addToCart(product);
    const orders=new OrderService({getCustomer:()=>null,isAuthenticated:()=>false} as any,cart,{} as any);
    const view=new CheckoutComponent(orders,cart,{} as any);view.sync();expect(view.signedIn).toBeFalse();expect(view.orderItems.length).toBe(1);expect(view.totalOrderPrice).toBe(18);
    view.remove(0);view.undoRemoval();expect(view.orderItems.length).toBe(1);
  });
});
