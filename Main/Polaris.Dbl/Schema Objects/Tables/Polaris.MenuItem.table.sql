CREATE TABLE [Polaris].[MenuItem]
(
[MenuItemId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) NOT NULL,
[DisplayName] [varchar] (50) NULL,
[SiteSectionId] [int] NOT NULL,
[ParentMenuItemId] [int] NULL,
[ContentTypeId] [int] NULL,
[SortOptionId] [int] NULL,
[FilterOptionId] [int] NULL
) ON [PRIMARY]


