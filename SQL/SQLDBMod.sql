-- DECLARE @survey1IDGUID UNIQUEIDENTIFIER;
-- DECLARE @survey2IDGUID UNIQUEIDENTIFIER;

-- DECLARE @surveyQuestion7IDGUID UNIQUEIDENTIFIER;
-- DECLARE @surveyQuestion8IDGUID UNIQUEIDENTIFIER;

-- SET @surveyQuestion7IDGUID = NEWID();
-- SET @surveyQuestion8IDGUID = NEWID();

-- SET @survey1IDGUID = '7007224a-4716-40bf-84d8-4deb387b1a6f';
-- SET @survey2IDGUID = '33917eb0-6b09-4b8b-8ae7-800b3151de26';

-- -- Test shared vlues
-- INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, senderCanView, isSurveyOptionList)
-- VALUES
--     (@surveyQuestion7IDGUID,'Shared text question', 1, 0),
--     (@surveyQuestion8IDGUID,'Shared secret question', 0, 0);

-- -- Test values for adding shared surveys
-- INSERT INTO app.SurveyQuestionXref (surveyQuestionXrefID, surveyID, surveyQuestionID, sortOrder, isActive)
-- VALUES
-- -- Gift survey
--     (NEWID(), @survey1IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
--     (NEWID(), @survey1IDGUID, @surveyQuestion8IDGUID, 'asc', 1),
-- -- Card survey
--     (NEWID(), @survey2IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
--     (NEWID(), @survey2IDGUID, @surveyQuestion8IDGUID, 'asc', 1);
-- DECLARE @sharedSurveyQuestion2GUID UNIQUEIDENTIFIER = 'ab44ac50-60fe-4e3f-b9b8-eeb799d334b9';

-- DECLARE @surveyOptionID4GUID UNIQUEIDENTIFIER = 'ec4faeca-5ee3-48df-9495-4b829b171e8e';
-- DECLARE @surveyOptionID5GUID UNIQUEIDENTIFIER = 'b3011251-3d67-4c58-b476-9706b10794e6';
-- DECLARE @surveyOptionID6GUID UNIQUEIDENTIFIER = '59b1c519-db73-4c62-b197-d0e22b7abc81';

-- INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionOptionXrefID, surveyQuestionID, surveyOptionID, sortOrder, isActive)
-- VALUES
--     (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID4GUID, 1, 1),
--     (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID5GUID, 2, 1),
--     (NEWID(), @sharedSurveyQuestion2GUID, @surveyOptionID6GUID, 3, 1);

-- DECLARE @giftExchangeSurveyQuestion2 UNIQUEIDENTIFIER = '96f938e5-dd90-44d1-8886-aec787bd7bca';
-- DECLARE @surveyOptionID7GUID UNIQUEIDENTIFIER = '1864696d-1710-4767-812e-3ec4d14f18bd';

-- INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionOptionXrefID, surveyQuestionID, surveyOptionID, sortOrder, isActive)
-- VALUES
--     (NEWID(), @giftExchangeSurveyQuestion2, @surveyOptionID7GUID, 2, 1);

-- DECLARE @massMailSurveyID UNIQUEIDENTIFIER = 'a03218f5-24e2-47d7-9644-a7f5a019d32c';
-- DECLARE @cardExchangeEventGUID UNIQUEIDENTIFIER = 'a2c93535-157c-4d7a-b3e6-57023cfd9b35';

-- INSERT INTO app.Survey (surveyID, eventTypeID, surveyDescription, isActive)
-- VALUES
--     (@massMailSurveyID, @cardExchangeEventGUID, 'Mass Mail Survey', 0);

-- DECLARE @massMailQuestionID UNIQUEIDENTIFIER = NEWID();
-- DECLARE @massMailSurveyID UNIQUEIDENTIFIER = 'a03218f5-24e2-47d7-9644-a7f5a019d32c';

-- INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, senderCanView, isSurveyOptionList)
-- VALUES
--     (@massMailQuestionID, 'Do you wish to recieve Mass Mail?', 0, 0);

-- INSERT INTO app.SurveyQuestionXref (surveyQuestionXrefID, surveyID, surveyQuestionID, sortOrder, isActive)
-- VALUES
--     (NEWID(), @massMailSurveyID, @massMailQuestionID, 1, 1);

-- UPDATE app.SurveyQuestion SET isSurveyOptionList=1 WHERE questionText='Do you wish to recieve Mass Mail?';

-- DECLARE @massMailerQuestionID UNIQUEIDENTIFIER = '0ff2d70a-6592-4549-84cf-6f78e197525f';
-- DECLARE @prodYesID UNIQUEIDENTIFIER = '4a31cb82-4b69-4bee-9333-eb60a8c6500e';
-- DECLARE @prodNoID UNIQUEIDENTIFIER = '43177455-0515-4acb-bc72-49cd93d706ec';

-- INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionOptionXrefID, surveyQuestionID, surveyOptionID, sortOrder, isActive)
-- VALUES
--     (NEWID(), @massMailerQuestionID, @prodYesID, 1, 1),
--     (NEWID(), @massMailerQuestionID, @prodNoID, 2, 1);

