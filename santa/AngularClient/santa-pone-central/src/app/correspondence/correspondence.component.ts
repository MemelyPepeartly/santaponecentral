import { Component, OnInit } from '@angular/core';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { EventType } from 'src/classes/eventType';
import { ChatService } from '../services/Chat.service';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { Client } from 'src/classes/client';
import { MapService } from '../services/mapService.service';



@Component({
  selector: 'app-correspondence',
  templateUrl: './correspondence.component.html',
  styleUrls: ['./correspondence.component.css']
})
export class CorrespondenceComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService,
    public ChatService: ChatService,
    public gatherer: GathererService,
    public mapper: MapService) { }

  public allChats: Array<MessageHistory> = []
  public unreadChats: Array<MessageHistory> = []
  public eventChats: Array<MessageHistory> = []
  public generalChats: Array<MessageHistory> = []
  public events: Array<EventType> = []

  public showClientCard: boolean = false;

  public selectedAnon: Client;


  public async ngOnInit() {
    this.ChatService.allChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.allChats = historyArray;
    });
    this.ChatService.allEventChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.eventChats = historyArray;
    });
    this.ChatService.allGeneralChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.generalChats = historyArray;
    });
    this.ChatService.allUnreadChats.subscribe((historyArray: Array<MessageHistory>) => {
      this.unreadChats = historyArray;
    });
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });

    await this.gatherer.gatherAllEvents();
    await this.gatherAllChats();
  }
  public async gatherAllChats()
  {
    await this.ChatService.gatherAllChats();
    await this.ChatService.gatherEventChats();
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

  public hideClientWindow()
  {

  }
  public async populateSelectAnonCard(meta: ClientMeta)
  {
    this.selectedAnon = this.mapper.mapClient(await this.SantaApiGet.getClient(meta.clientID).toPromise());
    console.log(this.selectedAnon);
  }
  async updateSelectedClient(clientID: string)
  {
    this.selectedAnon = this.mapper.mapClient(await this.SantaApiGet.getClient(clientID).toPromise());
  }
}
