import { Component, OnInit, Output } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiService } from '../../services/SantaApiService.service';
import { EventEmitter } from '@angular/core';
import { MapEventService } from '../../services/map-event.service';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiService, public mapper: MapEventService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
  approvedClients: Array<Client> = [];
  showSpinner: boolean = true;

  async ngOnInit() {
    
    await this.SantaApi.getAllClients().subscribe(res => {
      res.forEach(client => {
        var c = this.mapper.mapClient(client);
        if(c.clientStatus.statusDescription == "Approved")
        {
          this.approvedClients.push(c);
        }
      });
      this.showSpinner = false;
    });
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
}
