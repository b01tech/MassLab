import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
) => {
  const authService = inject(AuthService);
  const token = authService.accessToken();

  let authReq = req.clone({
    withCredentials: true,
  });

  if (token) {
    authReq = authReq.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`),
    });
  }

  return next(authReq).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        // Se for erro 401 na rota de login ou refresh, não tenta fazer refresh de novo
        if (req.url.includes('/login') || req.url.includes('/refresh-token')) {
          return throwError(() => error);
        }

        return handle401Error(authReq, next, authService);
      }
      return throwError(() => error);
    }),
  );
};

function handle401Error(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
) {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      switchMap((tokenResponse) => {
        isRefreshing = false;
        refreshTokenSubject.next(tokenResponse.accessToken);

        // Refaz a requisição original com o novo token
        const newRequest = request.clone({
          headers: request.headers.set('Authorization', `Bearer ${tokenResponse.accessToken}`),
        });

        return next(newRequest);
      }),
      catchError((err) => {
        isRefreshing = false;
        authService.logout();
        return throwError(() => err);
      }),
    );
  } else {
    // Se já estiver fazendo refresh, espera terminar e pega o novo token
    return refreshTokenSubject.pipe(
      filter((token) => token != null),
      take(1),
      switchMap((jwt) => {
        const newRequest = request.clone({
          headers: request.headers.set('Authorization', `Bearer ${jwt}`),
        });
        return next(newRequest);
      }),
    );
  }
}
