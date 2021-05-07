import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BaseClient, Client, HQClient, InfoContainer, StrippedClient } from 'src/classes/client';
import { Status } from 'src/classes/status';
import { ClientRelationshipsRequest, ClientSignupRequest, DeleteClientSenderRecipientRelationshipRequest, DeleteClientTagRelationshipRequest, EditClientAddressRequest, EditClientEmailRequest, EditClientIsAdminRequest, EditClientNameRequest, EditClientNicknameRequest, EditClientStatusRequest, EditProfileAssignmentStatusRequest } from 'src/classes/request-types';
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
  putClientAddress(id: string, updatedClient: EditClientAddressRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Address', updatedClient)
  }
  putClientEmail(id: string, updatedClient: EditClientEmailRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Email', updatedClient)
  }
  putClientNickname(id: string, updatedClient: EditClientNicknameRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Nickname', updatedClient)
  }
  putClientName(id: string, updatedClient: EditClientNameRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Name', updatedClient)
  }
  putClientIsAdmin(id: string, updatedClient: EditClientIsAdminRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Admin', updatedClient)
  }
  putAssignmentStatus(clientID: string, assignmentXrefID: string, response: EditProfileAssignmentStatusRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + clientID + '/Relationship/' + assignmentXrefID + "/AssignmentStatus", response)
  }
  putClientStatus(id: string, updatedClient: EditClientStatusRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Client/' + id + '/Status', updatedClient)
  }

  /* DELETE */
  deleteClient(id: string): Observable<any> 
  {
    return this.http.delete(endpoint + 'Client/' + id);
  }
  deleteClientRecipient(id: string, relationship: DeleteClientSenderRecipientRelationshipRequest): Observable<Client>
  {
    return this.http.delete<Client>(endpoint + 'Client/' + id + '/Recipient?assignmentClientID=' + relationship.clientID+'&eventID=' + relationship.clientEventTypeID);
  }
  deleteTagFromClient(clientTagRelationship: DeleteClientTagRelationshipRequest): Observable<Client> 
  {
    return this.http.delete<Client>(endpoint + 'Client/' + clientTagRelationship.clientID + '/Tag?tagID=' + clientTagRelationship.tagID);
  }
}
