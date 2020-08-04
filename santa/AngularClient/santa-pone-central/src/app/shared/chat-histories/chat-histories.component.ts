import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MessageHistory, ClientMeta, Message } from 'src/classes/message';
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

  @Input() onProfile: boolean = false;
  @Input() histories: Array<MessageHistory>
  @Input() disabled: boolean = false;

  columns: string[] = ["sender", "recipient", "event", "contact"];

  public isAdmin: boolean = false;

  @Output() chatSelectedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() recipientSelectedEvent: EventEmitter<{meta: ClientMeta, event: EventType}> = new EventEmitter<{meta: ClientMeta, event: EventType}>();


  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
  }
  public emitSelectedHistory(history: MessageHistory)
  {
    this.chatSelectedEvent.emit(history);
  }
  public emitSelectedRecipientInformation(historyMeta: ClientMeta, historyEvent: EventType)
  {
    this.recipientSelectedEvent.emit({meta: historyMeta, event: historyEvent});
  }
}
