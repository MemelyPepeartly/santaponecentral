import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { Message, MessageHistory, ClientMeta } from 'src/classes/message';
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
export class ContactPanelComponent implements OnInit, AfterViewChecked {

  constructor(public SantaApiPost: SantaApiPostService, public responseMapper: MapResponse, public auth: AuthService, public profileService: ProfileService) { }

  @Input() selectedHistory: MessageHistory;
  @Input() clientMeta: ClientMeta;
  @Input() showLoading: boolean = false;
  
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
  
  public async scrollToBottom()
  {
    try 
    {
      this.chatFrame.nativeElement.scrollTop = this.chatFrame.nativeElement.scrollHeight;
    }
    catch(err) { }
  }
}
