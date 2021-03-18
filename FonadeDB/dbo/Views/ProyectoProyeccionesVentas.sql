create view ProyectoProyeccionesVentas AS
SELECT SUM(u.Unidades) AS unidades, SUM(u.Unidades) * p.Precio AS total, ProyectoProducto.CodProyecto, p.CodProducto, u.Ano
FROM ProyectoProductoUnidadesVentas u 
INNER JOIN ProyectoProductoPrecio p ON u.CodProducto = p.CodProducto AND u.Ano = p.Periodo 
INNER JOIN ProyectoProducto ON p.CodProducto = ProyectoProducto.Id_Producto
GROUP BY p.Precio, ProyectoProducto.CodProyecto, p.CodProducto, u.Ano