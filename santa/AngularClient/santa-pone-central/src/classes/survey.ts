import { EventType } from './eventType';

export class Survey {
    surveyID: string;
    eventTypeID: string;
    surveyDescription: string;
    active: boolean;
    surveyQuestions: Array<Question> = [];
}
export class SurveyResponse {
    surveyResponseID: string;
    surveyID: string;
    clientID: string;
    responseEvent: EventType;
    surveyQuestion: Question;
    surveyOptionID?: string;
    questionText: string;
    responseText: string;
}
export class Question {
    questionID: string;
    questionText: string;
    isSurveyOptionList: boolean;
    senderCanView: boolean;
    surveyOptionList: Array<SurveyOption> = [];
}
export class SurveyOption {
    surveyOptionID: string;
    displayText: string;
    surveyOptionValue: number;
}
// Below classes for making responses with necessary information in the surveyform component
export class SurveyQA 
{
    clientID: string;
    surveyID: string;
    surveyQuestionID: string;
    eventTypeID: string;
    questionText: string;
    responseInputText: string;
    responseOptionSelected: SurveyFormOption;
    isSurveyOptionList: boolean;
    senderCanView: boolean;
    surveyOptionList: Array<SurveyFormOption> = [];
}

export class SurveyFormOption 
{
    surveyOptionID: string;
    optionText: string;
}

