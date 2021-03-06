USE [WebAnalytics]
GO
/****** Object:  Table [dbo].[Definition]    Script Date: 05/10/2011 10:09:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Definition](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[host] [varchar](100) NULL,
	[url] [varchar](max) NOT NULL,
	[page_name] [varchar](200) NULL,
	[var_name] [varchar](100) NOT NULL,
	[referrer] [varchar](400) NULL,
 CONSTRAINT [PK_Definition] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Baseline]    Script Date: 05/10/2011 10:09:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Baseline](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[result_id] [int] NULL,
	[definition_id] [int] NOT NULL,
	[expected_value] [varchar](max) NULL,
	[time_stamp] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Result]    Script Date: 05/10/2011 10:09:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Result](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[definition_id] [int] NOT NULL,
	[actual_result] [varchar](max) NULL,
	[time_stamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DoesVarResultAlreadyExist]    Script Date: 05/10/2011 10:08:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DoesVarResultAlreadyExist]
	(
	@definition_id int,
	@expected_result varchar(MAX),
	@definitionID int OUTPUT
	)
AS

set @definitionID = ( Select definition_id FROM Result
where definition_id = @definition_id
and actual_result = @expected_result )
GO
/****** Object:  StoredProcedure [dbo].[GetDefinitionID]    Script Date: 05/10/2011 10:08:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDefinitionID]
	(
	@host varchar(100),
	@referrer varchar(400),
	@pageName varchar(200),
	@varName varchar(100),
	@definitionID int OUTPUT
	)
AS

set @definitionID = ( Select ID FROM Definition
where host = @host
and referrer = @referrer
and var_name = @varName
and page_name = @pageName )
GO
/****** Object:  ForeignKey [FK_Result_Definition]    Script Date: 05/10/2011 10:09:01 ******/
ALTER TABLE [dbo].[Result]  WITH CHECK ADD  CONSTRAINT [FK_Result_Definition] FOREIGN KEY([definition_id])
REFERENCES [dbo].[Definition] ([id])
GO
ALTER TABLE [dbo].[Result] CHECK CONSTRAINT [FK_Result_Definition]
GO
