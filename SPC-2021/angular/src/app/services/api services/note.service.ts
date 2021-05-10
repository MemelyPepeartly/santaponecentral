import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddNoteRequest, EditNoteRequest } from 'src/classes/request-types';

const endpoint = environment.sharkTankServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class NoteService 
{
  constructor(private http: HttpClient) { }

  /* GET */
  getAllNotes(): Observable<any> {
    return this.http.get(endpoint + "Note");
  }
  getNoteByID(id: string): Observable<any> {
    return this.http.get(endpoint + "Note/" + id);
  }

  /* POST */
  postNewClientNote(newNoteResponse: AddNoteRequest): Observable<any> 
  {
    return this.http.post(endpoint + 'Note', newNoteResponse);
  }

  /* PUT */
  putNote(noteID: string, responseModel: EditNoteRequest): Observable<any> 
  {
    return this.http.put(endpoint + 'Note/' + noteID, responseModel);
  }

  /* DELETE */
  deleteNote(id: string): Observable<any> 
  {
    return this.http.delete(endpoint + 'Note/' + id);
  }
}