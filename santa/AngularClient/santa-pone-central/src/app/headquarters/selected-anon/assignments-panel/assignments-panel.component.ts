import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RelationshipMeta } from 'src/classes/client';

@Component({
  selector: 'app-assignments-panel',
  templateUrl: './assignments-panel.component.html',
  styleUrls: ['./assignments-panel.component.css']
})
export class AssignmentsPanelComponent implements OnInit {

  constructor() { }

  @Input() senders: Array<RelationshipMeta> = [];
  @Input() assignments: Array<RelationshipMeta> = [];

  @Output() relationshipSelectedEvent: EventEmitter<RelationshipMeta> = new EventEmitter<RelationshipMeta>();

  public selectedAsssignment: RelationshipMeta = new RelationshipMeta();
  public selectedSender: RelationshipMeta = new RelationshipMeta();

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
}
