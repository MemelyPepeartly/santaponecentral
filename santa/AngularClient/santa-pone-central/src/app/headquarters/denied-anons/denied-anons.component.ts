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
  @Input() gatheringAllClients: boolean;

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
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

}
