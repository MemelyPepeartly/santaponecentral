import { Address } from './address';
import { Status } from './status';

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
    senderNickname: string;
    senderEventTypeID: string;
}
export class Recipient {
    recipientClientID: string;
    recipientNickname: string;
    recipientEventTypeID: string;
}

