import { Injectable } from '@angular/core';
import { SantaApiGetService } from './SantaApiService.service';
import { MapService } from './MapService.service';
import { Client } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { Survey, Question } from 'src/classes/survey';
import { EventType } from 'src/classes/EventType';
import { Observable, BehaviorSubject } from 'rxjs';
import { Status } from 'src/classes/status';

@Injectable({
  providedIn: 'root'
})
export class GathererService {

  constructor(public SantaApiGet: SantaApiGetService, public ApiMapper: MapService) { }

  public gatheringAllClients: boolean = false;
  public gatheringAllTags: boolean = false;
  public gatheringAllSurveys: boolean = false;
  public gatheringAllQuestions: boolean = false;
  public gatheringAllEvents: boolean = false;
  public gatheringAllStatuses: boolean = false;


  private _allClients: BehaviorSubject<Array<Client>>= new BehaviorSubject([])
  private _allTags: BehaviorSubject<Array<Tag>> = new BehaviorSubject([])
  private _allSurveys: BehaviorSubject<Array<Survey>> = new BehaviorSubject([])
  private _allQuestions: BehaviorSubject<Array<Question>> = new BehaviorSubject([])
  private _allEvents: BehaviorSubject<Array<EventType>> = new BehaviorSubject([])
  private _allStatuses: BehaviorSubject<Array<Status>> = new BehaviorSubject([])


  private _onSelectedClient: boolean = false;
  get onSelectedClient(): boolean
  {
    return this._onSelectedClient;
  }
  set onSelectedClient(value: boolean)
  {
    this._onSelectedClient = value;
  }

  get allClients()
  {
    return this._allClients.asObservable();
  }
  private updateAllClient(clientArray: Array<Client>)
  {
    this._allClients.next(clientArray);
  }

  get allTags()
  {
    return this._allTags.asObservable();
  }
  private updateAllTags(tagArray: Array<Tag>)
  {
    this._allTags.next(tagArray);
  }

  get allSurveys()
  {
    return this._allSurveys.asObservable();
  }
  private updateAllSurveys(surveyArray: Array<Survey>)
  {
    this._allSurveys.next(surveyArray);
  }

  get allQuestions()
  {
    return this._allQuestions.asObservable();
  }
  private updateAllQuestions(questionArray: Array<Question>)
  {
    this._allQuestions.next(questionArray);
  }

  get allEvents()
  {
    return this._allEvents.asObservable();
  }
  private updateAllEvents(eventArray: Array<EventType>)
  {
    this._allEvents.next(eventArray);
  }

  get allStatuses()
  {
    return this._allStatuses.asObservable();
  }
  private updateAllStatuses(statusArray: Array<Status>)
  {
    this._allStatuses.next(statusArray);
  }
  
  public async gatherAllClients()
  {
    this.gatheringAllClients = true;

    this.updateAllClient([]);
    let clientList: Array<Client> = []

    var res = await this.SantaApiGet.getAllClients().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      clientList.push(this.ApiMapper.mapClient(res[i]));
    }
    this.updateAllClient(clientList);
    this.gatheringAllClients = false;
  }
  public async gatherAllTags()
  {
    this.gatheringAllTags = true;
    this.updateAllTags([])
    let tagList: Array<Tag> = []

    var res = await this.SantaApiGet.getAllTags().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      tagList.push(this.ApiMapper.mapTag(res[i]));
    }
    this.updateAllTags(tagList);
    this.gatheringAllTags = false;
  }
  public async gatherAllSurveys()
  {
    this.gatheringAllSurveys = true;
    this.updateAllSurveys([]);
    let surveyList: Array<Survey> = []

    var res = await this.SantaApiGet.getAllSurveys().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      surveyList.push(this.ApiMapper.mapSurvey(res[i]));
    }
    this.updateAllSurveys(surveyList);
    this.gatheringAllSurveys = false;
  }
  public async gatherAllQuestions()
  {
    this.gatheringAllQuestions = true;
    this.updateAllQuestions([]);
    let questionList: Array<Question> = []

    var res = await this.SantaApiGet.getAllSurveyQuestions().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      questionList.push(this.ApiMapper.mapQuestion(res[i]));
    }
    this.updateAllQuestions(questionList);
    this.gatheringAllQuestions = false;
  }
  public async gatherAllEvents()
  {
    this.gatheringAllEvents = true;
    this.updateAllEvents([])
    let eventList: Array<EventType> = []

    var res = await this.SantaApiGet.getAllEvents().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      eventList.push(this.ApiMapper.mapEvent(res[i]));
    }
    this.updateAllEvents(eventList);
    this.gatheringAllEvents = false;
  }
  public async gatherAllStatuses()
  {
    this.gatheringAllStatuses = true;
    this.updateAllStatuses([])
    let statusList: Array<Status> = []

    var res = await this.SantaApiGet.getAllStatuses().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      statusList.push(this.ApiMapper.mapStatus(res[i]));
    }
    this.updateAllStatuses(statusList);
    this.gatheringAllStatuses = false;
  }
  public async allGather()
  {
    await this.gatherAllClients();
    await this.gatherAllEvents();
    await this.gatherAllQuestions();
    await this.gatherAllSurveys();
    await this.gatherAllTags();
    await this.gatherAllStatuses();
  }
}
