import { Injectable } from '@angular/core';
import { Customer } from './customer/customer';
import { AdminDetails } from './profile/admin-details';

const ACCESS_TOKEN = 'access_token';
const REFRESH_TOKEN = 'refresh_token';
const Admin_INFO = 'admin_info';

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

  saveAdmin(admin: AdminDetails): void {
    localStorage.setItem(Admin_INFO, JSON.stringify(admin));
  }

  getAdmin(): AdminDetails | null {
    const adminInfo = localStorage.getItem(Admin_INFO);
    return adminInfo ? JSON.parse(adminInfo) : null;
  }

  removeAdmin(): void {
    localStorage.removeItem(Admin_INFO);
  }
}
