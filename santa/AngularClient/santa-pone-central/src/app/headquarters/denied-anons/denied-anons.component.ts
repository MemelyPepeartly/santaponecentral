import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client } from 'src/classes/client';
import { GathererService } from 'src/app/services/gatherer.service';

@Component({
  selector: 'app-denied-anons',
  templateUrl: './denied-anons.component.html',
  styleUrls: ['./denied-anons.component.css']
})
export class DeniedAnonsComponent implements OnInit {

  constructor(public gatherer: GathererService) { }

  @Input() deniedClients: Array<Client> = [];

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
  actionTaken: boolean = false;
  showSpinner: boolean = false;

  ngOnInit(): void {
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  public async refreshIncomingClientList()
  {
    if(this.actionTaken)
    {
      await this.gatherer.gatherAllClients();
      this.actionTaken = false;
      this.showSpinner = false;
    }
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  manualRefresh()
  {
    this.actionTaken = true;
    this.showSpinner = true;
    this.refreshIncomingClientList();
  }

}
