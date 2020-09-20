import { Component, OnInit } from '@angular/core';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Client } from 'src/classes/client';
import { Status } from 'src/classes/status';
import { Tag } from 'src/classes/tag';
import { GathererService } from '../services/gatherer.service';
import { EventType } from 'src/classes/eventType';

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

  public gatheringAllTags: boolean;
  public gatheringAllClientStatuses: boolean;
  public gatheringAllEvents: boolean;


  public separatorKeysCodes: number[] = [ENTER, COMMA];

  public value: string = '';


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

}
