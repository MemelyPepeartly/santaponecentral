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
export class ClientRelationshipResponse {
    recieverClientID: string;
    eventTypeID: string;
}
export class SurveyApiResponse {
    surveyID: string;
    clientID: string;
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
//Response for making new messages
export class MessageApiResponse {
    messageSenderClientID?: string = null;
    messageRecieverClientID?: string = null;
    clientRelationXrefID?: string = null;
    messageContent: string;
}
//Response for marking messages as read
export class MessageApiReadResponse {
    isMessageRead: boolean;
}
//Response type for approving anons and sending a new client to the Auth0 API
export class Auth0NewClientResponse {
    email: string;
    phone_number: string;
    user_metadata: {};
    blocked: boolean;
    email_verified: boolean;
    phone_verified: boolean;
    app_metadata: {};
    given_name: string;
    family_name: string;
    name: string;
    nickname: string;
    picture: string;
    user_id: string;
    connection: string;
    password: string;
    verify_email: boolean;
    username: string;
}