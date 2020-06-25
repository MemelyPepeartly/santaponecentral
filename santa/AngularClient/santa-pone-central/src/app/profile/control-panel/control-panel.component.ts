import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { Client } from 'src/classes/client';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';
import { Message, MessageHistory } from 'src/classes/message';
import { SantaApiGetService } from 'src/app/services/santaApiService.service';
import { AuthService } from 'src/app/auth/auth.service';
import { MapService } from 'src/app/services/mapService.service';
import { BehaviorSubject } from 'rxjs';
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
  @Input() profile: Profile

  @Output() selectedRecipientContactHistoryEvent: EventEmitter<ProfileRecipient> = new EventEmitter();
  @Output() selectedRecipientInformationEvent: EventEmitter<ProfileRecipient> = new EventEmitter();


  ngOnInit(): void {
  }
  public log()
  {
    console.log(this.histories);
    this.profileService.getHistories(this.profile.clientID);
  }
  public openInformation(recipient: ProfileRecipient)
  {
    this.selectedRecipientInformationEvent.emit(recipient)
  }

  public selectContactHistory(recipient: ProfileRecipient)
  {
    if(recipient != null)
    {
      this.selectedRecipientContactHistoryEvent.emit(recipient);
    }
    else
    {
      let blankRecipient = new ProfileRecipient;
      this.selectedRecipientContactHistoryEvent.emit(blankRecipient)
    }
  }
}
