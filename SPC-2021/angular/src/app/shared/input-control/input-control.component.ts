import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Message, ClientMeta, MessageHistory, ChatInfoContainer } from 'src/classes/message';
import { MessageApiResponse } from 'src/classes/request-types';
import { BaseClient, Client } from 'src/classes/client';
import { InputControlConstants } from 'src/app/shared/constants/inputControlConstants.enum';
import { AuthService } from 'src/app/auth/auth.service';
import { SantaApiGetService } from 'src/app/services/santa-api.service';
import { MapService } from 'src/app/services/utility services/mapper.service';
import { EventType } from 'src/classes/eventType';


@Component({
  selector: 'app-input-control',
  templateUrl: './input-control.component.html',
  styleUrls: ['./input-control.component.css']
})
export class InputControlComponent implements OnInit {

  constructor(public Auth: AuthService, public SantaApiGet: SantaApiGetService, public Mapper: MapService) { }

  @Output() sendClicked: EventEmitter<MessageApiResponse> = new EventEmitter<MessageApiResponse>();
  @Output() readAllAction: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() displayPinnedAction: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() timeZoneAction: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() colorAction: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() chatInfoContainer: ChatInfoContainer = new ChatInfoContainer();
  @Input() disabled: boolean = false;
  @Input() menuDisabled: boolean = false;

  private isAdmin: boolean;
  private profile: any;
  private adminClient: BaseClient;

  public messageFormControl = new FormControl('', [Validators.required, Validators.maxLength(1000)]);

  ngOnInit(): void {
    this.Auth.userProfile$.subscribe(data => {
      this.profile = data;
    });
    this.Auth.isAdmin.subscribe(async (admin: boolean) => {
      this.isAdmin = admin;
      if(this.isAdmin)
      {
        this.adminClient = this.Mapper.mapBaseClient(await this.SantaApiGet.getBasicClientByEmail(this.profile.email).toPromise());
      }
    });
  }
  public emitMessage(message: string)
  {

    let newMessage: MessageApiResponse =
    {
      messageSenderClientID: this.chatInfoContainer.messageSenderID,
      messageRecieverClientID: this.chatInfoContainer.messageRecieverID,
      clientRelationXrefID: this.chatInfoContainer.relationshipXrefID,
      eventTypeID: this.chatInfoContainer.relationshipXrefID == null || this.chatInfoContainer.relationshipXrefID == undefined ? null : this.chatInfoContainer.eventTypeID,
      messageContent: message,
      fromAdmin: this.chatInfoContainer.senderIsAdmin,
    };

    this.sendClicked.emit(newMessage);
  }
  public emitAction(action: InputControlConstants)
  {
    if(action == InputControlConstants.READALL)
    {
      this.readAllAction.emit(true);
    }
    else if(action == InputControlConstants.PINNED)
    {
      this.readAllAction.emit(true);
    }
    else if(action == InputControlConstants.TIMEZONE)
    {
      this.readAllAction.emit(true);
    }
    else if(action == InputControlConstants.COLOR)
    {
      this.readAllAction.emit(true);
    }
  }
  public clearForm()
  {
    this.messageFormControl.reset();
  }
  public getErrorMessage() {
    if (this.messageFormControl.hasError('required')) {
      return 'You must enter a value';
    }
    if (this.messageFormControl.hasError('maxlength')) {
      return 'Max length of a message is 1000 characters';
    }
    return 'Something is wrong with your message'
  }
}
