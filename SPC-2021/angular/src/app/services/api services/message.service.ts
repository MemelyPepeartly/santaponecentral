import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddMessageRequest, EditMessageApiReadAllRequest, EditMessageApiReadRequest } from 'src/classes/request-types';

const endpoint = environment.profileServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class MessageService 
{
  constructor(private http: HttpClient) { }

  /* GET */
  /**
   * Gets a message history by ID's. Automatically determines by the args if it is a general history or an assignment one.
   * @param clientID Conversation client ID
   * @param subjectID Subject ID (The person viewing the messages in browser). This is an argument ID for the API side
   * @param clientRelationXrefID The ID of the relationship if there is one
   */
  getClientMessageHistoryBySubjectIDAndXrefID(conversationClientID: string, subjectID: string, clientRelationXrefID?: string): Observable<any> 
  {
    //Necessary for the correct call to be made where the clientRelationXrefID is null
    // ClientID is the conversationClient, subjectID is who is reading the messages
    if(clientRelationXrefID == null || clientRelationXrefID == undefined)
    {
      return this.http.get(endpoint + "History/Client/" + conversationClientID + "/General?subjectID=" + subjectID);
    }
    return this.http.get(endpoint + "History/Relationship/" + clientRelationXrefID + "?subjectID=" + subjectID);
  }

  getAllMessageHistories(subjectID: string): Observable<any> 
  {
    // Gets all the message histories with a defined subjectID to determine the viewing party
    return this.http.get(endpoint + "History?subjectID=" + subjectID);
  }
  getAllMessageHistoriesByClientID(clientID: string): Observable<any> 
  {
    // Gets mesage histories for a client. ClientID is used as the subject of the conversation with this method to determine the viewing party
    return this.http.get(endpoint + "History/Client/" + clientID + "/Relationship");
  }

  /* POST */
  postMessage(messageResponse: AddMessageRequest): Observable<any> 
  {
    return this.http.post(endpoint + 'Message', messageResponse);
  }

  /* PUT */
  putMessageReadStatus(id: string, updatedMessage: EditMessageApiReadRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Message/' + id + '/Read', updatedMessage);
  }
  putMessageReadAll(messages: EditMessageApiReadAllRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Message/ReadAll', messages);
  }

  /* DELETE */
}
