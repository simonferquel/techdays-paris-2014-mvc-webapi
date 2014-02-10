CREATE TABLE [dbo].[T_UserRoles] (
    [UserId] NVARCHAR (120) NOT NULL,
    [RoleId] NVARCHAR (80)  NOT NULL,
    CONSTRAINT [PK_T_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_T_UserRoles_T_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[T_Roles] ([Id]),
    CONSTRAINT [FK_T_UserRoles_T_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[T_Users] ([Id])
);

