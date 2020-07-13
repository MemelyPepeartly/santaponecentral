  
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  url :string = "../assets/countries.json";

  constructor(private http:HttpClient) { }

  allCountries(): Observable<any>{
    return this.http.get(this.url);
  }
}