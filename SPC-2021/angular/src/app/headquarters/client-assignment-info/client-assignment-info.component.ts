import { Component, Input, OnInit } from '@angular/core';
import { ClientService } from 'src/app/services/api services/client.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { MapService } from 'src/app/services/utility services/mapper.service';
import { HQClient, RelationshipMeta } from 'src/classes/client';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-client-assignment-info',
  templateUrl: './client-assignment-info.component.html',
  styleUrls: ['./client-assignment-info.component.css']
})
export class ClientAssignmentInfoComponent implements OnInit {

  constructor(public gatherer: GeneralDataGathererService, public ClientService: ClientService, public mapper: MapService) { }

  @Input() client: HQClient = new HQClient();

  public events: Array<EventType> = [];

  public gatheringAllEvents: boolean = false;
  public gatheringInfoContainer: boolean = false;

  async ngOnInit() {
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {
      this.gatheringAllEvents = status;
    });
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.events = eventArray
    });
    if(this.client.infoContainer.agentID == undefined)
    {
      this.gatheringInfoContainer = true;
      this.client.infoContainer = this.mapper.mapInfoContainer(await this.ClientService.getInfoContainerByClientID(this.client.clientID).toPromise());
      this.gatheringInfoContainer = false;
    }
    await this.gatherer.gatherAllEvents();
  }

  public getEventSenders(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.infoContainer.senders.filter((sender: RelationshipMeta) => {return sender.eventType.eventTypeID == eventType.eventTypeID});
  }
  public getEventAssignments(eventType: EventType) : Array<RelationshipMeta>
  {
    return this.client.infoContainer.assignments.filter((assignment: RelationshipMeta) => {return assignment.eventType.eventTypeID == eventType.eventTypeID});
  }
}
