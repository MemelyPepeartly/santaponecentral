import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ClientResponse, ClientAddressResponse, ClientEmailResponse, ClientNicknameResponse, ClientNameResponse, ClientStatusResponse, ClientRelationshipResponse } from '../../classes/responseTypes';

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
}
@Injectable({
  providedIn: 'root'
})
export class SantaApiPostService {

  constructor(private http: HttpClient) { }
  postClient(client: ClientResponse): Observable<any> {
    return this.http.post(endpoint + 'Client', client);
  }
  postClientRelation(id: string, relationship: ClientRelationshipResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + '/Relationship', relationship);
  }
}

@Injectable({
  providedIn: 'root'
})
export class SantaApiPutService {

  constructor(private http: HttpClient) { }
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
}
