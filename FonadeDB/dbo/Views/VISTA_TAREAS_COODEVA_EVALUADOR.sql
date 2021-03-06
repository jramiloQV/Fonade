CREATE VIEW dbo.VISTA_TAREAS_COODEVA_EVALUADOR
AS
SELECT     tu.Id_TareaUsuario, p.Id_Proyecto, p.NomProyecto, tu.NomTareaUsuario, tu.Descripcion, ISNULL(c.Nombres, '') + ' ' + ISNULL(c.Apellidos, '') 
                      AS Agendado_A, ISNULL(c1.Nombres, '') + ' ' + ISNULL(c1.Apellidos, '') AS Agendado_Por, tur.Fecha, tur.Respuesta, tur.FechaCierre, 
                      cp.CodConvocatoria
FROM         dbo.TareaUsuario AS tu INNER JOIN
                      dbo.TareaUsuarioRepeticion AS tur ON tu.Id_TareaUsuario = tur.CodTareaUsuario INNER JOIN
                      dbo.Contacto AS c ON tu.CodContacto = c.Id_Contacto INNER JOIN
                      dbo.Contacto AS c1 ON tu.CodContactoAgendo = c1.Id_Contacto INNER JOIN
                      dbo.Proyecto AS p ON tu.CodProyecto = p.Id_Proyecto INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON tu.CodProyecto = cp.CodProyecto
WHERE     (tu.CodContactoAgendo IN
                          (SELECT     CodContacto
                            FROM          dbo.GrupoContacto
                            WHERE      (CodGrupo = 10))) AND (tu.CodContacto IN
                          (SELECT     CodContacto
                            FROM          dbo.GrupoContacto AS GrupoContacto_1
                            WHERE      (CodGrupo = 11))) AND (YEAR(tur.Fecha) = 2010)
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
         Begin Table = "tu"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tur"
            Begin Extent = 
               Top = 6
               Left = 268
               Bottom = 114
               Right = 478
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c1"
            Begin Extent = 
               Top = 114
               Left = 259
               Bottom = 222
               Right = 442
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp"
            Begin Extent = 
               Top = 222
               Left = 254
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
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Widt', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_TAREAS_COODEVA_EVALUADOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'h = 1500
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_TAREAS_COODEVA_EVALUADOR';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VISTA_TAREAS_COODEVA_EVALUADOR';

