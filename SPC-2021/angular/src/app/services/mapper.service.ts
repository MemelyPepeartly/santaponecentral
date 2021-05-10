import { Injectable } from '@angular/core';
import { Client, AssignmentStatus, RelationshipMeta, AllowedAssignmentMeta, PossiblePairingChoices, StrippedClient, HQClient, InfoContainer, BaseClient} from '../../classes/client';
import { Status } from '../../classes/status';
import { EventType } from '../../classes/eventType';
import { ClientEmailResponse, ClientNameResponse, ClientNicknameResponse, ClientAddressResponse, ClientStatusResponse, SurveyApiResponse as SurveyApiResponse, TagResponse, MessageApiResponse } from 'src/classes/responseTypes';
import { Survey, Question, SurveyOption, SurveyQA, SurveyResponse, SurveyMeta } from 'src/classes/survey';
import { Tag } from 'src/classes/tag';
import { Profile, ProfileAssignment as ProfileAssignment } from 'src/classes/profile';
import { Message, ClientMeta, MessageHistory, ClientChatMeta } from 'src/classes/message';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { Address } from 'src/classes/address';
import { Note } from 'src/classes/note'
import { Category, YuleLog } from 'src/classes/yuleLogTypes';

@Injectable({
  providedIn: 'root'
})
export class MapService {

  constructor() { }

