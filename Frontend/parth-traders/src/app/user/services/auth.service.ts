import { Injectable } from '@angular/core';
import { Customer } from '../core/models/Customer';
import { catchError, Observable, Subject, tap, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenService } from './token.service';

const API_URL = 'https://localhost:5031/';
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
    let isLoginSuccessful = false;
    this.http.post<any>(API_URL + 'api/user/authentication/login', body)
      .subscribe({
        next: (res) => {
          this.tokenService.saveToken(res.token);
          this.tokenService.saveRefreshToken(res.refresh_token);
          this.tokenService.saveCustomer(res.customer);
          this.customer = res.customer;
          this.customer!.token = res.token;
          this.router
            .navigate(['/user/home'])
            .then((_) => console.log('You are secure now!'));

          isLoginSuccessful = true;
        },
        error: (err) => {
          console.log(err);
        },
      });

    if (isLoginSuccessful) {
      return { success: true };
    }
    return { success: false };
  }

  async registerApi(username: string, email: string, password: string): Promise<any> {
    // Replace with actual API call
    this.removeAllTokens();

    const body: { userName: string, email: string, password: string } = {
      userName: username,
      email: email,
      password: password,
    };

    return this.http.post<any>(API_URL + 'api/user/authentication', body)
      .subscribe({
        next: () => {
          catchError(AuthService.handleError)
        },
        error: catchError(AuthService.handleError),
      });
  }

  async forgotPasswordApi(email: string): Promise<any> {
    // Replace with actual API call

    const body: { email: string } = {
      email: email,
    };

    return this.http.post<any>(API_URL + 'api/user/forgot-password', body)
      .subscribe({
        next: () => {
          catchError(AuthService.handleError)
        },
        error: catchError(AuthService.handleError),
      });
  }

  isAuthenticated(): boolean {
    const token = this.tokenService.getToken();

    return !this.jwtHelper.isTokenExpired(token as string);
  }

  updateProfile(customer: Customer): Observable<any> {
    return this.http
      .post<any>(
        API_URL + 'api/authentication/customer-details/' + customer.id,
        customer
      )
      .pipe(
        tap((res) => {
          this.tokenService.saveToken(res.token);
          this.tokenService.saveRefreshToken(res.refresh_token);
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

    return this.http.post<any>(API_URL + 'api/authentication/login', body).pipe(
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
    this.tokenService.removeCustomer();
    this.tokenService.removeToken();
    this.tokenService.removeRefreshToken();
  }

  private static handleError(error: HttpErrorResponse): any {
    console.log('error = ', error);
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` + `body was: ${error.error}`
      );
    }

    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
