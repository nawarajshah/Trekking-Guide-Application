import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EditUserRolesDto, UserManagementService, UserRolesDto } from '../service/user-management.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-user-management',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  userList: UserRolesDto[] = [];
  userForm: EditUserRolesDto = {
    userId: '',
    email: '',
    selectedRole: '',
    availableRoles: []
  };
  // Flags for enabling/disabling buttons
  isFormDisabled = true;

  constructor(
    private userService: UserManagementService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadUserList();
  }

  loadUserList() {
    this.userService.getManageableUsers().subscribe((data) => {
      this.userList = data;
    });
  }

  selectUser(userId: string) {
    this.userService.getUserForm(userId).subscribe((form) => {
      this.userForm = form;
      // If userId is empty or user not found, disable form
      this.isFormDisabled = !form.userId;
    });
  }

  updateUser() {
    if (!this.userForm.userId) return;
    this.userService.editUserRole(this.userForm).subscribe((updatedList) => {
      this.userList = updatedList;
      this.toastr.success('Role updated to ' + this.userForm.selectedRole);
      // clear the form
      this.clearForm();
    });
  }

  deleteUser() {
    if (!this.userForm.userId) return;
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.userService.deleteUser(this.userForm.userId).subscribe((updatedList) => {
      this.userList = updatedList;
      this.clearForm();
      this.toastr.success(this.userForm.email + ' has been deleted.');
    });
  }

  clearForm() {
    this.userForm = {
      userId: '',
      email: '',
      selectedRole: '',
      availableRoles: []
    };
    this.isFormDisabled = true;
  }
}
