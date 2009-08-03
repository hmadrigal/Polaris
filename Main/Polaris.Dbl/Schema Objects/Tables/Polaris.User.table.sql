CREATE TABLE [Polaris].[User]
(
[UserId] [bigint] NOT NULL IDENTITY(1, 1),
[Username] [varchar] (20) NOT NULL,
[Password] [varchar] (50) NOT NULL,
[FirstName] [varchar] (30) NOT NULL,
[LastName] [varchar] (30) NOT NULL,
[Email] [varchar] (100) NOT NULL,
[PlayCredits] [bigint] NOT NULL,
[RankingCredits] [bigint] NOT NULL
) ON [PRIMARY]


