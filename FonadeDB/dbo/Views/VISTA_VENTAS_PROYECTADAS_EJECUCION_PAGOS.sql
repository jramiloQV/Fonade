CREATE VIEW dbo.VISTA_VENTAS_PROYECTADAS_EJECUCION_PAGOS
AS
SELECT     dbo.Proyecto.Id_Proyecto, dbo.Proyecto.NomProyecto, dbo.Empresa.razonsocial, dbo.LegalizacionActa.CodConvocatoria, 
                      dbo.ProyectoProyeccionesVentasTotal.TotalVentas AS TotalVentasProyectadas, COUNT(dbo.PagoActividad.Id_PagoActividad) AS NumeroDesembolsos, 
                      SUM(dbo.PagoActividad.CantidadDinero) AS ValorDesembolsos, dbo.VISTA_VENTAS_REGISTRADAS.TotalVentas, 
                      dbo.Convocatoria.encargofiduciario
FROM         dbo.LegalizacionActa INNER JOIN
                      dbo.LegalizacionActaProyecto ON dbo.LegalizacionActa.Id_Acta = dbo.LegalizacionActaProyecto.CodActa INNER JOIN
                      dbo.Proyecto INNER JOIN
                      dbo.Empresa ON dbo.Proyecto.Id_Proyecto = dbo.Empresa.codproyecto AND dbo.Proyecto.Id_Proyecto = dbo.Empresa.codproyecto ON 
                      dbo.LegalizacionActaProyecto.CodProyecto = dbo.Proyecto.Id_Proyecto AND 
                      dbo.LegalizacionActaProyecto.CodProyecto = dbo.Empresa.codproyecto INNER JOIN
                      dbo.ProyectoProyeccionesVentasTotal ON dbo.Proyecto.Id_Proyecto = dbo.ProyectoProyeccionesVentasTotal.codproyecto INNER JOIN
                      dbo.PagoActividad ON dbo.Proyecto.Id_Proyecto = dbo.PagoActividad.CodProyecto INNER JOIN
                      dbo.VISTA_VENTAS_REGISTRADAS ON dbo.LegalizacionActaProyecto.CodProyecto = dbo.VISTA_VENTAS_REGISTRADAS.codproyecto INNER JOIN
                      dbo.Convocatoria ON dbo.LegalizacionActa.CodConvocatoria = dbo.Convocatoria.Id_Convocatoria
WHERE     (dbo.LegalizacionActaProyecto.Legalizado = 1)
GROUP BY dbo.Proyecto.NomProyecto, dbo.Proyecto.Id_Proyecto, dbo.Empresa.razonsocial, dbo.LegalizacionActa.CodConvocatoria, 
                      dbo.ProyectoProyeccionesVentasTotal.TotalVentas, dbo.PagoActividad.Estado, dbo.VISTA_VENTAS_REGISTRADAS.TotalVentas, 
                      dbo.Convocatoria.encargofiduciario
HAVING      (dbo.PagoActividad.Estado = 4)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -192
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LegalizacionActa"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 217
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LegalizacionActaProyecto"
            Begin Extent = 
               Top = 6
               Left = 255
               Bottom = 114
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Proyecto"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Empresa"
            Begin Extent = 
               Top = 114
               Left = 270
               Bottom = 222
               Right = 462
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ProyectoProyeccionesVentasTotal"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 315
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PagoActividad"
            Begin Extent = 
               Top = 318
               Left = 38
               Bottom = 426
               Right = 274
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "VISTA_VENTAS_REGISTRADAS"
            Begin Extent = 
               Top = 222
               Left = 243
               Bottom = 300
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_VENTAS_PROYECTADAS_EJECUCION_PAGOS';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Convocatoria"
            Begin Extent = 
               Top = 195
               Left = 491
               Bottom = 303
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_VENTAS_PROYECTADAS_EJECUCION_PAGOS';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_VENTAS_PROYECTADAS_EJECUCION_PAGOS';

