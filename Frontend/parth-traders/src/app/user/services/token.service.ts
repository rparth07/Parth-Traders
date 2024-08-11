import { Injectable } from '@angular/core';
import { Customer } from '../core/models/Customer';

const ACCESS_TOKEN = 'access_token';
const REFRESH_TOKEN = 'refresh_token';
const CUSTOMER_INFO = 'customer_info';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor() { }

  getToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN);
  }

  saveToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN, token);
  }

  saveRefreshToken(refreshToken: string): void {
    localStorage.setItem(REFRESH_TOKEN, refreshToken);
  }

  removeToken(): void {
    localStorage.removeItem(ACCESS_TOKEN);
  }

  removeRefreshToken(): void {
    localStorage.removeItem(REFRESH_TOKEN);
  }

  saveCustomer(customer: Customer): void {
    localStorage.setItem(CUSTOMER_INFO, JSON.stringify(customer));
  }

  getCustomer(): Customer | null {
    const customerInfo = localStorage.getItem(CUSTOMER_INFO);
    return customerInfo ? JSON.parse(customerInfo) : null;
  }

  removeCustomer(): void {
    localStorage.removeItem(CUSTOMER_INFO);
  }
}
