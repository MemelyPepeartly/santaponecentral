import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Message } from 'src/classes/message';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { SantaApiPostService } from 'src/app/services/santaApiService.service';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit {

  constructor(public SantaApiPost: SantaApiPostService, public responseMapper: MapResponse, public auth: AuthService) { }

  @Input() chat: Array<Message>;
  @Input() selectedChatHistory: ProfileRecipient;
  @Input() profile: Profile;
  public isAdmin: boolean;

  @Output() messagePosted: EventEmitter<boolean> = new EventEmitter<boolean>();

  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
  }

  public async memberPostTest()
  {
    var newMessageResponse = this.responseMapper.mapMessageResponse(this.selectedChatHistory, "I am a member posting", this.profile.clientID, null)
    var res = await this.SantaApiPost.postMessage(newMessageResponse).toPromise();
    this.messagePosted.emit(true);   
  }
  public async adminPostTest()
  {
    var newMessageResponse = this.responseMapper.mapMessageResponse(this.selectedChatHistory, "I am an admin posting", null, this.profile.clientID)
    var res = await this.SantaApiPost.postMessage(newMessageResponse).toPromise();
    this.messagePosted.emit(true);   
  }

}
