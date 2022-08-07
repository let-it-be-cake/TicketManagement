ALTER TABLE dbo.AspNetUserRoles
ADD CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId)     
    REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE