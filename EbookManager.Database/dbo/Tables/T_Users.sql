CREATE TABLE [dbo].[T_Users] (
    [Id]            NVARCHAR (120) NOT NULL,
    [UserName]      NVARCHAR (120) NOT NULL,
    [PasswordHash]  NVARCHAR (255) NULL,
    [SecurityStamp] NVARCHAR (255) NULL,
    CONSTRAINT [PK_T_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_T_Users]
    ON [dbo].[T_Users]([UserName] ASC);

