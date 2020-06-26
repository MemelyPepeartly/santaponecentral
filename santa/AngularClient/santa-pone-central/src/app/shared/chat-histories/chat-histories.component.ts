import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MessageHistory, MessageMeta } from 'src/classes/message';
import { ProfileService } from 'src/app/services/Profile.service';
import { AuthService } from 'src/app/auth/auth.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-chat-histories',
  templateUrl: './chat-histories.component.html',
  styleUrls: ['./chat-histories.component.css']
})
export class ChatHistoriesComponent implements OnInit {

  constructor(private profileService: ProfileService, private auth: AuthService) { }

  @Input() onProfile: boolean;

  columns: string[] = ["sender", "recipient", "event", "contact"];

  public isAdmin: boolean = false;

  @Input() histories: Array<MessageHistory>

  @Output() chatSelectedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() recipientSelectedEvent: EventEmitter<{meta: MessageMeta, event: EventType}> = new EventEmitter<{meta: MessageMeta, event: EventType}>();


  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
  }
  public emitSelectedHistory(history: MessageHistory)
  {
    this.chatSelectedEvent.emit(history);
  }
  public emitSelectedRecipientInformation(historyMeta: MessageMeta, historyEvent: EventType)
  {
    this.recipientSelectedEvent.emit({meta: historyMeta, event: historyEvent});
  }
  public log()
  {
    console.log(this.isAdmin);
    console.log(this.onProfile);
  }


}