CREATE DEFAULT [dbo].[defCero]
    AS 0;


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[Rol].[Organizador]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[Empleo18a24]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoDesmovilizados]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoDesplazados]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoDesvinculados]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoDirecto]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoDiscapacitados]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoMadres]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoMinorias]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoPrimerAno]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoMetaSocial].[EmpleoRecluidos]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[TabProyecto].[Realizado]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[Institucion].[Inactivo]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoProducto].[PrecioLanzamiento]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoGastos].[Protegido]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoContacto].[Beneficiario]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoContacto].[Inactivo]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoContacto].[Participacion]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[Contacto].[Inactivo]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[Proyecto].[Inactivo]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoProductoInsumo].[Cantidad]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[ProyectoProductoInsumo].[Desperdicio]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[Item].[Protegido]';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[defCero]', @objname = N'[dbo].[EmpresaInterventor].[Inactivo]';

