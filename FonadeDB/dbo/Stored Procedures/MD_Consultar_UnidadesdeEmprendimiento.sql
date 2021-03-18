-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- Modificacion: John Betancourt A. 1 Agosto 2014
-- =============================================  
CREATE PROCEDURE [dbo].[MD_Consultar_UnidadesdeEmprendimiento]  
 @codTipoInstitucion tinyint,
 @buscar varchar(100)
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 if @codTipoInstitucion = 2  
  begin  
   SELECT T.NomTipoInstitucion, I.NomInstitucion, I.NomUnidad, CD.NomCiudad, D.NomDepartamento, C.Email, C.Nombres, C.Apellidos, I.Telefono  
   FROM TipoInstitucion T, Institucion I, Contacto C, InstitucionContacto IC, Ciudad CD, Departamento D  
   WHERE T.Id_TipoInstitucion = I.CodTipoInstitucion  
   AND I.CodCiudad = CD.Id_Ciudad  
   AND CD.CodDepartamento = D.Id_Departamento  
   AND IC.CodInstitucion = I.Id_Institucion  
   AND IC.FechaFin IS NULL  
   AND IC.CodContacto = C.Id_Contacto  
   AND I.Inactivo = 0 AND NomUnidad like @buscar+'%'
  end  
 else  
 begin  
    -- Insert statements for procedure here  
  SELECT T.NomTipoInstitucion, I.NomInstitucion, I.NomUnidad, CD.NomCiudad, D.NomDepartamento, C.Email, C.Nombres, C.Apellidos, I.Telefono  
  FROM TipoInstitucion T, Institucion I, Contacto C, InstitucionContacto IC, Ciudad CD, Departamento D  
  WHERE T.Id_TipoInstitucion = I.CodTipoInstitucion  
  AND I.CodCiudad = CD.Id_Ciudad  
  AND CD.CodDepartamento = D.Id_Departamento  
  AND IC.CodInstitucion = I.Id_Institucion  
  AND IC.FechaFin IS NULL  
  AND IC.CodContacto = C.Id_Contacto  
  AND I.Inactivo = 0 AND I.codTipoInstitucion = @codTipoInstitucion AND NomUnidad like @buscar+'%'
  end  
END