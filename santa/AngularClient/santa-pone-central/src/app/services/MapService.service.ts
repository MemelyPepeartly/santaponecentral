import { Injectable } from '@angular/core';
import { Client, Recipient, Sender, ClientSenderRecipientRelationship } from '../../classes/client';
import { Status } from '../../classes/status';
import { EventType } from '../../classes/eventType';
import { ClientEmailResponse, ClientNameResponse, ClientNicknameResponse, ClientAddressResponse, ClientStatusResponse, SurveyApiResponse as SurveyApiResponse, TagResponse } from 'src/classes/responseTypes';
import { Survey, Question, SurveyOption, SurveyQA, SurveyResponse } from 'src/classes/survey';
import { Tag } from 'src/classes/tag';
import { Profile, ProfileRecipient } from 'src/classes/profile';

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

    client.tags.forEach(tag => {
      mappedClient.tags.push(this.mapTag(tag))
    });

    return mappedClient;
  }
  mapProfile(profile)
  {
    let mappedProfile = new Profile;

    mappedProfile.clientID = profile.clientID;
    mappedProfile.clientName = profile.clientName;
    mappedProfile.email = profile.email;
    mappedProfile.clientNickname = profile.nickname;

    mappedProfile.clientStatus.statusID = profile.clientStatus.statusID;
    mappedProfile.clientStatus.statusDescription = profile.clientStatus.statusDescription;

    mappedProfile.address.addressLineOne = profile.address.addressLineOne;
    mappedProfile.address.addressLineTwo = profile.address.addressLineTwo;
    mappedProfile.address.city = profile.address.city;
    mappedProfile.address.state = profile.address.state;
    mappedProfile.address.country = profile.address.country;
    mappedProfile.address.postalCode = profile.address.postalCode;

    profile.recipients.forEach(recipient => {
      mappedProfile.recipients.push(this.mapProfileRecipient(recipient));
    });
    profile.responses.forEach(response => {
      mappedProfile.responses.push(this.mapResponse(response));
    });
  }
  mapProfileRecipient(recipient)
  {
    let mappedProfileRecipient = new ProfileRecipient;


    recipient.responses.forEach(response => {
      mappedProfileRecipient.responses.push(this.mapResponse(response));
    });

    return mappedProfileRecipient;
  }
  mapClientRelationship(client: Client, eventTypeID: string)
  {
    let mappedRelationship = new ClientSenderRecipientRelationship;

    mappedRelationship.clientID = client.clientID;
    mappedRelationship.clientName = client.clientName;
    mappedRelationship.clientNickname = client.clientNickname;
    mappedRelationship.clientEventTypeID = eventTypeID;

    return mappedRelationship;

  }
  mapRecipient(recipient)
  {
    let mappedRecipient = new Recipient;

    mappedRecipient.recipientClientID = recipient.recipientClientID;
    mappedRecipient.recipientEventTypeID = recipient.recipientEventTypeID;

    return mappedRecipient
  }
  mapSender(sender)
  {
    let mappedSender = new Sender;

    mappedSender.senderClientID = sender.senderClientID;
    mappedSender.senderEventTypeID = sender.senderEventTypeID;

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
  mapSurvey(survey)
  {
    let mappedSurvey = new Survey;

    mappedSurvey.surveyID = survey.surveyID;
    mappedSurvey.eventTypeID = survey.eventTypeID;
    mappedSurvey.surveyDescription = survey.surveyDescription;
    mappedSurvey.active = survey.active;
    survey.surveyQuestions.forEach(question => {
      mappedSurvey.surveyQuestions.push(this.mapQuestion(question));
    });

    return mappedSurvey;
  }
  mapQuestion(question)
  {
    let mappedQuestion = new Question;

    mappedQuestion.questionID = question.questionID;
    mappedQuestion.questionText = question.questionText;
    mappedQuestion.isSurveyOptionList = question.isSurveyOptionList;
    question.surveyOptionList.forEach(surveyOption => {
      mappedQuestion.surveyOptionList.push(this.mapSurveyOption(surveyOption));
    });

    return mappedQuestion;
  }
  mapSurveyOption(surveyOption)
  {
    let mappedSurveyOption = new SurveyOption;

    mappedSurveyOption.surveyOptionID = surveyOption.surveyOptionID;
    mappedSurveyOption.displayText = surveyOption.displayText;
    mappedSurveyOption.surveyOptionValue = surveyOption.surveyOptionValue;

    return mappedSurveyOption;
  }
  mapResponse(surveyResponse)
  {
    let mappedSurveyResponse = new SurveyResponse;

    mappedSurveyResponse.surveyResponseID = surveyResponse.surveyResponseID;
    mappedSurveyResponse.surveyID = surveyResponse.surveyID;
    mappedSurveyResponse.clientID = surveyResponse.clientID;
    mappedSurveyResponse.surveyQuestionID = surveyResponse.surveyQuestionID;
    mappedSurveyResponse.surveyOptionID = surveyResponse.surveyOptionID;
    mappedSurveyResponse.responseText = surveyResponse.responseText;

    return mappedSurveyResponse;
  }
  mapTag(tag)
  {
    let mappedTag = new Tag;

    mappedTag.tagID = tag.tagID;
    mappedTag.tagName = tag.tagName;

    return mappedTag;
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
  mapSurveyApiResponse(response: SurveyQA)
  {
    let surveyApiResponse: SurveyApiResponse = new SurveyApiResponse();

    surveyApiResponse.surveyID = response.surveyID;
    surveyApiResponse.clientID = response.clientID;
    surveyApiResponse.surveyQuestionID = response.surveyQuestionID;
    if(response.isSurveyOptionList)
    {
      surveyApiResponse.surveyOptionID = response.responseOptionSelected.surveyOptionID;
      surveyApiResponse.responseText = response.responseOptionSelected.optionText;
    }
    else
    {
      surveyApiResponse.responseText = response.responseInputText;
    }
    
    return surveyApiResponse;
  }
  mapTagResponse(tag: Tag)
  {
    let tagResponse: TagResponse = new TagResponse();

    tagResponse.tagName = tag.tagName;
    
    return tagResponse;
  }
}