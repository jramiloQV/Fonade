CREATE VIEW dbo.[A1320_Indicadores_X_Proyecto]
AS
SELECT     P.Id_Proyecto AS Id, T16.NomConvocatoria AS Convocatoria, P.NomProyecto AS Proyecto, T2.razonsocial AS Nombre_Empresa, T2.Nit, T17.NomCiudad AS Ciudad, 
                      T18.NomDepartamento AS Departamento, T6.Denominador AS Meta_Ventas, T6.Numerador AS Ventas_Verificadas, T21.Total AS Total_Empleos_Propuestos, 
                      T21.Generado AS Empleos_primer_año_propuestos, T4.Denominador AS Meta_Empleos, T4.Numerador AS Empleos_Verificados, 
                      T8.Denominador AS Meta_Comercial, T8.Numerador AS Comercial_Verificado, T10.Denominador AS Meta_Mercadeo, T10.Numerador AS Mercadeo_Verificado, 
                      T12.Denominador AS Meta_Produccion, T12.Numerador AS Produccion_Verificada, T14.Denominador AS Meta_Presupuesto, 
                      T14.Numerador AS Presupuesto_Verificado, T17.IDH, T26.Generales AS Recomendación_Interventoria, T23.NomUnidad, T25.NomSector, T24.NomSubSector, 
                      P.Sumario
FROM         dbo.Proyecto AS P INNER JOIN
                      dbo.Empresa AS T2 ON P.Id_Proyecto = T2.codproyecto INNER JOIN
                      dbo.ConvocatoriaProyecto AS T15 ON P.Id_Proyecto = T15.CodProyecto AND T15.Viable = 1 INNER JOIN
                      dbo.Convocatoria AS T16 ON T16.Id_Convocatoria = T15.CodConvocatoria INNER JOIN
                      dbo.Ciudad AS T17 ON T17.Id_Ciudad = P.CodCiudad INNER JOIN
                      dbo.departamento AS T18 ON T18.Id_Departamento = T17.CodDepartamento INNER JOIN
                      dbo.EmpleosGenerados AS T21 ON T21.CodProyecto = P.Id_Proyecto AND T21.CodConvocatoria = T16.Id_Convocatoria INNER JOIN
                      dbo.Institucion AS T23 ON P.CodInstitucion = T23.Id_Institucion INNER JOIN
                      dbo.SubSector AS T24 ON P.CodSubSector = T24.Id_SubSector INNER JOIN
                      dbo.Sector AS T25 ON T24.CodSector = T25.Id_Sector INNER JOIN
                      dbo.EvaluacionObservacion AS T26 ON T26.CodProyecto = P.Id_Proyecto AND T26.CodConvocatoria = T16.Id_Convocatoria INNER JOIN
                      dbo.IndicadorGenericoEmpleo AS T3 ON T2.id_empresa = T3.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T4 ON T3.IndicadorGenerico = T4.Id_IndicadorGenerico INNER JOIN
                      dbo.IndicadorGenericoVentas AS T5 ON T2.id_empresa = T5.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T6 ON T5.IndicadorGenerico = T6.Id_IndicadorGenerico INNER JOIN
                      dbo.IndicadorGenericoComercial AS T7 ON T2.id_empresa = T7.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T8 ON T7.IndicadorGenerico = T8.Id_IndicadorGenerico INNER JOIN
                      dbo.IndicadorGenericoMercadeo AS T9 ON T2.id_empresa = T9.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T10 ON T9.IndicadorGenerico = T10.Id_IndicadorGenerico INNER JOIN
                      dbo.IndicadorGenericoProduccion AS T11 ON T2.id_empresa = T11.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T12 ON T11.IndicadorGenerico = T12.Id_IndicadorGenerico INNER JOIN
                      dbo.INdicadorGenericoPresupuestal AS T13 ON T2.id_empresa = T13.CodEmpresa INNER JOIN
                      dbo.IndicadorGenerico AS T14 ON T13.IndicadorGenerico = T14.Id_IndicadorGenerico
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
         Left = -1581
      End
      Begin Tables = 
         Begin Table = "P"
            Begin Extent = 
               Top = 14
               Left = 44
               Bottom = 122
               Right = 222
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T2"
            Begin Extent = 
               Top = 11
               Left = 262
               Bottom = 314
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T3"
            Begin Extent = 
               Top = 16
               Left = 694
               Bottom = 94
               Right = 861
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 135
               Left = 891
               Bottom = 243
               Right = 1074
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "T5"
            Begin Extent = 
               Top = 18
               Left = 477
               Bottom = 96
               Right = 644
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T6"
            Begin Extent = 
               Top = 134
               Left = 674
               Bottom = 242
               Right = 857
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "T7"
            Begin Extent = 
               Top = 11
               Left = 904
               Bottom = 89
               Right = 1071
            End
            DisplayFlags = 280
            TopColumn = 0
    ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'A1320_Indicadores_X_Proyecto';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     End
         Begin Table = "T8"
            Begin Extent = 
               Top = 129
               Left = 1101
               Bottom = 237
               Right = 1284
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "T9"
            Begin Extent = 
               Top = 11
               Left = 1105
               Bottom = 89
               Right = 1272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T10"
            Begin Extent = 
               Top = 128
               Left = 1301
               Bottom = 236
               Right = 1484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T11"
            Begin Extent = 
               Top = 11
               Left = 1300
               Bottom = 89
               Right = 1467
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T12"
            Begin Extent = 
               Top = 125
               Left = 1496
               Bottom = 233
               Right = 1679
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "T13"
            Begin Extent = 
               Top = 9
               Left = 1494
               Bottom = 87
               Right = 1661
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T14"
            Begin Extent = 
               Top = 132
               Left = 1689
               Bottom = 240
               Right = 1872
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "T15"
            Begin Extent = 
               Top = 6
               Left = 1699
               Bottom = 114
               Right = 1896
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T16"
            Begin Extent = 
               Top = 6
               Left = 1934
               Bottom = 114
               Right = 2099
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T17"
            Begin Extent = 
               Top = 6
               Left = 2137
               Bottom = 114
               Right = 2305
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T18"
            Begin Extent = 
               Top = 6
               Left = 2343
               Bottom = 99
               Right = 2513
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T21"
            Begin Extent = 
               Top = 102
               Left = 2343
               Bottom = 349
               Right = 2506
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T23"
            Begin Extent = 
               Top = 114
               Left = 1910
               Bottom = 222
               Right = 2083
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T24"
            Begin Extent = 
               Top = 114
               Left = 2121
               Bottom = 222
               Right = 2272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T25"
            Begin Extent = 
               Top = 222
               Lef', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'A1320_Indicadores_X_Proyecto';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane3', @value = N't = 1910
               Bottom = 330
               Right = 2061
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T26"
            Begin Extent = 
               Top = 222
               Left = 2099
               Bottom = 330
               Right = 2299
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
      Begin ColumnWidths = 28
         Width = 284
         Width = 1500
         Width = 7275
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
         Column = 1575
         Alias = 2595
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'A1320_Indicadores_X_Proyecto';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 3, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'A1320_Indicadores_X_Proyecto';

