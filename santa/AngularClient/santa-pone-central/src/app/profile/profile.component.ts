import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Client } from 'src/classes/client';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { Message, MessageHistory } from 'src/classes/message';
import { ContactComponent } from '../contact/contact.component';
import { ContactPanelComponent } from './contact-panel/contact-panel.component';
import { ProfileService } from '../services/Profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public profileService: ProfileService, 
    public SantaApiGet: SantaApiGetService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  public profile: Profile = new Profile();
  public authProfile: any;
  public selectedRecipient: ProfileRecipient;
  public selectedHistory: MessageHistory;
  public histories: Array<MessageHistory>;

  public async ngOnInit() {
    //Auth profile
    this.auth.userProfile$.subscribe(data => {
      this.authProfile = data;
    });

    // Profile service subscribe
    this.profileService.profile.subscribe((profile: Profile) => {
      this.profile = profile;
    });

    // Chat histories subscribe
    this.profileService.chatHistories.subscribe((histories: Array<MessageHistory>) => {
      this.histories = histories;
    });
    
    // Selected history subscribe
    this.profileService.selectedHistory.subscribe((selectedHistory: MessageHistory) => {
      this.selectedHistory = selectedHistory;
    });
    this.profileService.getProfile(this.authProfile.name).then(() => {
      this.profileService.getHistories(this.profile.clientID);
    });
    
  }
}
