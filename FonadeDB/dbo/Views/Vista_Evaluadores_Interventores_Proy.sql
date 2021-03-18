CREATE VIEW dbo.Vista_Evaluadores_Interventores_Proy
AS
SELECT     TOP (100) PERCENT con.Id_Convocatoria, con.NomConvocatoria, con.FechaInicio, con.FechaFin, dbo.Proyecto.Id_Proyecto, 
                      dbo.Proyecto.NomProyecto, c.Nombres + ' ' + c.Apellidos AS evaluador, ti.NomTipoIdentificacion, c.Identificacion, pc.CodConvocatoria, e.razonsocial, 
                      e.DomicilioEmpresa, e.Telefono, e.Nit, c2.Nombres + ' ' + c2.Apellidos AS interventor, ti2.NomTipoIdentificacion AS Expr1, c2.Identificacion AS Expr2, 
                      cp.Justificacion
FROM         dbo.Proyecto INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON dbo.Proyecto.Id_Proyecto = cp.CodProyecto AND cp.Viable = 1 INNER JOIN
                      dbo.ProyectoContacto AS pc ON dbo.Proyecto.Id_Proyecto = pc.CodProyecto AND pc.CodRol = 4 INNER JOIN
                          (SELECT     MAX(con.Id_Convocatoria) AS idconvocatoria, Proyecto_1.Id_Proyecto AS idproyecto
                            FROM          dbo.Proyecto AS Proyecto_1 INNER JOIN
                                                   dbo.ConvocatoriaProyecto AS cp ON Proyecto_1.Id_Proyecto = cp.CodProyecto AND cp.Viable = 1 INNER JOIN
                                                   dbo.ProyectoContacto AS pc ON Proyecto_1.Id_Proyecto = pc.CodProyecto AND pc.CodRol = 4 INNER JOIN
                                                   dbo.Convocatoria AS con ON con.Id_Convocatoria = cp.CodConvocatoria INNER JOIN
                                                   dbo.Contacto AS c ON c.Id_Contacto = pc.CodContacto INNER JOIN
                                                   dbo.TipoIdentificacion AS ti ON c.CodTipoIdentificacion = ti.Id_TipoIdentificacion INNER JOIN
                                                   dbo.Empresa AS e ON Proyecto_1.Id_Proyecto = e.codproyecto LEFT OUTER JOIN
                                                   dbo.EmpresaInterventor AS ei ON ei.CodEmpresa = e.id_empresa AND ei.Inactivo = 0 LEFT OUTER JOIN
                                                   dbo.Contacto AS c2 ON c2.Id_Contacto = ei.CodContacto
                            GROUP BY Proyecto_1.Id_Proyecto) AS t ON t.idproyecto = dbo.Proyecto.Id_Proyecto AND t.idconvocatoria = pc.CodConvocatoria INNER JOIN
                      dbo.Convocatoria AS con ON con.Id_Convocatoria = cp.CodConvocatoria AND pc.CodConvocatoria = con.Id_Convocatoria INNER JOIN
                      dbo.Contacto AS c ON c.Id_Contacto = pc.CodContacto INNER JOIN
                      dbo.TipoIdentificacion AS ti ON c.CodTipoIdentificacion = ti.Id_TipoIdentificacion INNER JOIN
                      dbo.Empresa AS e ON dbo.Proyecto.Id_Proyecto = e.codproyecto LEFT OUTER JOIN
                      dbo.EmpresaInterventor AS ei ON ei.CodEmpresa = e.id_empresa AND ei.Inactivo = 0 LEFT OUTER JOIN
                      dbo.Contacto AS c2 ON c2.Id_Contacto = ei.CodContacto LEFT OUTER JOIN
                      dbo.TipoIdentificacion AS ti2 ON c2.CodTipoIdentificacion = ti2.Id_TipoIdentificacion
ORDER BY dbo.Proyecto.Id_Proyecto
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
         Begin Table = "Proyecto"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp"
            Begin Extent = 
               Top = 6
               Left = 254
               Bottom = 114
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pc"
            Begin Extent = 
               Top = 6
               Left = 489
               Bottom = 114
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t"
            Begin Extent = 
               Top = 6
               Left = 710
               Bottom = 84
               Right = 861
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "con"
            Begin Extent = 
               Top = 84
               Left = 710
               Bottom = 192
               Right = 875
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ti"
            Begin Extent = 
               Top = 114
               Left = 266
               Bottom = 222
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Evaluadores_Interventores_Proy';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  End
         Begin Table = "e"
            Begin Extent = 
               Top = 114
               Left = 489
               Bottom = 222
               Right = 665
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ei"
            Begin Extent = 
               Top = 192
               Left = 703
               Bottom = 300
               Right = 896
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c2"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ti2"
            Begin Extent = 
               Top = 222
               Left = 266
               Bottom = 330
               Right = 451
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Evaluadores_Interventores_Proy';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Evaluadores_Interventores_Proy';

