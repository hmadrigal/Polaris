CREATE TABLE [Polaris].[User]
(
[UserId] [bigint] NOT NULL IDENTITY(1, 1),
[Username] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PlayCredits] [bigint] NOT NULL,
[RankingCredits] [bigint] NOT NULL
) ON [PRIMARY]


