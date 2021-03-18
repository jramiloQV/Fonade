-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_PermisosUsuario]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		BEGIN TRANSACTION

		EXECUTE AS CALLER
		SELECT 'GRANT ALL PRIVILEGES ON ' + TABLE_CATALOG + '.' + TABLE_SCHEMA + '.' + TABLE_NAME  FROM INFORMATION_SCHEMA.TABLES

		COMMIT
END