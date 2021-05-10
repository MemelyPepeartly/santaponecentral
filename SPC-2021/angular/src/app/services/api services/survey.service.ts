import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SurveySignupApiRequest, EditSurveyResponseRequest } from 'src/classes/request-types';

const endpoint = environment.surveyServiceEndpoint;

@Injectable({
  providedIn: 'root'
})
export class SurveyService 
{
  constructor(private http: HttpClient) { }

  /* GET */
  getAllSurveys(): Observable<any> 
  {
    return this.http.get(endpoint + 'Survey');
  }
  getSurvey(surveyID: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Survey/' + surveyID);
  }
  getAllSurveyQuestions(): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyQuestion');
  }
  getSurveyQuestion(questionID: string): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyQuestion/' + questionID);
  }
  getAllSurveyOptions(): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyOption');
  }
  getSurveyOption(questionOptionID: string): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyOption/' + questionOptionID);
  }
  getAllSurveyResponses(): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyResponse');
  }
  getSurveyResponse(surveyResponseID: string): Observable<any> 
  {
    return this.http.get(endpoint + 'SurveyResponse/' + surveyResponseID);
  }
  /* POST */
  postSurveyResponse(surveyResponse: SurveySignupApiRequest): Observable<any> 
  {
    return this.http.post(endpoint + 'SurveyResponse', surveyResponse);
  }
  /* PUT */
  putResponse(surveyResponseID: string, responseModel: EditSurveyResponseRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'SurveyResponse/' + surveyResponseID + '/ResponseText', responseModel);
  }
  /* DELETE */
  deleteQuestionRelationFromSurvey(surveyId: string, surveyQuestionId: string): Observable<any> 
  {
    return this.http.delete(endpoint + 'Survey/' + surveyId + "/SurveyQuestion/" + surveyQuestionId);
  }
}
