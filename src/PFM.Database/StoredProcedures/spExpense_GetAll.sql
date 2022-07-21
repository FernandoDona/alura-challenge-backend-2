CREATE PROCEDURE [dbo].[spExpense_GetAll]

AS
SET NOCOUNT ON;
	
SELECT 
	Id, Description, Type, Value, Date, Category
FROM Expenses
