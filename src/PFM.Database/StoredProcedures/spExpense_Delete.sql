CREATE PROCEDURE [dbo].[spExpense_Delete]
	@Id int
AS
SET NOCOUNT ON;
	
DELETE 
FROM Expenses
WHERE Id = @Id
