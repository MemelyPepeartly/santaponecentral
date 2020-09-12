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


--DELETE FROM app.ChatMessage
--DELETE FROM app.ClientRelationXref
--DELETE FROM app.ClientTagXref
--DELETE FROM app.Tag

--ALTER TABLE app.Client DROP COLUMN hasAccount
--ALTER TABLE app.Client ADD hasAccount BIT NOT NULL DEFAULT 0
--alter table app.Client drop constraint [DF__Client__hasAccou__230D49A5]
--ALTER TABLE app.Client DROP COLUMN hasAccount

--UPDATE app.Client SET hasAccount=1

-- ALTER TABLE app.ChatMessage ALTER COLUMN messageContent NVARCHAR(1000) NOT NULL;