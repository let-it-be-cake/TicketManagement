ALTER TABLE dbo.AspNetUserTokens
ADD CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId)     
    REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE