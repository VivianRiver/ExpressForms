CREATE DATABASE [ExpressFormsExample];
GO

USE ExpressFormsExample;
GO

CREATE TABLE dbo.Engineer
(
	Id INT IDENTITY(0,1) PRIMARY KEY,
	Name VARCHAR(100),
	Location VARCHAR(100),
	Available BIT NOT NULL,
	FavoriteLanguage VARCHAR(100),
	CodeSnippet VARCHAR(MAX)
);		

INSERT INTO dbo.Engineer
	(Name, Location, Available, FavoriteLanguage, CodeSnippet)
	VALUES
	('Ada Lovelace', 'London', 0, 'ada', 'We don''t have one here.'),
	('Daniel Langdon', 'Denver', 1, 'javascript', '(function() { alert("Hello World"); })();'),
	('31337 HaX0r', 'Fort Meade', 0, 'html', '<script>alert("I HACKS UR SITE!!! LOLOLOL0L)L!!!!!")</script>');