  mapClient(client) : Client
  {
    let mappedClient: Client =
    {
      clientID: client.clientID,
      clientName: client.clientName,
      email: client.email,
      clientNickname: client.nickname,
      clientStatus: this.mapStatus(client.clientStatus),
      isAdmin: client.isAdmin,
      hasAccount: client.hasAccount,
      address: this.mapAddress(client.address),
      responses: [],
      senders: [],
      assignments: [],
      tags: [],
      notes: []
    };

    client.responses.forEach(response => {
      mappedClient.responses.push(this.mapResponse(response))
    });

    client.assignments.forEach(recipient => {
      mappedClient.assignments.push(this.mapRelationshipMeta(recipient))
    });

    client.senders.forEach(sender => {
      mappedClient.senders.push(this.mapRelationshipMeta(sender))
    });

    client.tags.forEach(tag => {
      mappedClient.tags.push(this.mapTag(tag))
    });
    client.notes.forEach(note => {
      mappedClient.notes.push(this.mapNote(note))
    });

    return mappedClient;
  }
  mapBaseClient(client) : BaseClient
  {
    let mappedClient: BaseClient =
    {
      clientID: client.clientID,
      clientName: client.clientName,
      nickname: client.nickname,
      email: client.email,
      hasAccount: client.hasAccount,
      isAdmin: client.isAdmin
    }
    return mappedClient;
  }
  mapStaticClient(client) : Client
  {
    let mappedClient: Client =
    {
      clientID: client.clientID,
      clientName: client.clientName,
      email: client.email,
      clientNickname: client.nickname,
      clientStatus: new Status(),
      isAdmin: client.isAdmin,
      hasAccount: client.hasAccount,
      address: this.mapAddress(client.address),
      responses: [],
      senders: [],
      assignments: [],
      tags: [],
      notes: []
    };

    return mappedClient;
  }
  mapStrippedClient(client) : StrippedClient
  {
    let mappedStrippedClient: StrippedClient =
    {
      clientID: client.clientID,
      clientName: client.clientName,
      email: client.email,
      clientNickname: client.nickname,
      clientStatus: this.mapStatus(client.clientStatus),
      isAdmin: client.isAdmin,
      responses: [],
      tags: [],
    };

    client.responses.forEach(response => {
      mappedStrippedClient.responses.push(this.mapResponse(response))
    });
    client.tags.forEach(tag => {
      mappedStrippedClient.tags.push(this.mapTag(tag))
    });

    return mappedStrippedClient;
  }
  mapHQClient(client) : HQClient
  {
    let mappedHQClient: HQClient =
    {
      clientID: client.clientID,
      clientName: client.clientName,
      email: client.email,
      clientNickname: client.nickname,
      clientStatus: this.mapStatus(client.clientStatus),
      isAdmin: client.isAdmin,
      hasAccount: client.hasAccount,
      answeredSurveys: [],
      senders: client.senders,
      assignments: client.assignments,
      notes: client.notes,
      tags: [],
      infoContainer: new InfoContainer()
    };
    client.tags.forEach(tag => {
      mappedHQClient.tags.push(this.mapTag(tag))
    });
    client.answeredSurveys.forEach(answeredSurvey => {
      mappedHQClient.answeredSurveys.push(answeredSurvey)
    });

    return mappedHQClient;
  }
  mapInfoContainer(infoContainer) : InfoContainer
  {
    let mappedContainer: InfoContainer =
    {
      agentID: infoContainer.agentID,
      notes: [],
      senders: [],
      assignments: [],
      responses: []
    };

    infoContainer.assignments.forEach(assignment => {
      mappedContainer.assignments.push(this.mapRelationshipMeta(assignment))
    });
    infoContainer.senders.forEach(sender => {
      mappedContainer.senders.push(this.mapRelationshipMeta(sender))
    });
    infoContainer.notes.forEach(note => {
      mappedContainer.notes.push(this.mapNote(note))
    });
    infoContainer.responses.forEach(response => {
      mappedContainer.responses.push(this.mapResponse(response))
    });

    return mappedContainer;
  }
  mapProfile(profile) : Profile
  {
    let mappedProfile: Profile =
    {
      clientID: profile.clientID,
      clientName: profile.clientName,
      clientNickname: profile.nickname,
      email: profile.email,
      address: this.mapAddress(profile.address),
      assignments: [],
      responses: [],
      editable: profile.editable
    };

    profile.responses.forEach(response => {
      mappedProfile.responses.push(this.mapResponse(response));
    });
    return mappedProfile
  }
  mapProfileAssignment(recipient) : ProfileAssignment
  {
    let mappedProfileAssignment: ProfileAssignment =
    {
      recipientClient: this.mapMeta(recipient.recipientClient),
      relationXrefID: recipient.relationXrefID,
      assignmentStatus: this.mapAssignmentStatus(recipient.assignmentStatus),
      address: this.mapAddress(recipient.address),
      recipientEvent: this.mapEvent(recipient.recipientEvent),
      responses: []
    };

    recipient.responses.forEach(response => {
      mappedProfileAssignment.responses.push(this.mapResponse(response));
    });

    return mappedProfileAssignment;
  }
  mapAddress(address) : Address
  {
    let mappedAddress: Address =
    {
      addressLineOne: address.addressLineOne,
      addressLineTwo: address.addressLineTwo,
      city: address.city,
      state: address.state,
      country: address.country,
      postalCode: address.postalCode
    }
    return mappedAddress;
  }
  mapMessage(message) : Message
  {
    let mappedMessage: Message =
    {
      chatMessageID: message.chatMessageID,
      senderClient: this.mapClientChatMeta(message.senderClient),
      recieverClient: this.mapClientChatMeta(message.recieverClient),
      clientRelationXrefID: message.clientRelationXrefID,
      messageContent: message.messageContent,
      dateTimeSent: new Date(message.dateTimeSent),
      isMessageRead: message.isMessageRead,
      subjectMessage: message.subjectMessage,
      fromAdmin: message.fromAdmin,
    };

    return mappedMessage;
  }
  mapMessageHistory(messageHistory) : MessageHistory
  {
    let mappedMessageHistory: MessageHistory = <MessageHistory>(
    {
      relationXrefID: messageHistory.relationXrefID,
      eventType: this.mapEvent(messageHistory.eventType),
      assignmentSenderClient: this.mapClientChatMeta(messageHistory.assignmentSenderClient),
      assignmentRecieverClient: this.mapClientChatMeta(messageHistory.assignmentRecieverClient),
      conversationClient: this.mapClientChatMeta(messageHistory.conversationClient),
      subjectClient: this.mapClientChatMeta(messageHistory.subjectClient),
      assignmentStatus: this.mapAssignmentStatus(messageHistory.assignmentStatus),
      subjectMessages: [],
      recieverMessages: [],
      unreadCount: messageHistory.unreadCount
    });

    messageHistory.subjectMessages.forEach(message => {
      mappedMessageHistory.subjectMessages.push(this.mapMessage(message));
    });

    messageHistory.recieverMessages.forEach(message => {
      mappedMessageHistory.recieverMessages.push(this.mapMessage(message));
    });

    return mappedMessageHistory;
  }

