import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from 'src/app/services/santaApiService.service';
import { MapService } from 'src/app/services/mapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';
import { GathererService } from 'src/app/services/gatherer.service';

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

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService, public gatherer: GathererService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

  @Input() incomingClients: Array<Client> = [];
  showSpinner: boolean = false;
  actionTaken: boolean = false;
  

  ngOnInit() {
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  refreshSignupClientList()
  {
    if(this.actionTaken)
    {
      this.gatherer.gatherAllClients();
      this.actionTaken = false;
      this.showSpinner = false;
    }
     
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  manualRefresh()
  {
    this.actionTaken = true;
    this.showSpinner = true;
    this.refreshSignupClientList();
  }
}
