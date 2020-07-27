CREATE SCHEMA app;
GO

CREATE TABLE app.ClientStatus
(
    clientStatusID UNIQUEIDENTIFIER PRIMARY KEY,
    statusDescription NVARCHAR(25) NOT NULL
);

CREATE TABLE app.Client
(
    clientID UNIQUEIDENTIFIER PRIMARY KEY,
    clientStatusID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.ClientStatus(clientStatusID) NOT NULL,
    clientName NVARCHAR(50) NOT NULL,
    nickname NVARCHAR(50),
    email NVARCHAR(50) NOT NULL UNIQUE,
    addressLine1 NVARCHAR(50) NOT NULL,
    addressLine2 NVARCHAR(50),
    city NVARCHAR(50) NOT NULL,
    [state] NVARCHAR(50) NOT NULL,
    postalCode NVARCHAR(25) NOT NULL,
    country NVARCHAR(50) NOT NULL
);
CREATE TABLE app.EventType
(
    eventTypeID UNIQUEIDENTIFIER PRIMARY KEY,
    eventDescription NVARCHAR(100) UNIQUE NOT NULL,
    isActive BIT NOT NULL

);
CREATE TABLE app.ClientRelationXref
(
    clientRelationXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    senderClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    recipientClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    eventTypeID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.EventType(eventTypeID) NOT NULL,
    completed BIT NOT NULL,
    CONSTRAINT clientRelationXrefID UNIQUE (senderClientID, recipientClientID, eventTypeID) 
);
CREATE TABLE app.Survey
(
    surveyID UNIQUEIDENTIFIER PRIMARY KEY,
    eventTypeID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.EventType(eventTypeID) NOT NULL,
    surveyDescription NVARCHAR(100) NOT NULL,
    isActive BIT NOT NULL,
);
CREATE TABLE app.SurveyOption
(
    surveyOptionID UNIQUEIDENTIFIER PRIMARY KEY,
    displayText NVARCHAR(100) NOT NULL,
    surveyOptionValue NVARCHAR(50) NOT NULL,
);
CREATE TABLE app.SurveyQuestion
(
    surveyQuestionID UNIQUEIDENTIFIER PRIMARY KEY,
    questionText NVARCHAR(150) NOT NULL,
    senderCanView BIT NOT NULL,
    isSurveyOptionList BIT NOT NULL
)
CREATE TABLE app.SurveyResponse
(
    surveyResponseID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Survey(surveyID) NOT NULL,
    clientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    surveyOptionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID),
    responseText NVARCHAR(2000) NOT NULL
);
CREATE TABLE app.SurveyQuestionXref
(
    surveyQuestionXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Survey(surveyID) NOT NULL,
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    sortOrder NVARCHAR(5) NOT NULL,
    isActive BIT NOT NULL
);
CREATE TABLE app.SurveyQuestionOptionXref
(
    surveyQuestionOptionXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyQuestionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID) NOT NULL,
    surveyOptionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID) NOT NULL,
    sortOrder NVARCHAR(5) NOT NULL,
    isActive BIT NOT NULL
);
CREATE TABLE app.Tag
(
    tagID UNIQUEIDENTIFIER PRIMARY KEY,
    tagName NVARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE app.ClientTagXref
(
    clientTagXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    clientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    tagID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Tag(tagID) NOT NULL,
    CONSTRAINT clientTagXrefID UNIQUE (clientID, tagID) 
);
CREATE TABLE app.ChatMessage
(
    chatMessageID UNIQUEIDENTIFIER PRIMARY KEY,
    messageSenderClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID),
    messageRecieverClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID),
    clientRelationXrefID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.ClientRelationXref(clientRelationXrefID),
    messageContent NVARCHAR(1000) NOT NULL,
    dateTimeSent DATETIME NOT NULL,
    isMessageRead BIT NOT NULL
);

-- ALTER TABLE app.ChatMessage ALTER COLUMN messageContent NVARCHAR(1000) NOT NULL;