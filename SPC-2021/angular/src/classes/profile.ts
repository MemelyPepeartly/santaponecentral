import { Status } from './status';
import { Address } from './address';
import { SurveyResponse } from './survey';
import { EventType } from './EventType';
import { AssignmentStatus } from './client';
import { ClientMeta } from './message';

export class Profile
{
    clientID!: string;
    clientName!: string;
    clientNickname!: string;
    email!: string;
    address: Address = new Address;
    assignments: Array<ProfileAssignment> = [];
    responses: Array<SurveyResponse> = [];
    editable!: boolean;
}
export class ProfileAssignment
{
    recipientClient!: ClientMeta;
    relationXrefID?: string;
    address: Address = new Address();
    assignmentStatus: AssignmentStatus = new AssignmentStatus();
    recipientEvent: EventType = new EventType();
    responses: Array<SurveyResponse> = [];
}
