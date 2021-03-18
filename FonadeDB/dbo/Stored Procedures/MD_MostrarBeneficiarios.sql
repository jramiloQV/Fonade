
CREATE PROCEDURE [dbo].[MD_MostrarBeneficiarios]
	@codProyecto int
AS

BEGIN

	SELECT Case when Count(pa.CodPagoBeneficiario) = 0 Then 1 else 0 End CodPagoBeneficiario, pb.Id_PagoBeneficiario, pb.Nombre, pb.Apellido, pb.Email,
	pb.RazonSocial, pb.CodPagoSucursal, c.NomCiudad FROM PagoBeneficiario pb
	INNER JOIN Ciudad c ON pb.CodCiudad = c.Id_Ciudad 
	INNER JOIN Empresa e ON pb.CodEmpresa = e.id_empresa
	Inner JOin departamento d on d.Id_Departamento = c.CodDepartamento
	Left Join  PagoActividad pa on pa.CodPagoBeneficiario = pb.Id_PagoBeneficiario
	WHERE e.codproyecto = @codProyecto group by pb.Id_PagoBeneficiario, pb.Nombre, pb.Apellido, 
		pb.Email,pb.RazonSocial, pb.CodPagoSucursal, c.NomCiudad, d.NomDepartamento
	
	--SELECT CASE 
	--				WHEN COUNT(CodPagoBeneficiario) = 0 
	--					THEN 1
	--				ELSE  0
	--			END as CodPagoBeneficiario

	--, PagoBeneficiario.Id_PagoBeneficiario, PagoBeneficiario.Nombre, PagoBeneficiario.Apellido, 
	--PagoBeneficiario.Email,PagoBeneficiario.RazonSocial, PagoBeneficiario.CodPagoSucursal, 
	--Ciudad.NomCiudad  as NomCiudad, departamento.NomDepartamento as NomDepartamento
	--FROM PagoBeneficiario INNER JOIN Ciudad ON PagoBeneficiario.CodCiudad = Ciudad.Id_Ciudad INNER JOIN
	--Empresa ON PagoBeneficiario.CodEmpresa = Empresa.id_empresa
	--inner join departamento on Id_Departamento=Ciudad.CodDepartamento
	--left join PagoActividad on CodPagoBeneficiario= Id_PagoBeneficiario
	--WHERE     Empresa.codproyecto = @codProyecto
	--group by PagoBeneficiario.Id_PagoBeneficiario, PagoBeneficiario.Nombre, PagoBeneficiario.Apellido, 
	--PagoBeneficiario.Email,PagoBeneficiario.RazonSocial, PagoBeneficiario.CodPagoSucursal, NomCiudad, NomDepartamento
END