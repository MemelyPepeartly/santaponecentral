import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { SurveyResponse, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { CountriesService } from 'src/app/services/utility services/countries.service';
import { SurveyConstants } from 'src/app/shared/constants/surveyConstants.enum';
import { MessageHistory } from 'src/classes/message';
import { AuthService } from '@auth0/auth0-angular';
import { ProfileGatheringService } from 'src/app/services/gathering services/profile-gathering.service';
import { ProfileService } from 'src/app/services/api services/profile.service';
import { EditClientAddressRequest } from 'src/classes/request-types';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public ProfileService: ProfileService,
    public ProfileGatheringService: ProfileGatheringService,
    public auth: AuthService,
    public countryService: CountriesService) { }

  @Input() loading: boolean;
  @Input() profile: Profile;
  @Input() surveys: Array<Survey>;
  @Input() generalHistory: MessageHistory = new MessageHistory();

  @Output() generalHistoryButtonSelectedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();

  public editingResponse: boolean;
  public changingAddress: boolean;

  public showAddressChangeForm: boolean = false;
  public isAdmin: boolean;

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

    /* SANTAHERE fix this come the time
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
    */
  }
  public async submitNewAddress()
  {
    this.changingAddress = true;

    let newAddressResponse: EditClientAddressRequest =
    {
      clientAddressLine1: this.addressFormControls.addressLine1.value,
      clientAddressLine2: this.addressFormControls.addressLine2.value,
      clientCity: this.addressFormControls.city.value,
      clientState: this.addressFormControls.state.value,
      clientPostalCode: this.addressFormControls.postalCode.value,
      clientCountry: this.addressFormControls.country.value
    }

    await this.ProfileService.putProfileAddress(this.profile.clientID, newAddressResponse).toPromise();
    await this.ProfileGatheringService.getProfileByID(this.profile.clientID);

    this.clientAddressFormGroup.reset();
    this.showAddressChangeForm = false;
    this.changingAddress = false;
  }
  public async softRefreshProfile()
  {
    await this.ProfileGatheringService.getProfileByID(this.profile.clientID);
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
  public emitGeneralHistorySelected()
  {
    this.generalHistoryButtonSelectedEvent.emit(null);
  }
}
