import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Place, PlacesService } from '../../service/places.service';
import { ActivatedRoute } from '@angular/router';
import { Itinerary } from '../../service/itinerary.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-place-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './place-details.component.html',
  styleUrl: './place-details.component.css'
})
export class PlaceDetailsComponent implements OnInit {
  place?: Place;
  itineraries: Itinerary[] = [];
  showItineraries = false;
  userRoles: string[] = [];

  constructor(
    private route: ActivatedRoute,
    public placeService: PlacesService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    // get the placeId from the route (assuce route parameter "id")
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadPlace(id);
    // this.loadItineraries(id);
    this.loadUserRoles();
  }

  loadPlace(id: number): void {
    this.placeService.getPlace(id).subscribe({
      next: (p) => this.place = p,
      error: (err) => console.error('Error loading place', err)
    });
  }

  loadUserRoles(): void {
    // call an API endpoint that returns the current user's roles (JSON array)
    // this endpoint should read the session cookie and return roles like ["SuperAdmin"] or ["Guide"]
    this.http.get<string[]>('/api/profileapi/roles').subscribe({
      next: (roles) => (this.userRoles = roles),
      error: (err) => {
        console.error('Error fetching roles', err);
        this.userRoles = [];
      }
    });
  }

  getPhotoUrl() {
    if (!this.place || !this.place.photoPath) return '';
    return this.placeService.buildImageUrl(this.place.photoPath);
  }
}
