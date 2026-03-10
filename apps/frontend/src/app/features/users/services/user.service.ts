import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../../../environmets/environment';
import { Observable, tap } from 'rxjs';
import { PaginatedUserResponse } from '../models/user-response.model';
import { User } from '../../auth/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _http = inject(HttpClient);
  private _apiUrl = `${environment.apiBaseUrl}/users`;

  users = signal<User[]>([]);
  totalItems = signal<number>(0);

  getUsers(
    page: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
  ): Observable<PaginatedUserResponse> {
    let params = new HttpParams().set('page', page.toString()).set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this._http.get<PaginatedUserResponse>(this._apiUrl, { params }).pipe(
      tap((response) => {
        this.users.set(response.data);
        this.totalItems.set(response.pagination.totalItems);
      }),
    );
  }

  createUser(user: Partial<User>): Observable<User> {
    return this._http.post<User>(this._apiUrl, user);
  }

  updateUser(id: string, user: Partial<User>): Observable<User> {
    return this._http.put<User>(`${this._apiUrl}/${id}`, user);
  }

  activateUser(id: string): Observable<void> {
    return this._http.patch<void>(`${this._apiUrl}/${id}/activate`, {});
  }

  deactivateUser(id: string): Observable<void> {
    return this._http.patch<void>(`${this._apiUrl}/${id}/deactivate`, {});
  }

  changePassword(id: string, newPassword: string): Observable<void> {
    return this._http.patch<void>(`${this._apiUrl}/${id}/password`, {
      userId: id,
      newPassword,
    });
  }
}
