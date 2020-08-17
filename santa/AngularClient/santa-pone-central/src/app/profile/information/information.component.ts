import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';
import { EventType } from 'src/classes/eventType';
import { ChangeSurveyResponseModel, ClientAddressResponse } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { SurveyResponse } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { CountriesService } from 'src/app/services/countries.service';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public profileService: ProfileService,
    public SantaApiPut: SantaApiPutService,
    public countryService: CountriesService) { }

  @Input() loading: boolean;
  @Input() profile: Profile;
  @Input() events: Array<EventType>;
  @Input() clientResponseFormGroup: FormGroup;

  public editingResponse: boolean;
  public changingAddress: boolean;

  public showAddressChangeForm: boolean;

  public countries: Array<any> = [];
  public clientAddressFormGroup: FormGroup;
  get addressFormControls()
  {
    return this.clientAddressFormGroup.controls;
  }


  ngOnInit(): void {
    this.countries = this.countryService.allCountries();
    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      addressLine2: ['', [Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      state: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.maxLength(25)]],
      country: ['', Validators.required]
    });
  }
  public async submitNewResponse(surveyResponseID: string)
  {
    this.editingResponse = true;

    let editedResponse = new ChangeSurveyResponseModel();
    editedResponse.responseText = this.clientResponseFormGroup.get(surveyResponseID).value
    await this.SantaApiPut.putResponse(surveyResponseID, editedResponse).toPromise();
    await this.profileService.getProfile(this.profile.email);

    this.editingResponse = false;
  }
  public async submitNewAddress()
  {
    this.changingAddress = true;

    let newAddressResponse = new ClientAddressResponse();

    newAddressResponse.clientAddressLine1 = this.addressFormControls.addressLine1.value;
    newAddressResponse.clientAddressLine2 = this.addressFormControls.addressLine2.value;
    newAddressResponse.clientCity = this.addressFormControls.city.value;
    newAddressResponse.clientState = this.addressFormControls.state.value;
    newAddressResponse.clientCountry = this.addressFormControls.country.value;
    newAddressResponse.clientPostalCode = this.addressFormControls.postalCode.value;

    await this.SantaApiPut.putProfileAddress(this.profile.clientID, newAddressResponse).toPromise();
    await this.profileService.getProfile(this.profile.email);
    
    this.clientAddressFormGroup.reset();
    this.showAddressChangeForm = false;
    this.changingAddress = false;
  }
}
