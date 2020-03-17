import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Client } from 'src/classes/Client';
import { SantaApiService } from 'src/app/services/SantaApiService.service';
import { EventType } from 'src/classes/EventType';
import { Status } from 'src/classes/Status';
import { MapService } from '../../services/MapService.service';


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaApi: SantaApiService, public mapper: MapService) { }

  private client: Client = new Client();
  private events: Array<EventType> = [];
  private statuses: Array<Status> = [];

  ngOnInit() {
    this.SantaApi.getAllStatuses().subscribe(res => {
      res.forEach(status => {
        this.statuses.push(this.mapper.mapStatus(status))
      });
    });
  }
  public onSubmit(clientForm: NgForm)
  {
    this.client.clientName = clientForm.value.firstName + " " + clientForm.value.lastName;

    this.client.address.addressLineOne = clientForm.value.addressLineOne;
    this.client.address.addressLineTwo = clientForm.value.addressLineTwo;
    this.client.address.city = clientForm.value.city;
    this.client.address.state = clientForm.value.state;
    this.client.address.state = clientForm.value.state;

    this.client.clientStatus = 
    console.log(this.client);
  }
}