  mapMeta(meta) : ClientMeta
  {
    let mappedMeta: ClientMeta =
    {
      clientID: meta.clientId,
      clientName: meta.clientName,
      clientNickname: meta.clientNickname,
      hasAccount: meta.hasAccount,
      isAdmin: meta.isAdmin
    }

    return mappedMeta;
  }
  mapClientChatMeta(meta) : ClientChatMeta
  {
    let mappedMeta: ClientChatMeta =
    {
      clientID: meta.clientId,
      clientNickname: meta.clientNickname,
      hasAccount: meta.hasAccount,
      isAdmin: meta.isAdmin
    }

    return mappedMeta;
  }
  mapRelationshipMeta(relationship) : RelationshipMeta
  {
    let mappedRelationshipMeta: RelationshipMeta =
    {
      relationshipClient: this.mapMeta(relationship.relationshipClient),
      eventType: this.mapEvent(relationship.eventType),
      clientRelationXrefID: relationship.clientRelationXrefID,
      assignmentStatus: this.mapAssignmentStatus(relationship.assignmentStatus),
      tags: [],
      removable: relationship.removable,
      confirmDelete: false
    };
    relationship.tags.forEach(tag => {
      mappedRelationshipMeta.tags.push(this.mapTag(tag))
    });

    return mappedRelationshipMeta
  }
  mapAllowedAssignmentMeta(allowedAssignmentMeta) : AllowedAssignmentMeta
  {
    let mappedAllowedAssignmentMeta: AllowedAssignmentMeta =
    {
      clientMeta: this.mapMeta(allowedAssignmentMeta.clientMeta),
      answeredSurveys: [],
      tags: [],
      totalSenders: allowedAssignmentMeta.totalSenders,
      totalAssignments: allowedAssignmentMeta.totalAssignments
    };

    allowedAssignmentMeta.tags.forEach(tag => {
      mappedAllowedAssignmentMeta.tags.push(this.mapTag(tag))
    });
    allowedAssignmentMeta.answeredSurveys.forEach(surveyMeta => {
      mappedAllowedAssignmentMeta.answeredSurveys.push(this.mapSurveyMeta(surveyMeta));
    });

    return mappedAllowedAssignmentMeta;
  }
  mapAssignmentStatus(assignmentStatus) : AssignmentStatus
  {
    let mappedAssignmentStatus: AssignmentStatus =
    {
      assignmentStatusID: assignmentStatus.assignmentStatusID,
      assignmentStatusName: assignmentStatus.assignmentStatusName,
      assignmentStatusDescription: assignmentStatus.assignmentStatusDescription
    };

    return mappedAssignmentStatus;
  }
  mapPossiblePairing(possiblePairing) : PossiblePairingChoices
  {
    let mappedPossiblePairing: PossiblePairingChoices =
    {
      sendingAgent: this.mapHQClient(possiblePairing.sendingAgent),
      potentialAssignments: []
    };
    possiblePairing.potentialAssignments.forEach(assignment => {
      mappedPossiblePairing.potentialAssignments.push(this.mapHQClient(assignment));
    });
    return mappedPossiblePairing;
  }
  mapStatus(status) : Status
  {
    let mappedStatus: Status =
    {
      statusID: status.statusID,
      statusDescription: status.statusDescription
    };

    return mappedStatus;
  }
  mapEvent(event) : EventType
  {
    let mappedEventType: EventType =
    {
      eventTypeID: event.eventTypeID,
      eventDescription: event.eventDescription,
      isActive: event.active,
      removable: event.removable,
      immutable: event.immutable
    };

    return mappedEventType;
  }
  mapSurvey(survey) : Survey
  {
    let mappedSurvey: Survey =
    {
      surveyID: survey.surveyID,
      eventTypeID: survey.eventTypeID,
      surveyDescription: survey.surveyDescription,
      active: survey.active,
      removable: survey.removable,
      surveyQuestions: []
    };

    survey.surveyQuestions.forEach(question => {
      mappedSurvey.surveyQuestions.push(this.mapQuestion(question));
    });

    return mappedSurvey;
  }
  mapSurveyMeta(surveyMeta) : SurveyMeta
  {
    let mappedSurveyMeta: SurveyMeta =
    {
      surveyID: surveyMeta.surveyID,
      eventTypeID: surveyMeta.eventTypeID
    };
    return mappedSurveyMeta;
  }
  mapQuestion(question) : Question
  {
    let mappedQuestion: Question =
    {
      questionID: question.questionID,
      questionText: question.questionText,
      isSurveyOptionList: question.isSurveyOptionList,
      senderCanView: question.senderCanView,
      removable: question.removable,
      surveyOptionList: []
    };

    question.surveyOptionList.forEach(surveyOption => {
      mappedQuestion.surveyOptionList.push(this.mapSurveyOption(surveyOption));
    });

    return mappedQuestion;
  }
  mapSurveyOption(surveyOption): SurveyOption
  {
    let mappedSurveyOption: SurveyOption =
    {
      surveyOptionID: surveyOption.surveyOptionID,
      displayText: surveyOption.displayText,
      surveyOptionValue: surveyOption.surveyOptionValue,
      removable: surveyOption.removable
    };

    return mappedSurveyOption;
  }
  mapResponse(surveyResponse) : SurveyResponse
  {
    let mappedSurveyResponse: SurveyResponse =
    {
      surveyResponseID: surveyResponse.surveyResponseID,
      surveyID: surveyResponse.surveyID,
      clientID: surveyResponse.clientID,
      responseEvent: this.mapEvent(surveyResponse.responseEvent),
      surveyQuestion: this.mapQuestion(surveyResponse.surveyQuestion),
      surveyOptionID: surveyResponse.surveyOptionID,
      responseText: surveyResponse.responseText,
    };

    return mappedSurveyResponse;
  }
  mapTag(tag) : Tag
  {
    let mappedTag: Tag =
    {
      tagID: tag.tagID,
      tagName: tag.tagName,
      deletable: tag.deletable,
      tagImmutable: tag.tagImmutable
    };

    return mappedTag;
  }
  mapNote(note) : Note
  {
    let mappedNote: Note =
    {
      noteID: note.noteID,
      noteSubject: note.noteSubject,
      noteContents: note.noteContents
    }
    return mappedNote;
  }
  mapCategory(category) : Category
  {
    let mappedCategory: Category =
    {
      categoryID: category.categoryID,
      categoryName: category.categoryName,
      categoryDescription: category.categoryDescription
    };
    return mappedCategory;
  }
  mapYuleLog(log) : YuleLog
  {
    let mappedYuleLog: YuleLog =
    {
      logID: log.logID,
      category: this.mapCategory(log.category),
      logDate: new Date(log.logDate),
      logText: log.logText
    };
    return mappedYuleLog;
  }
}
@Injectable({
  providedIn: 'root'
})
export class MapResponse
{
  mapMessageResponse(senderClientID, recieverClientID, relationXrefID, messageContent, fromAdmin) : MessageApiResponse
  {
    let messageResponse: MessageApiResponse =
    {
      messageSenderClientID: senderClientID,
      messageRecieverClientID: recieverClientID,
      clientRelationXrefID: relationXrefID,
      messageContent: messageContent,
      fromAdmin: fromAdmin
    };

    return messageResponse;
  }
  mapClientEmailResponse(client: Client) : ClientEmailResponse
  {
    let clientEmailResponse: ClientEmailResponse =
    {
      clientEmail: client.email
    };

    return clientEmailResponse;
  }
  mapClientNicknameResponse(client: Client) : ClientNicknameResponse
  {
    let clientNicknameResponse: ClientNicknameResponse =
    {
      clientNickname: client.clientNickname
    };

    return clientNicknameResponse;
  }
  mapClientNameResponse(client: Client) : ClientNameResponse
  {
    let clientNameResponse: ClientNameResponse =
    {
      clientName: client.clientName
    };

    return clientNameResponse;
  }
  mapClientAddressResponse(client: Client) : ClientAddressResponse
  {
    let clientAddressResponse: ClientAddressResponse =
    {
      clientAddressLine1: client.address.addressLineOne,
      clientAddressLine2: client.address.addressLineTwo,
      clientCity: client.address.city,
      clientState: client.address.state,
      clientCountry: client.address.country,
      clientPostalCode: client.address.postalCode
    };

    return clientAddressResponse
  }
  mapSurveyApiResponse(response: SurveyQA) : SurveyApiResponse
  {
    let surveyApiResponse: SurveyApiResponse =
    {
      surveyID: response.surveyID,
      clientID: response.clientID,
      surveyQuestionID: response.surveyQuestionID,
      surveyOptionID: response.isSurveyOptionList ? response.responseOptionSelected.surveyOptionID : null,
      responseText: response.isSurveyOptionList ? response.responseOptionSelected.optionText : response.responseInputText
    };

    return surveyApiResponse;
  }
  mapTagResponse(tag: Tag) : TagResponse
  {
    let tagResponse: TagResponse =
    {
      tagName: tag.tagName
    };

    return tagResponse;
  }
}
@Injectable({
  providedIn: 'root'
})
export class MissionMapper
{
  mapBoardEntry(boardEntry: any) : BoardEntry
  {
    let mappedBoardEntry: BoardEntry =
    {
      boardEntryID: boardEntry.boardEntryID,
      entryType: this.mapEntryType(boardEntry.entryType),
      postDescription: boardEntry.postDescription,
      postNumber: boardEntry.postNumber,
      threadNumber: boardEntry.threadNumber,
      dateTimeEntered: new Date(boardEntry.dateTimeEntered),
      editing: false
    };

    return mappedBoardEntry;
  }
  mapEntryType(entryType: any) : EntryType
  {
    let mappedEntryType: EntryType =
    {
      entryTypeID: entryType.entryTypeID,
      entryTypeName: entryType.entryTypeName,
      entryTypeDescription: entryType.entryTypeDescription,
      adminOnly: entryType.adminOnly
    };

    return mappedEntryType;
  }
}
