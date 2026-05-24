IF OBJECT_ID('Admission') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Admission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StatusId] [int] NOT NULL,
	[FirstName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MiddleName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CNIC] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PreviousSchoolId] [int] NOT NULL,
	[IdentificationMark] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[BirthCity] [int] NOT NULL,
	[Allergies] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MedicalNotes] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MontherTongue] [int] NOT NULL,
	[Nationality] [int] NOT NULL,
	[Religion] [int] NOT NULL,
	[BloodGroup] [int] NOT NULL,
	[Caste] [int] NOT NULL,
	[GuardianTypeId] [int] NOT NULL,
	[FatherFullName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FatherCNIC] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FatherDateOfBirth] [datetime] NOT NULL,
	[FatherIsDeceased] [bit] NOT NULL,
	[FatherQaulification] [int] NOT NULL,
	[FatherPhoneNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FatherProfession] [int] NOT NULL,
	[FatherMonthlyIncome] [decimal](18, 0) NOT NULL,
	[Father_MontherTongue] [int] NOT NULL,
	[FatherNationality] [int] NOT NULL,
	[FatherReligion] [int] NOT NULL,
	[FatherBloodGroup] [int] NOT NULL,
	[FatherCaste] [int] NOT NULL,
	[MotherFullName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MotherCNIC] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MotherDateOfBirth] [datetime] NOT NULL,
	[MotherIsDeceased] [bit] NOT NULL,
	[MotherQaulification] [int] NOT NULL,
	[MotherPhoneNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MotherProfession] [int] NOT NULL,
	[MotherMonthlyIncome] [decimal](18, 0) NOT NULL,
	[Mother_MontherTongue] [int] NOT NULL,
	[MotherNationality] [int] NOT NULL,
	[MotherReligion] [int] NOT NULL,
	[MotherBloodGroup] [int] NOT NULL,
	[MotherCaste] [int] NOT NULL,
	[GuardianFullName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[GuardianCNIC] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GuardianDateOfBirth] [datetime] NOT NULL,
	[GuardianIsDeceased] [bit] NOT NULL,
	[GuardianQaulification] [int] NOT NULL,
	[GuardianPhoneNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[GuardianProfession] [int] NOT NULL,
	[GuardianMonthlyIncome] [decimal](18, 0) NOT NULL,
	[Guardian_MontherTongue] [int] NOT NULL,
	[GuardianNationality] [int] NOT NULL,
	[GuardianReligion] [int] NOT NULL,
	[GuardianBloodGroup] [int] NOT NULL,
	[GuardianCaste] [int] NOT NULL,
	[Createdby] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[FormNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [StatusId]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [PreviousSchoolId]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [BirthCity]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MontherTongue]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Nationality]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Religion]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [BloodGroup]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Caste]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianTypeId]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherIsDeceased]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherQaulification]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherProfession]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherMonthlyIncome]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Father_MontherTongue]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherNationality]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherReligion]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherBloodGroup]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [FatherCaste]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherIsDeceased]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherQaulification]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherProfession]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherMonthlyIncome]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Mother_MontherTongue]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherNationality]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherReligion]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherBloodGroup]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [MotherCaste]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianIsDeceased]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianQaulification]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianProfession]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianMonthlyIncome]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Guardian_MontherTongue]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianNationality]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianReligion]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianBloodGroup]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [GuardianCaste]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Createdby]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[Admission] ADD  DEFAULT ((0)) FOR [Deleted]
END
