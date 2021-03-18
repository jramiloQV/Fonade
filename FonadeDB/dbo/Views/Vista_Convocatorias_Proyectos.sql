CREATE VIEW dbo.Vista_Convocatorias_Proyectos
AS
SELECT     p.Id_Proyecto, p.NomProyecto, est.Id_Estado, est.NomEstado, conv.Id_Convocatoria, conv.NomConvocatoria, i.Id_Institucion, 
                      i.NomUnidad + ' (' + i.NomInstitucion + ')' AS NomInstitucion, cp.Viable, c.Nombres + ' ' + c.Apellidos AS Asesor, 
                      c2.Nombres + ' ' + c2.Apellidos AS JefeUnidad, dbo.Ciudad.NomCiudad, dbo.departamento.NomDepartamento
FROM         dbo.Proyecto AS p INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON p.Id_Proyecto = cp.CodProyecto INNER JOIN
                      dbo.Estado AS est ON est.Id_Estado = p.CodEstado INNER JOIN
                      dbo.Convocatoria AS conv ON conv.Id_Convocatoria = cp.CodConvocatoria INNER JOIN
                      dbo.Institucion AS i ON i.Id_Institucion = p.CodInstitucion INNER JOIN
                      dbo.Ciudad ON dbo.Ciudad.Id_Ciudad = p.CodCiudad INNER JOIN
                      dbo.departamento ON dbo.departamento.Id_Departamento = dbo.Ciudad.CodDepartamento LEFT OUTER JOIN
                      dbo.ProyectoContacto AS pc ON p.Id_Proyecto = pc.CodProyecto AND pc.CodRol IN (1, 2) AND pc.Inactivo = 0 LEFT OUTER JOIN
                      dbo.Contacto AS c ON c.Id_Contacto = pc.CodContacto INNER JOIN
                      dbo.InstitucionContacto AS ic ON p.CodInstitucion = ic.CodInstitucion AND ic.FechaFin IS NULL INNER JOIN
                      dbo.Contacto AS c2 ON c2.Id_Contacto = ic.CodContacto
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
         Begin Table = "cp"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "conv"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 114
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 476
               Bottom = 114
               Right = 654
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "est"
            Begin Extent = 
               Top = 6
               Left = 692
               Bottom = 114
               Right = 843
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "i"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Ciudad"
            Begin Extent = 
               Top = 114
               Left = 249
               Bottom = 222
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "departamento"
            Begin Extent = 
               Top = 114
               Left = 455
               Bottom = 207
               Right = 625
            End
            DisplayFlags = 280
            TopColumn ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Convocatorias_Proyectos';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'= 0
         End
         Begin Table = "pc"
            Begin Extent = 
               Top = 114
               Left = 663
               Bottom = 222
               Right = 846
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 210
               Left = 455
               Bottom = 318
               Right = 645
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ic"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c2"
            Begin Extent = 
               Top = 222
               Left = 244
               Bottom = 330
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Convocatorias_Proyectos';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Convocatorias_Proyectos';

