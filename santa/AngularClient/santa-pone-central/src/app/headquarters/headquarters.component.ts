import { Component, OnInit, ViewChild} from '@angular/core';
import { Client } from '../../classes/client';
import { MapService } from '../services/mapService.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { StatusConstants } from '../shared/constants/StatusConstants.enum';
import { ApprovedAnonsComponent } from './approved-anons/approved-anons.component';
import { DeniedAnonsComponent } from './denied-anons/denied-anons.component';
import { CompletedAnonsComponent } from './completed-anons/completed-anons.component';
import { IncomingSignupsComponent } from './incoming-signups/incoming-signups.component';
import { SelectedAnonComponent } from './selected-anon/selected-anon.component';

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
  public showManualSignupCard: boolean = false;
  public currentClient: Client;
  public allClients: Array<Client> = [];

  public gatheringAllClients: boolean;
  public clickAwayLocked: boolean;

  @ViewChild(ApprovedAnonsComponent) approvedAnonsComponent: ApprovedAnonsComponent;
  @ViewChild(CompletedAnonsComponent) completedAnonsComponent: CompletedAnonsComponent;
  @ViewChild(DeniedAnonsComponent) deniedAnonsComponent: DeniedAnonsComponent;
  @ViewChild(IncomingSignupsComponent) incomingSignupsComponent: IncomingSignupsComponent;
  @ViewChild(SelectedAnonComponent) selectedAnonComponent: SelectedAnonComponent;

  get readyForRefresh() : boolean
  {
    return this.approvedAnonsComponent.actionTaken || this.completedAnonsComponent.actionTaken || this.deniedAnonsComponent.actionTaken || this.incomingSignupsComponent.actionTaken
  }

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
  public async showClientWindow(client: Client)
  {
    this.currentClient = client;
    this.showManualSignupCard = false;
    this.showClientCard = true;
    this.gatherer.onSelectedClient = true;
    // If the list of all clients does not have the input client in it, refresh the list in the background (Used namely for manual refresh)
    if(!this.allClients.some((c: Client) => {return c.clientID == client.clientID}))
    {
      await this.gatherer.gatherAllClients()
    }
  }
  showManualSignupWindow()
  {
    this.showManualSignupCard = true;
  }
  async hideOpenWindow(forceRefresh: boolean = false)
  {
    if(!this.clickAwayLocked)
    {
      if(this.showClientCard)
      {
        this.showClientCard = false;
        this.gatherer.onSelectedClient = false;
        if(forceRefresh)
        {
          await this.gatherer.gatherAllClients();
        }
      }
      else if(this.showManualSignupCard)
      {
        this.showManualSignupCard = false;
      }
      // If any of the viewchildren are set to refresh
      if(this.readyForRefresh)
      {
        await this.gatherer.gatherAllClients();
        this.setChildrenAction(false);
      }
    }
    else
    {
      console.log("Clickaway is locked!");
    }
  }
  public setChildrenAction(actionTaken: boolean)
  {
    this.approvedAnonsComponent.actionTaken = actionTaken;
    this.completedAnonsComponent.actionTaken = actionTaken;
    this.deniedAnonsComponent.actionTaken = actionTaken;
    this.incomingSignupsComponent.actionTaken = actionTaken;
  }
  public async updateSelectedClient(clientID: string)
  {
    this.currentClient = this.mapper.mapClient(await this.SantaApiGet.getClientByClientID(clientID).toPromise());
  }
  public setClickawayLock(status: boolean)
  {
    this.clickAwayLocked = status;
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
