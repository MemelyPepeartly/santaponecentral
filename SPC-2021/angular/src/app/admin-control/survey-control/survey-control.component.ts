import { Component, OnInit, Input } from '@angular/core';
import { Survey, Question } from 'src/classes/survey';
import { FormBuilder} from '@angular/forms';
import { MapResponse, MapService } from 'src/app/services/utility services/mapper.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { Client } from 'src/classes/client';
import { ThrowStmt } from '@angular/compiler';
import { NewSurveyQuestionXrefsRequest } from 'src/classes/request-types';
import { SurveyService } from 'src/app/services/api services/survey.service';

@Component({
  selector: 'app-survey-control',
  templateUrl: './survey-control.component.html',
  styleUrls: ['./survey-control.component.css']
})
export class SurveyControlComponent implements OnInit {

  constructor(private SurveyService: SurveyService,
    public ResponseMapper: MapResponse,
    private gatherer: GeneralDataGathererService,
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

  async ngOnInit() {
    this.gatherer.gatheringAllSurveys.subscribe((status: boolean) => {
      this.gatheringAllSurveys = status;
    });
    this.constructFormGroups();
    await this.gatherer.gatherAllSurveys();
  }
  private constructFormGroups() {

  }
  public async removeQuestion(question: Question)
  {
    this.removingQuestion = true;

    this.selectedSurvey = this.ApiMapper.mapSurvey(await this.SurveyService.deleteQuestionRelationFromSurvey(this.selectedSurvey.surveyID, question.questionID).toPromise());
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
    let response: NewSurveyQuestionXrefsRequest = new NewSurveyQuestionXrefsRequest();
    this.selectedQuestions.forEach((question: Question) => {
      response.questions.push(question.questionID);
    })
    this.selectedSurvey = this.ApiMapper.mapSurvey(await this.SurveyService.postQuestionsToSurvey(this.selectedSurvey.surveyID, response).toPromise());
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
