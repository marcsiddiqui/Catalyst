IF OBJECT_ID('GenericDropDownOption') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[GenericDropDownOption](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[Text] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Value] [int] NOT NULL,
	[OrderBy] [int] NOT NULL,
	[Color] [nvarchar](7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsSystemOption] [bit] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[GenericDropDownOption] ADD  DEFAULT ((0)) FOR [EntityId]
ALTER TABLE [dbo].[GenericDropDownOption] ADD  DEFAULT ((0)) FOR [Value]
ALTER TABLE [dbo].[GenericDropDownOption] ADD  DEFAULT ((0)) FOR [OrderBy]
ALTER TABLE [dbo].[GenericDropDownOption] ADD  DEFAULT ((0)) FOR [IsSystemOption]
END
