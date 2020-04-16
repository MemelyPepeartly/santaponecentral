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
    cliendID: string;
    surveyQuestionID: string;
    surveyOptionID?: string;
    responseText: string;
}