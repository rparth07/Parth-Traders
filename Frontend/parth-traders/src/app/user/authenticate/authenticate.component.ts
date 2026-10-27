import { RegistrationField, RegistrationFieldErrors, validateRegistration } from '../../shared/registration-validation';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../services/auth.service';
@Component({ selector: 'app-authenticate', templateUrl: './authenticate.component.html', styleUrl: './authenticate.component.css' })
export class AuthenticateComponent {
    registering = false;
    submitting = false;
    showPassword = false;
    error = '';
    success = '';
    registrationErrors: RegistrationFieldErrors = {};
    loginErrors: Partial<Record<'email' | 'password', string[]>> = {};
    @ViewChild('errorSummary') errorSummary?: ElementRef<HTMLElement>;
    @ViewChild('authPanel') authPanel?: ElementRef<HTMLElement>;
    @ViewChild('registrationFields') registrationFields?: ElementRef<HTMLFormElement>;
    remember = !!localStorage.getItem('rememberedEmail');
    login = { email: localStorage.getItem('rememberedEmail') || '', password: '' };
    registration = { username: '', phone: '', address: '', email: '', password: '' };
    constructor(private auth: AuthService) { }
    toggleAuthForms() { if (this.submitting)
        return; this.registering = !this.registering; this.error = ''; this.success = ''; this.registrationErrors = {}; this.loginErrors = {}; this.showPassword = false; }
    async handleLogin(form: NgForm) {
        if (this.submitting)
            return;
        this.loginErrors = {};
        if (!this.login.email.trim() || form.controls['email']?.invalid) this.loginErrors.email = ['Enter a valid email address.'];
        if (!this.login.password) this.loginErrors.password = ['Enter your password.'];
        if (form.invalid || Object.keys(this.loginErrors).length) {
            this.error = 'Please correct the highlighted fields.';
            this.focusRegistrationError();
            return;
        }
        this.error = '';
        this.submitting = true;
        try {
            const result = await this.auth.loginApi(this.login.email.trim(), this.login.password);
            if (!result.success) { this.error = result.message; this.focusRegistrationError(); }
            else if (this.remember)
                localStorage.setItem('rememberedEmail', this.login.email.trim());
            else
                localStorage.removeItem('rememberedEmail');
        }
        catch {
            this.error = 'Sign-in could not be completed. Please try again.'; this.focusRegistrationError();
        }
        finally {
            this.submitting = false;
        }
    }
    async handleRegister(form: NgForm) {
        if (this.submitting)
            return;
        this.registrationErrors = validateRegistration(this.registration);
        if (form.invalid || Object.keys(this.registrationErrors).length) {
            this.error = 'Please correct the highlighted fields.';
            this.focusRegistrationError();
            return;
        }
        this.error = '';
        this.submitting = true;
        try {
            const r = this.registration;
            const result = await this.auth.registerApi(r.username.trim(), r.phone.trim(), r.address.trim(), r.email.trim(), r.password);
            if (result.success) {
                this.login.email = r.email;
                this.registering = false;
                this.registration.password = '';
                this.registrationErrors = {};
                this.success = 'Your account has been created. Sign in to continue.';
                this.showPassword = false;
            }
            else {
                this.error = result.message || 'Account creation was not completed.';
                this.registrationErrors = result.fieldErrors || {};
                this.focusRegistrationError();
            }
        }
        catch {
            this.error = 'Account creation could not be completed. Please try again.'; this.focusRegistrationError();
        }
        finally {
            this.submitting = false;
        }
    }
    clearRegistrationError(field: RegistrationField) {
        delete this.registrationErrors[field];
        if (!Object.keys(this.registrationErrors).length) this.error = '';
    }
    get errorLinks(): {id:string; label:string; message:string}[] {
        const fields = this.registering
            ? [{key:'username',id:'register-name',label:'Username'},{key:'phone',id:'register-phone',label:'Phone number'},{key:'address',id:'register-address',label:'Address'},{key:'email',id:'register-email',label:'Email address'},{key:'password',id:'register-password',label:'Password'}]
            : [{key:'email',id:'login-email',label:'Email address'},{key:'password',id:'login-password',label:'Password'}];
        const errors:RegistrationFieldErrors = this.registering ? this.registrationErrors : this.loginErrors;
        return fields.filter(f=>errors[f.key as RegistrationField]?.length).map(f=>({id:f.id,label:f.label,message:errors[f.key as RegistrationField]!.join(' ')}));
    }
    focusErrorField(id:string,event:Event) {
        event.preventDefault(); this.authPanel?.nativeElement.querySelector<HTMLElement>('#'+id)?.focus();
    }
    clearLoginError(field:'email'|'password') {
        delete this.loginErrors[field]; if(!Object.keys(this.loginErrors).length) this.error='';
    }
    private focusRegistrationError() {
        setTimeout(() => this.errorSummary?.nativeElement.focus(), 0);
    }
}
