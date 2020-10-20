import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from 'src/app/services/santa-api.service';
import { MapService } from 'src/app/services/mapper.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { StatusConstants } from 'src/app/shared/constants/StatusConstants.enum';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { Survey, SurveyResponse } from 'src/classes/survey';
import { PageEvent } from '@angular/material/paginator';

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
  @Output() manualSignUpClickedEvent: EventEmitter<any> = new EventEmitter();

  @Input() incomingClients: Array<Client> = [];
  @Input() gatheringInfo: boolean;
  @Input() allSurveys: Array<Survey> = [];

  public pagedClients: Array<Client> = [];
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
    await this.gatherer.gatherAllClients();
    this.showSpinner = false;
    this.actionTaken = false;
  }
  answeredForSurvey(client: Client, survey: Survey) : boolean
  {
    return client.responses.some((response: SurveyResponse) => {
      return response.surveyID == survey.surveyID;
    });
  }
  switchPage(event: PageEvent)
  {
    this.pagedClients = this.incomingClients.slice(event.pageIndex * event.pageSize, (event.pageIndex * event.pageSize) + event.pageSize)
    this.paginatorPageSize = event.pageSize;
    this.paginatorPageIndex = event.pageIndex;
  }
  resliceTable()
  {
    setTimeout(() => {
      this.pagedClients = this.incomingClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    });
  }
}
