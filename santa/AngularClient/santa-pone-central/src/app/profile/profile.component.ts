import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SantaApiGetService, SantaApiPostService } from '../services/santaApiService.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { ProfileService } from '../services/Profile.service';
import { MessageApiResponse } from 'src/classes/responseTypes';

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

  public profile: Profile = new Profile();
  public authProfile: any;

  public selectedRecipient: ProfileRecipient;
  public selectedHistory: MessageHistory;

  public histories: Array<MessageHistory>;
  public adminRecieverMeta: ClientMeta = new ClientMeta;

  public showChat: boolean = false;

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
    this.profileService.getHistories(this.profile.clientID);
  }
  public showSelectedChat()
  {
    this.showChat = true;
  }
  public hideSelectedChat()
  {
    this.showChat = false;
  }
  public async send(messageResponse: MessageApiResponse)
  {
    await this.SantaApiPost.postMessage(messageResponse).toPromise();
    this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
  }
  public async updateChat(event: boolean)
  {
    if(event)
    {
      this.profileService.getSelectedHistory(this.selectedHistory.conversationClient.clientID, this.selectedHistory.relationXrefID);
      this.profileService.getHistories(this.profile.clientID);
    }
  }
}
