using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fonade.Negocio.Proyecto
{
    public class ProyectoController
    {
        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosActivosADMIN()
        {

            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerTODOSLosProyectosActivos()
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();
            }

            return query;
        }

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosActivosJefeUnidad(int codInstitucion)
        {
            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerTODOSLosProyectosJefeUnidad(codInstitucion)
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();
            }
            return query;
        }

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosEmprendedorOAsesor(int codInstitucion, int codUsuario)
        {
            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerProyectosEmprendedorOAsesor(codInstitucion, codUsuario)
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();
            }
            return query;
        }

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosEvaluacionCoordinador(int codUsuario, int? codOperador, int codEstadoProyecto)
        {
            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerProyectosTodosEvaluacionCoordinacion(
                                                                                                    codUsuario
                                                                                                    , codEstadoProyecto
                                                                                                    , codOperador)
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();
            }
            return query;
        }

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosGerenciaEvaluacion(int codEstadoProyecto, int? codOperador)
        {
            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerTodosProyectosGerenciaEvaluacion(codEstadoProyecto, codOperador)
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();

            }
            return query;
        }

        public string verIdentificacionXArchivo(string idArchivo)
        {
            int codArchivo = Convert.ToInt32(idArchivo);
            string identificacion = "";
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                int codContactoArchivo = (from c in db.ContratosArchivosAnexos
                                          where c.IdContratoArchivoAnexo == codArchivo
                                          select c.CodContacto).FirstOrDefault() ?? 0;

                identificacion = (from c in db.Contacto
                                  where c.Id_Contacto == codContactoArchivo
                                  select c.Identificacion).FirstOrDefault().ToString();
            }

            return identificacion;
        }

        public List<ListadoPlanesDeNegocio> VerTodosLosProyectosLiderRegional(int? codDptoInst)
        {
            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>();
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                query = (from p in db.MD_VerTodosProyectosLiderRegional(codDptoInst)
                         select new ListadoPlanesDeNegocio
                         {
                             IdProyecto = p.Id_Proyecto,
                             NombreProyecto = p.NomProyecto,
                             CodigoInstitucion = p.Id_Institucion,
                             CodigoEstado = p.CodEstado,
                             NombreUnidad = p.NomUnidad,
                             NombreInstitucion = p.NomInstitucion,
                             NombreCiudad = p.NomCiudad,
                             NombreDepartamento = p.NomDepartamento,
                             idDpto = p.Id_Departamento,
                             idTipoInstitucion = p.CodTipoInstitucion,
                             idDptoInst = p.Id_Departamento1,
                             Estado = (p.Inactivo) ? "Inactivo" : "Activo",
                             codOperador = p.codOperador,
                             NombreOperador = p.NombreOperador
                         }).ToList();
            }
            return query;
        }

        public bool validarTerminosSCDProyecto(int _codUsuario)
        {
            bool validar;
            //validar si el estado del proyecto es 5 = Asignacion de Recursos
            int codProyecto = codProyectoXEmprendedor(_codUsuario);
            if (validarEstadoProyecto(codProyecto, Constantes.CONST_AsignacionRecursos)
                || validarEstadoProyecto(codProyecto, Constantes.CONST_Ejecucion))
            {
                //validar si el operador del proyecto aplican los terminos

                int? codOperador = codOperadorXProyecto(codProyecto);

                if (codOperador == null || codOperador == 0)
                {
                    validar = false;
                }
                else
                {
                    //validar si ya el emprendedor validó los terminosSCD
                    if (validarTerminosSCDXEmprendedor(_codUsuario))
                    {
                        //si el emprendedor ya validó los terminosSCD, no es necesario volver a pedir la confirmacion
                        validar = false;
                    }
                    else
                    {
                        validar = validarTerminosSCD(codOperador);
                    }
                }
            }
            else
            {
                validar = false;
            }

            return validar;
        }

        public bool validarTerminosSCDXEmprendedor(int _codEmprendedor)
        {
            bool validar = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                validar = (from c in db.Contacto
                           where c.Id_Contacto == _codEmprendedor
                           select c.AceptoTerminosSCD).FirstOrDefault() ?? false;
            }

            return validar;
        }

        public bool validarCargaCertificacionDigital(int _codProyecto, int _codUsuario)
        {
            bool Debecargar = false;
            int codOperador = 0;
            int codEstado = 0;
            bool validarConfig = false;

            //validar si el operador solicita cargar el archivo
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codOperador = (from p in db.Proyecto
                               where p.Id_Proyecto == _codProyecto
                               select p.codOperador).FirstOrDefault() ?? 0;
            }

            if (codOperador != 0)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    var configuracion = (from c in db.ConfiguracionOperadores
                                         where c.codOperador == codOperador
                                         select c).FirstOrDefault();

                    if (configuracion != null)
                    {
                        codEstado = configuracion.estadoProyectoCargaCertificadoDigAut ?? 0;
                        validarConfig = configuracion.cargarCertificadoDigitalAutenticado;
                    }
                }
            }

            if (validarConfig)
            {
                if (validarEstadoProyecto(_codProyecto, codEstado)
                    || validarEstadoProyecto(_codProyecto, Constantes.CONST_Ejecucion))
                {
                    //validar si el usuario ya cargó el archivo
                    if (!validarCargaArchivoCertDigAutenticado(_codProyecto, _codUsuario))
                    {
                        Debecargar = true;
                    }
                }
            }

            return Debecargar;
        }

        private bool validarCargaArchivoCertDigAutenticado(int _codProyecto, int _codUsuario)
        {
            bool cargado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var query = (from c in db.ContratosArchivosAnexos
                             where c.CodProyecto == _codProyecto
                             && c.CodContacto == _codUsuario
                             && c.NombreArchivo.Contains("SolicitudCertificadoDigitalAutenticada")
                             select c
                             ).ToList();

                if (query.Count > 0)
                {
                    cargado = true;
                }
            }

            return cargado;
        }

        private bool validarEstadoProyecto(int _codProyecto, int _codEstado)
        {
            bool validar = false;
            int idEstado = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                idEstado = (from p in db.Proyecto
                            where p.Id_Proyecto == _codProyecto
                            select p.CodEstado).FirstOrDefault();
            }

            if (idEstado == _codEstado)
                validar = true;

            return validar;
        }

        private bool validarTerminosSCD(int? _codOperador)
        {
            bool validar = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                validar = (from o in db.Operador
                           where o.IdOperador == _codOperador
                           select o.AplicaTerminosSCD).FirstOrDefault() ?? false;
            }

            return validar;
        }

        private int? codOperadorXProyecto(int _codProyecto)
        {
            int? codOperador = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codOperador = (from p in db.Proyecto
                               where p.Id_Proyecto == _codProyecto
                               select p.codOperador).FirstOrDefault();
            }

            return codOperador;
        }

        public int codProyectoXEmprendedor(int _codEmprendedor)
        {
            int _codProyecto = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                _codProyecto = (from p in db.ProyectoContactos
                                where p.CodContacto == _codEmprendedor
                                && p.Inactivo == false && p.CodRol == 3
                                orderby p.CodProyecto descending
                                select p.CodProyecto).FirstOrDefault();
            }

            return _codProyecto;
        }

        public int codEstadoPago(int _codPago)
        {
            int codEstado = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codEstado = (from p in db.PagoActividad
                             where p.Id_PagoActividad == _codPago
                             select p.Estado).FirstOrDefault() ?? 0;
            }

            return codEstado;
        }

        public string nombreProyectoXCodigo(int _codProyecto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from p in db.Proyecto
                          where p.Id_Proyecto == _codProyecto
                          select p.NomProyecto).FirstOrDefault();
            }

            return nombre;
        }

        private int _TimeOut = 200000;

        public EvalDatosGenerales infoEvalDatosGenerales(int _codProyecto, int _codConvocatoria)
        {
            EvalDatosGenerales general = new EvalDatosGenerales();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                general = (from p in db.SPIAE_TabEvaDatosGenerales(_codProyecto, _codConvocatoria)
                           select new EvalDatosGenerales
                           {
                               Localizacion = p.Localizacion,
                               ResumenConceptoGeneral = p.ResumenConcepto,
                               Sector = p.Sector
                           }).FirstOrDefault();
            }

            return general;
        }

        public List<InfoTabEvaluacionProyecto> infoEvalTabEvaluacion(int _codProyecto, int _codConvocatoria)
        {
            List<InfoTabEvaluacionProyecto> tabEvaluacion = new List<InfoTabEvaluacionProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                tabEvaluacion = (from p in db.SPIAE_TabEvaPuntajesDeAspectos(_codProyecto, _codConvocatoria)
                           select new InfoTabEvaluacionProyecto
                           {
                               campo = p.Campo,
                               idCampo = p.Id_Campo,
                               Observacion = p.Justificacion,
                               puntaje = p.Puntaje.HasValue ? p.Puntaje.Value.ToString() : ""
                           }).ToList();               
            }

            return tabEvaluacion;
        }

        public InfoGeneralProyecto infoGeneralProyecto(int _codProyecto)
        {
            InfoGeneralProyecto general = new InfoGeneralProyecto();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                general = (from p in db.SPIA_General(_codProyecto)
                           select new InfoGeneralProyecto
                           {
                               CodigoDane = p.CodigoDane,
                               CodInstitucion = p.CodInstitucion,
                               Id_proyecto = p.Id_proyecto,
                               NomCiudad = p.NomCiudad,
                               NomInstitucion = p.NomInstitucion,
                               NomProyecto = p.NomProyecto,
                               NomUnidad = p.NomUnidad,
                               CodigoCIIU = p.Codigo,
                               Sector = p.NomSector,
                               SubSector = p.NomSubsector,
                               fechaNacimiento = p.FechaNacimiento.HasValue ? p.FechaNacimiento.Value.ToShortDateString() : "",
                               Estado = p.Estado,
                               genero = p.Genero.ToString(),
                               Institucion = p.Institucion,
                               nivelEstudio = p.NivelEstudio,
                               Programa = p.Programa
                           }).FirstOrDefault();
            }

            return general;
        }



        public List<InfoGeneralProyecto> ListadoProyectosPorConvocatoria(int _codConvocatoria)
        {
            List<InfoGeneralProyecto> list = new List<InfoGeneralProyecto>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.Proyecto
                        join pc in db.ConvocatoriaProyectos on p.Id_Proyecto equals pc.CodProyecto
                        where pc.CodConvocatoria == _codConvocatoria
                        && (p.CodEstado >= Constantes.CONST_Convocatoria)
                        select new InfoGeneralProyecto
                        {
                            Id_proyecto = p.Id_Proyecto,
                            NomProyecto = p.NomProyecto
                        }).ToList();
            }

            return list;
        }

        public List<InfoGeneralProyecto> ListadoProyectosSinTenerEnCuentaEstado(int _codConvocatoria)
        {
            List<InfoGeneralProyecto> list = new List<InfoGeneralProyecto>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.Proyecto
                        join pc in db.ConvocatoriaProyectos on p.Id_Proyecto equals pc.CodProyecto
                        where pc.CodConvocatoria == _codConvocatoria
                        select new InfoGeneralProyecto
                        {
                            Id_proyecto = p.Id_Proyecto,
                            NomProyecto = p.NomProyecto
                        }).ToList();
            }

            return list;
        }

        public List<InfoGeneralProyecto> ListadoProyectosEvaluados(int _codConvocatoria)
        {
            List<InfoGeneralProyecto> list = new List<InfoGeneralProyecto>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.Proyecto
                        join pc in db.EvaluacionObservacions on p.Id_Proyecto equals pc.CodProyecto
                        where pc.CodConvocatoria == _codConvocatoria
                        select new InfoGeneralProyecto
                        {
                            Id_proyecto = p.Id_Proyecto,
                            NomProyecto = p.NomProyecto
                        }).ToList();
            }

            return list;
        }

        public List<ProtagonistaCliente> infoProtagonistaCliente(int _codProyecto)
        {
            List<ProtagonistaCliente> list = new List<ProtagonistaCliente>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_ProtagonistaCliente(_codProyecto)
                        select new ProtagonistaCliente
                        {
                            NombreCliente = p.Nombre,
                            Justificacion = p.Justificacion,
                            Localizacion = p.Localizacion,
                            Perfil = p.Perfil
                        }).ToList();
            }

            return list;
        }

        public Protagonista infoProtagonista(int _codProyecto)
        {
            Protagonista list = new Protagonista();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_Protagonista(_codProyecto)
                        select new Protagonista
                        {
                            Cliente = p.NecesidadesPotencialesClientes,
                            Consumidores = p.NecesidadesPotencialesConsumidores,
                            PerfilConsumidor = p.PerfilConsumidor
                        }).FirstOrDefault();
            }

            return list;
        }

        public OportunidadMercado infoOportunidadMercado(int _codProyecto)
        {
            OportunidadMercado list = new OportunidadMercado();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_OportunidadDeMercado(_codProyecto)
                        select new OportunidadMercado
                        {
                            OportunidadDeMercado = p.TendenciaCrecimiento
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<OportunidadMercadoCompetidores> infoOportunidadMercadoCompetidores(int _codProyecto)
        {
            List<OportunidadMercadoCompetidores> list = new List<OportunidadMercadoCompetidores>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_OportunidadDeMercadoCompetidores(_codProyecto)
                        select new OportunidadMercadoCompetidores
                        {
                            Localizacion = p.Localizacion,
                            LogisticaDistribucion = p.LogisticaDistribucion,
                            Nombre = p.Nombre,
                            OtroCual = p.OtroCual,
                            Precios = p.Precios,
                            ProductosServicios = p.ProductosServicios
                        }).ToList();
            }

            return list;
        }

        public SolucionSolucion infoSolucionSolucion(int _codProyecto)
        {
            SolucionSolucion list = new SolucionSolucion();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_SolucionSolucion(_codProyecto)
                        select new SolucionSolucion
                        {
                            ConceptoNegocio = p.ConceptoNegocio,
                            ComponenteInnovador = p.InnovadorConceptoNegocio,
                            ProductoServicio = p.ProductoServicio,
                            Proceso = p.Proceso,
                            AspectoTecnicoProductivo = p.TecnicoProductivo,
                            AspectoComercial = p.Comercial,
                            AspectoLegal = p.Legal,
                            AceptacionMercado = p.AceptacionProyecto
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<SolucionFichaTecnica> infoSolucionFichaTecnica(int _codProyecto)
        {
            List<SolucionFichaTecnica> list = new List<SolucionFichaTecnica>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_SolucionFichaTecnica(_codProyecto)
                        select new SolucionFichaTecnica
                        {
                            Composicion = p.Composicion,
                            CondicionesEspeciales = p.CondicionesEspeciales,
                            DescripcionGeneral = p.DescripcionGeneral,
                            NombreComercial = p.NombreComercial,
                            Otros = p.Otros,
                            ProductoMasRrepresentativo = p.ProductoMasRepresentativo,
                            UnidadDeMedida = p.UnidadMedida,
                            ProductoEspecifico = p.NomProducto
                        }).ToList();
            }

            return list;
        }

        public int estadoProyecto(int _codProyecto)
        {
            int codEstado = 1;
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codEstado = (from p in db.Proyecto
                             where p.Id_Proyecto == _codProyecto
                             select p.CodEstado).FirstOrDefault();
            }
            return codEstado;
        }

        public List<DesarrolloSolucionCondicionesComerciales> infoDesarrolloSolucionCondicionComercial(int _codProyecto)
        {
            List<DesarrolloSolucionCondicionesComerciales> list = new List<DesarrolloSolucionCondicionesComerciales>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionIngresosYCondicionesComer(_codProyecto)
                        select new DesarrolloSolucionCondicionesComerciales
                        {
                            CaracteristicasCompra = p.CaracteristicasCompra,
                            FormaPago = p.FormaPago,
                            Garantias = p.Garantias,
                            MargenComercializacion = p.MargenDeComercialización,
                            Precio = p.Precio,
                            RequisitosPostVenta = p.RequisitosPostVenta,
                            SitioCcompra = p.SitioCompra,
                            VolumenesFrecuencia = p.VolúmenesYFrecuencia,
                            Cliente = p.Nombre
                        }).ToList();
            }

            return list;
        }

        public List<ProyeccionListadoProducto> infoDesarrolloSolucionProyeccionListadoProductos(int _codProyecto)
        {
            List<ProyeccionListadoProducto> list = new List<ProyeccionListadoProducto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionProyeccionProductos(_codProyecto)
                        select new ProyeccionListadoProducto
                        {
                            FormaDePago = p.FormaDePago,
                            IVA = p.Iva.ToString() ?? "",
                            Justificacion = p.Justificacion,
                            NomProduto = p.NomProducto,
                            UnidadMedida = p.UnidadMedida
                        }).ToList();
            }

            return list;
        }

        public DesarrolloSolucionIngresos infoDesarrolloSolucionIngresos(int _codProyecto)
        {
            DesarrolloSolucionIngresos list = new DesarrolloSolucionIngresos();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionIngresos(_codProyecto)
                        select new DesarrolloSolucionIngresos
                        {
                            CaracteristicasParaCompra = p.CaracteristicasCompra,
                            DondeCompra = p.DondeCompra,
                            EstrategiaIngresos = p.Ingresos,
                            FrecuenciaCompra = p.FrecuenciaCompra,
                            Precio = p.Precio
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.IngresosPorVentas> infoIngresosPorVentas(int codigoProyecto)
        {
            return Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetIngresosPorVentas(codigoProyecto);
        }

        public DesarrolloSolucionNormatividad infoDesarrolloSolucionNormatividad(int _codProyecto)
        {
            DesarrolloSolucionNormatividad list = new DesarrolloSolucionNormatividad();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionNormatividad(_codProyecto)
                        select new DesarrolloSolucionNormatividad
                        {
                            CondicionesTecnicasOperacionNegocio = p.CondicionesTecnicas,
                            NormatividadAmbiental = p.Ambiental,
                            NormatividadEmpresarial = p.Empresarial,
                            NormatividadLaboral = p.Laboral,
                            NormatividadTecnica = p.Tecnica,
                            NormatividadTributaria = p.Tributaria,
                            RegistroMarca = p.RegistroMarca
                        }).FirstOrDefault();
            }

            return list;
        }

        public DesarrolloSolucionRequerimientos infoDesarrolloSolucionRequerimiento(int _codProyecto)
        {
            DesarrolloSolucionRequerimientos list = new DesarrolloSolucionRequerimientos();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionRequerimientos(_codProyecto)
                        select new DesarrolloSolucionRequerimientos
                        {
                            JustificacionLugarFisico = p.LugarFisico,
                            NecesarioLugarFisico = p.TieneLugarFisico
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<DesarrolloSolucionReqInversion> infoDesarrolloSolucionReqInversion(int _codProyecto, int _codTipo)
        {
            List<DesarrolloSolucionReqInversion> list = new List<DesarrolloSolucionReqInversion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionReqInversion(_codProyecto, _codTipo)
                        select new DesarrolloSolucionReqInversion
                        {
                            Cantidad = p.Cantidad ?? 0,
                            Descripcion = p.Descripcion,
                            FuenteFinanciacion = p.DescFuente,
                            RequisitosTecnicos = p.RequisitosTecnicos,
                            ValorUnitario = p.ValorUnidad ?? 0
                        }).ToList();
            }

            return list;
        }

        public DesarrolloSolucionProduccion infoDesarrolloSolucionProduccion(int _codProyecto)
        {
            DesarrolloSolucionProduccion list = new DesarrolloSolucionProduccion();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionProduccion(_codProyecto)
                        select new DesarrolloSolucionProduccion
                        {
                            ContemplaImportacion = p.RealizaImportacion,
                            DetalleActivos = p.ActivosProveedores,
                            DetalleCondicionesTecnicas = p.CondicionesTecnicas,
                            FinanciacionMayorValor = p.IncrementoValor,
                            JustificacionContemplaImportacion = p.JustificacionImportacion

                        }).FirstOrDefault();
            }

            return list;
        }

        public List<DesarrolloSolucionProcesoProduccion> infoDesarrolloSolucionProcesoProduccion(int _codProyecto)
        {
            List<DesarrolloSolucionProcesoProduccion> list = new List<DesarrolloSolucionProcesoProduccion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucionProcesoProduccion(_codProyecto)
                        select new DesarrolloSolucionProcesoProduccion
                        {
                            NombreProducto = p.NomProducto,
                            ProcesoProduccionProducto = p.DescripcionProceso
                        }).ToList();
            }

            return list;
        }

        public DesarrolloSolucionProductividad infoDesarrolloSolucionProductividad(int _codProyecto)
        {
            DesarrolloSolucionProductividad list = new DesarrolloSolucionProductividad();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucíonProductividad(_codProyecto)
                        select new DesarrolloSolucionProductividad
                        {
                            CapacidadProductivaEmpresa = p.CapacidadProductiva
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<DesarrolloSolucionPerfilEmprendedor> infoDesarrolloSolucionPerfilEmprendedor(int _codProyecto)
        {
            List<DesarrolloSolucionPerfilEmprendedor> list = new List<DesarrolloSolucionPerfilEmprendedor>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucíonProductividad(_codProyecto)
                        select new DesarrolloSolucionPerfilEmprendedor
                        {
                            Perfil = p.Perfil,
                            Rol = p.Rol
                        }).ToList();
            }

            return list;
        }

        public List<DesarrolloSolucionCargosOperacion> infoDesarrolloSolucionCargosOperacion(int _codProyecto)
        {
            List<DesarrolloSolucionCargosOperacion> list = new List<DesarrolloSolucionCargosOperacion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_DesarrolloSolucíonCargosOperación(_codProyecto)
                        select new DesarrolloSolucionCargosOperacion
                        {
                            NombreCargo = p.Cargo,
                            aportesEmprendedores = p.AportesEmprendedor ?? 0,
                            ExperienciaGeneral = p.ExperienciaGeneral,
                            FuncionesPrincipales = p.Funciones,
                            DedicacionTiempo = p.Dedicacion,
                            ExperienciaEspecifica = p.ExperienciaEspecifica,
                            ingresosPorVentas = p.IngresosVentas ?? 0,
                            otrosGastos = p.OtrosGastos,
                            remuneracionPrimerAno = p.RemuneraciónPrimerAño ?? 0,
                            tiempoVinculacion = p.TiempoVinculacion ?? 0,
                            TipoContratacion = p.TipoContratacion,
                            UnidadMedidaTiempo = p.UnidadDeMedidaDeTiempo,
                            valorConPrestaciones = p.ValorConPrestaciones ?? 0,
                            valorRemuneracion = p.ValorRemuneracion ?? 0,
                            valorSolicitadoFondoEmprender = p.ValorFondoEmprender ?? 0,
                            Perfil = p.Formacion
                        }).ToList();
            }

            return list;
        }

        public FuturoNegocioEstrategia infoFuturoNegocioEstrategia(int _codProyecto)
        {
            FuturoNegocioEstrategia list = new FuturoNegocioEstrategia();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_FuturoDelNegocioEstrategias(_codProyecto)
                        select new FuturoNegocioEstrategia
                        {
                            NombreEstrategiaComunicacion = p.EstrategiaComunicacion,
                            NombreEstrategiaDistribucion = p.EstrategiaDistribucion,
                            NombreEstrategiaPromocion = p.EstrategiaPromocion,
                            PropositoComunicacion = p.EstrategiaComunicacionProposito,
                            PropositoDistribucion = p.EstrategiaDistribucionProposito,
                            PropositoPromocion = p.EstrategiaPromocionProposito
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<FuturoNegocioActividades> infoFuturoNegocioActividadesPromocion(int _codProyecto)
        {
            List<FuturoNegocioActividades> list = new List<FuturoNegocioActividades>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_FuturoDelNegocioEstrategiasPromocion(_codProyecto)
                        select new FuturoNegocioActividades
                        {
                            Actividad = p.Actividad,
                            costo = p.Costo,
                            MesDeEjecucion = p.MesEjecucion,
                            RecursoRequerido = p.RecursosRequeridos,
                            Responsable = p.Responsable
                        }).ToList();
            }

            return list;
        }

        public List<FuturoNegocioActividades> infoFuturoNegocioActividadesComunicacion(int _codProyecto)
        {
            List<FuturoNegocioActividades> list = new List<FuturoNegocioActividades>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_FuturoDelNegocioEstrategiasComunicacion(_codProyecto)
                        select new FuturoNegocioActividades
                        {
                            Actividad = p.Actividad,
                            costo = p.Costo,
                            MesDeEjecucion = p.MesEjecucion,
                            RecursoRequerido = p.RecursosRequeridos,
                            Responsable = p.Responsable
                        }).ToList();
            }

            return list;
        }

        public List<FuturoNegocioActividades> infoFuturoNegocioActividadesDistribucion(int _codProyecto)
        {
            List<FuturoNegocioActividades> list = new List<FuturoNegocioActividades>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_FuturoDelNegocioEstrategiasDistribucion(_codProyecto)
                        select new FuturoNegocioActividades
                        {
                            Actividad = p.Actividad,
                            costo = p.Costo,
                            MesDeEjecucion = p.MesEjecucion,
                            RecursoRequerido = p.RecursosRequeridos,
                            Responsable = p.Responsable
                        }).ToList();
            }

            return list;
        }

        public FuturoNegocioPeriodoArranque infoFuturoNegocioPeriodoArranque(int _codProyecto)
        {
            FuturoNegocioPeriodoArranque list = new FuturoNegocioPeriodoArranque();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_FuturoDelNegocioPeriodoDeArranque(_codProyecto)
                        select new FuturoNegocioPeriodoArranque
                        {
                            PeriodoArranque = p.PeriodoArranque,
                            PeriodoImproductivo = p.PeriodoImproductivo
                        }).FirstOrDefault();
            }

            return list;
        }

        public Riesgos infoRiesgos(int _codProyecto)
        {
            Riesgos list = new Riesgos();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_Riesgos(_codProyecto)
                        select new Riesgos
                        {
                            ActoresExternosCriticos = p.ActoresExternos,
                            FactoresExternosAfectanOperacion = p.FactoresExternos
                        }).FirstOrDefault();
            }

            return list;
        }
        public ResumenEjecutivo infoResumenEjecutivo(int _codProyecto)
        {
            ResumenEjecutivo list = new ResumenEjecutivo();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_ResumenEjecutivo(_codProyecto)
                        select new ResumenEjecutivo
                        {
                            ConceptoNegocio = p.ConceptoDelNegocio,
                            contrapartidas = p.Contrapartidas ?? 0,
                            ejecucionPresupuestal = p.EjecucionPresupuestal ?? 0,
                            empleos = p.Empleos ?? 0,
                            IDH = p.IDH ?? 0,
                            mercadeo = p.Mercadeo ?? 0,
                            periodoImproductivo = p.PeriodoImproductivo,
                            RecursosAportadosEmprendedor = p.RecursosAportadosEmprendedor,
                            ventas = p.Ventas ?? 0,
                            VideoEmprendedor = p.VideoEmprendedor
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<PlanDeCompra> estructuraFinancieraPlanDeCompra(int _codProyecto)
        {
            List<PlanDeCompra> list = new List<PlanDeCompra>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_PlanDeCompras(_codProyecto)
                        select new PlanDeCompra
                        {
                            CantidadPresentacion = p.Cantidad + " " + p.Presentacion,
                            MargenDesperdicio = p.Desperdicio.ToString(),
                            MateriaPrima = p.nominsumo,
                            TipoMateriaPrima = p.NomTipoInsumo,
                            Unidad = p.Unidad
                        }).ToList();
            }

            return list;
        }

        public List<CostosDeProduccion> estructuraFinancieraTablaCostosProduccion(int _codProyecto)
        {
            List<CostosDeProduccion> list = new List<CostosDeProduccion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_TablaCostosDeProduccionEnPesos(_codProyecto)
                        select new CostosDeProduccion
                        {
                            Ano = p.AnioProyeccion ?? 0,
                            Valor = p.Ano.HasValue ? p.Ano.Value.ToString() : "0",
                            TipoInsumo = p.NombreInsumo
                        }).ToList();
            }

            return list;
        }

        public List<ProyeccionCompras> estructuraFinancieraTablaProyeccionComprasUnidades(int _codProyecto)
        {
            List<ProyeccionCompras> list = new List<ProyeccionCompras>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_TablaProyeccionDeComprarUnidades(_codProyecto)
                        select new ProyeccionCompras
                        {
                            ano = p.AnioProyeccion ?? 0,
                            Valor = p.Unidades.HasValue ? p.Unidades.Value.ToString() : "0",
                            TipoInsumo = p.NomTipoInsumo,
                            Insumo = p.NombreInsumo
                        }).ToList();
            }

            return list;
        }

        public List<ProyeccionCompras> estructuraFinancieraTablaProyeccionComprasPesos(int _codProyecto)
        {
            List<ProyeccionCompras> list = new List<ProyeccionCompras>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_TablaProyeccionDeComprarPesos(_codProyecto)
                        select new ProyeccionCompras
                        {
                            ano = p.AnioProyeccion ?? 0,
                            Valor = p.ComprasEnPesos.HasValue ? p.ComprasEnPesos.Value.ToString() : "0",
                            TipoInsumo = p.NomTipoInsumo,
                            Insumo = p.NombreInsumo
                        }).ToList();
            }

            return list;
        }

        public List<CostosAdministrativos> estructuraFinancieraGastosPuestaMarcha(int _codProyecto)
        {
            List<CostosAdministrativos> list = new List<CostosAdministrativos>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_CostosAdmnPuestaEnMarcha(_codProyecto)
                        select new CostosAdministrativos
                        {
                            Descripcion = p.Descripcion,
                            Valor = p.Valor.ToString()
                        }).ToList();
            }

            return list;
        }

        public List<CostosAdministrativos> estructuraFinancieraGastosAnuales(int _codProyecto)
        {
            List<CostosAdministrativos> list = new List<CostosAdministrativos>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_CostosAdmnGastoAnualesAdtvos(_codProyecto)
                        select new CostosAdministrativos
                        {
                            Descripcion = p.Descripcion,
                            Valor = p.Valor.ToString()
                        }).ToList();
            }

            return list;
        }

        public List<CostosAdministrativos> estructuraFinancieraAporteEmprendedores(int _codProyecto)
        {
            List<CostosAdministrativos> list = new List<CostosAdministrativos>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnAporteEmprend(_codProyecto)
                        select new CostosAdministrativos
                        {
                            Descripcion = p.Detalle,
                            Valor = p.Valor.ToString(),
                            Nombre = p.Nombre
                        }).ToList();
            }

            return list;
        }

        public string RecursosSolicitadoFondoEmprender(int _codProyecto)
        {
            string dato = "";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                dato = (from p in db.SPIA_RecursosSolicitadosProyecto(_codProyecto)
                        select p.recursos).FirstOrDefault().ToString();
            }

            return dato;
        }

        public List<RecursoCapital> estructuraFinancieraRecursoCapital(int _codProyecto)
        {
            List<RecursoCapital> list = new List<RecursoCapital>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnRecursoCapital(_codProyecto)
                        select new RecursoCapital
                        {
                            Cuantia = p.Cuantia.ToString(),
                            Destinacion = p.Destinacion,
                            FormaPago = p.Formapago,
                            Interes = p.Interes.HasValue ? p.Interes.Value.ToString() : "",
                            Plazo = p.Plazo,
                            Tipo = p.Tipo
                        }).ToList();
            }

            return list;
        }

        public List<DatoPorAno> estructuraFinancieraProyeccionIngresosVentas(int _codProyecto)
        {
            List<DatoPorAno> list = new List<DatoPorAno>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnProyeccionIngresosVentas(_codProyecto)
                        select new DatoPorAno
                        {
                            Dato = p.NomProducto,
                            ano_1 = p._1.HasValue ? p._1.Value.ToString() : "",
                            ano_2 = p._2.HasValue ? p._2.Value.ToString() : "",
                            ano_3 = p._3.HasValue ? p._3.Value.ToString() : "",
                            ano_4 = p._4.HasValue ? p._4.Value.ToString() : "",
                            ano_5 = p._5.HasValue ? p._5.Value.ToString() : "",
                            ano_6 = p._6.HasValue ? p._6.Value.ToString() : "",
                            ano_7 = p._7.HasValue ? p._7.Value.ToString() : "",
                            ano_8 = p._8.HasValue ? p._8.Value.ToString() : "",
                            ano_9 = p._9.HasValue ? p._9.Value.ToString() : "",
                            ano_10 = p._10.HasValue ? p._10.Value.ToString() : ""
                        }).ToList();
            }

            return list;
        }

        public string EstructuraFinancieraIndiceActuaMonetaria(int _codProyecto)
        {
            string dato = "";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                dato = (from p in db.SPIA_EstructuraFnIndiceActuaMonet(_codProyecto)
                        select p.ActualizacionMonetaria).FirstOrDefault().ToString();
            }

            return dato;
        }

        public List<Inversiones> estructuraFinancieraInversionesFijasDiferidas(int _codProyecto)
        {
            List<Inversiones> list = new List<Inversiones>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnInverFijayDife(_codProyecto)
                        select new Inversiones
                        {
                            Concepto = p.concepto,
                            FuenteFinanciacion = p.tipoFuente,
                            mes = p.mes.HasValue ? p.mes.Value.ToString() : "",
                            valor = p.valor.HasValue ? p.valor.Value.ToString() : "",
                        }).ToList();
            }

            return list;
        }

        public List<CostosAdministrativos> estructuraFinancieraCostoPuestaEnMarcha(int _codProyecto)
        {
            List<CostosAdministrativos> list = new List<CostosAdministrativos>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnCostosPuestaEnMar(_codProyecto)
                        select new CostosAdministrativos
                        {
                            Descripcion = p.Descripcion,
                            Valor = p.Valor.ToString()
                        }).ToList();
            }

            return list;
        }

        public DataTable estructuraFinancieraCostosAnualesAdmin(int _codProyecto)
        {
            int txtTiempoProyeccion;
            string txtActualizacionMonetaria;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                try
                {
                    var queryMonetaria = (from p in db.ProyectoFinanzasEgresos
                                          where p.CodProyecto == Convert.ToInt32(_codProyecto)
                                          select new { p.ActualizacionMonetaria }).First();
                    txtActualizacionMonetaria = queryMonetaria.ActualizacionMonetaria.ToString().Replace(',', '.');
                }
                catch
                {
                    txtActualizacionMonetaria = "1";
                }

                try
                {
                    var queryTiempoPro = (from p in db.ProyectoMercadoProyeccionVentas
                                          where p.CodProyecto == Convert.ToInt32(_codProyecto)
                                          select new { p.TiempoProyeccion }).First();
                    txtTiempoProyeccion = Convert.ToInt32(queryTiempoPro.TiempoProyeccion);
                }
                catch
                {
                    txtTiempoProyeccion = 3;
                }

                db.CommandTimeout = _TimeOut;

                var query = (from p in db.ProyectoGastos
                             where p.Tipo == "Anual"
                             && p.CodProyecto == Convert.ToInt32(_codProyecto)
                             orderby p.Descripcion ascending
                             select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

                DataTable datos = new DataTable();
                datos.Columns.Add("Descripcion");
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    datos.Columns.Add("Año " + i);
                }

                decimal[] total = new decimal[txtTiempoProyeccion + 1];
                foreach (var item in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["Descripcion"] = item.Descripcion;

                    decimal valor = item.Valor;
                    for (int i = 1; i <= txtTiempoProyeccion; i++)
                    {
                        dr["Año " + i] = valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                        total[i] += valor;
                        valor = Convert.ToDecimal(txtActualizacionMonetaria.Replace('.', ',')) * valor;
                    }
                    datos.Rows.Add(dr);
                }

                DataRow drTotal = datos.NewRow();
                drTotal["Descripcion"] = "Total";
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    drTotal["Año " + i] = total[i].ToString("0,0.00", CultureInfo.InvariantCulture);
                }
                datos.Rows.Add(drTotal);

                return datos;
            }
        }

        public DataTable estructuraFinancieraGastosDePersonal(int _codProyecto)
        {
            int txtTiempoProyeccion = 0;
            string txtActualizacionMonetaria;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;
                var query = (from p in db.ProyectoGastosPersonals
                             where p.CodProyecto == Convert.ToInt32(_codProyecto)
                             orderby p.Cargo ascending
                             select new { p.Cargo, p.ValorAnual, p.OtrosGastos, p.TiempoVinculacion, p.ValorRemuneracion });
                try
                {
                    var queryMonetaria = (from p in db.ProyectoFinanzasEgresos
                                          where p.CodProyecto == Convert.ToInt32(_codProyecto)
                                          select new { p.ActualizacionMonetaria }).First();
                    txtActualizacionMonetaria = queryMonetaria.ActualizacionMonetaria.ToString().Replace(',', '.');
                }
                catch
                {
                    txtActualizacionMonetaria = "1";
                }
                try
                {
                    var queryTiempoPro = (from p in db.ProyectoMercadoProyeccionVentas
                                          where p.CodProyecto == Convert.ToInt32(_codProyecto)
                                          select new { p.TiempoProyeccion }).First();
                    txtTiempoProyeccion = Convert.ToInt32(queryTiempoPro.TiempoProyeccion);
                }
                catch
                {
                    txtTiempoProyeccion = 3;
                }

                DataTable datos = new DataTable();
                datos.Columns.Add("Cargo");
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    datos.Columns.Add("Año " + i);
                }

                decimal[] total = new decimal[txtTiempoProyeccion + 1];
                foreach (var item in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["Cargo"] = item.Cargo;

                    decimal valor = (item.ValorRemuneracion.GetValueOrDefault(0) + item.OtrosGastos) * item.TiempoVinculacion.GetValueOrDefault(0);
                    for (int i = 1; i <= txtTiempoProyeccion; i++)
                    {
                        dr["Año " + i] = valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                        total[i] += valor;
                        valor = Convert.ToDecimal(txtActualizacionMonetaria.Replace('.', ',')) * valor;
                    }
                    datos.Rows.Add(dr);
                }

                DataRow drTotal = datos.NewRow();
                drTotal["Cargo"] = "Total";
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    drTotal["Año " + i] = total[i].ToString("0,0.00", CultureInfo.InvariantCulture);
                }
                datos.Rows.Add(drTotal);

                return datos;
            }
        }

        public List<CapitalTrabajo> estructuraFinancieraCapitalTrabajo(int _codProyecto)
        {
            List<CapitalTrabajo> list = new List<CapitalTrabajo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_EstructuraFnCapitalDeTrabajo(_codProyecto)
                        select new CapitalTrabajo
                        {
                            Componente = p.Componente,
                            FuenteFinanciacion = p.DescFuente,
                            Observacion = p.Observacion,
                            Valor = p.Valor.ToString()
                        }).ToList();
            }

            return list;
        }

        public List<CronogramaActividades> PlanOperativoActividades(int _codProyecto)
        {
            List<CronogramaActividades> list = new List<CronogramaActividades>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_PlanOperativoCronoActividades(_codProyecto)
                        select new CronogramaActividades
                        {
                            Item = p.item.ToString(),
                            Actividad = p.NomActividad,
                            FondoEmprenderMes_1 = p.Mes_1___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_1 = p.Mes_1___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_2 = p.Mes_2___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_2 = p.Mes_2___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_3 = p.Mes_3___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_3 = p.Mes_3___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_4 = p.Mes_4___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_4 = p.Mes_4___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_5 = p.Mes_5___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_5 = p.Mes_5___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_6 = p.Mes_6___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_6 = p.Mes_6___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_7 = p.Mes_7___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_7 = p.Mes_7___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_8 = p.Mes_8___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_8 = p.Mes_8___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_9 = p.Mes_9___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_9 = p.Mes_9___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_10 = p.Mes_10___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_10 = p.Mes_10___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_11 = p.Mes_11___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_11 = p.Mes_11___Aporte_Emprendedor.ToString(),
                            FondoEmprenderMes_12 = p.Mes_12___Fondo_Emprender.ToString(),
                            AporteEmprendedorMes_12 = p.Mes_12___Aporte_Emprendedor.ToString(),
                            AporteEmprendedor = p.AporteEmprendedor.ToString(),
                            FondoEmprender = p.FondoEmprender.ToString()
                        }).ToList();
            }

            return list;
        }

        public MetasSocialesPlanes PlanNacional_Regional_Cluster(int _codProyecto)
        {
            MetasSocialesPlanes dato = new MetasSocialesPlanes();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                dato = (from p in db.SPIA_PlanOperativoMetasSocialesPlan(_codProyecto)
                        select new MetasSocialesPlanes
                        {
                            Cluster = p.Cluster,
                            PlanNacional = p.PlanNacional,
                            PlanRegional = p.PlanRegional
                        }).FirstOrDefault();
            }

            return dato;
        }

        public List<MetasEmpleo> PlanOperativoEmpleos(int _codProyecto)
        {
            List<MetasEmpleo> list = new List<MetasEmpleo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_PlanOperativoEmpleo(_codProyecto)
                        select new MetasEmpleo
                        {
                            Cargo = p.Cargo,
                            EdadEntre1824Anos = p.EdadEntre18y24Años,
                            EsDesmovilizado = p.EsDesmovilizado,
                            EsDesplazado = p.EsDesplazado,
                            EsDesvinculado = p.EsDesvinculado,
                            EsDiscapacitado = p.EsDiscapacitado,
                            EsMadre = p.EsMadre,
                            EsMinoria = p.EsMinoria,
                            EsRecluido = p.EsRecluido,
                            generadoPrimerAno = p.GeneradoPrimerAnio,
                            TipoEmpleo = p.TipoDeEmpleoDirecto,
                            valorMensual = p.ValorMensual.HasValue ? p.ValorMensual.Value.ToString() : ""
                        }).ToList();
            }

            return list;
        }

        public GeneracionDeEmpleo PlanOperativoGenerarEmpleos(int _codProyecto)
        {
            GeneracionDeEmpleo dato = new GeneracionDeEmpleo();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                dato = (from p in db.SPIA_PlanOperativoEmpleoAGenerar(_codProyecto)
                        select new GeneracionDeEmpleo
                        {
                            EmpleosIndirectos = p.EmpleosIndirectos.HasValue ? p.EmpleosIndirectos.Value.ToString() : "",
                            GenerarPrimerAno = p.EmpleosAGenerarPrimerAño.HasValue ? p.EmpleosAGenerarPrimerAño.Value.ToString() : "",
                            GenerarTotalProyecto = p.EmpleosAGenerarTotalidadProyecto.HasValue ? p.EmpleosAGenerarTotalidadProyecto.Value.ToString() : ""
                        }).FirstOrDefault();
            }

            return dato;
        }

        public List<EmprendedoresPlanOperativo> PlanOperativoEmprendedores(int _codProyecto)
        {
            List<EmprendedoresPlanOperativo> list = new List<EmprendedoresPlanOperativo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIA_PlanOperativoParticipacionEmprendedores(_codProyecto)
                        select new EmprendedoresPlanOperativo
                        {
                            beneficiario = p.Beneficiario,
                            nombre = p.nombres,
                            participacion = p.Participacion.HasValue ? p.Participacion.Value.ToString():""
                        }).ToList();
            }

            return list;
        }

        public List<EvaluacionFinancieraIndicadores> evaluacionFinancieraIndicadores(int _codproyecto, int _codConvocatoria)
        {
            List<EvaluacionFinancieraIndicadores> evaluacionFinancieras = new List<EvaluacionFinancieraIndicadores>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionFinancieras = (from p in db.SPIAE_EvalFinanIndicadores(_codproyecto, _codConvocatoria)
                                         select new EvaluacionFinancieraIndicadores
                                         {
                                             Descripcion = p.Descripcion,
                                             Tipo = p.Tipo.ToString(),
                                             Valor = p.Tipo == '#' ? p.Valor.ToString("0.0000") :
                                                        (p.Tipo == '$' ? "$ "+p.Valor.ToString("0.0000") : 
                                                                p.Valor.ToString("0.0000")+" %")
                                         }).ToList(); 
            }

                return evaluacionFinancieras;
        }

        public List<EvaluacionCapitalEInversiones> evaluacionInversionesFijas(int _codproyecto, int _codConvocatoria)
        {
            List<EvaluacionCapitalEInversiones> evaluacionFinancieras = new List<EvaluacionCapitalEInversiones>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionFinancieras = (from p in db.SPIAE_ConceptoFinalAportesInverFijas(_codproyecto, _codConvocatoria)
                                         select new EvaluacionCapitalEInversiones
                                         {
                                             Detalle = p.Detalle,
                                             FuenteFinanciacion = p.DescFuente,
                                             Nombre = p.Nombre,
                                             PorcentajeRecomendado = p.PorcentajeRecomendado.HasValue ?
                                                                            p.PorcentajeRecomendado.Value.ToString() : "0",
                                             PorcentajeSolicitado = p.PorcentajeSolicitado.HasValue ?
                                                                            p.PorcentajeSolicitado.Value.ToString() : "0",
                                             TotalRecomendado = p.Recomendado.HasValue ?
                                                                        p.Recomendado.Value.ToString():"0",
                                             TotalSolicitado = p.Solicitado.ToString()

                                         }).ToList();
            }

            return evaluacionFinancieras;
        }

        public List<EvaluacionCapitalEInversiones> evaluacionCapitalTrabajoPrimerAno(int _codproyecto, int _codConvocatoria)
        {
            List<EvaluacionCapitalEInversiones> evaluacionFinancieras = new List<EvaluacionCapitalEInversiones>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionFinancieras = (from p in db.SPIAE_ConceptoFinalAportesCapTrabPrimerAnio(_codproyecto, _codConvocatoria)
                                         select new EvaluacionCapitalEInversiones
                                         {
                                             Detalle = p.Detalle,
                                             FuenteFinanciacion = p.FuentedeFinanciacion,
                                             Nombre = p.Nombre,
                                             PorcentajeRecomendado = p.PorcentajeRecomendado.ToString(),
                                             PorcentajeSolicitado = p.PorcentajeSolicitado.ToString(),
                                             TotalRecomendado = p.Recomendado.ToString(),
                                             TotalSolicitado = p.Solicitado.ToString()

                                         }).ToList();
            }

            return evaluacionFinancieras;
        }

        public List<EvaluacionCapitalEInversiones> evaluacionInversionesDiferidas(int _codproyecto, int _codConvocatoria)
        {
            List<EvaluacionCapitalEInversiones> evaluacionFinancieras = new List<EvaluacionCapitalEInversiones>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionFinancieras = (from p in db.SPIAE_ConceptoFinalAportesInverDiferidas(_codproyecto, _codConvocatoria)
                                         select new EvaluacionCapitalEInversiones
                                         {
                                             Detalle = p.Detalle,
                                             FuenteFinanciacion = p.DescFuente,
                                             Nombre = p.Nombre,
                                             PorcentajeRecomendado = p.PorcentajeRecomendado.ToString(),
                                             PorcentajeSolicitado = p.PorcentajeSolicitado.ToString(),
                                             TotalRecomendado = p.Recomendado.ToString(),
                                             TotalSolicitado = p.Solicitado.ToString()

                                         }).ToList();
            }

            return evaluacionFinancieras;
        }

        public List<EvaluacionIntegrantes> evaluacionIntegrantesIniciativa(int _codproyecto, int _codConvocatoria)
        {
            List<EvaluacionIntegrantes> evaluacionIntegrantes = new List<EvaluacionIntegrantes>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionIntegrantes = (from p in db.MD_GetIntegrantesIniciativa(_codproyecto.ToString(),_codConvocatoria.ToString())
                                         select new EvaluacionIntegrantes
                                         {
                                             Nombre = p.NomCompleto,
                                             Emprendedor = p.Beneficiario.HasValue ?
                                                            (p.Beneficiario.Value ? "Si": "No") : "",
                                             AporteDinero = p.AporteDinero.ToString(),
                                             AporteEspecie = p.AporteEspecie.ToString(),
                                             AporteTotal = sumaAporteTotal(p.AporteDinero,p.AporteEspecie,_codproyecto).ToString(),
                                             ClaseEspecie = p.DetalleEspecie,
                                             Otro =""
                                         }).ToList();
            }

            return evaluacionIntegrantes;
        }

        double sumaTotalAporte = 0;
        int codproyectoGlobal = 0;
        private double sumaAporteTotal(double aporteDinero, double aporteEspecie, int codproyecto)
        {
            if (codproyectoGlobal != codproyecto)
            {
                codproyectoGlobal = codproyecto;
                sumaTotalAporte = 0;
            }

            if(aporteDinero!=0 && aporteEspecie != 0)
            {
                sumaTotalAporte += ((aporteDinero + aporteEspecie) / Convert.ToDouble(1000));
            }

            return sumaTotalAporte;
        }

        public evaluacionObservacionAportes evaluacionObservacionAportes(int _codproyecto, int _codConvocatoria)
        {
            evaluacionObservacionAportes evaluacionObsAportes = new evaluacionObservacionAportes();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionObsAportes = (from p in db.SPIAE_ConceptoFinalAportesObservaciones(_codproyecto, _codConvocatoria)
                                         select new evaluacionObservacionAportes
                                         {
                                            EquipoTrabajo = p.EquipoTrabajo,
                                            RecursosSolicitados = p.RecursosSolicitados.ToString(),
                                            valorRecomendado = p.ValorRecomendado.HasValue ? 
                                                                p.ValorRecomendado.Value : 0
                                         }).FirstOrDefault();
            }

            return evaluacionObsAportes;
        }

        public List<riesgosEvaluacion> evaluacionRiesgos(int _codproyecto, int _codConvocatoria)
        {
            List<riesgosEvaluacion> evaluacionRiesgo = new List<riesgosEvaluacion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionRiesgo = (from p in db.SPIAE_ConceptoFinalRiesgos(_codproyecto,_codConvocatoria)
                                         select new riesgosEvaluacion
                                         {
                                             Mitigacion = p.Mitigacion,
                                             Riesgo = p.Riesgo
                                         }).ToList();
            }

            return evaluacionRiesgo;
        }

        public conceptoFinalEvaluacion evaluacionConceptoFinal(int _codproyecto, int _codConvocatoria)
        {
            conceptoFinalEvaluacion evaluacionObsAportes = new conceptoFinalEvaluacion();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                evaluacionObsAportes = (from p in db.SPIAE_ConceptoFinalConclusion(_codproyecto, _codConvocatoria)
                                        select new conceptoFinalEvaluacion
                                        {
                                            Conceptos = p.ConceptoDeJustificacion,
                                            Justificacion = p.Justificacion,
                                            Viable = p.Viable
                                        }).FirstOrDefault();
            }

            return evaluacionObsAportes;
        }

        public evaluacionIndicadoresGestion evaluacionIndicadorGestion(int _codproyecto, int _codConvocatoria)
        {
            evaluacionIndicadoresGestion eval = new evaluacionIndicadoresGestion();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                eval = (from p in db.SPIAE_IndicadoresDeGestion(_codproyecto, _codConvocatoria)
                                        select new evaluacionIndicadoresGestion
                                        {
                                            AportesEmprendedorEmprededor = p.RecursosAportadosEmprendedor,
                                            AportesEmprendedorEvaluador = p.RecursosAportadorEmprendedorEvaluador,
                                            ContrapartidaEmprendedor = p.ContrapartidasEmprendedor,
                                            ContrapartidaEvaluador = p.ContrapartidasEvaluador,
                                            EjecucionPresupuestalEmprendedor = p.EjecuciónPresupuestalEmprendedor,
                                            EjecucionPresupuestalEvaluador = p.EjecuciónPresupuestalEvaluador,
                                            IDHEmprendedor = p.IDHEmprendedor.HasValue ?
                                                               p.IDHEmprendedor.Value.ToString() : "" ,
                                            IDHEvaluador = p.IDHEvaluador.HasValue ? 
                                                               p.IDHEvaluador.Value.ToString() : "" ,
                                            PeriodoemproductivoEmprededor = p.PeriodoImproductivoEmprendedor.ToString(),
                                            PeriodoemproductivoEvaluador = p.PeriodoImproductivoEvaluador.ToString(),
                                            VentasEmprendedor = p.VentasEmprendedor.HasValue ? 
                                                                 p.VentasEmprendedor.Value.ToString() : "" ,
                                            VentasEvaluador = p.VentasEvaluador.ToString()
                                        }).FirstOrDefault();
            }

            return eval;
        }

        public List<evalIndicadorDeGestionProduccion> evaluacionIndicadorGestionProduccion(int _codproyecto, int _codConvocatoria)
        {
            List<evalIndicadorDeGestionProduccion> indicador = new List<evalIndicadorDeGestionProduccion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAE_IndicadoresDeGestionProduccion(_codproyecto, _codConvocatoria)
                                    select new evalIndicadorDeGestionProduccion
                                    {
                                       DatosEvaluador = p.UnidadesEvaluador.ToString(),
                                       Unidades = p.Unidades,
                                       Producto = p.Producto,
                                       ProductoRepresentativo = p.ProductoMasRepresentativo,
                                       ProductoRepresentativoEvaluacion = p.ProductoMasRepresentativoEvaluador
                                    }).ToList();
            }

            return indicador;
        }

        public List<evalIndicadorGestionMercadeoEmpleo> evaluacionIndicadorGestionMercadeo(int _codproyecto, int _codConvocatoria)
        {
            List<evalIndicadorGestionMercadeoEmpleo> indicador = new List<evalIndicadorGestionMercadeoEmpleo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {

                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAE_IndicadoresDeGestionMercadeo(_codproyecto, _codConvocatoria)
                             select new evalIndicadorGestionMercadeoEmpleo
                             {
                                 Actividad_Cargo = p.Actividad,
                                 DatosEvaluador = p.DatosEvaluador.ToString(),
                                 Cantidad = p.Cantidad
                             }).ToList();
            }

            return indicador;
        }

        public List<evalIndicadorGestionMercadeoEmpleo> evaluacionIndicadorGestionEmpleo(int _codproyecto, int _codConvocatoria)
        {
            List<evalIndicadorGestionMercadeoEmpleo> indicador = new List<evalIndicadorGestionMercadeoEmpleo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAE_IndicadoresDeGestionEmpleos(_codproyecto, _codConvocatoria)
                             select new evalIndicadorGestionMercadeoEmpleo
                             {
                                 Actividad_Cargo = p.Cargo,
                                 DatosEvaluador = p.DatosEvaluador.ToString(),
                                 Cantidad = p.Cantidad
                             }).ToList();
            }

            return indicador;
        }

        public List<ActividadesPlanOperativo> actividadesPlanOperativoXProyecto(int _codproyecto)
        {
            List<ActividadesPlanOperativo> indicador = new List<ActividadesPlanOperativo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_ActividadesPlanOperativoEjecucionMesXMes(_codproyecto)
                             select new ActividadesPlanOperativo
                             {
                                 codProyecto = _codproyecto,
                                 Actividad = p.NomActividad,
                                 Item = p.item.ToString()
                             }).ToList();
            }

            return indicador;
        }

        public List<CronogramaActividades> resumenMesXMesActividades(int _codproyecto)
        {
            List<CronogramaActividades> indicador = new List<CronogramaActividades>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_ActividadesPlanOperativoEjecucionMesXMes(_codproyecto)
                             select new CronogramaActividades
                             {                                  
                                 Actividad = p.NomActividad,
                                 Item = p.item.ToString(),
                                 AporteEmprendedorMes_1 = p.Mes_1___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_1 = p.Mes_1___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_2 = p.Mes_2___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_2 = p.Mes_2___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_3 = p.Mes_3___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_3 = p.Mes_3___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_4 = p.Mes_4___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_4 = p.Mes_4___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_5 = p.Mes_5___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_5 = p.Mes_5___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_6 = p.Mes_6___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_6 = p.Mes_6___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_7 = p.Mes_7___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_7 = p.Mes_7___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_8 = p.Mes_8___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_8 = p.Mes_8___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_9 = p.Mes_9___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_9 = p.Mes_9___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_10 = p.Mes_10___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_10 = p.Mes_10___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_11 = p.Mes_11___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_11 = p.Mes_11___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_12 = p.Mes_12___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_12 = p.Mes_12___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_13 = p.Mes_13___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_13 = p.Mes_13___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_14 = p.Mes_14___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_14 = p.Mes_14___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_15 = p.Mes_15___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_15 = p.Mes_15___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_16 = p.Mes_16___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_16 = p.Mes_16___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_17 = p.Mes_17___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_17 = p.Mes_17___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_18 = p.Mes_18___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_18 = p.Mes_18___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_19 = p.Mes_19___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_19 = p.Mes_19___Fondo_Emprender.ToString(),
                                 AporteEmprendedorMes_20 = p.Mes_20___Aporte_Emprendedor.ToString(),
                                 FondoEmprenderMes_20 = p.Mes_20___Fondo_Emprender.ToString(),
                                 AporteEmprendedor = p.AporteEmprendedor.ToString(),
                                 FondoEmprender = p.FondoEmprender.ToString()
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> avanceMesXMesActividades(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceMesAMesPorActividad(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = p.IdPlan,
                                 Actividad = p.Actividad,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvance.HasValue ? p.FechaAvance.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionesEmprendedor,
                                 FechaAprobacion = p.FechaAprobacion.HasValue ? p.FechaAprobacion.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionesInterventor,
                                 ActividaAprobada = p.ActividadAprobada
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> historicoAvanceActividades(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceHistoricoPlanOperativo(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.NomActividad,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvanceEmprendedor.HasValue ? p.FechaAvanceEmprendedor.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAvanceInterventor.HasValue ? p.FechaAvanceInterventor.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.Aprobado,
                                 ValorAporteEmprendedor = p.ValorAporteEmprendedor.HasValue ? p.ValorAporteEmprendedor.Value.ToString() : "0",
                                 ValorFondoEmprender = p.ValorFondoEmprender.HasValue ? p.ValorFondoEmprender.Value.ToString() : "0",
                                 RegistradoPor = p.RegistradoPor
                             }).ToList();
            }

            return indicador;
        }

        public List<CronogramaNominaCargos> nominaMesxMesxCargo(int _codproyecto)
        {
            List<CronogramaNominaCargos> indicador = new List<CronogramaNominaCargos>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_ActividadesNominaEjecucionMesXMes(_codproyecto)
                             select new CronogramaNominaCargos
                             {
                                 Cargo = p.Cargo,
                                 Item = p.Id_Nomina.ToString(),
                                 SueldoMes_1 = p.Mes_1___Sueldo.ToString(),
                                 PrestacionesMes_1 = p.Mes_1___Prestaciones.ToString(),
                                 SueldoMes_2 = p.Mes_2___Sueldo.ToString(),
                                 PrestacionesMes_2 = p.Mes_2___Prestaciones.ToString(),
                                 SueldoMes_3 = p.Mes_3___Sueldo.ToString(),
                                 PrestacionesMes_3 = p.Mes_3___Prestaciones.ToString(),
                                 SueldoMes_4 = p.Mes_4___Sueldo.ToString(),
                                 PrestacionesMes_4 = p.Mes_4___Prestaciones.ToString(),
                                 SueldoMes_5 = p.Mes_5___Sueldo.ToString(),
                                 PrestacionesMes_5 = p.Mes_5___Prestaciones.ToString(),
                                 SueldoMes_6 = p.Mes_6___Sueldo.ToString(),
                                 PrestacionesMes_6 = p.Mes_6___Prestaciones.ToString(),
                                 SueldoMes_7 = p.Mes_7___Sueldo.ToString(),
                                 PrestacionesMes_7 = p.Mes_7___Prestaciones.ToString(),
                                 SueldoMes_8 = p.Mes_8___Sueldo.ToString(),
                                 PrestacionesMes_8 = p.Mes_8___Prestaciones.ToString(),
                                 SueldoMes_9 = p.Mes_9___Sueldo.ToString(),
                                 PrestacionesMes_9 = p.Mes_9___Prestaciones.ToString(),
                                 SueldoMes_10 = p.Mes_10___Sueldo.ToString(),
                                 PrestacionesMes_10 = p.Mes_10___Prestaciones.ToString(),
                                 SueldoMes_11 = p.Mes_11___Sueldo.ToString(),
                                 PrestacionesMes_11 = p.Mes_11___Prestaciones.ToString(),
                                 SueldoMes_12 = p.Mes_12___Sueldo.ToString(),
                                 PrestacionesMes_12 = p.Mes_12___Prestaciones.ToString(),
                                 SueldoMes_13 = p.Mes_13___Sueldo.ToString(),
                                 PrestacionesMes_13 = p.Mes_13___Prestaciones.ToString(),
                                 SueldoMes_14 = p.Mes_14___Sueldo.ToString(),
                                 PrestacionesMes_14 = p.Mes_14___Prestaciones.ToString(),
                                 SueldoMes_15 = p.Mes_15___Sueldo.ToString(),
                                 PrestacionesMes_15 = p.Mes_15___Prestaciones.ToString(),
                                 SueldoMes_16 = p.Mes_16___Sueldo.ToString(),
                                 PrestacionesMes_16 = p.Mes_16___Prestaciones.ToString(),
                                 SueldoMes_17 = p.Mes_17___Sueldo.ToString(),
                                 PrestacionesMes_17 = p.Mes_17___Prestaciones.ToString(),
                                 SueldoMes_18 = p.Mes_18___Sueldo.ToString(),
                                 PrestacionesMes_18 = p.Mes_18___Prestaciones.ToString(),
                                 SueldoMes_19 = p.Mes_19___Sueldo.ToString(),
                                 PrestacionesMes_19 = p.Mes_19___Prestaciones.ToString(),
                                 SueldoMes_20 = p.Mes_20___Sueldo.ToString(),
                                 PrestacionesMes_20 = p.Mes_20___Prestaciones.ToString(),
                                 SueldoTotal = p.SueldoTotal.ToString(),
                                 PrestacionesTotal = p.PrestacionesTotal.ToString(),
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> AvanceNominaMesxMes(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvancesNominaMesXMes(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.Cargo,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvance.HasValue ? p.FechaAvance.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAprobacion.HasValue ? p.FechaAprobacion.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.ActividadAprobada
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> AvanceProduccionMesxMes(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvancesProduccionMesXMes(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.Producto,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvance.HasValue ? p.FechaAvance.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAprobacion.HasValue ? p.FechaAprobacion.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.ActividadAprobada
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> AvanceVentasMesxMes(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceMesAMesPorVentas(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.Producto,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvance.HasValue ? p.FechaAvance.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionesEmprendedor,
                                 FechaAprobacion = p.FechaAprobacion.HasValue ? p.FechaAprobacion.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionesInterventor,
                                 ActividaAprobada = p.ActividadAprobada
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> historicoAvanceNomina(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceHistoricoNomina(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.Cargo,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvanceEmprendedor.HasValue ? p.FechaAvanceEmprendedor.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAvanceInterventor.HasValue ? p.FechaAvanceInterventor.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.Aprobado,
                                 ValorAporteEmprendedor = p.valorSueldo.HasValue ? p.valorSueldo.Value.ToString() : "0",
                                 ValorFondoEmprender = p.valorPrestaciones.HasValue ? p.valorPrestaciones.Value.ToString() : "0",
                                 RegistradoPor = p.NombreCompleto
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> historicoAvanceProduccion(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceHistoricoProduccion(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.NomProducto,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvanceEmprendedor.HasValue ? p.FechaAvanceEmprendedor.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAvanceInterventor.HasValue ? p.FechaAvanceInterventor.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.Aprobado,
                                 ValorAporteEmprendedor = p.Cantidad.HasValue ? p.Cantidad.Value.ToString() : "0",
                                 ValorFondoEmprender = p.Costo.HasValue ? p.Costo.Value.ToString() : "0",
                                 RegistradoPor = p.NombreCompleto
                             }).ToList();
            }

            return indicador;
        }

        public List<AvanceMesxMesxProyecto> historicoAvanceVentas(int _codproyecto)
        {
            List<AvanceMesxMesxProyecto> indicador = new List<AvanceMesxMesxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_AvanceHistoricoVentas(_codproyecto)
                             select new AvanceMesxMesxProyecto
                             {
                                 idPlan = _codproyecto,
                                 Actividad = p.NomProducto,
                                 mes = p.Mes,
                                 FechaAvance = p.FechaAvanceEmprendedor.HasValue ? p.FechaAvanceEmprendedor.Value.ToString() : "",
                                 ObservacionesEmprendedor = p.ObservacionEmprendedor,
                                 FechaAprobacion = p.FechaAvanceInterventor.HasValue ? p.FechaAvanceInterventor.Value.ToString() : "",
                                 ObservacionesInterventor = p.ObservacionInterventor,
                                 ActividaAprobada = p.Aprobado,
                                 ValorAporteEmprendedor = p.Ventas.HasValue ? p.Ventas.Value.ToString() : "0",
                                 ValorFondoEmprender = p.Ingreso.HasValue ? p.Ingreso.Value.ToString() : "0",
                                 RegistradoPor = p.NombreCompleto
                             }).ToList();
            }

            return indicador;
        }

        public List<CronogramaProduccion> ProduccionMesxMes(int _codproyecto)
        {
            List<CronogramaProduccion> indicador = new List<CronogramaProduccion>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_ActividadesProduccionEjecucionMesXMes(_codproyecto)
                             select new CronogramaProduccion
                             {                                 
                                 Producto = p.NomProducto,
                                 CantidadMes_1 = p.Mes_1___Cantidad.ToString(),
                                 CostoMes_1 = p.Mes_1___Costo.ToString(),
                                 CantidadMes_2 = p.Mes_2___Cantidad.ToString(),
                                 CostoMes_2 = p.Mes_2___Costo.ToString(),
                                 CantidadMes_3 = p.Mes_3___Cantidad.ToString(),
                                 CostoMes_3 = p.Mes_3___Costo.ToString(),
                                 CantidadMes_4 = p.Mes_4___Cantidad.ToString(),
                                 CostoMes_4 = p.Mes_4___Costo.ToString(),
                                 CantidadMes_5 = p.Mes_5___Cantidad.ToString(),
                                 CostoMes_5 = p.Mes_5___Costo.ToString(),
                                 CantidadMes_6 = p.Mes_6___Cantidad.ToString(),
                                 CostoMes_6 = p.Mes_6___Costo.ToString(),
                                 CantidadMes_7 = p.Mes_7___Cantidad.ToString(),
                                 CostoMes_7 = p.Mes_7___Costo.ToString(),
                                 CantidadMes_8 = p.Mes_8___Cantidad.ToString(),
                                 CostoMes_8 = p.Mes_8___Costo.ToString(),
                                 CantidadMes_9 = p.Mes_9___Cantidad.ToString(),
                                 CostoMes_9 = p.Mes_9___Costo.ToString(),
                                 CantidadMes_10 = p.Mes_10___Cantidad.ToString(),
                                 CostoMes_10 = p.Mes_10___Costo.ToString(),
                                 CantidadMes_11 = p.Mes_11___Cantidad.ToString(),
                                 CostoMes_11 = p.Mes_11___Costo.ToString(),
                                 CantidadMes_12 = p.Mes_12___Cantidad.ToString(),
                                 CostoMes_12 = p.Mes_12___Costo.ToString(),
                                 CantidadMes_13 = p.Mes_13___Cantidad.ToString(),
                                 CostoMes_13 = p.Mes_13___Costo.ToString(),
                                 CantidadMes_14 = p.Mes_14___Cantidad.ToString(),
                                 CostoMes_14 = p.Mes_14___Costo.ToString(),
                                 CantidadMes_15 = p.Mes_15___Cantidad.ToString(),
                                 CostoMes_15 = p.Mes_15___Costo.ToString(),
                                 CantidadMes_16 = p.Mes_16___Cantidad.ToString(),
                                 CostoMes_16 = p.Mes_16___Costo.ToString(),
                                 CantidadMes_17 = p.Mes_17___Cantidad.ToString(),
                                 CostoMes_17 = p.Mes_17___Costo.ToString(),
                                 CantidadMes_18 = p.Mes_18___Cantidad.ToString(),
                                 CostoMes_18 = p.Mes_18___Costo.ToString(),
                                 CantidadMes_19 = p.Mes_19___Cantidad.ToString(),
                                 CostoMes_19 = p.Mes_19___Costo.ToString(),
                                 CantidadMes_20 = p.Mes_20___Cantidad.ToString(),
                                 CostoMes_20 = p.Mes_20___Costo.ToString(),
                                 CantidadTotal = p.CantidadTotal.ToString(),
                                 CostoTotal = p.CostoTotal.ToString(),
                             }).ToList();
            }

            return indicador;
        }

        public List<CronogramaVentas> VentasMesxMes(int _codproyecto)
        {
            List<CronogramaVentas> indicador = new List<CronogramaVentas>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                indicador = (from p in db.SPIAI_ActividadesVentasEjecucionMesXMes(_codproyecto)
                             select new CronogramaVentas
                             {
                                 Producto = p.NomProducto,
                                 VentasMes_1 = p.Mes_1___Ventas.ToString(),
                                 IngresoMes_1 = p.Mes_1___Ingreso.ToString(),
                                 VentasMes_2 = p.Mes_2___Ventas.ToString(),
                                 IngresoMes_2 = p.Mes_2___Ingreso.ToString(),
                                 VentasMes_3 = p.Mes_3___Ventas.ToString(),
                                 IngresoMes_3 = p.Mes_3___Ingreso.ToString(),
                                 VentasMes_4 = p.Mes_4___Ventas.ToString(),
                                 IngresoMes_4 = p.Mes_4___Ingreso.ToString(),
                                 VentasMes_5 = p.Mes_5___Ventas.ToString(),
                                 IngresoMes_5 = p.Mes_5___Ingreso.ToString(),
                                 VentasMes_6 = p.Mes_6___Ventas.ToString(),
                                 IngresoMes_6 = p.Mes_6___Ingreso.ToString(),
                                 VentasMes_7 = p.Mes_7___Ventas.ToString(),
                                 IngresoMes_7 = p.Mes_7___Ingreso.ToString(),
                                 VentasMes_8 = p.Mes_8___Ventas.ToString(),
                                 IngresoMes_8 = p.Mes_8___Ingreso.ToString(),
                                 VentasMes_9 = p.Mes_9___Ventas.ToString(),
                                 IngresoMes_9 = p.Mes_9___Ingreso.ToString(),
                                 VentasMes_10 = p.Mes_10___Ventas.ToString(),
                                 IngresoMes_10 = p.Mes_10___Ingreso.ToString(),
                                 VentasMes_11 = p.Mes_11___Ventas.ToString(),
                                 IngresoMes_11 = p.Mes_11___Ingreso.ToString(),
                                 VentasMes_12 = p.Mes_12___Ventas.ToString(),
                                 IngresoMes_12 = p.Mes_12___Ingreso.ToString(),
                                 VentasMes_13 = p.Mes_13___Ventas.ToString(),
                                 IngresoMes_13 = p.Mes_13___Ingreso.ToString(),
                                 VentasMes_14 = p.Mes_14___Ventas.ToString(),
                                 IngresoMes_14 = p.Mes_14___Ingreso.ToString(),
                                 VentasMes_15 = p.Mes_15___Ventas.ToString(),
                                 IngresoMes_15 = p.Mes_15___Ingreso.ToString(),
                                 VentasMes_16 = p.Mes_16___Ventas.ToString(),
                                 IngresoMes_16 = p.Mes_16___Ingreso.ToString(),
                                 VentasMes_17 = p.Mes_17___Ventas.ToString(),
                                 IngresoMes_17 = p.Mes_17___Ingreso.ToString(),
                                 VentasMes_18 = p.Mes_18___Ventas.ToString(),
                                 IngresoMes_18 = p.Mes_18___Ingreso.ToString(),
                                 VentasMes_19 = p.Mes_19___Ventas.ToString(),
                                 IngresoMes_19 = p.Mes_19___Ingreso.ToString(),
                                 VentasMes_20 = p.Mes_20___Ventas.ToString(),
                                 IngresoMes_20 = p.Mes_20___Ingreso.ToString(),
                                 VentasTotal = p.FondoEmprender.ToString(),
                                 IngresoTotal = p.AporteEmprendedor.ToString(),
                             }).ToList();
            }

            return indicador;
        }

        public List<indicadoresGestionxProyecto> GetIndicadoresGestionxProyectos(int idProyecto)
        {
            List<indicadoresGestionxProyecto> list = new List<indicadoresGestionxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_IndicadoresGenericos(idProyecto)
                             select new indicadoresGestionxProyecto
                             {
                                 IdPlan = idProyecto,
                                 Decripcion = p.Descripcion,
                                 Evaluacion = p.Evaluacion,
                                 Nombre = p.NombreIndicador,
                                 Observacion = p.Observacion
                             }).ToList();
            }

            return list;
        }

        public List<riesgosInternventorxProyecto> GetRiesgosInterventorxProyecto(int idProyecto)
        {
            List<riesgosInternventorxProyecto> list = new List<riesgosInternventorxProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_RiesgosInterventor(idProyecto)
                        select new riesgosInternventorxProyecto
                        {
                            IdPlan = idProyecto,
                            EjeFuncional = p.NomEjeFuncional,
                            Mitigacion = p.Mitigacion,
                            Riesgo = p.Riesgo,
                            Observacion = p.Observacion
                        }).ToList();
            }

            return list;
        }

        public ConceptoFinalRecomendaciones GetConceptoFinalRecomendaciones(int idProyecto)
        {
            ConceptoFinalRecomendaciones list = new ConceptoFinalRecomendaciones();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_ConceptoFinalYRecomendaciones(idProyecto)
                        select new ConceptoFinalRecomendaciones
                        {
                            IdPlan = idProyecto,
                            Observaciones = p.ObservacionesInterventor,
                            DificultadCentral = p.DificultadCentral
                        }).FirstOrDefault();
            }

            return list;
        }

        public contratoProyectoInfo GetContratoInterventoria(int idProyecto)
        {
            contratoProyectoInfo list = new contratoProyectoInfo();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_ContratoInterventoria(idProyecto)
                        select new contratoProyectoInfo
                        {
                            IdPlan = idProyecto,
                            CompaniaSeguroDeVida = p.CompaniaSeguros,
                            FechaActaDeInicio = p.FechaDeInicioContrato.HasValue ? 
                                                p.FechaDeInicioContrato.Value.ToString() : "",
                            FechaDelAp = p.FechaAP.HasValue ? p.FechaAP.Value.ToString() : "",
                            FechaFirmaDelContrato = p.FechaFirmaDelContrato.HasValue ? 
                                                    p.FechaFirmaDelContrato.Value.ToString() : "",
                            NoPolizaDeSeguroDeVida = p.NumeroPoliza,
                            NumeroDeContrato = p.NumeroContrato,
                            NumeroDelApPresupuestal = p.NumeroAPContrato.HasValue ?
                                                        p.NumeroAPContrato.Value.ToString() : "",
                            Objeto = p.ObjetoContrato,
                            Plazo = p.PlazoContratoMeses.HasValue ? 
                                        p.PlazoContratoMeses.ToString() : "",
                            ValorInicial = p.ValorInicialEnPesos.HasValue ? 
                                            p.ValorInicialEnPesos.Value.ToString() : ""
                            
                        }).FirstOrDefault();
            }

            return list;
        }

        public RegistroMercantilInfo GetRegistroMercantilxProyecto(int idProyecto)
        {
            RegistroMercantilInfo list = new RegistroMercantilInfo();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_RegistroMercantil(idProyecto)
                        select new RegistroMercantilInfo
                        {
                            IdPlan = idProyecto,
                            NombrePlan = p.NomProyecto,
                            RazonSocial = p.razonsocial,
                            ObjetoSocial = p.ObjetoSocial,
                            CapitalSocial = p.CapitalSocial.HasValue ? p.CapitalSocial.Value.ToString() : "0",
                            TipoSociedad = p.NomTipoSociedad,
                            CodigoCIIU = p.codigoCIIU,
                            NumeroEscrituraPublica = p.NumEscrituraPublica,
                            DomicilioEmpresa = p.DomicilioEmpresa,
                            Departamento = p.NomDepartamento,
                            Ciudad = p.NomCiudad,
                            Telefono = p.Telefono,
                            Email = p.Email,
                            Nit = p.Nit,
                            EsRegimenEspecial = p.RegimenEspecial,
                            NormaRegimenEspecial = p.RENorma,
                            FechaNormaRegimenEspecial = p.REFechaNorma.HasValue ? p.REFechaNorma.Value.ToString() : "",
                            EsContribuyente = p.Contribuyente,
                            NormaContribuyente = p.CNorma,
                            FechaNormaContribuyente = p.CFechaNorma.HasValue ? p.CFechaNorma.Value.ToString() : "",
                            EsAutoretenedor = p.AutoRetenedor,
                            NormaAutoretenedor = p.ARNorma,
                            FechaNormaAutoretenedor = p.ARFechaNorma.HasValue ? p.ARFechaNorma.Value.ToString() : "",
                            EsDeclarante = p.Declarante,
                            NormaDeclarante = p.DNorma,
                            FechaNormaDeclarante = p.DFechaNorma.HasValue ?p.DFechaNorma.Value.ToString() : "",
                            EsExentoDeRetencion = p.ExentoReteFuente,
                            NormaExentoDeRetencion = p.ERFNorma,
                            FechaNormaExentoDeRetencion = p.ERFFechaNorma.HasValue ? p.ERFFechaNorma.Value.ToString() : "",
                            EsGranContribuyente = p.GranContribuyente,
                            NormaGranContribuyente = p.GCNorma,
                            FechaNormaGranContribuyente = p.GCFechaNorma.HasValue ? p.GCFechaNorma.Value.ToString() : ""
                        }).FirstOrDefault();
            }

            return list;
        }

        public List<SociosInfo> GetSociosxProyecto(int idProyecto)
        {
            List<SociosInfo> list = new List<SociosInfo>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                db.CommandTimeout = _TimeOut;

                list = (from p in db.SPIAI_ListadoEmprendedoresSocios(idProyecto)
                        select new SociosInfo
                        {
                            IdPlan = idProyecto,
                            Nombre = p.NombreCompleto,
                            Email = p.Email,
                            Identificacion = p.Identificacion.ToString(),
                            Telefono = p.Telefono
                        }).ToList();
            }

            return list;
        }
    }

    public class SociosInfo
    {
        public int IdPlan { get; set; }
        private string nombre;
        private string identificacion;
        private string email;
        private string telefono;
        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
        public string Identificacion
        {
            get { return this.identificacion.quitarCaracteres(); }
            set { this.identificacion = value; }
        }
        public string Email
        {
            get { return this.email.quitarCaracteres(); }
            set { this.email = value; }
        }
        public string Telefono
        {
            get { return this.telefono.quitarCaracteres(); }
            set { this.telefono = value; }
        }
    }

    public class RegistroMercantilInfo
    {
        public int IdPlan { get; set; }
        private string nombrePlan;
        private string razonSocial;
        private string objetoSocial;
        private string capitalSocial;
        private string tipoSociedad;
        private string codigoCIIU;
        private string numeroEscrituraPublica;
        private string domicilioEmpresa;
        private string departamento;
        private string ciudad;
        private string telefono;
        private string email;
        private string nit;
        private string esRegimenEspecial;
        private string normaRegimenEspecial;
        private string fechaNormaRegimenEspecial;
        private string esContribuyente;
        private string normaContribuyente;
        private string fechaNormaContribuyente;
        private string esAutoretenedor;
        private string normaAutoretenedor;
        private string fechaNormaAutoretenedor;
        private string esDeclarante;
        private string normaDeclarante;
        private string fechaNormaDeclarante;
        private string esExentoDeRetencion;
        private string normaExentoDeRetencion;
        private string fechaNormaExentoDeRetencion;
        private string esGranContribuyente;
        private string normaGranContribuyente;
        private string fechaNormaGranContribuyente;

        public string NombrePlan
        {
            get { return this.nombrePlan.quitarCaracteres(); }
            set { this.nombrePlan = value; }
        }
        public string RazonSocial
        {
            get { return this.razonSocial.quitarCaracteres(); }
            set { this.razonSocial = value; }
        }
        public string ObjetoSocial
        {
            get { return this.objetoSocial.quitarCaracteres(); }
            set { this.objetoSocial = value; }
        }
        public string CapitalSocial
        {
            get { return this.capitalSocial.quitarCaracteres(); }
            set { this.capitalSocial = value; }
        }
        public string TipoSociedad
        {
            get { return this.tipoSociedad.quitarCaracteres(); }
            set { this.tipoSociedad = value; }
        }
        public string CodigoCIIU
        {
            get { return this.codigoCIIU.quitarCaracteres(); }
            set { this.codigoCIIU = value; }
        }
        public string NumeroEscrituraPublica
        {
            get { return this.numeroEscrituraPublica.quitarCaracteres(); }
            set { this.numeroEscrituraPublica = value; }
        }
        public string DomicilioEmpresa
        {
            get { return this.domicilioEmpresa.quitarCaracteres(); }
            set { this.domicilioEmpresa = value; }
        }
        public string Departamento
        {
            get { return this.departamento.quitarCaracteres(); }
            set { this.departamento = value; }
        }
        public string Ciudad
        {
            get { return this.ciudad.quitarCaracteres(); }
            set { this.ciudad = value; }
        }
        public string Telefono
        {
            get { return this.telefono.quitarCaracteres(); }
            set { this.telefono = value; }
        }
        public string Email
        {
            get { return this.email.quitarCaracteres(); }
            set { this.email = value; }
        }
        public string Nit
        {
            get { return this.nit.quitarCaracteres(); }
            set { this.nit = value; }
        }
        public string EsRegimenEspecial
        {
            get { return this.esRegimenEspecial.quitarCaracteres(); }
            set { this.esRegimenEspecial = value; }
        }
        public string NormaRegimenEspecial
        {
            get { return this.normaRegimenEspecial.quitarCaracteres(); }
            set { this.normaRegimenEspecial = value; }
        }
        public string FechaNormaRegimenEspecial
        {
            get { return this.fechaNormaRegimenEspecial.quitarCaracteres(); }
            set { this.fechaNormaRegimenEspecial = value; }
        }
        public string EsContribuyente
        {
            get { return this.esContribuyente.quitarCaracteres(); }
            set { this.esContribuyente = value; }
        }
        public string NormaContribuyente
        {
            get { return this.normaContribuyente.quitarCaracteres(); }
            set { this.normaContribuyente = value; }
        }
        public string FechaNormaContribuyente
        {
            get { return this.fechaNormaContribuyente.quitarCaracteres(); }
            set { this.fechaNormaContribuyente = value; }
        }
        public string EsAutoretenedor
        {
            get { return this.esAutoretenedor.quitarCaracteres(); }
            set { this.esAutoretenedor = value; }
        }
        public string NormaAutoretenedor
        {
            get { return this.normaAutoretenedor.quitarCaracteres(); }
            set { this.normaAutoretenedor = value; }
        }
        public string FechaNormaAutoretenedor
        {
            get { return this.fechaNormaAutoretenedor.quitarCaracteres(); }
            set { this.fechaNormaAutoretenedor = value; }
        }
        public string EsDeclarante
        {
            get { return this.esDeclarante.quitarCaracteres(); }
            set { this.esDeclarante = value; }
        }
        public string NormaDeclarante
        {
            get { return this.normaDeclarante.quitarCaracteres(); }
            set { this.normaDeclarante = value; }
        }
        public string FechaNormaDeclarante
        {
            get { return this.fechaNormaDeclarante.quitarCaracteres(); }
            set { this.fechaNormaDeclarante = value; }
        }
        public string EsExentoDeRetencion
        {
            get { return this.esExentoDeRetencion.quitarCaracteres(); }
            set { this.esExentoDeRetencion = value; }
        }
        public string NormaExentoDeRetencion
        {
            get { return this.normaExentoDeRetencion.quitarCaracteres(); }
            set { this.normaExentoDeRetencion = value; }
        }
        public string FechaNormaExentoDeRetencion
        {
            get { return this.fechaNormaExentoDeRetencion.quitarCaracteres(); }
            set { this.fechaNormaExentoDeRetencion = value; }
        }
        public string EsGranContribuyente
        {
            get { return this.esGranContribuyente.quitarCaracteres(); }
            set { this.esGranContribuyente = value; }
        }
        public string NormaGranContribuyente
        {
            get { return this.normaGranContribuyente.quitarCaracteres(); }
            set { this.normaGranContribuyente = value; }
        }
        public string FechaNormaGranContribuyente
        {
            get { return this.fechaNormaGranContribuyente.quitarCaracteres(); }
            set { this.fechaNormaGranContribuyente = value; }
        }
    }
    public class contratoProyectoInfo
    {
        public int IdPlan { get; set; }
        private string numeroDeContrato;
        private string fechaActaDeInicio;
        private string objeto;
        private string fechaDelAp;
        private string noPolizaDeSeguroDeVida;
        private string valorInicial;
        private string plazo;
        private string numeroDelApPresupuestal;
        private string fechaFirmaDelContrato;
        private string companiaSeguroDeVida;
        public string NumeroDeContrato
        {
            get { return this.numeroDeContrato.quitarCaracteres(); }
            set { this.numeroDeContrato = value; }
        }
        public string FechaActaDeInicio
        {
            get { return this.fechaActaDeInicio.quitarCaracteres(); }
            set { this.fechaActaDeInicio = value; }
        }
        public string Objeto
        {
            get { return this.objeto.quitarCaracteres(); }
            set { this.objeto = value; }
        }
        public string FechaDelAp
        {
            get { return this.fechaDelAp.quitarCaracteres(); }
            set { this.fechaDelAp = value; }
        }
        public string NoPolizaDeSeguroDeVida
        {
            get { return this.noPolizaDeSeguroDeVida.quitarCaracteres(); }
            set { this.noPolizaDeSeguroDeVida = value; }
        }
        public string ValorInicial
        {
            get { return this.valorInicial.quitarCaracteres(); }
            set { this.valorInicial = value; }
        }
        public string Plazo
        {
            get { return this.plazo.quitarCaracteres(); }
            set { this.plazo = value; }
        }
        public string NumeroDelApPresupuestal
        {
            get { return this.numeroDelApPresupuestal.quitarCaracteres(); }
            set { this.numeroDelApPresupuestal = value; }
        }
        public string FechaFirmaDelContrato
        {
            get { return this.fechaFirmaDelContrato.quitarCaracteres(); }
            set { this.fechaFirmaDelContrato = value; }
        }
        public string CompaniaSeguroDeVida
        {
            get { return this.companiaSeguroDeVida.quitarCaracteres(); }
            set { this.companiaSeguroDeVida = value; }
        }
    }
    public class ConceptoFinalRecomendaciones
    {
        public int IdPlan { get; set; }
        private string dificultadCentral;
        private string observaciones;
        public string DificultadCentral
        {
            get { return this.dificultadCentral.quitarCaracteres(); }
            set { this.dificultadCentral = value; }
        }
        public string Observaciones
        {
            get { return this.observaciones.quitarCaracteres(); }
            set { this.observaciones = value; }
        }
    }
    public class riesgosInternventorxProyecto
    {
        public int IdPlan { get; set; }
        private string ejeFuncional;
        private string riesgo;
        private string mitigacion;
        private string observacion;
        public string EjeFuncional
        {
            get { return this.ejeFuncional.quitarCaracteres(); }
            set { this.ejeFuncional = value; }
        }
        public string Riesgo
        {
            get { return this.riesgo.quitarCaracteres(); }
            set { this.riesgo = value; }
        }
        public string Mitigacion
        {
            get { return this.mitigacion.quitarCaracteres(); }
            set { this.mitigacion = value; }
        }
        public string Observacion
        {
            get { return this.observacion.quitarCaracteres(); }
            set { this.observacion = value; }
        }
    }

    public class indicadoresGestionxProyecto
    {
        public int IdPlan { get; set; }
        private string nombre;
        private string decripcion;
        private string evaluacion;
        private string observacion;
        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
        public string Decripcion
        {
            get { return this.decripcion.quitarCaracteres(); }
            set { this.decripcion = value; }
        }
        public string Evaluacion
        {
            get { return this.evaluacion.quitarCaracteres(); }
            set { this.evaluacion = value; }
        }
        public string Observacion
        {
            get { return this.observacion.quitarCaracteres(); }
            set { this.observacion = value; }
        }
    }

    public class AvanceMesxMesxProyecto
    {
        public int idPlan { get; set; }
        private string actividad;
        public int mes { get; set; }
        private string fechaAvance;
        private string observacionesEmprendedor;
        private string fechaAprobacion;
        private string observacionesInterventor;
        private string actividaAprobada;
        private string registradoPor;
        private string valorFondoEmprender;
        private string valorAporteEmprendedor;
        public string Actividad
        {
            get { return this.actividad.quitarCaracteres(); }
            set { this.actividad = value; }
        }
        public string FechaAvance
        {
            get { return this.fechaAvance.quitarCaracteres(); }
            set { this.fechaAvance = value; }
        }
        public string ObservacionesEmprendedor
        {
            get { return this.observacionesEmprendedor.quitarCaracteres(); }
            set { this.observacionesEmprendedor = value; }
        }
        public string FechaAprobacion
        {
            get { return this.fechaAprobacion.quitarCaracteres(); }
            set { this.fechaAprobacion = value; }
        }
        public string ObservacionesInterventor
        {
            get { return this.observacionesInterventor.quitarCaracteres(); }
            set { this.observacionesInterventor = value; }
        }
        public string ActividaAprobada
        {
            get { return this.actividaAprobada.quitarCaracteres(); }
            set { this.actividaAprobada = value; }
        }
        public string RegistradoPor
        {
            get { return this.registradoPor.quitarCaracteres(); }
            set { this.registradoPor = value; }
        }
        public string ValorFondoEmprender
        {
            get { return this.valorFondoEmprender.quitarCaracteres(); }
            set { this.valorFondoEmprender = value; }
        }
        public string ValorAporteEmprendedor
        {
            get { return this.valorAporteEmprendedor.quitarCaracteres(); }
            set { this.valorAporteEmprendedor = value; }
        }
    }

    public class evalIndicadorGestionMercadeoEmpleo
    {
        private string cantidad;
        private string actividad_Cargo;
        private string datosEvaluador;

        public string Cantidad
        {
            get { return this.cantidad.quitarCaracteres(); }
            set { this.cantidad = value; }
        }
        public string Actividad_Cargo
        {
            get { return this.actividad_Cargo.quitarCaracteres(); }
            set { this.actividad_Cargo = value; }
        }
        public string DatosEvaluador
        {
            get { return this.datosEvaluador.quitarCaracteres(); }
            set { this.datosEvaluador = value; }
        }
    }

    public class evalIndicadorDeGestionProduccion
    {
        private string productoRepresentativo;
        private string productoRepresentativoEvaluacion;
        private string producto;
        private string unidades;
        private string datosEvaluador;

        public string ProductoRepresentativo
        {
            get { return this.productoRepresentativo.quitarCaracteres(); }
            set { this.productoRepresentativo = value; }
        }
        public string ProductoRepresentativoEvaluacion
        {
            get { return this.productoRepresentativoEvaluacion.quitarCaracteres(); }
            set { this.productoRepresentativoEvaluacion = value; }
        }
        public string Producto
        {
            get { return this.producto.quitarCaracteres(); }
            set { this.producto = value; }
        }
        public string Unidades
        {
            get { return this.unidades.quitarCaracteres(); }
            set { this.unidades = value; }
        }
        public string DatosEvaluador
        {
            get { return this.datosEvaluador.quitarCaracteres(); }
            set { this.datosEvaluador = value; }
        }
    }

    public class evaluacionIndicadoresGestion
    {
        private string ejecucionPresupuestalEmprendedor;
        private string ejecucionPresupuestalEvaluador;
        private string iDHEmprendedor;
        private string iDHEvaluador;
        private string contrapartidaEmprendedor;
        private string contrapartidaEvaluador;
        private string ventasEmprendedor;
        private string ventasEvaluador;
        private string periodoemproductivoEmprededor;
        private string priodoemproductivoEvaluador;
        private string aportesEmprendedorEmprededor;
        private string aportesEmprendedorEvaluador;

        public string EjecucionPresupuestalEmprendedor
        {
            get { return this.ejecucionPresupuestalEmprendedor.quitarCaracteres(); }
            set { this.ejecucionPresupuestalEmprendedor = value; }
        }
        public string EjecucionPresupuestalEvaluador
        {
            get { return this.ejecucionPresupuestalEvaluador.quitarCaracteres(); }
            set { this.ejecucionPresupuestalEvaluador = value; }
        }
        public string IDHEmprendedor
        {
            get { return this.iDHEmprendedor.quitarCaracteres(); }
            set { this.iDHEmprendedor = value; }
        }
        public string IDHEvaluador
        {
            get { return this.iDHEvaluador.quitarCaracteres(); }
            set { this.iDHEvaluador = value; }
        }
        public string ContrapartidaEmprendedor
        {
            get { return this.contrapartidaEmprendedor.quitarCaracteres(); }
            set { this.contrapartidaEmprendedor = value; }
        }
        public string ContrapartidaEvaluador
        {
            get { return this.contrapartidaEvaluador.quitarCaracteres(); }
            set { this.contrapartidaEvaluador = value; }
        }
        public string VentasEmprendedor
        {
            get { return this.ventasEmprendedor.quitarCaracteres(); }
            set { this.ventasEmprendedor = value; }
        }
        public string VentasEvaluador
        {
            get { return this.ventasEvaluador.quitarCaracteres(); }
            set { this.ventasEvaluador = value; }
        }
        public string PeriodoemproductivoEmprededor
        {
            get { return this.periodoemproductivoEmprededor.quitarCaracteres(); }
            set { this.periodoemproductivoEmprededor = value; }
        }
        public string PeriodoemproductivoEvaluador
        {
            get { return this.priodoemproductivoEvaluador.quitarCaracteres(); }
            set { this.priodoemproductivoEvaluador = value; }
        }
        public string AportesEmprendedorEmprededor
        {
            get { return this.aportesEmprendedorEmprededor.quitarCaracteres(); }
            set { this.aportesEmprendedorEmprededor = value; }
        }
        public string AportesEmprendedorEvaluador
        {
            get { return this.aportesEmprendedorEvaluador.quitarCaracteres(); }
            set { this.aportesEmprendedorEvaluador = value; }
        }
    }
    public class conceptoFinalEvaluacion
    {
        private string viable;
        private string conceptos;
        private string justificacion;

        public string Viable
        {
            get { return this.viable.quitarCaracteres(); }
            set { this.viable = value; }
        }
        public string Conceptos
        {
            get { return this.conceptos.quitarCaracteres(); }
            set { this.conceptos = value; }
        }
        public string Justificacion
        {
            get { return this.justificacion.quitarCaracteres(); }
            set { this.justificacion = value; }
        }
    }
    public class riesgosEvaluacion
    {
        private string riesgo;
        private string mitigacion;

        public string Riesgo
        {
            get { return this.riesgo.quitarCaracteres(); }
            set { this.riesgo = value; }
        }
        public string Mitigacion
        {
            get { return this.mitigacion.quitarCaracteres(); }
            set { this.mitigacion = value; }
        }
    }

    public class evaluacionObservacionAportes
    {
        private string equipoTrabajo;
        private string recursosSolicitados;
        public double valorRecomendado { get; set; }

        public string EquipoTrabajo
        {
            get { return this.equipoTrabajo.quitarCaracteres(); }
            set { this.equipoTrabajo = value; }
        }
        public string RecursosSolicitados
        {
            get { return this.recursosSolicitados.quitarCaracteres(); }
            set { this.recursosSolicitados = value; }
        }
       
    }

    public class EvaluacionIntegrantes
    {
        private string nombre;
        private string emprendedor;
        private string otro;
        private string aporteTotal;
        private string aporteDinero;
        private string aporteEspecie;
        private string claseEspecie;

        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
        public string Emprendedor
        {
            get { return this.emprendedor.quitarCaracteres(); }
            set { this.emprendedor = value; }
        }
        public string Otro
        {
            get { return this.otro.quitarCaracteres(); }
            set { this.otro = value; }
        }
        public string AporteTotal
        {
            get { return this.aporteTotal.quitarCaracteres(); }
            set { this.aporteTotal = value; }
        }
        public string AporteDinero
        {
            get { return this.aporteDinero.quitarCaracteres(); }
            set { this.aporteDinero = value; }
        }
        public string AporteEspecie
        {
            get { return this.aporteEspecie.quitarCaracteres(); }
            set { this.aporteEspecie = value; }
        }
        public string ClaseEspecie
        {
            get { return this.claseEspecie.quitarCaracteres(); }
            set { this.claseEspecie = value; }
        }
    }

    public class EvaluacionCapitalEInversiones
    {
        private string nombre;
        private string detalle;
        private string fuenteFinanciacion;
        private string totalSolicitado;
        private string porcentajeSolicitado;
        private string totalRecomendado;
        private string porcentajeRecomendado;

        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
        public string Detalle
        {
            get { return this.detalle.quitarCaracteres(); }
            set { this.detalle = value; }
        }
        public string FuenteFinanciacion
        {
            get { return this.fuenteFinanciacion.quitarCaracteres(); }
            set { this.fuenteFinanciacion = value; }
        }
        public string TotalSolicitado
        {
            get { return this.totalSolicitado.quitarCaracteres(); }
            set { this.totalSolicitado = value; }
        }
        public string PorcentajeSolicitado
        {
            get { return this.porcentajeSolicitado.quitarCaracteres(); }
            set { this.porcentajeSolicitado = value; }
        }
        public string TotalRecomendado
        {
            get { return this.totalRecomendado.quitarCaracteres(); }
            set { this.totalRecomendado = value; }
        }
        public string PorcentajeRecomendado
        {
            get { return this.porcentajeRecomendado.quitarCaracteres(); }
            set { this.porcentajeRecomendado = value; }
        }
    }

    public class EvaluacionFinancieraIndicadores
    {
        private string descripcion;
        private string valor;
        private string tipo;

        public string Descripcion
        {
            get { return this.descripcion.quitarCaracteres(); }
            set { this.descripcion = value; }
        }
        
        public string Valor
        {
            get { return this.valor.quitarCaracteres(); }
            set { this.valor = value; }
        }
        public string Tipo
        {
            get { return this.tipo.quitarCaracteres(); }
            set { this.tipo = value; }
        }
    }

    public class EmprendedoresPlanOperativo
    {
        public string nombre { get; set; }
        public string beneficiario { get; set; }
        public string participacion { get; set; }
    }
    public class GeneracionDeEmpleo
    {
        public string GenerarPrimerAno { get; set; }
        public string GenerarTotalProyecto { get; set; }
        public string EmpleosIndirectos { get; set; }

    }
    public class MetasEmpleo
    {
        private string tipoEmpleo;
        private string cargo;
        public string TipoEmpleo
        {
            get { return this.tipoEmpleo.ToString(); }
            set { this.tipoEmpleo = value; }
        }
        public string Cargo
        {
            get { return this.cargo.ToString(); }
            set { this.cargo = value; }
        }
        public string valorMensual { get; set; }
        public string generadoPrimerAno { get; set; }
        public string EdadEntre1824Anos { get; set; }
        public string EsDesplazado { get; set; }
        public string EsMadre { get; set; }
        public string EsMinoria { get; set; }
        public string EsRecluido { get; set; }
        public string EsDesmovilizado { get; set; }
        public string EsDiscapacitado { get; set; }
        public string EsDesvinculado { get; set; }
    }
    public class MetasSocialesPlanes
    {
        private string planNacional;
        private string planRegional;
        private string cluster;

        public string PlanNacional
        {
            get { return this.planNacional.quitarCaracteres(); }
            set { this.planNacional = value; }
        }
        public string PlanRegional
        {
            get { return this.planRegional.quitarCaracteres(); }
            set { this.planRegional = value; }
        }
        public string Cluster
        {
            get { return this.cluster.quitarCaracteres(); }
            set { this.cluster = value; }
        }
    }
    public class CronogramaActividades
    {
        private string item;
        private string actividad;
        public string Item
        {
            get { return this.item.quitarCaracteres(); }
            set { this.item = value; }
        }
        public string Actividad
        {
            get { return this.actividad.quitarCaracteres(); }
            set { this.actividad = value; }
        }
        public string FondoEmprenderMes_1 { get; set; }
        public string AporteEmprendedorMes_1 { get; set; }
        public string FondoEmprenderMes_2 { get; set; }
        public string AporteEmprendedorMes_2 { get; set; }
        public string FondoEmprenderMes_3 { get; set; }
        public string AporteEmprendedorMes_3 { get; set; }
        public string FondoEmprenderMes_4 { get; set; }
        public string AporteEmprendedorMes_4 { get; set; }
        public string FondoEmprenderMes_5 { get; set; }
        public string AporteEmprendedorMes_5 { get; set; }
        public string FondoEmprenderMes_6 { get; set; }
        public string AporteEmprendedorMes_6 { get; set; }
        public string FondoEmprenderMes_7 { get; set; }
        public string AporteEmprendedorMes_7 { get; set; }
        public string FondoEmprenderMes_8 { get; set; }
        public string AporteEmprendedorMes_8 { get; set; }
        public string FondoEmprenderMes_9 { get; set; }
        public string AporteEmprendedorMes_9 { get; set; }
        public string FondoEmprenderMes_10 { get; set; }
        public string AporteEmprendedorMes_10 { get; set; }
        public string FondoEmprenderMes_11 { get; set; }
        public string AporteEmprendedorMes_11 { get; set; }
        public string FondoEmprenderMes_12 { get; set; }
        public string AporteEmprendedorMes_12 { get; set; }
        public string FondoEmprenderMes_13 { get; set; }
        public string AporteEmprendedorMes_13 { get; set; }
        public string FondoEmprenderMes_14 { get; set; }
        public string AporteEmprendedorMes_14 { get; set; }
        public string FondoEmprenderMes_15 { get; set; }
        public string AporteEmprendedorMes_15 { get; set; }
        public string FondoEmprenderMes_16 { get; set; }
        public string AporteEmprendedorMes_16 { get; set; }
        public string FondoEmprenderMes_17 { get; set; }
        public string AporteEmprendedorMes_17 { get; set; }
        public string FondoEmprenderMes_18 { get; set; }
        public string AporteEmprendedorMes_18 { get; set; }
        public string FondoEmprenderMes_19 { get; set; }
        public string AporteEmprendedorMes_19 { get; set; }
        public string FondoEmprenderMes_20 { get; set; }
        public string AporteEmprendedorMes_20 { get; set; }
        public string FondoEmprender { get; set; }
        public string AporteEmprendedor { get; set; }

    }

    public class CronogramaNominaCargos
    {
        private string item;
        private string cargo;
        public string Item
        {
            get { return this.item.quitarCaracteres(); }
            set { this.item = value; }
        }
        public string Cargo
        {
            get { return this.cargo.quitarCaracteres(); }
            set { this.cargo = value; }
        }
        public string SueldoMes_1 { get; set; }
        public string PrestacionesMes_1 { get; set; }
        public string SueldoMes_2 { get; set; }
        public string PrestacionesMes_2 { get; set; }
        public string SueldoMes_3 { get; set; }
        public string PrestacionesMes_3 { get; set; }
        public string SueldoMes_4 { get; set; }
        public string PrestacionesMes_4 { get; set; }
        public string SueldoMes_5 { get; set; }
        public string PrestacionesMes_5 { get; set; }
        public string SueldoMes_6 { get; set; }
        public string PrestacionesMes_6 { get; set; }
        public string SueldoMes_7 { get; set; }
        public string PrestacionesMes_7 { get; set; }
        public string SueldoMes_8 { get; set; }
        public string PrestacionesMes_8 { get; set; }
        public string SueldoMes_9 { get; set; }
        public string PrestacionesMes_9 { get; set; }
        public string SueldoMes_10 { get; set; }
        public string PrestacionesMes_10 { get; set; }
        public string SueldoMes_11 { get; set; }
        public string PrestacionesMes_11 { get; set; }
        public string SueldoMes_12 { get; set; }
        public string PrestacionesMes_12 { get; set; }
        public string SueldoMes_13 { get; set; }
        public string PrestacionesMes_13 { get; set; }
        public string SueldoMes_14 { get; set; }
        public string PrestacionesMes_14 { get; set; }
        public string SueldoMes_15 { get; set; }
        public string PrestacionesMes_15 { get; set; }
        public string SueldoMes_16 { get; set; }
        public string PrestacionesMes_16 { get; set; }
        public string SueldoMes_17 { get; set; }
        public string PrestacionesMes_17 { get; set; }
        public string SueldoMes_18 { get; set; }
        public string PrestacionesMes_18 { get; set; }
        public string SueldoMes_19 { get; set; }
        public string PrestacionesMes_19 { get; set; }
        public string SueldoMes_20 { get; set; }
        public string PrestacionesMes_20 { get; set; }
        public string SueldoTotal { get; set; }
        public string PrestacionesTotal { get; set; }

    }

    public class CronogramaProduccion
    {
        private string item;
        private string producto;
        public string Item
        {
            get { return this.item.quitarCaracteres(); }
            set { this.item = value; }
        }
        public string Producto
        {
            get { return this.producto.quitarCaracteres(); }
            set { this.producto = value; }
        }
        public string CantidadMes_1 { get; set; }
        public string CostoMes_1 { get; set; }
        public string CantidadMes_2 { get; set; }
        public string CostoMes_2 { get; set; }
        public string CantidadMes_3 { get; set; }
        public string CostoMes_3 { get; set; }
        public string CantidadMes_4 { get; set; }
        public string CostoMes_4 { get; set; }
        public string CantidadMes_5 { get; set; }
        public string CostoMes_5 { get; set; }
        public string CantidadMes_6 { get; set; }
        public string CostoMes_6 { get; set; }
        public string CantidadMes_7 { get; set; }
        public string CostoMes_7 { get; set; }
        public string CantidadMes_8 { get; set; }
        public string CostoMes_8 { get; set; }
        public string CantidadMes_9 { get; set; }
        public string CostoMes_9 { get; set; }
        public string CantidadMes_10 { get; set; }
        public string CostoMes_10 { get; set; }
        public string CantidadMes_11 { get; set; }
        public string CostoMes_11 { get; set; }
        public string CantidadMes_12 { get; set; }
        public string CostoMes_12 { get; set; }
        public string CantidadMes_13 { get; set; }
        public string CostoMes_13 { get; set; }
        public string CantidadMes_14 { get; set; }
        public string CostoMes_14 { get; set; }
        public string CantidadMes_15 { get; set; }
        public string CostoMes_15 { get; set; }
        public string CantidadMes_16 { get; set; }
        public string CostoMes_16 { get; set; }
        public string CantidadMes_17 { get; set; }
        public string CostoMes_17 { get; set; }
        public string CantidadMes_18 { get; set; }
        public string CostoMes_18 { get; set; }
        public string CantidadMes_19 { get; set; }
        public string CostoMes_19 { get; set; }
        public string CantidadMes_20 { get; set; }
        public string CostoMes_20 { get; set; }
        public string CantidadTotal { get; set; }
        public string CostoTotal { get; set; }

    }

    public class CronogramaVentas
    {
        private string item;
        private string producto;
        public string Item
        {
            get { return this.item.quitarCaracteres(); }
            set { this.item = value; }
        }
        public string Producto
        {
            get { return this.producto.quitarCaracteres(); }
            set { this.producto = value; }
        }
        public string VentasMes_1 { get; set; }
        public string IngresoMes_1 { get; set; }
        public string VentasMes_2 { get; set; }
        public string IngresoMes_2 { get; set; }
        public string VentasMes_3 { get; set; }
        public string IngresoMes_3 { get; set; }
        public string VentasMes_4 { get; set; }
        public string IngresoMes_4 { get; set; }
        public string VentasMes_5 { get; set; }
        public string IngresoMes_5 { get; set; }
        public string VentasMes_6 { get; set; }
        public string IngresoMes_6 { get; set; }
        public string VentasMes_7 { get; set; }
        public string IngresoMes_7 { get; set; }
        public string VentasMes_8 { get; set; }
        public string IngresoMes_8 { get; set; }
        public string VentasMes_9 { get; set; }
        public string IngresoMes_9 { get; set; }
        public string VentasMes_10 { get; set; }
        public string IngresoMes_10 { get; set; }
        public string VentasMes_11 { get; set; }
        public string IngresoMes_11 { get; set; }
        public string VentasMes_12 { get; set; }
        public string IngresoMes_12 { get; set; }
        public string VentasMes_13 { get; set; }
        public string IngresoMes_13 { get; set; }
        public string VentasMes_14 { get; set; }
        public string IngresoMes_14 { get; set; }
        public string VentasMes_15 { get; set; }
        public string IngresoMes_15 { get; set; }
        public string VentasMes_16 { get; set; }
        public string IngresoMes_16 { get; set; }
        public string VentasMes_17 { get; set; }
        public string IngresoMes_17 { get; set; }
        public string VentasMes_18 { get; set; }
        public string IngresoMes_18 { get; set; }
        public string VentasMes_19 { get; set; }
        public string IngresoMes_19 { get; set; }
        public string VentasMes_20 { get; set; }
        public string IngresoMes_20 { get; set; }
        public string VentasTotal { get; set; }
        public string IngresoTotal { get; set; }

    }

    public class CapitalTrabajo
    {
        private string componente;
        private string valor;
        private string fuenteFinanciacion;
        private string observacion;

        public string Componente
        {
            get { return this.componente.quitarCaracteres(); }
            set { this.componente = value; }
        }
        public string Valor
        {
            get { return this.valor.quitarCaracteres(); }
            set { this.valor = value; }
        }
        public string FuenteFinanciacion
        {
            get { return this.fuenteFinanciacion.quitarCaracteres(); }
            set { this.fuenteFinanciacion = value; }
        }
        public string Observacion
        {
            get { return this.observacion.quitarCaracteres(); }
            set { this.observacion = value; }
        }
    }
    public class Inversiones
    {
        private string concepto { get; set; }
        public string valor { get; set; }
        public string mes { get; set; }
        private string fuenteFinanciacion { get; set; }

        public string Concepto
        {
            get { return this.concepto.quitarCaracteres(); }
            set { this.concepto = value; }
        }

        public string FuenteFinanciacion
        {
            get { return this.fuenteFinanciacion.quitarCaracteres(); }
            set { this.fuenteFinanciacion = value; }
        }
    }
    public class DatoPorAno
    {
        private string dato { get; set; }
        public string ano_1 { get; set; }
        public string ano_2 { get; set; }
        public string ano_3 { get; set; }
        public string ano_4 { get; set; }
        public string ano_5 { get; set; }
        public string ano_6 { get; set; }
        public string ano_7 { get; set; }
        public string ano_8 { get; set; }
        public string ano_9 { get; set; }
        public string ano_10 { get; set; }

        public string Dato
        {
            get { return this.dato.quitarCaracteres(); }
            set { this.dato = value; }
        }
    }
    public class RecursoCapital
    {
        private string tipo { get; set; }
        private string cuantia { get; set; }
        private string plazo { get; set; }
        private string formaPago { get; set; }
        private string interes { get; set; }
        private string destinacion { get; set; }

        public string Tipo
        {
            get { return this.tipo.quitarCaracteres(); }
            set { this.tipo = value; }
        }
        public string Cuantia
        {
            get { return this.cuantia.quitarCaracteres(); }
            set { this.cuantia = value; }
        }
        public string Plazo
        {
            get { return this.plazo.quitarCaracteres(); }
            set { this.plazo = value; }
        }
        public string FormaPago
        {
            get { return this.formaPago.quitarCaracteres(); }
            set { this.formaPago = value; }
        }
        public string Interes
        {
            get { return this.interes.quitarCaracteres(); }
            set { this.interes = value; }
        }
        public string Destinacion
        {
            get { return this.destinacion.quitarCaracteres(); }
            set { this.destinacion = value; }
        }
    }

    public class CostosAdministrativos
    {
        private string descripcion { get; set; }
        private string valor { get; set; }
        private string nombre { get; set; }

        public string Descripcion
        {
            get { return this.descripcion.quitarCaracteres(); }
            set { this.descripcion = value; }
        }
        public string Valor
        {
            get { return this.valor.quitarCaracteres(); }
            set { this.valor = value; }
        }
        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
    }

    public class CostosDeProduccion
    {
        private string tipoInsumo { get; set; }
        private string valor { get; set; }
        public int Ano { get; set; }

        public string TipoInsumo
        {
            get { return this.tipoInsumo.quitarCaracteres(); }
            set { this.tipoInsumo = value; }
        }
        public string Valor
        {
            get { return this.valor.quitarCaracteres(); }
            set { this.valor = value; }
        }
    }

    public class ProyeccionCompras
    {
        private string tipoInsumo { get; set; }
        private string insumo { get; set; }
        private string valor { get; set; }
        public int ano { get; set; }

        public string TipoInsumo
        {
            get { return this.tipoInsumo.quitarCaracteres(); }
            set { this.tipoInsumo = value; }
        }
        public string Insumo
        {
            get { return this.insumo.quitarCaracteres(); }
            set { this.insumo = value; }
        }
        public string Valor
        {
            get { return this.valor.quitarCaracteres(); }
            set { this.valor = value; }
        }
    }

    public static class UtilidadProyecto
    {
        public static string quitarCaracteres(this String str)
        {
            if (str != null)
                return str.Replace("|", " ").Replace("¦", " ")
                    .Replace("§", " ").Replace(System.Environment.NewLine, " ").Replace("\n", " ").Replace("\t", " ");
            else
                return "";
        }

    }
    public class ResumenEjecutivo
    {
        private string conceptoNegocio;
        public int empleos { get; set; }
        public int contrapartidas { get; set; }
        public int ejecucionPresupuestal { get; set; }
        public double ventas { get; set; }
        public int mercadeo { get; set; }
        public int periodoImproductivo { get; set; }
        public double IDH { get; set; }
        private string recursosAportadosEmprendedor;
        private string videoEmprendedor;

        public string ConceptoNegocio
        {
            get { return this.conceptoNegocio.quitarCaracteres(); }
            set { this.conceptoNegocio = value; }
        }
        public string RecursosAportadosEmprendedor
        {
            get { return this.recursosAportadosEmprendedor.quitarCaracteres(); }
            set { this.recursosAportadosEmprendedor = value; }
        }
        public string VideoEmprendedor
        {
            get { return this.videoEmprendedor.quitarCaracteres(); }
            set { this.videoEmprendedor = value; }
        }
    }
    public class Riesgos
    {
        private string actoresExternosCriticos;
        private string factoresExternosAfectanOperacion;

        public string ActoresExternosCriticos
        {
            get { return this.actoresExternosCriticos.quitarCaracteres(); }
            set { this.actoresExternosCriticos = value; }
        }
        public string FactoresExternosAfectanOperacion
        {
            get { return this.factoresExternosAfectanOperacion.quitarCaracteres(); }
            set { this.factoresExternosAfectanOperacion = value; }
        }
    }
    public class FuturoNegocioPeriodoArranque
    {
        private string periodoArranque;
        private string periodoImproductivo;

        public string PeriodoArranque
        {
            get { return this.periodoArranque.quitarCaracteres(); }
            set { this.periodoArranque = value; }
        }
        public string PeriodoImproductivo
        {
            get { return this.periodoImproductivo.quitarCaracteres(); }
            set { this.periodoImproductivo = value; }
        }
    }
    public class FuturoNegocioActividades
    {
        private string actividad;
        private string recursoRequerido;
        private string mesDeEjecucion;
        public decimal costo { get; set; }
        private string responsable;

        public string Actividad
        {
            get { return this.actividad.quitarCaracteres(); }
            set { this.actividad = value; }
        }
        public string RecursoRequerido
        {
            get { return this.recursoRequerido.quitarCaracteres(); }
            set { this.recursoRequerido = value; }
        }
        public string Responsable
        {
            get { return this.responsable.quitarCaracteres(); }
            set { this.responsable = value; }
        }
        public string MesDeEjecucion
        {
            get { return this.mesDeEjecucion.quitarCaracteres(); }
            set { this.mesDeEjecucion = value; }
        }
    }
    public class FuturoNegocioEstrategia
    {
        private string nombreEstrategiaPromocion;
        private string propositoPromocion;
        private string nombreEstrategiaComunicacion;
        private string propositoComunicacion;
        private string nombreEstrategiaDistribucion;
        private string propositoDistribucion;

        public string NombreEstrategiaPromocion
        {
            get { return this.nombreEstrategiaPromocion.quitarCaracteres(); }
            set { this.nombreEstrategiaPromocion = value; }
        }
        public string PropositoPromocion
        {
            get { return this.propositoPromocion.quitarCaracteres(); }
            set { this.propositoPromocion = value; }
        }
        public string NombreEstrategiaComunicacion
        {
            get { return this.nombreEstrategiaComunicacion.quitarCaracteres(); }
            set { this.nombreEstrategiaComunicacion = value; }
        }
        public string PropositoComunicacion
        {
            get { return this.propositoComunicacion.quitarCaracteres(); }
            set { this.propositoComunicacion = value; }
        }
        public string NombreEstrategiaDistribucion
        {
            get { return this.nombreEstrategiaDistribucion.quitarCaracteres(); }
            set { this.nombreEstrategiaDistribucion = value; }
        }
        public string PropositoDistribucion
        {
            get { return this.propositoDistribucion.quitarCaracteres(); }
            set { this.propositoDistribucion = value; }
        }
    }
    public class DesarrolloSolucionCargosOperacion
    {
        private string nombreCargo;
        private string funcionesPrincipales;
        private string perfil;
        private string experienciaGeneral;
        private string experienciaEspecifica;
        private string tipoContratacion;
        private string dedicacionTiempo;
        private string unidadMedidaTiempo;
        public int tiempoVinculacion { get; set; }
        public decimal valorRemuneracion { get; set; }
        public decimal otrosGastos { get; set; }
        public decimal valorConPrestaciones { get; set; }
        public decimal remuneracionPrimerAno { get; set; }
        public decimal valorSolicitadoFondoEmprender { get; set; }
        public decimal aportesEmprendedores { get; set; }
        public decimal ingresosPorVentas { get; set; }

        public string NombreCargo
        {
            get { return this.nombreCargo.quitarCaracteres(); }
            set { this.nombreCargo = value; }
        }
        public string FuncionesPrincipales
        {
            get { return this.funcionesPrincipales.quitarCaracteres(); }
            set { this.funcionesPrincipales = value; }
        }
        public string Perfil
        {
            get { return this.perfil.quitarCaracteres(); }
            set { this.perfil = value; }
        }
        public string ExperienciaGeneral
        {
            get { return this.experienciaGeneral.quitarCaracteres(); }
            set { this.experienciaGeneral = value; }
        }
        public string ExperienciaEspecifica
        {
            get { return this.experienciaEspecifica.quitarCaracteres(); }
            set { this.experienciaEspecifica = value; }
        }
        public string TipoContratacion
        {
            get { return this.tipoContratacion.quitarCaracteres(); }
            set { this.tipoContratacion = value; }
        }
        public string DedicacionTiempo
        {
            get { return this.dedicacionTiempo.quitarCaracteres(); }
            set { this.dedicacionTiempo = value; }
        }
        public string UnidadMedidaTiempo
        {
            get { return this.unidadMedidaTiempo.quitarCaracteres(); }
            set { this.unidadMedidaTiempo = value; }
        }

    }
    public class DesarrolloSolucionPerfilEmprendedor
    {
        private string perfil;
        private string rol;
        public string Perfil
        {
            get { return this.perfil.quitarCaracteres(); }
            set { this.perfil = value; }
        }
        public string Rol
        {
            get { return this.rol.quitarCaracteres(); }
            set { this.rol = value; }
        }
    }
    public class DesarrolloSolucionProductividad
    {
        private string capacidadProductivaEmpresa;

        public string CapacidadProductivaEmpresa
        {
            get { return this.capacidadProductivaEmpresa.quitarCaracteres(); }
            set { this.capacidadProductivaEmpresa = value; }
        }
    }
    public class DesarrolloSolucionProcesoProduccion
    {
        private string nombreProducto;
        private string procesoProduccionProducto;

        public string NombreProducto
        {
            get { return this.nombreProducto.quitarCaracteres(); }
            set { this.nombreProducto = value; }
        }
        public string ProcesoProduccionProducto
        {
            get { return this.procesoProduccionProducto.quitarCaracteres(); }
            set { this.procesoProduccionProducto = value; }
        }
    }
    public class DesarrolloSolucionProduccion
    {
        private string detalleCondicionesTecnicas;
        private string contemplaImportacion;
        private string justificacionContemplaImportacion;
        private string detalleActivos;
        private string financiacionMayorValor;

        public string DetalleCondicionesTecnicas
        {
            get { return this.detalleCondicionesTecnicas.quitarCaracteres(); }
            set { this.detalleCondicionesTecnicas = value; }
        }
        public string ContemplaImportacion
        {
            get { return this.contemplaImportacion.quitarCaracteres(); }
            set { this.contemplaImportacion = value; }
        }
        public string JustificacionContemplaImportacion
        {
            get { return this.justificacionContemplaImportacion.quitarCaracteres(); }
            set { this.justificacionContemplaImportacion = value; }
        }
        public string DetalleActivos
        {
            get { return this.detalleActivos.quitarCaracteres(); }
            set { this.detalleActivos = value; }
        }
        public string FinanciacionMayorValor
        {
            get { return this.financiacionMayorValor.quitarCaracteres(); }
            set { this.financiacionMayorValor = value; }
        }
    }

    public class DesarrolloSolucionReqInversion
    {
        private string descripcion;
        private double cantidad;
        private decimal valorUnitario;
        private string fuenteFinanciacion;
        private string requisitosTecnicos;

        public string Descripcion
        {
            get { return this.descripcion.quitarCaracteres(); }
            set { this.descripcion = value; }
        }
        public double Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }
        public decimal ValorUnitario
        {
            get { return this.valorUnitario; }
            set { this.valorUnitario = value; }
        }
        public string FuenteFinanciacion
        {
            get { return this.fuenteFinanciacion.quitarCaracteres(); }
            set { this.fuenteFinanciacion = value; }
        }
        public string RequisitosTecnicos
        {
            get { return this.requisitosTecnicos.quitarCaracteres(); }
            set { this.requisitosTecnicos = value; }
        }
    }
    public class DesarrolloSolucionRequerimientos
    {
        private string necesarioLugarFisico;
        private string justificacionLugarFisico;

        public string NecesarioLugarFisico
        {
            get { return this.necesarioLugarFisico.quitarCaracteres(); }
            set { this.necesarioLugarFisico = value; }
        }
        public string JustificacionLugarFisico
        {
            get { return this.justificacionLugarFisico.quitarCaracteres(); }
            set { this.justificacionLugarFisico = value; }
        }
    }
    public class DesarrolloSolucionNormatividad
    {
        private string normatividadEmpresarial;
        private string normatividadTributaria;
        private string normatividadTecnica;
        private string normatividadLaboral;
        private string normatividadAmbiental;
        private string registroMarca;
        private string condicionesTecnicasOperacionNegocio;

        public string NormatividadEmpresarial
        {
            get { return this.normatividadEmpresarial.quitarCaracteres(); }
            set { this.normatividadEmpresarial = value; }
        }
        public string NormatividadTributaria
        {
            get { return this.normatividadTributaria.quitarCaracteres(); }
            set { this.normatividadTributaria = value; }
        }
        public string NormatividadTecnica
        {
            get { return this.normatividadTecnica.quitarCaracteres(); }
            set { this.normatividadTecnica = value; }
        }
        public string NormatividadLaboral
        {
            get { return this.normatividadLaboral.quitarCaracteres(); }
            set { this.normatividadLaboral = value; }
        }
        public string NormatividadAmbiental
        {
            get { return this.normatividadAmbiental.quitarCaracteres(); }
            set { this.normatividadAmbiental = value; }
        }
        public string RegistroMarca
        {
            get { return this.registroMarca.quitarCaracteres(); }
            set { this.registroMarca = value; }
        }
        public string CondicionesTecnicasOperacionNegocio
        {
            get { return this.condicionesTecnicasOperacionNegocio.quitarCaracteres(); }
            set { this.condicionesTecnicasOperacionNegocio = value; }
        }
    }

    public class DesarrolloSolucionIngresos
    {
        private string estrategiaIngresos;
        private string dondeCompra;
        private string caracteristicasParaCompra;
        private string frecuenciaCompra;
        private string precio;

        public string EstrategiaIngresos
        {
            get { return this.estrategiaIngresos.quitarCaracteres(); }
            set { this.estrategiaIngresos = value; }
        }
        public string DondeCompra
        {
            get { return this.dondeCompra.quitarCaracteres(); }
            set { this.dondeCompra = value; }
        }
        public string CaracteristicasParaCompra
        {
            get { return this.caracteristicasParaCompra.quitarCaracteres(); }
            set { this.caracteristicasParaCompra = value; }
        }
        public string FrecuenciaCompra
        {
            get { return this.frecuenciaCompra.quitarCaracteres(); }
            set { this.frecuenciaCompra = value; }
        }
        public string Precio
        {
            get { return this.precio.quitarCaracteres(); }
            set { this.precio = value; }
        }
    }

    public class DesarrolloSolucionCondicionesComerciales
    {
        private string volumenesFrecuencia;
        private string caracteristicasCompra;
        private string sitioCcompra;
        private string formaPago;
        private string precio;
        private string requisitosPostVenta;
        private string garantias;
        private string margenComercializacion;
        private string cliente;

        public string Cliente
        {
            get { return this.cliente.quitarCaracteres(); }
            set { this.cliente = value; }
        }
        public string VolumenesFrecuencia
        {
            get { return this.volumenesFrecuencia.quitarCaracteres(); }
            set { this.volumenesFrecuencia = value; }
        }
        public string CaracteristicasCompra
        {
            get { return this.caracteristicasCompra.quitarCaracteres(); }
            set { this.caracteristicasCompra = value; }
        }
        public string SitioCcompra
        {
            get { return this.sitioCcompra.quitarCaracteres(); }
            set { this.sitioCcompra = value; }
        }
        public string FormaPago
        {
            get { return this.formaPago.quitarCaracteres(); }
            set { this.formaPago = value; }
        }
        public string Precio
        {
            get { return this.precio.quitarCaracteres(); }
            set { this.precio = value; }
        }
        public string RequisitosPostVenta
        {
            get { return this.requisitosPostVenta.quitarCaracteres(); }
            set { this.requisitosPostVenta = value; }
        }
        public string Garantias
        {
            get { return this.garantias.quitarCaracteres(); }
            set { this.garantias = value; }
        }
        public string MargenComercializacion
        {
            get { return this.margenComercializacion.quitarCaracteres(); }
            set { this.margenComercializacion = value; }
        }
    }

    public class ProyeccionListadoProducto
    {
        private string nomProducto;
        private string unidadMedida;
        private string formaDePago;
        private string justificacion;
        private string iva;
        public string NomProduto
        {
            get { return this.nomProducto.quitarCaracteres(); }
            set { this.nomProducto = value; }
        }

        public string UnidadMedida
        {
            get { return this.unidadMedida.quitarCaracteres(); }
            set { this.unidadMedida = value; }
        }
        public string FormaDePago
        {
            get { return this.formaDePago.quitarCaracteres(); }
            set { this.formaDePago = value; }
        }
        public string Justificacion
        {
            get { return this.justificacion.quitarCaracteres(); }
            set { this.justificacion = value; }
        }
        public string IVA
        {
            get { return this.iva.quitarCaracteres(); }
            set { this.iva = value; }
        }
    }

    public class SolucionFichaTecnica
    {
        private string productoEspecifico;
        private string nombreComercial;
        private string unidadDeMedida;
        private string descripcionGeneral;
        private string condicionesEspeciales;
        private string composicion;
        private string otros;
        private string productoMasRrepresentativo;

        public string ProductoEspecifico
        {
            get { return this.productoEspecifico.quitarCaracteres(); }
            set { this.productoEspecifico = value; }
        }
        public string NombreComercial
        {
            get { return this.nombreComercial.quitarCaracteres(); }
            set { this.nombreComercial = value; }
        }
        public string UnidadDeMedida
        {
            get { return this.unidadDeMedida.quitarCaracteres(); }
            set { this.unidadDeMedida = value; }
        }
        public string DescripcionGeneral
        {
            get { return this.descripcionGeneral.quitarCaracteres(); }
            set { this.descripcionGeneral = value; }
        }
        public string CondicionesEspeciales
        {
            get { return this.condicionesEspeciales.quitarCaracteres(); }
            set { this.condicionesEspeciales = value; }
        }
        public string Composicion
        {
            get { return this.composicion.quitarCaracteres(); }
            set { this.composicion = value; }
        }
        public string Otros
        {
            get { return this.otros.quitarCaracteres(); }
            set { this.otros = value; }
        }
        public string ProductoMasRrepresentativo
        {
            get { return this.productoMasRrepresentativo.quitarCaracteres(); }
            set { this.productoMasRrepresentativo = value; }
        }
    }

    public class SolucionSolucion
    {
        private string conceptoNegocio;
        private string componenteInnovador;
        private string productoServicio;
        private string proceso;
        private string aceptacionMercado;
        private string aspectoTecnicoProductivo;
        private string aspectoComercial;
        private string aspectoLegal;

        public string ConceptoNegocio
        {
            get { return this.conceptoNegocio.quitarCaracteres(); }
            set { this.conceptoNegocio = value; }
        }
        public string ComponenteInnovador
        {
            get { return this.componenteInnovador.quitarCaracteres(); }
            set { this.componenteInnovador = value; }
        }
        public string ProductoServicio
        {
            get { return this.productoServicio.quitarCaracteres(); }
            set { this.productoServicio = value; }
        }
        public string Proceso
        {
            get { return this.proceso.quitarCaracteres(); }
            set { this.proceso = value; }
        }
        public string AceptacionMercado
        {
            get { return this.aceptacionMercado.quitarCaracteres(); }
            set { this.aceptacionMercado = value; }
        }
        public string AspectoTecnicoProductivo
        {
            get { return this.aspectoTecnicoProductivo.quitarCaracteres(); }
            set { this.aspectoTecnicoProductivo = value; }
        }
        public string AspectoComercial
        {
            get { return this.aspectoComercial.quitarCaracteres(); }
            set { this.aspectoComercial = value; }
        }
        public string AspectoLegal
        {
            get { return this.aspectoLegal.quitarCaracteres(); }
            set { this.aspectoLegal = value; }
        }
    }

    public class OportunidadMercado
    {
        private string oportunidadMercado;

        public string OportunidadDeMercado
        {
            get { return this.oportunidadMercado.quitarCaracteres(); }
            set { this.oportunidadMercado = value; }
        }
    }

    public class OportunidadMercadoCompetidores
    {
        private string nombre;
        private string localizacion;
        private string productosServicios;
        private string precios;
        private string logisticaDistribucion;
        private string otroCual;

        public string Nombre
        {
            get { return this.nombre.quitarCaracteres(); }
            set { this.nombre = value; }
        }
        public string Localizacion
        {
            get { return this.localizacion.quitarCaracteres(); }
            set { this.localizacion = value; }
        }
        public string ProductosServicios
        {
            get { return this.productosServicios.quitarCaracteres(); }
            set { this.productosServicios = value; }
        }
        public string Precios
        {
            get { return this.precios.quitarCaracteres(); }
            set { this.precios = value; }
        }
        public string LogisticaDistribucion
        {
            get { return this.logisticaDistribucion.quitarCaracteres(); }
            set { this.logisticaDistribucion = value; }
        }
        public string OtroCual
        {
            get { return this.otroCual.quitarCaracteres(); }
            set { this.otroCual = value; }
        }
    }

    public class Protagonista
    {
        private string perfilConsumidor;
        private string cliente;
        private string consumidores;


        public string PerfilConsumidor
        {
            get { return this.perfilConsumidor.quitarCaracteres(); }
            set { this.perfilConsumidor = value; }
        }
        public string Cliente
        {
            get { return this.cliente.quitarCaracteres(); }
            set { this.cliente = value; }
        }
        public string Consumidores
        {
            get { return this.consumidores.quitarCaracteres(); }
            set { this.consumidores = value; }
        }

    }

    public class ProtagonistaCliente
    {
        private string nombreCliente;
        private string localizacion;
        private string perfil;
        private string justificacion;

        public string NombreCliente
        {
            get { return this.nombreCliente.quitarCaracteres(); }
            set { this.nombreCliente = value; }
        }
        public string Perfil
        {
            get { return this.perfil.quitarCaracteres(); }
            set { this.perfil = value; }
        }
        public string Localizacion
        {
            get { return this.localizacion.quitarCaracteres(); }
            set { this.localizacion = value; }
        }
        public string Justificacion
        {
            get { return this.justificacion.quitarCaracteres(); }
            set { this.justificacion = value; }
        }
    }
    public class DynamicDictionary : DynamicObject
    {
        // The inner dictionary.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        // If you try to get a value of a property
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }
    }

    public class EvalDatosGenerales
    {
        private string localizacion;
        private string sector;
        private string resumenConceptoGeneral;

        public string Localizacion
        {
            get { return this.localizacion.quitarCaracteres(); }
            set { this.localizacion = value; }
        }
        public string Sector
        {
            get { return this.sector.quitarCaracteres(); }
            set { this.sector = value; }
        }

        public string ResumenConceptoGeneral
        {
            get { return this.resumenConceptoGeneral.quitarCaracteres(); }
            set { this.resumenConceptoGeneral = value; }
        }
    }

    public class InfoTabEvaluacionProyecto
    {
        public int idCampo { get; set; }
        public string campo { get; set; }
        public string puntaje { get; set;}
        private string observacion { get; set; }
        public string Observacion
        {
            get { return this.observacion.quitarCaracteres(); }
            set { this.observacion = value; }
        }
    }
    public class InfoGeneralProyecto
    {
        private string nomProyecto;
        private string nomInstitucion;
        private string nomUnidad;
        private string codigoDane;
        private string nomCiudad;
        private string codigoCIIU;
        private string sector;
        private string subSector;

        public string fechaNacimiento { get; set; }
        public string genero { get; set; }
        public string nivelEstudio { get; set; }
        public string Programa { get; set; }
        public string Institucion { get; set; }
        public string Estado { get; set; }

        public int Id_proyecto { get; set; }
        public string NomProyecto
        {
            get => nomProyecto.quitarCaracteres();
            set { nomProyecto = value; }
        }
        public int CodInstitucion { get; set; }
        public string NomInstitucion
        {
            get { return this.nomInstitucion.quitarCaracteres(); }
            set { this.nomInstitucion = value; }
        }
        public string NomUnidad
        {
            get { return this.nomUnidad.quitarCaracteres(); }
            set { this.nomUnidad = value; }
        }
        public string CodigoDane
        {
            get { return this.codigoDane.quitarCaracteres(); }
            set { this.codigoDane = value; }
        }
        public string NomCiudad
        {
            get { return this.nomCiudad.quitarCaracteres(); }
            set { this.nomCiudad = value; }
        }
        public string CodigoCIIU
        {
            get { return this.codigoCIIU.quitarCaracteres(); }
            set { this.codigoCIIU = value; }
        }
        public string Sector
        {
            get { return this.sector.quitarCaracteres(); }
            set { this.sector = value; }
        }
        public string SubSector
        {
            get { return this.subSector.quitarCaracteres(); }
            set { this.subSector = value; }
        }
    }

    public class ListadoPlanesDeNegocio
    {
        public int IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public int CodigoInstitucion { get; set; }
        public int CodigoEstado { get; set; }
        public string NombreUnidad { get; set; }
        public string NombreInstitucion { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreDepartamento { get; set; }
        public int idDpto { get; set; }
        public int idTipoInstitucion { get; set; }
        public int idDptoInst { get; set; }
        public string Estado { get; set; }
        public int? codOperador { get; set; }
        public string NombreOperador { get; set; }
    }

    public class PlanDeCompra
    {
        private string tipoMateriaPrima;
        private string materiaPrima;
        private string unidad;
        private string cantidadPresentacion;
        private string margenDesperdicio;

        public string TipoMateriaPrima
        {
            get { return this.tipoMateriaPrima.quitarCaracteres(); }
            set { this.tipoMateriaPrima = value; }
        }
        public string MateriaPrima
        {
            get { return this.materiaPrima.quitarCaracteres(); }
            set { this.materiaPrima = value; }
        }
        public string Unidad
        {
            get { return this.unidad.quitarCaracteres(); }
            set { this.unidad = value; }
        }
        public string CantidadPresentacion
        {
            get { return this.cantidadPresentacion.quitarCaracteres(); }
            set { this.cantidadPresentacion = value; }
        }
        public string MargenDesperdicio
        {
            get { return this.margenDesperdicio.quitarCaracteres(); }
            set { this.margenDesperdicio = value; }
        }
    }

    public class ActividadesPlanOperativo
    {
        public int codProyecto { get; set; }
        private string item;
        private string actividad;
        public string Item
        {
            get { return this.item.quitarCaracteres(); }
            set { this.item = value; }
        }
        public string Actividad
        {
            get { return this.actividad.quitarCaracteres(); }
            set { this.actividad = value; }
        }
    }
}
