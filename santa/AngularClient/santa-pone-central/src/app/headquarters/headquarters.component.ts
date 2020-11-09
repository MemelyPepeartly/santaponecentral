import { AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import { Client, HQClient } from '../../classes/client';
import { MapService } from '../services/mapper.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService } from '../services/santa-api.service';
import { GathererService } from '../services/gatherer.service';
import { StatusConstants } from '../shared/constants/StatusConstants.enum';
import { ApprovedAnonsComponent } from './approved-anons/approved-anons.component';
import { DeniedAnonsComponent } from './denied-anons/denied-anons.component';
import { CompletedAnonsComponent } from './completed-anons/completed-anons.component';
import { IncomingSignupsComponent } from './incoming-signups/incoming-signups.component';
import { SelectedAnonComponent } from './selected-anon/selected-anon.component';
import { EventType } from 'src/classes/eventType';
import { Survey } from 'src/classes/survey';
import { PageEvent } from '@angular/material/paginator';

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

  constructor(
    public SantaApiGet: SantaApiGetService,
    public mapper: MapService,
    public gatherer: GathererService,
    public ApiMapper: MapService) {}


  public showClientCard: boolean = false;
  public showManualSignupCard: boolean = false;
  public currentClientID: string;

  public allClients: Array<HQClient> = [];
  public allSurveys: Array<Survey> = [];

  public gatheringAllClients: boolean;
  public gatheringAllSurveys: boolean;
  public clickAwayLocked: boolean;

  public initializing: boolean = true;

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
    this.initializing = true;
    /* GET STATUS BOOLEAN SUBSCRIBE */
    this.gatherer.gatheringAllHQClients.subscribe((status: boolean) => {
      this.gatheringAllClients = status;
    });
    this.gatherer.gatheringAllSurveys.subscribe((status: boolean) => {
      this.gatheringAllSurveys = status;
    });

    /* ALL CLIENTS SUBSCRIBE */
    this.gatherer.allHQClients.subscribe((clients: Array<HQClient>) => {
      this.allClients = clients;
    });

    await this.getSurveys();
    await this.gatherer.gatherAllHQClients();
    this.initializing = false;
  }
  public async showClientWindow(client: HQClient)
  {
    this.currentClientID = client.clientID;
    this.showManualSignupCard = false;
    this.showClientCard = true;
    this.gatherer.onSelectedClient = true;
    // If the list of all clients does not have the input client in it, refresh the list in the background (Used namely for manual refresh)
    if(!this.allClients.some((c: HQClient) => {return c.clientID == client.clientID}))
    {
      await this.gatherer.gatherAllHQClients();
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
          await this.gatherer.gatherAllTruncatedClients();
        }
      }
      else if(this.showManualSignupCard)
      {
        this.showManualSignupCard = false;
      }
      // If any of the viewchildren are set to refresh
      if(this.readyForRefresh)
      {
        await this.gatherer.gatherAllTruncatedClients();
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
  public setClickawayLock(status: boolean)
  {
    this.clickAwayLocked = status;
  }
  public async getSurveys()
  {
    this.gatheringAllSurveys = true;

    var res = await this.SantaApiGet.getAllSurveys().toPromise().catch(err => {
      console.group()
      console.log("Something went wrong gathering getting all the surveys in headquarters");
      console.log(err);
      console.groupEnd();
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allSurveys.push(this.ApiMapper.mapSurvey(res[i]));
    }

    this.gatheringAllSurveys = false;
  }
  sortApproved()
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.APPROVED});
  }
  sortIncoming()
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.AWAITING});
  }
  sortDenied()
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.DENIED});
  }
  sortCompleted()
  {
    return this.allClients.filter((client) => { return client.clientStatus.statusDescription == StatusConstants.COMPLETED});
  }
}
