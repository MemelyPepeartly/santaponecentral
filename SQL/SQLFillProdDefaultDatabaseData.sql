DECLARE @statusID1GUID UNIQUEIDENTIFIER;
DECLARE @statusID2GUID UNIQUEIDENTIFIER;
DECLARE @statusID3GUID UNIQUEIDENTIFIER;
DECLARE @statusID4GUID UNIQUEIDENTIFIER;

DECLARE @client1IDGUID UNIQUEIDENTIFIER;
DECLARE @client2IDGUID UNIQUEIDENTIFIER;
DECLARE @client3IDGUID UNIQUEIDENTIFIER;

SET @statusID1GUID = NEWID();
SET @statusID2GUID = NEWID();
SET @statusID3GUID = NEWID();
SET @statusID4GUID = NEWID();

SET @client1IDGUID = NEWID();
SET @client2IDGUID = NEWID();
SET @client3IDGUID = NEWID();

INSERT INTO app.EventType (eventTypeID, eventDescription, isActive)
VALUES
    (NEWID(),'Gift Exchange', 1),
    (NEWID(),'Card Exchange', 1);

DECLARE @sharedSurveyQuestion1GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion2GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion3GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion4GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion5GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion6GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @sharedSurveyQuestion7GUID UNIQUEIDENTIFIER = NEWID();


DECLARE @giftExchangeSurveyQuestion1 UNIQUEIDENTIFIER = NEWID();
DECLARE @giftExchangeSurveyQuestion2 UNIQUEIDENTIFIER = NEWID();
DECLARE @giftExchangeSurveyQuestion3 UNIQUEIDENTIFIER = NEWID();

DECLARE @cardExchangeSurveyQuestion1 UNIQUEIDENTIFIER = NEWID();
DECLARE @cardExchangeSurveyQuestion2 UNIQUEIDENTIFIER = NEWID();
DECLARE @cardExchangeSurveyQuestion3 UNIQUEIDENTIFIER = NEWID();
DECLARE @cardExchangeSurveyQuestion4 UNIQUEIDENTIFIER = NEWID();
DECLARE @cardExchangeSurveyQuestion5 UNIQUEIDENTIFIER = NEWID();

INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, senderCanView, isSurveyOptionList)
VALUES
    -- Shared Questions --
    (@sharedSurveyQuestion1GUID,'Who are your favorite characters?', 1, 0),
    (@sharedSurveyQuestion2GUID,'Are you okay with spaghetti on the package?', 1, 1), -- Option
    (@sharedSurveyQuestion3GUID,'Are you okay with lewd?', 1, 1), -- Option
    (@sharedSurveyQuestion4GUID,'Are you okay with food items? If so, do you have any allergies?', 1, 0),
    (@sharedSurveyQuestion5GUID,'Any additional comments or concerns?', 1 ,0),
    (@sharedSurveyQuestion6GUID,'If you’re a past participant, what was your holiday ID? (N/A if this does not apply)', 0, 0),
    (@sharedSurveyQuestion7GUID,'If you were a past participant, would you like to keep your holiday ID or get a new one?', 0, 0),

    -- Gift Exchange --
    (@giftExchangeSurveyQuestion1,'Are you okay with receiving extra assignments for things such as Grinch rescue?', 0, 1), -- Option
    (@giftExchangeSurveyQuestion2,'Do you like handmade items?', 1, 1), -- Option
    (@giftExchangeSurveyQuestion3,'What is your wishlist?', 1, 0),

    -- Card Exchange --
    (@cardExchangeSurveyQuestion1,'How many cards would you like to exchange?', 0, 0),
    (@cardExchangeSurveyQuestion2,'Are you a mass mailer? Note: First-year participants that are mass mailers will have a max of 20 assignments. Return mailers can have higher assignment caps.', 0, 1), -- Option
    (@cardExchangeSurveyQuestion3,'Do you acknowledge your info will be shared with others assigned to send to you?', 0 ,1), -- Option
    (@cardExchangeSurveyQuestion4,'Are you willing to send cards internationally?', 0, 1), -- Option
    (@cardExchangeSurveyQuestion5,'Some generous anons like to send extra cards or small goodies to every single participant in the event (this is separate from the regular exchange). Do you consent to receiving such items? You do not have to send anything back.', 0, 1); -- Option
    
DECLARE @giftExchangeEventGUID UNIQUEIDENTIFIER = '82f3d537-274d-4c64-ac1f-01ce57e16e77';
DECLARE @cardExchangeEventGUID UNIQUEIDENTIFIER = '284e1b27-caea-47f1-89e4-0111d2b19cd7';

DECLARE @giftExchangeSurveyID UNIQUEIDENTIFIER = NEWID();
DECLARE @cardExchangeSurveyID UNIQUEIDENTIFIER = NEWID();

INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
VALUES
    (@giftExchangeSurveyID, @giftExchangeEventGUID, 'Gift Survey', 1),
    (@cardExchangeSurveyID, @cardExchangeEventGUID, 'Card Survey', 1);

DECLARE @surveyOptionID1GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @surveyOptionID2GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @surveyOptionID3GUID UNIQUEIDENTIFIER = NEWID();

DECLARE @surveyOptionID4GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @surveyOptionID5GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @surveyOptionID6GUID UNIQUEIDENTIFIER = NEWID();
DECLARE @surveyOptionID7GUID UNIQUEIDENTIFIER = NEWID();

INSERT INTO app.SurveyOption (surveyOptionID, displayText, surveyOptionValue)
VALUES
    (@surveyOptionID1GUID,'Yes','1'),
    (@surveyOptionID2GUID,'No','1'),
    (@surveyOptionID3GUID,'Not Applicable','1'),

    (@surveyOptionID4GUID,'Yes, go nuts','1'),
    (@surveyOptionID5GUID,'Mildly suggestive is fine','1'),
    (@surveyOptionID6GUID,'No, none at all','1'),

    (@surveyOptionID7GUID,'Yes, but I perfer official merchandise','1');

INSERT INTO app.SurveyQuestionXref (surveyQuestionXrefID, surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
-- Gift survey
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion6GUID, 1, 1), -- What was your old ID?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion7GUID, 2, 1), -- Want same ID?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion1GUID, 3, 1), -- Fav Characters?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion2GUID, 4, 1), -- Spaghetti?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion3GUID, 5, 1), -- Lewd?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion4GUID, 6, 1), -- Edible items?
    (NEWID(), @giftExchangeSurveyID, @giftExchangeSurveyQuestion2, 7, 1), -- Handmade stuff?
    (NEWID(), @giftExchangeSurveyID, @giftExchangeSurveyQuestion1, 8, 1), -- Okay with extra assignments?
    (NEWID(), @giftExchangeSurveyID, @giftExchangeSurveyQuestion3, 9, 1), -- Your wishlist?
    (NEWID(), @giftExchangeSurveyID, @sharedSurveyQuestion5GUID, 10, 1), -- Additional Comments?

-- Card survey
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion6GUID, 1, 1), -- What was your old ID?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion7GUID, 2, 1), -- Want same ID?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion1GUID, 3, 1), -- Fav Characters?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion2GUID, 4, 1), -- Spaghetti?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion3GUID, 5, 1), -- Lewd?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion4GUID, 6, 1), -- Edible items?
    (NEWID(), @cardExchangeSurveyID, @cardExchangeSurveyQuestion3, 7, 1), -- Acknowledge that assignees will see info?
    (NEWID(), @cardExchangeSurveyID, @cardExchangeSurveyQuestion1, 8, 1), -- How many cards to exchange?
    (NEWID(), @cardExchangeSurveyID, @cardExchangeSurveyQuestion2, 9, 1), -- Mass Mailer?
    (NEWID(), @cardExchangeSurveyID, @cardExchangeSurveyQuestion4, 10, 1), -- Sending Internationally?
    (NEWID(), @cardExchangeSurveyID, @cardExchangeSurveyQuestion5, 11, 1), -- Consent to recieving extra items?
    (NEWID(), @cardExchangeSurveyID, @sharedSurveyQuestion5GUID, 12, 1); -- Additional Comments?


INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionOptionXrefID, surveyQuestionID, surveyOptionID, sortOrder, isActive)
VALUES
    (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID4GUID, 1, 1),
    (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID5GUID, 2, 1),
    (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID6GUID, 3, 1),

    (NEWID(), @sharedSurveyQuestion3GUID, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @sharedSurveyQuestion3GUID, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @sharedSurveyQuestion4GUID, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @sharedSurveyQuestion4GUID, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @giftExchangeSurveyQuestion1, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @giftExchangeSurveyQuestion1, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @giftExchangeSurveyQuestion2, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @giftExchangeSurveyQuestion2, @surveyOptionID7GUID, 2, 1),
    (NEWID(), @giftExchangeSurveyQuestion2, @surveyOptionID2GUID, 3, 1),

    (NEWID(), @cardExchangeSurveyQuestion2, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @cardExchangeSurveyQuestion2, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @cardExchangeSurveyQuestion3, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @cardExchangeSurveyQuestion3, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @cardExchangeSurveyQuestion4, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @cardExchangeSurveyQuestion4, @surveyOptionID2GUID, 2, 1),

    (NEWID(), @cardExchangeSurveyQuestion5, @surveyOptionID1GUID, 1, 1),
    (NEWID(), @cardExchangeSurveyQuestion5, @surveyOptionID2GUID, 2, 1);
    
