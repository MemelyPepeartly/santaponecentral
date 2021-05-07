import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EditClientAddressRequest, EditProfileAssignmentStatusRequest } from 'src/classes/request-types';

const endpoint = environment.profileServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ProfileService 
{
  constructor(private http: HttpClient) { }

  getClientIDForProfile(email: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Profile/' + email + "/GetID");
  }
  getProfileByEmail(email: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Profile/Email/' + email);
  }
  getProfileByID(id: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Profile/' + id);
  }
  getProfileAssignments(id: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Profile/' + id + "/Assignments");
  }
  getUnloadedChatHistories(id: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Profile/' + id + "/UnloadedHistories");
  }
  putProfileAssignmentStatus(clientID: string, assignmentXrefID: string, response: EditProfileAssignmentStatusRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Profile/' + clientID + '/Assignment/' + assignmentXrefID + '/AssignmentStatus', response);
  }
  putProfileAddress(clientID: string, updatedAddress: EditClientAddressRequest): Observable<any> 
  {
    // Endpoints specifically has security checks to make sure data is secure in address change call
    return this.http.put(endpoint + 'Profile/' + clientID + '/Address', updatedAddress);
  }
}
