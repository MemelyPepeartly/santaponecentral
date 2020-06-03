import { Component, OnInit, Input } from '@angular/core';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-control-panel',
  templateUrl: './control-panel.component.html',
  styleUrls: ['./control-panel.component.css']
})
export class ControlPanelComponent implements OnInit {

  constructor() { }

  @Input() recipients: Array<Client> = []

  ngOnInit(): void {
  }

}
