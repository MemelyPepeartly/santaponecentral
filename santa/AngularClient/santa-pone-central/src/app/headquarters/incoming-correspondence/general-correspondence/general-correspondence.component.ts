import { Component, OnInit } from '@angular/core';
import { Message } from '../../../../interfaces/message';
import { Address } from '../../../../interfaces/address';
import { Anon } from '../../../../interfaces/anon';

@Component({
  selector: 'app-general-correspondence',
  templateUrl: './general-correspondence.component.html',
  styleUrls: ['./general-correspondence.component.css']
})
export class GeneralCorrespondenceComponent implements OnInit {

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

  messages: Message[] = [
  {
    anonInfo: this.anons[0],
    subject: 'Question',
    message: 'Hi, I just had a question',
  },
  {
    anonInfo: this.anons[1],
    subject: 'Santee question',
    message: 'I dunno if Imma be grinched, Ill be real'
  }];

  ngOnInit() {
  }

}
