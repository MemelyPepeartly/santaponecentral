import { ModuleWithComponentFactories } from '@angular/core';
import { EventType } from './eventType';
import { AssignmentStatus, Client } from './client';

//Message History class
export class MessageHistory {
    relationXrefID: string;
    eventType: EventType = new EventType();
    assignmentStatus: AssignmentStatus;

    // Client event sender... (Relationship dependent)
    assignmentSenderClient: ClientMeta = new ClientMeta();
    // Client event reciever... (Relationship dependent)
    assignmentRecieverClient: ClientMeta = new ClientMeta();
    // Client chat is with (Relationship Agnostic)
    conversationClient: ClientMeta = new ClientMeta();
    // The client that should be the "viewer" of the messages. Their messages should be blue
    subjectClient: ClientMeta = new ClientMeta();


    subjectMessages: Array<Message> = [];
    recieverMessages: Array<Message> = [];

    // Number of unread reciever messages
    unreadCount: number;
}
// Message class for correspondence
export class Message {
    chatMessageID: string;
    senderClient: ClientMeta = new ClientMeta();
    recieverClient: ClientMeta = new ClientMeta();
    clientRelationXrefID?: string;
    messageContent: string;
    dateTimeSent: Date;
    isMessageRead: boolean;
    // Determined when getting histories if something is a message of a particular subject (Hence, making it a blue message)
    subjectMessage: boolean;
    fromAdmin: boolean;
}
// Minimized meta information returned from API for easily naming messages without additional API calls
export class ClientMeta {
    clientID?: string = null;
    clientName?: string = null;
    clientNickname?: string = null;
    hasAccount: boolean;
    isAdmin: boolean;
}

