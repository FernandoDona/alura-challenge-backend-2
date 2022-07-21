CREATE PROCEDURE [dbo].[spRevenue_GetAll]

AS
SET NOCOUNT ON;
	
SELECT 
	Id, Description, Type, Value, Date, Category
FROM Revenues
