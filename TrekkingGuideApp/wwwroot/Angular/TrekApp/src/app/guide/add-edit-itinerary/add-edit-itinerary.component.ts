import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Itinerary, ItineraryService, ItineraryViewModel } from '../../service/itinerary.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-edit-itinerary',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-edit-itinerary.component.html',
  styleUrl: './add-edit-itinerary.component.css'
})
export class AddEditItineraryComponent implements OnInit {
  model: ItineraryViewModel = {
    placeId: 0,
    placeTitle: '',
    cost: 0,
    duration: '',
    description: ''
  };
  isEditMode = false;
  itineraryId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private itineraryService: ItineraryService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const placeId = +params['placeId'];
      const placeTitle = params['placeTitle'];
      const itinId = params['itineraryId'] ? +params['itineraryId'] : null;

      if (!placeId) {
        // redirect to place list if placeId is not provided
        this.router.navigate(['/places']);
        return;
      }
      
      this.model.placeId = placeId;
      this.model.placeTitle = placeTitle;
      if (itinId) {
        this.isEditMode = true;
        this.itineraryId = itinId;
        // load existing itinery
        this.itineraryService.getItineraryByPlace(placeId).subscribe({
          next: (data: Itinerary) => {
            if (data) {
              this.model.cost = data.cost;
              this.model.duration = data.duration;
              this.model.description = data.description;
            }
          },
          error: (err) => console.error(err)
        });
      }
    });
  }

  saveItinerary(isEditMode: boolean) {
    if (isEditMode) {
      if (this.isEditMode && this.itineraryId) {
        this.itineraryService.updateItinerary(this.itineraryId, this.model).subscribe({
          next: () => {
            this.toastr.success('Itinerary updated successfully!');
            this.router.navigate(['/places']);
          },
          error: (err) => {
            this.toastr.error('Itinerary updation fail!');
            console.error('Error on updating itinerary: ' + err);
          }
        });
      }
    } else {
      this.itineraryService.createItinerary(this.model).subscribe({
        next: () => {
          this.toastr.success('Itinerary created successfully!');
          this.router.navigate(['/places']);
        },
        error: (err) => {
          this.toastr.error('Itinerary creation fail!');
          console.error(err);
        }
      });
    }
  }
}
