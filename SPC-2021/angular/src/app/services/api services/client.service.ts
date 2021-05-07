import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StrippedClient } from 'src/classes/client';

const endpoint = environment.clientServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ClientService 
{
  constructor(private http: HttpClient) { }

  getAllTruncatedClients(): Observable<Array<StrippedClient>>
  {
    return this.http.get<Array<StrippedClient>>(endpoint + 'Client/Truncated')
  }
}
