import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { SantaApiGetService } from 'src/app/services/santaApiService.service';
import { AuthService } from 'src/app/auth/auth.service';
import { MapService } from 'src/app/services/mapService.service';
import { ProfileService } from 'src/app/services/Profile.service';

@Component({
  selector: 'app-control-panel',
  templateUrl: './control-panel.component.html',
  styleUrls: ['./control-panel.component.css']
})
export class ControlPanelComponent implements OnInit {

  constructor(public ApiMapper: MapService,
    public SantaApiGet: SantaApiGetService,
    public auth: AuthService,
    public profileService: ProfileService) { }
    
  @Input() histories: Array<MessageHistory>
  @Input() generalHistory: MessageHistory;
  @Input() profile: Profile;

  @Output() chatClickedEvent: EventEmitter<any> = new EventEmitter<any>();

  public selectedRecipient: ProfileRecipient;

  public showRecipientData: boolean = false;


  ngOnInit(): void {
  }
  // Event is from the chat histories component, and contains {meta: ClientMeta, event: EventType}
  public showRecipientCard(eventInformation)
  {
    this.selectedRecipient = this.getProfileRecipientByMetaAndEventID(eventInformation.meta, eventInformation.event.eventTypeID);
    this.showRecipientData = true;
  }
  public hideRecipientCard()
  {
    this.selectedRecipient = undefined;
    this.showRecipientData = false;
  }
  public getProfileRecipientByMetaAndEventID(meta: ClientMeta, eventID)
  {
    let profileRecipient = this.profile.recipients.find((recipient: ProfileRecipient) => {
      return recipient.clientID == meta.clientID && recipient.recipientEvent.eventTypeID == eventID;
    });
    return profileRecipient;
  }
  public getSelectedHistoryMessages(history: MessageHistory)
  {
    if(history != null)
    {
      this.profileService.getSelectedHistory(this.profile.clientID, history.relationXrefID);
      this.chatClickedEvent.emit(true);
    }
    else
    {
      this.profileService.getSelectedHistory(this.profile.clientID, null);
      this.chatClickedEvent.emit(true);
    }
  }
  public getGeneralUnreadNumber()
  {
    return this.histories.find((history: MessageHistory) => {
      return history.eventSenderClient.clientID == null && history.eventRecieverClient.clientID == null;
    }).memberUnreadCount
  }
  public checkBadgeHidden()
  {
    if(this.histories.find((history: MessageHistory) => { return history.eventSenderClient.clientID == null && history.eventRecieverClient.clientID == null; }).memberUnreadCount == 0)
    {
      return true;
    }
    else 
    {
      return false;
    }
  }
}
