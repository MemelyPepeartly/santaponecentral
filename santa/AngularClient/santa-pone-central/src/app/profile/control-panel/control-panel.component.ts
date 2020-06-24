import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Client } from 'src/classes/client';
import { ProfileRecipient, Profile } from 'src/classes/profile';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-control-panel',
  templateUrl: './control-panel.component.html',
  styleUrls: ['./control-panel.component.css']
})
export class ControlPanelComponent implements OnInit {

  constructor(private gatherer: GathererService) { }

  @Input() recipients: Array<ProfileRecipient> = []
  @Output() selectedRecipientContactHistoryEvent: EventEmitter<ProfileRecipient> = new EventEmitter();
  columns: string[] = ["recipient", "event", "contact"];

  ngOnInit(): void {
  }

  public openContactHistory(recipient: ProfileRecipient)
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
