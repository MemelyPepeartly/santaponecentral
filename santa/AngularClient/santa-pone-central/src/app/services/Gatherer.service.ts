import { Injectable } from '@angular/core';
import { SantaApiGetService } from './SantaApiService.service';
import { MapService } from './MapService.service';
import { Client } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { Survey, Question } from 'src/classes/survey';
import { EventType } from 'src/classes/EventType';

@Injectable({
  providedIn: 'root'
})
export class GathererService {

  constructor(public SantaApiGet: SantaApiGetService, public ApiMapper: MapService) { }

  public allClients: Array<Client> = []
  public allTags: Array<Tag> = []
  public allSurveys: Array<Survey> = []
  public allQuestions: Array<Question> = []
  public allEvents: Array<EventType> = []

  public gatheringAllClients: boolean = false;
  public gatheringAllTags: boolean = false;
  public gatheringAllSurveys: boolean = false;
  public gatheringAllQuestions: boolean = false;
  public gatheringAllEvents: boolean = false;


  
  public async gatherAllClients()
  {
    this.gatheringAllClients = true;
    this.allClients = [];

    var res = await this.SantaApiGet.getAllClients().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allClients.push(this.ApiMapper.mapClient(res[i]));
    }
    this.gatheringAllClients = false;
  }
  public async gatherAllTags()
  {
    this.gatheringAllTags = true;
    this.allTags = [];

    var res = await this.SantaApiGet.getAllTags().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allTags.push(this.ApiMapper.mapTag(res[i]));
    }
    this.gatheringAllTags = false;
  }
  public async gatherAllSurveys()
  {
    this.gatheringAllSurveys = true;
    this.allSurveys = [];

    var res = await this.SantaApiGet.getAllSurveys().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allSurveys.push(this.ApiMapper.mapSurvey(res[i]));
    }
    this.gatheringAllSurveys = false;
  }
  public async gatherAllQuestions()
  {
    this.gatheringAllQuestions = true;
    this.allQuestions = [];

    var res = await this.SantaApiGet.getAllSurveyQuestions().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allQuestions.push(this.ApiMapper.mapQuestion(res[i]));
    }
    this.gatheringAllQuestions = false;
  }
  public async gatherAllEvents()
  {
    this.gatheringAllEvents = true;
    this.allEvents = [];

    var res = await this.SantaApiGet.getAllEvents().toPromise().catch(err => {
      console.log(err); 
    });

    for(let i = 0; i < res.length; i++)
    {
      this.allEvents.push(this.ApiMapper.mapEvent(res[i]));
    }
    this.gatheringAllEvents = false;
  }
}
