import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Question, SurveyResponse, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ChangeSurveyResponseModel, SurveyApiResponse } from 'src/classes/responseTypes';
import { SantaApiPutService, SantaApiPostService } from 'src/app/services/santaApiService.service';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-response-list',
  templateUrl: './response-list.component.html',
  styleUrls: ['./response-list.component.css']
})
export class ResponseListComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    private SantaApiPut: SantaApiPutService,
    private SantaApiPost: SantaApiPostService) { }

  @Input() survey: Survey = new Survey();
  @Input() client: Client = new Client();
  @Input() responses: Array<SurveyResponse> = [];
  @Input() editable: boolean = false;

  @Output() submitClickedRefreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public surveyFormGroup: FormGroup;
  public get surveyControls()
  {
    return this.surveyFormGroup.controls
  }

  public selectedQuestion: Question = new Question();

  public puttingResponse: boolean = false;

  ngOnInit(): void {
    this.surveyFormGroup = this.formBuilder.group({});
    this.addFields();
  }
  addFields()
  {
    this.survey.surveyQuestions.forEach((question: Question) => {
      this.surveyFormGroup.addControl(question.questionID, new FormControl('', [Validators.required, Validators.maxLength(2000)]))
    });
  }
  setSelectedQuestion(question: Question)
  {
    this.selectedQuestion = question
  }
  getResponseFromSelectedQuestion() : SurveyResponse
  {
    let response: SurveyResponse = this.responses.find((response: SurveyResponse) => {return response.surveyQuestion.questionID == this.selectedQuestion.questionID})
    return  response == undefined ? new SurveyResponse() : response
  }
  public submitNewResponse()
  {
    this.puttingResponse = true;

    // If there is a response already for this question
    if(this.getResponseFromSelectedQuestion().surveyResponseID != undefined)
    {
      // Get the response body, and put the response
      let editedResponse: ChangeSurveyResponseModel =
      {
        responseText: this.surveyFormGroup.get(this.selectedQuestion.questionID).value
      };

      this.SantaApiPut.putResponse(this.getResponseFromSelectedQuestion().surveyResponseID, editedResponse).subscribe((res) => {
        this.submitClickedRefreshEvent.emit(true);
        this.surveyFormGroup.get(this.selectedQuestion.questionID).reset();
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
      let newResponse: SurveyApiResponse =
      {
        surveyID: this.survey.surveyID,
        clientID: this.client.clientID,
        surveyQuestionID: this.selectedQuestion.questionID,
        responseText: this.surveyFormGroup.get(this.selectedQuestion.questionID).value
      };

      this.SantaApiPost.postSurveyResponse(newResponse).subscribe((res) => {
        this.submitClickedRefreshEvent.emit(true);
        this.surveyFormGroup.get(this.selectedQuestion.questionID).reset();
        this.puttingResponse = false;
      },
      err => {
        console.log("Something went wrong!");
        console.log(err);
        this.puttingResponse = false;
      });
    }
  }
}
