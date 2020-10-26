import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, SurveyQA, SurveyFormOption, Survey } from 'src/classes/survey';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { SurveyConstants } from 'src/app/shared/constants/surveyConstants.enum';

@Component({
  selector: 'app-survey-form',
  templateUrl: './survey-form.component.html',
  styleUrls: ['./survey-form.component.css']
})
export class SurveyFormComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() survey: Survey = new Survey();
  @Output() validity: EventEmitter<boolean>= new EventEmitter;

  public isValid: boolean = false;
  public get isMassMailerSurvey() : boolean
  {
    return this.survey.surveyDescription == SurveyConstants.MASS_MAIL_SURVEY
  }

  public formQuestionsFormatted: Array<SurveyQA>

  public surveyFormGroup: FormGroup;

  public get surveyFormControls()
  {
    return this.surveyFormGroup.controls;
  }

  ngOnInit() {

    this.formQuestionsFormatted = this.setQuestions(this.survey.surveyQuestions)

    this.surveyFormGroup = this.formBuilder.group({});
    this.addFields();

    this.surveyFormGroup.valueChanges.subscribe(() => {
      this.isValid = this.checkValid();
      this.validity.emit(this.checkValid());
    })
  }
  addFields()
  {
    this.formQuestionsFormatted.forEach(question => {
      this.surveyFormGroup.addControl(question.surveyQuestionID, new FormControl('', [Validators.required, Validators.maxLength(4000)]))
    });
  }
  public setQuestions(questions: Array<Question>)
  {
    let formQuestions = new Array<SurveyQA>();
    for(let i =0; i<questions.length; i++)
    {
      let QAObject = new SurveyQA();
      QAObject.surveyID = this.survey.surveyID;
      QAObject.surveyQuestionID = questions[i].questionID;
      QAObject.questionText = questions[i].questionText;
      QAObject.senderCanView = questions[i].senderCanView;

      // If is a survey option list
      if(questions[i].isSurveyOptionList)
      {
        // Set to true
        QAObject.isSurveyOptionList = questions[i].isSurveyOptionList;

        // Set the options
        for(let j =0; j<questions[i].surveyOptionList.length; j++)
        {
          let formOptionObject = new SurveyFormOption();
          formOptionObject.surveyOptionID = questions[i].surveyOptionList[j].surveyOptionID;
          formOptionObject.optionText = questions[i].surveyOptionList[j].displayText;
          QAObject.surveyOptionList.push(formOptionObject);
        }
      }

      formQuestions.push(QAObject);
    }
    return formQuestions;
  }
  public checkValid() : boolean
  {
    return this.surveyFormGroup.valid
  }
  public isFirstQuestion(question: SurveyQA) : boolean
  {
    let result: boolean = false;
    if(question != undefined)
    {
      if(this.survey.surveyQuestions[0].questionID == question.surveyQuestionID)
      {
        result = true;
      }
    }
    return result;
  }
  public isLastQuestion(question: SurveyQA) : boolean
  {
    let result: boolean = false;
    if(question != undefined)
    {
      if(this.survey.surveyQuestions[this.survey.surveyQuestions.length - 1].questionID == question.surveyQuestionID)
      {
        result = true
      }
    }
    return result;
  }
}
