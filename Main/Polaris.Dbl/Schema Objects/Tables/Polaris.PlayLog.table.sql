CREATE TABLE [Polaris].[PlayLog]
(
[PlayLogId] [bigint] NOT NULL IDENTITY(1, 1),
[GameId] [bigint] NOT NULL,
[UserId] [bigint] NOT NULL,
[Date] [datetime] NOT NULL,
[Score] [decimal] (18, 0) NOT NULL
) ON [PRIMARY]


