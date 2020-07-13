  
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import * as countries from "../../assets/countries.json"

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  constructor() { }



  allCountries(){
    return countries.Countries;
  }
}