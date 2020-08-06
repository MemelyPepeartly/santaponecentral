import { Address } from './address';
import { Status } from './status';
import { EventType } from './eventType';
import { Tag } from './tag';
import { SurveyResponse } from './survey';

export class Client {
    clientID: string;
    clientName: string;
    clientNickname: string;
    clientStatus = new Status;
    isAdmin: boolean;
    address = new Address;
    email: string;
    responses: Array<SurveyResponse> = [];
    senders: Array<Sender> = [];
    recipients: Array<Recipient> = [];
    tags: Array<Tag> = [];
}
// Class used for holding sender and event ID information
export class Sender {
    senderClientID: string;
    senderEventTypeID: string;
    removable: boolean;
    completed: boolean;
}
// Class used for holding recipient and event ID information
export class Recipient {
    recipientClientID: string;
    recipientEventTypeID: string;
    removable: boolean;
    completed: boolean;
}
// Class used for holding smaller amounts of data for clients in the sender/reciever lists for a client. More data can be recieved by using API get methods with the clientID
export class ClientSenderRecipientRelationship {
    clientID: string;
    clientNickname: string;
    clientName: string;
    clientEventTypeID: string;
    removable: boolean;
    completed: boolean;
}

