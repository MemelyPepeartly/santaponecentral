import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MessageHistory, ClientMeta, Message, ChatInfoContainer } from 'src/classes/message';
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
import { OrganizerEmailConstants } from '../shared/constants/organizerEmailConstants.enum';
import { EventConstants } from '../shared/constants/eventConstants.enum';
import { ChatComponent } from '../shared/chat/chat.component';



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

  @ViewChild(SelectedAnonComponent) selectedAnonComponent: SelectedAnonComponent;
  @ViewChild(ChatComponent) chatComponent: ChatComponent;

  /** Auth profile for the person accessing the component */
  public profile: any;
  /** Base client object of the subject. This will be the same as the sender meta, just a different type for input reasons on other components */
  public subject: BaseClient = new BaseClient();
  /** Meta of the sending admin */
  public adminSenderMeta: ClientMeta = new ClientMeta();

  public allChats: Array<MessageHistory> = []
  public events: Array<EventType> = []
  public assignmentStatuses: Array<AssignmentStatus> = [];

  public gettingAllChats: boolean = false;
  public softGettingAllChats: boolean = false;
  public gettingSelectedHistory: boolean = false;
  public loadingClient: boolean = false;
  public initializing: boolean;

  public showClientCard: boolean = false;
  public showChat: boolean = false;
  public showRelatedIntelligenceCard: boolean = false;
  public updateOnClickaway: boolean = false;
  public clickAwayLocked: boolean = false;
  public get showOverlay() : boolean
  {
    return this.showClientCard || this.showChat || this.showRelatedIntelligenceCard
  }

  public agentControlID: string;
  public selectedAnonMeta: ClientMeta = new ClientMeta();
  public selectedRecieverMeta: ClientMeta = new ClientMeta();
  public selectedFilteredChats: Array<MessageHistory> = [];
  public chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();

  public async ngOnInit() {
    this.initializing = true;

    /* Sets the subject viewer of the messages as a meta */
    this.Auth.userProfile$.subscribe((data: any) => {
      this.profile = data
    });
    this.subject = this.mapper.mapBaseClient(await this.SantaApiGet.getBasicClientByEmail(this.profile.email).toPromise());
    this.adminSenderMeta =
    {
      clientID: this.subject.clientID,
      clientName: this.subject.clientName,
      clientNickname: this.subject.nickname,
      hasAccount: this.subject.hasAccount,
      isAdmin: this.subject.isAdmin
    };

    this.chatInfoContainer =
    {
      senderIsAdmin: this.adminSenderMeta.isAdmin,
      messageSenderID: this.adminSenderMeta.clientID,
      // Values here are null and undefined until a chat is selected
      messageRecieverID: undefined,
      conversationClientID: undefined,
      eventTypeID: null,
      relationshipXrefID: null,
    };


    // Boolean subscribes
    this.ChatService.gettingAllChats.subscribe((status: boolean) => {
      this.gettingAllChats = status;
    });
    this.ChatService.softGettingAllChats.subscribe((status: boolean) => {
      this.softGettingAllChats = status;
    });

    /* -- Data subscribes -- */
    // All chats
    this.ChatService.allChats.subscribe((historyArray: Array<MessageHistory>) => {
      // If the auth profile email is Santapone's
      if(this.profile.email == OrganizerEmailConstants.SANTAPONE)
      {
        // All chats is equal to all the chats that are general chats, or for the gift exchange
        this.allChats = historyArray.filter((history: MessageHistory) => {
          return history.relationXrefID == null || history.eventType.eventDescription == EventConstants.GIFT_EXCHANGE_EVENT
        });
      }
      // Else just set it equal to the whole history response
      else
      {
        this.allChats = historyArray;
      }
    });

    // All events
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });

    // Assignment Statuses
    this.gatherer.allAssignmentStatuses.subscribe((assignmentStatusArray: Array<AssignmentStatus>) => {
      this.assignmentStatuses = assignmentStatusArray;
    });
    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllAssignmentStatuses();
    await this.ChatService.gatherAllChats(this.subject.clientID, false);

    this.initializing = false;

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
  public async hideWindow()
  {
    if(!this.clickAwayLocked)
    {
      if(this.showRelatedIntelligenceCard)
      {
        this.showRelatedIntelligenceCard = false;
      }
      if(this.chatComponent)
      {
        // If the chat component isn't marking read, and the button for sending isnt disabled (implying sending) and showChat is true
        if(!this.chatComponent.chatWindowComponent.markingRead && !this.chatComponent.inputComponent.disabled && this.showChat == true)
        {
          this.showChat = false;
          // SANTAHERE needs updating
          //this.selectedHistory = new MessageHistory();

          // If the updater variable is true, refresh on clicking away
          if(this.updateOnClickaway)
          {
            await this.ChatService.gatherAllChats(this.subject.clientID, true);
            this.updateOnClickaway = false;
          }
        }
      }
      // If the selectedAnonComponent is up
      if(this.selectedAnonComponent)
      {
        // Close it out
        if(this.showClientCard == true)
        {
          this.showClientCard = false;

          // If the updater variable is true, refresh on clicking away
          if(this.updateOnClickaway)
          {
            await this.ChatService.gatherAllChats(this.subject.clientID, true);
            this.updateOnClickaway = false;
          }
        }
      }
      this.selectedAnonMeta = new ClientMeta();
    }
  }
  public setClickawayLock(status: boolean)
  {
    this.clickAwayLocked = status;
  }
  public async updateChats(isSoftUpdate: boolean = false, skipSelected: boolean = false)
  {
    this.updateOnClickaway = true;
    if(!skipSelected)
    {
      // SANTAHERE needs updating
      //await this.ChatService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.subject.clientID, this.selectedHistory.relationXrefID, isSoftUpdate);
    }
    await this.ChatService.gatherAllChats(this.subject.clientID ,isSoftUpdate);
  }
  public updateSpecificChat(historyEvent: MessageHistory)
  {
    var chatIndex = this.allChats.findIndex((history: MessageHistory) => {
      return history.relationXrefID == historyEvent.relationXrefID &&
      history.conversationClient.clientID == historyEvent.conversationClient.clientID &&
      history.assignmentRecieverClient.clientID == historyEvent.assignmentRecieverClient.clientID &&
      history.assignmentSenderClient.clientID == historyEvent.assignmentSenderClient.clientID &&
      history.eventType.eventTypeID == historyEvent.eventType.eventTypeID
    });

    if(chatIndex != undefined)
    {
      this.allChats[chatIndex] = historyEvent
    }
    else
    {
      console.log(chatIndex);
      console.log("Could not find chat to update");
    }
  }
  public async manualRefresh(isSoftUpdate: boolean = false)
  {
    await this.ChatService.gatherAllChats(this.subject.clientID, isSoftUpdate);
  }
  public filterRelatedChats() : Array<MessageHistory>
  {
    return this.allChats.filter((history: MessageHistory) => {
      return history.conversationClient.clientID == this.selectedAnonMeta.clientID;
    });
  }
  public async openRelatedIntelligenceCard(meta: ClientMeta)
  {
   this.selectedAnonMeta = meta;
   this.showRelatedIntelligenceCard = true;
  }
  public async openSelectedChat(history: MessageHistory)
  {
    this.chatInfoContainer.conversationClientID = history.conversationClient.clientID;
    this.chatInfoContainer.messageRecieverID = history.conversationClient.clientID;
    this.chatInfoContainer.relationshipXrefID = history.relationXrefID;
    if(history.relationXrefID != null && history.relationXrefID != undefined)
    {
      this.chatInfoContainer.eventTypeID = history.eventType.eventTypeID
    }
    this.showChat = true;
  }
  public openClientCard(clientMeta: ClientMeta)
  {
    this.agentControlID = clientMeta.clientID;
    this.showRelatedIntelligenceCard = false;
    this.showClientCard = true;
  }
  public backToRelatedIntelligence()
  {
    this.showClientCard = false;
    this.agentControlID = undefined;
    this.showRelatedIntelligenceCard = true;
  }
}
