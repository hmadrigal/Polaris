CREATE TABLE [Polaris].[MenuItem]
(
[MenuItemId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SiteSection] [int] NOT NULL,
[ParentMenuItemId] [int] NULL,
[ContentTypeId] [int] NULL,
[SortOptionId] [int] NULL,
[FilterOptionId] [int] NULL
) ON [PRIMARY]


