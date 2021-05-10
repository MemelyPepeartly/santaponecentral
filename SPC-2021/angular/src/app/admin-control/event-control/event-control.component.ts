import { Component, OnInit, Input } from '@angular/core';
import { EventType } from 'src/classes/eventType';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santa-api.service';
import { MapResponse, MapService } from 'src/app/services/utility services/mapper.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-event-control',
  templateUrl: './event-control.component.html',
  styleUrls: ['./event-control.component.css']
})
export class EventControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ResponseMapper: MapResponse,
    private gatherer: GeneralDataGathererService,
    public ApiMapper: MapService) { }

  @Input() allEvents: Array<EventType> = []

  public addEventFormGroup: FormGroup;
  public editEventFormGroup: FormGroup;

  public tagsInUse: Array<EventType> = [];

  public gatheringAllEvents: boolean = false;

  async ngOnInit() {
    this.constructFormGroups();
    this.gatherer.gatheringAllEvents.subscribe((status: boolean) => {
      this.gatheringAllEvents = status;
    });
    await this.gatherer.gatherAllEvents();
  }
  private constructFormGroups() {
  }
  public deleteEvent(event: EventType)
  {

  }
  public editEvent()
  {

  }
  public setSelectedEvent(event: EventType)
  {

  }
  public unsetSelectedEvent()
  {

  }
  public addNewEvent()
  {

  }
}
