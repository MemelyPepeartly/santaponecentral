  
import { Injectable } from '@angular/core';
import * as countries from "../../../assets/data/countries.json"

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  constructor() { }

  allCountries(){
    return countries.Countries;
  }
}
