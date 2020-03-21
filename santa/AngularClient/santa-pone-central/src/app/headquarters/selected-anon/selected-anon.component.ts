import { Component, OnInit, Input, Output } from '@angular/core';
import { Client } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService } from 'src/app/services/SantaApiService.service';
import { MapService, MapResponse } from 'src/app/services/MapService.service';
import { EventConstants } from 'src/app/shared/constants/EventConstants';
import { Status } from 'src/classes/status';
import { ClientStatusResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-selected-anon',
  templateUrl: './selected-anon.component.html',
  styleUrls: ['./selected-anon.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(600 )
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(600, style({opacity: 0})))
    ])
  ]
})

export class SelectedAnonComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public SantaApiPut: SantaApiPutService, public ApiMapper: MapService, public responseMapper: MapResponse) { }

  @Input() client: Client = new Client();
  public senders: Array<Client> = new Array<Client>();
  public recievers: Array<Client> = new Array<Client>();

  public showSpinner: boolean = false;
  public showSuccess: boolean = false;
  public showFail: boolean = false;

  ngOnInit() {
    this.client.senders.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.ApiMapper.mapClient(client); 
        this.senders.push(c);
      });
    });
    console.log(this.client.recipients);
    this.client.recipients.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.ApiMapper.mapClient(client);
        this.recievers.push(c);
      });
    });
  }
  public approveAnon()
  {
    this.showSpinner = true;
    var putClient: Client = this.client;
    var approvedStatus: Status = new Status;

    this.SantaApiGet.getAllStatuses().subscribe(res =>{
      res.forEach(status => {
        if (status.statusDescription == EventConstants.APPROVED)
        {
          approvedStatus = this.ApiMapper.mapStatus(status);
          putClient.clientStatus.statusID = approvedStatus.statusID;
          var clientStatusResponse: ClientStatusResponse = this.responseMapper.mapClientStatusResponse(putClient)
    
          this.SantaApiPut.putClientStatus(this.client.clientID, clientStatusResponse).subscribe(res => {
            this.showSpinner = false;
            this.showSuccess = true;
          },
          err => {
            console.log(err);
            this.showSpinner = false;
            this.showFail = true;
          });
        } 
      });
    });
  }
}
