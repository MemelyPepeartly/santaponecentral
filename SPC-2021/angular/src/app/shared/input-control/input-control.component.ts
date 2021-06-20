import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ChatInfoContainer } from 'src/classes/message';
import { BaseClient } from 'src/classes/client';
import { InputControlConstants } from 'src/app/shared/constants/inputControlConstants.enum';
import { MapService } from 'src/app/services/utility services/mapper.service';
import { ClientService } from 'src/app/services/api services/client.service';
import { AddMessageRequest } from 'src/classes/request-types';
import { AuthService } from '@auth0/auth0-angular';


@Component({
  selector: 'app-input-control',
  templateUrl: './input-control.component.html',
  styleUrls: ['./input-control.component.css']
})
export class InputControlComponent implements OnInit {

  constructor(public Auth: AuthService, public ClientService: ClientService, public Mapper: MapService) { }

  @Output() sendClicked: EventEmitter<AddMessageRequest> = new EventEmitter<AddMessageRequest>();
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
    this.Auth.user$.subscribe(data => {
      this.profile = data;
    });
    /* SANTAHERE Fix this come the time
    this.Auth.isAdmin.subscribe(async (admin: boolean) => {
      this.isAdmin = admin;
      if(this.isAdmin)
      {
        this.adminClient = this.Mapper.mapBaseClient(await this.ClientService.getBasicClientByEmail(this.profile.email).toPromise());
      }
    });
    */ 
  }
  public emitMessage(message: string)
  {

    let newMessage: AddMessageRequest =
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
