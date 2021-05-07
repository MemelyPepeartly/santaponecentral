import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BaseClient, Client, HQClient, InfoContainer, StrippedClient } from 'src/classes/client';
import { Status } from 'src/classes/status';
import { ClientRelationshipsRequest, ClientSignupRequest } from 'src/classes/responseTypes';
import { ClientRequest } from 'node:http';

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

  /* GET */
  getAllTruncatedClients(): Observable<Array<StrippedClient>> 
  {
    return this.http.get<Array<StrippedClient>>(endpoint + 'Client/Truncated')
  }
  getTruncatedClientByID(id: string): Observable<StrippedClient> 
  {
    return this.http.get<StrippedClient>(endpoint + 'Client/Truncated/' + id)
  }
  getAllHQClients(): Observable<Array<HQClient>> 
  {
    return this.http.get<Array<HQClient>>(endpoint + 'Client/HQClient')
  }
  getHQClientByID(id: string): Observable<HQClient> 
  {
    return this.http.get<HQClient>(endpoint + 'Client/HQClient/' + id)
  }
  getInfoContainerByClientID(id: string): Observable<InfoContainer> 
  {
    return this.http.get<InfoContainer>(endpoint + 'Client/' + id + "/InfoContainer")
  }
  getClientByClientID(id: string): Observable<Client> 
  {
    return this.http.get<Client>(endpoint + 'Client/' + id)
  }
  getClientByEmail(email: string): Observable<Client>  
  {
    return this.http.get<Client>(endpoint + 'Client/Email/' + email)
  }
  getBasicClientByClientID(id: string): Observable<BaseClient> 
  {
    return this.http.get<BaseClient>(endpoint + 'Client/Basic/' + id)
  }
  getBasicClientByEmail(email: string): Observable<BaseClient>
  {
    return this.http.get<BaseClient>(endpoint + 'Client/Basic/Email/' + email)
  }
  getAllowedAssignmentsByID(clientID: string, eventTypeID: string) : Observable<any> 
  {
    return this.http.get(endpoint + 'Client/' + clientID + '/AllowedAssignment/' + eventTypeID)
  }
  getAllStatuses(): Observable<Array<Status>> 
  {
    return this.http.get<Array<Status>>(endpoint + 'Status')
  }
  getStatus(id: string): Observable<Status> 
  {
    return this.http.get<Status>(endpoint + 'Status/' + id)
  }
  getClientStatusByEmail(email: string): Observable<Status> 
  {
    return this.http.get<Status>(endpoint + 'Status/Check/' + email)
  }
  /* POST */
  postClient(client: ClientRequest): Observable<Client> 
  {
    return this.http.post<Client>(endpoint + 'Client', client)
  }
  postClientRecipients(id: string, relationships: ClientRelationshipsRequest): Observable<any> 
  {
    return this.http.post<any>(endpoint + 'Client/' + id + '/Recipients', relationships)
  }
  postClientSignup(signup: ClientSignupRequest): Observable<any> {
    return this.http.post<any>(endpoint + 'Client/Signup', signup)
  }
  /* PUT */

  /* DELETE */
}
