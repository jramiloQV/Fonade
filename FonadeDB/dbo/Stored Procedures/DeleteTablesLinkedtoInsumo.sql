CREATE PROCEDURE [dbo].[DeleteTablesLinkedtoInsumo]
(
@CodInsumo numeric
)
  as
delete ProyectoProductoInsumo where  CodInsumo = @CodInsumo 
delete ProyectoInsumoPrecio where  CodInsumo = @CodInsumo 
delete ProyectoInsumo where  id_Insumo = @CodInsumo