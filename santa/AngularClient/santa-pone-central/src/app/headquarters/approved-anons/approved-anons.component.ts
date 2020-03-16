import { Component, OnInit, Output } from '@angular/core';
import { Client } from '../../../interfaces/client';
import { Address } from '../../../interfaces/address';
import { SantaApiService } from '../../services/SantaApiService.service';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();
  approvedClients: any = [];
  showSpinner: boolean = true;

  async ngOnInit() {
    
    await this.SantaApi.getAllClients().subscribe(res => {
      this.approvedClients = res;
      this.showSpinner = false;
    });
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
}
