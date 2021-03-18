using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Fonade.Negocio.PlanDeNegocioV2.LiderRegional
{
    public class LiderRegionalBLL
    {
        static string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public static List<LiderRegionalRUDTO> getDatosRegistroUnico()
        {
            List<LiderRegionalRUDTO> list = new List<LiderRegionalRUDTO>();

            using (Datos.RegistroUnicoDataContext db = new Datos.RegistroUnicoDataContext(conexion))
            {
                list = (from r in db.SP_RegistroUnico_Total()
                        select new LiderRegionalRUDTO
                        {
                            Id_ContactoRegistro = r.Id_ContactoRegistro,
                            FechaRegistro = r.FechaRegistro,
                            Nombres = r.Nombres,
                            Apellidos = r.Apellidos,
                            NomTipoIdentificacion = r.NomTipoIdentificacion,
                            Identificacion = r.Identificacion,
                            CiudadExpedicion = r.CiudadExpedicion,
                            DeptoExpedicion = r.DeptoExpedicion,
                            Fecha_Nacimiento = r.Fecha_Nacimiento,
                            CiudadNacimiento = r.CiudadNacimiento,
                            DeptoNacimiento = r.DeptoNacimiento,
                            Genero = r.Genero.ToString(),
                            NomEstadoCivil = r.NomEstadoCivil,
                            NomOcupacion = r.NomOcupacion,
                            Telefono = r.Telefono,
                            Correo = r.Correo,
                            CiudadResidencia = r.CiudadResidencia,
                            DeptoResidencia = r.DeptoResidencia,
                            DireccionDondeReside = r.DireccionDondeReside,
                            Estrato = r.Estrato.ToString(),
                            NomNivelEstudio = r.NomNivelEstudio,
                            ProgramaAcademico = r.ProgramaAcademico,
                            NomInstitucionEducativa = r.NomInstitucionEducativa,
                            CiudadEstudio = r.CiudadEstudio,
                            DeptoEstudio = r.DeptoEstudio,
                            EstadoEstudio = r.EstadoEstudio,
                            FechaINIEstudio = r.FechaINIEstudio.ToShortDateString(),
                            SemestresCursados = r.SemestresCursados.ToString(),
                            FechaFinMaterias = r.FechaFinMaterias.ToShortDateString(),
                            FechaGradoEstudio = r.FechaGradoEstudio.ToShortDateString(),
                            NomTipoAprendiz = r.NomTipoAprendiz,
                            CentroDesaEmpresarial = r.CentroDesaEmpresarial,
                            CiudadCentEmpresarial = r.CiudadCentEmpresarial,
                            DeptoCentEmpresarial = r.DeptoCentEmpresarial,
                            NomServicio = r.NomServicio,
                            FormacionInteres = r.FormacionInteres,
                            CiudEmprendeRural = r.CiudEmprendeRural,
                            dptoEmprendeRural = r.dptoEmprendeRural,
                            NomProyecto = r.NomProyecto,
                            DescripcionProyecto = r.DescripcionProyecto,
                            CiudadFondoEmprender = r.CiudadFondoEmprender,
                            DeptoFondoEmprender = r.DeptoFondoEmprender,
                            NomSubSector = r.NomSubSector,
                            NomSector = r.NomSector,
                            local = r.local,
                            Regional = r.Regional,
                            Nacional = r.Nacional,
                            Internacional = r.Internacional,
                            ProductoServicioOferta = r.ProductoServicioOferta,
                            ProductoEnElMercado = r.ProductoEnElMercado,
                            EmpleadosACargo = r.EmpleadosACargo,
                            CantidadEmpleados = r.CantidadEmpleados.ToString(),
                            EmpresaFortalecimiento = r.EmpresaFortalecimiento,
                            CorreoEmpresa = r.CorreoEmpresa,
                            TelefonoEmpresa = r.TelefonoEmpresa,
                            DireccionEmpresa = r.DireccionEmpresa,
                            CiudadEmpresa = r.CiudadEmpresa,
                            DepartamentoEmpresa = r.DepartamentoEmpresa,
                            NomSubSectorEmpresa = r.NomSubSectorEmpresa,
                            NomSectorEmpresa = r.NomSectorEmpresa,
                            FechaConstitucionEmpresa = r.FechaConstitucionEmpresa.ToShortDateString(),
                            TamaEmpresa = r.TamaEmpresa,
                            localEmpresa = r.localEmpresa,
                            RegionalEmpresa = r.RegionalEmpresa,
                            NacionalEmpresa = r.NacionalEmpresa,
                            InternacionalEmpresa = r.InternacionalEmpresa,
                            ProductoServicioOfertaEmpresa = r.ProductoServicioOfertaEmpresa,
                            DescripcionActividadEmpresa = r.DescripcionActividadEmpresa,
                            ValorVentasAnuales = r.ValorVentasAnuales.ToString(),
                            EmpleadosEmpresa = r.EmpleadosEmpresa.ToString(),
                            EsElPropietario = r.EsElPropietario,
                            CargoQueOcupa = r.CargoQueOcupa
                        }).ToList();

                #region oldConsulta
                //list = (from c in db.RU_ContactoRegistro
                //        join s in db.RU_Servicio on c.Id_RUServicio equals s.Id_RUServicio
                //        join d in db.RU_CentroDesarrolloEmpresarial
                //                on c.Id_RUCentroDesarrollo equals d.Id_RUCentroDesarrollo
                //        join o in db.RU_Ocupacion on c.Id_RUOcupacion equals o.Id_RUOcupacion
                //        orderby c.FechaRegistro descending
                //        select new LiderRegionalRUDTO
                //        {
                //            nombres = c.Nombres,
                //            apellidos = c.Apellidos,
                //            Identificacion = c.Identificacion,
                //            Correo = c.Correo,
                //            Telefono = c.Telefono,
                //            DireccionResidencia = c.DireccionDondeReside,
                //            codCiudadResidencia = c.CiudadDondeReside,
                //            CiudadResidencia = NomCiudad(c.CiudadDondeReside),
                //            DeptoResidencia = NomDepartamento(c.CiudadDondeReside),
                //            codCentroDesarrollo = c.Id_RUCentroDesarrollo,
                //            CentroDesarrollo = d.NomCentro,
                //            codServicio = c.Id_RUServicio,
                //            Servicio = s.NomServicio,
                //            NomProyecto = c.NomProyecto,
                //            fechaRegistro = c.FechaRegistro
                //        }
                //        ).ToList();
                #endregion

            }

            return list;
        }

        public static List<ListItem> getItemCentrosDesarrollo()
        {
            List<ListItem> list = new List<ListItem>();

            using (Datos.RegistroUnicoDataContext db = new Datos.RegistroUnicoDataContext(conexion))
            {
                list = (from d in db.RU_CentroDesarrolloEmpresarial
                        select new ListItem
                        {
                            Value = d.Id_RUCentroDesarrollo.ToString(),
                            Text = d.NomCentro
                        }
                        ).ToList();
            }

            return list;
        }

        private static List<ciudadDTO> ciudadesXDepto(int _codDepartamento)
        {
            List<ciudadDTO> ciudadDTOs = new List<ciudadDTO>();
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                ciudadDTOs = (from d in db.departamento
                              join c in db.Ciudad on d.Id_Departamento equals c.CodDepartamento
                              where d.Id_Departamento == _codDepartamento
                              select new ciudadDTO
                              {
                                  Ciudad = c.NomCiudad,
                                  codCiudad = c.Id_Ciudad
                              }).ToList();
            }


            return ciudadDTOs;
        }

        private static int codDepartamentoXLider(int _codUsuario)
        {
            int codDepartamento = 0;

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                codDepartamento = (from u in db.Contacto
                                   join c in db.Ciudad on u.CodCiudad equals c.Id_Ciudad
                                   join d in db.departamento on c.CodDepartamento equals d.Id_Departamento
                                   where u.Id_Contacto == _codUsuario
                                   select d.Id_Departamento).FirstOrDefault();
            }

            return codDepartamento;
        }

        public static List<ListItem> getItemCentrosDesarrolloXLider(int _codUsuario)
        {
            List<ListItem> list = new List<ListItem>();
            int codDepartamento = 0;
            List<ciudadDTO> codCiudades = new List<ciudadDTO>();

            //buscar el dpto del lider
            codDepartamento = codDepartamentoXLider(_codUsuario);
            codCiudades = ciudadesXDepto(codDepartamento);

            using (Datos.RegistroUnicoDataContext db = new Datos.RegistroUnicoDataContext(conexion))
            {
                foreach (var c in codCiudades)
                {
                    var centro = (from d in db.RU_CentroDesarrolloEmpresarial
                                  where d.CodCiudad == c.codCiudad
                                  select new
                                  {
                                      value = d.Id_RUCentroDesarrollo,
                                      text = d.NomCentro
                                  }).ToList();

                    if (centro != null)
                    {

                        foreach (var cen in centro)
                        {
                            ListItem elemento = new ListItem
                            {
                                Value = cen.value.ToString(),
                                Text = cen.text.ToString()
                            };

                            list.Add(elemento);

                        }
                    }

                }
            }

            return list;
        }

        public static string NomCiudad(int codCiudad)
        {
            string ciudad = "";

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                ciudad = (from c in db.Ciudad
                          where c.Id_Ciudad == codCiudad
                          select c.NomCiudad).FirstOrDefault();
            }

            return ciudad;
        }

        public static string NomDepartamento(int codCiudad)
        {
            string ciudad = "";

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                ciudad = (from c in db.Ciudad
                          join d in db.departamento on c.CodDepartamento equals d.Id_Departamento
                          where c.Id_Ciudad == codCiudad
                          select d.NomDepartamento).FirstOrDefault();
            }

            return ciudad;
        }

        public static List<LiderRegionalRUDTO> getDatosRegistroUnicoFiltrarCentro(int _codCentro)
        {
            List<LiderRegionalRUDTO> list = new List<LiderRegionalRUDTO>();

            using (Datos.RegistroUnicoDataContext db = new Datos.RegistroUnicoDataContext(conexion))
            {
                list = (from r in db.SP_RegistroUnico_Total()
                        where r.Id_RUCentroDesarrollo == _codCentro
                        select new LiderRegionalRUDTO
                        {
                            Id_ContactoRegistro = r.Id_ContactoRegistro,
                            FechaRegistro = r.FechaRegistro,
                            Nombres = r.Nombres,
                            Apellidos = r.Apellidos,
                            NomTipoIdentificacion = r.NomTipoIdentificacion,
                            Identificacion = r.Identificacion,
                            CiudadExpedicion = r.CiudadExpedicion,
                            DeptoExpedicion = r.DeptoExpedicion,
                            Fecha_Nacimiento = r.Fecha_Nacimiento,
                            CiudadNacimiento = r.CiudadNacimiento,
                            DeptoNacimiento = r.DeptoNacimiento,
                            Genero = r.Genero.ToString(),
                            NomEstadoCivil = r.NomEstadoCivil,
                            NomOcupacion = r.NomOcupacion,
                            Telefono = r.Telefono,
                            Correo = r.Correo,
                            CiudadResidencia = r.CiudadResidencia,
                            DeptoResidencia = r.DeptoResidencia,
                            DireccionDondeReside = r.DireccionDondeReside,
                            Estrato = r.Estrato.ToString(),
                            NomNivelEstudio = r.NomNivelEstudio,
                            ProgramaAcademico = r.ProgramaAcademico,
                            NomInstitucionEducativa = r.NomInstitucionEducativa,
                            CiudadEstudio = r.CiudadEstudio,
                            DeptoEstudio = r.DeptoEstudio,
                            EstadoEstudio = r.EstadoEstudio,
                            FechaINIEstudio = r.FechaINIEstudio.ToShortDateString(),
                            SemestresCursados = r.SemestresCursados.ToString(),
                            FechaFinMaterias = r.FechaFinMaterias.ToShortDateString(),
                            FechaGradoEstudio = r.FechaGradoEstudio.ToShortDateString(),
                            NomTipoAprendiz = r.NomTipoAprendiz,
                            CentroDesaEmpresarial = r.CentroDesaEmpresarial,
                            CiudadCentEmpresarial = r.CiudadCentEmpresarial,
                            DeptoCentEmpresarial = r.DeptoCentEmpresarial,
                            NomServicio = r.NomServicio,
                            FormacionInteres = r.FormacionInteres,
                            CiudEmprendeRural = r.CiudEmprendeRural,
                            dptoEmprendeRural = r.dptoEmprendeRural,
                            NomProyecto = r.NomProyecto,
                            DescripcionProyecto = r.DescripcionProyecto,
                            CiudadFondoEmprender = r.CiudadFondoEmprender,
                            DeptoFondoEmprender = r.DeptoFondoEmprender,
                            NomSubSector = r.NomSubSector,
                            NomSector = r.NomSector,
                            local = r.local,
                            Regional = r.Regional,
                            Nacional = r.Nacional,
                            Internacional = r.Internacional,
                            ProductoServicioOferta = r.ProductoServicioOferta,
                            ProductoEnElMercado = r.ProductoEnElMercado,
                            EmpleadosACargo = r.EmpleadosACargo,
                            CantidadEmpleados = r.CantidadEmpleados.ToString(),
                            EmpresaFortalecimiento = r.EmpresaFortalecimiento,
                            CorreoEmpresa = r.CorreoEmpresa,
                            TelefonoEmpresa = r.TelefonoEmpresa,
                            DireccionEmpresa = r.DireccionEmpresa,
                            CiudadEmpresa = r.CiudadEmpresa,
                            DepartamentoEmpresa = r.DepartamentoEmpresa,
                            NomSubSectorEmpresa = r.NomSubSectorEmpresa,
                            NomSectorEmpresa = r.NomSectorEmpresa,
                            FechaConstitucionEmpresa = r.FechaConstitucionEmpresa.ToShortDateString(),
                            TamaEmpresa = r.TamaEmpresa,
                            localEmpresa = r.localEmpresa,
                            RegionalEmpresa = r.RegionalEmpresa,
                            NacionalEmpresa = r.NacionalEmpresa,
                            InternacionalEmpresa = r.InternacionalEmpresa,
                            ProductoServicioOfertaEmpresa = r.ProductoServicioOfertaEmpresa,
                            DescripcionActividadEmpresa = r.DescripcionActividadEmpresa,
                            ValorVentasAnuales = r.ValorVentasAnuales.ToString(),
                            EmpleadosEmpresa = r.EmpleadosEmpresa.ToString(),
                            EsElPropietario = r.EsElPropietario,
                            CargoQueOcupa = r.CargoQueOcupa
                        }).ToList();

                #region oldSQL
                //list = (from c in db.RU_ContactoRegistro
                //        join s in db.RU_Servicio on c.Id_RUServicio equals s.Id_RUServicio
                //        join d in db.RU_CentroDesarrolloEmpresarial
                //                on c.Id_RUCentroDesarrollo equals d.Id_RUCentroDesarrollo
                //        join o in db.RU_Ocupacion on c.Id_RUOcupacion equals o.Id_RUOcupacion
                //        where d.Id_RUCentroDesarrollo == _codCentro
                //        orderby c.FechaRegistro descending
                //        select new LiderRegionalRUDTO
                //        {
                //            nombres = c.Nombres,
                //            apellidos = c.Apellidos,
                //            Identificacion = c.Identificacion,
                //            Correo = c.Correo,
                //            Telefono = c.Telefono,
                //            DireccionResidencia = c.DireccionDondeReside,
                //            codCiudadResidencia = c.CiudadDondeReside,
                //            CiudadResidencia = NomCiudad(c.CiudadDondeReside),
                //            DeptoResidencia = NomDepartamento(c.CiudadDondeReside),
                //            codCentroDesarrollo = c.Id_RUCentroDesarrollo,
                //            CentroDesarrollo = d.NomCentro,
                //            codServicio = c.Id_RUServicio,
                //            Servicio = s.NomServicio,
                //            NomProyecto = c.NomProyecto,
                //            fechaRegistro = c.FechaRegistro
                //        }
                //        ).ToList();
                #endregion
            }

            return list;
        }

        public static List<LiderRegionalRUDTO> getDatosRegistroUnicoXLiderRegional(int _codusuario)
        {
            List<LiderRegionalRUDTO> list = new List<LiderRegionalRUDTO>();

            int codDepartamento = 0;
            List<ciudadDTO> codCiudades = new List<ciudadDTO>();

            //buscar el dpto del lider
            codDepartamento = codDepartamentoXLider(_codusuario);
            codCiudades = ciudadesXDepto(codDepartamento);

            using (Datos.RegistroUnicoDataContext db = new Datos.RegistroUnicoDataContext(conexion))
            {
                foreach (var ciu in codCiudades)
                {
                    List<LiderRegionalRUDTO> listCiudad = new List<LiderRegionalRUDTO>();

                    listCiudad = (from r in db.SP_RegistroUnico_Total()
                                         where r.Id_Ciudad2 == ciu.codCiudad
                                  select new LiderRegionalRUDTO
                                  {
                                      Id_ContactoRegistro = r.Id_ContactoRegistro,
                                      FechaRegistro = r.FechaRegistro,
                                      Nombres = r.Nombres,
                                      Apellidos = r.Apellidos,
                                      NomTipoIdentificacion = r.NomTipoIdentificacion,
                                      Identificacion = r.Identificacion,
                                      CiudadExpedicion = r.CiudadExpedicion,
                                      DeptoExpedicion = r.DeptoExpedicion,
                                      Fecha_Nacimiento = r.Fecha_Nacimiento,
                                      CiudadNacimiento = r.CiudadNacimiento,
                                      DeptoNacimiento = r.DeptoNacimiento,
                                      Genero = r.Genero.ToString(),
                                      NomEstadoCivil = r.NomEstadoCivil,
                                      NomOcupacion = r.NomOcupacion,
                                      Telefono = r.Telefono,
                                      Correo = r.Correo,
                                      CiudadResidencia = r.CiudadResidencia,
                                      DeptoResidencia = r.DeptoResidencia,
                                      DireccionDondeReside = r.DireccionDondeReside,
                                      Estrato = r.Estrato.ToString(),
                                      NomNivelEstudio = r.NomNivelEstudio,
                                      ProgramaAcademico = r.ProgramaAcademico,
                                      NomInstitucionEducativa = r.NomInstitucionEducativa,
                                      CiudadEstudio = r.CiudadEstudio,
                                      DeptoEstudio = r.DeptoEstudio,
                                      EstadoEstudio = r.EstadoEstudio,
                                      FechaINIEstudio = r.FechaINIEstudio.ToShortDateString(),
                                      SemestresCursados = r.SemestresCursados.ToString(),
                                      FechaFinMaterias = r.FechaFinMaterias.ToShortDateString(),
                                      FechaGradoEstudio = r.FechaGradoEstudio.ToShortDateString(),
                                      NomTipoAprendiz = r.NomTipoAprendiz,
                                      CentroDesaEmpresarial = r.CentroDesaEmpresarial,
                                      CiudadCentEmpresarial = r.CiudadCentEmpresarial,
                                      DeptoCentEmpresarial = r.DeptoCentEmpresarial,
                                      NomServicio = r.NomServicio,
                                      FormacionInteres = r.FormacionInteres,
                                      CiudEmprendeRural = r.CiudEmprendeRural,
                                      dptoEmprendeRural = r.dptoEmprendeRural,
                                      NomProyecto = r.NomProyecto,
                                      DescripcionProyecto = r.DescripcionProyecto,
                                      CiudadFondoEmprender = r.CiudadFondoEmprender,
                                      DeptoFondoEmprender = r.DeptoFondoEmprender,
                                      NomSubSector = r.NomSubSector,
                                      NomSector = r.NomSector,
                                      local = r.local,
                                      Regional = r.Regional,
                                      Nacional = r.Nacional,
                                      Internacional = r.Internacional,
                                      ProductoServicioOferta = r.ProductoServicioOferta,
                                      ProductoEnElMercado = r.ProductoEnElMercado,
                                      EmpleadosACargo = r.EmpleadosACargo,
                                      CantidadEmpleados = r.CantidadEmpleados.ToString(),
                                      EmpresaFortalecimiento = r.EmpresaFortalecimiento,
                                      CorreoEmpresa = r.CorreoEmpresa,
                                      TelefonoEmpresa = r.TelefonoEmpresa,
                                      DireccionEmpresa = r.DireccionEmpresa,
                                      CiudadEmpresa = r.CiudadEmpresa,
                                      DepartamentoEmpresa = r.DepartamentoEmpresa,
                                      NomSubSectorEmpresa = r.NomSubSectorEmpresa,
                                      NomSectorEmpresa = r.NomSectorEmpresa,
                                      FechaConstitucionEmpresa = r.FechaConstitucionEmpresa.ToShortDateString(),
                                      TamaEmpresa = r.TamaEmpresa,
                                      localEmpresa = r.localEmpresa,
                                      RegionalEmpresa = r.RegionalEmpresa,
                                      NacionalEmpresa = r.NacionalEmpresa,
                                      InternacionalEmpresa = r.InternacionalEmpresa,
                                      ProductoServicioOfertaEmpresa = r.ProductoServicioOfertaEmpresa,
                                      DescripcionActividadEmpresa = r.DescripcionActividadEmpresa,
                                      ValorVentasAnuales = r.ValorVentasAnuales.ToString(),
                                      EmpleadosEmpresa = r.EmpleadosEmpresa.ToString(),
                                      EsElPropietario = r.EsElPropietario,
                                      CargoQueOcupa = r.CargoQueOcupa
                                  }).ToList();

                    #region oldSQL
                    //     (from c in db.RU_ContactoRegistro
                    //      join s in db.RU_Servicio on c.Id_RUServicio equals s.Id_RUServicio
                    //      join d in db.RU_CentroDesarrolloEmpresarial
                    //              on c.Id_RUCentroDesarrollo equals d.Id_RUCentroDesarrollo
                    //      join o in db.RU_Ocupacion on c.Id_RUOcupacion equals o.Id_RUOcupacion
                    //      where d.CodCiudad == ciu.codCiudad
                    //      orderby c.FechaRegistro descending
                    //      select new LiderRegionalRUDTO
                    //      {
                    //          nombres = c.Nombres,
                    //          apellidos = c.Apellidos,
                    //          Identificacion = c.Identificacion,
                    //          Correo = c.Correo,
                    //          Telefono = c.Telefono,
                    //          DireccionResidencia = c.DireccionDondeReside,
                    //          codCiudadResidencia = c.CiudadDondeReside,
                    //          CiudadResidencia = NomCiudad(c.CiudadDondeReside),
                    //          DeptoResidencia = NomDepartamento(c.CiudadDondeReside),
                    //          codCentroDesarrollo = c.Id_RUCentroDesarrollo,
                    //          CentroDesarrollo = d.NomCentro,
                    //          codServicio = c.Id_RUServicio,
                    //          Servicio = s.NomServicio,
                    //          NomProyecto = c.NomProyecto,
                    //          fechaRegistro = c.FechaRegistro
                    //      }
                    //).ToList();
                    #endregion

                    if (listCiudad != null)
                    {
                        list.AddRange(listCiudad);
                    }

                }
            }

            return list;
        }
    }

    public class ciudadDTO
    {
        public int codCiudad { get; set; }
        public string Ciudad { get; set; }
    }


    public class LiderRegionalRUDTO
    {
        public int Id_ContactoRegistro { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NomTipoIdentificacion { get; set; }
        public double Identificacion { get; set; }
        public string CiudadExpedicion { get; set; }
        public string DeptoExpedicion { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string CiudadNacimiento { get; set; }
        public string DeptoNacimiento { get; set; }
        public string Genero { get; set; }
        public string NomEstadoCivil { get; set; }
        public string NomOcupacion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string CiudadResidencia { get; set; }
        public string DeptoResidencia { get; set; }
        public string DireccionDondeReside { get; set; }
        public string Estrato { get; set; }
        public string NomNivelEstudio { get; set; }
        public string ProgramaAcademico { get; set; }
        public string NomInstitucionEducativa { get; set; }
        public string CiudadEstudio { get; set; }
        public string DeptoEstudio { get; set; }
        public string EstadoEstudio { get; set; }
        public string FechaINIEstudio { get; set; }
        public string FechaFinMaterias { get; set; }
        public string FechaGradoEstudio { get; set; }
        public string SemestresCursados { get; set; }
        public string NomTipoAprendiz { get; set; }
        public string CentroDesaEmpresarial { get; set; }
        public string CiudadCentEmpresarial { get; set; }
        public string DeptoCentEmpresarial { get; set; }
        public string NomServicio { get; set; }
        public string FormacionInteres { get; set; }
        public string CiudEmprendeRural { get; set; }
        public string dptoEmprendeRural { get; set; }
        public string NomProyecto { get; set; }
        public string local { get; set; }
        public string Regional { get; set; }
        public string Nacional { get; set; }
        public string Internacional { get; set; }
        public string DescripcionProyecto { get; set; }
        public string ProductoServicioOferta { get; set; }
        public string ProductoEnElMercado { get; set; }
        public string EmpleadosACargo { get; set; }
        public string CantidadEmpleados { get; set; }
        public string NomSubSector { get; set; }
        public string NomSector { get; set; }
        public string CiudadFondoEmprender { get; set; }
        public string DeptoFondoEmprender { get; set; }
        public string EmpresaFortalecimiento { get; set; }
        public string CorreoEmpresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public string FechaConstitucionEmpresa { get; set; }
        public string TamaEmpresa { get; set; }
        public string localEmpresa { get; set; }
        public string RegionalEmpresa { get; set; }
        public string NacionalEmpresa { get; set; }
        public string InternacionalEmpresa { get; set; }
        public string ProductoServicioOfertaEmpresa { get; set; }
        public string DescripcionActividadEmpresa { get; set; }
        public string ValorVentasAnuales { get; set; }
        public string EmpleadosEmpresa { get; set; }
        public string EsElPropietario { get; set; }
        public string CargoQueOcupa { get; set; }
        public string CiudadEmpresa { get; set; }
        public string DepartamentoEmpresa { get; set; }
        public string NomSubSectorEmpresa { get; set; }
        public string NomSectorEmpresa { get; set; }      
        public DateTime FechaRegistro { get; set; }
    }
}
