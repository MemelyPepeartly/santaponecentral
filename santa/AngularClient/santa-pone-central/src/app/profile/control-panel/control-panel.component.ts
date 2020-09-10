import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { MessageHistory, ClientMeta } from 'src/classes/message';
import { SantaApiGetService } from 'src/app/services/santaApiService.service';
import { AuthService } from 'src/app/auth/auth.service';
import { MapService } from 'src/app/services/mapService.service';
import { ProfileService } from 'src/app/services/Profile.service';
import { Survey } from 'src/classes/survey';

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

  @Input() gettingAllHistories: boolean;
  @Input() gettingGeneralHistory: boolean;
  @Input() gettingSelectedHistory: boolean;
  @Input() gettingProfile: boolean;

  @Input() loading: boolean;
  @Input() histories: Array<MessageHistory> = []
  @Input() generalHistory: MessageHistory = new MessageHistory();
  @Input() profile: Profile;
  @Input() surveys: Array<Survey>;

  @Output() chatClickedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();

  public selectedRecipient: ProfileRecipient;

  public isAdmin: boolean;
  public initializing: boolean;

  public showRecipientData: boolean = false;


  ngOnInit(): void {
    this.initializing = true;
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
      this.initializing = false;
    });
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
  public async historySelected(history: MessageHistory)
  {
    this.chatClickedEvent.emit(history);
  }
}
