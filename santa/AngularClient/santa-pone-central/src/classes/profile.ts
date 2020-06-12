import { Status } from './status';
import { Address } from './address';
import { SurveyResponse } from './survey';

export class Profile 
{
    clientID: string;
    clientStatus = new Status;
    clientName: string;
    clientNickname: string;
    email: string;
    address = new Address;
    recipients: Array<ProfileRecipient> = [];
    responses: Array<SurveyResponse> = [];
}
export class ProfileRecipient 
{
    recipientClientID: string;
    recipientEventTypeID: string;
    clientName: string;
    clientNickname: string;
    address = new Address;
    responses: Array<SurveyResponse> = [];
}
