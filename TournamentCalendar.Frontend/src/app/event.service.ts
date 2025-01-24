import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { EventInput } from "@fullcalendar/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class EventService{
    private apiUrl='http://localhost:5247/api/Event';
    constructor(private http: HttpClient) {}

    getEvents(): Observable<EventInput[]>{
        return this.http.get<EventInput[]>(`${this.apiUrl}/get-all`);
    }

    createEvent(newEvent: EventInput): Observable<EventInput> {
        return this.http.post<EventInput>(`${this.apiUrl}/add`, newEvent);
    }

    deleteEvent(id: string): Observable<EventInput> {
    return this.http.delete<EventInput>(`${this.apiUrl}/remove/${id}`);
    }
}