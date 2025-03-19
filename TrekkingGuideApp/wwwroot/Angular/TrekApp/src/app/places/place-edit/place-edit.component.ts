import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EditorComponent } from '@tinymce/tinymce-angular';
import { Place, PlacesService } from '../../service/places.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-place-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, EditorComponent],
  templateUrl: './place-edit.component.html',
  styleUrl: './place-edit.component.css'
})
export class PlaceEditComponent implements OnInit {
  init: EditorComponent['init'] = {
    plugins: 'lists link image table code help wordcount',
  };
  
  place!: Place;
  selectedFile: File | null = null;

  constructor(
    public route: ActivatedRoute,
    public router: Router,
    public placeService: PlacesService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.placeService.getPlace(id).subscribe({
      next: (p) => {
        this.place = p;
      },
      error: (err) => console.error(err)
    });
  }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    }
  }

  updatePlace(): void {
    const formData = new FormData();
    formData.append('Id', this.place.id.toString());
    formData.append('Title', this.place.title);
    formData.append('Description', this.place.description);
    if (this.selectedFile) {
      formData.append('Photo', this.selectedFile);
    }

    this.placeService.updatePlace(this.place.id, formData).subscribe({
      next: (updated) => {
        alert('Place updated!');
        this.router.navigate(['/places']);
      },
      error: (err) => {
        alert('Error updating place');
        console.error(err);
      }
    });
  }
}
