import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AssignmentStatusControllerComponent } from 'src/app/shared/assignment-status-controller/assignment-status-controller.component';
import { Client, RelationshipMeta } from 'src/classes/client';
import { ProfileAssignment } from 'src/classes/profile';

@Component({
  selector: 'app-assignments-panel',
  templateUrl: './assignments-panel.component.html',
  styleUrls: ['./assignments-panel.component.css']
})
export class AssignmentsPanelComponent implements OnInit {

  constructor() { }

  @Input() senders: Array<RelationshipMeta> = [];
  @Input() assignments: Array<RelationshipMeta> = [];
  @Input() agent: Client;
  @Input() infoOnly: boolean = false;

  @Output() updatedStatusEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() clickAwayAllowedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() removeClickedEvent: EventEmitter<RelationshipMeta> = new EventEmitter<RelationshipMeta>();
  @Output() switchClickedEvent: EventEmitter<RelationshipMeta> = new EventEmitter<RelationshipMeta>();


  @ViewChild("assignmentControl") assignmentStatusController: AssignmentStatusControllerComponent;
  @ViewChild("senderControl") senderStatusController: AssignmentStatusControllerComponent;

  public selectedAsssignment: RelationshipMeta = new RelationshipMeta();
  public selectedSender: RelationshipMeta = new RelationshipMeta();

  public get mappedSelectedAssignment() : ProfileAssignment
  {
    let mappedSelectedAssignment: ProfileAssignment =
    {
      recipientClient: this.selectedAsssignment.relationshipClient,
      relationXrefID: this.selectedAsssignment.clientRelationXrefID,
      address: undefined,
      assignmentStatus: this.selectedAsssignment.assignmentStatus,
      recipientEvent: this.selectedAsssignment.eventType,
      responses: []
    }
    return mappedSelectedAssignment;
  }

  public get mappedSelectedSender() : ProfileAssignment
  {
    let mappedSelectedSender: ProfileAssignment =
    {
      recipientClient: this.selectedSender.relationshipClient,
      relationXrefID: this.selectedSender.clientRelationXrefID,
      address: undefined,
      assignmentStatus: this.selectedSender.assignmentStatus,
      recipientEvent: this.selectedSender.eventType,
      responses: []
    }
    return mappedSelectedSender;
  }

  ngOnInit(): void {
  }
  public selectAssignment(relationship: RelationshipMeta)
  {
    this.selectedAsssignment = relationship;
    if(this.assignmentStatusController)
    {
      this.assignmentStatusController.showSuccess = false;
      this.assignmentStatusController.showError = false;
    }
  }
  public selectSender(relationship: RelationshipMeta)
  {
    this.selectedSender = relationship;
    if(this.senderStatusController)
    {
      this.senderStatusController.showSuccess = false;
      this.senderStatusController.showError = false;
    }

  }
  public setClickawayLock(status: boolean)
  {
    this.clickAwayAllowedEvent.emit(status);
  }
  public setNewStatus(relationshipMeta: RelationshipMeta, isSender: boolean = false)
  {
    if(!isSender)
    {
      this.assignments.find((meta: RelationshipMeta) => {return meta.clientRelationXrefID == this.selectedAsssignment.clientRelationXrefID}).assignmentStatus = relationshipMeta.assignmentStatus;
    }
    else
    {
      this.senders.find((meta: RelationshipMeta) => {return meta.clientRelationXrefID == this.selectedSender.clientRelationXrefID}).assignmentStatus = relationshipMeta.assignmentStatus;
    }
    this.updatedStatusEvent.emit(true);
  }
  public emitSwitchAnon(relationship: RelationshipMeta)
  {
    this.switchClickedEvent.emit(relationship);
  }
  public emitRemoveRecipient(relationship: RelationshipMeta)
  {
    this.removeClickedEvent.emit(relationship);
    if(this.assignments.some((relation: RelationshipMeta) => {return relation.clientRelationXrefID == relationship.clientRelationXrefID}))
    {
      console.log("Got here");

      this.selectedAsssignment = new RelationshipMeta();
    }
    else if(this.senders.some((relation: RelationshipMeta) => {return relation.clientRelationXrefID == relationship.clientRelationXrefID}))
    {
      console.log("Got here");

      this.selectedSender = new RelationshipMeta();
    }
  }
}
