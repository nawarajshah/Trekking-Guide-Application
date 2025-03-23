import { Component } from '@angular/core';
import { PlaceListComponent } from "../../places/place-list/place-list.component";

@Component({
  selector: 'app-itinerary',
  imports: [PlaceListComponent],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.css'
})
export class ItineraryComponent {

}
