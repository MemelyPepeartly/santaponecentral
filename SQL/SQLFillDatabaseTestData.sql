DECLARE @eventTypeIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestionIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionIDGUID UNIQUEIDENTIFIER;
DECLARE @firstClientStatusIDGUID UNIQUEIDENTIFIER;
DECLARE @secondClientStatusIDGUID UNIQUEIDENTIFIER;
DECLARE @senderClientIDGUID UNIQUEIDENTIFIER;
DECLARE @recipientClientIDGUID UNIQUEIDENTIFIER;
DECLARE @surveyResponseIDGUID UNIQUEIDENTIFIER;

SET @eventTypeIDGUID = NEWID();
SET @surveyQuestionIDGUID = NEWID();
SET @surveyIDGUID = NEWID();
SET @surveyOptionIDGUID = NEWID();
SET @surveyResponseIDGUID = NEWID();

SET @firstClientStatusIDGUID = NEWID();
SET @secondClientStatusIDGUID = NEWID();

SET @senderClientIDGUID = NEWID();
SET @recipientClientIDGUID = NEWID();

PRINT N'eventTypeIDGUID:------------ ' + (CAST (@eventTypeIDGUID AS NVARCHAR(50)));
PRINT N'surveyQuestionIDGUID:------- ' + (CAST (@surveyQuestionIDGUID AS NVARCHAR(50)));
PRINT N'surveyIDGUID:--------------- ' + (CAST (@surveyIDGUID AS NVARCHAR(50)));
PRINT N'surveyOptionIDGUID:--------- ' + (CAST (@surveyOptionIDGUID AS NVARCHAR(50)));
PRINT N'surveyResponseIDGUID:------- ' + (CAST (@surveyResponseIDGUID AS NVARCHAR(50)));

PRINT N'firstClientStatusIDGUID:---- ' + (CAST (@firstClientStatusIDGUID AS NVARCHAR(50)));
PRINT N'secondClientStatusIDGUID:--- ' + (CAST (@secondClientStatusIDGUID AS NVARCHAR(50)));

PRINT N'senderClientIDGUID:--------- ' + (CAST (@senderClientIDGUID AS NVARCHAR(50)));
PRINT N'recipientClientIDGUID:------ ' + (CAST (@recipientClientIDGUID AS NVARCHAR(50)));




INSERT INTO app.EventType (eventTypeID, eventDescription, isActive)
VALUES
    (@eventTypeIDGUID,'Event Description', 1);

INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, isSurveyOptionList)
VALUES
    (@surveyQuestionIDGUID,'Question 1',1);

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@surveyIDGUID, @eventTypeIDGUID, 'surveyDescription', 1);

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionIDGUID,'Display Text','Survey Option Value');

INSERT INTO app.SurveyQuestionXref (surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
    (@surveyIDGUID, @surveyQuestionIDGUID, 'asc', 1);

INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (@surveyQuestionIDGUID, @surveyOptionIDGUID, 'asc', 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@firstClientStatusIDGUID, 'Status Description 1'),
    (@secondClientStatusIDGUID, 'Status Description 2');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country)
VALUES
    (@senderClientIDGUID, @firstClientStatusIDGUID, 'Sender Client Name', 'First Nickname', 'firstemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country'),
    (@recipientClientIDGUID, @secondClientStatusIDGUID, 'Recipient Client Name', 'Second Nickname', 'secondemail@email.com', 'Address 1', 'Address 2', 'City', 'State', 'Postal Code', 'Country');

INSERT INTO app.SurveyResponse (surveyResponseID, surveyID, clientID, surveyQuestionID, surveyOptionID, responseText)
VALUES
    (@surveyResponseIDGUID, @surveyIDGUID, @senderClientIDGUID, @surveyQuestionIDGUID, @surveyOptionIDGUID, 'Response Text');
    
INSERT INTO app.ClientRelationXref (senderClientID, recipientClientID, eventTypeID)
VALUES
    (@senderClientIDGUID, @recipientClientIDGUID, @eventTypeIDGUID);



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



