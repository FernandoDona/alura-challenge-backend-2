CREATE PROCEDURE [dbo].[spRevenue_Get]
	@Id int = NULL,
	@Description VARCHAR(100) = NULL
AS
SET NOCOUNT ON;
	
IF @Id IS NOT NULL
BEGIN
	SELECT 
		Id, Description, Type, Value, Date, Category
	FROM Revenues
	WHERE Id = @Id
END
ELSE
BEGIN
	SELECT 
		Id, Description, Type, Value, Date, Category
	FROM Revenues
	WHERE 
		Description LIKE '%' + @Description + '%'
END