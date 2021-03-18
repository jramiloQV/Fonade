CREATE VIEW dbo.VISTA_RECURSOS_PROYECTOS_EJECUCION
AS
SELECT     TOP (100) PERCENT p.Id_Proyecto, p.NomProyecto, c.Id_Convocatoria, eo.ValorRecomendado, DATEPART(yyyy, c.FechaInicio) AS AnnoConvocatoria, 
                      CASE DatePart(yyyy, fechaInicio) 
                      WHEN '2004' THEN ValorRecomendado * 358000 WHEN '2005' THEN ValorRecomendado * 381500 WHEN '2006' THEN ValorRecomendado * 408000 WHEN
                       '2007' THEN ValorRecomendado * 433700 WHEN '2008' THEN ValorRecomendado * 461500 WHEN '2009' THEN ValorRecomendado * 496900 ELSE ValorRecomendado
                       * 515000 END AS TotalRecomendado, pa.DineroDesembolsado
FROM         dbo.Proyecto AS p INNER JOIN
                          (SELECT     CodProyecto, MAX(CodConvocatoria) AS CodConvocatoria
                            FROM          dbo.ConvocatoriaProyecto
                            WHERE      (Viable = 1)
                            GROUP BY CodProyecto) AS CP ON CP.CodProyecto = p.Id_Proyecto LEFT OUTER JOIN
                      dbo.EvaluacionObservacion AS eo ON eo.CodProyecto = p.Id_Proyecto AND eo.CodConvocatoria = CP.CodConvocatoria LEFT OUTER JOIN
                      dbo.Convocatoria AS c ON c.Id_Convocatoria = eo.CodConvocatoria LEFT OUTER JOIN
                          (SELECT     CodProyecto, COUNT(Id_PagoActividad) AS pago, SUM(CantidadDinero) AS DineroDesembolsado
                            FROM          dbo.PagoActividad
                            WHERE      (Estado >= 1) AND (Estado < 5)
                            GROUP BY CodProyecto) AS pa ON p.Id_Proyecto = pa.CodProyecto
WHERE     (p.CodEstado = 7) AND (p.Id_Proyecto NOT IN (1212, 2305, 14147))
ORDER BY p.Id_Proyecto
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
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CP"
            Begin Extent = 
               Top = 6
               Left = 254
               Bottom = 84
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "eo"
            Begin Extent = 
               Top = 6
               Left = 489
               Bottom = 114
               Right = 689
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pa"
            Begin Extent = 
               Top = 84
               Left = 254
               Bottom = 177
               Right = 434
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
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_RECURSOS_PROYECTOS_EJECUCION';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'= 720
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_RECURSOS_PROYECTOS_EJECUCION';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_RECURSOS_PROYECTOS_EJECUCION';

