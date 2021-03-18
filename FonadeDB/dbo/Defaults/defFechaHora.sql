CREATE DEFAULT [dbo].[defFechaHora]
    AS getdate();


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defFechaHora]', @objname = N'[dbo].[LogIngreso].[FechaUltimoIngreso]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defFechaHora]', @objname = N'[dbo].[Contacto].[fechaCreacion]';

