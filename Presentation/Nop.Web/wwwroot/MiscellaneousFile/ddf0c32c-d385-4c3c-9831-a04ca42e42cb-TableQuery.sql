IF OBJECT_ID('Grade') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Grade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[BaseFeeAmount] [decimal](18, 0) NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[LimitedToStores] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((1)) FOR [IsActive]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [Deleted]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [BaseFeeAmount]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [StoreId]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[Grade] ADD  DEFAULT ((0)) FOR [LimitedToStores]
END
