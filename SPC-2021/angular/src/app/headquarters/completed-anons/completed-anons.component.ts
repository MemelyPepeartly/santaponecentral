import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { HQClient } from 'src/classes/client';

@Component({
  selector: 'app-completed-anons',
  templateUrl: './completed-anons.component.html',
  styleUrls: ['./completed-anons.component.css']
})
export class CompletedAnonsComponent implements OnInit {

  constructor(public gatherer: GeneralDataGathererService) { }

  @Input() completedClients: Array<HQClient> = [];
  @Input() gatheringInfo: boolean;

  @Output() clickedClient: EventEmitter<HQClient> = new EventEmitter();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  public paginatorPageSize: number = 10;
  public paginatorPageIndex: number = 1;

  actionTaken: boolean = false;
  showSpinner: boolean = false;

  ngOnInit(): void {
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
      return this.completedClients.slice(this.paginator.pageIndex * this.paginator.pageSize, (this.paginator.pageIndex * this.paginator.pageSize) + this.paginator.pageSize);
    }
    else
    {
      return this.completedClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    }
  }
}
