import { Component } from '@angular/core';
import { PlaceListComponent } from "./place-list/place-list.component";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-places',
  imports: [PlaceListComponent, RouterModule],
  templateUrl: './places.component.html',
  styleUrl: './places.component.css'
})
export class PlacesComponent {

}
