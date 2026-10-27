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
import { AdminDetails } from './profile/admin-details';

import { AppSetting } from '../shared/app-settings';
const API_URL = AppSetting.API_URL;
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  redirectUrl = '';
  admin: AdminDetails | null;
  adminName = new Subject<string>();

  private static handleError(error: HttpErrorResponse): any {
    return throwError(() => error);
  }

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    private tokenService: TokenService
  ) {
    this.admin = tokenService.getAdmin();
  }

  //reactor this method to first validate the token and then calculate the expiration date later
  isAuthenticated(): boolean {
    const token = this.tokenService.getToken();

    try { return !!token && !!this.tokenService.getAdmin() && !this.jwtHelper.isTokenExpired(token); } catch { return false; }
  }

  updateProfile(admin: AdminDetails): Observable<any> {
    return this.http
      .post<any>(
        API_URL + 'api/authentication/admin-details/' + admin.id,
        admin
      )
      .pipe(
        tap((res) => {
          if (res.token) this.tokenService.saveToken(res.token);
          if (res.refresh_token) this.tokenService.saveRefreshToken(res.refresh_token);
          this.tokenService.saveAdmin(res.admin);
          this.admin = res.admin;
          this.adminName.next(this.admin!.userName);
        }),
        catchError(AuthService.handleError)
      );
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
        this.tokenService.saveAdmin(res.admin);
        this.admin = res.admin;
        this.adminName.next(this.admin!.userName);
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
