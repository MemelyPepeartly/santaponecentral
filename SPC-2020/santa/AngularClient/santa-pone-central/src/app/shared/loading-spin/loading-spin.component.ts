import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-loading-spin',
  templateUrl: './loading-spin.component.html',
  styleUrls: ['./loading-spin.component.css']
})
export class LoadingSpinComponent implements OnInit {

  constructor() { }

  // Spinner type 1 is for SantaShark loading
  // Spinner type 2 is for Card dragon loading
  // Spinner type 3 is for Duk gift loading
  @Input() spinnerType: number = 1;

  ngOnInit() {
  }

}
