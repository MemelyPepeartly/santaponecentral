import { Address } from './address';
import { Status } from './status';
import { EventType } from './eventType';
import { Tag } from './tag';
import { Survey, SurveyMeta, SurveyResponse } from './survey';
import { ClientMeta } from './message';
import { Note } from './note';

export class Client {
  clientID!: string;
  clientName!: string;
  clientNickname!: string;
  clientStatus = new Status();
  isAdmin!: boolean;
  hasAccount!: boolean;
  address = new Address;
  email!: string;
  responses: Array<SurveyResponse> = [];
  senders: Array<RelationshipMeta> = [];
  assignments: Array<RelationshipMeta> = [];
  tags: Array<Tag> = [];
  notes: Array<Note>= [];
}
export class BaseClient {
  clientID!: string;
  clientName!: string;
  nickname!: string;
  email!: string;
  isAdmin!: boolean;
  hasAccount!: boolean;
}
export class StrippedClient {
  clientID!: string;
  clientName!: string;
  clientNickname!: string;
  email!: string;
  clientStatus = new Status();
  responses: Array<SurveyResponse> = [];
  tags: Array<Tag> = [];
  isAdmin!: boolean;
}
export class HQClient {
  clientID!: string;
  clientName!: string;
  clientNickname!: string;
  clientStatus = new Status();
  isAdmin!: boolean;
  hasAccount!: boolean;
  email!: string;
  answeredSurveys: Array<string> = [];
  senders!: number;
  assignments!: number;
  notes!: number;
  tags: Array<Tag> = [];
  infoContainer: InfoContainer = new InfoContainer();
}
export class InfoContainer {
  agentID!: string;
  notes: Array<Note> = [];
  assignments: Array<RelationshipMeta> = [];
  senders: Array<RelationshipMeta> = [];
  responses: Array<SurveyResponse>= [];
}
// Class used for holding sender and event ID information
export class RelationshipMeta {
  relationshipClient: ClientMeta = new ClientMeta();
  eventType: EventType = new EventType();
  tags!: Array<Tag>;
  clientRelationXrefID!: string;
  assignmentStatus: AssignmentStatus = new AssignmentStatus();
  removable!: boolean;
  /* For assignment panel component purposes. Not a return from the API */
  confirmDelete: boolean = false;
}
export class AllowedAssignmentMeta {
  clientMeta!: ClientMeta;
  answeredSurveys: Array<SurveyMeta> = [];
  tags: Array<Tag> = [];
  totalSenders!: number;
  totalAssignments!: number;
}
export class AssignmentStatus {
  assignmentStatusID!: string;
  assignmentStatusName!: string;
  assignmentStatusDescription!: string;
}
export class PossiblePairingChoices {
  sendingAgent!: HQClient;
  potentialAssignments: Array<HQClient> = [];
}

