DROP TABLE app.SurveyResponse;
DROP TABLE app.SurveyQuestionOptionXref;
DROP TABLE app.SurveyOption;
DROP TABLE app.SurveyQuestionXref;
DROP TABLE app.SurveyQuestion;
DROP TABLE app.Survey;
DROP TABLE app.ChatMessage;
DROP TABLE app.ClientRelationXref;
DROP TABLE app.EventType;
DROP TABLE app.ClientTagXref;
DROP TABLE app.Client;
DROP TABLE app.ClientStatus;
DROP TABLE app.Tag;

DROP TABLE app.BoardEntry;
DROP TABLE app.EntryType;

DROP SCHEMA app;


--DELETE FROM app.ChatMessage
--DELETE FROM app.ClientRelationXref
--DELETE FROM app.ClientTagXref
--DELETE FROM app.Tag

--ALTER TABLE app.Client DROP COLUMN hasAccount
ALTER TABLE app.Client ADD hasAccount BIT NOT NULL DEFAULT 0
--alter table app.Client drop constraint [DF__Client__hasAccou__230D49A5]
--ALTER TABLE app.Client DROP COLUMN hasAccount

--UPDATE app.Client SET hasAccount=1