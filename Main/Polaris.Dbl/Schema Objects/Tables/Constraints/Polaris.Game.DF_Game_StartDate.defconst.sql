ALTER TABLE [Polaris].[Game] ADD CONSTRAINT [DF_Game_StartDate] DEFAULT (getdate()) FOR [StartDate]
