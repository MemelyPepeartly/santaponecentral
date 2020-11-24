import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ChatInfoContainer, MessageHistory } from 'src/classes/message';
import { MessageApiResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../contact-panel/contact-panel.component';
import { InputControlComponent } from '../input-control/input-control.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  constructor() { }

  @Input() chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();
  @Input() inputDisabled: boolean;
  @Input() showChatLoading: boolean;
  @Input() showChatActionProgressBar: boolean;
  @Input() chatRefreshing: boolean;

  @Output() historyUpdatedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() manualRefreshClickedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() sendClickedEvent: EventEmitter<MessageApiResponse> = new EventEmitter<MessageApiResponse>();
  @Output() readAllClickedEvent: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild(ContactPanelComponent) chatWindowComponent: ContactPanelComponent;
  @ViewChild(InputControlComponent) inputComponent: InputControlComponent;

  ngOnInit(): void {
  }
  emitHistoryUpdatedEvent(messageHistoryEvent: MessageHistory)
  {
    this.historyUpdatedEvent.emit(messageHistoryEvent);
  }
  emitManualRefresh(event: boolean)
  {
    this.manualRefreshClickedEvent.emit(event);
  }
  emitSend(messageApiResponseEvent: MessageApiResponse)
  {
    this.sendClickedEvent.emit(messageApiResponseEvent);
  }
  emitReadAllClicked()
  {
    this.readAllClickedEvent.emit(true);
  }
  manualRefresh(isSoftUpdate: boolean = false)
  {

  }
  send(messageApiResponseEvent: MessageApiResponse)
  {

  }
  readAll()
  {

  }
}
