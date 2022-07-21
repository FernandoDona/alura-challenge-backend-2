CREATE PROCEDURE [dbo].[spRevenue_Delete]
	@Id int
AS
SET NOCOUNT ON;
	
DELETE 
FROM Revenues
WHERE Id = @Id
