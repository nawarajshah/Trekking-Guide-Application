import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Itinerary, ItineraryService } from '../../service/itinerary.service';
import { ToastrService } from 'ngx-toastr';
import { PlacesService } from '../../service/places.service';

@Component({
  selector: 'app-guide-itinerary-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './guide-itinerary-list.component.html',
  styleUrl: './guide-itinerary-list.component.css'
})
export class GuideItineraryListComponent implements OnInit {
  itineraries: Itinerary[] = [];
  searchQuery: string = '';

  constructor(
    private itineraryService: ItineraryService,
    private toastr: ToastrService,
    public placeService: PlacesService
  ) {}

  ngOnInit(): void {
      this.loadItineraries();
  }

  loadItineraries(): void {
    // load all itineraries via search with empty query
    this.itineraryService.getAllItineraries().subscribe({
      next: (data) => {
        this.itineraries = data;
      },
      error: (err) => {
        this.toastr.error('Error loading itineraries');
        console.error('Load Error: ' + err);
      }
    });
  }

  searchItineraries(): void {
    this.itineraryService.searchItineraries(this.searchQuery).subscribe({
      next: (data) => {
        this.itineraries = data;
      },
      error: (err) => {
        this.toastr.error('Error during search');
        console.error("Search Error: ", err);
      }
    })
  }
}
