CREATE TABLE [Polaris].[Game]
(
[GameId] [bigint] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (30) NOT NULL,
[Key] [uniqueidentifier] NOT NULL,
[Active] [bit] NOT NULL,
[DevelopmentTeamId] [bigint] NOT NULL
) ON [PRIMARY]


