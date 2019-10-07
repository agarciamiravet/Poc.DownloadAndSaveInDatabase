![Image name](https://ibb.co/N6rbQvC)

[![Image name](https://ibb.co/N6rbQvC))](http://www.host.com/link.html)

# Steps 

**STEP 1: Create database and table with the following script**

CREATE DATABASE CsvMigrationDatabase

USE [CsvMigrationDatabase]
GO

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

****STEP 2:** Modify few values in **appsettings.json** file:

**ConnectionString :** SQL Server connection string

**BlobStorageFileUrl**: File url of blob storage

**SourceFile**: File where download blbo storage file

**DestinationPath** : directory where the file is to be saved

****STEP 3: Run Project **Poc.DownloadAndSaveInDatabase**
