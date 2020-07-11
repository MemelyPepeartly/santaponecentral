import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CountriesService } from 'src/app/services/countries.service';

@Component({
  selector: 'app-address-request',
  templateUrl: './address-request.component.html',
  styleUrls: ['./address-request.component.css']
})
export class AddressRequestComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public countryService: CountriesService) { }

  public clientAddressFormGroup: FormGroup;
  public countries: Array<any>=[];

  ngOnInit(): void {
    this.getCountries();
    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', Validators.required],
      addressLine2: ['', Validators.nullValidator],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required]
    });
    
  }
  public getCountries(){
    this.countryService.allCountries().
    subscribe(
      data2 => {
        this.countries=data2.Countries;
      },
      err => console.log(err))
  }

}
