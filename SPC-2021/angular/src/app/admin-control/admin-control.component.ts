import { Component, OnInit} from '@angular/core';
import { Tag } from 'src/classes/tag';
import { MapService } from '../services/utility services/mapper.service';
import { Question, Survey } from 'src/classes/survey';
import { EventType } from 'src/classes/eventType';
import { GeneralDataGathererService } from '../services/gathering services/general-data-gatherer.service';
import { HQClient } from 'src/classes/client';
import { AuthService } from '@auth0/auth0-angular';
import { Category, YuleLog } from 'src/classes/yuleLogTypes';

@Component({
  selector: 'app-admin-control',
  templateUrl: './admin-control.component.html',
  styleUrls: ['./admin-control.component.css']
})
export class AdminControlComponent implements OnInit
{

  constructor(public auth: AuthService,
    public gatherer: GeneralDataGathererService,
    public ApiMapper: MapService) { }

  public profile: any;
  public isDev: boolean;

  public allTags: Array<Tag> = [];
  public allEvents: Array<EventType> = [];
  public allSurveys: Array<Survey> = [];
  public allQuestions: Array<Question> = [];
  public allClients: Array<HQClient> = [];
  public allCategories: Array<Category> = [];
  public allYuleLogs: Array<YuleLog> = [];

  async ngOnInit() {
    this.allTags = [];
    this.allClients = [];
    this.allEvents = [];
    this.allQuestions = [];
    this.allSurveys = [];
    this.allCategories = [];
    this.allYuleLogs = [];

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
    this.gatherer.allHQClients.subscribe((clients: Array<HQClient>) => {
      this.allClients = clients;
    });
    this.gatherer.allCategories.subscribe((categoryArray: Array<Category>) => {
      this.allCategories = categoryArray;
    });
    this.gatherer.allYuleLogs.subscribe((logArray: Array<YuleLog>) => {
      this.allYuleLogs = logArray;
    });

    // Auth
    this.auth.user$.subscribe(data => {
      this.profile = data;
    });
    /* SANTAHERE Fix this come the time
    this.auth.isDev.subscribe((status: boolean) => {
      this.isDev = status;
    });
    */
  }
}
