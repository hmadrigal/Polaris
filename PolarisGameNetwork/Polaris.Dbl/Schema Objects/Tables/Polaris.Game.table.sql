CREATE TABLE [Polaris].[Game] (
    [GameId]            INT              IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (30)     NOT NULL,
    [Key]               UNIQUEIDENTIFIER NOT NULL,
    [Active]            BIT              NOT NULL,
    [DevelopmentTeamId] INT              NOT NULL,
    [StartDate]         DATETIME         NOT NULL,
    [EndDate]           DATETIME         NOT NULL,
    [FeaturedStartDate] DATETIME         NULL,
    [FeaturedEndDate]   DATETIME         NULL
);




