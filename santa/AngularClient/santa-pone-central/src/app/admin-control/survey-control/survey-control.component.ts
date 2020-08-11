import { Component, OnInit, Input } from '@angular/core';
import { Survey, Question } from 'src/classes/survey';
import { Validators, FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santaApiService.service';
import { MapResponse, MapService } from 'src/app/services/mapService.service';
import { GathererService } from 'src/app/services/gatherer.service';
import { Client } from 'src/classes/client';
import { ThrowStmt } from '@angular/compiler';

@Component({
  selector: 'app-survey-control',
  templateUrl: './survey-control.component.html',
  styleUrls: ['./survey-control.component.css']
})
export class SurveyControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ResponseMapper: MapResponse,
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  @Input() allSurveys: Array<Survey> = []
  @Input() allQuestions: Array<Question> = []

  public selectedSurvey: Survey = new Survey();
  public selectedQuestions: Array<Question> = [];

  public postingNewSurvey: boolean = false;
  public updatingSurveyName: boolean = false;
  public deletingSurvey: boolean = false;

  public gatheringAllSurveys: boolean = false;

  ngOnInit(): void {
    this.gatherer.gatheringAllSurveys.subscribe((status: boolean) => {
      this.gatheringAllSurveys = status;
    });
    this.constructFormGroups();
  }
  private constructFormGroups() {

  }
  public deleteSurvey(survey: Survey)
  {

  }
  public editSurvey()
  {

  }
  public setSelectedSurvey(survey: Survey)
  {
    this.selectedSurvey = survey;
  }
  public addNewSurvey()
  {

  }
  public sortAddableQuestions() : Array<Question>
  {
    let questionsThatCanBeAdded: Array<Question>= []
    this.allQuestions.forEach((question: Question) => {
      if(!this.selectedSurvey.surveyQuestions.some((thing: Question) => {return thing.questionID == question.questionID}))
      {
        questionsThatCanBeAdded.push(question);
      }
    });
    return questionsThatCanBeAdded;
  }
}
