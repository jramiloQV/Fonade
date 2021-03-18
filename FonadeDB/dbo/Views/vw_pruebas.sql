CREATE VIEW dbo.vw_pruebas
AS
SELECT        Id_Proyecto, Empresas, Ciudad, Sector, COUNT(Cargos) AS Cargos, CodDepartamento, NomDepartamento
FROM            (SELECT DISTINCT 
                                                    p.Id_Proyecto, em.Nit AS Empresas, ci.NomCiudad AS Ciudad, sec.NomSector AS Sector, pgp.Id_Cargo AS Cargos, 
                                                    d.Id_Departamento AS CodDepartamento, d.NomDepartamento
                          FROM            dbo.Proyecto AS p INNER JOIN
                                                    dbo.Estado AS e ON p.CodEstado = e.Id_Estado INNER JOIN
                                                    dbo.ConvocatoriaProyecto AS cp ON p.Id_Proyecto = cp.CodProyecto INNER JOIN
                                                    dbo.SubSector AS ss ON p.CodSubSector = ss.Id_SubSector INNER JOIN
                                                    dbo.Sector AS sec ON ss.CodSector = sec.Id_Sector INNER JOIN
                                                    dbo.Convocatoria AS c ON cp.CodConvocatoria = c.Id_Convocatoria INNER JOIN
                                                        (SELECT        p.Id_Proyecto AS proyecto, MAX(cp.Fecha) AS fecha
                                                          FROM            dbo.ConvocatoriaProyecto AS cp INNER JOIN
                                                                                    dbo.Proyecto AS p ON cp.CodProyecto = p.Id_Proyecto
                                                          GROUP BY p.Id_Proyecto) AS fecha ON p.Id_Proyecto = fecha.proyecto AND cp.Fecha = fecha.fecha INNER JOIN
                                                    dbo.Empresa AS em ON p.Id_Proyecto = em.codproyecto INNER JOIN
                                                    dbo.Ciudad AS ci ON p.CodCiudad = ci.Id_Ciudad INNER JOIN
                                                    dbo.departamento AS d ON ci.CodDepartamento = d.Id_Departamento INNER JOIN
                                                    dbo.ProyectoGastosPersonal AS pgp ON p.Id_Proyecto = pgp.CodProyecto CROSS JOIN
                                                    dbo.ProyectoAporte AS pa
                          WHERE        (cp.Viable = 1) AND (p.CodEstado NOT IN (4, 11, 10))) AS val1
GROUP BY Id_Proyecto, Empresas, Ciudad, Sector, CodDepartamento, NomDepartamento
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
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[49] 3) )"
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
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "val1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 246
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
      PaneHidden = 
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_pruebas';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_pruebas';

