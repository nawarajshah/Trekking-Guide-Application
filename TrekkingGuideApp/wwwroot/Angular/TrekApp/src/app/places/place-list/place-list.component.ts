import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Place, PlacesService } from '../../service/places.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-place-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './place-list.component.html',
  styleUrl: './place-list.component.css'
})
export class PlaceListComponent implements OnInit {
  places: Place[] = [];

  constructor(public placeService: PlacesService, public router: Router) {}

  ngOnInit(): void {
    this.placeService.getPlaces().subscribe(data => {
      this.places = data;
    });
  }
  
  deletePlace(id: number) {
    if (!confirm('Are you sure you want to delete this place?')) return;
    this.placeService.deletePlace(id).subscribe(() => {
      // refresh list
      this.places = this.places.filter(p => p.id !== id);
    });
  }

  getPhotoUrl(place: Place) {
    if (!place.photoPath) return '';
    return this.placeService.buildImageUrl(place.photoPath);
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
