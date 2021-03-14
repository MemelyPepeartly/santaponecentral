CREATE SCHEMA app;
GO
CREATE TABLE app.ClientStatus
(
    clientStatusID UNIQUEIDENTIFIER PRIMARY KEY,
    statusDescription NVARCHAR(25) NOT NULL
)
CREATE TABLE app.Client
(
    clientID UNIQUEIDENTIFIER PRIMARY KEY ,
    clientStatusID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.ClientStatus(clientStatusID) NOT NULL,
    clientName NVARCHAR(50) NOT NULL,
    nickname NVARCHAR(50),
    email NVARCHAR(50) NOT NULL UNIQUE,
    addressLine1 NVARCHAR(50) NOT NULL,
    addressLine2 NVARCHAR(50),
    city NVARCHAR(50) NOT NULL,
    [state] NVARCHAR(50) NOT NULL,
    postalCode NVARCHAR(25) NOT NULL,
    country NVARCHAR(50) NOT NULL,
    isAdmin BIT NOT NULL,
    hasAccount BIT NOT NULL
);
CREATE TABLE app.Note
(
    noteID UNIQUEIDENTIFIER PRIMARY KEY,
    clientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID) NOT NULL,
    noteSubject NVARCHAR(100) NOT NULL,
    noteContents NVARCHAR(2000)
)
CREATE TABLE app.EventType
(
    eventTypeID UNIQUEIDENTIFIER PRIMARY KEY,
    eventDescription NVARCHAR(100) UNIQUE NOT NULL,
    isActive BIT NOT NULL

);
CREATE TABLE app.AssignmentStatus
(
    assignmentStatusID UNIQUEIDENTIFIER PRIMARY KEY,
    assignmentStatusName NVARCHAR(100) UNIQUE NOT NULL,
    assignmentStatusDescription NVARCHAR(200) UNIQUE NOT NULL
);
CREATE TABLE app.ClientRelationXref
(
    clientRelationXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    senderClientID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Client(clientID),
    recipientClientID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Client(clientID),
    eventTypeID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.EventType(eventTypeID),
    assignmentStatusID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.AssignmentStatus(assignmentStatusID),
    CONSTRAINT clientRelationXrefID UNIQUE (senderClientID, recipientClientID, eventTypeID) 
);
CREATE TABLE app.Survey
(
    surveyID UNIQUEIDENTIFIER PRIMARY KEY,
    eventTypeID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.EventType(eventTypeID) ,
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
    questionText NVARCHAR(300) NOT NULL,
    senderCanView BIT NOT NULL,
    isSurveyOptionList BIT NOT NULL
)
CREATE TABLE app.SurveyResponse
(
    surveyResponseID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Survey(surveyID),
    clientID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Client(clientID),
    surveyQuestionID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID),
    surveyOptionID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID),
    responseText NVARCHAR(4000) NOT NULL
);
CREATE TABLE app.SurveyQuestionXref
(
    surveyQuestionXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Survey(surveyID),
    surveyQuestionID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID),
    sortOrder NVARCHAR(5) NOT NULL,
    isActive BIT NOT NULL
);
CREATE TABLE app.SurveyQuestionOptionXref
(
    surveyQuestionOptionXrefID UNIQUEIDENTIFIER PRIMARY KEY,
    surveyQuestionID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.SurveyQuestion(surveyQuestionID),
    surveyOptionID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.SurveyOption(surveyOptionID),
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
    clientID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Client(clientID),
    tagID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Tag(tagID),
    CONSTRAINT clientTagXrefID UNIQUE (clientID, tagID) 
);
CREATE TABLE app.ChatMessage
(
    chatMessageID UNIQUEIDENTIFIER PRIMARY KEY,
    messageSenderClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID),
    messageReceiverClientID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.Client(clientID),
    clientRelationXrefID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.ClientRelationXref(clientRelationXrefID),
    messageContent NVARCHAR(1000) NOT NULL,
    dateTimeSent DATETIME NOT NULL,
    isMessageRead BIT NOT NULL,
    fromAdmin BIT NOT NULL
);
CREATE TABLE app.EntryType
(
    entryTypeID UNIQUEIDENTIFIER PRIMARY KEY,
    entryTypeName NVARCHAR(100) NOT NULL UNIQUE,
    entryTypeDescription NVARCHAR(200) NOT NULL,
    adminOnly BIT NOT NULL
);
CREATE TABLE app.BoardEntry
(
    boardEntryID UNIQUEIDENTIFIER PRIMARY KEY,
    entryTypeID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES app.EntryType(EntryTypeID),
    threadNumber INT NOT NULL,
    postNumber INT NOT NULL,
    postDescription NVARCHAR(100) NOT NULL,
    dateTimeEntered DATETIME NOT NULL,
    CONSTRAINT boardEntryID UNIQUE (threadNumber, postNumber) 
);
CREATE TABLE app.Category
(
    categoryID UNIQUEIDENTIFIER PRIMARY KEY,
    categoryName NVARCHAR(1000) NOT NULL,
    categoryDescription NVARCHAR(1000) NOT NULL
);
CREATE TABLE app.YuleLog
(
    logID UNIQUEIDENTIFIER PRIMARY KEY,
    logDate DATETIME NOT NULL,
    categoryID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES app.Category(categoryID),
    logText NVARCHAR(1000) NOT NULL
);

GO
CREATE TRIGGER app.cascadeTrigger ON app.Client INSTEAD OF DELETE
NOT FOR REPLICATION
AS BEGIN 
    SET NOCOUNT ON;
    begin try
    begin tran;

    delete app.ClientTagXref
    where clientID in(select clientID from deleted);
    
    --Deletes chat message things
    delete app.ChatMessage
    where messageSenderClientID in(select clientID from deleted);

    delete app.ChatMessage
    where messageReceiverClientID in(select clientID from deleted);

    -- Deletes xref relationships
    delete app.ClientRelationXref
    where recipientClientID in (select clientID from deleted)

    delete app.ClientRelationXref
    where senderClientID in (select clientID from deleted)

    delete app.SurveyResponse
    where clientID in(select clientID from deleted);

    delete app.Client
    where clientID in(select clientID from deleted);

    delete app.Note 
    where clientID in(select clientID from deleted)

    commit tran;
    end try
    begin catch
    if @@trancount > 0
    rollback tran;
    throw;
    end catch;
END

CREATE INDEX ClientStatusIndex ON app.Client (clientStatusID);

CREATE INDEX NoteClientIndex ON app.Note (clientID);

CREATE INDEX ResponseSurveyIndex ON app.SurveyResponse (surveyID);
CREATE INDEX ResponseClientIndex ON app.SurveyResponse (clientID);
CREATE INDEX ResponseQuestionIndex ON app.SurveyResponse (surveyQuestionID);

CREATE INDEX RelationSenderIndex ON app.ClientRelationXref (senderClientID);
CREATE INDEX RelationRecipientIndex ON app.ClientRelationXref (recipientClientID);
CREATE INDEX RelationEventIndex ON app.ClientRelationXref (eventTypeID);
CREATE INDEX RelationAssignmentStatusIndex ON app.ClientRelationXref (assignmentStatusID);

CREATE INDEX TagIndex ON app.ClientTagXref (tagID);
CREATE INDEX TagClientIndex ON app.ClientTagXref (clientID);

CREATE INDEX ChatSenderIndex ON app.ChatMessage (messageSenderClientID);
CREATE INDEX ChatRecieverIndex ON app.ChatMessage (messageReceiverClientID);
CREATE INDEX ChatRelationIndex ON app.ChatMessage (clientRelationXrefID);