INSERT INTO app.ClientStatus (clientStatusID, statusDescription)
VALUES
    (@statusID1GUID, 'Awaiting'),
    (@statusID2GUID, 'Approved'),
    (@statusID3GUID, 'Denied'),
    (@statusID4GUID, 'Completed');
    
INSERT INTO app.Client (clientID, clientStatusID, clientName, nickname, email, addressLine1, addressLine2, city, [state], postalCode, country, isAdmin)
VALUES
    (@client1IDGUID, @statusID1GUID, 'Memely', 'Santa Dev', 'santaponecentraldev@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania', 1),
    (@client2IDGUID, @statusID2GUID, 'Santa Pone', 'Twilight Sparkle', 'mlpsantapone@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania', 1),
    (@client3IDGUID, @statusID2GUID, 'Cardslut', 'Golen Heart', 'thecardslut@gmail.com', 'This', 'can', 'be', 'changed', '12457', 'Albania', 1);

DECLARE @tag1IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag2IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag3IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag4IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag5IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag6IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag7IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag8IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag9IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag10IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag11IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag12IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag13IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag14IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag15IDGUID UNIQUEIDENTIFIER = NEWID();
DECLARE @tag16IDGUID UNIQUEIDENTIFIER = NEWID();

INSERT INTO app.Tag (tagID, tagName)
VALUES
    -- (@tag1IDGUID, 'Grinch'),
    -- (@tag2IDGUID, 'Mass Mailer'),
    -- (@tag3IDGUID, 'Mass Mail Recipient'),
    (@tag4IDGUID, 'Accepts Food'),
    (@tag5IDGUID, 'Canada Only'),
    (@tag6IDGUID, 'Declines Food'),
    (@tag7IDGUID, 'US Only'),
    (@tag8IDGUID, 'Perfers Show Merchandise'),
    (@tag9IDGUID, 'Perfers Handmade Gifts'),
    (@tag10IDGUID, 'No Spaghetti'),
    (@tag11IDGUID, 'Russian'),
    (@tag12IDGUID, 'No Lewd'),
    (@tag13IDGUID, 'Lewd'),
    (@tag14IDGUID, 'US Anon'),
    (@tag15IDGUID, 'International Anon'),
    (@tag16IDGUID, 'Spaghetti On the Outside');

    
INSERT INTO app.EntryType (entryTypeID, entryTypeName, entryTypeDescription, adminOnly)
VALUES
    (NEWID(), 'Announcements (Deadlines)', 'Deadline announcement and news', 1),
    (NEWID(), 'Announcements (Contests)', 'Contest announcements and information', 1),
    (NEWID(), 'Announcements (Development)', 'Site development announcments', 1),
    (NEWID(), 'Announcements (Other)', 'Any other important announcements', 1);

INSERT INTO app.AssignmentStatus (assignmentStatusID, assignmentStatusName, assignmentStatusDescription)
VALUES
    (NEWID(), 'Assigned', 'Fresh assignment that has yet to be started'),
    (NEWID(), 'In Progress', 'Assignment is in progress of being made, shopped for, or being completed in some way'),
    (NEWID(), 'Shipping', 'Assignment is on its way! Cheer inbound!'),
    (NEWID(), 'Completed', 'Your delivery of cheer has made its mark! Mission accomplished!');

INSERT INTO app.Category (categoryID, categoryName, categoryDescription)
VALUES
    (NEWID(), 'Get All History', 'A list of all histories was requested'),
    (NEWID(), 'Modified Profile', 'A modifcation of a profile was made'),
    (NEWID(), 'Get History', 'A message history object was requested'),
    (NEWID(), 'Assignments Given', 'An assignment or list of assignments was given to a client'),
    (NEWID(), 'Modified Client', 'A client''s information was modified'),  
    (NEWID(), 'Get All Clients', 'A list of all clients was requested'),
    (NEWID(), 'Get Client By ID', 'A request to get a client by their ID was made'),
    (NEWID(), 'Get Profile', 'A profile was requested'),
    (NEWID(), 'Modified Assignment Status', 'A status was updated for an assignment'),
    (NEWID(), 'Modified Answer', 'A survey answer was modified'),

    (NEWID(), 'Created New Message', 'A new message was posted to a chat'),
    (NEWID(), 'Modified Message Read Status', 'A message was marked as read'),
    (NEWID(), 'New Client Created', 'A new client object has been created'),
    (NEWID(), 'New Auth0 Account Created', 'A new Auth0 account was created for a client'),
    (NEWID(), 'Deleted Client', 'A client has been deleted'),
    (NEWID(), 'Deleted Assignment', 'An assignment has been removed from a client'),
    (NEWID(), 'Modified Client Status', 'A client''s status was modified'),
    (NEWID(), 'Added New Tag', 'A new tag was created'),
    (NEWID(), 'Added New Tag To Client', 'A tag was assigned to a client');



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



