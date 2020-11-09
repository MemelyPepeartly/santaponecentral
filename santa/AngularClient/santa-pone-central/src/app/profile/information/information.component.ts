import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/profile.service';
import { EventType } from 'src/classes/eventType';
import { ChangeSurveyResponseModel, ClientAddressResponse } from 'src/classes/responseTypes';
import { ProfileApiService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { SurveyResponse, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { CountriesService } from 'src/app/services/countries.service';
import { SurveyConstants } from 'src/app/shared/constants/surveyConstants.enum';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public ProfileApiService: ProfileApiService,
    public profileService: ProfileService,
    public SantaApiPut: SantaApiPutService,
    public countryService: CountriesService) { }

  @Input() loading: boolean;
  @Input() profile: Profile;
  @Input() surveys: Array<Survey>;

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
  public async submitNewAddress()
  {
    this.changingAddress = true;

    let newAddressResponse: ClientAddressResponse =
    {
      clientAddressLine1: this.addressFormControls.addressLine1.value,
      clientAddressLine2: this.addressFormControls.addressLine2.value,
      clientCity: this.addressFormControls.city.value,
      clientState: this.addressFormControls.state.value,
      clientPostalCode: this.addressFormControls.postalCode.value,
      clientCountry: this.addressFormControls.country.value
    }

    await this.ProfileApiService.putProfileAddress(this.profile.clientID, newAddressResponse).toPromise();
    await this.profileService.getProfileByID(this.profile.clientID);

    this.clientAddressFormGroup.reset();
    this.showAddressChangeForm = false;
    this.changingAddress = false;
  }
  public async softRefreshProfile()
  {
    await this.profileService.getProfileByID(this.profile.clientID);
  }
  public showSurvey(survey: Survey)
  {
    // If the responses from the client have any responses for the survey, return true to show that survey
    if(this.profile.responses.some((response: SurveyResponse) => {return response.surveyID == survey.surveyID}) || survey.surveyDescription == SurveyConstants.MASS_MAIL_SURVEY)
    {
      return true;
    }
    // Else, they didn't answer any questions for it, so dont show it
    else
    {
      return false;
    }
  }
}
