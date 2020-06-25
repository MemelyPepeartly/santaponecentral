import { Component, OnInit, Input } from '@angular/core';
import { MessageHistory } from 'src/classes/message';

@Component({
  selector: 'app-chat-histories',
  templateUrl: './chat-histories.component.html',
  styleUrls: ['./chat-histories.component.css']
})
export class ChatHistoriesComponent implements OnInit {

  constructor() { }

  columns: string[] = ["recipient", "event", "contact"];

  @Input() histories: Array<MessageHistory>

  ngOnInit(): void {
  }

}
