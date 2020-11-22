import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { SantaApiPostService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { ContactPanelComponent } from 'src/app/shared/contact-panel/contact-panel.component';
import { InputControlComponent } from 'src/app/shared/input-control/input-control.component';
import { BaseClient, Client } from 'src/classes/client';
import { ClientMeta, MessageHistory } from 'src/classes/message';

@Component({
  selector: 'app-related-intelligence',
  templateUrl: './related-intelligence.component.html',
  styleUrls: ['./related-intelligence.component.css']
})
export class RelatedIntelligenceComponent implements OnInit {

  constructor(public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public ChatService: ChatService) { }

  @Input() clientHistories: Array<MessageHistory> = [];
  @Input() adminSenderMeta: ClientMeta = new ClientMeta();
  @Input() subject: BaseClient = new BaseClient();
  @Input() selectedAnonMeta: ClientMeta = new ClientMeta();

  @Output() openAgentControlEvent: EventEmitter<ClientMeta> = new EventEmitter<ClientMeta>();

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;
  @ViewChild(InputControlComponent) inputComponent: InputControlComponent;

  public selectedHistory: MessageHistory = new MessageHistory();

  public refreshing: boolean = false;

  ngOnInit(): void {
  }
  setSelectedHistory(history: MessageHistory)
  {
    if(history != null)
    {
      this.selectedHistory = history;
    }
    else
    {
      this.selectedHistory = this.getGeneralHistory();
    }
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);
  }
  public async readAll()
  {

  }
  public async send(event)
  {

  }
  public async updateSpecificChat(event)
  {

  }
  public async manualRefreshSelectedChat(isSoftUpdate: boolean = false)
  {
    this.refreshing = true;
    await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID, isSoftUpdate);
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);
    this.refreshing = false;
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
}
