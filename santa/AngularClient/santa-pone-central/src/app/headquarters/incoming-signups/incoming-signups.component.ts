import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Client } from '../../../classes/Client';
import { Address } from '../../../classes/Address';
import { SantaApiService } from 'src/app/services/SantaApiService.service';
import { MapService } from 'src/app/services/MapService.service';

@Component({
  selector: 'app-incoming-signups',
  templateUrl: './incoming-signups.component.html',
  styleUrls: ['./incoming-signups.component.css']
})
export class IncomingSignupsComponent implements OnInit {

  constructor(public SantaApi: SantaApiService, public mapper: MapService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
  awaitingClients: Array<Client> = [];
  showSpinner: boolean = true;

  async ngOnInit() {
    await this.SantaApi.getAllClients().subscribe(res => {
      res.forEach(client => {
        var c = this.mapper.mapClient(client);
        if(c.clientStatus.statusDescription == "Awaiting")
        {
          this.awaitingClients.push(c);
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
