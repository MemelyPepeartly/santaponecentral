import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef, SecurityContext } from '@angular/core';
import { Message, MessageHistory, ClientMeta } from 'src/classes/message';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/Profile.service';
import { MessageApiReadResponse } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { MapResponse } from 'src/app/services/mapService.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit{

  constructor(
    protected sanitizer: DomSanitizer,
    public SantaApiPut: SantaApiPutService,
    public responseMapper: MapResponse,
    public auth: AuthService) { }

  // Boolean value for passing whether or not the emit is a soft update or not
  @Output() messageUpdatedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() manualRefreshClickedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() selectedHistory: MessageHistory = new MessageHistory();
  @Input() sendingClientMeta: ClientMeta = new ClientMeta();
  @Input() onProfile: boolean
  @Input() showLoading: boolean = false;
  @Input() showActionProgressBar: boolean = false;
  @Input() refreshing: boolean = false;

  @ViewChild('chatFrame', {static: false}) chatFrame: ElementRef;

  public isAdmin: boolean;
  public markingRead: boolean = false;

  ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });

  }
  public totalHistory() : Array<Message>
  {
    let allMessages: Array<Message> = []
    this.selectedHistory.recieverMessages.forEach((message: Message) => {
      allMessages.push(message);
    });
    this.selectedHistory.subjectMessages.forEach((message: Message) => {
      allMessages.push(message);
    });
    return allMessages.sort((a: Message, b: Message) => {
      return a.dateTimeSent.getTime() - b.dateTimeSent.getTime();
    });
  }
  spotURL(text: string) {
    var urlRegex = /(https?:\/\/[^\s]+)/g;
    return this.sanitizer.bypassSecurityTrustHtml(text.replace(urlRegex, function(url) {
      return '<a style="color: #123765;" href="' + url + '"target="_blank">' + url + '</a>';
    }))
  }
  parseElement(htmlString: string)
  {
    let parser = new DOMParser();
    return parser.parseFromString(htmlString, 'text/html');
  }
  public scrollToBottom(): void {
    try {
        this.chatFrame.nativeElement.scrollTop = this.chatFrame.nativeElement.scrollHeight;
    } catch(err) { }
  }
  public async manualRefreshChat()
  {
    // Lets any parent components know that the user clicked manual refresh
    this.manualRefreshClickedEvent.emit(true);
  }
  public async markRead(message: Message)
  {
    this.markingRead = true;

    let putMessage = new MessageApiReadResponse();

    putMessage.isMessageRead = true;
    await this.SantaApiPut.putMessageReadStatus(message.chatMessageID, putMessage).toPromise().catch((err) => {console.log(err);});

    this.markingRead = false;
    this.messageUpdatedEvent.emit(true);
  }
  public showRead(message: Message) : boolean
  {
    return (message.isMessageRead && message.senderClient.clientID != this.selectedHistory.subjectClient.clientID && this.isAdmin && !message.fromAdmin) ||
    (!this.isAdmin && message.fromAdmin && this.onProfile && message.isMessageRead)
  }
  public showButton(message: Message) : boolean
  {
    return (!message.isMessageRead && !message.subjectMessage && this.isAdmin && !message.fromAdmin) ||
    (!this.isAdmin && message.fromAdmin && this.onProfile && !message.isMessageRead)
  }
}
