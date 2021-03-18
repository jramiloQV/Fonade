CREATE PROCEDURE CAMBIAPROPIETARIO
 
 AS
 
 DECLARE @tabla VARCHAR(100)
 DECLARE @propietario VARCHAR(100)
 DECLARE @catalogo VARCHAR(100)
 DECLARE @strsql nVARCHAR(100)
 
 DECLARE bd_cursor CURSOR FOR
 SELECT TABLE_CATALOG,TABLE_NAME,TABLE_SCHEMA from INFORMATION_SCHEMA.tables where table_schema<>'dbo'
 
 OPEN bd_cursor
 FETCH NEXT FROM bd_cursor
 INTO @catalogo,@tabla,@propietario
 WHILE @@FETCH_STATUS = 0
 	BEGIN
 		select @strsql = N'sp_changeobjectowner N''' + @propietario+'.'+@tabla+ ''', N''dbo'''
 		exec sp_executeSQL @strsql
 		print @tabla + ' cambiada'
 		FETCH NEXT FROM bd_cursor
 		INTO @catalogo,@tabla,@propietario
 	END 
 
 CLOSE bd_cursor
 DEALLOCATE bd_cursor