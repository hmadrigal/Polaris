ALTER TABLE [Polaris].[MenuItem] ADD
CONSTRAINT [FK_MenuItem_ContentType] FOREIGN KEY ([ContentTypeId]) REFERENCES [Polaris].[ContentType] ([ContentTypeId])


