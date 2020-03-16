import { Component, OnInit } from '@angular/core';
import { Anon } from '../../../interfaces/anon';
import { Address } from '../../../interfaces/address';
import { SantaApiService } from '../../services/SantaApiService.service';
import { EventEmitter } from 'protractor';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiService) { }
  approvedClients: any = [];

  ngOnInit() {
    this.SantaApi.getAllClients().subscribe((data: {}) => {
      console.log(data);
      this.approvedClients = data;
    });

  }
}
