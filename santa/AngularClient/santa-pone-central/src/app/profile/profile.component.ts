import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SantaApiGetService, SantaApiPostService, SantaApiPutService } from '../services/santaApiService.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { MessageHistory, ClientMeta, Message } from 'src/classes/message';
import { ProfileService } from '../services/Profile.service';
import { MessageApiResponse, MessageApiReadAllResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public profileService: ProfileService,
    public gatherer: GathererService,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiPut: SantaApiPutService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;

  public profile: Profile = new Profile();
  public authProfile: any;

  public selectedRecipient: ProfileRecipient = new ProfileRecipient();
  public selectedHistory: MessageHistory = new MessageHistory();
  public generalHistory: MessageHistory = new MessageHistory();

  public histories: Array<MessageHistory> = [];
  public events: Array<EventType> = [];
  public adminRecieverMeta: ClientMeta = new ClientMeta;

  public showOverlay: boolean = false;
  public showChat: boolean = false;

  public postingMessage: boolean = false;
  public puttingMessage: boolean = false;

  public gettingAllHistories: boolean = false;
  public gettingGeneralHistory: boolean = false;
  public gettingSelectedHistory: boolean = false;
  public gettingProfile: boolean = false;


  public async ngOnInit() {
    // Boolean subscribes
    this.profileService._gettingProfile.subscribe((status: boolean) => {
      this.gettingProfile = status;
    });
    this.profileService._gettingGeneralHistory.subscribe((status: boolean) => {
      this.gettingGeneralHistory = status;
    });
    this.profileService._gettingHistories.subscribe((status: boolean) => {
      this.gettingAllHistories = status;
    });
    this.profileService._gettingSelectedHistory.subscribe((status: boolean) => {
      this.gettingSelectedHistory = status;
    });
    // Profile service subscribe
    this.profileService.profile.subscribe((profile: Profile) => {
      this.profile = profile;
    });
    
    // Auth profile
    this.auth.userProfile$.subscribe(data => {
      this.authProfile = data;
    });

    // Chat histories subscribe
    this.profileService.chatHistories.subscribe((histories: Array<MessageHistory>) => {
      this.histories = histories;
    });

    // Selected history subscribe
    this.profileService.selectedHistory.subscribe((selectedHistory: MessageHistory) => {
      this.selectedHistory = selectedHistory;
    });

    // General history subscribe
    this.profileService.generalHistory.subscribe((generalHistory: MessageHistory) => {
      this.generalHistory = generalHistory;
    });

    // Events subscribe
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray
      
    });

    await this.profileService.getProfile(this.authProfile.email).catch(err => {console.log(err)});
    await this.gatherer.gatherAllEvents();
    await this.profileService.getHistories(this.profile.clientID);
    await this.profileService.gatherGeneralHistory(this.profile.clientID)
  }
  public async showSelectedChat(history: MessageHistory)
  {
    this.showOverlay = true;
    this.showChat = true;

    if(history == null)
    {
      await this.profileService.getSelectedHistory(this.profile.clientID, null);
      setTimeout(() => this.chatComponent.scrollToBottom(), 0);
      
    }
    else
    {
      await this.profileService.getSelectedHistory(this.profile.clientID, history.relationXrefID);
      setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    }
  }
  public hideWindow()
  {
    if(!this.chatComponent.markingRead)
    {
      this.selectedHistory = new MessageHistory();
      this.profileService.getHistories(this.profile.clientID);
      this.showChat = false;
      this.showOverlay = false;
    }
  }
  public async send(messageResponse: MessageApiResponse)
  {
    this.postingMessage = true;

    await this.SantaApiPost.postMessage(messageResponse).toPromise();
    await this.profileService.getSelectedHistory(this.profile.clientID, this.selectedHistory.relationXrefID, true);

    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.postingMessage = false;
  }
  public async readAll()
  {
    this.puttingMessage = true;

    let unreadMessages: Array<Message> = this.selectedHistory.history.filter((message: Message) => { return message.isMessageRead == false && message.recieverClient.clientID == this.profile.clientID });
    
    let response: MessageApiReadAllResponse = new MessageApiReadAllResponse();
    unreadMessages.forEach((message: Message) => { response.messages.push(message.chatMessageID)});

    await this.SantaApiPut.putMessageReadAll(response).toPromise().catch((err) => {console.log(err)});
    await this.profileService.getSelectedHistory(this.profile.clientID, this.selectedHistory.relationXrefID, true);

    setTimeout(() => this.chatComponent.scrollToBottom(), 0);

    this.puttingMessage = false;

    
  }
  public softRefreshSelectedChat(isSoftUpdate: boolean)
  {
    this.profileService.getSelectedHistory(this.profile.clientID, this.selectedHistory.relationXrefID, isSoftUpdate)
  }
}
