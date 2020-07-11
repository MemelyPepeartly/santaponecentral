import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SantaApiGetService, SantaApiPostService } from '../services/santaApiService.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { ProfileService } from '../services/Profile.service';
import { MessageApiResponse } from 'src/classes/responseTypes';
import { ContactPanelComponent } from '../shared/contact-panel/contact-panel.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public profileService: ProfileService,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPost: SantaApiPostService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  @ViewChild(ContactPanelComponent) chatComponent: ContactPanelComponent;

  public profile: Profile = new Profile();
  public authProfile: any;

  public selectedRecipient: ProfileRecipient = new ProfileRecipient();
  public selectedHistory: MessageHistory = new MessageHistory();
  public generalHistory: MessageHistory = new MessageHistory();

  public histories: Array<MessageHistory>;
  public adminRecieverMeta: ClientMeta = new ClientMeta;

  public showOverlay: boolean = false;
  public showChat: boolean = false;
  public showAddressRequest: boolean = false;
  public showContactRequest: boolean = false;

  public postingMessage: boolean = false;
  public gettingAnyHistories: boolean = false;



  public async ngOnInit() {
    //Auth profile
    this.auth.userProfile$.subscribe(data => {
      this.authProfile = data;
    });

    // Profile service subscribe
    this.profileService.profile.subscribe((profile: Profile) => {
      this.profile = profile;
    });
    await this.profileService.getProfile(this.authProfile.name).catch(err => {console.log(err)});

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

    this.profileService.getHistories(this.profile.clientID);
  }
  public showSelectedChat()
  {
    this.showOverlay = true;
    this.showChat = true;
  }
  public hideWindow()
  {
    if(this.chatComponent == undefined)
    {
      this.showContactRequest = false;
      this.showAddressRequest = false;
      this.showOverlay = false;
    }
    else if(!this.chatComponent.markingRead)
    {
      this.showChat = false;
      this.showOverlay = false;
    }
  }
  public async send(messageResponse: MessageApiResponse)
  {
    this.postingMessage = true;

    await this.SantaApiPost.postMessage(messageResponse).toPromise();
    await this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
    
    this.postingMessage = false;
  }
  public async updateChat(event: boolean)
  {
    if(event)
    {
      this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
      this.profileService.getHistories(this.profile.clientID, true);
    }
  }
  public scrollTheChat(isUpdateScroll?: boolean)
  {
    if(isUpdateScroll)
    {
      this.chatComponent.scrollToBottom();
    }
  }
  public sendContactRequest()
  {
    this.showContactRequest = true;
    this.showOverlay = true;
    console.log("Contact request");
  }
  public sendAddressRequest()
  {
    this.showAddressRequest = true;
    this.showOverlay = true;
    console.log("Address request");
  }
}
