import { Component, OnInit } from '@angular/core';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from '../services/SantaApiService.service';
import { Tag } from 'src/classes/tag';
import { MapService } from '../services/MapService.service';
import { Question, Survey } from 'src/classes/survey';
import { EventType } from 'src/classes/EventType';

@Component({
  selector: 'app-admin-control',
  templateUrl: './admin-control.component.html',
  styleUrls: ['./admin-control.component.css']
})
export class AdminControlComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ApiMapper: MapService) { }

  public allTags: Array<Tag> = [];
  public allEvents: Array<EventType> = [];
  public allSurveys: Array<Survey> = [];
  public allQuestions: Array<Question> = [];

  public tagControlSelected: boolean = false;
  public eventControlSelected: boolean = false;
  public surveyControlSelected: boolean = false;
  public questionControlSelected: boolean = false;


  async ngOnInit() {
    await this.gatherAllTags();
    console.log("Tag done");
    await this.gatherAllEvents();
    console.log("Event Done");
    await this.gatherAllSurveys();
    console.log("Survey Done");
    await this.gatherAllQuestions();
    console.log("Question Done");
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
  public async refreshTags(event: boolean)
  {
    if(event)
    {
      await this.gatherAllTags();
    }
  }
  public async gatherAllTags()
  {
    this.allTags = [];
    var tagRes = await this.SantaApiGet.getAllTags().toPromise();
    for(let i = 0; i < tagRes.length; i++)
    {
      this.allTags.push(this.ApiMapper.mapTag(tagRes[i]))
    }
  }
  public async gatherAllEvents()
  {
    this.allEvents = [];
    var eventRes = await this.SantaApiGet.getAllEvents().toPromise();
    for(let i = 0; i < eventRes.length; i++)
    {
      this.allEvents.push(this.ApiMapper.mapEvent(eventRes[i]))
    }
  }
  public async gatherAllSurveys()
  {
    this.allSurveys = [];
    var surveyRes = await this.SantaApiGet.getAllSurveys().toPromise();
    for(let i = 0; i < surveyRes.length; i++)
    {
      this.allSurveys.push(this.ApiMapper.mapSurvey(surveyRes[i]))
    }
  }
  public async gatherAllQuestions()
  {
    this.allQuestions = [];
    var questionRes = await this.SantaApiGet.getAllSurveyQuestions().toPromise();
    for(let i = 0; i < questionRes.length; i++)
    {
      this.allQuestions.push(this.ApiMapper.mapQuestion(questionRes[i]))
    }
  }
}
