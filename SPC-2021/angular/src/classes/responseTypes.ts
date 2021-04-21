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
    isAdmin: boolean = false;
    hasAccount: boolean = false;
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
    wantsAccount: boolean;
}
export class ClientIsAdminResponse {
    isAdmin: boolean;
}

export class ClientRelationshipsResponse {
    eventTypeID: string;
    assignmentStatusID: string;
    assignments: Array<string> = []
}
export class SurveyApiResponse {
    surveyID: string;
    clientID: string;
    surveyQuestionID: string;
    surveyOptionID?: string = null;
    responseText: string;
}
// Used for posting multiple questions to a survey relationship to add questions to a survey
export class SurveyQuestionXrefsResponseModel {
  questions: Array<string> = [];
}
export class ChangeSurveyResponseModel {
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
    clientEventTypeID: string;
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
/* MISSION BOARD RESPONSE TYPES */
export class NewBoardEntryResponse {
  entryTypeID: string;
  threadNumber: number;
  postNumber: number;
  postDescription: string;
}
export class EditBoardEntryPostNumberResponse {
  postNumber: number;
}
export class EditBoardEntryThreadNumberResponse {
  threadNumber: number;
}
export class EditBoardEntryPostDescriptionResponse {
  postDescription: string;
}
export class EditBoardEntryTypeResponse {
  entryTypeID: string;
}
export class NewEntryTypeResponse {
  entryTypeName: string;
  entryTypeDescription: string;
  adminOnly: boolean;
}
export class EditEntryTypeName {
  entryTypeName: string;
}
export class EditEntryTypeDescription {
  entryTypeDescription: string;
}
/* RESPONSE TYPES FOR ASSIGNMENT STATUSES */
export class NewAssignmentStatusResponse {
  assignmentStatusName: string;
  assignmentStatusDescription: string;
}
export class EditProfileAssignmentStatusResponse {
  assignmentStatusID: string;
}
/* CATALOGUE */
export class SearchQueryModelResponse {
  tags: Array<string> = [];
  statuses: Array<string> = [];
  events: Array<string> = [];
  names: Array<string> = [];
  nicknames: Array<string> = [];
  emails: Array<string> = [];
  responses: Array<string>= [];
  cardAssignments: Array<number>= [];
  giftAssignments: Array<number>= [];
  isHardSearch: boolean;
}

/* AutoAssignment response models */
export class Pairing {
  senderAgentID: string;
  assignmentClientID: string
}
export class SelectedAutoAssignmentsResponse
{
  pairings: Array<Pairing> = [];
}

/* Note response models */
export class NewNoteResponse
{
  clientID: string;
  noteSubject: string;
  noteContents: string;
}
export class EditNoteResponse
{
  noteSubject: string;
  noteContents: string;
}
