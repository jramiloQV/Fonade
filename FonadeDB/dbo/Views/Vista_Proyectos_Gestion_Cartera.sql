CREATE VIEW dbo.Vista_Proyectos_Gestion_Cartera
AS
SELECT     p.Id_Proyecto, ce.NumeroContrato, em.razonsocial, p.NomProyecto, em.Nit, em.DomicilioEmpresa, em.Telefono, em.Email, ciu.NomCiudad, 
                      dep.NomDepartamento, MAX(con.Id_Convocatoria) AS 'Id Convocatoria', con.NomConvocatoria, ce.FechaDeInicioContrato, p.Sumario, 
                      ce.ValorInicialEnPesos AS 'Valor Contrato', c.Nombres + ' ' + c.Apellidos AS Beneficiario, c.Identificacion, c.Email AS 'Email Beneficiario', 
                      c.Direccion AS 'Direccion Beneficiario', c.Telefono AS 'Telefono Beneficiario', ciu2.NomCiudad AS 'Ciudad Beneficiario', 
                      dep2.NomDepartamento AS 'Departamento Beneficiario', i.NomUnidad + ' (' + i.NomInstitucion + ')' AS Institucion, 
                      c2.Nombres + ' ' + c2.Apellidos AS 'Lider Unidad Emprendimiento', Desembolso.CantidadDinero
FROM         dbo.Proyecto AS p INNER JOIN
                      dbo.Empresa AS em ON p.Id_Proyecto = em.codproyecto INNER JOIN
                      dbo.ContratoEmpresa AS ce ON em.id_empresa = ce.CodEmpresa INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON p.Id_Proyecto = cp.CodProyecto AND cp.Viable = 1 INNER JOIN
                      dbo.Convocatoria AS con ON con.Id_Convocatoria = cp.CodConvocatoria INNER JOIN
                      dbo.ProyectoContacto AS pc ON p.Id_Proyecto = pc.CodProyecto AND pc.CodRol = 3 AND pc.Inactivo = 0 INNER JOIN
                      dbo.Contacto AS c ON c.Id_Contacto = pc.CodContacto INNER JOIN
                      dbo.Ciudad AS ciu ON ciu.Id_Ciudad = em.CodCiudad INNER JOIN
                      dbo.Ciudad AS ciu2 ON ciu2.Id_Ciudad = c.CodCiudad LEFT OUTER JOIN
                      dbo.departamento AS dep ON dep.Id_Departamento = ciu.CodDepartamento LEFT OUTER JOIN
                      dbo.departamento AS dep2 ON dep2.Id_Departamento = ciu2.CodDepartamento INNER JOIN
                      dbo.Institucion AS i ON i.Id_Institucion = p.CodInstitucion INNER JOIN
                      dbo.InstitucionContacto AS ic ON i.Id_Institucion = ic.CodInstitucion AND ic.FechaFin IS NULL INNER JOIN
                      dbo.Contacto AS c2 ON c2.Id_Contacto = ic.CodContacto LEFT OUTER JOIN
                          (SELECT     SUM(pa.CantidadDinero) AS CantidadDinero, p1.Id_Proyecto
                            FROM          dbo.PagoActividad AS pa INNER JOIN
                                                   dbo.Proyecto AS p1 ON pa.CodProyecto = p1.Id_Proyecto
                            WHERE      (pa.Estado NOT IN (0, 5))
                            GROUP BY p1.Id_Proyecto) AS Desembolso ON p.Id_Proyecto = Desembolso.Id_Proyecto
GROUP BY p.Id_Proyecto, ce.NumeroContrato, em.razonsocial, p.NomProyecto, em.Nit, em.DomicilioEmpresa, em.Telefono, em.Email, ciu.NomCiudad, 
                      dep.NomDepartamento, con.NomConvocatoria, ce.FechaDeInicioContrato, p.Sumario, ce.ValorInicialEnPesos, c.Nombres, c.Apellidos, c.Identificacion, 
                      c.Email, c.Direccion, c.Telefono, ciu2.NomCiudad, dep2.NomDepartamento, i.NomInstitucion, i.NomUnidad, c2.Nombres, c2.Apellidos, 
                      Desembolso.CantidadDinero
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
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "em"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 114
               Right = 462
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ce"
            Begin Extent = 
               Top = 6
               Left = 500
               Bottom = 114
               Right = 729
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cp"
            Begin Extent = 
               Top = 6
               Left = 767
               Bottom = 114
               Right = 980
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "con"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pc"
            Begin Extent = 
               Top = 114
               Left = 257
               Bottom = 222
               Right = 456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 494
               Bottom = 222
               Right = 700
            End
            DisplayFlags = 280
            TopColumn = 0
         En', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Proyectos_Gestion_Cartera';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'd
         Begin Table = "ciu"
            Begin Extent = 
               Top = 114
               Left = 738
               Bottom = 222
               Right = 922
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ciu2"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 222
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "dep"
            Begin Extent = 
               Top = 222
               Left = 260
               Bottom = 315
               Right = 446
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "dep2"
            Begin Extent = 
               Top = 222
               Left = 484
               Bottom = 315
               Right = 670
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "i"
            Begin Extent = 
               Top = 222
               Left = 708
               Bottom = 330
               Right = 897
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ic"
            Begin Extent = 
               Top = 318
               Left = 260
               Bottom = 426
               Right = 444
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c2"
            Begin Extent = 
               Top = 318
               Left = 482
               Bottom = 426
               Right = 688
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Desembolso"
            Begin Extent = 
               Top = 6
               Left = 1018
               Bottom = 84
               Right = 1188
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Proyectos_Gestion_Cartera';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Proyectos_Gestion_Cartera';

