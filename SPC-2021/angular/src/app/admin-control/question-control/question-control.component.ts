import { Component, OnInit, Input } from '@angular/core';
import { MapResponse, MapService } from 'src/app/services/utility services/mapper.service';
import { GeneralDataGathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { Client } from 'src/classes/client';
import { Question } from 'src/classes/survey';

@Component({
  selector: 'app-question-control',
  templateUrl: './question-control.component.html',
  styleUrls: ['./question-control.component.css']
})
export class QuestionControlComponent implements OnInit {

  constructor(public ResponseMapper: MapResponse,
    private gatherer: GeneralDataGathererService,
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
