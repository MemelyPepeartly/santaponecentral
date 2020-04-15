import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, SurveyOption, SurveyResponse, SurveyFormQuestion, SurveyFormOption } from 'src/classes/survey';
import { EventType } from 'src/classes/EventType';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyAnswerResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-survey-form',
  templateUrl: './survey-form.component.html',
  styleUrls: ['./survey-form.component.css']
})
export class SurveyFormComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() surveyID: string;
  @Input() questions: Array<Question>;
  formQuestionsFormatted: Array<SurveyFormQuestion>

  inputAnswers: Array<any>=[];
  optionAnswers: Array<SurveyOption>=[];

  ngOnInit() {
    this.formQuestionsFormatted = this.setQuestions(this.questions)
  }
  public setQuestions(questions: Array<Question>)
  {
    console.log("Got to setQuestions");
    
    let formQuestions = new Array<SurveyFormQuestion>();
    for(let i =0; i<questions.length; i++)
    {
      let q = new SurveyFormQuestion();
      q.surveyQuestionID = questions[i].questionID;
      if(questions[i].isSurveyOptionList)
      q.isSurveyOptionList = questions[i].isSurveyOptionList;
      {
        for(let j =0; j<questions[i].surveyOptionList.length; j++)
        {
          let o = new SurveyFormOption();
          o.surveyOptionID = questions[i].surveyOptionList[j].surveyOptionID;
          o.optionText = questions[i].surveyOptionList[j].displayText;
          q.surveyOptionList.push(o);
        }
      }
      
      formQuestions.push(q);
    }
    return formQuestions;
  }
}
