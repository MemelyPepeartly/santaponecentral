import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client } from 'src/classes/client';
import { GathererService } from 'src/app/services/gatherer.service';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-denied-anons',
  templateUrl: './denied-anons.component.html',
  styleUrls: ['./denied-anons.component.css']
})
export class DeniedAnonsComponent implements OnInit {

  constructor(public gatherer: GathererService) { }

  @Input() deniedClients: Array<Client> = [];
  @Input() gatheringAllClients: boolean;

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

  public pagedClients: Array<Client> = [];
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
    this.pagedClients = this.deniedClients.slice(event.pageIndex * event.pageSize, (event.pageIndex * event.pageSize) + event.pageSize)
    this.paginatorPageSize = event.pageSize;
    this.paginatorPageIndex = event.pageIndex;
  }
  resliceTable()
  {
    setTimeout(() => {
      this.pagedClients = this.deniedClients.slice(this.paginatorPageIndex * this.paginatorPageSize, (this.paginatorPageIndex * this.paginatorPageSize) + this.paginatorPageSize);
    });
  }
}
