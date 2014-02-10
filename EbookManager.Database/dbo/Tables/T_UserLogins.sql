CREATE TABLE [dbo].[T_UserLogins] (
    [LoginProvider] NVARCHAR (80)  NOT NULL,
    [ProviderKey]   NVARCHAR (120) NOT NULL,
    [UserId]        NVARCHAR (120) NOT NULL,
    CONSTRAINT [PK_T_UserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_T_UserLogins_T_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[T_Users] ([Id])
);

