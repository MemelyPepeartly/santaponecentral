import { Address } from './address';
import { Status } from './status';
import { EventType } from './EventType';

export class Client {
    clientID: string;
    clientName: string;
    clientNickname: string;
    clientStatus = new Status;
    address = new Address;
    senders: Array<Sender> = [];
    recipients: Array<Recipient> = [];
    email: string;
}
export class Sender {
    senderClientID: string;
    senderEventTypeID: string;
}
export class Recipient {
    recipientClientID: string;
    recipientEventTypeID: string;
}
export class ClientSenderRecipientRelationship {
    clientID: string;
    clientNickname: string;
    clientName: string;
    clientEventTypeID: string;
}

