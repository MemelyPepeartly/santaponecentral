import { Component, OnInit, Input } from '@angular/core';
import { Client, HQClient, PossiblePairingChoices,  } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { TagConstants } from 'src/app/shared/constants/tagConstants.enum'
import { GathererService } from 'src/app/services/gathering services/general-data-gatherer.service';
import { SantaApiPostService, SantaApiGetService } from 'src/app/services/santa-api.service';
import { MapService } from 'src/app/services/utility services/mapper.service';
import { Pairing, SelectedAutoAssignmentsResponse } from 'src/classes/responseTypes';
import { StatusConstants } from 'src/app/shared/constants/statusConstants.enum';

@Component({
  selector: 'app-auto-assignment',
  templateUrl: './auto-assignment.component.html',
  styleUrls: ['./auto-assignment.component.css']
})
export class AutoAssignmentComponent implements OnInit {
  constructor(public gatherer: GathererService,
    public SantaApiGet: SantaApiGetService,
    public mapper: MapService) { }

  @Input() allClients: Array<HQClient> = []

  public selectedClientID: string;
  public possiblePairings: Array<PossiblePairingChoices> = [];

  public gatheringAllClients: boolean = false;
  public gettingAssignmentPairings: boolean = false;

  public buttonClicked: boolean = false;
  public refreshList: boolean = false;

  public showClientCard: boolean = false;
  public pairingIndex: number = 0;

  async ngOnInit() {
    this.buttonClicked = false;
    this.gatherer.gatheringAllHQClients.subscribe((status: boolean) => {
      this.gatheringAllClients = status
    });
    await this.gatherer.gatherAllHQClients();
  }
  public sortMassMailers() : Array<HQClient>
  {
    return this.allClients.filter((client: HQClient) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAILER}) && client.clientStatus.statusDescription == StatusConstants.APPROVED})
  }
  public sortMassMailRecipients() : Array<HQClient>
  {
    return this.allClients.filter((client: HQClient) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAIL_RECIPIENT}) && client.clientStatus.statusDescription == StatusConstants.APPROVED})
  }
  public async getAssignmentPairings()
  {
    this.buttonClicked = true;
    this.gettingAssignmentPairings = true;

    var response = await this.SantaApiGet.getAutoAssignmentPairings().toPromise();
    this.possiblePairings = [];
    response.forEach((pairing) => {
      this.possiblePairings.push(this.mapper.mapPossiblePairing(pairing));
    });

    this.gettingAssignmentPairings = false;
  }
  public selectClient(client: HQClient)
  {
    this.selectedClientID = client.clientID;
    this.showClientCard = true;
  }
  public async hideClientWindow()
  {
    this.showClientCard = false;
    this.selectedClientID = undefined;
    if(this.refreshList)
    {
      await this.gatherer.gatherAllHQClients();
    }
  }
  public setRefreshListAction(event: boolean)
  {
    this.refreshList = event;
  }
  public previousMailer()
  {
    this.pairingIndex -= 1;
  }
  public nextMailer()
  {
    this.pairingIndex += 1;
  }
}
