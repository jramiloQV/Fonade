CREATE VIEW dbo.VISTA_PAGOS_INDICADORES
AS
SELECT DISTINCT 
                      T1.encargofiduciario, T4.CodConvocatoria, T3.razonsocial, T7.Id_Proyecto, T7.NomProyecto, T4.ValorRecomendado AS ValorAprobado, T5.NombreIndicador, 
                      p.pago AS NoDesembolsos, p.dinero AS ValorDesembolsos, indic.denom AS Proyectado, indic.numer AS Registrado, MAX(num.VtasRegistradas) 
                      AS PorcentajeVtas
FROM         dbo.Proyecto AS T7 LEFT OUTER JOIN
                      dbo.ConvocatoriaProyecto AS T2 ON T7.Id_Proyecto = T2.CodProyecto LEFT OUTER JOIN
                      dbo.EvaluacionObservacion AS T4 ON T7.Id_Proyecto = T4.CodProyecto AND T2.CodConvocatoria = T4.CodConvocatoria LEFT OUTER JOIN
                      dbo.Empresa AS T3 ON T7.Id_Proyecto = T3.codproyecto INNER JOIN
                      dbo.Convocatoria AS T1 ON T2.CodConvocatoria = T1.Id_Convocatoria INNER JOIN
                      dbo.IndicadorGenerico AS T5 ON T3.id_empresa = T5.CodEmpresa INNER JOIN
                          (SELECT     CodProyecto, COUNT(Id_PagoActividad) AS pago, SUM(CantidadDinero) AS dinero
                            FROM          dbo.PagoActividad
                            WHERE      (Estado = 4)
                            GROUP BY CodProyecto) AS p ON T7.Id_Proyecto = p.CodProyecto INNER JOIN
                          (SELECT     CodEmpresa, NombreIndicador, CONVERT(Numeric(18, 1), CONVERT(Numeric(18, 2), Numerador) / CONVERT(Numeric(18, 2), Denominador)) 
                                                   * 100 AS VtasRegistradas
                            FROM          dbo.IndicadorGenerico AS T5
                            WHERE      (Denominador <> 0) AND (Denominador IS NOT NULL)) AS num ON T3.id_empresa = num.CodEmpresa AND 
                      T5.NombreIndicador = num.NombreIndicador INNER JOIN
                          (SELECT     CodProyecto, MAX(CodConvocatoria) AS codconvoc
                            FROM          dbo.EvaluacionObservacion
                            GROUP BY CodProyecto) AS convoc ON T4.CodProyecto = convoc.CodProyecto AND T4.CodConvocatoria = convoc.codconvoc INNER JOIN
                          (SELECT     CodEmpresa, NombreIndicador, MAX(Numerador) AS numer, MAX(Denominador) AS denom
                            FROM          dbo.IndicadorGenerico
                            GROUP BY CodEmpresa, NombreIndicador) AS indic ON T3.id_empresa = indic.CodEmpresa AND T5.NombreIndicador = indic.NombreIndicador
WHERE     (T7.CodEstado = 7) AND (T2.Viable = 1)
GROUP BY T7.Id_Proyecto, T4.CodConvocatoria, T7.NomProyecto, T3.razonsocial, T4.ValorRecomendado, T5.NombreIndicador, indic.denom, indic.numer, p.pago, p.dinero, 
                      T1.encargofiduciario, num.VtasRegistradas
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[10] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
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
         Begin Table = "T7"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T2"
            Begin Extent = 
               Top = 6
               Left = 254
               Bottom = 114
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 6
               Left = 489
               Bottom = 114
               Right = 689
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T3"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 19
         End
         Begin Table = "T1"
            Begin Extent = 
               Top = 114
               Left = 252
               Bottom = 222
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T5"
            Begin Extent = 
               Top = 114
               Left = 455
               Bottom = 222
               Right = 638
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 315
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         E', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_PAGOS_INDICADORES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'nd
         Begin Table = "num"
            Begin Extent = 
               Top = 222
               Left = 227
               Bottom = 315
               Right = 405
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "convoc"
            Begin Extent = 
               Top = 222
               Left = 443
               Bottom = 300
               Right = 610
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "indic"
            Begin Extent = 
               Top = 300
               Left = 443
               Bottom = 408
               Right = 621
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
      Begin ColumnWidths = 13
         Width = 284
         Width = 1500
         Width = 1500
         Width = 645
         Width = 3660
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_PAGOS_INDICADORES';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_PAGOS_INDICADORES';

