import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface Itinerary {
    id: number;
    placeId: number;
    guideId: string;
    cost: number;
    duration: string;
    description: string;
    createdDate: string;
    place: {
        id: number;
        title: string;
        description: string;
        photoPath: string;
    };
}

export interface ItineraryViewModel {
    placeId: number;
    placeTitle: string;
    cost: number;
    duration: string;
    description: string;
}

@Injectable({
    providedIn: 'root'
})
export class ItineraryService {
    private baseUrl = '/api/itinerariesapi';

    constructor(private http: HttpClient) {}

    getItineraryByPlace(placeId: number): Observable<Itinerary> {
        return this.http.get<Itinerary>(`${this.baseUrl}/byplace?placeId=${placeId}`);
    }

    getItineraryById(itineraryId: number): Observable<Itinerary> {
        return this.http.get<Itinerary>(`${this.baseUrl}/${itineraryId}`);
    }

    getMyItineraries(): Observable<Itinerary[]> {
        return this.http.get<Itinerary[]>(`${this.baseUrl}/myitineraries`);
    }

    getAllItineraries(): Observable<Itinerary[]> {
        return this.http.get<Itinerary[]>(`${this.baseUrl}/all`);
    }

    createItinerary(model: ItineraryViewModel): Observable<Itinerary> {
        return this.http.post<Itinerary>(`${this.baseUrl}`, model);
    }

    updateItinerary(id: number, model: ItineraryViewModel): Observable<Itinerary> {
        return this.http.put<Itinerary>(`${this.baseUrl}/${id}`, model);
    }

    searchItineraries(title: string): Observable<Itinerary[]> {
        return this.http.get<Itinerary[]>(`${this.baseUrl}/search?title=${title}`);
    }

    requestItinerary(itineraryId: number): Observable<any> {
        return this.http.post<any>(`${this.baseUrl}/request?itineraryId=${itineraryId}`, {});
    }
}