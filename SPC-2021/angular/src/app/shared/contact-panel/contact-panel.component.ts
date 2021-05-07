import { Component, OnInit, Input, EventEmitter, Output, ViewChild, ElementRef, SecurityContext } from '@angular/core';
import { Message, MessageHistory, ClientMeta, ChatInfoContainer } from 'src/classes/message';
import { AuthService } from 'src/app/auth/auth.service';
import { ProfileService } from 'src/app/services/gathering services/profile-gathering.service';
import { MessageApiReadResponse } from 'src/classes/responseTypes';
import { SantaApiGetService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { MapResponse, MapService } from 'src/app/services/utility services/mapper.service';
import { DomSanitizer } from '@angular/platform-browser';
import { NgScrollbar } from 'ngx-scrollbar';
import { SmoothScroll, SMOOTH_SCROLL_OPTIONS } from 'ngx-scrollbar/smooth-scroll';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit{

  constructor(
    protected sanitizer: DomSanitizer,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public responseMapper: MapResponse,
    public ApiMapper: MapService,
    public auth: AuthService) { }

  /** Dictates if the chat window is on the profile component */
  @Input() onProfile: boolean
  /**  Dictates if the loading shark is present and other elements are not shown yet */
  @Input() showLoading: boolean = false;
  /** Dictates if other elements are shown, but the action in progress bar along the top is on or off */
  @Input() showActionProgressBar: boolean = false;
  /** Dictates if the chat is being refreshed. This activates the action progress bar, but also disables the refresh button */
  @Input() refreshing: boolean = false;
  /** Dictates if the button to switch to a client profile is viewable */
  @Input() showControlButton: boolean = false;
  /** Informational container of the chat. This contains the ID's to show what is sending to where */
  @Input() chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();

  /** A message was updated */
  @Output() messageUpdatedEvent: EventEmitter<Message> = new EventEmitter<Message>();
  /** A history was updated */
  @Output() historyUpdatedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  /** Manual refresh was clicked */
  @Output() manualRefreshClickedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  /** The agent control button was clicked */
  @Output() openAgentControlEvent: EventEmitter<ClientMeta> = new EventEmitter<ClientMeta>();
  /** The chat is refreshing */
  @Output() refreshingStatusEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  /** The chat is loading */
  @Output() loadingStatusEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('chatFrame', {static: false}) chatFrame: ElementRef;
  @ViewChild(NgScrollbar) scrollbarRef: NgScrollbar;

  public isAdmin: boolean;
  public markingRead: boolean = false;

  public messageHistory: MessageHistory = new MessageHistory();

  ngOnInit(): void {
    // Emits both for the sake of being in OnInit
    this.refreshingStatusEvent.emit(true);
    this.loadingStatusEvent.emit(true);

    this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(this.chatInfoContainer.conversationClientID, this.chatInfoContainer.messageSenderID, this.chatInfoContainer.relationshipXrefID).subscribe((historyRes) => {
      this.messageHistory = this.ApiMapper.mapMessageHistory(historyRes);
      this.refreshingStatusEvent.emit(false);
      this.loadingStatusEvent.emit(false);
      setTimeout(()=> this.scrollToBottom() ,0);
    }, err => {
      this.refreshingStatusEvent.emit(false);
      this.loadingStatusEvent.emit(false);
      console.group();
      console.log("An error has occured getting the history for the chat window!");
      console.log(err);
      console.groupEnd();
    });
  }
  public totalHistory() : Array<Message>
  {
    let allMessages: Array<Message> = []
    this.messageHistory.recieverMessages.forEach((message: Message) => {
      allMessages.push(message);
    });
    this.messageHistory.subjectMessages.forEach((message: Message) => {
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
    this.scrollbarRef.scrollTo({ bottom: 0, duration: 0 });
  }
  public manualRefreshChat(isSoftRefresh: boolean = false)
  {
    // Lets any parent components know that the user clicked manual refresh and emits the loading status
    this.manualRefreshClickedEvent.emit(true);
    this.refreshingStatusEvent.emit(!isSoftRefresh);
    if(!isSoftRefresh)
    {
      this.loadingStatusEvent.emit(true);
    }


    this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(this.chatInfoContainer.conversationClientID, this.chatInfoContainer.messageSenderID, this.chatInfoContainer.relationshipXrefID).subscribe((history) => {
      this.messageHistory = this.ApiMapper.mapMessageHistory(history);

      this.historyUpdatedEvent.emit(this.messageHistory);
      this.refreshingStatusEvent.emit(false);
      this.loadingStatusEvent.emit(false);
      setTimeout(()=> this.scrollToBottom() ,0);
    }, err => {
      this.refreshingStatusEvent.emit(false);
      this.loadingStatusEvent.emit(false);

      console.group();
      console.log("An error has occured getting the history for the chat window!");
      console.log(err);
      console.groupEnd();
    })
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
      messageIndex = this.messageHistory.subjectMessages.findIndex((message: Message) => {return message.chatMessageID == newMessage.chatMessageID});
      this.messageHistory.subjectMessages[messageIndex] = newMessage
    }
    else
    {
      messageIndex = this.messageHistory.recieverMessages.findIndex((message: Message) => {return message.chatMessageID == newMessage.chatMessageID});
      this.messageHistory.recieverMessages[messageIndex] = newMessage
    }
    this.messageHistory.unreadCount -= 1;

    this.historyUpdatedEvent.emit(this.messageHistory);
    this.messageUpdatedEvent.emit(newMessage);
  }
  public showRead(message: Message) : boolean
  {
    return message.isMessageRead && !message.subjectMessage
  }
  public showButton(message: Message) : boolean
  {
    return !message.isMessageRead && !message.subjectMessage
  }
  public emitAgentControlClicked()
  {
    this.openAgentControlEvent.emit(this.messageHistory.assignmentRecieverClient)
  }
}
