import { Component, OnInit, Output, Input, ViewChild } from '@angular/core';
import { HQClient } from '../../../classes/client';
import { EventEmitter } from '@angular/core';
import { MapService } from '../../services/utility services/mapper.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { Survey } from 'src/classes/survey';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public mapper: MapService, private gatherer: GeneralDataGathererService) { }

  @Output() clickedClient: EventEmitter<HQClient> = new EventEmitter();

  @Input() approvedClients: Array<HQClient> = [];
  @Input() gatheringInfo: boolean;
  @Input() allSurveys: Array<Survey> = [];

  @Output() refreshClientObjectEvent: EventEmitter<HQClient> = new EventEmitter<HQClient>();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  public paginatorPageSize: number = 25;
  public paginatorPageIndex: number = 1;

  public actionTaken: boolean = false;
  public showSpinner: boolean = false;

  ngOnInit() {
  }
  showCardInfo(client: HQClient)
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
    await this.gatherer.gatherAllHQClients();
    this.showSpinner = false;
    this.actionTaken = false;
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
  emitClientUpdateEvent(client: HQClient)
  {
    this.refreshClientObjectEvent.emit(client);
  }
}
