import { Injectable } from '@angular/core';
import { Client, Recipient, Sender } from '../../classes/client';
import { Status } from '../../classes/status';
import { EventType } from '../../classes/EventType';
import { ClientEmailResponse, ClientNameResponse, ClientNicknameResponse, ClientAddressResponse, ClientStatusResponse } from 'src/classes/responseTypes';

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
    mappedClient.address.addressLineTwo = client.address.addressLineTwo;
    mappedClient.address.city = client.address.city;
    mappedClient.address.state = client.address.state;
    mappedClient.address.country = client.address.country;
    mappedClient.address.postalCode = client.address.postalCode;

    client.recipients.forEach(recipient => {
      mappedClient.recipients.push(this.mapRecipient(recipient))
    });

    client.senders.forEach(sender => {
      mappedClient.senders.push(this.mapSender(sender))
    });

    return mappedClient;
  }
  mapRecipient(recipient)
  {
    let mappedRecipient = new Recipient;

    mappedRecipient.recipientClientID = recipient.recipientClientID;
    mappedRecipient.recipientEventTypeID = recipient.recipientEventTypeID;
    mappedRecipient.recipientNickname = recipient.recipientNickname;

    return mappedRecipient
  }
  mapSender(sender)
  {
    let mappedSender = new Sender;

    mappedSender.senderClientID = sender.senderClientID;
    mappedSender.senderEventTypeID = sender.senderEventTypeID;
    mappedSender.senderNickname = sender.senderNickname;

    return mappedSender
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
@Injectable({
  providedIn: 'root'
})
export class MapResponse
{
  mapClientEmailResponse(client: Client)
  {
    let clientEmailResponse: ClientEmailResponse = new ClientEmailResponse();
    clientEmailResponse.clientEmail = client.email
    return clientEmailResponse;
  }
  mapClientNicknameResponse(client: Client)
  {
    let clientNicknameResponse: ClientNicknameResponse = new ClientNicknameResponse();
    clientNicknameResponse.clientNickname = client.clientNickname;
    return clientNicknameResponse;
  }
  mapClientNameResponse(client: Client)
  {
    let clientNameResponse: ClientNameResponse = new ClientNameResponse();
    clientNameResponse.clientName = client.clientName;
    return clientNameResponse;
  }
  mapClientAddressResponse(client: Client)
  {
    let clientAddressResponse: ClientAddressResponse = new ClientAddressResponse();

    clientAddressResponse.clientAddressLine1 = client.address.addressLineOne;
    clientAddressResponse.clientAddressLine2 = client.address.addressLineTwo;
    clientAddressResponse.clientCity = client.address.city;
    clientAddressResponse.clientState = client.address.state;
    clientAddressResponse.clientCountry = client.address.country;
    clientAddressResponse.clientPostalCode = client.address.postalCode;
    
    return clientAddressResponse
  }
  mapClientStatusResponse(client: Client)
  {
    let clientStatusResponse: ClientStatusResponse = new ClientStatusResponse();
    clientStatusResponse.clientStatusID = client.clientStatus.statusID;
    return clientStatusResponse;
  }
}