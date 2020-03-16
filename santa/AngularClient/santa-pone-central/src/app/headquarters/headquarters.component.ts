import { Component, OnInit } from '@angular/core';
import { Client } from '../../interfaces/client'

@Component({
  selector: 'app-headquarters',
  templateUrl: './headquarters.component.html',
  styleUrls: ['./headquarters.component.css']
})
export class HeadquartersComponent implements OnInit {

  constructor() { }

  showClientCard: boolean = false;
  currentClient: Client;

  ngOnInit() {
  }
  showClientWindow(client)
  {
    console.log(client);
  }
}
