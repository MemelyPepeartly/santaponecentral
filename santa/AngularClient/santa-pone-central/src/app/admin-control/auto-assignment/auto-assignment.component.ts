import { Component, OnInit, Input } from '@angular/core';
import { Client, PossiblePairingChoices,  } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { TagConstants } from '../../shared/constants/TagConstants.enum'
import { GathererService } from 'src/app/services/gatherer.service';
import { SantaApiPostService, SantaApiGetService } from 'src/app/services/santa-api.service';
import { MapService } from 'src/app/services/mapper.service';
import { Pairing, SelectedAutoAssignmentsResponse } from 'src/classes/responseTypes';
import { StatusConstants } from 'src/app/shared/constants/StatusConstants.enum';

@Component({
  selector: 'app-auto-assignment',
  templateUrl: './auto-assignment.component.html',
  styleUrls: ['./auto-assignment.component.css']
})
export class AutoAssignmentComponent implements OnInit {
  constructor(public gatherer: GathererService,
    public SantaApiGet: SantaApiGetService,
    public mapper: MapService) { }

  @Input() allClients: Array<Client> = []

  public selectedClient: Client = new Client();
  public possiblePairings: Array<PossiblePairingChoices> = [];

  public gatheringAllClients: boolean = false;
  public gettingAssignmentPairings: boolean = false;

  public buttonClicked: boolean = false;
  public refreshList: boolean = false;

  public showClientCard: boolean = false;

  async ngOnInit() {
    this.buttonClicked = false;
    this.gatherer.gatheringAllClients.subscribe((status: boolean) => {
      this.gatheringAllClients = status
    });
    await this.gatherer.gatherAllClients();
  }
  public sortMassMailers() : Array<Client>
  {
    return this.allClients.filter((client: Client) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAILER}) && client.clientStatus.statusDescription == StatusConstants.APPROVED})
  }
  public sortMassMailRecipients() : Array<Client>
  {
    return this.allClients.filter((client: Client) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAIL_RECIPIENT}) && client.clientStatus.statusDescription == StatusConstants.APPROVED})
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

  public async updateSelectedClient(clientID: string)
  {
    this.selectedClient = this.mapper.mapClient(await this.SantaApiGet.getClientByClientID(clientID).toPromise());
  }
  public selectClient(client: Client)
  {
    this.selectedClient = client;
    this.showClientCard = true;
  }
  public async hideClientWindow()
  {
    this.showClientCard = false;
    this.selectedClient = new Client();
    if(this.refreshList)
    {
      await this.gatherer.gatherAllClients();
    }
  }
  public setRefreshListAction(event: boolean)
  {
    console.log("Seting refresh list to " + event);

    this.refreshList = event;
  }
}
