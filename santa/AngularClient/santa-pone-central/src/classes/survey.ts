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

