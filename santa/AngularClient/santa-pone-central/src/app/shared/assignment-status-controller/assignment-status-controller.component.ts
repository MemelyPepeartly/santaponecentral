import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { GathererService } from 'src/app/services/gatherer.service';
import { MapService } from 'src/app/services/mapService.service';
import { SantaApiPutService } from 'src/app/services/santaApiService.service';
import { AssignmentStatus } from 'src/classes/client';
import { ProfileRecipient } from 'src/classes/profile';
import { EditProfileAssignmentStatusResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-assignment-status-controller',
  templateUrl: './assignment-status-controller.component.html',
  styleUrls: ['./assignment-status-controller.component.css']
})
export class AssignmentStatusControllerComponent implements OnInit {

  constructor(private gatherer: GathererService,
    private santaApiPut: SantaApiPutService,
    private mapper: MapService) { }

  @Input() assignment: ProfileRecipient;
  @Input() clientID: string;

  @Output() newStatusPutEvent: EventEmitter<AssignmentStatus> = new EventEmitter();

  public allAssignmentStatuses: Array<AssignmentStatus> = [];

  public selectedAssignmentStatus: AssignmentStatus;

  public gatheringAllAssignmentStatuses: boolean;
  public puttingAssignmentStatus: boolean;

  ngOnInit(): void {
    this.gatherer.allAssignmentStatuses.subscribe((assignmentStatusArray: Array<AssignmentStatus>) => {
      this.allAssignmentStatuses = assignmentStatusArray;
    });
    this.gatherer.gatheringAllAssignmentsStatuses.subscribe((status: boolean) => {
      this.gatheringAllAssignmentStatuses = status;
    });
  }
  public changeAssignmentStatus()
  {
    this.puttingAssignmentStatus = true;

    let responseModel: EditProfileAssignmentStatusResponse =
    {
      assignmentStatusID: this.selectedAssignmentStatus.assignmentStatusID
    };

    this.santaApiPut.putProfileAssignmentStatus(this.clientID, this.assignment.relationXrefID, responseModel).subscribe((res) => {
      let newAssignmentStatus: AssignmentStatus = this.mapper.mapAssignmentStatus(res);
      this.newStatusPutEvent.emit(newAssignmentStatus);
    }, err => {
      console.group()
      console.log("Something went wrong!");
      console.log(err);
      console.groupEnd();

      this.puttingAssignmentStatus = false;
    });

    this.puttingAssignmentStatus = false;
  }

}
