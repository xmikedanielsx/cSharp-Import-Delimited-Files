CREATE TYPE [dbo].[OriginalTableList] AS TABLE(
	[tbl] [varchar](200) NOT NULL, [srcFile] [varchar](5000) NULL,
	UNIQUE NONCLUSTERED 
(
	[tbl] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)



