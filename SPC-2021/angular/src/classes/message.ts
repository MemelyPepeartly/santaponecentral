import { ModuleWithComponentFactories } from '@angular/core';
import { EventType } from './EventType';
import { AssignmentStatus, Client } from './client';

//Message History class
export class MessageHistory {
    relationXrefID!: string;
    eventType: EventType = new EventType();
    assignmentStatus!: AssignmentStatus;

    // Client event sender... (Relationship dependent)
    assignmentSenderClient: ClientChatMeta = new ClientChatMeta();
    // Client event reciever... (Relationship dependent)
    assignmentRecieverClient: ClientChatMeta = new ClientChatMeta();
    // Client chat is with (Relationship Agnostic)
    conversationClient: ClientChatMeta = new ClientChatMeta();
    // The client that should be the "viewer" of the messages. Their messages should be blue
    subjectClient: ClientChatMeta = new ClientChatMeta();


    subjectMessages: Array<Message> = [];
    recieverMessages: Array<Message> = [];

    // Number of unread reciever messages
    // Number of unread reciever messages
    unreadCount!: number;
}
// Message class for correspondence
export class Message {
    chatMessageID!: string;
    senderClient: ClientChatMeta = new ClientChatMeta();
    recieverClient: ClientChatMeta = new ClientChatMeta();
    clientRelationXrefID?: string;
    messageContent!: string;
    dateTimeSent!: Date;
    isMessageRead!: boolean;
    // Determined when getting histories if something is a message of a particular subject (Hence, making it a blue message)
    // Determined when getting histories if something is a message of a particular subject (Hence, making it a blue message)
    subjectMessage!: boolean;
    fromAdmin!: boolean;
}
// Minimized meta information returned from API for easily naming messages without additional API calls
export class ClientMeta {
    clientID?: string;
    clientName?: string;
    clientNickname?: string;
    hasAccount!: boolean;
    isAdmin!: boolean;
}
export class ClientChatMeta {
    clientID?: string;
    clientNickname?: string;
    hasAccount!: boolean;
    isAdmin!: boolean;
}
/**
 * This is the container of information to help minimize confusion on ID's needed to parse histories for the chat window. It contains a number of different
 *
 * @property messageSenderID This is the ID of the person sending
 * @property messageRecieverID This is the ID of the person recieving the messages
 * @property senderIsAdmin This is a boolean that dictates if the sender is or is not an admin. This is false by default.
 * @property conversationClientID This is the ID of the owner of the chat history. That is to say, the ID of the agent who holds the general chat or assignment
 * @property relationshipXrefID This is the ID of the assignment relationship if one exists
 * @property eventTypeID This is the ID of the event of the assignment relationship if one exists
 */
export class ChatInfoContainer {
  messageSenderID!: string;
  messageRecieverID!: string;
  senderIsAdmin: boolean = false;
  conversationClientID!: string;
  relationshipXrefID!: string;
  eventTypeID!: string;
}
