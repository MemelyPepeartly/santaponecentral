import { Address } from './address';
import { Status } from './status';
import { EventType } from './eventType';
import { Tag } from './tag';
import { SurveyResponse } from './survey';
import { ClientMeta } from './message';

export class Client {
  clientID: string;
  clientName: string;
  clientNickname: string;
  clientStatus = new Status();
  isAdmin: boolean;
  hasAccount: boolean;
  address = new Address;
  email: string;
  responses: Array<SurveyResponse> = [];
  senders: Array<RelationshipMeta> = [];
  assignments: Array<RelationshipMeta> = [];
  tags: Array<Tag> = [];
}
// Class used for holding sender and event ID information
export class RelationshipMeta {
  relationshipClient: ClientMeta;
  relationshipEventTypeID: string;
  clientRelationXrefID: string;
  assignmentStatus: AssignmentStatus;
  removable: boolean;
  completed: boolean;
}
export class AssignmentStatus {
  assignmentStatusID: string;
  assignmentStatusName: string;
  assignmentStatusDescription: string;
}

