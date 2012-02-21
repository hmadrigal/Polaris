ALTER TABLE [Polaris].[PlayLog] ADD
CONSTRAINT [FK_PlayLog_User] FOREIGN KEY ([UserId]) REFERENCES [Polaris].[User] ([UserId])


