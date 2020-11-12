import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MessageHistory, ClientMeta, Message } from 'src/classes/message';
import { EventType } from 'src/classes/eventType';
import { ChatService } from '../services/chat.service';
import { SantaApiGetService, SantaApiPostService, SantaApiPutService } from '../services/santa-api.service';
import { GathererService } from '../services/gatherer.service';
import { AssignmentStatus, BaseClient, Client, HQClient } from 'src/classes/client';
import { MapService } from '../services/mapper.service';
import { FormGroup } from '@angular/forms';
import { MessageApiResponse, MessageApiReadAllResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';
import { InputControlComponent } from '../shared/input-control/input-control.component';
import { AuthService } from '../auth/auth.service';
import { SelectedAnonComponent } from '../headquarters/selected-anon/selected-anon.component';



@Component({
  selector: 'app-correspondence',
  templateUrl: './correspondence.component.html',
  styleUrls: ['./correspondence.component.css']
})
export class CorrespondenceComponent implements OnInit, OnDestroy {

  constructor(public SantaApiGet: SantaApiGetService,
    public Auth: AuthService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public ChatService: ChatService,
    public gatherer: GathererService,
    public mapper: MapService) { }

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;
  @ViewChild(InputControlComponent) inputComponent: InputControlComponent;
  @ViewChild(SelectedAnonComponent) selectedAnonComponent: SelectedAnonComponent;

  public profile: any;
  public subject: BaseClient = new BaseClient();
  public adminSenderMeta: ClientMeta = new ClientMeta();

  public allChats: Array<MessageHistory> = []
  public eventChats: Array<MessageHistory> = []
  public events: Array<EventType> = []
  public assignmentStatuses: Array<AssignmentStatus> = [];

  public gettingAllChats: boolean = false;
  public softGettingAllChats: boolean = false;
  public gettingSelectedHistory: boolean = false;
  public loadingClient: boolean = false;
  public puttingMessage: boolean = false;
  public refreshing: boolean = false;
  public initializing: boolean;

  public showClientCard: boolean = false;
  public showChat: boolean = false;
  public postingMessage: boolean = false;
  public updateOnClickaway: boolean = false;
  public clickAwayLocked: boolean = false;

  public selectedAnonID: string;
  public selectedRecieverMeta: ClientMeta = new ClientMeta();
  public selectedHistory: MessageHistory = new MessageHistory();


  public async ngOnInit() {
    this.initializing = true;

    /* Sets the subject viewer of the messages as a meta */
    this.Auth.userProfile$.subscribe((data: any) => {
      this.profile = data
    });
    this.subject = this.mapper.mapBaseClient(await this.SantaApiGet.getBasicClientByEmail(this.profile.email).toPromise());

    this.adminSenderMeta.clientID = this.subject.clientID
    this.adminSenderMeta.clientName = this.subject.clientName
    this.adminSenderMeta.clientNickname = this.subject.nickname


    // Boolean subscribes
    this.ChatService.gettingAllChats.subscribe((status: boolean) => {
      this.gettingAllChats = status;
    });
    this.ChatService.softGettingAllChats.subscribe((status: boolean) => {
      this.softGettingAllChats = status;
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

    // Assignment Statuses
    this.gatherer.allAssignmentStatuses.subscribe((assignmentStatusArray: Array<AssignmentStatus>) => {
      this.assignmentStatuses = assignmentStatusArray;
    });

    // Selected history
    this.ChatService.selectedHistory.subscribe((history: MessageHistory) => {
      this.selectedHistory = history;
    });

    this.initializing = false;

    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllAssignmentStatuses();
    await this.ChatService.gatherAllChats(this.subject.clientID, false);

  }
  ngOnDestroy(): void {
    this.ChatService.clearAllChats();
  }
  public sortByEvent(eventType: EventType)
  {
    return this.allChats.filter((history: MessageHistory) => {
      return history.eventType.eventTypeID == eventType.eventTypeID;
    });
  }
  public sortByAssignmentStatus(assignmentStatus: AssignmentStatus)
  {
    return this.allChats.filter((history: MessageHistory) => {
      return history.assignmentStatus.assignmentStatusID == assignmentStatus.assignmentStatusID;
    });
  }
  public sortByUnread()
  {
    return this.allChats.filter((history: MessageHistory) => {
      return history.unreadCount > 0;
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

    this.updateOnClickaway = true;
    await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID);
    this.inputComponent.clearForm();

    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.postingMessage = false;

  }
  public async readAll()
  {
    this.puttingMessage = true;

    let unreadMessages: Array<Message> = this.selectedHistory.recieverMessages.filter((message: Message) => { return message.isMessageRead == false });
    let response: MessageApiReadAllResponse = new MessageApiReadAllResponse();
    unreadMessages.forEach((message: Message) => { response.messages.push(message.chatMessageID)});

    await this.SantaApiPut.putMessageReadAll(response).toPromise();
    this.updateOnClickaway = true;

    await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID, true);
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.puttingMessage = false;
  }
  public async hideWindow()
  {
    if(!this.clickAwayLocked)
    {
      if(this.chatComponent == undefined && this.showClientCard == true)
      {
        this.showClientCard = false;
        this.selectedAnonID = undefined;

        // If the updater variable is true, refresh on clicking away
        if(this.updateOnClickaway)
        {
          await this.ChatService.gatherAllChats(this.subject.clientID, true);
          this.updateOnClickaway = false;
        }
      }
      // If the chat component isn't marking read, and the button for sending isnt disabled (implying sending) and showChat is true
      else if(!this.chatComponent.markingRead && !this.inputComponent.disabled && this.showChat == true)
      {
        this.showChat = false;
        this.selectedHistory = new MessageHistory();
        // If the updater variable is true, refresh on clicking away
        if(this.updateOnClickaway)
        {
          await this.ChatService.gatherAllChats(this.subject.clientID, true);
          this.updateOnClickaway = false;
        }
      }
    }
  }
  public setClickawayLock(status: boolean)
  {
    this.clickAwayLocked = status;
  }
  public async populateSelectAnonCard(meta: ClientMeta)
  {
    this.loadingClient = true;
    this.showClientCard = true;

    this.selectedAnonID = meta.clientID

    this.loadingClient = false;
  }
  public async openSelectedChat(history: MessageHistory)
  {
    this.selectedHistory = history;
    this.selectedRecieverMeta = history.conversationClient;
    this.showChat = true;
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);
  }
  public async updateChats(isSoftUpdate: boolean = false, skipSelected: boolean = false)
  {
    this.updateOnClickaway = true;
    if(!skipSelected)
    {
      await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID, isSoftUpdate);
    }
    await this.ChatService.gatherAllChats(this.subject.clientID ,isSoftUpdate);
  }
  public async manualRefreshSelectedChat(isSoftUpdate: boolean = false)
  {
    this.refreshing = true;
    await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID, isSoftUpdate);
    setTimeout(() => this.chatComponent.scrollToBottom(), 0);
    this.refreshing = false;
  }
  public async manualRefresh()
  {
    await this.ChatService.gatherAllChats(this.subject.clientID);
  }
}
