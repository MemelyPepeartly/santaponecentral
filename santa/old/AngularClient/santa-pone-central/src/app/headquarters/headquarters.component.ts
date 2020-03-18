import { Component, OnInit } from '@angular/core';
import { Client } from '../../classes/client';
import { MapService } from '../services/MapService.service';
import { ConstantPool } from '@angular/compiler';

@Component({
  selector: 'app-headquarters',
  templateUrl: './headquarters.component.html',
  styleUrls: ['./headquarters.component.css']
})
export class HeadquartersComponent implements OnInit {

  constructor(public mapper: MapService) {}

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
