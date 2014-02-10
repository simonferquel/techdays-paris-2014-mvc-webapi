CREATE TABLE [dbo].[T_UserEbooks] (
    [UserId]  NVARCHAR (120)   NOT NULL,
    [EbookId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_T_UserEbooks] PRIMARY KEY CLUSTERED ([UserId] ASC, [EbookId] ASC),
    CONSTRAINT [FK_T_UserEbooks_T_Ebooks] FOREIGN KEY ([EbookId]) REFERENCES [dbo].[T_Ebooks] ([Id]),
    CONSTRAINT [FK_T_UserEbooks_T_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[T_Users] ([Id])
);