-- INSERT INTO app.SurveyQuestionOptionXref (surveyQuestionOptionXrefID, surveyQuestionID, surveyOptionID, sortOrder, isActive)
-- VALUES
--     (NEWID(), 'ab44ac50-60fe-4e3f-b9b8-eeb799d334b9', '24537330-8fb0-4bc0-a251-93336ca53b93', 0, 1);


--DELETE FROM app.ChatMessage
--DELETE FROM app.ClientRelationXref
--DELETE FROM app.ClientTagXref
--DELETE FROM app.Tag

--ALTER TABLE app.Client DROP COLUMN hasAccount
--ALTER TABLE app.Client ADD hasAccount BIT NOT NULL DEFAULT 0
--alter table app.Client drop constraint [DF__Client__hasAccou__230D49A5]
--ALTER TABLE app.Client DROP COLUMN hasAccount
--ALTER TABLE app.SurveyQuestionOptionXref DROP COLUMN sortOrder
--ALTER TABLE app.SurveyQuestionXref DROP COLUMN sortOrder
--ALTER TABLE app.SurveyQuestionXref ADD sortOrder INT NOT NULL DEFAULT 0;
--ALTER TABLE app.SurveyQuestionOptionXref ADD sortOrder INT NOT NULL DEFAULT 0;
--ALTER TABLE app.SurveyQuestion ALTER COLUMN questionText nvarchar(300) NOT NULL;

-- UPDATE app.Client SET hasAccount=1
-- UPDATE app.Client SET isAdmin=1 WHERE nickname='SlenderDuck';
-- UPDATE app.SurveyQuestion SET isSurveyOptionList=0 WHERE questionText='Are you okay with food items? If so, do you have any allergies?';
-- UPDATE app.SurveyQuestion SET questionText='Do you acknowledge your info will be shared with others assigned to send to you?' WHERE questionText='Do you aknowledge your info will be shared with other assigned to send to you?';
-- UPDATE app.SurveyQuestion SET questionText='Are you okay with receiving extra assignments for things such as Grinch rescue?' WHERE questionText='Are you okay with receiving extra assignments?';
-- UPDATE app.SurveyQuestionOptionXref SET sortOrder=3 WHERE surveyQuestionOptionXrefID='3b5aabb9-948e-42f7-9fde-69e52df60aff';
-- UPDATE app.SurveyQuestion SET isSurveyOptionList=1 WHERE questionText='Do you wish to recieve Mass Mail?';
-- UPDATE app.SurveyOption SET displayText='Yes, go nuts' WHERE surveyOptionID='ec4faeca-5ee3-48df-9495-4b829b171e8e';
-- UPDATE app.SurveyOption SET displayText='Yes, but be subtle' WHERE surveyOptionID='b3011251-3d67-4c58-b476-9706b10794e6';
-- UPDATE app.SurveyOption SET displayText='Yes, but only a little' WHERE surveyOptionID='24537330-8fb0-4bc0-a251-93336ca53b93';

-- ALTER TABLE app.ChatMessage ALTER COLUMN messageContent NVARCHAR(1000) NOT NULL;

--TRUNCATE TABLE app.AssignmentStatus

-- DELETE FROM app.ChatMessage
-- DELETE FROM app.ClientRelationXref
-- DELETE FROM app.BoardEntry
-- DELETE FROM app.EntryType
-- DELETE FROM app.SurveyResponse;
-- DELETE FROM app.SurveyQuestionOptionXref;
-- DELETE FROM app.SurveyOption;
-- DELETE FROM app.SurveyQuestionXref;
-- DELETE FROM app.SurveyQuestion;
-- DELETE FROM app.Survey;
-- DELETE FROM app.SurveyQuestionOptionXref WHERE surveyQuestionID='7c425125-7365-47c2-9028-9cd741b7a57e'
-- DELETE FROM app.SurveyQuestionOptionXref WHERE surveyQuestionID='ab44ac50-60fe-4e3f-b9b8-eeb799d334b9'
-- DELETE FROM app.SurveyQuestionOptionXref WHERE surveyQuestionOptionXrefID='516bffd4-51ef-4863-a795-f6f0756910cc';

-- ALTER TABLE app.ClientRelationXref DROP COLUMN completed

-- SELECT * FROM app.SurveyQuestion;
-- SELECT * FROM app.SurveyOption;
-- SELECT * FROM app.SurveyQuestionOptionXref WHERE surveyQuestionID='ab44ac50-60fe-4e3f-b9b8-eeb799d334b9';
-- SELECT * FROM app.Survey;
-- SELECT * FROM app.EventType;
-- SELECT * FROM app.ChatMessage;
-- SELECT * FROM app.SurveyQuestionOptionXref WHERE surveyQuestionID='ab44ac50-60fe-4e3f-b9b8-eeb799d334b9';
-- SELECT * FROM app.YuleLog ORDER BY logDate
-- SELECT COUNT(*) FROM app.ClientRelationXref
-- UPDATE app.ChatMessage SET isMessageRead=0
-- UPDATE app.EntryType SET entryTypeDescription='Site development announcements' WHERE entryTypeName='Announcements (Development)';