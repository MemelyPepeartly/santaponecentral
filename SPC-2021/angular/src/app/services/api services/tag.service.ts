import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Client } from 'src/classes/client';
import { AddClientTagRelationshipsRequest, AddOrEditTagRequest, DeleteClientTagRelationshipRequest } from 'src/classes/request-types';

const endpoint = environment.clientServiceEndpoint;

@Injectable({
  providedIn: 'root'
})
export class TagService 
{
  constructor(private http: HttpClient) { }

  /* GET */
  getAllTags(): Observable<any> 
  {
    return this.http.get(endpoint + "Tag");
  }
  getTag(id): Observable<any> 
  {
    return this.http.get(endpoint + "Tag/" + id);
  }

  /* POST */
  postTag(tagResponse: AddOrEditTagRequest): Observable<any> 
  {
    return this.http.post(endpoint + 'Tag', tagResponse);
  }
  postTagsToClient(clientID: string, clientTagRelationships: AddClientTagRelationshipsRequest): Observable<Client> 
  {
    return this.http.post<Client>(endpoint + 'Client/'+ clientID + "/Tags", clientTagRelationships)
  }

  /* PUT */

  putTagName(id: string, updatedTag: AddOrEditTagRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Tag/' + id, updatedTag);
  }

  /* DELETE */
  deleteTag(id: string): Observable<any> 
  {
    return this.http.delete(endpoint + 'Tag/' + id);
  }
  deleteTagFromClient(clientTagRelationship: DeleteClientTagRelationshipRequest): Observable<Client> 
  {
    return this.http.delete<Client>(endpoint + 'Client/' + clientTagRelationship.clientID + '/Tag?tagID=' + clientTagRelationship.tagID);
  }
}
