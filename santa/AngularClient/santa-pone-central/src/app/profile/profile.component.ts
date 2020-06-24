import { Component, OnInit } from '@angular/core';
import { Client } from 'src/classes/client';
import { SantaApiGetService } from '../services/santaApiService.service';
import { GathererService } from '../services/gatherer.service';
import { MapService } from '../services/mapService.service';
import { AuthService } from '../auth/auth.service';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { Message } from 'src/classes/message';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public gatherer: GathererService, 
    public SantaApiGet: SantaApiGetService,
    public auth: AuthService,
    public ApiMapper: MapService) { }

  public profile: Profile = new Profile();
  public chat: Array<Message> = [];
  public selectedChatHistory: ProfileRecipient;
  public selectedRecipient: ProfileRecipient;
  
  public showRecipientData: boolean = false;

  public async ngOnInit() {
    var data = this.auth.userProfile$.subscribe(async data => {
      this.profile = this.ApiMapper.mapProfile(await this.SantaApiGet.getProfile(data.email).toPromise());
    });
  }
  public async populateChat(recipient: ProfileRecipient)
  {
    this.chat = [];
    this.selectedChatHistory = recipient;

    var res = await this.SantaApiGet.getMessageHistoryByClientIDAndXrefID(this.profile.clientID, recipient.relationXrefID).subscribe(res => {
      res.forEach(message => {
        this.chat.push(this.ApiMapper.mapMessage(message)); 
      });
    });
  }
  public showRecipientCard(recipient: ProfileRecipient)
  {
    this.selectedRecipient = recipient;
    this.showRecipientData = true;
  }
  public hideRecipientCard()
  {
    this.selectedRecipient = undefined;
    this.showRecipientData = false;
  }
  public async refreshMessages(newMessagePosted: boolean)
  {
    if(newMessagePosted)
    {
      await this.populateChat(this.selectedChatHistory)
    }
  }
}
