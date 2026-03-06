import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environmets/environment';
import { TokenPayload } from '../models/token-payload';
import { TokenResponse } from '../models/token-response';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _accessToken = signal<string | null>(null);

  loggedUser = signal<User | null>(null);

  private readonly _httpClient = inject(HttpClient);

  get accessToken() {
    return this._accessToken.asReadonly();
  }

  login(username: string, password: string): Observable<TokenResponse> {
    return this._httpClient
      .post<TokenResponse>(
        `${environment.apiBaseUrl}/login`,
        {
          username,
          password,
        },
        { withCredentials: true },
      ) // send cookies
      .pipe(
        tap((response) => {
          this.setSession(response.accessToken);
        }),
      );
  }

  refreshToken(): Observable<TokenResponse> {
    return this._httpClient
      .post<TokenResponse>(`${environment.apiBaseUrl}/refresh-token`, {}, { withCredentials: true })
      .pipe(
        tap((response) => {
          this.setSession(response.accessToken);
        }),
        catchError((err) => {
          this.logoutLocal();
          return of();
        }),
      );
  }

  logout() {
    this._httpClient
      .post(`${environment.apiBaseUrl}/logout`, {}, { withCredentials: true })
      .subscribe({
        complete: () => this.logoutLocal(),
        error: () => this.logoutLocal(),
      });
  }

  private logoutLocal() {
    this._accessToken.set(null);
    this.loggedUser.set(null);
  }

  private setSession(token: string) {
    this._accessToken.set(token);
    this.setLoggedInUser(token);
  }

  decodeToken(token: string): TokenPayload {
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload));
      return decoded;
    } catch (e) {
      console.error('Erro ao decodificar token', e);
      throw e;
    }
  }

  isAuthenticated(): boolean {
    const token = this._accessToken();
    if (!token) return false;

    try {
      const payload = this.decodeToken(token);
      return payload.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }

  private setLoggedInUser(token: string) {
    const payload = this.decodeToken(token);
    this.loggedUser.set({
      id: payload.nameid,
      username: payload.unique_name,
      role: payload.role,
    });
  }
}
