import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { Message, MessageHistory } from 'src/classes/message';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { SantaApiPostService } from 'src/app/services/santaApiService.service';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/Profile.service';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit {

  constructor(public SantaApiPost: SantaApiPostService, public responseMapper: MapResponse, public auth: AuthService, public profileService: ProfileService) { }

  @Input() selectedHistory: MessageHistory;
  @Input() selectedProfileRecipient: ProfileRecipient;
  @Input() profile: Profile;

  @Output() messagePosted: EventEmitter<boolean> = new EventEmitter<boolean>();
  
  @ViewChild('chatFrame', {static: false}) chatFrame: ElementRef;

  public isAdmin: boolean;


  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
  }
  ngAfterViewChecked() {        
    this.scrollToBottom();      
  } 

  public async memberPostTest()
  {
    let test = new ProfileRecipient();
    var newMessageResponse = this.responseMapper.mapMessageResponse(test, "I am a member posting", this.profile.clientID, null)
    var res = await this.SantaApiPost.postMessage(newMessageResponse).toPromise();
    this.messagePosted.emit(true);
  }
  public async adminPostTest()
  {
    var newMessageResponse = this.responseMapper.mapMessageResponse(this.selectedProfileRecipient, "I am an admin posting", null, this.profile.clientID)
    var res = await this.SantaApiPost.postMessage(newMessageResponse).toPromise();
    this.messagePosted.emit(true); 
  }
  public async scrollToBottom()
  {
    try 
    {
      this.chatFrame.nativeElement.scrollTop = this.chatFrame.nativeElement.scrollHeight;
    }
    catch(err) { }
  }
}
