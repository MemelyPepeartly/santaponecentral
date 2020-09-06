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

--ALTER TABLE app.Client ALTER COLUMN hasAccount BIT NOT NULL