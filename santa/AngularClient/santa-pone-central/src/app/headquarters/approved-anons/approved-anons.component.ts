import { Component, OnInit, Output } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from '../../services/SantaApiService.service';
import { EventEmitter } from '@angular/core';
import { MapService } from '../../services/MapService.service';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
  approvedClients: Array<Client> = [];
  showSpinner: boolean = true;
  actionTaken: boolean = false;

  ngOnInit() {
    
    this.SantaApi.getAllClients().subscribe(res => {
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
  refreshApprovedClientList()
  {
    if(this.actionTaken)
    {
      this.SantaApi.getAllClients().subscribe(res => {
        this.approvedClients = [];
        this.showSpinner = true;
        res.forEach(client => {
          var c = this.mapper.mapClient(client);
          if(c.clientStatus.statusDescription == "Approved")
          {
            this.approvedClients.push(c);
          }
        });
        this.showSpinner = false;
        this.actionTaken = false;
      });
    }
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  manualRefresh()
  {
    this.actionTaken = true;
    this.refreshApprovedClientList();
  }
}
