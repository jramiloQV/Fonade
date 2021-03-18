CREATE DEFAULT [dbo].[defFecha]
    AS CONVERT (CHAR (10), getdate(), 101);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defFecha]', @objname = N'[dbo].[EvaluacionSeguimiento].[FechaActualizacion]';

