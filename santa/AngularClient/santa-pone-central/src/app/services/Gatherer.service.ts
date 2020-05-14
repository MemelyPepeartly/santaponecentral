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

  
  public async gatherAllClients()
  {

  }
  public async gatherAllTags()
  {

  }
  public async gatherAllSurveys()
  {

  }
  public async gatherAllQuestions()
  {

  }
  public async gatherAllEvents()
  {

  }
}
