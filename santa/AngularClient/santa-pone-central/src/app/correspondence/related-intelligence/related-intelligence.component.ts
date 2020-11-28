import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { MapService } from 'src/app/services/mapper.service';
import { SantaApiGetService, SantaApiPostService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { ChatComponent } from 'src/app/shared/chat/chat.component';
import { ContactPanelComponent } from 'src/app/shared/contact-panel/contact-panel.component';
import { InputControlComponent } from 'src/app/shared/input-control/input-control.component';
import { BaseClient, Client } from 'src/classes/client';
import { ChatInfoContainer, ClientMeta, MessageHistory } from 'src/classes/message';
import { MessageApiResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-related-intelligence',
  templateUrl: './related-intelligence.component.html',
  styleUrls: ['./related-intelligence.component.css']
})
export class RelatedIntelligenceComponent implements OnInit {

  constructor(public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiGet: SantaApiGetService,
    public mapper: MapService,
    public ChatService: ChatService) { }

  @Input() clientHistories: Array<MessageHistory> = [];
  @Input() adminSenderMeta: ClientMeta = new ClientMeta();
  @Input() subject: BaseClient = new BaseClient();
  @Input() selectedAnonMeta: ClientMeta = new ClientMeta();
  @Input() chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();

  @Output() openAgentControlEvent: EventEmitter<ClientMeta> = new EventEmitter<ClientMeta>();
  @Output() messageSentEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();

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
    console.log(this.chatInfoContainer);

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
    var chatIndex = this.clientHistories.findIndex((history: MessageHistory) => {
      return history.relationXrefID == historyEvent.relationXrefID &&
      history.conversationClient.clientID == historyEvent.conversationClient.clientID &&
      history.assignmentRecieverClient.clientID == historyEvent.assignmentRecieverClient.clientID &&
      history.assignmentSenderClient.clientID == historyEvent.assignmentSenderClient.clientID &&
      history.eventType.eventTypeID == historyEvent.eventType.eventTypeID
    });

    if(chatIndex != undefined)
    {
      this.clientHistories[chatIndex] = historyEvent;
    }
    else
    {
      console.log(chatIndex);
      console.log("Could not find chat to update");
    }
  }
}
