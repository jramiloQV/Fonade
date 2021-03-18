/****** Script for SelectTopNRows command from SSMS  ******/

CREATE Procedure [dbo].[GetPosicionArancelaria]
(
@Descripcion  varchar(800)
)
as
declare @Codigo  varchar(10)
if isnumeric(@Descripcion)=1
begin 
	set @Codigo=@Descripcion
	set @Descripcion=''
end
else
begin
	set @Codigo=''
end
SELECT distinct rtrim(PosicionArancelaria) +' '+ Descripcion Descripcion 
   FROM [Fonade].[dbo].[PosicionArancelaria]
   where (PosicionArancelaria like ''+ @Codigo+ '%' or @Codigo='') and (Descripcion like '%'+ @Descripcion +'%' or @Descripcion='')
   order by Descripcion
/*
SELECT distinct len(descripcion) Longitud, (len(descripcion)/120)+1 Lineas, rtrim(PosicionArancelaria) PosicionArancelaria   , substring(Descripcion ,0,500) Descripcion 
   FROM [Fonade].[dbo].[PosicionArancelaria]
   where (PosicionArancelaria like ''+ @Codigo+ '%' or @Codigo='') and (Descripcion like '%'+ @Descripcion +'%' or @Descripcion='')
   order by Descripcion
  GetPosicionArancelaria '3405900000'
  GetPosicionArancelaria trigo
  GetPosicionArancelaria celular
  select len('carnes y despojos comestibles, salados o en salmuera, secos o ahumados ')
  select len('incluidos la harina y polvo comestibles, de carne o despojos, de')

*/