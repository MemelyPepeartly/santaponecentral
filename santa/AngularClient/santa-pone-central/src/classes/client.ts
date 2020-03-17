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
export class ClientResponse {
    clientID: string;
    clientStatusID: string;
    clientName: string;
    clientNickname: string;
    address = new Address;
    email: string;

    toJSON()
    {
        return 
    }
}
