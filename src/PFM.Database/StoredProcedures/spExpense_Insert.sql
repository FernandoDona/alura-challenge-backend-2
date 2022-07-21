CREATE PROCEDURE [dbo].[spExpense_Insert]
	@Description VARCHAR(100),
	@Type INT,
	@Value DECIMAL(12, 2),
	@Date DATETIME,
	@Category VARCHAR(100) = NULL

AS
	SET NOCOUNT ON;

	INSERT INTO Expenses
		(Description, Type, Value, Date, Category)
	VALUES
		(@Description, @Type, @Value, @Date, @Category)

RETURN SELECT SCOPE_IDENTITY()
