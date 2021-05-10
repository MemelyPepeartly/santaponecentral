import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ClientService } from 'src/app/services/api services/client.service';
import { MapService } from 'src/app/services/utility services/mapper.service';
import { HQClient } from 'src/classes/client';

@Component({
  selector: 'app-client-note-info',
  templateUrl: './client-note-info.component.html',
  styleUrls: ['./client-note-info.component.css']
})
export class ClientNoteInfoComponent implements OnInit {

  constructor(public ClientService: ClientService, public mapper: MapService) { }

  @Input() client: HQClient = new HQClient();

  @Output() refreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() clickawayLockedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public gatheringInfoContainer: boolean = false;

  async ngOnInit() {
    this.gatheringInfoContainer = true;
    this.client.infoContainer = this.mapper.mapInfoContainer(await this.ClientService.getInfoContainerByClientID(this.client.clientID).toPromise());
    this.gatheringInfoContainer = false;
  }
  public emitRefreshAction()
  {
    this.refreshEvent.emit(true);
  }
  public emitLockEvent(clickawayLocked: boolean)
  {
    this.clickawayLockedEvent.emit(clickawayLocked);
  }
}
