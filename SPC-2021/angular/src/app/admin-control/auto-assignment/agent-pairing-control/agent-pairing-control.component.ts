import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatSelectionList } from '@angular/material/list';
import { ClientService } from 'src/app/services/api services/client.service';
import { HQClient, PossiblePairingChoices } from 'src/classes/client';
import { Pairing, SelectedAutoAssignmentsResponse } from 'src/classes/request-types';

@Component({
  selector: 'app-agent-pairing-control',
  templateUrl: './agent-pairing-control.component.html',
  styleUrls: ['./agent-pairing-control.component.css']
})
export class AgentPairingControlComponent implements OnInit {

  constructor(public ClientService: ClientService) { }

  @Input() possiblePairingsObject: PossiblePairingChoices = new PossiblePairingChoices();

  @Output() completedPostEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() failedPostEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public postingNewAssignments: boolean = false;

  public selectedPotentialAssignments: Array<HQClient> = [];

  @ViewChild("pairingList") public pairingList: MatSelectionList;

  ngOnInit(): void {
  }
  public async postAssignmentPairings()
  {
    this.postingNewAssignments = true;
    let pairingModel: Array<Pairing> = [];

    this.selectedPotentialAssignments.forEach((client: HQClient) => {
      let modelPair: Pairing =
        {
          senderAgentID: this.possiblePairingsObject.sendingAgent.clientID,
          assignmentClientID: client.clientID
        }
        pairingModel.push(modelPair);
    });

    let responseModel: SelectedAutoAssignmentsResponse =
    {
      pairings: pairingModel
    }

    this.ClientService.postSelectedAutoAssignments(responseModel).subscribe(async () => {
      this.completedPostEvent.emit(true);
      // Set the potential assignments that were successfully posted equal to a list without the ones that were not posted
      // Small local update to avoid having to gather everything again
      this.possiblePairingsObject.potentialAssignments = this.possiblePairingsObject.potentialAssignments.filter((client: HQClient) => {
        return !this.selectedPotentialAssignments.some((selection: HQClient) => {return selection.clientID == client.clientID
        });
      });
      this.postingNewAssignments = false;
    }, err =>
    {
      this.failedPostEvent.emit(true);
      this.postingNewAssignments = false;
      console.group()
      console.log("Something went wrong posting selected auto assignments!")
      console.log(err);
      console.groupEnd();
    });

  }
}
