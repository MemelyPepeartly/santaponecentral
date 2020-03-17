import { Component, OnInit, Input } from '@angular/core';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-selected-anon',
  templateUrl: './selected-anon.component.html',
  styleUrls: ['./selected-anon.component.css']
})
export class SelectedAnonComponent implements OnInit {

  constructor() { }

  @Input() client: Client = new Client();

  ngOnInit() {
    
  }

}
