import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../environmets/environment';
import { TokenPayload } from '../models/token-payload';
import { TokenResponse } from '../models/token-response';
import { User } from '../models/user.model';

const TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loggedUser = signal<User | null>(null);
  private readonly _httpClient = inject(HttpClient);

  constructor() {
    this.restoreSession();
  }

  login(username: string, password: string): Observable<TokenResponse> {
    return this._httpClient
      .post<TokenResponse>(`${environment.apiBaseUrl}/login`, {
        username,
        password,
      })
      .pipe(
        tap((response) => {
          localStorage.setItem(TOKEN_KEY, response.accessToken);
          this.setLoggedInUser(response.accessToken);
        }),
      );
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    this.loggedUser.set(null);
  }

  decodeToken(token: string): TokenPayload {
    const payload = token.split('.')[1];
    const decoded = JSON.parse(atob(payload));
    return decoded;
  }
  private getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private restoreSession(): void {
    const token = this.getToken();
    if (token && this.isAuthenticated()) {
      this.setLoggedInUser(token);
    } else {
      this.logout();
    }
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    const payload = this.decodeToken(token);
    return payload.exp * 1000 > Date.now();
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
