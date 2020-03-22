import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from 'src/app/services/SantaApiService.service';
import { MapService } from 'src/app/services/MapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-incoming-signups',
  templateUrl: './incoming-signups.component.html',
  styleUrls: ['./incoming-signups.component.css'],
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
export class IncomingSignupsComponent implements OnInit {

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

  awaitingClients: Array<Client> = [];
  showSpinner: boolean = true;
  actionTaken: boolean = false;
  

  ngOnInit() {
    this.SantaApi.getAllClients().subscribe(res => {
      res.forEach(client => {
        var c = this.mapper.mapClient(client);
        if(c.clientStatus.statusDescription == "Awaiting")
        {
          this.awaitingClients.push(c);
        }
      });
      this.showSpinner = false;
    });
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  refreshSignupClientList()
  {
    if(this.actionTaken)
    {
      this.SantaApi.getAllClients().subscribe(res => {
        this.awaitingClients = [];
        this.showSpinner = true;
        res.forEach(client => {
          var c = this.mapper.mapClient(client);
          if(c.clientStatus.statusDescription == "Awaiting")
          {
            this.awaitingClients.push(c);
          }
        });
        this.showSpinner = false;
        this.actionTaken = false;
      }); 
    }
     
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
}
