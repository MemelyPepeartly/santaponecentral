import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef, SecurityContext } from '@angular/core';
import { Message, MessageHistory, ClientMeta } from 'src/classes/message';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/profile.service';
import { MessageApiReadResponse } from 'src/classes/responseTypes';
import { SantaApiPutService } from 'src/app/services/santa-api.service';
import { MapResponse, MapService } from 'src/app/services/mapper.service';
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
    public ApiMapper: MapService,
    public auth: AuthService) { }

  // Boolean value for passing whether or not the emit is a soft update or not
  @Output() messageUpdatedEvent: EventEmitter<Message> = new EventEmitter<Message>();
  @Output() historyUpdatedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() manualRefreshClickedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() openAgentControlEvent: EventEmitter<ClientMeta> = new EventEmitter<ClientMeta>();

  @Input() selectedHistory: MessageHistory = new MessageHistory();
  @Input() sendingClientMeta: ClientMeta = new ClientMeta();
  @Input() onProfile: boolean
  @Input() showLoading: boolean = false;
  @Input() showActionProgressBar: boolean = false;
  @Input() refreshing: boolean = false;
  @Input() showControlButton: boolean = false;

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
    let newMessage: Message = this.ApiMapper.mapMessage(await this.SantaApiPut.putMessageReadStatus(message.chatMessageID, putMessage).toPromise().catch((err) => {console.log(err);}));

    this.markingRead = false;

    var messageIndex = undefined;
    if(newMessage.subjectMessage)
    {
      messageIndex = this.selectedHistory.subjectMessages.findIndex((message: Message) => {return message.chatMessageID == newMessage.chatMessageID});
      this.selectedHistory.subjectMessages[messageIndex] = newMessage
    }
    else
    {
      messageIndex = this.selectedHistory.recieverMessages.findIndex((message: Message) => {return message.chatMessageID == newMessage.chatMessageID});
      this.selectedHistory.recieverMessages[messageIndex] = newMessage
    }
    this.selectedHistory.unreadCount -= 1;

    this.historyUpdatedEvent.emit(this.selectedHistory);
    this.messageUpdatedEvent.emit(newMessage);
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
  public emitAgentControlClicked()
  {
    this.openAgentControlEvent.emit(this.selectedHistory.assignmentRecieverClient)
  }
}
