CREATE VIEW dbo.Vista_Formacion_Academica
AS
SELECT     p.Id_Proyecto, p.NomProyecto, cp.CodConvocatoria, dbo.Contacto.Nombres, dbo.Contacto.Apellidos, dbo.Contacto.Identificacion, dbo.Contacto.Email, 
                      r.Nombre, ne.NomNivelEstudio, ce.TituloObtenido, ce.Institucion, ce.FechaInicio, ce.FechaFinMaterias, ce.FechaGrado, ce.Finalizado, ce.AnoTitulo, 
                      ce.SemestresCursados
FROM         dbo.ProyectoContacto AS pc INNER JOIN
                      dbo.Proyecto AS p ON pc.CodProyecto = p.Id_Proyecto INNER JOIN
                      dbo.Contacto LEFT OUTER JOIN
                      dbo.ContactoEstudio AS ce ON dbo.Contacto.Id_Contacto = ce.CodContacto ON pc.CodContacto = dbo.Contacto.Id_Contacto INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON p.Id_Proyecto = cp.CodProyecto INNER JOIN
                      dbo.Rol AS r ON pc.CodRol = r.Id_Rol INNER JOIN
                      dbo.NivelEstudio AS ne ON ce.CodNivelEstudio = ne.Id_NivelEstudio
WHERE     (pc.Inactivo = 0) AND (pc.CodRol IN (1, 2, 3))
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
         Begin Table = "pc"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 259
               Bottom = 114
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Contacto"
            Begin Extent = 
               Top = 6
               Left = 475
               Bottom = 114
               Right = 665
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ce"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 234
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp"
            Begin Extent = 
               Top = 114
               Left = 272
               Bottom = 222
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 114
               Left = 507
               Bottom = 222
               Right = 658
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ne"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
     ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Formacion_Academica';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'    End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Formacion_Academica';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Formacion_Academica';

