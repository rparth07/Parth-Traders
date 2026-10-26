import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError, timeout } from 'rxjs';
import { AppSetting } from '../shared/app-settings';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}
  intercept(request: HttpRequest<unknown>, next: HttpHandler) {
    // Only send credentials to the configured API, never to third-party assets.
    if (!request.url.startsWith(AppSetting.API_URL)) return next.handle(request);
    const path = request.url.slice(AppSetting.API_URL.length).toLowerCase();
    const admin = path.startsWith('api/admin/') || path.startsWith('api/authentication/');
    const prefix = admin ? 'admin_' : '';
    const token = localStorage.getItem(prefix + 'access_token');
    if (token) request = request.clone({setHeaders: {Authorization: 'Bearer ' + token}});
    return next.handle(request).pipe(
      timeout(request.method === 'GET' ? 90000 : 75000),
      catchError((error: HttpErrorResponse) => {
        const authenticating = /\/(login|signup)$/.test(path);
        if (error.status === 401 && !authenticating && token) {
          localStorage.removeItem(prefix + 'access_token');
          localStorage.removeItem(prefix + 'refresh_token');
          localStorage.removeItem(admin ? 'admin_info' : 'customer_info');
          void this.router.navigate([admin ? '/admin/login' : '/user/authenticate']);
        }
        return throwError(() => error);
      })
    );
  }
}
