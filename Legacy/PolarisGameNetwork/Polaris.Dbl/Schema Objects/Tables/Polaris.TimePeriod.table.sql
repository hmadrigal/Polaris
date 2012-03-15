CREATE TABLE [Polaris].[TimePeriod]
(
[TimePeriodId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) NOT NULL,
[Hours] [int] NOT NULL
) ON [PRIMARY]


GO
EXEC sp_addextendedproperty N'MS_Description', N'Number of hours associated with this period of time.', 'SCHEMA', N'Polaris', 'TABLE', N'TimePeriod', 'COLUMN', N'Hours'

