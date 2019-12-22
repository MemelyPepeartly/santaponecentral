import { Component, OnInit } from '@angular/core';
import { Anon } from '../../../interfaces/anon';
import { Address } from '../../../interfaces/address';

@Component({
  selector: 'app-incoming-signups',
  templateUrl: './incoming-signups.component.html',
  styleUrls: ['./incoming-signups.component.css']
})
export class IncomingSignupsComponent implements OnInit {

  constructor() { }

  // test data
  
  address: Address = 
  {
    streetAddress: '123 Test Street',
    city: 'TestCity',
    state: 'TS',
    zipcode: '12345'
  };
  requests: Anon[] = [
  {
  realName: 'Zoomer',
  holidayID: 'Zippy Zoomer',
  address: this.address,
  wishlist: 'I want ponies!',
  email: 'anon@anon.non',
  otherInfo: 'Im allergic to normies'
  },
  {
  realName: 'Wojak',
  holidayID: 'Wilin Wojak',
  address: this.address,
  wishlist: 'I want to die!',
  email: 'wojak@anon.non',
  otherInfo: 'Im allergic to happiness'
  },
  ];

  ngOnInit() {
  }

}
