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

  //Two way binding
  @Output() formEmit: EventEmitter<FormGroup> = new EventEmitter();

  ngOnInit() {
  }
  set form(value)
  {
    
  }
}
