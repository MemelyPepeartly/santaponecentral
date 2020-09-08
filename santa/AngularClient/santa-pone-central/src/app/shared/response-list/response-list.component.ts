import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Question, SurveyResponse, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-response-list',
  templateUrl: './response-list.component.html',
  styleUrls: ['./response-list.component.css']
})
export class ResponseListComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() survey: Survey = new Survey();
  @Input() responses: Array<SurveyResponse> = [];
  @Input() editable: boolean = false;

  @Output() submitClickedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() formsValidEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public surveyFormGroup: FormGroup;


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
}
