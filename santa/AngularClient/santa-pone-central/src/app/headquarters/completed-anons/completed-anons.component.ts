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
  public async refreshCompletedClientList()
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
    this.refreshCompletedClientList();
  }

}
