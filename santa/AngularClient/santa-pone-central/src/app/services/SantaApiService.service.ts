import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ClientResponse, ClientAddressResponse, ClientEmailResponse, ClientNicknameResponse, ClientNameResponse, ClientStatusResponse, ClientRelationshipResponse, SurveyApiResponse, TagResponse, ClientTagRelationshipResponse } from '../../classes/responseTypes';
import { ClientSenderRecipientRelationship } from 'src/classes/client';

//const endpoint = 'https://dev-santaponecentral-api.azurewebsites.net/api/';
const endpoint = 'https://localhost:5001/api/';

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
  getSurveyResponseByClientID(id): Observable<any> {
    return this.http.get(endpoint + 'Client/'+ id + '/Response').pipe(
      map(this.extractData));
  }
  getProfile(email): Observable<any> {
    return this.http.get(endpoint + 'Profile/' + email).pipe(
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
  getAllSurveys(): Observable<any> {
    return this.http.get(endpoint + 'Survey').pipe(
      map(this.extractData));
  }
  getSurvey(id): Observable<any> {
    return this.http.get(endpoint + 'Survey/' + id).pipe(
      map(this.extractData));
  }
  getAllSurveyQuestions(): Observable<any> {
    return this.http.get(endpoint + 'SurveyQuestion').pipe(
      map(this.extractData));
  }
  getSurveyQuestion(id): Observable<any> {
    return this.http.get(endpoint + 'SurveyQuestion/' + id).pipe(
      map(this.extractData));
  }
  getAllSurveyOptions(): Observable<any> {
    return this.http.get(endpoint + 'SurveyOption').pipe(
      map(this.extractData));
  }
  getSurveyOption(id): Observable<any> {
    return this.http.get(endpoint + 'SurveyOption/' + id).pipe(
      map(this.extractData));
  }
  getAllSurveyResponses(): Observable<any> {
    return this.http.get(endpoint + 'SurveyResponse').pipe(
      map(this.extractData));
  } 
  getSurveyResponse(id): Observable<any> {
    return this.http.get(endpoint + 'SurveyResponse/' + id).pipe(
      map(this.extractData));
  }
  getAllTags(): Observable<any> {
    return this.http.get(endpoint + "Tag").pipe(
      map(this.extractData));
  }
  getTag(id): Observable<any> {
    return this.http.get(endpoint + "Tag/" + id).pipe(
      map(this.extractData));
  }
}
@Injectable({
  providedIn: 'root'
})
export class SantaApiPostService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  postClient(client: ClientResponse): Observable<any> {
    return this.http.post(endpoint + 'Client', client);
  }
  postClientRecipient(id: string, relationship: ClientRelationshipResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + '/Recipient', relationship);
  }
  postSurveyResponse(surveyResponse: SurveyApiResponse): Observable<any> {
    return this.http.post(endpoint + 'SurveyResponse', surveyResponse);
  }
  postTag(tagResponse: TagResponse): Observable<any> {
    return this.http.post(endpoint + 'Tag', tagResponse);
  }
  postTagToClient(clientTagRelationship: ClientTagRelationshipResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/'+ clientTagRelationship.clientID + "/Tag?tagID=" + clientTagRelationship.tagID, null);
  }
}

@Injectable({
  providedIn: 'root'
})
export class SantaApiPutService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  putClientAddress(id: string, updatedClient: ClientAddressResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Address', updatedClient);
  }
  putClientEmail(id: string, updatedClient: ClientEmailResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Email', updatedClient);
  }
  putClientNickname(id: string, updatedClient: ClientNicknameResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Nickname', updatedClient);
  }
  putClientName(id: string, updatedClient: ClientNameResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Name', updatedClient);
  }
  putClientStatus(id: string, updatedClient: ClientStatusResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Status', updatedClient);
  }
  putTagName(id: string, updatedTag: TagResponse): Observable<any> {
    return this.http.put(endpoint + 'Tag/' + id, updatedTag).pipe(map(this.extractData));
   
  }
}
@Injectable({
  providedIn: 'root'
})
export class SantaApiDeleteService {
  
  constructor(private http: HttpClient) { }
  deleteClientRecipient(id: string, relationship: ClientSenderRecipientRelationship): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + id + '/Recipient?recipientID=' + relationship.clientID+'&eventID=' + relationship.clientEventTypeID);
  }
  deleteTagFromClient(clientTagRelationship: ClientTagRelationshipResponse): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + clientTagRelationship.clientID + '/Tag?tagID=' + clientTagRelationship.tagID);
  }
  deleteTag(id: string): Observable<any> {
    return this.http.delete(endpoint + 'Tag/' + id);
  }
}
