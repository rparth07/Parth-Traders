import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { catchError, Observable, Subject, tap } from 'rxjs';
import { throwError } from 'rxjs';
import { TokenService } from './token.service';
import decode from 'jwt-decode';
import { AdminDetails } from './profile/admin-details';

const API_URL = 'https://localhost:5031/';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  redirectUrl = '';
  adminSubject = new Subject<AdminDetails>();

  private static handleError(error: HttpErrorResponse): any {
    console.log(error);
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

  private static log(message: string): any {
    console.log(message);
  }

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    private tokenService: TokenService
  ) {}

  //reactor this method to first validate the token and then calculate the expiration date later
  isAuthenticated(): boolean {
    const token = this.tokenService.getToken();

    return !this.jwtHelper.isTokenExpired(token as string);
  }

  login(loginData: any): Observable<any> {
    this.tokenService.removeToken();
    this.tokenService.removeRefreshToken();
    const body: { username: string; password: string } = {
      username: loginData.username,
      password: loginData.password,
    };

    return this.http.post<any>(API_URL + 'api/authentication/login', body).pipe(
      tap((res) => {
        this.tokenService.saveToken(res.token);
        this.tokenService.saveRefreshToken(res.refresh_token);
        const adminDetails: AdminDetails = {
          firstName: res.admin.name,
          lastName: res.admin.lastName,
          id: res.admin.id,
          email: res.admin.email,
          phone: res.admin.phone,
          userName: res.admin.userName,
          password: res.admin.password,
          adminImage: res.admin.adminImage,
        };
        this.adminSubject.next(adminDetails);
      }),
      catchError(AuthService.handleError)
    );
  }
  //refactor this method
  refreshToken(refreshData: any): Observable<any> {
    this.tokenService.removeToken();
    this.tokenService.removeRefreshToken();
    const body = new HttpParams().set(
      'refresh_token',
      refreshData.refresh_token
    );

    return this.http.post<any>(API_URL + 'api/authentication/login', body).pipe(
      tap((res) => {
        this.tokenService.saveToken(res.token);
        this.tokenService.saveRefreshToken(res.refresh_token);
      }),
      catchError(AuthService.handleError)
    );
  }

  logout(): void {
    this.tokenService.removeToken();
    this.tokenService.removeRefreshToken();
    this.router.navigate(['/admin/login']);
  }
}
