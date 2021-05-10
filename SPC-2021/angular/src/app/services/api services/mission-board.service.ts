import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EditBoardEntryPostDescriptionRequest, EditBoardEntryPostNumberRequest, EditBoardEntryThreadNumberRequest, EditBoardEntryTypeRequest, EditEntryTypeDescriptionRequest, EditEntryTypeNameRequest, NewBoardEntryRequest, NewEntryTypeRequest } from 'src/classes/request-types';

const endpoint = environment.sharkTankServiceEndpoint;

@Injectable({
  providedIn: 'root'
})
export class MissionBoardService 
{
  constructor(private http: HttpClient) { }

    /* GET */
    getAllBoardEntries(): Observable<any> 
    {
      return this.http.get(endpoint + 'Board');
    }
    getBoardEntryByID(entryID: string): Observable<any> 
    {
      return this.http.get(endpoint + 'Board/' + entryID);
    }
    getBoardEntryByPostNumber(threadNumber: number, postNumber: number): Observable<any> 
    {
      return this.http.get(endpoint + 'Board/ThreadNumber/' + threadNumber + 'PostNumber/' + postNumber);
    }
    /* ENTRY TYPES */
    getAllEntryTypes(): Observable<any> 
    {
      return this.http.get(endpoint + 'EntryType');
    }
    getEntryTypeByID(entryTypeID: string): Observable<any> 
    {
      return this.http.get(endpoint + 'EntryType/' + entryTypeID);
    }

    /* POST */
    postNewBoardEntry(body: NewBoardEntryRequest): Observable<any> 
    {
      return this.http.post(endpoint + 'Board', body);
    }
    postNewEntryType(body: NewEntryTypeRequest): Observable<any> 
    {
      return this.http.post(endpoint + 'EntryType', body);
    }

    /* PUT */
    putBoardEntryThreadNumber(entryID: string, body: EditBoardEntryThreadNumberRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'Board/' + entryID + "/ThreadNumber", body);
    }
    putBoardEntryPostNumber(entryID: string, body: EditBoardEntryPostNumberRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'Board/' + entryID + "/PostNumber", body);
    }
    putBoardEntryPostDescription(entryID: string, body: EditBoardEntryPostDescriptionRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'Board/' + entryID + "/PostDescription", body);
    }
    putBoardEntryType(entryID: string, body: EditBoardEntryTypeRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'Board/' + entryID + "/EntryType", body);
    }
    putEntryTypeName(entryTypeID: string, body: EditEntryTypeNameRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'EntryType/' + entryTypeID + "/Name", body);
    }
    putEntryTypeDescription(entryTypeID: string, body: EditEntryTypeDescriptionRequest): Observable<any> 
    {
      return this.http.put(endpoint + 'EntryType/' + entryTypeID + "/Description", body);
    }

    /* DELETE */
    deleteBoardEntryByID(boardEntryID: string): Observable<any> 
    {
      return this.http.delete(endpoint + 'Board/' + boardEntryID);
    }
}