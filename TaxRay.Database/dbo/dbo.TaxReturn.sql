CREATE TABLE [dbo].[TaxReturn]
([Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
[AtlasId] NVARCHAR(20) NOT NULL,
	[Description] NVARCHAR(50) NOT NULL,
	[Client] NVARCHAR(50) NOT NULL, 
	[TaxPayer] NVARCHAR(50) NOT NULL, 
	[Year] INT NOT NULL, 
	[DueDate] DATE NOT NULL, 
	[Status] NVARCHAR(50) NOT NULL, 
	[AssignedToId] INT NULL, 
	[CreatedById] INT NOT NULL, 
    CONSTRAINT [FK_AssignedTo_User] FOREIGN KEY (AssignedToId) REFERENCES [User]([Id]),
    CONSTRAINT [FK_CreatedBy_User] FOREIGN KEY (CreatedById) REFERENCES [User]([Id])
	
	
)
