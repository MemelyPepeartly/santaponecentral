import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GathererService } from 'src/app/services/gatherer.service';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-completed-anons',
  templateUrl: './completed-anons.component.html',
  styleUrls: ['./completed-anons.component.css']
})
export class CompletedAnonsComponent implements OnInit {

  constructor(public gatherer: GathererService) { }

  @Input() completedClients: Array<Client> = [];
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
