DECLARE @eventTypeID1GUID UNIQUEIDENTIFIER;
DECLARE @eventTypeID2GUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion1IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion2IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion3IDGUID UNIQUEIDENTIFIER;
DECLARE @survey1IDGUID UNIQUEIDENTIFIER;
DECLARE @survey2IDGUID UNIQUEIDENTIFIER;


DECLARE @surveyOptionID1GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID2GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID3GUID UNIQUEIDENTIFIER;

DECLARE @firstClientStatusIDGUID UNIQUEIDENTIFIER;
DECLARE @secondClientStatusIDGUID UNIQUEIDENTIFIER;

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
SET @survey1IDGUID = NEWID();
SET @survey2IDGUID = NEWID();
SET @surveyOptionID1GUID = NEWID();
SET @surveyOptionID2GUID = NEWID();
SET @surveyOptionID3GUID = NEWID();
SET @surveyResponse1IDGUID = NEWID();
SET @surveyResponse2IDGUID = NEWID();

SET @firstClientStatusIDGUID = NEWID();
SET @secondClientStatusIDGUID = NEWID();


SET @client1IDGUID = NEWID();
SET @client2IDGUID = NEWID();
SET @client3IDGUID = NEWID();
SET @client4IDGUID = NEWID();

PRINT N'eventTypeID1GUID:------------ ' + (CAST (@eventTypeID1GUID AS NVARCHAR(50)));
PRINT N'eventTypeID2GUID:------------ ' + (CAST (@eventTypeID2GUID AS NVARCHAR(50)));
PRINT N'surveyQuestion1IDGUID:------- ' + (CAST (@surveyQuestion1IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion2IDGUID:------- ' + (CAST (@surveyQuestion2IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion3IDGUID:------- ' + (CAST (@surveyQuestion3IDGUID AS NVARCHAR(50)));
PRINT N'survey1IDGUID:--------------- ' + (CAST (@survey1IDGUID AS NVARCHAR(50)));
PRINT N'survey2IDGUID:--------------- ' + (CAST (@survey2IDGUID AS NVARCHAR(50)));
PRINT N'surveyOptionID1GUID:--------- ' + (CAST (@surveyOptionID1GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID2GUID:--------- ' + (CAST (@surveyOptionID2GUID AS NVARCHAR(50)));
PRINT N'surveyOptionID3GUID:--------- ' + (CAST (@surveyOptionID3GUID AS NVARCHAR(50)));

PRINT N'surveyResponse1IDGUID:------- ' + (CAST (@surveyResponse1IDGUID AS NVARCHAR(50)));
PRINT N'surveyResponse2IDGUID:------- ' + (CAST (@surveyResponse2IDGUID AS NVARCHAR(50)));

PRINT N'firstClientStatusIDGUID:----- ' + (CAST (@firstClientStatusIDGUID AS NVARCHAR(50)));
PRINT N'secondClientStatusIDGUID:---- ' + (CAST (@secondClientStatusIDGUID AS NVARCHAR(50)));

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
    (@surveyQuestion2IDGUID,'Gift Question 2',0),
    (@surveyQuestion3IDGUID,'Card Question 1',0);

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@survey1IDGUID, @eventTypeID1GUID, 'Gift Survey', 1),
    (@survey2IDGUID, @eventTypeID2GUID, 'Card Survey', 1);

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionID1GUID,'Twilight Sparkle','1'),
    (@surveyOptionID2GUID,'Pinkie Pie','1'),
    (@surveyOptionID3GUID,'Fluttershy','1');

INSERT INTO app.SurveyQuestionXref (surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
    (@survey1IDGUID, @surveyQuestion1IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion2IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion3IDGUID, 'asc', 1);

INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (@surveyQuestion1IDGUID, @surveyOptionID1GUID, 'asc', 1),
    (@surveyQuestion1IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion1IDGUID, @surveyOptionID3GUID, 'asc', 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@firstClientStatusIDGUID, 'Awaiting'),
    (@secondClientStatusIDGUID, 'Approved');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country)
VALUES
    (@client1IDGUID, @secondClientStatusIDGUID, 'Wobble Wub', 'Wibble Wab', 'firstemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client2IDGUID, @secondClientStatusIDGUID, 'Evershade', 'Sharona Virus', 'secondemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client3IDGUID, @secondClientStatusIDGUID, 'Venport', 'Picky Wikket', 'thirdemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client4IDGUID, @secondClientStatusIDGUID, 'Memely', 'Duk', 'fourthemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country');

INSERT INTO app.SurveyResponse (surveyResponseID, surveyID, clientID, surveyQuestionID, surveyOptionID, responseText)
VALUES
    (@surveyResponse1IDGUID, @survey1IDGUID, @client1IDGUID, @surveyQuestion1IDGUID, @surveyOptionID1GUID, 'Pinkie Pie'),
    (@surveyResponse2IDGUID, @survey2IDGUID, @client1IDGUID, @surveyQuestion3IDGUID, null , 'This is my card answer');
    
-- INSERT INTO app.ClientRelationXref (senderClientID, recipientClientID, eventTypeID)
-- VALUES
--     (@client1IDGUID, @client2IDGUID, @eventTypeID1GUID),
--     (@client2IDGUID, @client3IDGUID, @eventTypeID1GUID),
--     (@client2IDGUID, @client4IDGUID, @eventTypeID1GUID),
--     (@client4IDGUID, @client1IDGUID, @eventTypeID1GUID);




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



