import { Component, Input, OnInit } from '@angular/core';
import { GathererService } from 'src/app/services/gatherer.service';
import { Client, RelationshipMeta } from 'src/classes/client';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-client-assignment-info',
  templateUrl: './client-assignment-info.component.html',
  styleUrls: ['./client-assignment-info.component.css']
})
export class ClientAssignmentInfoComponent implements OnInit {

  constructor(public gatherer: GathererService) { }

  @Input() client: Client = new Client();

  public events: Array<EventType> = [];

  async ngOnInit() {
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {

    });
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray
    });
    await this.gatherer.gatherAllEvents();
  }

  public getEventSenders(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.senders.filter((sender: RelationshipMeta) => {return sender.eventType.eventTypeID == eventType.eventTypeID});
  }
  public getEventAssignments(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.assignments.filter((assignment: RelationshipMeta) => {return assignment.eventType.eventTypeID == eventType.eventTypeID});
  }
}
