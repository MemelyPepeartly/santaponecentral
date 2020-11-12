import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ClientResponse,
  ClientAddressResponse,
  ClientEmailResponse,
  ClientNicknameResponse,
  ClientNameResponse,
  ClientStatusResponse,
  SurveyApiResponse,
  TagResponse,
  MessageApiResponse,
  MessageApiReadResponse,
  ClientSignupResponse,
  ClientRelationshipsResponse,
  QuestionReadabilityResponse,
  ClientSenderRecipientRelationshipReponse,
  MessageApiReadAllResponse,
  ClientTagRelationshipsResponse,
  ClientTagRelationshipResponse,
  ClientIsAdminResponse,
  ChangeSurveyResponseModel,
  SurveyQuestionXrefsResponseModel,
  NewBoardEntryResponse,
  EditBoardEntryPostNumberResponse,
  EditBoardEntryPostDescriptionResponse,
  EditBoardEntryTypeResponse,
  NewEntryTypeResponse,
  EditEntryTypeName,
  EditEntryTypeDescription,
  EditBoardEntryThreadNumberResponse, NewAssignmentStatusResponse, EditProfileAssignmentStatusResponse, SearchQueryModelResponse, SelectedAutoAssignmentsResponse, NewNoteResponse, EditNoteResponse} from '../../classes/responseTypes';
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
  getAllTruncatedClients(): Observable<any> {
    return this.http.get(endpoint + 'Client/Truncated').pipe(
      map(this.extractData));
  }
  getAllHQClients(): Observable<any> {
    return this.http.get(endpoint + 'Client/Headquarters').pipe(
      map(this.extractData));
  }
  getInfoContainerByClientID(id: string): Observable<any> {
    return this.http.get(endpoint + 'Client/' + id + "/InfoContainer").pipe(
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
  getBasicClientByClientID(id): Observable<any> {
    return this.http.get(endpoint + 'Client/Basic/' + id).pipe(
      map(this.extractData));
  }
  getBasicClientByEmail(email: string): Observable<any> {
    return this.http.get(endpoint + 'Client/Basic/Email/' + email).pipe(
      map(this.extractData));
  }
  getAllowedAssignmentsByID(clientID: string, eventTypeID: string) : Observable<any> {
    return this.http.get(endpoint + 'Client/' + clientID + '/AllowedAssignment/' + eventTypeID).pipe(
      map(this.extractData));
  }
  getSurveyResponseByClientID(id): Observable<any> {
    return this.http.get(endpoint + 'Client/'+ id + '/Response').pipe(
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
  getClientStatusByEmail(email: string): Observable<any> {
    return this.http.get(endpoint + 'Status/Check/' + email).pipe(
      map(this.extractData));
  }
  getAllAssignmentStatuses(): Observable<any> {
    return this.http.get(endpoint + 'AssignmentStatus').pipe(
      map(this.extractData));
  }
  getAssignmentStatusByID(assignmentStatusID: string): Observable<any> {
    return this.http.get(endpoint + 'AssignmentStatus/' + assignmentStatusID).pipe(
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
  getAllNotes(): Observable<any> {
    return this.http.get(endpoint + "Note").pipe(
      map(this.extractData));
  }
  getNoteByID(id: string): Observable<any> {
    return this.http.get(endpoint + "Note/" + id).pipe(
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
  getAutoAssignmentPairings(): Observable<any> {
    // Returns a list of strings of added relationships
    return this.http.get(endpoint + 'Client/AutoAssignmentPairs').pipe(
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
    return this.http.post(endpoint + 'Client', client).pipe(
      map(this.extractData));
  }
  postClientRecipients(id: string, relationships: ClientRelationshipsResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + '/Recipients', relationships).pipe(
      map(this.extractData));
  }
  postClientSignup(signup: ClientSignupResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/Signup', signup).pipe(
      map(this.extractData));
  }
  postClientNewAuth0Account(clientID: string): Observable<any> {
    return this.http.post(endpoint + 'Client/' + clientID + '/CreateAccount', {}).pipe(
      map(this.extractData));
  }
  postSurveyResponse(surveyResponse: SurveyApiResponse): Observable<any> {
    return this.http.post(endpoint + 'SurveyResponse', surveyResponse).pipe(
      map(this.extractData));
  }
  postTag(tagResponse: TagResponse): Observable<any> {
    return this.http.post(endpoint + 'Tag', tagResponse).pipe(
      map(this.extractData));
  }
  postTagsToClient(clientID: string, clientTagRelationships: ClientTagRelationshipsResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/'+ clientID + "/Tags", clientTagRelationships).pipe(
      map(this.extractData));
  }
  postMessage(messageResponse: MessageApiResponse): Observable<any> {
    return this.http.post(endpoint + 'Message', messageResponse).pipe(
      map(this.extractData));
  }
  postPasswordResetToClient(id: string): Observable<any> {
    return this.http.post(endpoint + 'Client/' + id + "/Password", {}).pipe(
      map(this.extractData));
  }
  postQuestionsToSurvey(surveyID: string, questions: SurveyQuestionXrefsResponseModel): Observable<any> {
    return this.http.post(endpoint + 'Survey/' + surveyID + "/SurveyQuestion", questions).pipe(
      map(this.extractData));
  }
  postAssignmentStatus(assignmentStatusResponse: NewAssignmentStatusResponse): Observable<any> {
    return this.http.post(endpoint + 'AssignmentStatus', assignmentStatusResponse).pipe(
      map(this.extractData));
  }
  postSelectedAutoAssignments(assignmentPairingResponse: SelectedAutoAssignmentsResponse): Observable<any> {
    return this.http.post(endpoint + 'Client/AutoAssignments', assignmentPairingResponse).pipe(
      map(this.extractData));
  }
  postNewClientNote(newNoteResponse: NewNoteResponse): Observable<any> {
    return this.http.post(endpoint + 'Note', newNoteResponse).pipe(
      map(this.extractData));
  }
  searchClients(body: SearchQueryModelResponse): Observable<any> {
    return this.http.post(endpoint + 'Catalogue/SearchClients', body).pipe(
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
    return this.http.put(endpoint + 'Client/' + id + '/Address', updatedClient).pipe(map(this.extractData));
  }
  putClientEmail(id: string, updatedClient: ClientEmailResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Email', updatedClient).pipe(map(this.extractData));
  }
  putClientNickname(id: string, updatedClient: ClientNicknameResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Nickname', updatedClient).pipe(map(this.extractData));
  }
  putClientName(id: string, updatedClient: ClientNameResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Name', updatedClient).pipe(map(this.extractData));
  }
  putClientIsAdmin(id: string, updatedClient: ClientIsAdminResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Admin', updatedClient).pipe(map(this.extractData));
  }
  putAssignmentStatus(clientID: string, assignmentXrefID: string, response: EditProfileAssignmentStatusResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + clientID + '/Relationship/' + assignmentXrefID + "/AssignmentStatus", response).pipe(map(this.extractData));
  }
  putClientStatus(id: string, updatedClient: ClientStatusResponse): Observable<any> {
    return this.http.put(endpoint + 'Client/' + id + '/Status', updatedClient).pipe(map(this.extractData));
  }
  putTagName(id: string, updatedTag: TagResponse): Observable<any> {
    return this.http.put(endpoint + 'Tag/' + id, updatedTag).pipe(map(this.extractData));
  }
  putMessageReadStatus(id: string, updatedMessage: MessageApiReadResponse): Observable<any> {
    return this.http.put(endpoint + 'Message/' + id + '/Read', updatedMessage).pipe(map(this.extractData));
  }
  putMessageReadAll(messages: MessageApiReadAllResponse): Observable<any> {
    return this.http.put(endpoint + 'Message/ReadAll', messages).pipe(map(this.extractData));
  }
  putQuestionReadability(id: string, questionModel: QuestionReadabilityResponse): Observable<any> {
    return this.http.put(endpoint + 'SurveyQuestion/' + id + '/Readability', questionModel).pipe(map(this.extractData));
  }
  putResponse(surveyResponseID: string, responseModel: ChangeSurveyResponseModel): Observable<any> {
    return this.http.put(endpoint + 'SurveyResponse/' + surveyResponseID + '/ResponseText', responseModel).pipe(map(this.extractData));
  }
  putNote(noteID: string, responseModel: EditNoteResponse): Observable<any> {
    return this.http.put(endpoint + 'Note/' + noteID, responseModel).pipe(map(this.extractData));
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
    return this.http.delete(endpoint + 'Client/' + id + '/Recipient?assignmentClientID=' + relationship.clientID+'&eventID=' + relationship.clientEventTypeID);
  }
  deleteTagFromClient(clientTagRelationship: ClientTagRelationshipResponse): Observable<any> {
    return this.http.delete(endpoint + 'Client/' + clientTagRelationship.clientID + '/Tag?tagID=' + clientTagRelationship.tagID);
  }
  deleteTag(id: string): Observable<any> {
    return this.http.delete(endpoint + 'Tag/' + id);
  }
  deleteQuestionRelationFromSurvey(surveyId: string, surveyQuestionId: string): Observable<any> {
    return this.http.delete(endpoint + 'Survey/' + surveyId + "/SurveyQuestion/" + surveyQuestionId);
  }
  deleteNote(id: string): Observable<any> {
    return this.http.delete(endpoint + 'Note/' + id);
  }
}
@Injectable({
  providedIn: 'root'
})
export class MissionBoardAPIService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  /* BOARD ENTRIES */
  getAllBoardEntries(): Observable<any> {
    return this.http.get(endpoint + 'Board').pipe(
      map(this.extractData));
  }
  getBoardEntryByID(entryID: string): Observable<any> {
    return this.http.get(endpoint + 'Board/' + entryID).pipe(
      map(this.extractData));
  }
  getBoardEntryByPostNumber(threadNumber: number, postNumber: number): Observable<any> {
    return this.http.get(endpoint + 'Board/ThreadNumber/' + threadNumber + 'PostNumber/' + postNumber).pipe(
      map(this.extractData));
  }
  postNewBoardEntry(body: NewBoardEntryResponse): Observable<any> {
    return this.http.post(endpoint + 'Board', body).pipe(
      map(this.extractData));
  }
  putBoardEntryThreadNumber(entryID: string, body: EditBoardEntryThreadNumberResponse): Observable<any> {
    return this.http.put(endpoint + 'Board/' + entryID + "/ThreadNumber", body).pipe(
      map(this.extractData));
  }
  putBoardEntryPostNumber(entryID: string, body: EditBoardEntryPostNumberResponse): Observable<any> {
    return this.http.put(endpoint + 'Board/' + entryID + "/PostNumber", body).pipe(
      map(this.extractData));
  }
  putBoardEntryPostDescription(entryID: string, body: EditBoardEntryPostDescriptionResponse): Observable<any> {
    return this.http.put(endpoint + 'Board/' + entryID + "/PostDescription", body).pipe(
      map(this.extractData));
  }
  putBoardEntryType(entryID: string, body: EditBoardEntryTypeResponse): Observable<any> {
    return this.http.put(endpoint + 'Board/' + entryID + "/EntryType", body).pipe(
      map(this.extractData));
  }
  deleteBoardEntryByID(boardEntryID: string): Observable<any> {
    return this.http.delete(endpoint + 'Board/' + boardEntryID);
  }
  /* ENTRY TYPES */
  getAllEntryTypes(): Observable<any> {
    return this.http.get(endpoint + 'EntryType').pipe(
      map(this.extractData));
  }
  getEntryTypeByID(entryTypeID: string): Observable<any> {
    return this.http.get(endpoint + 'EntryType/' + entryTypeID).pipe(
      map(this.extractData));
  }
  postNewEntryType(body: NewEntryTypeResponse): Observable<any> {
    return this.http.post(endpoint + 'EntryType', body).pipe(
      map(this.extractData));
  }
  putEntryTypeName(entryTypeID: string, body: EditEntryTypeName): Observable<any> {
    return this.http.put(endpoint + 'EntryType/' + entryTypeID + "/Name", body).pipe(
      map(this.extractData));
  }
  putEntryTypeDescription(entryTypeID: string, body: EditEntryTypeDescription): Observable<any> {
    return this.http.put(endpoint + 'EntryType/' + entryTypeID + "/Description", body).pipe(
      map(this.extractData));
  }

}

@Injectable({
  providedIn: 'root'
})
export class YuleLogService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  getAllCategories(): Observable<any> {
    return this.http.get(endpoint + 'Category').pipe(
      map(this.extractData));
  }
  getAllLogs(): Observable<any> {
    return this.http.get(endpoint + 'Log').pipe(
      map(this.extractData));
  }
  getAllLogsByCategoryID(id: string): Observable<any> {
    return this.http.get(endpoint + 'Log/Category/' + id).pipe(
      map(this.extractData));
  }
}

@Injectable({
  providedIn: 'root'
})
export class ProfileApiService {

  constructor(private http: HttpClient) { }
  private extractData(res: Response) {
    const body = res;
    return body || { };
  }
  getClientIDForProfile(email: string): Observable<any> {
    return this.http.get(endpoint + 'Profile/' + email + "/GetID").pipe(
      map(this.extractData));
  }
  getProfileByEmail(email: string): Observable<any> {
    return this.http.get(endpoint + 'Profile/Email/' + email).pipe(
      map(this.extractData));
  }
  getProfileByID(id: string): Observable<any> {
    return this.http.get(endpoint + 'Profile/' + id).pipe(
      map(this.extractData));
  }
  getProfileAssignments(id: string): Observable<any> {
    return this.http.get(endpoint + 'Profile/' + id + "/Assignments").pipe(
      map(this.extractData));
  }
  getUnloadedChatHistories(id: string): Observable<any> {
    return this.http.get(endpoint + 'Profile/' + id + "/UnloadedHistories").pipe(
      map(this.extractData));
  }
  putProfileAssignmentStatus(clientID: string, assignmentXrefID: string, response: EditProfileAssignmentStatusResponse): Observable<any> {
    return this.http.put(endpoint + 'Profile/' + clientID + '/Assignment/' + assignmentXrefID + '/AssignmentStatus', response).pipe(map(this.extractData));
  }
  putProfileAddress(clientID: string, updatedAddress: ClientAddressResponse): Observable<any> {
    // Endpoints specifically has security checks to make sure data is secure in address change call
    return this.http.put(endpoint + 'Profile/' + clientID + '/Address', updatedAddress).pipe(map(this.extractData));
  }
}
