import { Component, OnInit } from '@angular/core';
import { NgForm, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Client, ClientResponse } from '../../../classes/client';
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/SantaApiService.service';
import { EventType } from '../../../classes/EventType';
import { Status } from '../../../classes/status';
import { MapService } from '../../services/MapService.service';
import { EventConstants } from '../../shared/constants/EventConstants';
import { Guid } from "guid-typescript";


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService,
    public SantaPost: SantaApiPostService,
    public mapper: MapService,
    private formBuilder: FormBuilder) { }

  private client: Client = new Client();
  private events: Array<EventType> = [];
  private statuses: Array<Status> = [];
  public isLinear: boolean = true;

  public clientFormGroup: FormGroup;

  ngOnInit() {
    this.clientFormGroup = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],

      addressLine1: ['', Validators.required],
      addressLine2: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
    });

    //API Call for getting statuses
    this.SantaGet.getAllStatuses().subscribe(res => {
      res.forEach(status => {
        this.statuses.push(this.mapper.mapStatus(status))
      });
    });
    console.log("Statuses");
    console.log(this.statuses);

    //API Call for getting events
    this.SantaGet.getAllEvents().subscribe(res => {
      res.forEach(eventType => {
        this.events.push(this.mapper.mapEvent(eventType))
      });
    });
    console.log("Events");
    console.log(this.events);
  }
  public onSubmit()
  {
    console.log(this.clientFormGroup);
    let newClient: ClientResponse = new ClientResponse();
    newClient.clientName = this.clientFormGroup.value.firstName + " " + this.clientFormGroup.value.lastName;
    newClient.clientEmail = this.clientFormGroup.value.email;

    newClient.clientAddressLine1 = this.clientFormGroup.value.addressLine1;
    newClient.clientAddressLine2 = this.clientFormGroup.value.addressLine2;
    newClient.clientCity = this.clientFormGroup.value.city;
    newClient.clientState = this.clientFormGroup.value.state;
    newClient.clientPostalCode = this.clientFormGroup.value.postalCode;
    newClient.clientCountry = this.clientFormGroup.value.country;

    var awaitingStatusID = this.statuses.find(status => status.statusDescription == "Awaiting");
    newClient.clientStatusID = awaitingStatusID.statusID

    this.SantaPost.postClient(newClient).subscribe(createRes => {
      console.log(createRes);
    });
  }
}
