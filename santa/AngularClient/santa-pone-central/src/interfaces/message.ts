import { Anon } from './anon';

export interface Message {
    anonInfo: Anon;
    subject: string;
    message: string;
}
