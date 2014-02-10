CREATE TABLE [dbo].[T_Ebooks] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Title]     NVARCHAR (120)   NOT NULL,
    [Summary]   NVARCHAR (500)   NOT NULL,
    [Thumbnail] VARBINARY (MAX)  NOT NULL,
    CONSTRAINT [PK_T_Ebooks] PRIMARY KEY CLUSTERED ([Id] ASC)
);



