CREATE TABLE [dbo].[T_UserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ClaimType]  NVARCHAR (120) NOT NULL,
    [ClaimValue] NVARCHAR (120) NOT NULL,
    [UserId]     NVARCHAR (120) NOT NULL,
    CONSTRAINT [PK_T_UserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_UserClaims_T_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[T_Users] ([Id])
);

