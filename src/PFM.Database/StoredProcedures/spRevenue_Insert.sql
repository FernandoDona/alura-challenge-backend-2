CREATE PROCEDURE [dbo].[spRevenue_Insert]
	@Description VARCHAR(100),
	@Type INT,
	@Value DECIMAL(12, 2),
	@Date DATETIME,
	@Category VARCHAR(100) = NULL

AS
	SET NOCOUNT ON;

	INSERT INTO Revenues
		(Description, Type, Value, Date, Category)
	VALUES
		(@Description, @Type, @Value, @Date, @Category)

    SELECT SCOPE_IDENTITY();
