import { Address } from './address';
import { Status } from './status';

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
    clientStatusID: string;
    clientName: string;
    clientEmail: string;
    clientNickname: string;
    clientAddressLine1: string;
    clientAddressLine2: string;
    clientCity: string;
    clientState: string;
    clientPostalCode: string;
    clientCountry: string;
}
