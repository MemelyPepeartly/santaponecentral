import { Address } from './Address';
import { Status } from './Status';

export class Client {
    clientID: string;
    clientName: string;
    clientNickname: string;
    clientStatus = new Status;
    address = new Address;
    senders: Array<string>;
    recipients: Array<string>;
    email: string;
}
