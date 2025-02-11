# dotnet 9

appsettings.json, config ConnectionStrings.DefaultConnection

# dotnet ef migrations

dotnet ef migrations add CreateIdentityTables

dotnet ef database update

# sql server 2022

USE [sqldb]
GO

/**\*\*** Object: Table [dbo].[tb_note] Script Date: 2/11/2025 6:07:31 AM **\*\***/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_note](
[id] [int] IDENTITY(1,1) NOT NULL,
[title] [varchar](10) NOT NULL,
[content] [varchar](100) NULL,
[created_at] [datetime] NOT NULL,
[updated_at] [datetime] NULL,
[user_id] [nvarchar](450) NOT NULL,
CONSTRAINT [PK_tb_note] PRIMARY KEY CLUSTERED
(
[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tb_note] ADD CONSTRAINT [DF_tb_note_created_at] DEFAULT (getdate()) FOR [created_at]
GO
