import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ChatComponent } from 'src/app/shared/chat/chat.component';
import { BaseClient } from 'src/classes/client';
import { ChatInfoContainer, ClientMeta, MessageHistory } from 'src/classes/message';

@Component({
  selector: 'app-related-intelligence',
  templateUrl: './related-intelligence.component.html',
  styleUrls: ['./related-intelligence.component.css']
})
export class RelatedIntelligenceComponent implements OnInit {

  constructor() { }

  @Input() clientHistories: Array<MessageHistory> = [];
  @Input() adminSenderMeta: ClientMeta = new ClientMeta();
  @Input() subject: BaseClient = new BaseClient();
  @Input() selectedAnonMeta: ClientMeta = new ClientMeta();
  @Input() chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();

  @Output() openAgentControlEvent: EventEmitter<ClientMeta> = new EventEmitter<ClientMeta>();
  @Output() messageSentEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() historyUpdatedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();

  @ViewChild(ChatComponent) chatComponent: ChatComponent;

  public refreshing: boolean = false;
  public postingMessage: boolean = false;
  public showSelectedChat: boolean = false;
  public initializing: boolean = false;

  ngOnInit(): void {
    this.initializing = true;
    this.initializing = false;
  }
  public setInfoContainerValues(history: MessageHistory)
  {
    this.chatInfoContainer =
    {
      messageSenderID: this.chatInfoContainer.messageSenderID,
      senderIsAdmin: this.chatInfoContainer.senderIsAdmin,
      conversationClientID: history.conversationClient.clientID,
      messageRecieverID: history.conversationClient.clientID,
      relationshipXrefID: history.relationXrefID,
      eventTypeID: history.eventType.eventTypeID
    }

    this.showSelectedChat = true;
  }
  public getGeneralHistory() : MessageHistory
  {
    return this.clientHistories.find((history: MessageHistory) => {
      return history.relationXrefID == undefined;
    });
  }
  public emitAgentControlSelected(clientMeta: ClientMeta)
  {
    this.openAgentControlEvent.emit(clientMeta);
  }
  public async updateSpecificChat(historyEvent: MessageHistory)
  {
    this.historyUpdatedEvent.emit(historyEvent);
  }
}
