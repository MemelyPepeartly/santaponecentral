import { Component, OnInit } from '@angular/core';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Client } from 'src/classes/client';
import { Status } from 'src/classes/status';
import { Tag } from 'src/classes/tag';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';

export class SearchQueryObjectContainer
{
  tags: Array<Tag> = [];
  events: Array<EventType> = [];
  statuses: Array<Status> = [];
}
@Component({
  selector: 'app-agent-catalogue',
  templateUrl: './agent-catalogue.component.html',
  styleUrls: ['./agent-catalogue.component.css']
})
export class AgentCatalogueComponent implements OnInit {

  constructor(private gatherer: GathererService,
    private formBuilder: FormBuilder) { }

  public allTags: Array<Tag> = [];
  public allEvents: Array<EventType> = [];
  public allClientStatuses: Array<Status> = [];

  public foundClients: Array<Client> = [];

  public gatheringAllTags: boolean;
  public gatheringAllClientStatuses: boolean;
  public gatheringAllEvents: boolean;

  public showHelper: boolean;

  public searchQueryString: string = '';
  public searchQueryObjectHolder: SearchQueryObjectContainer = new SearchQueryObjectContainer();

  public quoteReg: RegExp = new RegExp(/"([^"]+)"/g);
  public tagReg: RegExp = new RegExp(/"tag:([^"]+)"/g);
  public eventReg: RegExp = new RegExp(/"event:([^"]+)"/g);
  public statusReg: RegExp = new RegExp(/"status:([^"]+)"/g);

  public get allHelperTags() : Array<Tag>
  {
    let allowedTagArray: Array<Tag> = [];
    this.allTags.forEach((object: Tag) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.tags.some((tag: Tag) => {return tag.tagID == object.tagID}))
      {
        allowedTagArray.push(object);
      }
    });
    return allowedTagArray;
  }
  public get allHelperStatuses() : Array<Status>
  {
    let allowedStatusArray: Array<Status> = [];
    this.allClientStatuses.forEach((object: Status) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.statuses.some((status: Status) => {return status.statusID == object.statusID}))
      {
        // Push the object into the array
        allowedStatusArray.push(object);
      }
    });
    return allowedStatusArray;
  }
  public get allHelperEvents() : Array<EventType>
  {
    let allowedEventArray: Array<EventType> = [];
    this.allEvents.forEach((object: EventType) => {
      // If the query object does not already have that in the list
      if(!this.searchQueryObjectHolder.events.some((event: EventType) => {return event.eventTypeID == object.eventTypeID}))
      {
        // Push the object into the array
        allowedEventArray.push(object);
      }
    });
    return allowedEventArray;
  }

  async ngOnInit() {
    /* Gathering status subscribes */
    this.gatherer.gatheringAllTags.subscribe((status: boolean) => {
      this.gatheringAllTags = status;
    });
    this.gatherer.gatheringAllStatuses.subscribe((status: boolean) => {
      this.gatheringAllClientStatuses = status;
    });
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {
      this.gatheringAllEvents = status;
    });

    /* Gathering data subscribes */
    this.gatherer.allTags.subscribe((objectArray: Array<Tag>) => {
      this.allTags = objectArray;
    });
    this.gatherer.allStatuses.subscribe((objectArray: Array<Status>) => {
      this.allClientStatuses = objectArray;
    });
    this.gatherer.allEvents.subscribe((objectArray: Array<EventType>) => {
      this.allEvents = objectArray;
    });

    await this.gatherer.allGather();
  }
  addQuery(helper: any)
  {

    if(helper.tagID != undefined)
    {
      this.searchQueryString += '"tag:' + (<Tag>helper).tagName + '" ';
    }
    else if(helper.statusID != undefined)
    {
      this.searchQueryString += '"status:' + (<Status>helper).statusDescription + '" ';
    }
    else if(helper.eventTypeID != undefined)
    {
      this.searchQueryString += '"event:' + (<EventType>helper).eventDescription + '" ';
    }
    this.constructQueryObject();
  }
  constructQueryObject()
  {
    this.clearHolder();
    let tagMatches = Array.from(this.searchQueryString.matchAll(this.tagReg))
    let eventMatches = Array.from(this.searchQueryString.matchAll(this.eventReg))
    let statusMatches = Array.from(this.searchQueryString.matchAll(this.statusReg))

    tagMatches.forEach((element: RegExpMatchArray) => {
      // If allTags has a tag that equals the regex element group, and the searchable query object holder doesnt yet have that tag if so
      if(this.allTags.some((tag: Tag) => {return tag.tagName == element[1]}) && !this.searchQueryObjectHolder.tags.some((tag: Tag) => {return tag.tagName == element[1]}))
      {
        // Find and add the tag to the object holder
        this.searchQueryObjectHolder.tags.push(this.allTags.find((tag: Tag) => {return tag.tagName == element[1]}));
      }
    });
    eventMatches.forEach((element: RegExpMatchArray) => {
      // If allEvents has a event that equals the regex element group, and the searchable query object holder doesnt yet have that event if so
      if(this.allEvents.some((event: EventType) => {return event.eventDescription == element[1]}) && !this.searchQueryObjectHolder.events.some((event: EventType) => {return event.eventDescription == element[1]}))
      {
        // Find and add the event to the object holder
        this.searchQueryObjectHolder.events.push(this.allEvents.find((event: EventType) => {return event.eventDescription == element[1]}));
      }
    });
    statusMatches.forEach((element: RegExpMatchArray) => {
      // If allEvents has a event that equals the regex element group, and the searchable query object holder doesnt yet have that event if so
      if(this.allClientStatuses.some((status: Status) => {return status.statusDescription == element[1]}) && !this.searchQueryObjectHolder.statuses.some((status: Status) => {return status.statusDescription == element[1]}))
      {
        // Find and add the event to the object holder
        this.searchQueryObjectHolder.statuses.push(this.allClientStatuses.find((status: Status) => {return status.statusDescription == element[1]}));
      }
    });
  }
  public removeTagQuery(tag: Tag)
  {
    this.searchQueryObjectHolder.tags.splice(this.searchQueryObjectHolder.tags.indexOf(tag), 1);
    this.searchQueryString = this.searchQueryString.replace('"tag:'+ tag.tagName + '" ', "")
  }
  public removeEventQuery(event: EventType)
  {
    this.searchQueryObjectHolder.events.splice(this.searchQueryObjectHolder.events.indexOf(event), 1);
    this.searchQueryString = this.searchQueryString.replace('"event:'+ event.eventDescription + '" ', "")
  }
  public removeStatusQuery(status: Status)
  {
    this.searchQueryObjectHolder.statuses.splice(this.searchQueryObjectHolder.statuses.indexOf(status), 1);
    this.searchQueryString = this.searchQueryString.replace('"status:'+ status.statusDescription + '" ', "")
  }
  clearHolder()
  {
    this.searchQueryObjectHolder.tags = [];
    this.searchQueryObjectHolder.events = [];
    this.searchQueryObjectHolder.statuses = [];
  }
  public search()
  {

  }
  public showCardInfo(client: Client)
  {

  }
}
