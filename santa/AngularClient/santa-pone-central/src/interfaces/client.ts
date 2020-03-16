import { Address } from './address';

export interface Client {
    clientID: string;
    clientName: string;
    clientNickname: string;
    address: Address;
    clientSenderList: Array<string>;
    clientRecieverList: Array<string>;
    email: string;
    otherInfo: string;
}
