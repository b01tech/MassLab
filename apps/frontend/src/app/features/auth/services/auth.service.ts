import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../../../environmets/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loggedUser = signal<User | null>(null);
  private readonly _httpClient = inject(HttpClient);

  login(username: string, password: string): Observable<User> {
    return this._httpClient.post<User>(`${environment.apiBaseUrl}/login`, {
      username,
      password,
    });
  }

  logout() {
    this.loggedUser.set(null);
  }
}
