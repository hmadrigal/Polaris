ALTER TABLE [Polaris].[MenuItem] ADD
CONSTRAINT [FK_MenuItem_FilterOption] FOREIGN KEY ([FilterOptionId]) REFERENCES [Polaris].[FilterOption] ([FilterOptionId])


