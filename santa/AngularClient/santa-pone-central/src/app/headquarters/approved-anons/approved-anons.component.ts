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
    holidayID: 'Probably Racist Pepe',
    address: this.address,
    wishlist: 'I want to feel!',
    email: 'pepe@anon.non',
    otherInfo: 'REEEEEEEEE'
    },
    {
    realName: 'Boomer',
    holidayID: 'Okay Boomer',
    address: this.address,
    wishlist: 'Quaaludes',
    email: 'boomer@aol.com',
    otherInfo: 'Hehe yup... Zoomers gonna zoom...'
    },
    ];

  ngOnInit() {
  }

}
