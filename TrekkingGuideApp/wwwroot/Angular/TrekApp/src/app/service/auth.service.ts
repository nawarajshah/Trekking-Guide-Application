import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private rolesUrl = '/api/profileapi/roles';

  constructor(private http: HttpClient) {}

  // calls the backed to fetch current user's roles (e.g., ["SupperAdmin"] or ["User"])
  getUserRoles(): Observable<string[]> {
    return this.http.get<string[]>(this.rolesUrl).pipe(
      catchError(err => {
        console.error("Error fetching roles", err);
        return of([]); // return empty if error
      })
    );
  }
}
