import { Injectable } from '@angular/core';
import { SantaApiGetService } from './santaApiService.service';
import { MapService } from './mapService.service';
import { Client } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { Survey, Question } from 'src/classes/survey';
import { EventType } from 'src/classes/eventType';
import { Observable, BehaviorSubject } from 'rxjs';
import { Status } from 'src/classes/status';
import { Message } from 'src/classes/message';

@Injectable({
  providedIn: 'root'
})
export class GathererService {

  constructor(private SantaApiGet: SantaApiGetService, private ApiMapper: MapService) { }

  public onSelectedClient: boolean = false;
  /* BEHAVIOR SUBJECTS FOR GATHERING STATUS */
  public _gatheringAllClients: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllClients()
  {
    return this._gatheringAllClients.asObservable();
  }

  public _gatheringAllTags: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllTags()
  {
    return this._gatheringAllTags.asObservable();
  }

  public _gatheringAllSurveys: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllSurveys()
  {
    return this._gatheringAllSurveys.asObservable();
  }

  public _gatheringAllQuestions: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllQuestions()
  {
    return this._gatheringAllQuestions.asObservable();
  }

  public _gatheringAllEvents: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllEvents()
  {
    return this._gatheringAllEvents.asObservable();
  }

  public _gatheringAllStatuses: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllStatuses()
  {
    return this._gatheringAllStatuses.asObservable();
  }

  public _gatheringAllMessages: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gatheringAllMessages()
  {
    return this._gatheringAllMessages.asObservable();
  }

  /* BEHAVIOR SUBJECTS FOR DATA */
  private _allClients: BehaviorSubject<Array<Client>>= new BehaviorSubject([])
  private _allTags: BehaviorSubject<Array<Tag>> = new BehaviorSubject([])
  private _allSurveys: BehaviorSubject<Array<Survey>> = new BehaviorSubject([])
  private _allQuestions: BehaviorSubject<Array<Question>> = new BehaviorSubject([])
  private _allEvents: BehaviorSubject<Array<EventType>> = new BehaviorSubject([])
  private _allStatuses: BehaviorSubject<Array<Status>> = new BehaviorSubject([])
  private _allMessages: BehaviorSubject<Array<Message>> = new BehaviorSubject([])

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
  
  get allMessages()
  {
    return this._allMessages.asObservable();
  }
  private updateAllMessages(messageArray: Array<Message>)
  {
    this._allMessages.next(messageArray);
  }
  
  /* GATHERING METHODS */
  public async gatherAllClients()
  {
    this._gatheringAllClients.next(true);

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
    this._gatheringAllClients.next(false);
  }
  public async gatherAllTags()
  {
    this._gatheringAllTags.next(true);
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
    this._gatheringAllTags.next(false);
  }
  public async gatherAllSurveys()
  {
    this._gatheringAllSurveys.next(true);
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
    this._gatheringAllSurveys.next(false);
  }
  public async gatherAllQuestions()
  {
    this._gatheringAllQuestions.next(true);
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
    this._gatheringAllQuestions.next(false);
  }
  public async gatherAllEvents()
  {
    this._gatheringAllEvents.next(true);
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
    this._gatheringAllEvents.next(false);
  }
  public async gatherAllStatuses()
  {
    this._gatheringAllStatuses.next(true);
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
    this._gatheringAllStatuses.next(false);
  }
  public async gatherAllMessages()
  {
    this._gatheringAllMessages.next(true);
    this.updateAllMessages([])
    let messageList: Array<Message> = []

    var res = await this.SantaApiGet.getAllMessages().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      messageList.push(this.ApiMapper.mapMessage(res[i]));
    }
    this.updateAllMessages(messageList);
    this._gatheringAllMessages.next(false);
  }
  
  // Utility methods
  public async allGather()
  {
    await this.gatherAllClients();
    await this.gatherAllEvents();
    await this.gatherAllQuestions();
    await this.gatherAllSurveys();
    await this.gatherAllTags();
    await this.gatherAllStatuses();
    //await this.gatherAllMessages();
  }
  public clearAll()
  {
    this.updateAllClient([]);
    this.updateAllEvents([]);
    this.updateAllQuestions([]);
    this.updateAllSurveys([]);
    this.updateAllTags([]);
    this.updateAllStatuses([]);
    //this.updateAllMessages([]);
  }
}
