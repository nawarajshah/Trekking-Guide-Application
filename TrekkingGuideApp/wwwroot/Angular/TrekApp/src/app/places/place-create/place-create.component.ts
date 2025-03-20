import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EditorComponent } from '@tinymce/tinymce-angular'
import { PlacesService } from '../../service/places.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-place-create',
  standalone: true,
  imports: [CommonModule, FormsModule, EditorComponent],
  templateUrl: './place-create.component.html',
  styleUrl: './place-create.component.css'
})
export class PlaceCreateComponent {
  init: EditorComponent['init'] = {
    plugins: 'lists link image table code help wordcount',
    toolbar: 'undo redo | styleselect | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | code'
  };

  title = '';
  description = '';
  selectedFile: File | null = null;
  photoFile?: File;
  photoPreview?: string;

  constructor(private placeService: PlacesService, private router: Router) {}

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    }
  }

  createPlace() {
    const formData = new FormData();
    formData.append('Title', this.title);
    formData.append('Description', this.description);
    if (this.selectedFile) {
      formData.append('Photo', this.selectedFile);
    }

    this.placeService.createPlace(formData).subscribe({
      next: (createdPlace) => {
        // alert('Place created successfully!');
        this.router.navigate(['/places']);
      },
      error: (err) => {
        alert('Error creating place');
        console.error(err);
      }
    });
  }
}
