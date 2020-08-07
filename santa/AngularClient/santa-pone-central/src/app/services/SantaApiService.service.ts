import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ClientResponse, ClientAddressResponse, ClientEmailResponse, ClientNicknameResponse, ClientNameResponse, ClientStatusResponse, SurveyApiResponse, TagResponse, MessageApiResponse, MessageApiReadResponse, ClientSignupResponse, ClientRelationshipsResponse, RecipientCompletionResponse, QuestionReadabilityResponse, ClientSenderRecipientRelationshipReponse, MessageApiReadAllResponse, ClientTagRelationshipsResponse, ClientTagRelationshipResponse, ClientIsAdminResponse } from '../../classes/responseTypes';
import { ClientSenderRecipientRelationship } from 'src/classes/client';
import { AuthService } from '../auth/auth.service';
import { environment } from 'src/environments/environment';

const endpoint = environment.apiUrl;

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
  getClientByClientID(id): Observable<any> {
    return this.http.get(endpoint + 'Client/' + id).pipe(
      map(this.extractData));
  }
  getClientByEmail(email: string): Observable<any> {
    return this.http.get(endpoint + 'Client/Email/' + email).pipe(
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
  getAllMessages(): Observable<any> {
    return this.http.get(endpoint + "Message").pipe(
      map(this.extractData));
  }
  getMessage(id): Observable<any> {
    return this.http.get(endpoint + "Message/" + id).pipe(
      map(this.extractData));
  }
  getAllMessageHistories(subjectID: string): Observable<any> {
    // Gets all the message histories with a defined subjectID to determine the viewing party
    return this.http.get(endpoint + "History?subjectID=" + subjectID).pipe(
      map(this.extractData));
  }
  getAllMessageHistoriesByClientID(clientID): Observable<any> {
    // Gets mesage histories for a client. ClientID is used as the subject of the conversation with this method to determine the viewing party
    return this.http.get(endpoint + "History/Client/" + clientID + "/Relationship").pipe(
      map(this.extractData));
  }
  getClientMessageHistoryBySubjectIDAndXrefID(clientID: string, subjectID: string, clientRelationXrefID?: string): Observable<any> {
    //Necessary for the correct call to be made where the clientRelationXrefID is null
    // ClientID is the conversationClient, subjectID is who is reading the messages
    if(clientRelationXrefID == null || clientRelationXrefID == undefined)
    {
      return this.http.get(endpoint + "History/Client/" + clientID + "/General?subjectID=" + subjectID).pipe(
        map(this.extractData));
    }
    return this.http.get(endpoint + "History/Relationship/" + clientRelationXrefID + "?subjectID=" + subjectID).pipe(
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
  postClientRecipients(id: string, relationships: ClientRelationshipsResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + '/Recipients', relationships);
  }
  postClientSignup(signup: ClientSignupResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/Signup', signup);
  }
  postSurveyResponse(surveyResponse: SurveyApiResponse): Observable<any> {
    return this.http.post(endpoint + 'SurveyResponse', surveyResponse);
  }
  postTag(tagResponse: TagResponse): Observable<any> {
    return this.http.post(endpoint + 'Tag', tagResponse);
  }
  postTagsToClient(clientID: string, clientTagRelationships: ClientTagRelationshipsResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/'+ clientID + "/Tags", clientTagRelationships);
  }
  postMessage(messageResponse: MessageApiResponse): Observable<any> {
    return this.http.post(endpoint + 'Message', messageResponse);
  }
  postPasswordResetToClient(id: string): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + "/Password", {});
  }
  postAutoAssignmentRequest(): Observable<any> {
    // Returns a list of strings of added relationships
    return this.http.post(endpoint + 'Client/AutoAssign', {}).pipe(
      map(this.extractData));
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
  putClientIsAdmin(id: string, updatedClient: ClientIsAdminResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Admin', updatedClient);
  }
  putClientRelationshipCompletionStatus(id: string, relationshipModel: RecipientCompletionResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Recipient', relationshipModel);
  }
  putClientStatus(id: string, updatedClient: ClientStatusResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Status', updatedClient);
  }
  putTagName(id: string, updatedTag: TagResponse): Observable<any> {
    return this.http.put(endpoint + 'Tag/' + id, updatedTag).pipe(map(this.extractData));
  }
  putMessageReadStatus(id: string, updatedMessage: MessageApiReadResponse): Observable<any> {
    return this.http.put(endpoint + 'Message/' + id + '/Read', updatedMessage);
  }
  putMessageReadAll(messages: MessageApiReadAllResponse): Observable<any> {
    return this.http.put(endpoint + 'Message/ReadAll', messages);
  }
  putQuestionReadability(id: string, questionModel: QuestionReadabilityResponse): Observable<any> {
    return this.http.put(endpoint + 'SurveyQuestion/' + id + '/Readability', questionModel);
  }
}
@Injectable({
  providedIn: 'root'
})
export class SantaApiDeleteService {
  
  constructor(private http: HttpClient) { }
  deleteClient(id: string): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + id);
  }
  deleteClientRecipient(id: string, relationship: ClientSenderRecipientRelationshipReponse): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + id + '/Recipient?recipientID=' + relationship.clientID+'&eventID=' + relationship.clientEventTypeID);
  }
  deleteTagFromClient(clientTagRelationship: ClientTagRelationshipResponse): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + clientTagRelationship.clientID + '/Tag?tagID=' + clientTagRelationship.tagID);
  }
  deleteTag(id: string): Observable<any> {
    return this.http.delete(endpoint + 'Tag/' + id);
  }
}
