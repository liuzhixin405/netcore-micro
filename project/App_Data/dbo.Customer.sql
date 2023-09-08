USE [workdb]
GO

/****** 对象: Table [dbo].[Customer] 脚本日期: 2023/9/8 12:05:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [FirstName]  VARCHAR (10) NOT NULL,
    [LastName]   VARCHAR (10) NOT NULL,
    [CreateTime] BIGINT       NOT NULL
);


