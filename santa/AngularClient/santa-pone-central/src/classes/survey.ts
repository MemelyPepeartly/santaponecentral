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
    surveyQuestionID: string;
    surveyOptionID?: string;
    responseText: string;
}
export class Question {
    questionID: string;
    questionText: string;
    isSurveyOptionList: boolean;
    surveyOptionList: Array<SurveyOption> = [];
}
export class SurveyOption {
    surveyOptionID: string;
    displayText: string;
    surveyOptionValue: number;
}
// Below classes for making responses with necessary information in the surveyform component
export class SurveyFormQuestion 
{
    clientID: string;
    surveyQuestionID: string;
    questionText: string;
    responseText: string;
    isSurveyOptionList: boolean;
    surveyOptionList: Array<SurveyFormOption> = [];
}

export class SurveyFormOption 
{
    surveyOptionID: string;
    optionText: string;
}

