import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';

const endpoint = environment.eventServiceEndpoint;

@Injectable({
  providedIn: 'root'
})
export class EventService 
{
  constructor(private http: HttpClient) { }

  getAllEvents(): Observable<any> 
  {
    return this.http.get(endpoint + 'Event')
  }
  getEvent(id: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Event/' + id)
  }
}
