DECLARE @eventTypeID1GUID UNIQUEIDENTIFIER;
DECLARE @eventTypeID2GUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion1IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion2IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion3IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion4IDGUID UNIQUEIDENTIFIER;
DECLARE @survey1IDGUID UNIQUEIDENTIFIER;
DECLARE @survey2IDGUID UNIQUEIDENTIFIER;

DECLARE @tag1IDGUID UNIQUEIDENTIFIER;
DECLARE @tag2IDGUID UNIQUEIDENTIFIER;
DECLARE @tag3IDGUID UNIQUEIDENTIFIER;

-- Attatched to reciever
DECLARE @message1IDGUID UNIQUEIDENTIFIER;
DECLARE @message1DateTime DATETIME;

--Not Attatched to reciever
DECLARE @message2IDGUID UNIQUEIDENTIFIER;


DECLARE @surveyOptionID1GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID2GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID3GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID4GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID5GUID UNIQUEIDENTIFIER;

DECLARE @statusID1GUID UNIQUEIDENTIFIER;
DECLARE @statusID2GUID UNIQUEIDENTIFIER;
DECLARE @statusID3GUID UNIQUEIDENTIFIER;
DECLARE @statusID4GUID UNIQUEIDENTIFIER;

DECLARE @client1IDGUID UNIQUEIDENTIFIER;
DECLARE @client2IDGUID UNIQUEIDENTIFIER;
DECLARE @client3IDGUID UNIQUEIDENTIFIER;
DECLARE @client4IDGUID UNIQUEIDENTIFIER;

DECLARE @surveyResponse1IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyResponse2IDGUID UNIQUEIDENTIFIER;

SET @eventTypeID1GUID = NEWID();
SET @eventTypeID2GUID = NEWID();
SET @surveyQuestion1IDGUID = NEWID();
SET @surveyQuestion2IDGUID = NEWID();
SET @surveyQuestion3IDGUID = NEWID();
SET @surveyQuestion4IDGUID = NEWID();
SET @survey1IDGUID = NEWID();
SET @survey2IDGUID = NEWID();
SET @surveyOptionID1GUID = NEWID();
SET @surveyOptionID2GUID = NEWID();
SET @surveyOptionID3GUID = NEWID();
SET @surveyOptionID4GUID = NEWID();
SET @surveyOptionID5GUID = NEWID();
SET @surveyResponse1IDGUID = NEWID();
SET @surveyResponse2IDGUID = NEWID();

SET @tag1IDGUID = NEWID();
SET @tag2IDGUID = NEWID();
SET @tag3IDGUID = NEWID();

SET @message1IDGUID = NEWID();
SET @message1DateTime = GETUTCDATE();
SET @message2IDGUID = NEWID();

SET @statusID1GUID = NEWID();
SET @statusID2GUID = NEWID();
SET @statusID3GUID = NEWID();
SET @statusID4GUID = NEWID();


SET @client1IDGUID = NEWID();
SET @client2IDGUID = NEWID();
SET @client3IDGUID = NEWID();
SET @client4IDGUID = NEWID();

