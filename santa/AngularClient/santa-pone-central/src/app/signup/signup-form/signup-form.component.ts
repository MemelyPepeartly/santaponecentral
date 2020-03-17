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

  public firstFormGroup: FormGroup;
  public secondFormGroup: FormGroup;

  ngOnInit() {
    this.firstFormGroup = this.formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    
    this.secondFormGroup = this.formBuilder.group({
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
  public onSubmit(clientForm: NgForm)
  {
    console.log(this.firstFormGroup);
    console.log(this.secondFormGroup);
    console.log(clientForm);
    let newClient: ClientResponse = new ClientResponse();
    newClient.clientName = clientForm.value.firstName + " " + clientForm.value.lastName;
    newClient.email = clientForm.value.email;

    newClient.address.addressLineOne = clientForm.value.addressLineOne;
    newClient.address.addressLineTwo = clientForm.value.addressLineTwo;
    newClient.address.city = clientForm.value.city;
    newClient.address.state = clientForm.value.state;
    newClient.address.state = clientForm.value.state;

    newClient.clientStatusID = this.statuses.find(status => status.statusDescription == this.constants.AWAITING).statusID;
    
    this.SantaPost.postClient(Guid.create().toString(), newClient).subscribe(createRes => {
      console.log(createRes);
      clientForm.resetForm();
    });
  }
}
