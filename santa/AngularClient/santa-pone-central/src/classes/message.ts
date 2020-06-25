import { ModuleWithComponentFactories } from '@angular/core';
import { EventType } from './eventType';

//Message History class
export class MessageHistory {
    history: Array<Message>;
    relationXrefID: string;
    eventType: EventType;
    eventSenderClient: MessageMeta;
    eventRecieverClient: MessageMeta;

    get sentUnreadCount()
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
    get recievedUnreadCount()
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

