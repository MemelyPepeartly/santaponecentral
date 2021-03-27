import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SantaApiGetService, SantaApiPutService, SantaApiPostService, SantaApiDeleteService } from 'src/app/services/santa-api.service';
import { MapResponse, MapService } from 'src/app/services/mapper.service';
import { GathererService } from 'src/app/services/gatherer.service';
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

  public tagsInUse: Array<Question> = [];
  public allClients: Array<Client> = [];

  public gatheringAllQuestions: boolean = false;

  async ngOnInit() {
    this.constructFormGroups();
    this.gatherer.gatheringAllQuestions.subscribe((status: boolean) => {
      this.gatheringAllQuestions = status;
    });
    await this.gatherer.gatherAllQuestions();
  }
  private constructFormGroups() {
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
