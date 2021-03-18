CREATE DEFAULT [dbo].[defUno]
    AS 1;


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defUno]', @objname = N'[dbo].[Objeto].[Menu]';

