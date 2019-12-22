import { Anon } from './anon';

export interface ShippingMessage {
    anonInfo: Anon;
    shippingNumber: string;
    message: string;
}
