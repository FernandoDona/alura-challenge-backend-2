CREATE TABLE [dbo].[Revenues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Description] VARCHAR(100) NOT NULL, 
    [Value] DECIMAL(12, 2) NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Type] INT NOT NULL, 
    [Category] VARCHAR(100) NULL

    FOREIGN KEY ([Type]) REFERENCES ExpenseRevenueType(Id)
)
