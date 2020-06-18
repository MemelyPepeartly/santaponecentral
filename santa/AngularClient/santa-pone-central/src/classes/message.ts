import { ModuleWithComponentFactories } from '@angular/core';

// Message class for correspondance
export class Message {
    chatMessageID: string;
    senderClient: MessageMeta;
    recieverClient: MessageMeta;
    clientRelationXrefID?: string;
    messageContent: string;
    dateTimeSent: Date = new Date();
    isMessageRead: boolean;
}
// Minimized meta information returned from API for easily naming messages without additional API calls
export class MessageMeta {
    clientID?: string;
    clientName?: string;
    clientNickname?: string;
}
// A history is for one-sided histories of messages
export class History {
    messages: Array<Message> = []
}
