import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Place {
  id: number;
  title: string;
  description: string;
  photoPath?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PlacesService {
  private baseUrl = '/api/placesapi';

  constructor(private http: HttpClient) { }

  getPlaces(): Observable<Place[]> {
    return this.http.get<Place[]>(this.baseUrl);
  }

  getPlace(id: number): Observable<Place> {
    return this.http.get<Place>(`${this.baseUrl}/${id}`);
  }

  // create with multipart
  createPlace(formData: FormData): Observable<Place> {
    return this.http.post<Place>(this.baseUrl, formData);
  }

  // update with multipart
  updatePlace(id: number, formData: FormData): Observable<Place> {
    return this.http.put<Place>(`${this.baseUrl}/${id}`, formData);
  }

  deletePlace(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  buildImageUrl(fileName: string): string {
    return `${this.baseUrl}/image/${fileName}`;
  }
}
