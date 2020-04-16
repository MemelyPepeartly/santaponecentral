import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { NgForm, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Client } from '../../../classes/client';
import { ClientResponse, SurveyApiResponse } from '../../../classes/responseTypes'
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/SantaApiService.service';
import { EventType } from '../../../classes/EventType';
import { Status } from '../../../classes/status';
import { MapService, MapResponse } from '../../services/MapService.service';
import { EventConstants } from '../../shared/constants/EventConstants';
import { Guid } from "guid-typescript";
import { Survey, Question, SurveyFormQuestion } from 'src/classes/survey';
import { SurveyFormComponent } from '../survey-form/survey-form.component';


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService,
    public SantaPost: SantaApiPostService,
    public objectMapper: MapService,
    public responseMapper: MapResponse,
    private formBuilder: FormBuilder) { }

  public events: Array<EventType> = [];
  public selectedEvents: Array<EventType> = new Array<EventType>();

  public statuses: Array<Status> = [];
  public surveys: Array<Survey> = [];

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

  public clientInfoFormGroup: FormGroup;
  public clientAddressFormGroup: FormGroup;
  public clientEventFormGroup: FormGroup;
  public surveyFormGroup: FormGroup

  @ViewChildren(SurveyFormComponent) surveyForms: QueryList<SurveyFormComponent>;

  async ngOnInit() {
    this.isLinear = true;
    this.clientInfoFormGroup = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required]
    });
    this.clientAddressFormGroup = this.formBuilder.group({
      addressLine1: ['', Validators.required],
      addressLine2: ['', Validators.nullValidator],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required]
    });
    this.clientEventFormGroup = this.formBuilder.group({
      eventDescription: ['', Validators.required]
    });

    this.isDoneLoading = false;

    //API Call for getting statuses
    var statusApiResponse = await this.SantaGet.getAllStatuses().toPromise();
    for(let i =0; i<statusApiResponse.length; i++)
    {
      this.statuses.push(this.objectMapper.mapStatus(statusApiResponse[i]));
    }

    //API Call for getting events
    var eventApiResponse = await this.SantaGet.getAllEvents().toPromise();
    for(let i =0; i<eventApiResponse.length; i++)
    {
      if(eventApiResponse[i].active == true)
        {
          this.events.push(this.objectMapper.mapEvent(eventApiResponse[i]))
        }
    }

    //API Call for getting surveys
    var surveyApiResponse = await this.SantaGet.getAllSurveys().toPromise().catch(err => {console.log(err)});

    for(let i =0; i<surveyApiResponse.length; i++)
    {
      var mappedsurvey = this.objectMapper.mapSurvey(surveyApiResponse[i]);
      this.surveys.push(mappedsurvey);
    }
    
    this.isDoneLoading = true;
  }
  public onSubmit()
  {
    this.showSpinner = true;
    let newClient: ClientResponse = new ClientResponse();
    newClient.clientName = this.clientInfoFormGroup.value.firstName + " " + this.clientInfoFormGroup.value.lastName;
    newClient.clientEmail = this.clientInfoFormGroup.value.email;
    newClient.clientNickname = "Anon"

    newClient.clientAddressLine1 = this.clientAddressFormGroup.value.addressLine1;
    newClient.clientAddressLine2 = this.clientAddressFormGroup.value.addressLine2;
    newClient.clientCity = this.clientAddressFormGroup.value.city;
    newClient.clientState = this.clientAddressFormGroup.value.state;
    newClient.clientPostalCode = this.clientAddressFormGroup.value.postalCode;
    newClient.clientCountry = this.clientAddressFormGroup.value.country;

    var awaitingStatusID = this.statuses.find(status => status.statusDescription == "Awaiting");
    newClient.clientStatusID = awaitingStatusID.statusID

    this.SantaPost.postClient(newClient).subscribe(
      clientRes => {
        let newClient = this.objectMapper.mapClient(clientRes);

        // Posts client responses to surveys if the client response has a new client
        this.surveyForms.forEach(surveyForm => {
          console.log("---Posting response for client--");
          for(let i =0; i<surveyForm.formQuestionsFormatted.length; i++)
          {
            let surveyApiResponse: SurveyApiResponse = this.responseMapper.mapSurveyApiResponse(surveyForm.formQuestionsFormatted[i]);
            surveyApiResponse.clientID = newClient.clientID

            console.log(surveyApiResponse);
            
          
            this.SantaPost.postSurveyResponse(surveyApiResponse).toPromise().catch(err => {console.log(err)});
          }
        });
          
        this.showSomethingWrong = false;
        this.showSpinner = false;
        this.showFinished = true;
        this.clientInfoFormGroup.reset();
        this.clientAddressFormGroup.reset();
    },
    err => {
      this.showSomethingWrong = true;
      this.showSpinner = false;
    }
    )};
  public resetSubmitBools()
  {
    this.showFinished = false;
    this.showSpinner = false;
    this.clientInfoFormGroup.reset();
    this.clientAddressFormGroup.reset();
  }
  public displayAnswersSelected()
  {
    this.surveyForms.forEach(thing => {
      console.log("---------------ANSWERS----------------")
      console.log(thing.formQuestionsFormatted);
    });
  }
}
