import { Component, OnInit} from '@angular/core';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from '../services/santaApiService.service';
import { Tag } from 'src/classes/tag';
import { MapService } from '../services/mapService.service';
import { Question, Survey } from 'src/classes/survey';
import { EventType } from 'src/classes/eventType';
import { GathererService } from '../services/gatherer.service';
import { TagControlComponent } from './tag-control/tag-control.component';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-admin-control',
  templateUrl: './admin-control.component.html',
  styleUrls: ['./admin-control.component.css']
})
export class AdminControlComponent implements OnInit
{

  constructor(public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  public allTags: Array<Tag> = [];
  public allEvents: Array<EventType> = [];
  public allSurveys: Array<Survey> = [];
  public allQuestions: Array<Question> = [];
  public allClients: Array<Client> = [];

  public tagControlSelected: boolean = false;
  public eventControlSelected: boolean = false;
  public surveyControlSelected: boolean = false;
  public questionControlSelected: boolean = false;

  async ngOnInit() {
    this.allTags = [];
    this.allClients = [];
    this.allEvents = [];
    this.allQuestions = [];
    this.allSurveys = [];
    
    this.gatherer.allTags.subscribe((tagArray: Array<Tag>) => {
      this.allTags = tagArray;
    });
    this.gatherer.allEvents.subscribe((eventArray: Array<EventType>) => {
      this.allEvents = eventArray;
    });
    this.gatherer.allSurveys.subscribe((surveyArray: Array<Survey>) => {
      this.allSurveys = surveyArray;
    });
    this.gatherer.allQuestions.subscribe((questionArray: Array<Question>) => {
      this.allQuestions = questionArray;
    });
    this.gatherer.allClients.subscribe((clients: Array<Client>) => {
      this.allClients = clients;
    });
    await this.gatherer.gatherAllClients();
    await this.gatherer.gatherAllTags();
    await this.gatherer.gatherAllEvents();
    await this.gatherer.gatherAllSurveys();
    await this.gatherer.gatherAllQuestions();
  }
  public selectTagControl()
  {
    this.tagControlSelected = true;
    this.eventControlSelected = false;
    this.surveyControlSelected = false;
    this.questionControlSelected = false;
  }
  public selectEventControl()
  {
    this.tagControlSelected = false;
    this.eventControlSelected = true;
    this.surveyControlSelected = false;
    this.questionControlSelected = false;
  }
  public selectSurveyControl()
  {
    this.tagControlSelected = false;
    this.eventControlSelected = false;
    this.surveyControlSelected = true;
    this.questionControlSelected = false;
  }
  public selectQuestionControl()
  {
    this.tagControlSelected = false;
    this.eventControlSelected = false;
    this.surveyControlSelected = false;
    this.questionControlSelected = true;
  }
}
