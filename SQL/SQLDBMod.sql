DECLARE @survey1IDGUID UNIQUEIDENTIFIER;
DECLARE @survey2IDGUID UNIQUEIDENTIFIER;

DECLARE @surveyQuestion7IDGUID UNIQUEIDENTIFIER;
DECLARE @surveyQuestion8IDGUID UNIQUEIDENTIFIER;

SET @surveyQuestion7IDGUID = NEWID();
SET @surveyQuestion8IDGUID = NEWID();

SET @survey1IDGUID = NEWID();
SET @survey2IDGUID = NEWID();

-- Test shared vlues
INSERT INTO app.SurveyQuestion (surveyQuestionID, questionText, senderCanView, isSurveyOptionList)
VALUES
    (@surveyQuestion7IDGUID,'Shared text question', 1, 0),
    (@surveyQuestion8IDGUID,'Shared secret question', 0, 0);

-- Test values for adding shared surveys
INSERT INTO app.SurveyQuestionXref (surveyQuestionXrefID, surveyID, surveyQuestionID, sortOrder, isActive)
VALUES
-- Gift survey
    (NEWID(), @survey1IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
    (NEWID(), @survey1IDGUID, @surveyQuestion8IDGUID, 'asc', 1),
-- Card survey
    (NEWID(), @survey2IDGUID, @surveyQuestion7IDGUID, 'asc', 1),
    (NEWID(), @survey2IDGUID, @surveyQuestion8IDGUID, 'asc', 1);