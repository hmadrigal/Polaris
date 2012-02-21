CREATE TABLE [Polaris].[User] (
    [UserId]         INT           IDENTITY (1, 1) NOT NULL,
    [Username]       VARCHAR (20)  NOT NULL,
    [Password]       VARCHAR (50)  NOT NULL,
    [FirstName]      VARCHAR (30)  NOT NULL,
    [LastName]       VARCHAR (30)  NOT NULL,
    [Email]          VARCHAR (100) NOT NULL,
    [Active]         BIT           NOT NULL,
    [PlayCredits]    INT           NOT NULL,
    [RankingCredits] INT           NOT NULL
);




