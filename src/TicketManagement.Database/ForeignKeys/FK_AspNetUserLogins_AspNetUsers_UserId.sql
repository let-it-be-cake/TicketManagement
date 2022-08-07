ALTER TABLE dbo.AspNetUserLogins
ADD CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId)     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE