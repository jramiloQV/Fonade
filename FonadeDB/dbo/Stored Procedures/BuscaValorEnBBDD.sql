CREATE PROC [dbo].[BuscaValorEnBBDD]
(
@StrValorBusqueda nvarchar(100)
)
AS
BEGIN

CREATE TABLE #Resultado (NombreColumna nvarchar(370), ValorColumna nvarchar(3630))
SET NOCOUNT ON

DECLARE @NombreTabla nvarchar(256),
@NombreColumna nvarchar(128),
@StrValorBusqueda2 nvarchar(110)

SET  @NombreTabla = ''
SET @StrValorBusqueda2 = QUOTENAME('%' + @StrValorBusqueda + '%','''')

WHILE @NombreTabla IS NOT NULL
     BEGIN
     SET @NombreColumna = ''
     SET @NombreTabla =
     (SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
     FROM INFORMATION_SCHEMA.TABLES
     WHERE TABLE_TYPE = 'BASE TABLE'
     AND QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @NombreTabla
     AND OBJECTPROPERTY(
     OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), 'IsMSShipped') = 0)

     WHILE (@NombreTabla IS NOT NULL) AND (@NombreColumna IS NOT NULL)
         BEGIN
         SET @NombreColumna =
         (SELECT MIN(QUOTENAME(COLUMN_NAME))
         FROM INFORMATION_SCHEMA.COLUMNS
         WHERE TABLE_SCHEMA = PARSENAME(@NombreTabla, 2)
         AND TABLE_NAME = PARSENAME(@NombreTabla, 1)
         AND DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar')
         AND QUOTENAME(COLUMN_NAME) > @NombreColumna)

         IF @NombreColumna IS NOT NULL
              BEGIN
              INSERT INTO #Resultado
              EXEC
              ('SELECT ''' + @NombreTabla + '.' + @NombreColumna + ''', LEFT(' + @NombreColumna + ', 3630)
              FROM ' + @NombreTabla + ' (NOLOCK) ' + ' WHERE ' + @NombreColumna + ' LIKE ' + @StrValorBusqueda2)
              END 
         END
     END
     SELECT NombreColumna, ValorColumna FROM #Resultado
END