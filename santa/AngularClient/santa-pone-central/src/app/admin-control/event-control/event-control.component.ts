import { Component, OnInit, Input } from '@angular/core';
import { EventType } from 'src/classes/EventType';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/SantaApiService.service';
import { MapResponse, MapService } from 'src/app/services/MapService.service';
import { GathererService } from 'src/app/services/Gatherer.service';
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
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  @Input() allEvents: Array<EventType> = []

  public addEventFormGroup: FormGroup;
  public editEventFormGroup: FormGroup;


  public selectedEvent: EventType;
  public deletableEvents: Array<EventType> = [];
  public tagsInUse: Array<EventType> = [];
  public allClients: Array<Client> = [];

  public postingNewEvent: boolean = false;
  public updatingEventName: boolean = false;
  public deletingEvent: boolean = false;

  // Getters for form values for ease-of-use
  get newEvent() {
    var formControlObj = this.addEventFormGroup.get('newEvent') as FormControl
    return formControlObj.value
  }
  get editedEventName() {
    var formControlObj = this.editEventFormGroup.get('editEvent') as FormControl
    return formControlObj.value
  }

  ngOnInit(): void {
    this.constructFormGroups();
  }
  private constructFormGroups() {
    this.addEventFormGroup = this.formBuilder.group({
      newEvent: [null, Validators.nullValidator && Validators.pattern],
    });
    this.editEventFormGroup = this.formBuilder.group({
      editEvent: [null, Validators.nullValidator && Validators.pattern],
    });
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
