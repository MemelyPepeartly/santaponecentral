import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/SantaApiService.service';
import { MapResponse, MapService } from 'src/app/services/MapService.service';
import { GathererService } from 'src/app/services/Gatherer.service';
import { Client } from 'src/classes/client';
import { Question } from 'src/classes/survey';

@Component({
  selector: 'app-question-control',
  templateUrl: './question-control.component.html',
  styleUrls: ['./question-control.component.css']
})
export class QuestionControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService,
    public ResponseMapper: MapResponse,
    public gatherer: GathererService,
    public ApiMapper: MapService) { }

  @Input() allQuestions: Array<Question> = [];

  public addQuestionFormGroup: FormGroup;
  public editQuestionFormGroup: FormGroup;


  public selectedQuestion: Question;
  public deletableQuestions: Array<Question> = [];
  public tagsInUse: Array<Question> = [];
  public allClients: Array<Client> = [];

  public postingNewQuestion: boolean = false;
  public updatingQuestionName: boolean = false;
  public deletingQuestion: boolean = false;

  // Getters for form values for ease-of-use
  get newQuestion() {
    var formControlObj = this.addQuestionFormGroup.get('newQuestion') as FormControl
    return formControlObj.value
  }
  get editedQuestionName() {
    var formControlObj = this.editQuestionFormGroup.get('editQuestion') as FormControl
    return formControlObj.value
  }

  ngOnInit(): void {
    this.constructFormGroups();
  }
  private constructFormGroups() {
    this.addQuestionFormGroup = this.formBuilder.group({
      newQuestion: [null, Validators.nullValidator && Validators.pattern],
    });
    this.editQuestionFormGroup = this.formBuilder.group({
      editQuestion: [null, Validators.nullValidator && Validators.pattern],
    });
  }
  public deleteQuestion(question: Question)
  {

  }
  public editQuestion()
  {

  }
  public setSelectedQuestion(question: Question)
  {

  }
  public unsetSelectedQuestion()
  {

  }
  public addNewQuestion()
  {
    
  }
}
