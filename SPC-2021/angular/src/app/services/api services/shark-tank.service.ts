import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';

const endpoint = environment.sharkTankServiceEndpoint;

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class SharkTankService 
{
  constructor(private http: HttpClient) { }

  getAllCategories(): Observable<any> 
  {
    return this.http.get(endpoint + 'Category');
  }
  getAllLogs(): Observable<any> 
  {
    return this.http.get(endpoint + 'Log');
  }
  getAllLogsByCategoryID(id: string): Observable<any> 
  {
    return this.http.get(endpoint + 'Log/Category/' + id);
  }
}