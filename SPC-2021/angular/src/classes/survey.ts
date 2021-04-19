import { EventType } from "./eventType";

export class Survey {
  surveyID!: string;
  eventTypeID!: string;
  surveyDescription!: string;
  active!: boolean;
  surveyQuestions: Array<Question> = [];
  removable!: boolean;
}
export class SurveyMeta {
  surveyID!: string;
  eventTypeID!: string;
}
export class SurveyResponse {
  surveyResponseID!: string;
  surveyID!: string;
  clientID!: string;
  responseEvent!: EventType;
  surveyQuestion!: Question;
  surveyOptionID?: string;
  responseText!: string;
}
export class Question {
  questionID!: string;
  questionText!: string;
  isSurveyOptionList!: boolean;
  senderCanView!: boolean;
  surveyOptionList: Array<SurveyOption> = [];
  removable!: boolean;
}
export class SurveyOption {
  surveyOptionID!: string;
  displayText!: string;
  surveyOptionValue!: number;
  removable!: boolean;
}
// Below classes for making responses with necessary information in the surveyform component
export class SurveyQA {
  clientID!: string;
  surveyID!: string;
  surveyQuestionID!: string;
  eventTypeID!: string;
  questionText!: string;
  responseInputText!: string;
  responseOptionSelected!: SurveyFormOption;
  isSurveyOptionList!: boolean;
  senderCanView!: boolean;
  surveyOptionList: Array<SurveyFormOption> = [];
}

export class SurveyFormOption {
  surveyOptionID!: string;
  optionText!: string;
}

