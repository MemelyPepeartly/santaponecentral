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
  @Input() showSenderAssignmentCount: boolean;
  @Input() showAgentName: boolean;
  @Input() showAgentNickname: boolean;
  @Input() showCompletedSurveys: boolean;

  @Output() clientClickedEvent: EventEmitter<HQClient> = new EventEmitter<HQClient>();

  public infoContainer: InfoContainer = new InfoContainer();

  public showAssignments: boolean;
  public showNotes: boolean;
  public showSurveyResponses: boolean;
  public get infoSectionOpen() : boolean
  {
    return this.showAssignments || this.showNotes || this.showSurveyResponses
  }

  ngOnInit(): void {
  }
  public answeredForSurvey(survey: Survey) : boolean
  {
    return this.client.answeredSurveys.some((surveyID: string) => {
      return surveyID == survey.surveyID;
    });
  }
  public async getInfoContainer()
  {
    //If the info container isnt filled => Get the info container from the API
    if(this.infoContainer.agentID == undefined)
    {
      this.infoContainer = this.mapper.mapInfoContainer(await this.SantaApiGet.getInfoContainerByClientID(this.client.clientID).toPromise())
    }
    //Else, just show the info container contents. No second API call needed
  }
  public emitClientClicked()
  {
    this.clientClickedEvent.emit(this.client);
  }
}
