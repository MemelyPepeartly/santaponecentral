import { Status } from './status';
import { Address } from './address';
import { SurveyResponse } from './survey';
import { EventType } from './eventType';

export class Profile 
{
    clientID: string;
    clientStatus: Status = new Status();
    clientName: string;
    clientNickname: string;
    email: string;
    address: Address = new Address;
    recipients: Array<ProfileRecipient> = [];
    responses: Array<SurveyResponse> = [];
}
export class ProfileRecipient 
{
    clientID: string;
    relationXrefID?: string = null;
    clientName: string;
    clientNickname: string;
    address: Address = new Address();
    recipientEvent: EventType = new EventType();
    responses: Array<SurveyResponse> = [];
}
