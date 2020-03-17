import { Component, OnInit } from '@angular/core';
import { NgForm, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Client, ClientResponse } from 'src/classes/Client';
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/SantaApiService.service';
import { EventType } from 'src/classes/EventType';
import { Status } from 'src/classes/Status';
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
  private constants: EventConstants = new EventConstants();

  public clientFormGroup: FormGroup;

  ngOnInit() {
    this.clientFormGroup = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],

      addressLineOne: ['', Validators.required],
      addressLineTwo: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
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
    newClient.email = this.clientFormGroup.value.email;

    newClient.address.addressLineOne = this.clientFormGroup.value.addressLineOne;
    newClient.address.addressLineTwo = this.clientFormGroup.value.addressLineTwo;
    newClient.address.city = this.clientFormGroup.value.city;
    newClient.address.state = this.clientFormGroup.value.state;
    newClient.address.postalCode = this.clientFormGroup.value.postalCode;
    newClient.address.country = this.clientFormGroup.value.country;

    var awaitingStatusID = this.statuses.find(status => status.statusDescription == "Awaiting");
    newClient.clientStatusID = awaitingStatusID.statusID
    
    var newClientID = Guid.create().toString()

    
    /*
    this.SantaPost.postClient(newClientID, newClient).subscribe(createRes => {
      console.log(createRes);
    });
    */
  }
}
