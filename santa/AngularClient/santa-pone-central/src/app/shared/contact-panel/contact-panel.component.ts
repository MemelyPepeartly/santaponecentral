import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { Message, MessageHistory, ClientMeta } from 'src/classes/message';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/Profile.service';
import { MessageApiReadResponse } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { MapResponse } from 'src/app/services/mapService.service';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit, AfterViewChecked {

  constructor(public SantaApiPut: SantaApiPutService, public responseMapper: MapResponse, public auth: AuthService) { }

  @Output() messageUpdatedEvent: EventEmitter<boolean> = new EventEmitter<any>();

  @Input() selectedHistory: MessageHistory;
  @Input() sendingClientMeta: ClientMeta;
  @Input() showLoading: boolean = false;
  
  @ViewChild('chatFrame', {static: false}) chatFrame: ElementRef;

  public isAdmin: boolean;
  public markingRead: boolean = false;


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
  public async markRead(message: Message)
  {
    this.markingRead = true;
    let putMessage = new MessageApiReadResponse();

    putMessage.isMessageRead = true;
    this.SantaApiPut.putMessageReadStatus(message.chatMessageID, putMessage).subscribe(() => {
      this.markingRead = false;
      this.messageUpdatedEvent.emit(true);
    },err => {console.log(err)});
  }
}
