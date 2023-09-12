USE [workdb]
GO

/****** 对象: Table [dbo].[Product] 脚本日期: 2023/9/12 14:42:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product] (
    [Id]         VARCHAR (100)   NOT NULL,
    [Name]       NVARCHAR (MAX)  NOT NULL,
    [Price]      DECIMAL (18, 2) NOT NULL,
    [CreateTime] BIGINT          NOT NULL,
    [Version]    ROWVERSION      NOT NULL
);


