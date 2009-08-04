ALTER TABLE [Polaris].[PlayLog] ADD
CONSTRAINT [FK_PlayLog_Game] FOREIGN KEY ([GameId]) REFERENCES [Polaris].[Game] ([GameId])


