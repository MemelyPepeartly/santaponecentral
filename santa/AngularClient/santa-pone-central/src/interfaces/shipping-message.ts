import { Client } from './client';

export interface ShippingMessage {
    anonInfo: Client;
    shippingNumber: string;
    message: string;
}
