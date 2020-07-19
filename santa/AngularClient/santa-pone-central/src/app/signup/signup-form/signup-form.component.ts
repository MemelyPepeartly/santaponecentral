import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { NgForm, FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { Client } from '../../../classes/client';
import { ClientResponse, SurveyApiResponse, ClientSignupResponse } from '../../../classes/responseTypes'
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/santaApiService.service';
import { EventType } from '../../../classes/eventType';
import { Status } from '../../../classes/status';
import { MapService, MapResponse } from '../../services/mapService.service';
import { EventConstants } from '../../shared/constants/eventConstants.enum';
import { Survey, Question, SurveyQA } from 'src/classes/survey';
import { SurveyFormComponent } from '../survey-form/survey-form.component';
import { CountriesService } from 'src/app/services/countries.service';
import { Address } from 'src/classes/address';
import { GathererService } from 'src/app/services/gatherer.service';


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService,
    public SantaPost: SantaApiPostService,
    public gatherer: GathererService,
    public mapper: MapService,
    public responseMapper: MapResponse,
    public countryService: CountriesService,
    private formBuilder: FormBuilder) { }

  public events: Array<EventType> = [];
  public selectedEvents: Array<EventType> = new Array<EventType>();

  public statuses: Array<Status> = [];
  public surveys: Array<Survey> = [];
  public countries: Array<any>=[];

  //Shows and hides the spinner
  public showSpinner: boolean = false;
  //Shows and hides the finished dialogue when the form is properly filled out
  public showFinished: boolean = false;
  //Shows the dialogue where something is wrong in the form to be edited again
  public showSomethingWrong: boolean = false;
  //Shows a loading spinner until the form is done loading to be filled out
  public isDoneLoading: boolean = false;
  //For setting the form to a linear format
  public isLinear: boolean = true;
  //For determining of all questions on the surveys selected are answered
  public allQuestionsAnswered: boolean = false;

  public clientInfoFormGroup: FormGroup;
  public clientAddressFormGroup: FormGroup;
  public clientEventFormGroup: FormGroup;
  public surveyFormGroup: FormGroup

  @ViewChildren(SurveyFormComponent) surveyForms: QueryList<SurveyFormComponent>;

  async ngOnInit() {
    this.isLinear = true;
    this.isDoneLoading = false;

    // Creates form groups
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
    
    this.isDoneLoading = true;
  }
  get clientName() 
  {
    var formControlFirst = this.clientInfoFormGroup.get('firstName') as FormControl
    var formControlLast = this.clientInfoFormGroup.get('lastName') as FormControl
    let clientName: string = formControlFirst.value + " " + formControlLast.value
    return clientName;
  }
  get clientEmail()
  {
    var formControlEmail = this.clientInfoFormGroup.get('email') as FormControl
    return formControlEmail.value;
  }
  get clientAddress() 
  {
    let address: Address = new Address();
    var formControlLine1 = this.clientAddressFormGroup.get('addressLine1') as FormControl;
    var formControlLine2 = this.clientAddressFormGroup.get('addressLine2') as FormControl;
    var formControlCity = this.clientAddressFormGroup.get('city') as FormControl;
    var formControlState = this.clientAddressFormGroup.get('state') as FormControl;
    var formControlPostal = this.clientAddressFormGroup.get('postalCode') as FormControl;
    var formControlCountry = this.clientAddressFormGroup.get('country') as FormControl;

    address.addressLineOne = formControlLine1.value;
    address.addressLineTwo = formControlLine2.value;
    address.city = formControlCity.value;
    address.state = formControlState.value;
    address.postalCode = formControlPostal.value;
    address.country = formControlCountry.value;
    return address;
  }
  get addressFormControls()
  {
    return this.clientAddressFormGroup.controls;
  }
  get nameFormControls()
  {
    return this.clientInfoFormGroup.controls;
  }
  public async onSubmit()
  {
    this.showSpinner = true;

    // Construction of new client response
    let newClient: ClientSignupResponse = new ClientSignupResponse();
    newClient.clientName = this.clientName;
    newClient.clientEmail = this.clientEmail;
    newClient.clientNickname = "Anon"

    newClient.clientAddressLine1 = this.clientAddress.addressLineOne;
    newClient.clientAddressLine2 = this.clientAddress.addressLineTwo;
    newClient.clientCity = this.clientAddress.city;
    newClient.clientState = this.clientAddress.state
    newClient.clientPostalCode = this.clientAddress.postalCode;
    newClient.clientCountry = this.clientAddress.country;

    var awaitingStatusID = this.statuses.find(status => status.statusDescription == EventConstants.AWAITING);
    newClient.clientStatusID = awaitingStatusID.statusID

    var thing = this.surveyForms.toArray()
    // Set client's answers
    thing.forEach((surveyForm: SurveyFormComponent) => {
      let response = new SurveyApiResponse;
      for (const field in surveyForm.surveyFormGroup.controls) // 'field' is a string equal to question ID
      {
        var control = surveyForm.surveyFormGroup.get(field); // 'control' is a FormControl

        response.surveyQuestionID = field;
        response.surveyID = surveyForm.surveyID;

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
    console.log(newClient);
    
    // Post client with answers
    await this.SantaPost.postClientSignup(newClient).toPromise();

    this.clientInfoFormGroup.reset();
    this.clientAddressFormGroup.reset();

    this.showSomethingWrong = false;
    this.showSpinner = false;
    this.showFinished = true;
  }
  public resetSubmitBools()
  {
    this.showFinished = false;
    this.showSpinner = false;
    this.clientInfoFormGroup.reset();
    this.clientAddressFormGroup.reset();
  }
  public createFormGroups()
  {
    this.clientInfoFormGroup = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(20), Validators.pattern("[A-Za-z]{1,20}")]],
      lastName: ['', [Validators.required, Validators.maxLength(20), Validators.pattern("[A-Za-z]{1,20}")]],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
    });
    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      addressLine2: ['', [Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      state: ['', [Validators.required, Validators.pattern("[A-Za-z0-9 ]{1,50}"), Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.pattern("[0-9]{1,25}"), Validators.maxLength(25)]],
      country: ['', Validators.required]
    });
    this.clientEventFormGroup = this.formBuilder.group({
      eventDescription: ['', Validators.required]
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
}
