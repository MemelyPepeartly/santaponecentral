export class ManualAddClientRequest {
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
export class ClientSignupRequest {
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
    responses: Array<SurveySignupApiRequest> = []
}
export class EditClientAddressRequest {
    clientAddressLine1: string;
    clientAddressLine2: string;
    clientCity: string;
    clientState: string;
    clientPostalCode: string;
    clientCountry: string;
}
export class EditClientEmailRequest {
    clientEmail: string;
}
export class EditClientNicknameRequest {
    clientNickname: string;
}
export class EditClientNameRequest {
    clientName: string;
}
export class EditClientStatusRequest {
    clientStatusID: string;
    wantsAccount: boolean;
}
export class EditClientIsAdminRequest {
    isAdmin: boolean;
}

export class ClientRelationshipsRequest {
    eventTypeID: string;
    assignmentStatusID: string;
    assignments: Array<string> = []
}
export class AddSurveyResponseRequest {
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
export class EditSurveyResponseRequest {
  responseText: string;
}

export class SurveySignupApiRequest {
    surveyID: string;
    surveyQuestionID: string;
    surveyOptionID?: string = null;
    responseText: string;
}
export class AddOrEditTagRequest {
    tagName: string;
}
export class DeleteClientTagRelationshipRequest {
    clientID: string;
    tagID: string;
}
export class AddClientTagRelationshipsRequest {
    tags: Array<string> = [];
}
export class DeleteClientSenderRecipientRelationshipRequest {
    clientID: string;
    clientEventTypeID: string;
}
// Response model for marking if a question is viewable by the person(s) sending to them
export class QuestionReadabilityResponse
{
    senderCanView: boolean;
}
// Response for making new messages
export class AddMessageRequest {
    messageSenderClientID?: string = null;
    messageRecieverClientID?: string = null;
    clientRelationXrefID?: string = null;
    eventTypeID?: string;
    messageContent: string;
    fromAdmin: boolean;
}
//Response for marking messages as read
export class EditMessageApiReadRequest {
    isMessageRead: boolean;
}
//Response for marking a list of messages as read
export class EditMessageApiReadAllRequest {
    messages: Array<string> = [];
}
/* MISSION BOARD RESPONSE TYPES */
export class NewBoardEntryRequest {
  entryTypeID: string;
  threadNumber: number;
  postNumber: number;
  postDescription: string;
}
export class EditBoardEntryPostNumberRequest {
  postNumber: number;
}
export class EditBoardEntryThreadNumberRequest {
  threadNumber: number;
}
export class EditBoardEntryPostDescriptionRequest {
  postDescription: string;
}
export class EditBoardEntryTypeRequest {
  entryTypeID: string;
}
export class NewEntryTypeRequest {
  entryTypeName: string;
  entryTypeDescription: string;
  adminOnly: boolean;
}
export class EditEntryTypeNameRequest {
  entryTypeName: string;
}
export class EditEntryTypeDescriptionRequest {
  entryTypeDescription: string;
}
/* RESPONSE TYPES FOR ASSIGNMENT STATUSES */
export class NewAssignmentStatusRequest {
  assignmentStatusName: string;
  assignmentStatusDescription: string;
}
export class EditProfileAssignmentStatusRequest {
  assignmentStatusID: string;
}
/* CATALOGUE */
export class SearchQueryModelRequest {
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
export class AddNoteRequest
{
  clientID: string;
  noteSubject: string;
  noteContents: string;
}
export class EditNoteRequest
{
  noteSubject: string;
  noteContents: string;
}
