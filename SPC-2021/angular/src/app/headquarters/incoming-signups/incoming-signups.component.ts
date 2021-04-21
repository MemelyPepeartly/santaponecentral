import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { HQClient } from '../../../classes/client';
import { SantaApiGetService } from 'src/app/services/santa-api.service';
import { MapService } from 'src/app/services/mapper.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { GathererService } from 'src/app/services/gatherer.service';
import { Survey} from 'src/classes/survey';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

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

  @Output() clickedClient: EventEmitter<HQClient> = new EventEmitter();
  @Output() manualSignUpClickedEvent: EventEmitter<any> = new EventEmitter();

  @Input() awaitingClients: Array<HQClient> = [];
  @Input() gatheringInfo: boolean;
  @Input() allSurveys: Array<Survey> = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  public paginatorPageSize: number = 25;
  public paginatorPageIndex: number = 1;

  showSpinner: boolean = false;
  actionTaken: boolean = false;


  ngOnInit() {
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  emitOpenManualSignupWindow()
  {
    this.manualSignUpClickedEvent.emit(true);
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  async manualRefresh()
  {
    this.showSpinner = true;
    await this.gatherer.gatherAllHQClients();
    this.showSpinner = false;
    this.actionTaken = false;
  }
  answeredForSurvey(client: HQClient, survey: Survey) : boolean
  {
    return client.answeredSurveys.some((surveyID: string) => {
      return surveyID == survey.surveyID;
    });
  }
  switchPage(event: PageEvent)
  {
    this.paginatorPageSize = event.pageSize;
    this.paginatorPageIndex = event.pageIndex;
  }
  pagedClients() : Array<HQClient>
  {
    if(this.paginator != undefined)
    {
      return this.awaitingClients.slice(this.paginator.pageIndex * this.paginator.pageSize, (this.paginator.pageIndex * this.paginator.pageSize) + this.paginator.pageSize);
    }
    else
    {
      return this.awaitingClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    }
  }
}
