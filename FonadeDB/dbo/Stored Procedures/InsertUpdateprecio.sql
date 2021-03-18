CREATE procedure [dbo].[InsertUpdateprecio](@Codproducto int,  @Precio char(10),@Periodo int)
as
if(SELECT   count(1) FROM [ProyectoProductoPrecio] where Codproducto=@Codproducto and Periodo=@Periodo)=0
begin
INSERT INTO ProyectoProductoPrecio(Codproducto,Precio,Periodo) VALUES (@Codproducto,rtrim(ltrim(@Precio)),@Periodo)
end
ELSE
BEGIN
	UPDATE ProyectoProductoPrecio SET Precio=rtrim(ltrim(@Precio)) WHERE  Codproducto=@Codproducto AND PERIODO=@Periodo
END