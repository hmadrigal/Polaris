CREATE TABLE [Polaris].[PlayLog]
(
[PlayLogId] [int] NOT NULL IDENTITY(1, 1),
[GameId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[Date] [datetime] NOT NULL,
[Score] [decimal] (18, 0) NOT NULL
) ON [PRIMARY]


