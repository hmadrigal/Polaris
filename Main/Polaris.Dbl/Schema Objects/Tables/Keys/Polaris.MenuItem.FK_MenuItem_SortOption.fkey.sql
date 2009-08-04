ALTER TABLE [Polaris].[MenuItem] ADD
CONSTRAINT [FK_MenuItem_SortOption] FOREIGN KEY ([SortOptionId]) REFERENCES [Polaris].[SortOption] ([SortOptionId])


