import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Place, PlacesService } from '../../service/places.service';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { ItineraryService } from '../../service/itinerary.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-place-list',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLink],
  templateUrl: './place-list.component.html',
  styleUrl: './place-list.component.css'
})
export class PlaceListComponent implements OnInit {
  places: Place[] = [];
  selectedPlaceForGuide: number | null = null;

  // Directory mapping placeId to itinerary id (if exists)
  itineraryMapping: { [placeId: number]: number } = {};

  constructor(
    public placeService: PlacesService, 
    public router: Router,
    public itineraryService: ItineraryService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadPlaces();
    this.loadMyItineraries();
  }

  loadPlaces() {
    this.placeService.getPlaces().subscribe(data => {
      this.places = data;
    });
  }

  loadMyItineraries() {
    this.itineraryService.getMyItineraries().subscribe(data => {
      data.forEach(itin => {
        this.itineraryMapping[itin.placeId] = itin.id;
      });
    });
  }

  geToItinerary(place: Place) {
    // check if itinerary exists for the place
    const itineraryId = this.itineraryMapping[place.id];
    if (itineraryId) {
      // navigate to itinerary page for editing, passing both placeid and itineraryid
      this.router.navigate(['/guide/addedditinerary'], { queryParams: { placeId: place.id, itineraryId: itineraryId, placeTitle: place.title } });
    } else {
      // navigate to itinerary page for creation, with placeId required
      this.router.navigate(['/guide/addedditinerary'], { queryParams: { placeId: place.id, placeTitle: place.title } });
    }
  }
  
  deletePlace(id: number) {
    if (!confirm('Are you sure you want to delete this place?')) return;
    this.placeService.deletePlace(id).subscribe(() => {
      this.toastr.success('Place deleted');
      // refresh list
      this.places = this.places.filter(p => p.id !== id);
    });
  }

  toggleGuideDropdown(placeId: number) {
    if (this.selectedPlaceForGuide == placeId) {
      this.selectedPlaceForGuide = null;
    } else {
      this.selectedPlaceForGuide = placeId;
    }
  }

  truncateHTML(text: string): string {
    let charlimit = 160;
    if(!text || text.length <= charlimit )
      return text;

    let without_html = text.replace(/<(?:.|\n)*?>/gm, '');
    let shortened = without_html.substring(0, charlimit) + "...";
    return shortened;
  }
}
