import { Component, OnInit, ViewChildren, QueryList, Output, EventEmitter } from '@angular/core';
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/santa-api.service';
import { GathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { MapService, MapResponse } from 'src/app/services/utility services/mapper.service';
import { CountriesService } from 'src/app/services/utility services/countries.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { SurveyFormComponent } from 'src/app/signup/survey-form/survey-form.component';
import { EventType } from 'src/classes/eventType';
import { Survey } from 'src/classes/survey';
import { Status } from 'src/classes/status';
import { ClientSignupResponse, SurveyApiResponse } from 'src/classes/request-types';
import { Address } from 'src/classes/address';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';
import { Client } from 'src/classes/client';
import { SurveyConstants } from 'src/app/shared/constants/surveyConstants.enum';

@Component({
  selector: 'app-manual-add',
  templateUrl: './manual-add.component.html',
  styleUrls: ['./manual-add.component.css']
})
export class ManualAddComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService,
    public SantaPost: SantaApiPostService,
    public gatherer: GathererService,
    public mapper: MapService,
    public responseMapper: MapResponse,
    public countryService: CountriesService,
    private formBuilder: FormBuilder) { }

  @Output() clientPostedEvent: EventEmitter<Client> = new EventEmitter();

  @ViewChildren(SurveyFormComponent) surveyForms: QueryList<SurveyFormComponent>;

  public selectedEvents: Array<EventType> = new Array<EventType>();

  public events: Array<EventType> = [];
  public statuses: Array<Status> = [];
  public surveys: Array<Survey> = [];
  public countries: Array<any>=[];

  public clientInfoFormGroup: FormGroup;
  public clientAddressFormGroup: FormGroup;
  public clientEventFormGroup: FormGroup;
  public surveyFormGroup: FormGroup;

  //For determining of all questions on the surveys selected are answered
  public allQuestionsAnswered: boolean = false;
  public showSpinner: boolean = false;
  public showError: boolean = false;

  get readyToSubmit(): boolean
  {
    return this.clientAddressFormGroup.valid && this.clientInfoFormGroup.valid && this.allQuestionsAnswered;
  }

  get addressFormControls()
  {
    return this.clientAddressFormGroup.controls;
  }
  get nameFormControls()
  {
    return this.clientInfoFormGroup.controls;
  }
  get clientName()
  {
    var formControlFirst = this.clientInfoFormGroup.get('firstName') as FormControl
    var formControlLast = this.clientInfoFormGroup.get('lastName') as FormControl
    let clientName: string = formControlFirst.value + " " + formControlLast.value
    return clientName;
  }
  get clientAddress()
  {
    var formControlLine1 = this.clientAddressFormGroup.get('addressLine1') as FormControl;
    var formControlLine2 = this.clientAddressFormGroup.get('addressLine2') as FormControl;
    var formControlCity = this.clientAddressFormGroup.get('city') as FormControl;
    var formControlState = this.clientAddressFormGroup.get('state') as FormControl;
    var formControlPostal = this.clientAddressFormGroup.get('postalCode') as FormControl;
    var formControlCountry = this.clientAddressFormGroup.get('country') as FormControl;

    let address: Address =
    {
      addressLineOne: formControlLine1.value,
      addressLineTwo: formControlLine2.value,
      city: formControlCity.value,
      state: formControlState.value,
      country: formControlCountry.value,
      postalCode: formControlPostal.value,
    }

    return address;
  }

  async ngOnInit() {
    this.createFormGroups();

    // JSON call for getting country data
    this.countries = this.countryService.allCountries();

    // API Call for getting statuses
    this.gatherer.allStatuses.subscribe((statuses: Array<Status>) => {
      this.statuses = statuses;
    });

    // API Call for getting events
    this.gatherer.allEvents.subscribe((events: Array<EventType>) => {
      this.events = events;
    });

    // API Call for getting surveys
    this.gatherer.allSurveys.subscribe((surveys: Array<Survey>) => {
      this.surveys = surveys;
    });

    await this.gatherer.gatherAllStatuses();
    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllSurveys();
  }

  public createFormGroups()
  {
    this.clientInfoFormGroup = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
    });
    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', [Validators.required, Validators.maxLength(50)]],
      addressLine2: ['', [Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      state: ['', [Validators.required, Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.maxLength(25)]],
      country: ['', Validators.required]
    });
    this.clientEventFormGroup = this.formBuilder.group({
      eventDescription: ['', Validators.required]
    });
  }
  public async onSubmit()
  {
    this.showSpinner = true;
    var awaitingStatusID = this.statuses.find(status => status.statusDescription == StatusConstants.AWAITING);

    // Construction of new client response
    let newClient: ClientSignupResponse =
    {
      clientStatusID: awaitingStatusID.statusID,
      clientName: this.clientName,
      clientEmail: this.clientInfoFormGroup.get('email').value,
      clientNickname: "Anon",
      clientAddressLine1: this.clientAddress.addressLineOne,
      clientAddressLine2: this.clientAddress.addressLineTwo,
      clientCity: this.clientAddress.city,
      clientState: this.clientAddress.state,
      clientPostalCode: this.clientAddress.postalCode,
      clientCountry: this.clientAddress.country,
      isAdmin: false,
      hasAccount: false,
      responses: []
    }

    var forms = this.surveyForms.toArray()

    // Set client's answers
    forms.forEach((surveyForm: SurveyFormComponent) => {
      let response = new SurveyApiResponse;
      for (const field in surveyForm.surveyFormGroup.controls) // 'field' is a string equal to question ID
      {
        var control = surveyForm.surveyFormGroup.get(field); // 'control' is a FormControl

        response.surveyQuestionID = field;
        response.surveyID = surveyForm.survey.surveyID;

        if (control.value.surveyOptionID !== undefined) {
          response.surveyOptionID = control.value.surveyOptionID;
          response.responseText = control.value.optionText;
        }
        else {
          response.responseText = control.value;
        }
        newClient.responses.push(response);
        response = new SurveyApiResponse();
      }
    });

    // Post client with answers
    this.SantaPost.postClientSignup(newClient).subscribe(async (res) => {
      this.clientInfoFormGroup.reset();
      this.clientAddressFormGroup.reset();
      this.showSpinner = false;
      this.showError = false;
      this.clientPostedEvent.emit(this.mapper.mapClient(res));
    },(err) => {
      console.log("Something went wrong on signup. Error is as follows: ");
      console.log(err);
      this.showSpinner = false
      this.showError = true;
    });
  }
  public setQuestionValidity(childValidity: boolean)
  {
    let validArray: Array<boolean> = []
    this.surveyForms.forEach(surveyForm => {
      validArray.push(surveyForm.isValid);
    });

    this.allQuestionsAnswered = validArray.every(v => v == true);
  }
  public isMassMailSurvey(survey: Survey) : boolean
  {
    return survey.surveyDescription == SurveyConstants.MASS_MAIL_SURVEY;
  }
}
