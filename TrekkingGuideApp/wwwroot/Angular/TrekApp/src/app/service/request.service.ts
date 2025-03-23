import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface Request {
    id: number;
    userId: string;
    itineraryId: number;
    status: string;
    createdDate: string;
    itinerary: {
        id: number;
        palceId: number;
        guideId: string;
        cost: number;
        duration: string;
        description: string;
        createdDate: string;
        place: {
            id: number;
            title: string;
            description: string;
            photoPath?: string;
        }
    };
}

@Injectable({
    providedIn: 'root'
})
export class RequestService {
    private baseUrl = '/api/requestapi';

    constructor(private http: HttpClient) {}

    createRequest(itineraryId: number): Observable<any> {
        return this.http.post<any>(`${this.baseUrl}?itineraryId=${itineraryId}`, {});
    }

    getMyRequests(): Observable<Request[]> {
        return this.http.get<Request[]>(`${this.baseUrl}/myrequests`);
    }

    getGuideRequests(): Observable<Request[]> {
        return this.http.get<Request[]>(`${this.baseUrl}/guide`);
    }

    acceptRequest(requestId: number): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/accept?requestId=${requestId}`, {});
    }

    rejectRequest(requestId: number): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/reject?requestId=${requestId}`, {});
    }
}