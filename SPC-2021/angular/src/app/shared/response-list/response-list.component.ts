import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Question, SurveyResponse, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Client } from 'src/classes/client';
import { Profile } from 'src/classes/profile';
import { EditSurveyResponseRequest, AddSurveyResponseRequest } from 'src/classes/request-types';
import { SurveyService } from 'src/app/services/api services/survey.service';

@Component({
  selector: 'app-response-list',
  templateUrl: './response-list.component.html',
  styleUrls: ['./response-list.component.css']
})
export class ResponseListComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    private SurveyService: SurveyService) { }

  @Input() survey: Survey = new Survey();
  @Input() clientID: string;
  @Input() responses: Array<SurveyResponse> = [];
  @Input() editable: boolean = false;
  @Input() viewingAssignment: boolean = false;

  @Output() submitClickedRefreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public surveyFormGroup: FormGroup;
  public get surveyControls()
  {
    return this.surveyFormGroup.controls
  }
  public get showDidNotAnswerWarning() : boolean
  {
    let result: boolean = true;
    // Foreach response, if any questions have a response
    this.responses.forEach((response: SurveyResponse) => {
      if(this.survey.surveyQuestions.some((question: Question) => {return question.questionID == response.surveyQuestion.questionID && this.survey.surveyID == response.surveyID}))
      {
        result = false;
      }
    });
    return result
  }

  public selectedQuestion: Question = new Question();

  public puttingResponse: boolean = false;
  public showAll: boolean = false;

  ngOnInit(): void {
    this.surveyFormGroup = this.formBuilder.group({});
    this.addFields();
  }
  addFields()
  {
    this.survey.surveyQuestions.forEach((question: Question) => {
      this.surveyFormGroup.addControl(this.survey.surveyID + question.questionID, new FormControl('', [Validators.required, Validators.maxLength(4000)]))
    });
  }
  setSelectedQuestion(question: Question)
  {
    this.selectedQuestion = question
  }
  getResponseFromSelectedQuestion(question: Question = null) : SurveyResponse
  {
    if(question == null)
    {
      let response: SurveyResponse = this.responses.find((response: SurveyResponse) => {return response.surveyQuestion.questionID == this.selectedQuestion.questionID && response.surveyID == this.survey.surveyID})
      return  response == undefined ? new SurveyResponse() : response
    }
    else
    {
      let response: SurveyResponse = this.responses.find((response: SurveyResponse) => {return response.surveyQuestion.questionID == question.questionID && response.surveyID == this.survey.surveyID})
      return  response == undefined ? new SurveyResponse() : response
    }

  }
  public submitNewResponse()
  {
    this.puttingResponse = true;

    // If there is a response already for this question
    if(this.getResponseFromSelectedQuestion().surveyResponseID != undefined)
    {
      // Get the response body, and put the response
      let editedResponse: EditSurveyResponseRequest =
      {
        responseText: this.surveyFormGroup.get(this.getFormControlNameFromQuestion(this.selectedQuestion)).value
      };

      this.SurveyService.putResponse(this.getResponseFromSelectedQuestion().surveyResponseID, editedResponse).subscribe((res) => {
        this.submitClickedRefreshEvent.emit(true);
        this.surveyFormGroup.get(this.getFormControlNameFromQuestion(this.selectedQuestion)).reset();
        this.puttingResponse = false;
      },
      err => {
        console.log("Something went wrong!");
        console.log(err);
        this.puttingResponse = false;
      });
    }
    // Else if there is no response, and the question was unanswered
    else
    {
      // Create a new response and post it
      let newResponse: AddSurveyResponseRequest =
      {
        surveyID: this.survey.surveyID,
        clientID: this.clientID,
        surveyQuestionID: this.selectedQuestion.questionID,
        responseText: this.surveyFormGroup.get(this.getFormControlNameFromQuestion(this.selectedQuestion)).value
      };

      this.SurveyService.postSurveyResponse(newResponse).subscribe((res) => {
        this.submitClickedRefreshEvent.emit(true);
        this.surveyFormGroup.get(this.getFormControlNameFromQuestion(this.selectedQuestion)).reset();
        this.puttingResponse = false;
      },
      err => {
        console.log("Something went wrong!");
        console.log(err);
        this.puttingResponse = false;
      });
    }
  }
  public getViewableQuestions() : Array<Question>
  {
    return this.survey.surveyQuestions.filter((question: Question) => {return question.senderCanView})
  }
  public getFormControlNameFromQuestion(question: Question) : string
  {
    return this.survey.surveyID + question.questionID
  }
}
