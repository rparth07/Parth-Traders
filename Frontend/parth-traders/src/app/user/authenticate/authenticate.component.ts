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
    @ViewChild('registrationFields') registrationFields?: ElementRef<HTMLFormElement>;
    remember = !!localStorage.getItem('rememberedEmail');
    login = { email: localStorage.getItem('rememberedEmail') || '', password: '' };
    registration = { username: '', phone: '', address: '', email: '', password: '' };
    constructor(private auth: AuthService) { }
    toggleAuthForms() { if (this.submitting)
        return; this.registering = !this.registering; this.error = ''; this.success = ''; this.registrationErrors = {}; this.showPassword = false; }
    async handleLogin(form: NgForm) {
        if (this.submitting)
            return;
        if (form.invalid) {
            this.error = 'Enter a valid email address and your password.';
            return;
        }
        this.error = '';
        this.submitting = true;
        try {
            const result = await this.auth.loginApi(this.login.email.trim(), this.login.password);
            if (!result.success)
                this.error = result.message;
            else if (this.remember)
                localStorage.setItem('rememberedEmail', this.login.email.trim());
            else
                localStorage.removeItem('rememberedEmail');
        }
        catch {
            this.error = 'Sign-in could not be completed. Please try again.';
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
            this.error = 'Account creation could not be completed. Please try again.';
        }
        finally {
            this.submitting = false;
        }
    }
    clearRegistrationError(field: RegistrationField) {
        delete this.registrationErrors[field];
        if (!Object.keys(this.registrationErrors).length) this.error = '';
    }
    private focusRegistrationError() {
        setTimeout(() => this.registrationFields?.nativeElement.querySelector<HTMLElement>('[aria-invalid="true"]')?.focus(), 0);
    }
}
