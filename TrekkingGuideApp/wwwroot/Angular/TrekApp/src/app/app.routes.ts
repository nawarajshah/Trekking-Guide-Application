import { Routes } from '@angular/router';
import { PlaceListComponent } from './places/place-list/place-list.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { ManageProfileComponent } from './manage-profile/manage-profile.component';
import { PlacesComponent } from './places/places.component';
import { PlaceCreateComponent } from './places/place-create/place-create.component';
import { PlaceDetailsComponent } from './places/place-details/place-details.component';
import { PlaceEditComponent } from './places/place-edit/place-edit.component';
import { AuthGuard } from './auth.guard';

export const routes: Routes = [
    { 
        path: '', 
        component: PlaceListComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['User', 'Guide', 'Admin', 'SuperAdmin'] }, 
        title: 'Home' 
    },
    { 
        path: 'user-management', 
        component: UserManagementComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Admin', 'SuperAdmin'] }, 
        title: 'User Management' 
    },
    { 
        path: 'places', 
        component: PlacesComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Admin', 'SuperAdmin'] }, 
        title: 'Places' 
    },
    { 
        path: 'places/create', 
        component: PlaceCreateComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Admin', 'SuperAdmin'] }, 
        title: 'Create Place' 
    },
    { 
        path: 'places/:id/edit', 
        component: PlaceEditComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Admin', 'SuperAdmin'] }, 
        title: 'Edit Place' 
    },
    { 
        path: 'places/:id/details', 
        component: PlaceDetailsComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['User', 'Guide', 'Admin', 'SuperAdmin'] }, 
        title: 'Place Details' 
    },
    { 
        path: 'manage-profile', 
        component: ManageProfileComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['User', 'Guide', 'Admin', 'SuperAdmin'] }, 
        title: 'Manage Profile' 
    }
    // {path: 'test',component: TrekListingComponent,title: 'Home page', pathMatch: 'full'},
];
