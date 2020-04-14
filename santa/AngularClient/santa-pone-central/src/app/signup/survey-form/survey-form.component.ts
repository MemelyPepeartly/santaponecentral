import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, SurveyOption, SurveyResponse } from 'src/classes/survey';
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

  inputAnswers: Array<any>=[];
  optionAnswers: Array<SurveyOption>=[];

  answers: Array<SurveyAnswerResponse>=[];

  ngOnInit() {
    for(let i =0; i<this.questions.length;i++)
    {

    }
  }
  showThing()
  {
    console.log("inputAnswers");
    console.log(this.inputAnswers);
    console.log("OptionAnswers");
    console.log(this.optionAnswers);
    console.log("------------------------------");
    
  }
  placeInputResponse (question: Question)
  {
    
  }
}
