ALTER TABLE [Polaris].[Developer] ADD
CONSTRAINT [FK_Developer_User] FOREIGN KEY ([UserId]) REFERENCES [Polaris].[User] ([UserId])


