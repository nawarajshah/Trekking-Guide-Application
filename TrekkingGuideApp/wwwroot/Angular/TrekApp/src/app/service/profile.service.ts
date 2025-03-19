import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ManageProfileViewModel {
  fullName: string;
  bio?: string;
  phone?: string;
  address?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private baseUrl = 'http://localhost:5270/api/profileapi';

  constructor(private http: HttpClient) { }
  
  updateProfile(model: ManageProfileViewModel): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/update`, model);
  }

  getProfile(): Observable<ManageProfileViewModel> {
    return this.http.get<ManageProfileViewModel>(`${this.baseUrl}/profile`);
  }
  
  getUserRoles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/roles`);
  }
}
