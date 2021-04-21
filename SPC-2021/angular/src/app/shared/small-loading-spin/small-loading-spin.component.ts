import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-small-loading-spin',
  templateUrl: './small-loading-spin.component.html',
  styleUrls: ['./small-loading-spin.component.css']
})
export class SmallLoadingSpinComponent implements OnInit {

  constructor() { }

  // Spinner type 1 is for SantaShark loading
  // Spinner type 2 is for Card dragon loading
  // Spinner type 3 is for Duk gift loading
  @Input() spinnerType: number = 1;

  ngOnInit() {
  }

}
