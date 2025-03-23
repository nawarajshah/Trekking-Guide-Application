import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule, NgbCollapseModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  isCollapsed = true;
  
  roles: string[] = [];

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    // fetch the current user's roles from the api
    this.http.get<string[]>('/api/profileapi/roles').subscribe({
      next: (data) => this.roles = data,
      error: (err) => console.error('Error fetching roles', err)
    });
  }

  hasAdminAccess(): boolean {
    // only allow access to places and user management if the user is admin or superadmin
    return this.roles.some(r => r === 'Admin' || r === 'SuperAdmin');
  }

  hasGuideAccess(): boolean {
    // only allow access to places and user management if the user is Guide
    return this.roles.some(r => r === 'Guide');
  }

  hasUserAccess(): boolean {
    // only allow access to places and user management if the user is User
    return this.roles.some(r => r === 'User');
  }

  logout() {
    this.http.post('/api/account/logout', {}).subscribe({
      next: () => {
        window.location.href = '/Account/Login';
      },
      error: (err) => {
        this.toastr.error('Error during logout', 'Error');
        console.error('Logout error: ', err);
      }
    });
  }
}
