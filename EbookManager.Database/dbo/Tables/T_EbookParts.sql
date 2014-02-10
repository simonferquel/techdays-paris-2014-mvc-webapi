CREATE TABLE [dbo].[T_EbookParts] (
    [EbookId]     UNIQUEIDENTIFIER NOT NULL,
    [Position]    INT              NOT NULL,
    [PartContent] VARBINARY (MAX)  NOT NULL,
    [ContentType] NVARCHAR (50)    NOT NULL,
    [FileName]    NVARCHAR (120)   NOT NULL,
    CONSTRAINT [PK_T_EbookParts] PRIMARY KEY CLUSTERED ([EbookId] ASC, [Position] ASC),
    CONSTRAINT [FK_T_EbookParts_T_Ebooks] FOREIGN KEY ([EbookId]) REFERENCES [dbo].[T_Ebooks] ([Id])
);





