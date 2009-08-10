ALTER TABLE [Polaris].[Developer] ADD
CONSTRAINT [FK_Developer_DevelopmentTeam] FOREIGN KEY ([DevelopmentTeamId]) REFERENCES [Polaris].[DevelopmentTeam] ([DevelopmentTeamId])


