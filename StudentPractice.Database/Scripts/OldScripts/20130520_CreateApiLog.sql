USE [StudentPractice-App]
GO

/****** Object:  Table [dbo].[StudentPracticeApiLog]    Script Date: 20/5/2013 3:11:58 μμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StudentPracticeApiLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCaller] [int] NOT NULL,
	[ServiceCalledAt] [datetime] NOT NULL,
	[ServiceMethodCalled] [nvarchar](50) NOT NULL,
	[ServiceCallerID] [int] NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[Success] [bit] NULL,
	[IP] [nvarchar](50) NULL,
	[Request] [xml] NULL,
 CONSTRAINT [PK_InternalServiceLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

