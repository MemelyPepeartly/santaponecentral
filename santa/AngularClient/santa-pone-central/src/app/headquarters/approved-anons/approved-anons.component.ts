import { Component, OnInit } from '@angular/core';
import { Anon } from '../../../interfaces/anon';
import { Address } from '../../../interfaces/address';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor() { }

    // test data
  
    address: Address = 
    {
      streetAddress: '123 Test Street',
      city: 'TestCity',
      state: 'TS',
      zipcode: '12345'
    };
    santas: Anon[] = [
    {
    realName: 'Pepe',
    address: this.address,
    wishlist: 'I want to feel!',
    email: 'pepe@anon.non',
    otherInfo: 'REEEEEEEEE'
    },
    {
    realName: 'Virgin',
    address: this.address,
    wishlist: 'I dont want much...',
    email: 'virgin@anon.non',
    otherInfo: 'Im... er... nevermind...'
    },
    ];

  ngOnInit() {
  }

}
