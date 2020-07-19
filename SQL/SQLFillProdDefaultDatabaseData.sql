DECLARE @eventTypeID1GUID UNIQUEIDENTIFIER;
DECLARE @eventTypeID2GUID UNIQUEIDENTIFIER;

DECLARE @surveyQuestion1IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion2IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion3IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion4IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion5IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion6IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion7IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion8IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion9IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion10IDGUID UNIQUEIDENTIFIER;

DECLARE @survey1IDGUID UNIQUEIDENTIFIER;
DECLARE @survey2IDGUID UNIQUEIDENTIFIER;

DECLARE @surveyOptionID1GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID2GUID UNIQUEIDENTIFIER;
DECLARE @surveyOptionID3GUID UNIQUEIDENTIFIER;

DECLARE @statusID1GUID UNIQUEIDENTIFIER;
DECLARE @statusID2GUID UNIQUEIDENTIFIER;
DECLARE @statusID3GUID UNIQUEIDENTIFIER;
DECLARE @statusID4GUID UNIQUEIDENTIFIER;

DECLARE @client1IDGUID UNIQUEIDENTIFIER;
DECLARE @client2IDGUID UNIQUEIDENTIFIER;
DECLARE @client3IDGUID UNIQUEIDENTIFIER;

SET @eventTypeID1GUID = NEWID();
SET @eventTypeID2GUID = NEWID();

SET @surveyQuestion1IDGUID = NEWID();
SET @surveyQuestion2IDGUID = NEWID();
SET @surveyQuestion3IDGUID = NEWID();
SET @surveyQuestion4IDGUID = NEWID();
SET @surveyQuestion5IDGUID = NEWID();
SET @surveyQuestion6IDGUID = NEWID();
SET @surveyQuestion7IDGUID = NEWID();
SET @surveyQuestion8IDGUID = NEWID();
SET @surveyQuestion9IDGUID = NEWID();
SET @surveyQuestion10IDGUID = NEWID();

SET @survey1IDGUID = NEWID();
SET @survey2IDGUID = NEWID();

SET @surveyOptionID1GUID = NEWID();
SET @surveyOptionID2GUID = NEWID();
SET @surveyOptionID3GUID = NEWID();

SET @statusID1GUID = NEWID();
SET @statusID2GUID = NEWID();
SET @statusID3GUID = NEWID();
SET @statusID4GUID = NEWID();


SET @client1IDGUID = NEWID();
SET @client2IDGUID = NEWID();
SET @client3IDGUID = NEWID();

INSERT INTO app.EventType (eventTypeID, eventDescription, isActive)
VALUES
    (@eventTypeID1GUID,'Gift Exchange', 1),
    (@eventTypeID2GUID,'Card Exchange', 1);

INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, isSurveyOptionList)
VALUES
    (@surveyQuestion1IDGUID,'How many cards would you like to send?',0),
    (@surveyQuestion2IDGUID,'How many cards would you like to recieve?',0),
    (@surveyQuestion3IDGUID,'Who are your favorite characters?',0),
    (@surveyQuestion4IDGUID,'Are you okay with spaghetti on the package?',1),
    (@surveyQuestion5IDGUID,'Are you okay with lewd?',1),
    (@surveyQuestion6IDGUID,'Are you okay with edible items?',1),
    (@surveyQuestion7IDGUID,'Are you okay with receiving extra assignments?',1),
    (@surveyQuestion8IDGUID,'Additional comments?',0),
    (@surveyQuestion9IDGUID,'Wishlist',0),
    (@surveyQuestion10IDGUID,'Do you like handmade items?',1);

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@survey1IDGUID, @eventTypeID1GUID, 'Gift Survey', 1),
    (@survey2IDGUID, @eventTypeID2GUID, 'Card Survey', 1);

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionID1GUID,'Yes','1'),
    (@surveyOptionID2GUID,'No','1'),
    (@surveyOptionID3GUID,'Maybe','1');

INSERT INTO app.SurveyQuestionXref (surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
-- Card survey
    (@survey2IDGUID, @surveyQuestion1IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion2IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion3IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion4IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion5IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion6IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
    (@survey2IDGUID, @surveyQuestion8IDGUID, 'asc', 1),
-- Gift survey
    (@survey1IDGUID, @surveyQuestion3IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion4IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion5IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion6IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion8IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion9IDGUID, 'asc', 1),
    (@survey1IDGUID, @surveyQuestion10IDGUID, 'asc', 1);

INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (@surveyQuestion4IDGUID, @surveyOptionID1GUID, 'asc', 1),
    (@surveyQuestion4IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion4IDGUID, @surveyOptionID3GUID, 'asc', 1),


    (@surveyQuestion5IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion5IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion5IDGUID, @surveyOptionID3GUID, 'asc', 1),

    (@surveyQuestion6IDGUID, @surveyOptionID3GUID, 'asc', 1),
    (@surveyQuestion6IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion6IDGUID, @surveyOptionID3GUID, 'asc', 1),

    (@surveyQuestion7IDGUID, @surveyOptionID3GUID, 'asc', 1),
    (@surveyQuestion7IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion7IDGUID, @surveyOptionID3GUID, 'asc', 1),

    (@surveyQuestion10IDGUID, @surveyOptionID3GUID, 'asc', 1),
    (@surveyQuestion10IDGUID, @surveyOptionID2GUID, 'asc', 1),
    (@surveyQuestion10IDGUID, @surveyOptionID3GUID, 'asc', 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@statusID1GUID, 'Awaiting'),
    (@statusID2GUID, 'Approved'),
    (@statusID3GUID, 'Denied'),
    (@statusID4GUID, 'Completed');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country)
VALUES
    (@client1IDGUID, @statusID1GUID, 'Memely Pepeartly', 'Santa Dev', 'santaponecentraldev@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania'),
    (@client2IDGUID, @statusID2GUID, 'Santa Pone', 'Twilight Sparkle', 'mlpsantapone@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania'),
    (@client3IDGUID, @statusID2GUID, 'Cardslut', 'Golen Heart', 'thecardslut@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania');


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



