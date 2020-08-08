import { Component, OnInit, Input } from '@angular/core';
import { Client } from 'src/classes/client';
import { Tag } from 'src/classes/tag';
import { TagConstants } from '../../shared/constants/tagConstants.enum'
import { GathererService } from 'src/app/services/gatherer.service';
import { SantaApiPostService } from 'src/app/services/santaApiService.service';

@Component({
  selector: 'app-auto-assignment',
  templateUrl: './auto-assignment.component.html',
  styleUrls: ['./auto-assignment.component.css']
})
export class AutoAssignmentComponent implements OnInit {
  constructor(public gatherer: GathererService, public SantaApiPost: SantaApiPostService) { }

  @Input() allClients: Array<Client> = []

  public gatheringAllClients: boolean = false;
  public postingNewAssignments: boolean = false;

  public buttonClicked: boolean;

  public logArray: Array<string> = []

  ngOnInit(): void {
    this.buttonClicked = false;
    this.gatherer.gatheringAllClients.subscribe((status: boolean) => {
      this.gatheringAllClients = status
    });
  }
  public sortMassMailers() : Array<Client>
  {
    return this.allClients.filter((client: Client) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAILER})})
  }
  public sortMassMailRecipients() : Array<Client>
  {
    return this.allClients.filter((client: Client) => {return client.tags.some((tag: Tag) => {return tag.tagName == TagConstants.MASS_MAIL_RECIPIENT})})
  }
  public async giveAssignments()
  {
    this.logArray = []
    this.buttonClicked = true;
    this.postingNewAssignments = true;

    var logResponse = await this.SantaApiPost.postAutoAssignmentRequest().toPromise();
    logResponse.forEach((entry:string) => {
      this.logArray.push(entry)
    });

    this.postingNewAssignments = false;
  }

}
