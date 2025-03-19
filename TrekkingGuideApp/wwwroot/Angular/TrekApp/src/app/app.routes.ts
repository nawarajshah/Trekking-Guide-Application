import { Routes } from '@angular/router';
import { PlaceListComponent } from './places/place-list/place-list.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { ManageProfileComponent } from './manage-profile/manage-profile.component';
import { PlacesComponent } from './places/places.component';
import { PlaceCreateComponent } from './places/place-create/place-create.component';
import { PlaceDetailsComponent } from './places/place-details/place-details.component';
import { PlaceEditComponent } from './places/place-edit/place-edit.component';

export const routes: Routes = [
    { path: '', component: PlaceListComponent, title: 'Home' },
    { path: 'user-management', component: UserManagementComponent, title: 'User Management' },
    { path: 'places', component: PlacesComponent, title: 'Places' },
    { path: 'places/create', component: PlaceCreateComponent, title: 'Create Place' },
    { path: 'places/:id/edit', component: PlaceEditComponent, title: 'Edit Place' },
    { path: 'places/:id/details', component: PlaceDetailsComponent, title: 'Place Details' },
    { path: 'manage-profile', component: ManageProfileComponent, title: 'Manage Profile' }
    // {path: 'test',component: TrekListingComponent,title: 'Home page', pathMatch: 'full'},
];
