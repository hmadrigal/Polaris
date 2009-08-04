CREATE TABLE [Polaris].[User]
(
[UserId] [int] NOT NULL IDENTITY(1, 1),
[Username] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Active] [bit] NOT NULL,
[PlayCredits] [int] NOT NULL,
[RankingCredits] [int] NOT NULL
) ON [PRIMARY]


