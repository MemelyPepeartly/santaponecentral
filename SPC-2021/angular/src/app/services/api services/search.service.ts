import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SearchQueryModelRequest } from 'src/classes/request-types';

const endpoint = environment.searchServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class SearchService 
{
  constructor(private http: HttpClient) { }

  searchClients(body: SearchQueryModelRequest): Observable<any> {
    return this.http.post(endpoint + 'Catalogue/SearchClients', body);
  }
}