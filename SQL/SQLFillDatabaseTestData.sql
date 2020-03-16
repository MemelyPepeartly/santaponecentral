DECLARE @eventTypeIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion1IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion2IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion3IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionIDGUID UNIQUEIDENTIFIER;
DECLARE @firstClientStatusIDGUID UNIQUEIDENTIFIER;
DECLARE @secondClientStatusIDGUID UNIQUEIDENTIFIER;

DECLARE @client1IDGUID UNIQUEIDENTIFIER;
DECLARE @client2IDGUID UNIQUEIDENTIFIER;
DECLARE @client3IDGUID UNIQUEIDENTIFIER;
DECLARE @client4IDGUID UNIQUEIDENTIFIER;

DECLARE @surveyResponseIDGUID UNIQUEIDENTIFIER;

SET @eventTypeIDGUID = NEWID();
SET @surveyQuestion1IDGUID = NEWID();
SET @surveyQuestion2IDGUID = NEWID();
SET @surveyQuestion3IDGUID = NEWID();
SET @surveyIDGUID = NEWID();
SET @surveyOptionIDGUID = NEWID();
SET @surveyResponseIDGUID = NEWID();

SET @firstClientStatusIDGUID = NEWID();
SET @secondClientStatusIDGUID = NEWID();


SET @client1IDGUID = NEWID();
SET @client2IDGUID = NEWID();
SET @client3IDGUID = NEWID();
SET @client4IDGUID = NEWID();

PRINT N'eventTypeIDGUID:------------ ' + (CAST (@eventTypeIDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion1IDGUID:------ ' + (CAST (@surveyQuestion1IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion2IDGUID:------ ' + (CAST (@surveyQuestion2IDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestion3IDGUID:------ ' + (CAST (@surveyQuestion3IDGUID AS NVARCHAR(50)));
PRINT N'surveyIDGUID:--------------- ' + (CAST (@surveyIDGUID AS NVARCHAR(50)));
PRINT N'surveyOptionIDGUID:--------- ' + (CAST (@surveyOptionIDGUID AS NVARCHAR(50)));
PRINT N'surveyResponseIDGUID:------- ' + (CAST (@surveyResponseIDGUID AS NVARCHAR(50)));

PRINT N'firstClientStatusIDGUID:---- ' + (CAST (@firstClientStatusIDGUID AS NVARCHAR(50)));
PRINT N'secondClientStatusIDGUID:--- ' + (CAST (@secondClientStatusIDGUID AS NVARCHAR(50)));

PRINT N'client1IDGUID:--------- ' + (CAST (@client1IDGUID AS NVARCHAR(50)));
PRINT N'client2IDGUID:--------- ' + (CAST (@client2IDGUID AS NVARCHAR(50)));
PRINT N'client3IDGUID:--------- ' + (CAST (@client3IDGUID AS NVARCHAR(50)));
PRINT N'client4IDGUID:--------- ' + (CAST (@client4IDGUID AS NVARCHAR(50)));




INSERT INTO app.EventType (eventTypeID, eventDescription, isActive)
VALUES
    (@eventTypeIDGUID,'Event Description', 1);

INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, isSurveyOptionList)
VALUES
    (@surveyQuestion1IDGUID,'Question 1',1),
    (@surveyQuestion2IDGUID,'Question 2',1),
    (@surveyQuestion3IDGUID,'Question 3',1);

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@surveyIDGUID, @eventTypeIDGUID, 'surveyDescription', 1);

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionIDGUID,'Display Text','Survey Option Value');

INSERT INTO app.SurveyQuestionXref (surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
    (@surveyIDGUID, @surveyQuestion1IDGUID, 'asc', 1),
    (@surveyIDGUID, @surveyQuestion2IDGUID, 'asc', 1),
    (@surveyIDGUID, @surveyQuestion3IDGUID, 'asc', 1);

INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (@surveyQuestion1IDGUID, @surveyOptionIDGUID, 'asc', 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@firstClientStatusIDGUID, 'Awaiting'),
    (@secondClientStatusIDGUID, 'Approved');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country)
VALUES
    (@client1IDGUID, @firstClientStatusIDGUID, 'Wobble Wub', 'DJ Fucknasty', 'firstemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client2IDGUID, @secondClientStatusIDGUID, 'Evershade', 'Sharona Virus', 'secondemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client3IDGUID, @firstClientStatusIDGUID, 'Venport', 'Vampire Nigga', 'thirdemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@client4IDGUID, @secondClientStatusIDGUID, 'Memely', 'OC Fag', 'fourthemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country');

INSERT INTO app.SurveyResponse (surveyResponseID, surveyID, clientID, surveyQuestionID, surveyOptionID, responseText)
VALUES
    (@surveyResponseIDGUID, @surveyIDGUID, @client1IDGUID, @surveyQuestion1IDGUID, @surveyOptionIDGUID, 'Response Text');
    
INSERT INTO app.ClientRelationXref (senderClientID, recipientClientID, eventTypeID)
VALUES
    (@client1IDGUID, @client2IDGUID, @eventTypeIDGUID),
    (@client2IDGUID, @client3IDGUID, @eventTypeIDGUID),
    (@client2IDGUID, @client4IDGUID, @eventTypeIDGUID),
    (@client4IDGUID, @client1IDGUID, @eventTypeIDGUID);



SELECT * FROM app.EventType;
SELECT * FROM app.SurveyQuestion;
SELECT * FROM app.Survey;
SELECT * FROM app.SurveyOption;
SELECT * FROM app.SurveyQuestionXref;
SELECT * FROM app.SurveyQuestionOptionXref;
SELECT * FROM app.ClientStatus;
SELECT * FROM app.Client;
SELECT * FROM app.SurveyResponse;
SELECT * FROM app.ClientRelationXref;



