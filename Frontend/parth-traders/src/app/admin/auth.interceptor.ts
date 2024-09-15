import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { TokenService } from './token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private tokenService: TokenService,
    private authService: AuthService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): any {
    const token = this.tokenService.getToken();
    const refreshToken = this.tokenService.getRefreshToken();

    // Add Authorization header if token is present
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: 'Bearer ' + token,
        },
      });
    }

    // Only set Content-Type header for JSON requests
    if (request.body && !(request.body instanceof FormData)) {
      request = request.clone({
        setHeaders: {
          'Content-Type': 'application/json',
        },
      });
    }

    // Set Accept header for all requests
    request = request.clone({
      headers: request.headers.set('Accept', 'application/json'),
    });

    return next.handle(request).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          console.log('event--->>>', event);
        }
        return event;
      }),
      catchError((error: HttpErrorResponse) => {
        console.log(error.error.error);
        if (error.status === 401) {
          if (error.error.error === 'invalid_token') {
            this.authService
              .refreshToken({ refresh_token: refreshToken })
              .subscribe(() => {
                location.reload();
              });
          } else {
            this.router
              .navigate(['/admin/login'])
              .then(() => console.log('redirect to login'));
          }
        }
        return throwError(error);
      })
    );
  }
}
