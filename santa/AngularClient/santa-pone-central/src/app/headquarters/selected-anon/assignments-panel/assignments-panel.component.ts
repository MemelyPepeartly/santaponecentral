import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AssignmentStatus, Client, RelationshipMeta } from 'src/classes/client';
import { ProfileRecipient } from 'src/classes/profile';

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

  @Output() relationshipSelectedEvent: EventEmitter<RelationshipMeta> = new EventEmitter<RelationshipMeta>();

  public selectedAsssignment: RelationshipMeta = new RelationshipMeta();
  public selectedSender: RelationshipMeta = new RelationshipMeta();

  public get mappedSelectedAssignment() : ProfileRecipient
  {
    let mappedSelectedAssignment: ProfileRecipient =
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

  public get mappedSelectedSender() : ProfileRecipient
  {
    let mappedSelectedSender: ProfileRecipient =
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
  }
  public selectSender(relationship: RelationshipMeta)
  {
    this.selectedSender = relationship;
  }
  public setClickawayLock()
  {

  }
  public setNewStatus(assignmentStatus: AssignmentStatus)
  {

  }
}
