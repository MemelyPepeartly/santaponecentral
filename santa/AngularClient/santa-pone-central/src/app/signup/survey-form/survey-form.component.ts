import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, SurveyQA, SurveyFormOption } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-survey-form',
  templateUrl: './survey-form.component.html',
  styleUrls: ['./survey-form.component.css']
})
export class SurveyFormComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() surveyID: string;
  @Input() questions: Array<Question>;
  @Output() isCompleted: EventEmitter<boolean>= new EventEmitter;

  public formQuestionsFormatted: Array<SurveyQA>

  public surveyFormGroup: FormGroup;

  ngOnInit() {
    
    this.formQuestionsFormatted = this.setQuestions(this.questions)

    this.surveyFormGroup = this.formBuilder.group({});
    this.addFields();

    this.surveyFormGroup.valueChanges.subscribe(() => {
      this.isCompleted.emit(this.checkValid());
    })
  }
  addFields()
  {
    this.formQuestionsFormatted.forEach(question => {
      this.surveyFormGroup.addControl(question.surveyQuestionID, new FormControl('', Validators.required))
    });
  }
  public setQuestions(questions: Array<Question>)
  {
    let formQuestions = new Array<SurveyQA>();
    for(let i =0; i<questions.length; i++)
    {
      let q = new SurveyQA();
      q.surveyID = this.surveyID;
      q.surveyQuestionID = questions[i].questionID;
      q.questionText = questions[i].questionText;
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
  public checkValid() : boolean
  {
    console.log(this.surveyFormGroup.valid);
    return this.surveyFormGroup.valid
  }
}
