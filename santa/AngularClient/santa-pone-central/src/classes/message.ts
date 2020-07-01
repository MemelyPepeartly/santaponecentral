import { ModuleWithComponentFactories } from '@angular/core';
import { EventType } from './eventType';
import { Client } from './client';

//Message History class
export class MessageHistory {
    history: Array<Message> = [];
    relationXrefID: string;
    conversationClient: ClientMeta;
    eventType: EventType;
    eventSenderClient: ClientMeta;
    eventRecieverClient: ClientMeta;

    get adminUnreadCount() : number
    {
        
        if(this.history == undefined || this.history == null || this.history.length == 0)
        {
            return 0;
        }
        else
        {
            var count = 0;
            this.history.forEach((message: Message) => {
                if(message.isMessageRead == false && message.senderClient.clientID != null)
                {
                    count += 1;
                }
            });
            return count;
        }
    }
    get memberUnreadCount() : number
    {
        
        if(this.history == undefined || this.history == null || this.history.length == 0)
        {
            return 0;
        }
        else
        {
            var count = 0;
            this.history.forEach((message: Message) => {
                if(message.isMessageRead == false && message.recieverClient.clientID != null)
                {
                    count += 1;
                }
            });
            return count;
        }
    }
}
// Message class for correspondance
export class Message {
    chatMessageID: string;
    senderClient: ClientMeta = new ClientMeta();
    recieverClient: ClientMeta = new ClientMeta();
    clientRelationXrefID?: string;
    messageContent: string;
    dateTimeSent: Date = new Date();
    isMessageRead: boolean;
}
// Minimized meta information returned from API for easily naming messages without additional API calls
export class ClientMeta {
    clientID?: string = null;
    clientName?: string = null;
    clientNickname?: string = null;
}

