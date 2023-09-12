CREATE TABLE [dbo].[Product] (
    [Id]         BIGINT   NOT NULL,
    [Name]       NVARCHAR (MAX)  NOT NULL,
    [Price]      DECIMAL (18, 2) NOT NULL,
    [CreateTime] BIGINT          NOT NULL,
    [Version]    ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);

