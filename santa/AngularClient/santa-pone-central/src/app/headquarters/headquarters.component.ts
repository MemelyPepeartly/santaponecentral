import { Component, OnInit } from '@angular/core';
import { Client } from '../../classes/client';
import { MapService } from '../services/MapService.service';
import { ConstantPool } from '@angular/compiler';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-headquarters',
  templateUrl: './headquarters.component.html',
  styleUrls: ['./headquarters.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(200)
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(200, style({opacity: 0})))
    ])
  ]
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
