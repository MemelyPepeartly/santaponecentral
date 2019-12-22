import { Component, OnInit } from '@angular/core';
import { ShippingMessage } from '../../../../interfaces/shipping-message';
import { Address } from '../../../../interfaces/address';
import { Anon } from '../../../../interfaces/anon';

@Component({
  selector: 'app-shipping-correspondence',
  templateUrl: './shipping-correspondence.component.html',
  styleUrls: ['./shipping-correspondence.component.css']
})
export class ShippingCorrespondenceComponent implements OnInit {

  constructor() { }

    //test data

    address: Address = 
    {
      streetAddress: '123 Test Street',
      city: 'TestCity',
      state: 'TS',
      zipcode: '12345'
    };
    anons: Anon[] = [
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
    }];

    Shippingmessages: ShippingMessage[] = [
      {
        anonInfo = this.anons[0],
        shippingNumber = '0000000000',
        message = 'Here is my shipping number'
      },
      {
        anonInfo =this.anons[1],
        shippingNumber = '111111111',
        message = 'Haha look, I aint a grinch'
      }];

  ngOnInit() {
  }

}
