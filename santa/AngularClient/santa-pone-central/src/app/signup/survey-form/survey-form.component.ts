import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, SurveyOption } from 'src/classes/survey';
import { EventType } from 'src/classes/EventType';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-survey-form',
  templateUrl: './survey-form.component.html',
  styleUrls: ['./survey-form.component.css']
})
export class SurveyFormComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() events: Array<EventType>;
  @Input() questions: Array<Question>;

  inputAnswers: Array<any>;
  optionAnswers: Array<any>;
  onQuestion: Question = new Question();

  //Two way binding
  @Output() formEmit: EventEmitter<FormGroup> = new EventEmitter();

  ngOnInit() {
  }
  showThing()
  {
    console.log("Question");
    console.log(this.onQuestion);
    console.log("inputAnswers");
    console.log(this.inputAnswers);
    console.log("OptionAnswers");
    console.log(this.optionAnswers);
    
  }
}
