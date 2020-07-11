import { Component, OnInit} from '@angular/core';
import { Client } from '../../classes/client';
import { MapService } from '../services/mapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { EventConstants } from '../shared/constants/eventConstants.enum';

@Component({
  selector: 'app-headquarters',
  templateUrl: './headquarters.component.html',
  styleUrls: ['./headquarters.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(200)
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(200, style({opacity: 0})))
    ])
  ]
})
export class HeadquartersComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public mapper: MapService, public gatherer: GathererService) {}


  public showClientCard: boolean = false;
  public currentClient: Client;
  public allClients: Array<Client> = [];
  

  ngOnInit() {
    this.gatherer.allClients.subscribe((clients: Array<Client>) => {
      this.allClients = clients;
    })
    this.gatherer.gatherAllClients();
  }

  showClientWindow(client)
  {
    this.currentClient = client;
    this.showClientCard = true;
    this.gatherer.onSelectedClient = true;
  }
  hideClientWindow()
  {
    this.showClientCard = false;
    this.gatherer.onSelectedClient = false;
  }
  async updateSelectedClient(clientID: string)
  {
    this.currentClient = this.mapper.mapClient(await this.SantaApiGet.getClient(clientID).toPromise());
  }
  sortApproved() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == EventConstants.APPROVED});
  }
  sortIncoming() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == EventConstants.AWAITING});
  }
  sortDenied() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == EventConstants.DENIED});
  }
}
