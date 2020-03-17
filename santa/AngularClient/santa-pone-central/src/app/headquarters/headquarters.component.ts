import { Component, OnInit } from '@angular/core';
import { Client } from '../../classes/Client';
import { MapEventService } from '../services/map-event.service';
import { ConstantPool } from '@angular/compiler';

@Component({
  selector: 'app-headquarters',
  templateUrl: './headquarters.component.html',
  styleUrls: ['./headquarters.component.css']
})
export class HeadquartersComponent implements OnInit {

  constructor(public mapper: MapEventService) {}

  public showClientCard: boolean = false;
  public currentClient: Client;

  ngOnInit() {
  }

  showClientWindow(client)
  {
    this.currentClient = client;
    this.showClientCard = true;
  }
  hideClientWindow()
  {
    this.showClientCard = false;
  }
}
