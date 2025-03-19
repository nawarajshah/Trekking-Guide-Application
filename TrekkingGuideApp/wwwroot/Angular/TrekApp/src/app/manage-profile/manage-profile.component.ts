import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ManageProfileViewModel, ProfileService } from '../service/profile.service';
import { Observable } from 'rxjs';

@Component({
  standalone: true,
  selector: 'app-manage-profile',
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './manage-profile.component.html',
  styleUrl: './manage-profile.component.css'
})
export class ManageProfileComponent implements OnInit {
  model: ManageProfileViewModel = {
    fullName: '',
    bio: '',
    phone: '',
    address: ''
  };

  // This array will hold the actual roles of the current user fetched from the API
  roles: string[] = [];
  successMessage: string = '';
  errorMessage: string = '';

  constructor(private profileService: ProfileService) {}

  ngOnInit(): void {
    // fetch the current user's roles from the api
    this.profileService.getProfile().subscribe({
      next: (profile) => {
        this.model = profile;
      },
      error: (err) => {
        console.error('Error fetching profile', err);
      }
    });

    // Fetch the roles (using your existing method)
    this.profileService.getUserRoles().subscribe({
      next: (r: string[]) => {
        this.roles = r;
      },
      error: (err) => {
        console.error('Error fetching roles', err);
        this.roles = [];
      }
    });
  }

  updateProfile(): void {
    this.profileService.updateProfile(this.model).subscribe({
      next: (response) => {
        this.successMessage = response.message;
        this.errorMessage = '';
      },
      error: (error) => {
        this.errorMessage = 'Error updating profile';
        this.successMessage = '';
      }
    });
  }
}
