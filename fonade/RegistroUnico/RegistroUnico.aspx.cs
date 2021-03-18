using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Configuration;
using System.Net.Mail;
using Fonade.Clases;

namespace Fonade.RegistroUnico
{
    public partial class RegistroUnico : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCheckBox(cblMercadoOfer, GetListadoMercadoOferta());
                CargarCheckBox(cblMercadoOferForta, GetListadoMercadoOferta());

                cargarcombobox(cmbDepartamentoExpedicion, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbDepartamentoNacimiento, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbDepartamentoReside, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbNivelEstudio, llenarNivelEstudio().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbDepartamentoEmpresarial, llenarDepartamento(true).Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbFormacion, llenarformacion().Cast<Object>().ToList(), "NombreFormacion", "Id_Formacion");
                cargarcombobox(cmbDepartamentoDesarrollarProyecto, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbSector, llenarSectores().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbUbicacionEmpresaD, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
                cargarcombobox(cmbSectorEmpresa, llenarSectores().Cast<Object>().ToList(), "Nombre", "Id");


            }

            if (IsPostBack)
            {
                if (Request["__EVENTTARGET"].ToString().Equals("GuardarDatos"))
                {
                    InsertarRegistro(sender, e);


                }
            }

        }



        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private void CargarCheckBox(CheckBoxList cbl, List<MercadoOfertar> Mercados)
        {
            foreach (var p in Mercados)
            {
                ListItem item = new ListItem { Text = p.Nombre, Value = p.Id.ToString() };

                cbl.Items.Add(item);


            }
        }
        private void cargarcombobox(DropDownList ddl, List<object> Lista, string Nombre, string id)
        {

            ddl.DataSource = Lista;
            ddl.DataTextField = Nombre;
            ddl.DataValueField = id;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Seleccione..", "0"));

        }




        private List<MercadoOfertar> GetListadoMercadoOferta()
        {
            List<MercadoOfertar> Mercados = new List<MercadoOfertar>
            {
                new MercadoOfertar()
                {
                    Id=1,
                    Nombre= "Local"
                },
                 new MercadoOfertar()
                {
                    Id=2,
                    Nombre= "Regional"
                },
                  new MercadoOfertar()
                {
                    Id=3,
                    Nombre= "Nacional"
                },
                   new MercadoOfertar()
                {
                    Id=4,
                    Nombre= "Internacional"
                }
            };

            return Mercados;

        }


        public static List<Departamento> llenarDepartamento(bool mostrar = false)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (mostrar)
                {
                    var departamentos = (from dep in db.departamento
                                         where dep.Mostrar == true
                                         select new Departamento
                                         {
                                             Id = dep.Id_Departamento,
                                             Nombre = dep.NomDepartamento
                                         }).ToList();
                    return departamentos;
                }
                else
                {
                    var departamentos = (from dep in db.departamento                                         
                                         select new Departamento
                                         {
                                             Id = dep.Id_Departamento,
                                             Nombre = dep.NomDepartamento
                                         }).ToList();
                    return departamentos;
                }                
            }
        }

        [WebMethod]
        public static List<Ciudad> llenarciudad(int CodDepartamento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var Ciudades = (from ciu in db.Ciudad
                                where ciu.CodDepartamento == CodDepartamento
                                select new Ciudad
                                {
                                    Id = ciu.Id_Ciudad,
                                    Nombre = ciu.NomCiudad,
                                    CodigoDepartamento = ciu.CodDepartamento
                                }).ToList();

                return Ciudades;
            }
        }

        public static List<NivelEstudio> llenarNivelEstudio()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {


                var NivelesEstudio = (from nivel in db.NivelEstudios
                                      select new NivelEstudio
                                      {
                                          Id = nivel.Id_NivelEstudio,
                                          Nombre = nivel.NomNivelEstudio
                                      }).ToList();
                return NivelesEstudio;

            }
        }

        [WebMethod]
        public static List<ProgramaAcademico> llenarProgramaAcademico(int CodNivelEstudio)

        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var programas = (from pro in db.ProgramaAcademicos
                                 where pro.CodNivelEstudio == CodNivelEstudio
                                 select new ProgramaAcademico
                                 {
                                     Id = pro.Id_ProgramaAcademico,
                                     Nombre = pro.NomProgramaAcademico
                                 }).ToList();

                return programas;


            }
        }

        [WebMethod]
        public static List<InstitucionEdu> LlenarInstitucionEducativa(int CodPrograma)

        {
            if (CodPrograma == 0)
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var Instituciones = (from ins in db.InstitucionEducativas
                                         select new InstitucionEdu
                                         {
                                             Id_InstitucionEducativa = ins.Id_InstitucionEducativa,
                                             NomInstitucionEducativa = ins.NomInstitucionEducativa

                                         }).ToList();

                    return Instituciones;
                }
            }
            else
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var InstitucionesEdu = (from pro in db.ProgramaAcademicos
                                            join inst in db.InstitucionEducativas on pro.CodInstitucionEducativa equals inst.Id_InstitucionEducativa
                                            where pro.Id_ProgramaAcademico == CodPrograma
                                            select new InstitucionEdu
                                            {
                                                Id_InstitucionEducativa = inst.Id_InstitucionEducativa,
                                                NomInstitucionEducativa = inst.NomInstitucionEducativa,
                                                CodPrograma = pro.Id_ProgramaAcademico
                                            }).ToList();
                    return InstitucionesEdu;
                }




            }
        }


        [WebMethod]
        public static List<Ciudad> llenarCiudadInstitucion(int CodInstitucion, int CodPrograma)

        {

            if (CodInstitucion != 0 && CodPrograma != 0)
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var CiuInst = (from pro in db.ProgramaAcademicos
                                   join Ciu in db.Ciudad on pro.CodCiudad equals Ciu.Id_Ciudad
                                   where pro.Id_ProgramaAcademico == CodPrograma && pro.CodInstitucionEducativa == CodInstitucion
                                   select new Ciudad
                                   {
                                       Id = Ciu.Id_Ciudad,
                                       Nombre = Ciu.NomCiudad

                                   }).ToList();

                    return CiuInst;
                }

            }
            else
            {

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var CiuInst = (from Ciu in db.Ciudad
                                   select new Ciudad
                                   {
                                       Id = Ciu.Id_Ciudad,
                                       Nombre = Ciu.NomCiudad

                                   }).ToList();

                    return CiuInst;
                }


            }


        }


        [WebMethod]

        public static List<CentrosEmpresariales> llenarCentroEmpresarial(int CodCiudad)
        {
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {


                var centros = (from cent in db.RU_CentroDesarrolloEmpresarial
                               where cent.CodCiudad == CodCiudad

                               select new CentrosEmpresariales
                               {
                                   Id = cent.Id_RUCentroDesarrollo,
                                   Nombre = cent.NomCentro + " - " + cent.Direccion

                               }).ToList();


                return centros;

            }
        }

        public static List<Formacion> llenarformacion()
        {
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                var Formaciones = (from form in db.RU_CursoFormacion
                                   select new Formacion
                                   {
                                       Id_Formacion = form.CodCurso,
                                       NombreFormacion = form.NomCurso
                                   }
                                ).Distinct().ToList();
                return Formaciones;
            }

        }

        [WebMethod]

        public static List<DepartamentoFormacion> llenardepartamentoformacion(int CodCurso)
        {

            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            List<DepartamentoFormacion> coddepar = new List<DepartamentoFormacion>();
            List<DepartamentoFormacion> coddepar1 = new List<DepartamentoFormacion>();


            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {

                using (Datos.FonadeDBDataContext db2 = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {


                    var lugarFor = (from codc in db.RU_CursoFormacion
                                    where codc.CodCurso == CodCurso
                                    select new { codc.CodDepartamento }).Distinct().ToList();




                    foreach (var i in lugarFor)
                    {
                        coddepar = (from d in db2.departamento

                                    where i.CodDepartamento == d.Id_Departamento
                                    select new DepartamentoFormacion
                                    {
                                        Id_DepartamentoFormacion = d.Id_Departamento,
                                        NomDepartamentoFormacion = d.NomDepartamento
                                    }).Distinct().ToList();



                        foreach (var p in coddepar)
                        {
                            coddepar1.Add(p);
                        }

                    }



                    return coddepar1;
                }

            }

        }

        [WebMethod]
        public static List<CiudadesFormacion> llenarciudadesformacion(int CodDepartamento, int CodCurso)
        {

            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            List<CiudadesFormacion> codciu = new List<CiudadesFormacion>();
            List<CiudadesFormacion> codciu1 = new List<CiudadesFormacion>();


            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {

                using (Datos.FonadeDBDataContext db2 = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {


                    var lugarFor = (from codc in db.RU_CursoFormacion
                                    where codc.CodDepartamento == CodDepartamento && codc.CodCurso == CodCurso
                                    select new { codc.CodCiudad }).Distinct().ToList();




                    foreach (var i in lugarFor)
                    {
                        codciu = (from c in db2.Ciudad

                                  where i.CodCiudad == c.Id_Ciudad
                                  select new CiudadesFormacion
                                  {
                                      Id_CiudadFormacion = c.Id_Ciudad,
                                      NomCiudadFormacion = c.NomCiudad
                                  }).Distinct().ToList();



                        foreach (var p in codciu)
                        {
                            codciu1.Add(p);
                        }

                    }



                    return codciu1;
                }

            }

        }


        public static List<Sectores> llenarSectores()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var sectores = (from sec in db.Sector
                                select new Sectores
                                {
                                    Id = sec.Id_Sector,
                                    Nombre = sec.NomSector
                                }
                                    ).ToList();
                return sectores;
            }
        }

        [WebMethod]
        public static List<SubSectores> llenarSubSectores(int CodSector)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var subsectores = (from subs in db.SubSector
                                   where subs.CodSector == CodSector
                                   select new SubSectores
                                   {
                                       Id = subs.Id_SubSector,
                                       Nombre = subs.NomSubSector,

                                   }
                                    ).ToList();


                return subsectores;
            }

        }


        public List<Tipoidentificacion> getTipoIdentificacion()
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var TipoDocumentos = (from tipodoc in db.TipoIdentificacions
                                      select new Tipoidentificacion
                                      {
                                          Id = tipodoc.Id_TipoIdentificacion,
                                          Nombre = tipodoc.NomTipoIdentificacion
                                      }
                                      ).ToList();

                return TipoDocumentos;
            }

        }

        public List<EstadoCivil> getEstadoCivil()
        {

            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {

                var EstadoCivil = (from Civil in db.RU_EstadoCivil
                                   select new EstadoCivil
                                   {
                                       Id_EstadoCivil = Civil.Id_EstadoCivil,
                                       NomEstadoCivil = Civil.NomEstadoCivil
                                   }).ToList();

                return EstadoCivil;
            }
        }

        public List<Ocupaciones> getOcupacion()
        {

            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {

                var ocupacion = (from ocupa in db.RU_Ocupacion
                                 select new Ocupaciones
                                 {
                                     Id_Ocupacion = ocupa.Id_RUOcupacion,
                                     NombreOcupacion = ocupa.NomOcupacion
                                 }).ToList();

                return ocupacion;
            }
        }

        public List<TipoApren> getTipoAprendiz()
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var TpApren = (from tpapren in db.TipoAprendizs
                               select new TipoApren
                               {
                                   Id_TipoAprendiz = tpapren.Id_TipoAprendiz,
                                   NomTipoAprendiz = tpapren.NomTipoAprendiz
                               }).ToList();

                return TpApren;
            }
        }


        public List<ServicioRequerido> getServicioRequerido()
        {

            using (Datos.RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                var servicio = (from serv in db.RU_Servicio
                                select new ServicioRequerido
                                {
                                    Id_Servicio = serv.Id_RUServicio,
                                    NombreServicio = serv.NomServicio
                                }
                                ).ToList();

                return servicio;
            }


        }


        public void InsertarRegistro(object sender, EventArgs e)
        {
            int id_registro = 0;
            int id_CentroEmpresarial = 0;
            int tipoindeti = 0;
            using (RegistroUnicoDataContext ru = new RegistroUnicoDataContext(conexion))
            {
                int ValidarUsuario = (from val in ru.RU_ContactoRegistro
                                      where val.Identificacion == Convert.ToInt32(TextNumIdentificacion.Text)
                                      select val.Identificacion).Count();

                Datos.RU_ContactoRegistro contactoregistroDatosCorreo = new Datos.RU_ContactoRegistro();

                if (ValidarUsuario == 0)
                {

                    if (Convert.ToInt32(cmbServicioRequerido.SelectedValue) == 2) //se realiza la separacion por Id_RUServicio, para que no se registre informacion que no es la correcta al servicio requerido.
                    {
                        Datos.RU_ContactoRegistro contactoregistronuevo = new Datos.RU_ContactoRegistro
                        {
                            Nombres = txtNombres.Text,
                            Apellidos = TextApellidos.Text,
                            CodTipoIdentificacion = Convert.ToInt16(cmbTipoIdentificacion.SelectedValue),
                            Identificacion = Convert.ToDouble(TextNumIdentificacion.Text),
                            LugarExpedicionDI = Convert.ToInt32(CodCiudadExpedicion.Text),
                            Correo = TextCorreo.Text,
                            Genero = Convert.ToChar(cmbGenero.SelectedValue),
                            Fecha_Nacimiento = Convert.ToDateTime(Textfechanac.Text),
                            CiudadNacimiento = Convert.ToInt32(CodCiudadNacimiento.Text),
                            Telefono = TextTelefono.Text,
                            CodTipoAprendiz = Convert.ToInt32(cmbTipoAprendiz.SelectedValue),
                            Id_EstadoCivil = Convert.ToInt32(cmbEstadoCivil.SelectedValue),
                            Id_RUOcupacion = Convert.ToInt32(cmbOcupacion.SelectedValue),
                            CiudadDondeReside = Convert.ToInt32(CodCiudadReside.Text),
                            DireccionDondeReside = TextDireccionReside.Text,
                            Estrato = Convert.ToChar(TextEstrato.Text),
                            Id_RUCentroDesarrollo = Convert.ToInt32(CodCentroDesarrollo.Text),
                            Id_RUServicio = Convert.ToInt32(cmbServicioRequerido.SelectedValue),
                            CodCurso = Convert.ToInt32(cmbFormacion.SelectedValue),
                            CiudadCurso = Convert.ToInt32(CodCiudadCurso.Text),
                            FechaRegistro = DateTime.Now

                        };

                        ru.RU_ContactoRegistro.InsertOnSubmit(contactoregistronuevo);
                        ru.SubmitChanges();

                        id_registro = contactoregistronuevo.Id_ContactoRegistro;
                        id_CentroEmpresarial = contactoregistronuevo.Id_RUCentroDesarrollo;
                        tipoindeti = contactoregistronuevo.CodTipoIdentificacion;

                        contactoregistroDatosCorreo = contactoregistronuevo;
                    }
                    else if (Convert.ToInt32(cmbServicioRequerido.SelectedValue) == 3 || Convert.ToInt32(cmbServicioRequerido.SelectedValue) == 4)
                    {
                        var CanEmple = TextCuantosEmpleados.Text == "" ? 0 : Convert.ToInt32(TextCuantosEmpleados.Text);
                        Datos.RU_ContactoRegistro contactoregistronuevo = new Datos.RU_ContactoRegistro
                        {
                            Nombres = txtNombres.Text,
                            Apellidos = TextApellidos.Text,
                            CodTipoIdentificacion = Convert.ToInt16(cmbTipoIdentificacion.SelectedValue),
                            Identificacion = Convert.ToDouble(TextNumIdentificacion.Text),
                            LugarExpedicionDI = Convert.ToInt32(CodCiudadExpedicion.Text),
                            Correo = TextCorreo.Text,
                            Genero = Convert.ToChar(cmbGenero.SelectedValue),
                            Fecha_Nacimiento = Convert.ToDateTime(Textfechanac.Text),
                            CiudadNacimiento = Convert.ToInt32(CodCiudadNacimiento.Text),
                            Telefono = TextTelefono.Text,
                            CodTipoAprendiz = Convert.ToInt32(cmbTipoAprendiz.SelectedValue),
                            Id_EstadoCivil = Convert.ToInt32(cmbEstadoCivil.SelectedValue),
                            Id_RUOcupacion = Convert.ToInt32(cmbOcupacion.SelectedValue),
                            CiudadDondeReside = Convert.ToInt32(CodCiudadReside.Text),
                            DireccionDondeReside = TextDireccionReside.Text,
                            Estrato = Convert.ToChar(TextEstrato.Text),
                            Id_RUCentroDesarrollo = Convert.ToInt32(CodCentroDesarrollo.Text),
                            Id_RUServicio = Convert.ToInt32(cmbServicioRequerido.SelectedValue),
                            CiudadDesarrollarProyecto = Convert.ToInt32(CodCiudadDesaProyec.Text),
                            CodSubSector = Convert.ToInt32(CodSubsectorEmprendimiento.Text),
                            NomProyecto = TextNomProyecto.Text,
                            Local = SeleccionCheckboxList(cblMercadoOfer, "Local") == true ? '1' : '0',
                            Regional = SeleccionCheckboxList(cblMercadoOfer, "Regional") == true ? '1' : '0',
                            Nacional = SeleccionCheckboxList(cblMercadoOfer, "Nacional") == true ? '1' : '0',
                            Internacional = SeleccionCheckboxList(cblMercadoOfer, "Internacional") == true ? '1' : '0',
                            DescripcionProyecto = TextDescPro.Text,
                            ProductoServicioOferta = TextProductoOferta.Text,
                            ProductoEnElMercado = TextProductoMercado.Text,
                            EmpleadosACargo = Convert.ToChar(cmbEmpleados.SelectedValue),
                            CantidadEmpleados = CanEmple,
                            FechaRegistro = DateTime.Now
                        };

                        ru.RU_ContactoRegistro.InsertOnSubmit(contactoregistronuevo);
                        ru.SubmitChanges();
                        id_registro = contactoregistronuevo.Id_ContactoRegistro;
                        id_CentroEmpresarial = contactoregistronuevo.Id_RUCentroDesarrollo;
                        tipoindeti = contactoregistronuevo.CodTipoIdentificacion;

                        contactoregistroDatosCorreo = contactoregistronuevo;
                    }
                    else if (Convert.ToInt32(cmbServicioRequerido.SelectedValue) == 5)
                    {
                        var CanEmple = TextNumEmpleados.Text == "" ? 0 : Convert.ToInt32(TextNumEmpleados.Text);

                        Datos.RU_ContactoRegistro contactoregistronuevo = new Datos.RU_ContactoRegistro
                        {
                            Nombres = txtNombres.Text,
                            Apellidos = TextApellidos.Text,
                            CodTipoIdentificacion = Convert.ToInt16(cmbTipoIdentificacion.SelectedValue),
                            Identificacion = Convert.ToDouble(TextNumIdentificacion.Text),
                            LugarExpedicionDI = Convert.ToInt32(CodCiudadExpedicion.Text),
                            Correo = TextCorreo.Text,
                            Genero = Convert.ToChar(cmbGenero.SelectedValue),
                            Fecha_Nacimiento = Convert.ToDateTime(Textfechanac.Text),
                            CiudadNacimiento = Convert.ToInt32(CodCiudadNacimiento.Text),
                            Telefono = TextTelefono.Text,
                            CodTipoAprendiz = Convert.ToInt32(cmbTipoAprendiz.SelectedValue),
                            Id_EstadoCivil = Convert.ToInt32(cmbEstadoCivil.SelectedValue),
                            Id_RUOcupacion = Convert.ToInt32(cmbOcupacion.SelectedValue),
                            CiudadDondeReside = Convert.ToInt32(CodCiudadReside.Text),
                            DireccionDondeReside = TextDireccionReside.Text,
                            Estrato = Convert.ToChar(TextEstrato.Text),
                            Id_RUCentroDesarrollo = Convert.ToInt32(CodCentroDesarrollo.Text),
                            Id_RUServicio = Convert.ToInt32(cmbServicioRequerido.SelectedValue),
                            NombreEmpresa = TextNomEmpresa.Text,
                            CodCiudadEmpresa = Convert.ToInt32(CodCiudadEmpresa.Text),
                            FechaConstitucion = Convert.ToDateTime(TextFechaEmpresa.Text),
                            CodSubSector = Convert.ToInt32(CodSubSectEmpresa.Text),
                            DireccionEmpresa = TextDireccionEmpresa.Text,
                            TelefonoEmpresa = TextTelefonoEmpresa.Text,
                            CorreoEmpresa = TextCorreoEmpresa.Text,
                            TamaEmpresa = cmbTamaEmpresa.SelectedValue,
                            ValorVentasAnuales = Convert.ToDouble(TextVentasAnuales.Text),
                            CantidadEmpleados = CanEmple,
                            UstedEsElPropietario = Convert.ToChar(cmbPropietario.SelectedValue),
                            CargoQueOcupa = TextCargoOcupa.Text,
                            ProductoServicioOferta = TextProductoOfertaE.Text,
                            DescripcionActividadEmpresa = TextActividadEconomica.Text,
                            Local = SeleccionCheckboxList(cblMercadoOferForta, "Local") == true ? '1' : '0',
                            Regional = SeleccionCheckboxList(cblMercadoOferForta, "Regional") == true ? '1' : '0',
                            Nacional = SeleccionCheckboxList(cblMercadoOferForta, "Nacional") == true ? '1' : '0',
                            Internacional = SeleccionCheckboxList(cblMercadoOferForta, "Internacional") == true ? '1' : '0',
                            FechaRegistro = DateTime.Now

                        };

                        ru.RU_ContactoRegistro.InsertOnSubmit(contactoregistronuevo);
                        ru.SubmitChanges();
                        id_registro = contactoregistronuevo.Id_ContactoRegistro;
                        id_CentroEmpresarial = contactoregistronuevo.Id_RUCentroDesarrollo;
                        tipoindeti = contactoregistronuevo.CodTipoIdentificacion;

                        contactoregistroDatosCorreo = contactoregistronuevo;
                    }


                    DateTime fechafin = Textfechafin.Text == "" ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(Textfechafin.Text);
                    DateTime fechaGrado = Textfechagraducacion.Text == "" ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(Textfechagraducacion.Text);
                    var SemestreActual = TextSemestreActual.Text == "" ? 0 : Convert.ToInt32(TextSemestreActual.Text);

                    Datos.RU_ContactoRegistroEstudio contactoestudio = new Datos.RU_ContactoRegistroEstudio
                    {
                        CodTipoNivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue),
                        CodProgramaRealizado = Convert.ToInt32(CodProAca.Text),
                        CodInstitucion = Convert.ToInt32(CodInstituAca.Text),
                        CodCiudad = Convert.ToInt32(CodCiudInst.Text),
                        Estado = cmbEstadoEstudio.SelectedValue,
                        Fecha_Inicio = Convert.ToDateTime(txtfechaini.Text),
                        FechaFinMaterias = fechafin,
                        FechaGrado = fechaGrado,
                        SemestresCursados = SemestreActual,
                        Id_ContactoRegistro = Convert.ToInt32(id_registro)
                    };

                    ru.RU_ContactoRegistroEstudio.InsertOnSubmit(contactoestudio);
                    ru.SubmitChanges();

                    EnviarEmailRegistroUnico(id_CentroEmpresarial, txtNombres.Text, TextApellidos.Text, TextNumIdentificacion.Text
                        , id_registro, tipoindeti, contactoregistroDatosCorreo, contactoestudio);

                    string MensajeOk = "La información se registro correctamente";
                    string Ruta = "http://www.fondoemprender.com";

                    Alert(MensajeOk, Ruta);
                }
                else
                {
                    string MensajeValidacion = string.Format("alert('El usuario ya ha enviado el formulario, solo se puede registrar una vez');");
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", MensajeValidacion, true);
                }


            }


        }

        private void Alert(string mensaje, string ruta = "")
        {
            if (ruta == "")
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');" +
                    "window.location='" + ruta + "';", true);
        }


        public bool SeleccionCheckboxList(CheckBoxList Mercado, string valorabuscar)
        {
            bool seleccion = false;


            foreach (ListItem item in Mercado.Items)
            {

                if (item.Text == valorabuscar)
                {
                    seleccion = item.Selected;
                    break;
                }

            }
            return seleccion;
        }

        private void EnviarEmailRegistroUnico(int centro, string nom, string apell, string NumIdentificacion
            , int id_Registro, int tipoiden, RU_ContactoRegistro contactoRegistro, RU_ContactoRegistroEstudio contactoRegistroEstudio)
        {

            RegistroUnicoDataContext con = new RegistroUnicoDataContext(conexion);

            Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);


            var tpidnt = (from ti in db.TipoIdentificacions
                          where ti.Id_TipoIdentificacion == tipoiden
                          select new
                          {
                              ti.NomTipoIdentificacion
                          }).FirstOrDefault();

            //ya no se va a buscar el lider regional sino directamente en la tabla de encargados
            var result = (from c in con.RU_CentroDesarrolloEmpresarial
                          where c.Id_RUCentroDesarrollo.Equals(centro)
                          select new
                          {
                              c.NombreContacto,
                              c.Correo,
                              c.CodCiudad
                          }).FirstOrDefault();

            var depar = (from ciud in db.Ciudad
                         join dep in db.departamento on ciud.CodDepartamento equals dep.Id_Departamento
                         where ciud.Id_Ciudad == result.CodCiudad
                         select new { dep.Id_Departamento }).FirstOrDefault();

            //ya no se va a buscar el lider regional sino directamente en la tabla de encargados
            var InformaLider = (from Cont in db.Contacto
                                join Ciu in db.Ciudad on Cont.CodCiudad equals Ciu.Id_Ciudad
                                join dep2 in db.departamento on Ciu.CodDepartamento equals dep2.Id_Departamento
                                join gp in db.GrupoContactos on Cont.Id_Contacto equals gp.CodContacto
                                join g in db.Grupos on gp.CodGrupo equals g.Id_Grupo
                                where g.Id_Grupo == Constantes.CONST_LiderRegional && dep2.Id_Departamento == depar.Id_Departamento
                                select new { Cont.Nombres, Cont.Apellidos, Cont.Email }).FirstOrDefault();


            string nombres = "";
            if (InformaLider!=null)
                nombres = InformaLider.Nombres ?? "" + " " + InformaLider.Apellidos ?? "";                      

            string cuerpo =
                "<h4>Cordial Saludo.</h4>" +
                "<br />" +
                "<br />" +
                "<p>Señores(as): " + result.NombreContacto + "," + nombres + ",</p>" +
                "<br />" +
                "<br />" +
                "<p>Con la presente comunicación se informa que el usuario:" + nom + " " + apell +
                " identificado con numero de:" + tpidnt.NomTipoIdentificacion + " " + NumIdentificacion +
                " realizo el registro unico de la solicitud: " + id_Registro + " para ser atendido por parte del personal del centro empresarial</p>" +
                "<br />" +
                "<h4>Información diligenciada:</h4>" +
                "<br />" +
                "<div>Nombres: " + contactoRegistro.Nombres + " " + contactoRegistro.Apellidos + "</div>" +
                "<br />" +
                "<div>Identificacion: " + contactoRegistro.Identificacion + "</div>" +
                "<br />" +
                "<div>Departamento Expedición: " + buscarNomDepartamento(contactoRegistro.LugarExpedicionDI) + "</div>" +
                "<br />" +
                "<div>Ciudad Expedición: " + buscarNomCiudad(contactoRegistro.LugarExpedicionDI) + "</div>" +
                "<br />" +
                "<div>Fecha Nacimiento: " + contactoRegistro.Fecha_Nacimiento.ToShortDateString() + "</div>" +
                "<br />" +
                "<div>Departamento Nacimiento: " + buscarNomDepartamento(contactoRegistro.CiudadNacimiento) + "</div>" +
                "<br />" +
                "<div>Ciudad Nacimiento: " + buscarNomCiudad(contactoRegistro.CiudadNacimiento) + "</div>" +
                "<br />" +
                "<div>Género: " + (contactoRegistro.Genero == 'M' ? "Masculino" : "Femenino") + "</div>" +
                "<br />" +
                "<div>Estado civil: " + contactoRegistro.RU_EstadoCivil.NomEstadoCivil + "</div>" +
                "<br />" +
                "<div>Ocupación: " + contactoRegistro.RU_Ocupacion.NomOcupacion + "</div>" +
                "<br />" +
                "<div>Telefono: " + contactoRegistro.Telefono + "</div>" +
                "<br />" +
                "<div>Correo Electronico: " + contactoRegistro.Correo + "</div>" +
                "<br />" +
                "<div>Departamento de Residencia: " + buscarNomDepartamento(contactoRegistro.CiudadDondeReside) + "</div>" +
                "<br />" +
                "<div>Municipio de Residencia: " + buscarNomCiudad(contactoRegistro.CiudadDondeReside) + "</div>" +
                "<br />" +
                "<div>Direccion de Residencia: " + contactoRegistro.DireccionDondeReside + "</div>" +
                "<br />" +
                "<div>Estrato: " + contactoRegistro.Estrato + "</div>" +
                "<br />" +
                "<div>Nivel Estudio: " + buscarNomNivelEstudio(contactoRegistroEstudio.CodTipoNivelEstudio) + "</div>" +
                "<br />" +
                "<div>Programa: " + buscarNomPrograma(contactoRegistroEstudio.CodProgramaRealizado) + "</div>" +
                "<br />" +
                "<div>Institucion: " + buscarNomInstitucionEducativa(contactoRegistroEstudio.CodInstitucion) + "</div>" +
                "<br />" +
                "<div>Ciudad Institucion: " + buscarNomCiudad(contactoRegistroEstudio.CodCiudad) + "</div>" +
                "<br />" +
                "<div>Estado: " + contactoRegistroEstudio.Estado + "</div>" +
                "<br />" +
                "<div>Fecha Inicio: " + (contactoRegistroEstudio.Fecha_Inicio.HasValue ?
                                    contactoRegistroEstudio.Fecha_Inicio.Value.ToShortDateString() : "") + "</div>" +

                estadoEstudio(contactoRegistroEstudio) +
                "<br />" +
                "<div>Tipo Aprendiz: " + buscarTipoAprendiz(contactoRegistro.CodTipoAprendiz) + "</div>"+
                "<br />" +
                "<div>Departamento Centro Desarrollo Empresarial: " + buscarNomDepartamento(contactoRegistro.RU_CentroDesarrolloEmpresarial.CodCiudad) + "</div>" +
                "<br />" +
                "<div>Ciudad Centro Desarrollo Empresarial: " + buscarNomCiudad(contactoRegistro.RU_CentroDesarrolloEmpresarial.CodCiudad) + "</div>" +
                "<br />" +
                "<div>Centro Desarrollo Empresarial: " + contactoRegistro.RU_CentroDesarrolloEmpresarial.NomCentro + "</div>" +
                "<br />" +
                "<div>Servicio acerca del cual desea mayor información: " + contactoRegistro.RU_Servicio.NomServicio + "</div>" +
                 estructuraServicioSeleccionado(contactoRegistro) +
                "<br />" +
                "<br />" +
                "Cordialmente," +
                "<br />" +
                "<br />" +
                "Fondo Emprender" +
                "<br />" +
                "<br />";

            //Enviar correo con copia oculta
            string responsableRegional = buscarResponsableRegional(contactoRegistro.RU_CentroDesarrolloEmpresarial.CodCiudad);
            string enviarEmailRegional = buscarCorreoRegional(contactoRegistro.RU_CentroDesarrolloEmpresarial.CodCiudad);
            string enviarEmailCopiaOculta = buscarCorreoRegionalCopiaOculta(contactoRegistro.RU_CentroDesarrolloEmpresarial.CodCiudad);

            Correo correo = new Correo("info@fondoemprender.com", "Fondo Emprender", enviarEmailRegional, responsableRegional
                            , "Registro Unico", cuerpo, enviarEmailCopiaOculta);

            correo.Enviar();


        }

        private string buscarResponsableRegional(int _codCiudad)
        {
            string responsable = "";
            int codDepto = 0;

            //buscar el codigo del departamento
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codDepto = (from c in db.Ciudad
                            where c.Id_Ciudad == _codCiudad
                            select c.CodDepartamento).FirstOrDefault();
            }

            //buscar el correo de la regional
            using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                responsable = (from c in db.RU_CorreoRegionales
                          where c.codDepartamento == codDepto
                          select c.Responsable).FirstOrDefault();
            }

            return responsable;
        }

        private string buscarCorreoRegional(int _codCiudad)
        {
            string correo = "";
            int codDepto = 0;

            //buscar el codigo del departamento
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codDepto = (from c in db.Ciudad
                          where c.Id_Ciudad == _codCiudad
                          select c.CodDepartamento).FirstOrDefault();
            }

            //buscar el correo de la regional
            using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                correo = (from c in db.RU_CorreoRegionales
                          where c.codDepartamento == codDepto
                          select c.Email).FirstOrDefault();
            }

                return correo;
        }

        private string buscarCorreoRegionalCopiaOculta(int _codCiudad)
        {
            string correo = "";
            int codDepto = 0;

            //buscar el codigo del departamento
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codDepto = (from c in db.Ciudad
                            where c.Id_Ciudad == _codCiudad
                            select c.CodDepartamento).FirstOrDefault();
            }

            //buscar el correo de la regional
            using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                correo = (from c in db.RU_CorreoRegionales
                          where c.codDepartamento == codDepto
                          select c.EmailConCopiaOculta).FirstOrDefault();
            }

            return correo;
        }

        private string estructuraServicioSeleccionado(RU_ContactoRegistro _contactoRegistro)
        {
            string estrutura = "";

            if (_contactoRegistro.RU_Servicio.Id_RUServicio == 2)//SENA Emprende Rural
            {
                estrutura = 
                "<br />" +
                "<div>Formacion en la que esta interesado: " + buscarNombreCursoFormacion(_contactoRegistro.CodCurso)  + "</div>"+
                "<br />" +
                "<div>Departamento de Formación: " + buscarNomDepartamento(_contactoRegistro.CiudadCurso??0) + "</div>"+
                "<br />" +
                "<div>Ciudad de Formación: " + buscarNomCiudad(_contactoRegistro.CiudadCurso ?? 0) + "</div>"
                ;
            }
            if (_contactoRegistro.RU_Servicio.Id_RUServicio == 3 || _contactoRegistro.RU_Servicio.Id_RUServicio == 4)//3 Emprendimiento - 4 Fondo Emprender
            {
                estrutura =
                "<br />" +
                "<div>Departamento donde va desarrollar el Proyecto: " + buscarNomDepartamento(_contactoRegistro.CiudadDesarrollarProyecto ?? 0) + "</div>" +
                "<br />" +
                "<div>Ciudad donde va desarrollar el Proyecto: " + buscarNomCiudad(_contactoRegistro.CiudadDesarrollarProyecto ?? 0) + "</div>" +
                "<br />" +
                "<div>Sector: " + buscarSector(_contactoRegistro.CodSubSector) + "</div>" +
                "<br />" +
                "<div>Subsector: " + buscarSubSector(_contactoRegistro.CodSubSector) + "</div>" +
                "<br />" +
                "<div>Nombre Del Proyecto: " + _contactoRegistro.NomProyecto + "</div>" +
                "<br />" +
                "<div>Mercado en el que vende o proyecta vender su producto o servicio: </div>" +
                "<br />" +
                "<div>Local: " + (_contactoRegistro.Local.HasValue ? (_contactoRegistro.Local.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Regional: " + (_contactoRegistro.Regional.HasValue ? (_contactoRegistro.Regional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Nacional: " + (_contactoRegistro.Nacional.HasValue ? (_contactoRegistro.Nacional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Internacional: " + (_contactoRegistro.Internacional.HasValue ? (_contactoRegistro.Internacional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Descripcion del Proyecto: " + _contactoRegistro.DescripcionProyecto + " </div>" +
                "<br />" +
                "<div>¿Producto o servicio que oferta o proyecta ofertar?: " + _contactoRegistro.ProductoServicioOferta + " </div>" +
                "<br />" +
                "<div>¿Actualmente comercializa su producto en el mercado?: " + _contactoRegistro.ProductoEnElMercado + " </div>" +
                "<br />" +
                "<div>¿Tiene empleados a su cargo?: " + (_contactoRegistro.EmpleadosACargo.HasValue ? 
                                                                (_contactoRegistro.EmpleadosACargo.Value == '1' ? "Si" : "No") : "")  + " </div>" +
                "<br />" +
                "<div>Cuantos: " + _contactoRegistro.CantidadEmpleados.Value??0 + " </div>";
            }
           
            if (_contactoRegistro.RU_Servicio.Id_RUServicio == 5)//Fortalecimiento
            {
                estrutura =
                "<br />" +
                "<div>Nombre de la Empresa: " + _contactoRegistro.NombreEmpresa + "</div>" +
                "<br />" +
                "<div>Departamento donde se encuentra su empresa: " + buscarNomDepartamento(_contactoRegistro.CodCiudadEmpresa ?? 0) + "</div>" +
                "<br />" +
                "<div>Ciudad donde se encuentra su empresa: " + buscarNomCiudad(_contactoRegistro.CodCiudadEmpresa ?? 0) + "</div>"+
                "<br />" +
                "<div>Sector: " + buscarSector(_contactoRegistro.CodSubSector ?? 0) + "</div>"+
                "<br />" +
                "<div>Subsector: " + buscarSubSector(_contactoRegistro.CodSubSector ?? 0) + "</div>"+
                "<br />" +
                "<div>Direccion de la Empresa: " + _contactoRegistro.DireccionEmpresa + "</div>"+
                "<br />" +
                "<div>Correo de la Empresa: " + _contactoRegistro.CorreoEmpresa + "</div>"+
                "<br />" +
                "<div>Teléfono de la Empresa: " + _contactoRegistro.TelefonoEmpresa + "</div>" +
                "<br />" +
                "<div>Fecha Constitución: " + (_contactoRegistro.FechaConstitucion.Value.ToShortDateString()??"") + "</div>" +
                "<br />" +
                "<div>Tamaño de la empresa: " + _contactoRegistro.TamaEmpresa + "</div>" +
                "<br />" +
                "<div>Mercado en el que vende su producto o servicio: </div>" +
                "<br />" +
                "<div>Local: " + (_contactoRegistro.Local.HasValue ? (_contactoRegistro.Local.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Regional: " + (_contactoRegistro.Regional.HasValue ? (_contactoRegistro.Regional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Nacional: " + (_contactoRegistro.Nacional.HasValue ? (_contactoRegistro.Nacional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Internacional: " + (_contactoRegistro.Internacional.HasValue ? (_contactoRegistro.Internacional.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Producto o servicio que oferta: " + _contactoRegistro.ProductoServicioOferta + " </div>" +
                "<br />" +
                "<div>Descripción de la actividad económica que desarrolla la empresa: " + _contactoRegistro.DescripcionActividadEmpresa + " </div>" +
                "<br />" +
                "<div>Valor de las ventas anuales ($): " + (_contactoRegistro.ValorVentasAnuales??0).ToString() + " </div>" +
                "<br />" +
                "<div>Número de empleados: " + (_contactoRegistro.CantidadEmpleados??0).ToString() + " </div>" +
                "<br />" +
                "<div>Es usted el propietario: " + (_contactoRegistro.UstedEsElPropietario.HasValue ? (_contactoRegistro.UstedEsElPropietario.Value == '1' ? "Si" : "No") : "") + " </div>" +
                "<br />" +
                "<div>Cargo que ocupa: " + _contactoRegistro.CargoQueOcupa + " </div>" 
                ;
            }

            return estrutura;
        }

        private string buscarSubSector(int? _idSubsector)
        {
            string nombre = "";

            if (_idSubsector.HasValue)
            {
                using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
                {
                    nombre = (from ss in db.SubSector                             
                              where ss.Id_SubSector == _idSubsector
                              select ss.NomSubSector).FirstOrDefault();
                }
            }

            return nombre;
        }

        private string buscarSector(int? _idSubsector)
        {
            string nombre = "";

            if (_idSubsector.HasValue)
            {
                using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
                {
                    nombre = (from ss in db.SubSector
                              join s in db.Sector on ss.CodSector equals s.Id_Sector
                              where ss.Id_SubSector == _idSubsector
                              select s.NomSector).FirstOrDefault();
                }
            }

            return nombre;
        }

        private string buscarNombreCursoFormacion(int? _idCurso)
        {
            string nombre = "";

            if (_idCurso.HasValue)
            {
                using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
                {
                    nombre = (from c in db.RU_CursoFormacion
                              where c.CodCurso == _idCurso
                              select c.NomCurso).FirstOrDefault();
                }
            }           

            return nombre;
        }

        private string buscarTipoAprendiz(int _tipoAprendiz)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.TipoAprendizs
                          where c.Id_TipoAprendiz == _tipoAprendiz
                          select c.NomTipoAprendiz).FirstOrDefault();
            }

            return nombre;
        }

        private string estadoEstudio(RU_ContactoRegistroEstudio _ContactoRegistroEstudio)
        {
            string estructura = "";

            if (_ContactoRegistroEstudio.Estado == "Actualmente cursando")
            {
                estructura = "<br />" +
                            "<div>Semestre actual u horas dedicadas: "
                                + (_ContactoRegistroEstudio.SemestresCursados.HasValue ?
                                _ContactoRegistroEstudio.SemestresCursados.Value.ToString() : "") + "</div>";
            }

            if (_ContactoRegistroEstudio.Estado == "Finalizado")
            {
                estructura = "<br />" +
                            "<div>Fecha Finalización: "
                                + (_ContactoRegistroEstudio.FechaFinMaterias.HasValue ?
                                _ContactoRegistroEstudio.FechaFinMaterias.Value.ToShortDateString() : "") + "</div>"
                             + "<br />" +
                            "<div>Fecha Graduación: "
                                + (_ContactoRegistroEstudio.FechaGrado.HasValue ?
                                _ContactoRegistroEstudio.FechaGrado.Value.ToShortDateString() : "") + "</div>";
            }

            return estructura;
        }

        private string buscarNomInstitucionEducativa(int? _idInstitucion)
        {
            string nombre = "";

            if (_idInstitucion.HasValue)
            {
                using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
                {
                    nombre = (from c in db.InstitucionEducativas
                              where c.Id_InstitucionEducativa == _idInstitucion
                              select c.NomInstitucionEducativa).FirstOrDefault();
                }
            }

            return nombre;
        }

        private string buscarNomPrograma(int? _idPrograma)
        {
            string nombre = "";

            if (_idPrograma.HasValue)
            {
                using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
                {
                    nombre = (from c in db.ProgramaAcademicos
                              where c.Id_ProgramaAcademico == _idPrograma
                              select c.NomProgramaAcademico).FirstOrDefault();
                }
            }

            return nombre;
        }

        private string buscarNomDepartamento(int codCiudad)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.Ciudad
                          join d in db.departamento on c.CodDepartamento equals d.Id_Departamento
                          where c.Id_Ciudad == codCiudad
                          select d.NomDepartamento).FirstOrDefault();
            }

            return nombre;
        }

        private string buscarNomNivelEstudio(int? _idNivelEstudio)
        {
            string nombre = "";

            if (_idNivelEstudio.HasValue)
            {
                using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
                {
                    nombre = (from c in db.NivelEstudios
                              where c.Id_NivelEstudio == _idNivelEstudio
                              select c.NomNivelEstudio).FirstOrDefault();
                }
            }

            return nombre;
        }

        private string buscarEstadoCivil(int _idEstado)
        {
            string nombre = "";

            using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                nombre = (from c in db.RU_EstadoCivil
                          where c.Id_EstadoCivil == _idEstado
                          select c.NomEstadoCivil).FirstOrDefault();
            }

            return nombre;
        }

        private string buscarNomCiudad(int codCiudad)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.Ciudad
                          where c.Id_Ciudad == codCiudad
                          select c.NomCiudad).FirstOrDefault();
            }

            return nombre;
        }

        public class Sectores
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class SubSectores
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Departamento
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Ciudad
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int CodigoDepartamento { get; set; }
        }

        public class Tipoidentificacion
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class NivelEstudio
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class ProgramaAcademico
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int CodigoNivelEstudio { get; set; }
        }
        public class Instituciones
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int CodigoPrograma { get; set; }
            public int Id_Ciudad { get; set; }
            public string NombreCiudad { get; set; }

        }

        public class EstadoCivil
        {
            public int Id_EstadoCivil { get; set; }
            public string NomEstadoCivil { get; set; }

        }

        public class TipoApren
        {
            public int Id_TipoAprendiz { get; set; }
            public string NomTipoAprendiz { get; set; }

        }
        public class Ocupaciones
        {
            public int Id_Ocupacion { get; set; }
            public string NombreOcupacion { get; set; }

        }

        public class CentrosEmpresariales
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Direccion { get; set; }
        }


        public class ServicioRequerido
        {
            public int Id_Servicio { get; set; }
            public string NombreServicio { get; set; }
        }

        public class Formacion
        {
            public long Id_Formacion { get; set; }
            public string NombreFormacion { get; set; }

        }

        public class DepartamentoFormacion
        {
            public int Id_DepartamentoFormacion { get; set; }
            public string NomDepartamentoFormacion { get; set; }

        }
        public class CiudadesFormacion
        {
            public int Id_CiudadFormacion { get; set; }
            public string NomCiudadFormacion { get; set; }
        }

        public class InstitucionEdu
        {
            public int Id_InstitucionEducativa { get; set; }
            public string NomInstitucionEducativa { get; set; }
            public int CodPrograma { get; set; }

        }

        public class MercadoOfertar
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class RegistrarFormulario
        {
            public int Id { get; set; }
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public double Identificacion { get; set; }
            public int DepartamentoExpedicion { get; set; }
            public int CiudadExpedicion { get; set; }
            public string Correo { get; set; }
            public char Genero { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public int DepartamentoNacimiento { get; set; }
            public int CiudadNacimiento { get; set; }
            public string Telefono { get; set; }
            public int EstadoCivil { get; set; }
            public int Ocupacion { get; set; }
            public int DepartamentoResidencia { get; set; }
            public int CiudadResidencia { get; set; }
            public string DireccionResidencia { get; set; }
            public char Estrato { get; set; }
            public int NivelEstudio { get; set; }
            public int CodigoPrograma { get; set; }
            public int CodigoInstitucion { get; set; }
            public int CiudadInstitucion { get; set; }
            public bool IsEstudioFinalizado { get; set; }
            public DateTime FechaIncio { get; set; }

        }



    }
}