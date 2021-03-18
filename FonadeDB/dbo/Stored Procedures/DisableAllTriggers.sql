CREATE PROCEDURE [dbo].[DisableAllTriggers]
	@condicion varchar(20)
AS
	DECLARE @string VARCHAR(8000)
	DECLARE @tableName NVARCHAR(500)
	DECLARE cur CURSOR
	FOR SELECT name AS tbname FROM sysobjects WHERE id IN(SELECT parent_obj FROM sysobjects WHERE xtype='tr')
	OPEN cur
	FETCH next FROM cur INTO @tableName
	WHILE @@fetch_status = 0
	BEGIN
		--SET @string ='Alter table '+ @tableName + ' Enable trigger all'
		--SET @string ='Alter table '+ @tableName + ' ' + @condicion + ' trigger all'
		SET @string ='Alter table '+ @tableName + ' ' + @condicion + ' trigger ' + @tableName + '_ChangeTracking'

		BEGIN TRY
			EXEC (@string);
		END TRY
		BEGIN CATCH
		END CATCH;

		FETCH next FROM cur INTO @tableName
	END
	CLOSE cur
	DEALLOCATE cur