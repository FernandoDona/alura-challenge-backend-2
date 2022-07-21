CREATE PROCEDURE [dbo].[spRevenue_Update]
	@Id INT,
	@Description VARCHAR(100),
	@Type INT,
	@Value DECIMAL(12, 2),
	@Date DATETIME,
	@Category VARCHAR(100) = NULL

AS
SET NOCOUNT ON;

UPDATE Revenues
SET
	Description = @Description, 
	Type = @Type, 
	Value = @Value, 
	Date = @Date,
	Category = @Category
WHERE Id = @Id
