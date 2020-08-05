export class ClientResponse {
    clientStatusID: string;
    clientName: string;
    clientEmail: string;
    clientNickname: string;
    clientAddressLine1: string;
    clientAddressLine2: string;
    clientCity: string;
    clientState: string;
    clientPostalCode: string;
    clientCountry: string;
}
export class ClientSignupResponse {
    clientStatusID: string;
    clientName: string;
    clientEmail: string;
    clientNickname: string;
    clientAddressLine1: string;
    clientAddressLine2: string;
    clientCity: string;
    clientState: string;
    clientPostalCode: string;
    clientCountry: string;
    responses: Array<SurveySignupApiResponse> = []
}
export class ClientAddressResponse {
    clientAddressLine1: string;
    clientAddressLine2: string;
    clientCity: string;
    clientState: string;
    clientPostalCode: string;
    clientCountry: string;
}
export class ClientEmailResponse {
    clientEmail: string;
}
export class ClientNicknameResponse {
    clientNickname: string;
}
export class ClientNameResponse {
    clientName: string;
}
export class ClientStatusResponse {
    clientStatusID: string;
}

export class ClientRelationshipsResponse {
    eventTypeID: string;
    assignments: Array<string> = []
}
export class SurveyApiResponse {
    surveyID: string;
    clientID: string;
    surveyQuestionID: string;
    surveyOptionID?: string = null;
    responseText: string;
}
export class SurveySignupApiResponse {
    surveyID: string;
    surveyQuestionID: string;
    surveyOptionID?: string = null;
    responseText: string;
}
export class TagResponse {
    tagName: string;
}
export class ClientTagRelationshipResponse {
    clientID: string;
    tagID: string;
}
export class ClientTagRelationshipsResponse {
    tags: Array<string> = [];
}
export class ClientSenderRecipientRelationshipReponse {
    clientID: string;
    clientNickname: string;
    clientName: string;
    clientEventTypeID: string;
}
// Response model for modifying the completion status of a sender/reciever relationship
export class RecipientCompletionResponse
{
    recipientID: string;
    eventTypeID: string;
    completed: boolean;
}
// Response model for marking if a question is viewable by the person(s) sending to them
export class QuestionReadabilityResponse
{
    senderCanView: boolean;
}
// Response for making new messages
export class MessageApiResponse {
    messageSenderClientID?: string = null;
    messageRecieverClientID?: string = null;
    clientRelationXrefID?: string = null;
    eventTypeID?: string;
    messageContent: string;
    fromAdmin: boolean;
}
//Response for marking messages as read
export class MessageApiReadResponse {
    isMessageRead: boolean;
}
//Response for marking a list of messages as read
export class MessageApiReadAllResponse {
    messages: Array<string> = [];
}