PRINT N'eventTypeID1GUID:------------ ' + (CAST (@eventTypeID1GUID AS NVARCHAR(50)));
PRINT N'eventTypeID2GUID:------------ ' + (CAST (@eventTypeID2GUID AS NVARCHAR(50)));
PRINT N'surveyQuestion1IDGUID:------- ' + (CAST (@surveyQuestion1IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion2IDGUID:------- ' + (CAST (@surveyQuestion2IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion3IDGUID:------- ' + (CAST (@surveyQuestion3IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion4IDGUID:------- ' + (CAST (@surveyQuestion4IDGUID AS NVARCHAR(50)));
PRINT N'survey1IDGUID:--------------- ' + (CAST (@survey1IDGUID AS NVARCHAR(50)));
PRINT N'survey2IDGUID:--------------- ' + (CAST (@survey2IDGUID AS NVARCHAR(50)));
PRINT N'surveyOptionID1GUID:--------- ' + (CAST (@surveyOptionID1GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID2GUID:--------- ' + (CAST (@surveyOptionID2GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID3GUID:--------- ' + (CAST (@surveyOptionID3GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID4GUID:--------- ' + (CAST (@surveyOptionID3GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID5GUID:--------- ' + (CAST (@surveyOptionID3GUID AS NVARCHAR(50)));

PRINT N'tag1IDGUID:------------------ ' + (CAST (@tag1IDGUID AS NVARCHAR(50)));
PRINT N'tag2IDGUID:------------------ ' + (CAST (@tag2IDGUID AS NVARCHAR(50)));
PRINT N'tag3IDGUID:------------------ ' + (CAST (@tag3IDGUID AS NVARCHAR(50)));

PRINT N'message1IDGUID:-------------- ' + (CAST (@message1IDGUID AS NVARCHAR(50)));
PRINT N'message2IDGUID:-------------- ' + (CAST (@message2IDGUID AS NVARCHAR(50)));

PRINT N'surveyResponse1IDGUID:------- ' + (CAST (@surveyResponse1IDGUID AS NVARCHAR(50)));
PRINT N'surveyResponse2IDGUID:------- ' + (CAST (@surveyResponse2IDGUID AS NVARCHAR(50)));

PRINT N'statusID1GUID:--------------- ' + (CAST (@statusID1GUID AS NVARCHAR(50)));
PRINT N'statusID2GUID:--------------- ' + (CAST (@statusID2GUID AS NVARCHAR(50)));
PRINT N'statusID3GUID:--------------- ' + (CAST (@statusID3GUID AS NVARCHAR(50)));
PRINT N'statusID4GUID:--------------- ' + (CAST (@statusID4GUID AS NVARCHAR(50)));


PRINT N'client1IDGUID:--------------- ' + (CAST (@client1IDGUID AS NVARCHAR(50)));
PRINT N'client2IDGUID:--------------- ' + (CAST (@client2IDGUID AS NVARCHAR(50)));
PRINT N'client3IDGUID:--------------- ' + (CAST (@client3IDGUID AS NVARCHAR(50)));
PRINT N'client4IDGUID:--------------- ' + (CAST (@client4IDGUID AS NVARCHAR(50)));




INSERT INTO app.EventType (eventTypeID, eventDescription, isActive)
VALUES
    (@eventTypeID1GUID,'Gift Exchange', 1),
    (@eventTypeID2GUID,'Card Exchange', 1);

INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, isSurveyOptionList)
VALUES
    (@surveyQuestion1IDGUID,'Who is your favorite pony?',1),
    (@surveyQuestion2IDGUID,'What kind of things do you want for Christmas?',0),
    (@surveyQuestion3IDGUID,'Any card requests?',0),
    (@surveyQuestion4IDGUID,'Is this a christmas thing?',1);

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@survey1IDGUID, @eventTypeID1GUID, 'Gift Survey', 1),
    (@survey2IDGUID, @eventTypeID2GUID, 'Card Survey', 1);

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionID1GUID,'Rainbow Dash','1'),
    (@surveyOptionID2GUID,'Twilight Sparkle','1'),
    (@surveyOptionID3GUID,'Pinkie Pie','1'),
    (@surveyOptionID4GUID,'Yes','1'),
    (@surveyOptionID5GUID,'No','1');

INSERT INTO app.SurveyQuestionXref (surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
    (@survey1IDGUID, @surveyQuestion1IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion2IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion3IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion4IDGUID, 'asc', 1);

INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (@surveyQuestion1IDGUID, @surveyOptionID1GUID, 'asc', 1),
    (@surveyQuestion1IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion1IDGUID, @surveyOptionID3GUID, 'asc', 1),
    (@surveyQuestion4IDGUID, @surveyOptionID4GUID, 'asc', 1),
    (@surveyQuestion4IDGUID, @surveyOptionID5GUID, 'asc', 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@statusID1GUID, 'Awaiting'),
    (@statusID2GUID, 'Approved'),
    (@statusID3GUID, 'Denied'),
    (@statusID4GUID, 'Completed');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country)
VALUES
    (@client1IDGUID, @statusID2GUID, 'Memely Pepeartly', 'Santa Dev', 'santaponecentraldev@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania'),
    (@client2IDGUID, @statusID2GUID, 'Santa Pone', 'Twilight Sparkle', 'mlpsantapone@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania'),
    (@client3IDGUID, @statusID2GUID, 'Cardslut', 'Golen Heart', 'thecardslut@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania'),
    (@client4IDGUID, @statusID2GUID, 'Venport Measure', 'Picky Wikket', 'sorengylfietwilightdigger@gmail.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country');

INSERT INTO app.Tag (tagID, tagName)
VALUES
    (@tag1IDGUID, 'Grinch'),
    (@tag2IDGUID, 'Mass Mailer'),
    (@tag3IDGUID, 'Twifag');

INSERT INTO app.ClientTagXref (clientID, tagID)
VALUES
    (@client1IDGUID, @tag1IDGUID),
    (@client1IDGUID, @tag2IDGUID),
    (@client2IDGUID, @tag3IDGUID),
    (@client3IDGUID, @tag1IDGUID);


SELECT * FROM app.SurveyQuestion;
SELECT * FROM app.Survey;
SELECT * FROM app.SurveyOption;
SELECT * FROM app.SurveyQuestionXref;
SELECT * FROM app.SurveyQuestionOptionXref;
SELECT * FROM app.ClientStatus;
SELECT * FROM app.SurveyResponse;

SELECT * FROM app.EventType;
SELECT * FROM app.Client;
SELECT * FROM app.ClientRelationXref;

SELECT * FROM app.Tag;
SELECT * FROM app.ClientTagXref;
SELECT * FROM app.ChatMessage;



