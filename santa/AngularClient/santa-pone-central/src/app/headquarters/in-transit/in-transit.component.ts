import { Component, OnInit } from '@angular/core';
import { Anon } from '../../../interfaces/anon';
import { Address } from '../../../interfaces/address';


@Component({
  selector: 'app-in-transit',
  templateUrl: './in-transit.component.html',
  styleUrls: ['./in-transit.component.css']
})
export class InTransitComponent implements OnInit {

  constructor() { }

  // Test data

  address: Address = 
  {
    streetAddress: '123 Test Street',
    city: 'TestCity',
    state: 'TS',
    zipcode: '12345'
  };
  santas: Anon[] = [
  {
  realName: 'Chad',
  address: this.address,
  wishlist: 'I want ponies!',
  email: 'chad@anon.non',
  otherInfo: 'Im allergic to normies'
  },
  {
  realName: 'Stacy',
  address: this.address,
  wishlist: 'I want to die!',
  email: 'stacy@anon.non',
  otherInfo: 'Im allergic to happiness'
  },
  ];

  ngOnInit() {
  }

}
