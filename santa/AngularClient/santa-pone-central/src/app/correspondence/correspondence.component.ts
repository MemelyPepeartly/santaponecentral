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
import { InputControlComponent } from '../shared/input-control/input-control.component';



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
  @ViewChild(InputControlComponent) inputComponent: InputControlComponent;
  
  public allChats: Array<MessageHistory> = []
  public eventChats: Array<MessageHistory> = []
  public events: Array<EventType> = []

  public gettingAllChats: boolean = false;
  public gettingAllEventChats: boolean = false;
  public gettingSelectedHistory: boolean = false;

  public showClientCard: boolean = false;
  public showChat: boolean = false;
  public postingMessage: boolean = false;
  public updateOnClickaway: boolean = false;

  public selectedAnon: Client = new Client();
  public adminSenderMeta: ClientMeta = new ClientMeta();
  public selectedRecieverMeta: ClientMeta = new ClientMeta();
  public selectedHistory: MessageHistory = new MessageHistory();


  public async ngOnInit() {
    // Boolean subscribes
    this.ChatService.gettingAllChats.subscribe((status: boolean) => {
      this.gettingAllChats = status;
    });
    this.ChatService.gettingAllEventChats.subscribe((status: boolean) => {

      this.gettingAllEventChats = status;
    });
    this.ChatService.gettingSelectedHistory.subscribe((status: boolean) => {
      this.gettingSelectedHistory = status;
    });
    
    /* -- Data subscribes -- */
    // All chats
    this.ChatService.allChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.allChats = historyArray;
    });

    // All events
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });
    
    // Selected history
    this.ChatService.selectedHistory.subscribe((history: MessageHistory) => {
      this.selectedHistory = history;
    });

    await this.gatherer.gatherAllEvents();
    await this.ChatService.gatherAllChats(false);
    
  }
  public sortByEvent(eventType: EventType)
  { 
    return this.allChats.filter((history: MessageHistory) => {
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
  public async hideWindow()
  {
    if(this.chatComponent == undefined && this.showClientCard == true)
    {
      this.showClientCard = false;
      this.selectedAnon = new Client();
    }
    // If the chat component isn't marking read, and the button for sending isnt disabled (implying sending) and showChat is true
    else if(!this.chatComponent.markingRead && !this.inputComponent.disabled && this.showChat == true)
    {
      this.showChat = false;
      this.selectedHistory = new MessageHistory();
      // If the updater variable is true, refresh on clicking away
      if(this.updateOnClickaway)
      {
        await this.ChatService.gatherAllChats(true);
        this.updateOnClickaway = false;
      }
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
  public async updateChats(event: boolean)
  {
    if(event)
    {
      this.updateOnClickaway = true
      this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
      await this.ChatService.gatherAllChats(true);
    }
  }
}
