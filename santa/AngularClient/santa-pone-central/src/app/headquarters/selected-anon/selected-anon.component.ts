import { Component, OnInit, Input } from '@angular/core';
import { Client } from '../../../classes/client';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SantaApiGetService, SantaApiPutService } from 'src/app/services/SantaApiService.service';
import { MapService } from 'src/app/services/MapService.service';

@Component({
  selector: 'app-selected-anon',
  templateUrl: './selected-anon.component.html',
  styleUrls: ['./selected-anon.component.css'],
  animations: [
    // the fade-in/fade-out animation.
    trigger('simpleFadeAnimation', [

      // the "in" style determines the "resting" state of the element when it is visible.
      state('in', style({opacity: 1})),

      // fade in when created. this could also be written as transition('void => *')
      transition(':enter', [
        style({opacity: 0}),
        animate(600 )
      ]),

      // fade out when destroyed. this could also be written as transition('void => *')
      transition(':leave',
        animate(600, style({opacity: 0})))
    ])
  ]
})
export class SelectedAnonComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public SantaApiPut: SantaApiPutService, public mapper: MapService) { }

  @Input() client: Client = new Client();
  public senders: Array<Client> = new Array<Client>();
  public recievers: Array<Client> = new Array<Client>();

  ngOnInit() {
    this.client.senders.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.mapper.mapClient(client); 
        this.senders.push(c);
      });
    });
    console.log(this.client.recipients);
    this.client.recipients.forEach(clientID => {
      this.SantaApiGet.getClient(clientID).subscribe(client => {
        var c = this.mapper.mapClient(client);
        this.recievers.push(c);
      });
    });
  }
  public approveAnon()
  {
    
  }
}
