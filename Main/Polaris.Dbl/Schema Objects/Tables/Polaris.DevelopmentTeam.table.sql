CREATE TABLE [Polaris].[DevelopmentTeam]
(
[DevelopmentTeamId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Key] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


