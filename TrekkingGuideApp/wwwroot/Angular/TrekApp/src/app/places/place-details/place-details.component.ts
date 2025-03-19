import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Place, PlacesService } from '../../service/places.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-place-details',
  imports: [CommonModule],
  templateUrl: './place-details.component.html',
  styleUrl: './place-details.component.css'
})
export class PlaceDetailsComponent {
  placeId!: number;
  place?: Place;

  constructor(
    private route: ActivatedRoute,
    private placeService: PlacesService
  ) {}

  ngOnInit(): void {
    this.placeId = +this.route.snapshot.paramMap.get('id')!;
    this.loadPlace(this.placeId);
  }

  loadPlace(id: number) {
    this.placeService.getPlace(id).subscribe({
      next: (p) => this.place = p,
      error: (err) => console.error(err)
    });
  }

  getPhotoUrl() {
    if (!this.place || !this.place.photoPath) return '';
    return this.placeService.buildImageUrl(this.place.photoPath);
  }
}
