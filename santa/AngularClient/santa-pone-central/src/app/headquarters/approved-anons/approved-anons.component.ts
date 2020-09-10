import { Component, OnInit, Output, Input } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from '../../services/santaApiService.service';
import { EventEmitter } from '@angular/core';
import { MapService } from '../../services/mapService.service';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';
import { GathererService } from 'src/app/services/gatherer.service';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService, public gatherer: GathererService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

  @Input() approvedClients: Array<Client> = [];
  @Input() gatheringAllClients: boolean;

  actionTaken: boolean = false;
  showSpinner: boolean = false;


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
    await this.gatherer.gatherAllClients();
    this.showSpinner = false;
    this.actionTaken = false;
  }
}
