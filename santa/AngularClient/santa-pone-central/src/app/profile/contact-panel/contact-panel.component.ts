import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/classes/message';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit {

  constructor() { }

  @Input() chat: Array<Message>;

  ngOnInit(): void {
  }

}
