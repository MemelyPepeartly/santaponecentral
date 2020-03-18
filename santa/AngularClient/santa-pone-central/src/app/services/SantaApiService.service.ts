import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { Client, ClientResponse } from '../../classes/Client';

const endpoint = 'https://dev-santaponecentral-api.azurewebsites.net/api/';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class SantaApiGetService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  getAllClients(): Observable<any> {
    return this.http.get(endpoint + 'Client').pipe(
      map(this.extractData));
  }
  getClient(id): Observable<any> {
    return this.http.get(endpoint + 'Client/' + id).pipe(
      map(this.extractData));
  }
  getAllEvents(): Observable<any> {
    return this.http.get(endpoint + 'Event').pipe(
      map(this.extractData));
  }
  getEvent(id): Observable<any> {
    return this.http.get(endpoint + 'Event/' + id).pipe(
      map(this.extractData));
  }
  getAllStatuses(): Observable<any> {
    return this.http.get(endpoint + 'Status').pipe(
      map(this.extractData));
  }
  getStatus(id): Observable<any> {
    return this.http.get(endpoint + 'Status/' + id).pipe(
      map(this.extractData));
  }
}
@Injectable({
  providedIn: 'root'
})
export class SantaApiPostService {

  constructor(private http: HttpClient) { }
  postClient(client: ClientResponse): Observable<any> {
    return this.http.post(endpoint + 'Client', client);
  }
}
