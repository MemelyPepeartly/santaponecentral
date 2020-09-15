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
  @Output() lockClickawayEvent: EventEmitter<boolean> = new EventEmitter();

  public allAssignmentStatuses: Array<AssignmentStatus> = [];
  public get allowedAssignmentStatuses()
  {
    return this.allAssignmentStatuses.filter((status: AssignmentStatus) => {return status.assignmentStatusID != this.assignment.assignmentStatus.assignmentStatusID})
  }

  public selectedAssignmentStatus: AssignmentStatus = new AssignmentStatus();

  public gatheringAllAssignmentStatuses: boolean;
  public puttingAssignmentStatus: boolean;

  public openChangeForm: boolean = false;
  public showSuccess: boolean = false;
  public showError: boolean = false;


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
    this.lockClickawayEvent.emit(this.puttingAssignmentStatus);

    let responseModel: EditProfileAssignmentStatusResponse =
    {
      assignmentStatusID: this.selectedAssignmentStatus.assignmentStatusID
    };

    this.santaApiPut.putProfileAssignmentStatus(this.clientID, this.assignment.relationXrefID, responseModel).subscribe((res) => {
      let newAssignmentStatus: AssignmentStatus = this.mapper.mapAssignmentStatus(res);
      this.newStatusPutEvent.emit(newAssignmentStatus);

      this.puttingAssignmentStatus = false;
      this.lockClickawayEvent.emit(this.puttingAssignmentStatus);
      this.openChangeForm = false;
      this.showSuccess = true;
      this.showError = false;
      this.selectedAssignmentStatus = new AssignmentStatus();
    }, err => {
      console.group()
      console.log("Something went wrong!");
      console.log(err);
      console.groupEnd();

      this.puttingAssignmentStatus = false;
      this.lockClickawayEvent.emit(this.puttingAssignmentStatus);
      this.showError = true;
      this.showSuccess = false;
    });
  }
  public setSelectedStatus(assignmentStatus: AssignmentStatus)
  {
    this.selectedAssignmentStatus = assignmentStatus;
  }
}
