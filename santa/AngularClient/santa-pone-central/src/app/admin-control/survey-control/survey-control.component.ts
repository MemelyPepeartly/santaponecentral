import { Component, OnInit, Input } from '@angular/core';
import { Survey, Question } from 'src/classes/survey';
import { Validators, FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santa-api.service';
import { MapResponse, MapService } from 'src/app/services/mapper.service';
import { GathererService } from 'src/app/services/gatherer.service';
import { Client } from 'src/classes/client';
import { ThrowStmt } from '@angular/compiler';
import { SurveyQuestionXrefsResponseModel } from 'src/classes/responseTypes';

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
  public selectedQuestion: Question = new Question();
  public selectedQuestions: Array<Question> = [];

  public postingNewSurvey: boolean = false;
  public updatingSurveyName: boolean = false;
  public deletingSurvey: boolean = false;
  public removingQuestion: boolean = false;
  public addingQuestions: boolean = false;

  public gatheringAllSurveys: boolean = false;

  ngOnInit(): void {
    this.gatherer.gatheringAllSurveys.subscribe((status: boolean) => {
      this.gatheringAllSurveys = status;
    });
    this.constructFormGroups();
  }
  private constructFormGroups() {

  }
  public async removeQuestion(question: Question)
  {
    this.removingQuestion = true;

    this.selectedSurvey = this.ApiMapper.mapSurvey(await this.SantaApiDelete.deleteQuestionRelationFromSurvey(this.selectedSurvey.surveyID, question.questionID).toPromise());
    await this.gatherer.gatherAllQuestions();
    await this.gatherer.gatherAllSurveys();

    this.selectedQuestion = new Question();

    this.removingQuestion = false;
  }
  public setSelectedQuestion(question: Question)
  {
    this.selectedQuestion = question;
  }
  public setSelectedSurvey(survey: Survey)
  {
    this.selectedSurvey = survey;
    this.selectedQuestion = new Question();
  }
  public async addQuestions()
  {
    this.addingQuestions = true;
    let response: SurveyQuestionXrefsResponseModel = new SurveyQuestionXrefsResponseModel();
    this.selectedQuestions.forEach((question: Question) => {
      response.questions.push(question.questionID);
    })
    this.selectedSurvey = this.ApiMapper.mapSurvey(await this.SantaApiPost.postQuestionsToSurvey(this.selectedSurvey.surveyID, response).toPromise());
    await this.gatherer.gatherAllSurveys();

    this.addingQuestions = false;
  }
  public sortAddableQuestions() : Array<Question>
  {
    let questionsThatCanBeAdded: Array<Question>= []
    this.allQuestions.forEach((question: Question) => {
      if(!this.selectedSurvey.surveyQuestions.some((questionObj: Question) => {return questionObj.questionID == question.questionID}))
      {
        questionsThatCanBeAdded.push(question);
      }
    });
    return questionsThatCanBeAdded;
  }
  public sortRemovableQuestions() : Array<Question>
  {
    return this.selectedSurvey.surveyQuestions.filter((question: Question) => {return question.removable})
  }
}
