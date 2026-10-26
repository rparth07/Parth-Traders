import { Injectable } from '@angular/core';
import { Customer } from '../core/models/Customer';
import { catchError, firstValueFrom, Observable, Subject, tap, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenService } from './token.service';
import { AppSetting } from 'src/app/shared/app-settings';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private customer: Customer | null = null;
  redirectUrl = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    private tokenService: TokenService
  ) {
    if (this.isAuthenticated()) {
      this.customer = this.tokenService.getCustomer();
    } else {
      this.removeAllTokens();
    }
  }

  async loginApi(email: string, password: string): Promise<any> {
    this.removeAllTokens();

    const body: { email: string; password: string } = {
      email: email,
      password: password,
    };
    try {
      const res = await firstValueFrom(this.http.post<any>(AppSetting.API_URL + 'api/user/authentication/login', body));
      if (!res.token || !res.customer) throw new Error('Invalid login response');
      this.tokenService.saveToken(res.token);
      this.tokenService.saveCustomer(res.customer);
      this.customer = res.customer;
      await this.router.navigate(['/user/home']);
      return { success: true };
    } catch (error) {
      return { success: false, message: error instanceof HttpErrorResponse && error.status === 401
        ? 'Email or password is incorrect.' : 'Unable to sign in. Please try again or contact Parth Traders.' };
    }
  }

  async registerApi(username: string, phone: string, address: string, email: string, password: string): Promise<any> {
    try {
      await firstValueFrom(this.http.post(AppSetting.API_URL + 'api/user/authentication/signup', {
        userName: username, phoneNumber: phone, customerAddress: address, email, password
      }));
      return { success: true };
    } catch {
      return { success: false, message: 'Registration could not be completed. Check your details and try again.' };
    }
  }

  async forgotPasswordApi(email: string): Promise<any> {
    // The backend does not implement a password reset endpoint.
    return { success: false, message: 'Online password reset is unavailable. Contact Parth Traders for account help.' };
  }

  isAuthenticated(): boolean {
    const token = this.tokenService.getToken();

    try { return !!token && !!this.tokenService.getCustomer() && !this.jwtHelper.isTokenExpired(token); } catch { return false; }
  }

  updateProfile(customer: Customer): Observable<any> {
    return this.http
      .post<any>(
        AppSetting.API_URL + 'api/user/authentication/customer-details/' + customer.id,
        customer
      )
      .pipe(
        tap((res) => {
          if (res.token) this.tokenService.saveToken(res.token);
          if (res.refresh_token) this.tokenService.saveRefreshToken(res.refresh_token);
          this.tokenService.saveCustomer(res.customer);
          this.customer = res.customer;
        }),
        catchError(AuthService.handleError)
      );
  }

  //refactor this method
  refreshToken(refreshData: any): Observable<any> {
    this.removeAllTokens();

    const body = new HttpParams().set(
      'refresh_token',
      refreshData.refresh_token
    );

    return this.http.post<any>(AppSetting.API_URL + 'api/authentication/login', body).pipe(
      tap((res) => {
        this.tokenService.saveToken(res.token);
        this.tokenService.saveRefreshToken(res.refresh_token);
        this.tokenService.saveCustomer(res.customer);
        this.customer = res.customer;
        this.customer!.token = res.token;
      }),
      catchError(AuthService.handleError)
    );
  }

  logout(): void {
    this.removeAllTokens();
    this.router.navigate(['/user/authenticate']);
  }

  getCustomer() {
    return this.customer!;
  }

  getCustomerName(): string {
    return this.customer == null ? '' : this.customer.userName;
  }

  redirectToLogIn(): void {
    this.router.navigate(['/user/authenticate']);
  }

  removeAllTokens() {
    this.customer = null;
    this.tokenService.removeCustomer();
    this.tokenService.removeToken();
    this.tokenService.removeRefreshToken();
  }

  private static handleError(error: HttpErrorResponse): any {
    return throwError(() => error);
  }
}
