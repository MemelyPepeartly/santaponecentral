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

    /* Unread counts are based on unread messages that are not the subject's messages */
    get unreadCount() : number
    {

        if(this.recieverMessages == undefined || this.recieverMessages == null || this.recieverMessages.length == 0)
        {
            return 0;
        }
        else
        {
            var count = 0;
            this.recieverMessages.forEach((message: Message) => {
                if(message.isMessageRead == false)
                {
                    count += 1;
                }
            });
            return count;
        }
    }
    get adminUnreadCount() : number
    {

        if(this.recieverMessages == undefined || this.recieverMessages == null || this.recieverMessages.length == 0)
        {
            return 0;
        }
        else
        {
            var count = 0;
            this.recieverMessages.forEach((message: Message) => {
                if(message.isMessageRead == false && !message.fromAdmin)
                {
                    count += 1;
                }
            });
            return count;
        }
    }
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
}

