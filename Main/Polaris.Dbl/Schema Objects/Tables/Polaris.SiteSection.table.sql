CREATE TABLE [Polaris].[SiteSection]
(
[SiteSectionId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (30) NOT NULL,
[Controller] [varchar] (30) NOT NULL,
[Action] [varchar] (30) NOT NULL,
[BaseRoute] [varchar] (max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


