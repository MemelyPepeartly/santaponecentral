import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Client } from 'src/classes/client';
import { GathererService } from 'src/app/services/gatherer.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-denied-anons',
  templateUrl: './denied-anons.component.html',
  styleUrls: ['./denied-anons.component.css']
})
export class DeniedAnonsComponent implements OnInit {

  constructor(public gatherer: GathererService) { }

  @Input() deniedClients: Array<Client> = [];
  @Input() gatheringInfo: boolean;

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

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
    await this.gatherer.gatherAllClients();
    this.showSpinner = false;
    this.actionTaken = false;
  }
  switchPage(event: PageEvent)
  {
    this.paginatorPageSize = event.pageSize;
    this.paginatorPageIndex = event.pageIndex;
  }
  pagedClients() : Array<Client>
  {
    if(this.paginator != undefined)
    {
      return this.deniedClients.slice(this.paginator.pageIndex * this.paginator.pageSize, (this.paginator.pageIndex * this.paginator.pageSize) + this.paginator.pageSize);
    }
    else
    {
      return this.deniedClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    }
  }
}
