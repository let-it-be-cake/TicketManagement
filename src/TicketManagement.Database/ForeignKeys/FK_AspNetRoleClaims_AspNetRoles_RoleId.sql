ALTER TABLE dbo.AspNetRoleClaims
ADD CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId)     
    REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE