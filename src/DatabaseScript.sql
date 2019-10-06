CREATE DATABASE CsvMigrationDatabase

USE [CsvMigrationDatabase]
GO

/****** Object:  Table [dbo].[CsvData]    Script Date: 06/10/2019 11:54:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CsvData](
	[PointOfSale] [varchar](150) NULL,
	[Date] [varchar](150) NULL,
	[Stock] [varchar](500) NULL,
	[Product] [varchar](500) NULL
) ON [PRIMARY]
GO


