CREATE VIEW dbo.EmpleosGenerados
AS
SELECT     t.Id_Proyecto AS CodProyecto, cpro.CodConvocatoria, ISNULL(SUM(t.c0), 0) AS Total, ISNULL(SUM(t.c1), 0) AS Generado, SUM(t.c2) AS Joven, SUM(t.c3) 
                      AS Desplazado, SUM(t.c4) AS Madre, SUM(t.c5) AS Minoria, SUM(t.c6) AS Recluido, SUM(t.c7) AS Desmovilizado, SUM(t.c8) AS Discapacitado, SUM(t.c9)
                       AS Desvinculado
FROM         (SELECT     P.Id_Proyecto, COUNT(DISTINCT GP.Id_Cargo) AS c0, SUM(CASE WHEN GeneradoprimerAno <> 0 THEN 1 ELSE 0 END) AS c1, 
                                              SUM(CASE WHEN Joven = 1 THEN 1 ELSE 0 END) AS c2, SUM(CASE WHEN Desplazado = 1 THEN 1 ELSE 0 END) AS c3, 
                                              SUM(CASE WHEN Madre = 1 THEN 1 ELSE 0 END) AS c4, SUM(CASE WHEN Minoria = 1 THEN 1 ELSE 0 END) AS c5, 
                                              SUM(CASE WHEN Recluido = 1 THEN 1 ELSE 0 END) AS c6, SUM(CASE WHEN Desmovilizado = 1 THEN 1 ELSE 0 END) AS c7, 
                                              SUM(CASE WHEN Discapacitado = 1 THEN 1 ELSE 0 END) AS c8, SUM(CASE WHEN Desvinculado = 1 THEN 1 ELSE 0 END) AS c9
                       FROM          dbo.ProyectoEmpleoCargo AS PE RIGHT OUTER JOIN
                                              dbo.ProyectoGastosPersonal AS GP ON PE.CodCargo = GP.Id_Cargo RIGHT OUTER JOIN
                                              dbo.Proyecto AS P ON P.Id_Proyecto = GP.CodProyecto
                       GROUP BY P.Id_Proyecto
                       UNION ALL
                       SELECT     p.Id_Proyecto, COUNT(DISTINCT PI.Id_Insumo) AS c0, SUM(CASE WHEN GeneradoprimerAno <> 0 THEN 1 ELSE 0 END) AS c1, 
                                             SUM(CASE WHEN Joven = 1 THEN 1 ELSE 0 END) AS Expr1, SUM(CASE WHEN Desplazado = 1 THEN 1 ELSE 0 END) AS Expr2, 
                                             SUM(CASE WHEN Madre = 1 THEN 1 ELSE 0 END) AS Expr3, SUM(CASE WHEN Minoria = 1 THEN 1 ELSE 0 END) AS Expr4, 
                                             SUM(CASE WHEN Recluido = 1 THEN 1 ELSE 0 END) AS Expr5, SUM(CASE WHEN Desmovilizado = 1 THEN 1 ELSE 0 END) AS Expr6, 
                                             SUM(CASE WHEN Discapacitado = 1 THEN 1 ELSE 0 END) AS Expr7, SUM(CASE WHEN Desvinculado = 1 THEN 1 ELSE 0 END) 
                                             AS Expr8
                       FROM         dbo.ProyectoEmpleoManoObra AS PE RIGHT OUTER JOIN
                                             dbo.ProyectoInsumo AS PI ON PE.CodManoObra = PI.Id_Insumo RIGHT OUTER JOIN
                                             dbo.Proyecto AS p ON PI.CodProyecto = p.Id_Proyecto
                       WHERE     (PI.codTipoInsumo = 2)
                       GROUP BY p.Id_Proyecto) AS t INNER JOIN
                      dbo.ConvocatoriaProyecto AS cpro ON t.Id_Proyecto = cpro.CodProyecto
GROUP BY t.Id_Proyecto, t.Id_Proyecto, cpro.CodConvocatoria
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
         Begin Table = "t"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cpro"
            Begin Extent = 
               Top = 6
               Left = 243
               Bottom = 114
               Right = 456
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'EmpleosGenerados';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'EmpleosGenerados';

