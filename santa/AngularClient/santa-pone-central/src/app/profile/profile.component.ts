import { Component, OnInit } from '@angular/core';
import { Client } from 'src/classes/client';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public gatherer: GathererService) { }

  public recipients: Array<Client> = []

  ngOnInit() {
    
    this.gatherer.allClients.subscribe((clientArray: Array<Client>) => {
      this.recipients = clientArray;
    });
    this.gatherer.gatherAllClients();
  }

}
