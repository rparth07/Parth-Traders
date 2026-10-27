import { FormControl, Validators } from '@angular/forms';
export type RegistrationField = 'username' | 'phone' | 'address' | 'email' | 'password';
export type RegistrationFieldErrors = Partial<Record<RegistrationField, string[]>>;
export interface RegistrationResult { success: boolean; message?: string; fieldErrors?: RegistrationFieldErrors; }
export interface RegistrationInput { username: string; phone: string; address: string; email: string; password: string; }
export const phoneGuidance = 'Enter a 10-digit local number, or an international number beginning with + and its country code.';
export const usernameGuidance = 'Use 2-100 letters, numbers, dots, underscores, @, + or hyphens. Spaces are not allowed.';
export function normalizeRegistrationPhone(value: string): string | null {
  if (value.length > 32) return null;
  const compact = value.replace(/[\s().-]/g, '');
  return /^(?:[0-9]{10}|[+][1-9][0-9]{6,14})$/.test(compact) ? compact : null;
}
export function validateRegistration(input: RegistrationInput): RegistrationFieldErrors {
  const errors: RegistrationFieldErrors = {};
  const username = input.username.trim(), email = input.email.trim(), address = input.address.trim();
  if (username.length < 2 || username.length > 100 || !/^[A-Za-z0-9._@+-]+$/.test(username)) errors.username = [usernameGuidance];
  if (!normalizeRegistrationPhone(input.phone)) errors.phone = [phoneGuidance];
  if (!address) errors.address = ['Enter your address.'];
  else if (address.length > 500) errors.address = ['Use no more than 500 characters for your address.'];
  if (!email || email.length > 254 || Validators.email(new FormControl(email))) errors.email = ['Enter a valid email address.'];
  const passwordErrors: string[] = [];
  if (input.password.length < 10) passwordErrors.push('Use at least 10 characters in your password.');
  if (!/[0-9]/.test(input.password)) passwordErrors.push('Include at least one number in your password.');
  if (passwordErrors.length) errors.password = passwordErrors;
  return errors;
}
export function registrationFailure(error: any): RegistrationResult {
  if (error?.status === 0 || error?.name === 'TimeoutError') return {success:false,message:'We cannot reach the account service. Check your connection and try again. Your entries are still on this page.'};
  if (error?.status === 429) return {success:false,message:'Too many registration attempts. Wait a moment and try again. Your entries are still on this page.'};
  if (error?.status >= 500) return {success:false,message:'Account creation is temporarily unavailable. Your details have not been rejected. Please try again in a moment.'};
  const fieldErrors: RegistrationFieldErrors = {};
  const raw = error?.error?.errors || error?.error;
  if (raw && typeof raw === 'object') for (const [key, value] of Object.entries(raw)) {
    const messages = (Array.isArray(value) ? value : [value]).filter((v): v is string => typeof v === 'string');
    for (const original of messages) {
      const code = key.toLowerCase().replace(/[^a-z]/g, '');
      let field: RegistrationField | undefined;
      if (code.includes('username')) field='username';
      else if (code.includes('password')) field='password';
      else if (code.includes('phone')) field='phone';
      else if (code.includes('email')) field='email';
      else if (code.includes('address')) field='address';
      else if (code==='customerdto') field=/phone/i.test(original)?'phone':/email/i.test(original)?'email':/name/i.test(original)?'username':undefined;
      if (!field) continue;
      const message=code==='invalidusername'?usernameGuidance:code==='duplicateusername'?'This username is already in use. Choose another username.':code==='duplicateemail'?'An account already uses this email address. Sign in, or use another email.':original.replace(/^Validation Error *:? */i,'').slice(0,400);
      (fieldErrors[field] ||= []).push(message);
    }
  }
  if (Object.keys(fieldErrors).length) return {success:false,message:'Please correct the highlighted fields.',fieldErrors};
  return {success:false,message:error?.status===409?'These details conflict with an existing account. Sign in, or use different account details.':'Account creation was not completed. Please try again, or contact Parth Traders for help.'};
}
