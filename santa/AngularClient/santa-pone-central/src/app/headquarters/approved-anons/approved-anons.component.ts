import { Component, OnInit, Output, Input } from '@angular/core';
import { Client } from '../../../classes/client';
import { Address } from '../../../classes/address';
import { SantaApiGetService } from '../../services/santa-api.service';
import { EventEmitter } from '@angular/core';
import { MapService } from '../../services/mapper.service';
import { StatusConstants } from 'src/app/shared/constants/StatusConstants.enum';
import { GathererService } from 'src/app/services/gatherer.service';
import { Survey, SurveyResponse } from 'src/classes/survey';

@Component({
  selector: 'app-approved-anons',
  templateUrl: './approved-anons.component.html',
  styleUrls: ['./approved-anons.component.css']
})
export class ApprovedAnonsComponent implements OnInit {

  constructor(public SantaApi: SantaApiGetService, public mapper: MapService, public gatherer: GathererService) { }

  @Output() clickedClient: EventEmitter<any> = new EventEmitter();

  @Input() approvedClients: Array<Client> = [];
  @Input() gatheringInfo: boolean;
  @Input() allSurveys: Array<Survey> = [];

  actionTaken: boolean = false;
  showSpinner: boolean = false;


  ngOnInit() {
  }
  showCardInfo(client)
  {
    this.clickedClient.emit(client);
  }
  setAction(event: boolean)
  {
    this.actionTaken = event;
  }
  async manualRefresh()
  {
    this.showSpinner = true;
    await this.gatherer.gatherAllClients();
    this.showSpinner = false;
    this.actionTaken = false;
  }
  answeredForSurvey(client: Client, survey: Survey) : boolean
  {
    return client.responses.some((response: SurveyResponse) => {
      return response.surveyID == survey.surveyID;
    });
  }
}
