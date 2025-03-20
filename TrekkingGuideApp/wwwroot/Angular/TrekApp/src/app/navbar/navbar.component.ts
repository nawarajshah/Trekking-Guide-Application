import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProfileService } from '../service/profile.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  
  roles: string[] = [];

  constructor(private profileService: ProfileService, private http: HttpClient) {}

  ngOnInit(): void {
    // fetch the current user's roles from the api
    this.http.get<string[]>('/api/profileapi/roles').subscribe({
      next: (data) => this.roles = data,
      error: (err) => console.error('Error fetching roles', err)
    });
    // this.profileService.getUserRoles().subscribe({
    //   next: (r: string[]) => {
    //     this.roles = r;
    //   },
    //   error: (err) => {
    //     console.log('Error fetching roles', err);
    //     this.roles = []; // fallback to empty array
    //   }
    // });
  }

  hasAdminAccess(): boolean {
    // only allow access to places and user management if the user is admin or superadmin
    return this.roles.some(r => r === 'Admin' || r === 'SuperAdmin');
  }

  logout() {
    this.http.post('/api/account/logout', {}).subscribe({
      next: () => {
        window.location.href = '/Account/Login';
      },
      error: (err) => console.error('Logout error', err)
    });
  }
}
