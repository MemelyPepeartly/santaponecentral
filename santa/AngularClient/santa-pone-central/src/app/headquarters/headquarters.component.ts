import { Component, OnInit} from '@angular/core';
import { Client } from '../../classes/client';
import { MapService } from '../services/mapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { StatusConstants } from '../shared/constants/statusConstants.enum';

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

  public gatheringAllClients: boolean;

  async ngOnInit() {
    /* GET STATUS BOOLEAN SUBSCRIBE */
    this.gatherer.gatheringAllClients.subscribe((status: boolean) => {
      this.gatheringAllClients = status;
    });
    /* ALL CLIENTS SUBSCRIBE */
    this.gatherer.allClients.subscribe((clients: Array<Client>) => {
      this.allClients = clients;
    });
    await this.gatherer.gatherAllClients();
  }

  showClientWindow(client)
  {
    this.currentClient = client;
    this.showClientCard = true;
    this.gatherer.onSelectedClient = true;
  }
  async hideClientWindow(forceRefresh: boolean = false)
  {
    this.showClientCard = false;
    this.gatherer.onSelectedClient = false;
    if(forceRefresh)
    {
      await this.gatherer.gatherAllClients();
    }
  }
  async updateSelectedClient(clientID: string)
  {
    this.currentClient = this.mapper.mapClient(await this.SantaApiGet.getClientByClientID(clientID).toPromise());
  }
  sortApproved() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.APPROVED});
  }
  sortIncoming() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.AWAITING});
  }
  sortDenied() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.DENIED});
  }
  sortCompleted() : Array<Client>
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.COMPLETED});
  }
}
