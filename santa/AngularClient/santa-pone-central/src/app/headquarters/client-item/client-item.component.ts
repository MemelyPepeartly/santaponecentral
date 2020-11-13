import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MapService } from 'src/app/services/mapper.service';
import { SantaApiGetService } from 'src/app/services/santa-api.service';
import { HQClient, InfoContainer } from 'src/classes/client';
import { Survey } from 'src/classes/survey';

@Component({
  selector: 'app-client-item',
  templateUrl: './client-item.component.html',
  styleUrls: ['./client-item.component.css']
})
export class ClientItemComponent implements OnInit {

  constructor(public SantaApiGet: SantaApiGetService, public mapper: MapService) { }

  @Input() client: HQClient = new HQClient();
  @Input() allSurveys: Array<Survey> = [];

  @Output() clientClickedEvent: EventEmitter<HQClient> = new EventEmitter<HQClient>();

  public infoContainer: InfoContainer = new InfoContainer();

  ngOnInit(): void {
  }
  public answeredForSurvey(survey: Survey) : boolean
  {
    return this.client.answeredSurveys.some((surveyID: string) => {
      return surveyID == survey.surveyID;
    });
  }
  public getInfoContainer()
  {

  }
  public viewClientNotesClicked()
  {

  }
  public viewAssignmentsClicked()
  {

  }
  public emitClientClicked()
  {
    this.clientClickedEvent.emit(this.client);
  }
}
