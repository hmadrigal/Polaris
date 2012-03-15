ALTER TABLE [Polaris].[Game] ADD
CONSTRAINT [FK_Game_DevelopmentTeam] FOREIGN KEY ([DevelopmentTeamId]) REFERENCES [Polaris].[DevelopmentTeam] ([DevelopmentTeamId])


