import { Status } from './status';
import { Address } from './address';
import { SurveyResponse } from './survey';
import { EventType } from './eventType';
import { AssignmentStatus } from './client';
import { ClientMeta } from './message';

export class Profile
{
    clientID: string;
    clientStatus: Status = new Status();
    clientName: string;
    clientNickname: string;
    email: string;
    address: Address = new Address;
    assignments: Array<ProfileRecipient> = [];
    responses: Array<SurveyResponse> = [];
    editable: boolean;
}
export class ProfileRecipient
{
    recipientClient: ClientMeta;
    relationXrefID?: string = null;
    address: Address = new Address();
    assignmentStatus: AssignmentStatus = new AssignmentStatus();
    recipientEvent: EventType = new EventType();
    responses: Array<SurveyResponse> = [];
}
