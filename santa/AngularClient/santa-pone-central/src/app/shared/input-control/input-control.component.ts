import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Message, ClientMeta, MessageHistory } from 'src/classes/message';
import { MessageApiResponse } from 'src/classes/responseTypes';
import { Client } from 'src/classes/client';
import { InputControlConstants } from 'src/app/shared/constants/inputControlConstants.enum';
import { AuthService } from 'src/app/auth/auth.service';
import { SantaApiGetService } from 'src/app/services/santaApiService.service';
import { MapService } from 'src/app/services/mapService.service';
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

  @Input() relationshipID: string;
  @Input() sender: ClientMeta;
  @Input() reciever: ClientMeta;
  @Input() disabled: boolean = false;
  @Input() onProfile: boolean;
  @Input() eventType: EventType = new EventType()

  private isAdmin: boolean;
  private profile: any;
  private adminClient: Client;

  public messageFormControl = new FormControl('', [Validators.required, Validators.maxLength(1000)]);

  ngOnInit(): void {
    this.Auth.userProfile$.subscribe(data => {
      this.profile = data;
    });
    this.Auth.isAdmin.subscribe(async (admin: boolean) => {
      this.isAdmin = admin;
      if(this.isAdmin)
      {
        this.adminClient = this.Mapper.mapClient(await this.SantaApiGet.getClientByEmail(this.profile.email).toPromise());
      }
    });
  }
  public emitMessage(message: string)
  {
    
    let newMessage = new MessageApiResponse();
    newMessage.messageContent = message;
    newMessage.clientRelationXrefID = this.relationshipID;
    newMessage.messageSenderClientID = this.isAdmin ? this.adminClient.clientID : this.sender.clientID;
    newMessage.messageRecieverClientID = this.reciever.clientID;
    newMessage.eventTypeID = this.relationshipID == null || undefined ? null : this.eventType.eventTypeID;
    newMessage.fromAdmin = this.onProfile ? false : this.isAdmin;
    console.log(newMessage);
    

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
