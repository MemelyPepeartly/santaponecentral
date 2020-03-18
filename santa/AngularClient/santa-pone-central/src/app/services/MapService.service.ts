import { Injectable } from '@angular/core';
import { Client } from '../../classes/client';
import { Status } from '../../classes/status';
import { EventType } from '../../classes/eventType';

@Injectable({
  providedIn: 'root'
})
export class MapService {

  constructor() { }

  mapClient(client)
  { 
    let mappedClient = new Client;
    
    mappedClient.clientID = client.clientID;
    mappedClient.clientName = client.clientName;
    mappedClient.email = client.email;
    mappedClient.clientNickname = client.nickname;

    mappedClient.clientStatus.statusID = client.clientStatus.statusID;
    mappedClient.clientStatus.statusDescription = client.clientStatus.statusDescription;

    mappedClient.address.addressLineOne = client.address.addressLineOne;
    mappedClient.address.addressLineTwo = client.address.addressLineOne;
    mappedClient.address.city = client.address.city;
    mappedClient.address.state = client.address.state;
    mappedClient.address.country = client.address.country;
    mappedClient.address.postalCode = client.address.postalCode;

    mappedClient.recipients = client.recipients;
    mappedClient.senders = client.senders;

    return mappedClient;
  }
  mapStatus(status)
  {
    let mappedStatus = new Status;

    mappedStatus.statusID = status.statusID;
    mappedStatus.statusDescription = status.statusDescription;

    return mappedStatus;
  }
  mapEvent(event)
  {
    let mappedEventType = new EventType;

    mappedEventType.eventTypeID = event.eventTypeID;
    mappedEventType.eventDescription = event.eventDescription;
    mappedEventType.isActive = event.isActive;

    return mappedEventType;
  }
}
