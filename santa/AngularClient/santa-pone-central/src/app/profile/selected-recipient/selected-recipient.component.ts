import { Component, OnInit, Input } from '@angular/core';
import { ProfileRecipient } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';
import { Survey } from 'src/classes/survey';
import { GathererService } from 'src/app/services/gatherer.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-selected-recipient',
  templateUrl: './selected-recipient.component.html',
  styleUrls: ['./selected-recipient.component.css']
})
export class SelectedRecipientComponent implements OnInit {

  constructor(public profileService: ProfileService,
    public gatherer: GathererService) { }

  @Input() selectedRecipient: ProfileRecipient;
  public events: Array<EventType> = []

  ngOnInit(): void {
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray;
    });
    this.gatherer.gatherAllEvents();
  }

}
