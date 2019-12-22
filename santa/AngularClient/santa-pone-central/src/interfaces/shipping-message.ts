import { Anon } from './anon';

export interface ShippingMessage {
    anonInfo: Anon;
    message: string;
    shippingNumber: string;
}
