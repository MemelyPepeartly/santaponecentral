import { Component, OnInit } from '@angular/core';
import { ShippingMessage } from '../../../../interfaces/shipping-message';
import { Address } from '../../../../interfaces/address';
import { Client } from '../../../../interfaces/client';

@Component({
  selector: 'app-shipping-correspondence',
  templateUrl: './shipping-correspondence.component.html',
  styleUrls: ['./shipping-correspondence.component.css']
})
export class ShippingCorrespondenceComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
