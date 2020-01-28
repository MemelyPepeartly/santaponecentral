CREATE SCHEMA app;
GO

CREATE TABLE app.ClientStatus
(
    clientStatusID INT PRIMARY KEY,
    statusDescription NVARCHAR(25) NOT NULL
);

CREATE TABLE app.Client
(
    clientID UNIQUEIDENTIFIER PRIMARY KEY,
    clientStatusID INT FOREIGN KEY REFERENCES app.ClientStatus(clientStatusID) NOT NULL,
    clientName NVARCHAR(50) NOT NULL,
    nickname NVARCHAR(50) NOT NULL UNIQUE,
    email NVARCHAR(50) NOT NULL UNIQUE,
    addressLine1 NVARCHAR(50) NOT NULL,
    addressLine2 NVARCHAR(50),
    city NVARCHAR(25) NOT NULL,
    state NVARCHAR(25) NOT NULL,
    postalCode NVARCHAR(10) NOT NULL,
    country NVARCHAR(50) NOT NULL
);
CREATE TABLE app.EventType
(
    eventTypeID INT PRIMARY KEY,
    eventDescription NVARCHAR(100) NOT NULL,
    isActive BIT NOT NULL

);
CREATE TABLE app.ClientRelationXref
(
    senderClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    recipientClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    eventTypeID INT FOREIGN KEY REFERENCES app.EventType(eventTypeID) NOT NULL
);
CREATE TABLE app.Survey
(
    surveyID UNIQUEIDENTIFIER PRIMARY KEY,
    eventTypeID INT FOREIGN KEY REFERENCES app.EventType(eventTypeID) NOT NULL,
    surveyDescription NVARCHAR(100) NOT NULL,
    isActive BIT NOT NULL,
);
CREATE TABLE app.SurveyOption
(
    surveyOptionID UNIQUEIDENTIFIER PRIMARY KEY,
    displayText INT NOT NULL,
    surveyOptionValue NVARCHAR(10) NOT NULL,
);
CREATE TABLE app.SurveyQuestion
(
    surveyQuestionID UNIQUEIDENTIFIER PRIMARY KEY,
    questionText NVARCHAR(150) NOT NULL,
    isSurveyOptionList BIT NOT NULL
)
CREATE TABLE app.SurveyResponse
(
    surveyResponseID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Survey(surveyID) NOT NULL,
    clientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    surveyOptionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID) NOT NULL,
    responseText NVARCHAR(150) NOT NULL
);
CREATE TABLE app.SurveyQuestionXref
(
    surveyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Survey(surveyID) NOT NULL,
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    sortOrder NVARCHAR(5) NOT NULL,
    isAction BIT NOT NULL
);
CREATE TABLE app.SurveyQuestionOptionXref
(
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    surveyOptionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID) NOT NULL,
    sortOrder NVARCHAR(5) NOT NULL,
    isAction BIT NOT NULL
);