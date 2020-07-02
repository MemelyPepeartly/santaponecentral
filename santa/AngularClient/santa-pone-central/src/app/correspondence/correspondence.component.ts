import { Component, OnInit, ViewChild } from '@angular/core';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { EventType } from 'src/classes/eventType';
import { ChatService } from '../services/Chat.service';
import { SantaApiGetService, SantaApiPostService, SantaApiPutService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { Client } from 'src/classes/client';
import { MapService } from '../services/mapService.service';
import { FormGroup } from '@angular/forms';
import { MessageApiResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';



@Component({
  selector: 'app-correspondence',
  templateUrl: './correspondence.component.html',
  styleUrls: ['./correspondence.component.css']
})
export class CorrespondenceComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public ChatService: ChatService,
    public gatherer: GathererService,
    public mapper: MapService) { }

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;
  
  public allChats: Array<MessageHistory> = []
  public eventChats: Array<MessageHistory> = []
  public events: Array<EventType> = []

  public showClientCard: boolean = false;
  public showChat: boolean = false;
  public postingMessage: boolean = false;

  public selectedAnon: Client = new Client();
  public adminSenderMeta: ClientMeta = new ClientMeta();
  public selectedRecieverMeta: ClientMeta = new ClientMeta();
  public selectedHistory: MessageHistory = new MessageHistory();


  public async ngOnInit() {
    this.ChatService.allChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.allChats = historyArray;
    });
    this.ChatService.allEventChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.eventChats = historyArray;
    });
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });
    this.ChatService.selectedHistory.subscribe((history: MessageHistory) => {
      this.selectedHistory = history;
    });

    await this.gatherer.gatherAllEvents();
    await this.gatherAllChats(false);
  }
  public async gatherAllChats(isSoftGather: boolean)
  {
    await this.ChatService.gatherAllChats(isSoftGather);
    await this.ChatService.gatherEventChats(isSoftGather);
  }
  public sortByEvent(eventType: EventType)
  {   
    return this.eventChats.filter((history: MessageHistory) => {
      return history.eventType.eventTypeID == eventType.eventTypeID;
    });
  }
  public sortByUnread()
  {   
    return this.allChats.filter((history: MessageHistory) => {
      return history.adminUnreadCount > 0;
    });
  }
  public sortByGeneral()
  {   
    return this.allChats.filter((history: MessageHistory) => {
      return history.relationXrefID == null;
    });
  }
  public async send(messageResponse: MessageApiResponse)
  {
    this.postingMessage = true;

    await this.SantaApiPost.postMessage(messageResponse).toPromise();
    this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
    this.chatComponent.scrollToBottom();

    this.postingMessage = false;
    
  }
  public hideWindow()
  {
    if(!this.chatComponent.markingRead)
    {
      this.showClientCard = false;
      this.showChat = false;
      this.selectedAnon = undefined;
      this.selectedHistory = undefined;
    }
  }
  public async populateSelectAnonCard(meta: ClientMeta)
  {
    this.SantaApiGet.getClient(meta.clientID).subscribe(client => {
      this.selectedAnon = this.mapper.mapClient(client);
      this.showClientCard = true;
    },err => { console.log(err); });
  }
  public async openSelectedChat(history: MessageHistory)
  {
    this.selectedHistory = history;
    this.selectedRecieverMeta = history.conversationClient;
    this.showChat = true;
  }
  public async updateSelectedClient(clientID: string)
  {
    this.selectedAnon = this.mapper.mapClient(await this.SantaApiGet.getClient(clientID).toPromise());
  }
  public async updateChat(event: boolean)
  {
    if(event)
    {
      this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
      this.gatherAllChats(true);
    }
  }
}
