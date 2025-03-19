import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface UserRolesDto {
  userId: string;
  email: string;
  roles: string[];
}

export interface EditUserRolesDto {
  userId: string;
  email: string;
  selectedRole: string;
  availableRoles: string[];
}

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {
  private baseUrl = '/api/usermanagementapi';
  // adjust the port/URL to match your ASP.NET app

  constructor(private http: HttpClient) { }

  getManageableUsers(): Observable<UserRolesDto[]> {
    return this.http.get<UserRolesDto[]>(`${this.baseUrl}/users`);
  }

  getUserForm(userId: string): Observable<EditUserRolesDto> {
    return this.http.get<EditUserRolesDto>(`${this.baseUrl}/userform?userId=${userId}`);
  }

  editUserRole(model: EditUserRolesDto): Observable<UserRolesDto[]> {
    return this.http.post<UserRolesDto[]>(`${this.baseUrl}/edit`, model);
  }

  deleteUser(userId: string): Observable<UserRolesDto[]> {
    return this.http.delete<UserRolesDto[]>(`${this.baseUrl}/delete/${userId}`);
  }
}
