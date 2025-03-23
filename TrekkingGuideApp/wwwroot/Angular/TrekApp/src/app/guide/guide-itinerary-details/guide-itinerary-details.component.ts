import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Itinerary, ItineraryService } from '../../service/itinerary.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PlacesService } from '../../service/places.service';
import { RequestService } from '../../service/request.service';

@Component({
  selector: 'app-guide-itinerary-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './guide-itinerary-details.component.html',
  styleUrl: './guide-itinerary-details.component.css'
})
export class GuideItineraryDetailsComponent implements OnInit {
  itinerary?: Itinerary;

  constructor(
    private route: ActivatedRoute,
    private itineraryService: ItineraryService,
    private toastr: ToastrService,
    public placeService: PlacesService,
    private router: Router,
    private requestService: RequestService
  ) {}

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;
    if (!id) {
      this.router.navigate(['/guide-itineraries']);
      return;
    }
    // fetch the itinerary by ID
    this.itineraryService.getItineraryById(id).subscribe({
      next: (data) => {
        this.itinerary = data;
      },
      error: (err) => {
        this.toastr.error('Error fetching itinerary details');
        console.error('Itinerary fetch error: ' + err);
      }
    })
  }

  requestItinerary() {
    if (!this.itinerary) return;
    this.requestService.createRequest(this.itinerary.id).subscribe({
      next: (res) => {
        this.toastr.success(res.message);
      },
      error: (err) => {
        this.toastr.error('Error sending request');
        console.error('Error sending request' + err);
      }
    })
  }

}
