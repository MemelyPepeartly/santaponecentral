import { Client } from './client';

export interface Message {
    anonInfo: Client;
    subject: string;
    message: string;
}
