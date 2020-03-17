import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Client } from 'src/classes/Client';
import { SantaApiService } from 'src/app/services/SantaApiService.service';


@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(public SantaApi: SantaApiService) { }

  private client: Client = new Client();
  private events: EventType = new EventType();

  ngOnInit() {
  }
  public onSubmit(clientForm: NgForm)
  {
    this.client.clientName = clientForm.value.firstName + " " + clientForm.value.lastName;

    this.client.address.addressLineOne = clientForm.value.addressLineOne;
    this.client.address.addressLineTwo = clientForm.value.addressLineTwo;
    this.client.address.city = clientForm.value.city;
    this.client.address.state = clientForm.value.state;
    this.client.address.state = clientForm.value.state;

    this.client.clientStatus = this.SantaApi.
    console.log(this.client);
  }
}
