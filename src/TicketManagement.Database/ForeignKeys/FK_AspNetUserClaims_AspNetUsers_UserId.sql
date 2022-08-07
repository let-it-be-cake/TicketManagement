ALTER TABLE dbo.AspNetUserClaims
ADD CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId)     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE