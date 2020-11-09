import { Component, OnInit, Output, Input, ViewChild } from '@angular/core';
import { Client, HQClient } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from '../../services/santa-api.service';
import { EventEmitter } from '@angular/core';
import { MapService } from '../../services/mapper.service';
import { StatusConstants } from 'src/app/shared/constants/StatusConstants.enum';
import { GathererService } from 'src/app/services/gatherer.service';
import { Survey, SurveyResponse } from 'src/classes/survey';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService, public gatherer: GathererService) { }

  @Output() clickedClient: EventEmitter<HQClient> = new EventEmitter();

  @Input() approvedClients: Array<HQClient> = [];
  @Input() gatheringInfo: boolean;
  @Input() allSurveys: Array<Survey> = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  public paginatorPageSize: number = 25;
  public paginatorPageIndex: number = 1;

  public actionTaken: boolean = false;
  public showSpinner: boolean = false;
  public showAssignmentQuickInfoCard: boolean = false;
  public showNoteQuickInfoCard: boolean = false;
  public clickawayLocked: boolean = false;

  public selectedAssignmentClient: Client = new Client();
  public selectedNoteClient: Client = new Client();

  ngOnInit() {
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  async manualRefresh()
  {
    this.showSpinner = true;
    await this.gatherer.gatherAllTruncatedClients();
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
      return this.approvedClients.slice(this.paginator.pageIndex * this.paginator.pageSize, (this.paginator.pageIndex * this.paginator.pageSize) + this.paginator.pageSize);
    }
    else
    {
      return this.approvedClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    }
  }
  hideOpenWindow()
  {
    if(!this.clickawayLocked)
    {
      this.showAssignmentQuickInfoCard = false;
      this.showNoteQuickInfoCard = false;
      this.selectedAssignmentClient = new Client();
      this.selectedNoteClient = new Client();
    }
  }
  viewClientNotes(client: Client)
  {
    this.selectedNoteClient = client;
    this.showNoteQuickInfoCard = true;
  }
  viewAssignments(client: Client)
  {
    this.selectedAssignmentClient = client;
    this.showAssignmentQuickInfoCard = true;
  }
  async softRefreshClient()
  {
    let refreshedClient: Client = this.mapper.mapClient(await this.SantaApi.getClientByClientID(this.selectedNoteClient.clientID != undefined ? this.selectedNoteClient.clientID : this.selectedAssignmentClient.clientID).toPromise())
    if(this.selectedNoteClient.clientID != undefined)
    {
      this.selectedNoteClient = refreshedClient;
    }
    else if(this.selectedAssignmentClient.clientID != undefined)
    {
      this.selectedAssignmentClient = refreshedClient;
    }
  }
  setClickawayLock(clickawayLocked: boolean)
  {
    this.clickawayLocked = clickawayLocked;
  }
}
