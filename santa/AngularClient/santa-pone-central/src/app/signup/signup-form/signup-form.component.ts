import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Client, ClientResponse } from 'src/classes/Client';
import { SantaApiGetService, SantaApiPostService } from 'src/app/services/SantaApiService.service';
import { EventType } from 'src/classes/EventType';
import { Status } from 'src/classes/Status';
import { MapService } from '../../services/MapService.service';
import { EventConstants } from 'src/shared/constants/EventConstants';
import { Guid } from "guid-typescript";


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaGet: SantaApiGetService, public SantaPost: SantaApiPostService, public mapper: MapService) { }

  private client: Client = new Client();
  private events: Array<EventType> = [];
  private statuses: Array<Status> = [];
  private constants: EventConstants = new EventConstants();

  ngOnInit() {
    //API Call for getting statuses
    this.SantaGet.getAllStatuses().subscribe(res => {
      res.forEach(status => {
        this.statuses.push(this.mapper.mapStatus(status))
      });
    });

    //API Call for getting events
    this.SantaGet.getAllEvents().subscribe(res => {
      res.forEach(eventType => {
        this.events.push(this.mapper.mapEvent(eventType))
      });
    });
  }
  public onSubmit(clientForm: NgForm)
  {
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
      clientForm.resetForm();
    });
  }
}
