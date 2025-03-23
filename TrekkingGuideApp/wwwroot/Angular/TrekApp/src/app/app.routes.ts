import { Routes } from '@angular/router';
import { PlaceListComponent } from './places/place-list/place-list.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { ManageProfileComponent } from './manage-profile/manage-profile.component';
import { PlacesComponent } from './places/places.component';
import { PlaceCreateComponent } from './places/place-create/place-create.component';
import { PlaceDetailsComponent } from './places/place-details/place-details.component';
import { PlaceEditComponent } from './places/place-edit/place-edit.component';
import { AuthGuard } from './auth.guard';
import { ItineraryComponent } from './guide/itinerary/itinerary.component';
import { AddEditItineraryComponent } from './guide/add-edit-itinerary/add-edit-itinerary.component';
import { GuideItineraryListComponent } from './guide/guide-itinerary-list/guide-itinerary-list.component';
import { GuideItineraryDetailsComponent } from './guide/guide-itinerary-details/guide-itinerary-details.component';
import { MyRequestsComponent } from './guide/my-requests/my-requests.component';
import { GuideRequestsComponent } from './guide/guide-requests/guide-requests.component';

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
        path: 'places/edit/:id', 
        component: PlaceEditComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['Admin', 'SuperAdmin'] }, 
        title: 'Edit Place' 
    },
    { 
        path: 'places/details/:id', 
        component: PlaceDetailsComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['User', 'Guide', 'Admin', 'SuperAdmin'] }, 
        title: 'Place Details' 
    },
    {
        path: 'guide/itinerary',
        component: ItineraryComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Guide'] },
        title: 'Itinerary Page'
    },
    {
        path: 'guide/addedditinerary',
        component: AddEditItineraryComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Guide'] },
        title: 'Add Edit Itinerary'
    },
    {
        path: 'guide/guide-itineraries',
        component: GuideItineraryListComponent,
        canActivate: [AuthGuard],
        data: { roles: ['User'] },
        title: 'Chose Guide'
    },
    {
        path: 'guide/guide-itinerary-details/:id',
        component: GuideItineraryDetailsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['User'] },
        title: 'Itinerary Details'
    },
    {
        path: 'my-requests',
        component: MyRequestsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['User'] },
        title: 'My Requests'
    },
    {
        path: 'guide-requests',
        component: GuideRequestsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Guide'] },
        title: 'Guide Requests'
    },
    { 
        path: 'manage-profile', 
        component: ManageProfileComponent, 
        canActivate: [AuthGuard], 
        data: { roles: ['User', 'Guide', 'Admin', 'SuperAdmin'] }, 
        title: 'Manage Profile' 
    }
];
