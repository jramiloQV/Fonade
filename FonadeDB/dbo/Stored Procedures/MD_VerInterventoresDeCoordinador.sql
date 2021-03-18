
CREATE PROCEDURE [dbo].[MD_VerInterventoresDeCoordinador]

	@CodContacto int

	
AS

BEGIN

select distinct nombres + ' ' + apellidos as nombre,e.RazonSocial as nomproyecto  from contacto c  INNER JOIN 
Interventor i ON c.Id_Contacto = i.CodContacto 
left JOIN EmpresaInterventor ei on ei.CodContacto= c.Id_Contacto
left join Empresa e on  e.id_empresa = ei.CodEmpresa
 WHERE (i.CodCoordinador = @CodContacto)
 AND e.Id_Empresa = ei.CodEmpresa AND ei.Inactivo = 0 
END