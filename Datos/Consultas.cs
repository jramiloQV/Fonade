using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Datos.DataType;


namespace Datos
{
    public class Consultas
    {
 
        private readonly string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        
        private readonly FonadeDBDataContext db;
        public SqlParameter[] Parameters;
    
        public Consultas()
        {
            db = new FonadeDBDataContext(_cadena);
        }

        public FonadeDBDataContext Db
        {
            get { return db; }
        }

        /// <summary>
        /// validacion de contacto
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidarContacto(string email, string password)
        {
            bool isValid = false;

            try
            {
                isValid = db.Contacto.Any(t => t.Email == email && t.Clave == password);
            }
            catch (Exception ex)
            {
                isValid = false;
            }                       

            return isValid;
        }
        /// <summary>
        /// obtiene contacto recibe como argumento el email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UsuarioFonade GetContacto(string email)
        {
            using (var db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from contactos in db.Contacto
                              from grupoContactos in db.GrupoContactos
                              where contactos.Id_Contacto == grupoContactos.CodContacto && contactos.Inactivo == false && contactos.Email == email
                              select new UsuarioFonade(
                                                      contactos.Nombres,
                                                      contactos.Apellidos,
                                                      contactos.Email,
                                                      grupoContactos.CodGrupo,
                                                      contactos.Id_Contacto,
                                                      contactos.CodInstitucion.HasValue ? contactos.CodInstitucion.Value : -1,
                                                      contactos.Identificacion,
                                                      contactos.fechaCreacion.HasValue ? contactos.fechaCreacion.Value : DateTime.Now,
                                                      contactos.fechaCambioClave.HasValue ? contactos.fechaCambioClave.Value : DateTime.Now,
                                                      contactos.Clave,
                                                      contactos.AceptoTerminosYCondiciones.GetValueOrDefault(false),
                                                      contactos.codOperador
                                                      )
                                        ).FirstOrDefault();
                return entity;
            }
        }

        /// <summary>
        /// crea contactos
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CrearContacto(
            string username,
            string password,
            string email)
        {
            return true;
        }


        /// <summary>
        /// obtiene el grupo
        /// </summary>
        /// <param name="codGrupo"></param>
        /// <returns></returns>
        public Grupo GetGrupo(int codGrupo)
        {
            return db.Grupos.First(t => t.Id_Grupo == codGrupo);
        }

        public List<Grupo> GetGrupos()
        {
            return db.Grupos.ToList();
        }
        /// <summary>
        /// visualiza en lista el estudio del asesor
        /// </summary>
        /// <param name="Id_usuario"></param>
        /// <returns></returns>
        public List<MD_VerEstudiosAsesorResult> VerEstudiosAsesor(int Id_usuario)
        {
            List<MD_VerEstudiosAsesorResult> tasks = db.MD_VerEstudiosAsesor(Id_usuario).ToList();
            return tasks;
        }
        /// <summary>
        /// visualiza en lista la formalizacion de proyectos
        /// </summary>
        /// <param name="Id_usuario"></param>
        /// <param name="anexos"></param>
        /// <param name="cod_institucion"></param>
        /// <param name="inscripcion"></param>
        /// <returns></returns>
        public List<MD_FormalizarProyectoResult> FormalizarProyecto(int Id_usuario, int anexos, int cod_institucion,
                                                                    int inscripcion)
        {
            List<MD_FormalizarProyectoResult> tasks =
                db.MD_FormalizarProyecto(Id_usuario, anexos, cod_institucion, inscripcion).ToList();
            return tasks;
        }
        /// <summary>
        /// visualiza formalizacion
        /// </summary>
        /// <param name="Id_proyecto"></param>
        /// <param name="rolasesor"></param>
        /// <param name="rolasesorlider"></param>
        /// <param name="rolemprendedor"></param>
        /// <param name="caso"></param>
        /// <returns></returns>
        public List<MD_VerFormalizacionResult> VerFormalizacion(int Id_proyecto, int rolasesor, int rolasesorlider,
                                                                int rolemprendedor, int caso)
        {
            List<MD_VerFormalizacionResult> tasks =
                db.MD_VerFormalizacion(Id_proyecto, rolasesor, rolasesorlider, rolemprendedor, caso).ToList();
            return tasks;
        }

        /// <summary>
        /// visualiza beneficiarios
        /// </summary>
        /// <param name="Id_proyecto"></param>
        /// <returns></returns>
        public List<MD_MostrarBeneficiariosResult> MostrarBeneficiarios(int Id_proyecto)
        {
            List<MD_MostrarBeneficiariosResult> tasks = db.MD_MostrarBeneficiarios(Id_proyecto).ToList();
            return tasks;
        }

        /// <summary>
        /// visualiza programas academicos
        /// </summary>
        /// <param name="Ciudad"></param>
        /// <param name="Nivel"></param>
        /// <param name="institucion"></param>
        /// <param name="programa"></param>
        /// <returns></returns>
        public List<MD_MostrarProgramasAcademicosResult> MostrarProgramasAcademicos(int Ciudad, int Nivel,
                                                                                    string institucion, string programa)
        {
            List<MD_MostrarProgramasAcademicosResult> tasks =
                db.MD_MostrarProgramasAcademicos(Ciudad, Nivel, institucion, programa).ToList();
            return tasks;
        }
      
        /// <summary>
        /// visualiza  archivos offline
        /// </summary>
        /// <param name="CodContacto"></param>
        /// <returns></returns>
        public List<MD_MostrarArchivosOfflineResult> MostrarArchivosOffline(int CodContacto)
        {
            List<MD_MostrarArchivosOfflineResult> tasks = db.MD_MostrarArchivosOffline(CodContacto).ToList();
            return tasks;
        }
        
        /// <summary>
        /// visualiza formalizacion emprendedores
        /// </summary>
        /// <param name="Id_proyecto"></param>
        /// <param name="rolemprendedor"></param>        
        public List<MD_VerFormalizacionEmprendedorResult> VerFormalizacionEmprendedor(int Id_proyecto,
                                                                                      int rolemprendedor)
        {
            List<MD_VerFormalizacionEmprendedorResult> tasks =
                db.MD_VerFormalizacionEmprendedor(Id_proyecto, rolemprendedor).ToList();
            return tasks;
        }
        /// <summary>
        /// visualiza formalizacion de empleos
        /// </summary>
        /// <param name="Id_proyecto"></param>
        /// <returns></returns>
        public List<MD_VerFormalizacionEmpleosResult> VerFormalizacionEmpleos(int Id_proyecto)
        {
            List<MD_VerFormalizacionEmpleosResult> tasks = db.MD_VerFormalizacionEmpleos(Id_proyecto).ToList();
            return tasks;
        }
        /// <summary>
        /// visualiza emprendedores inactivos
        /// </summary>
        /// <param name="CodigoIntitucion"></param>
        /// <param name="ConstRolEmprendedor"></param>
        /// <param name="caso"></param>
        /// <param name="whereSeleccion"></param>
        /// <returns></returns>
        public List<MD_VerEmprendedoresInactResult> VerEmprendedoresInact(int CodigoIntitucion, int ConstRolEmprendedor,
                                                                          string caso, string whereSeleccion)
        {
            List<MD_VerEmprendedoresInactResult> tasks =
                db.MD_VerEmprendedoresInact(CodigoIntitucion, ConstRolEmprendedor, caso, whereSeleccion).ToList();
            return tasks;
        }


        /// <summary>
        /// retorna consulta planes de negocio
        /// </summary>
        /// <param name="codeGroup"></param>
        /// <param name="codInstitucion"></param>
        /// <param name="codUsuario"></param>
        /// <returns></returns>
        public List<PlanDeNegocio> misPlanesDeNegocio(int codeGroup, int codInstitucion, int codUsuario)
        {
            var consulta = from p in db.Proyecto
                           from c in db.Ciudad
                           from d in db.departamento
                           from i in db.Institucions
                           where c.Id_Ciudad == p.CodCiudad
                                 & c.CodDepartamento == d.Id_Departamento
                                 & p.CodInstitucion == i.Id_Institucion
                           select new PlanDeNegocio
                           {
                               IdProyecto = p.Id_Proyecto,
                               NombreProyecto = p.NomProyecto,
                               CodigoInstitucion = i.Id_Institucion,
                               CodigoEstado = p.CodEstado,
                               NombreUnidad = i.NomUnidad,
                               NombreInstitucion = i.NomInstitucion,
                               NombreCiudad = c.NomCiudad,
                               NombreDepartamento = d.NomDepartamento,
                               Inactivo = p.Inactivo
                           };

            List<PlanDeNegocio> result = new List<PlanDeNegocio>();

            switch (codeGroup)
            {
                case Constantes.CONST_AdministradorSistema:
                case Constantes.CONST_AdministradorSena:
                    result = consulta.Where(p => p.Inactivo == false).ToList();
                    break;
                case Constantes.CONST_JefeUnidad:
                    result = consulta.Where(p => p.CodigoInstitucion == codInstitucion).ToList();
                    break;
                case Constantes.CONST_Asesor:
                case Constantes.CONST_Emprendedor:
                    var tempConsult = db.ProyectoContactos.Where(p => p.Proyecto.Id_Proyecto == p.CodProyecto
                                                                      && p.CodContacto == codUsuario
                                                                      && p.Inactivo == false).Select(t => t.CodProyecto);

                    result = consulta.Where(p => tempConsult.Contains(p.IdProyecto)
                                                 && p.Inactivo == false
                                                 && p.CodigoInstitucion == codInstitucion).ToList();
                    break;
                case Constantes.CONST_Evaluador:
                case Constantes.CONST_CoordinadorEvaluador:
                    var tempConsult1 = db.ProyectoContactos.Where(p => p.Proyecto.Id_Proyecto == p.CodProyecto
                                                                       && p.CodContacto == codUsuario
                                                                       && p.Inactivo == false).Select(t => t.CodProyecto);

                    result = consulta.Where(p => tempConsult1.Contains(p.IdProyecto)
                                                 && p.Inactivo == false
                                                 && p.CodigoEstado == Constantes.CONST_Evaluacion).ToList();
                    break;
                case Constantes.CONST_GerenteEvaluador:
                    result = consulta.Where(p => p.Inactivo == false
                                                 && (p.CodigoEstado == Constantes.CONST_Convocatoria
                                                     || p.CodigoEstado == Constantes.CONST_Evaluacion)).ToList();
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// retorna las tareas x asesor
        /// </summary>
        /// <param name="usuario"></param>        
        public List<RetornarTareasAsesorResult> misTareas(int usuario)
        {
            List<RetornarTareasAsesorResult> tareas = db.RetornarTareasAsesor(usuario).ToList();
            return tareas;
        }

        /// <summary>
        /// obtiene el usuario recibe por argumento el email
        /// </summary>               
        public List<UsuarioFonade> ObtenerUsuario(string email)
        {
            try
            {
                List<UsuarioFonade> Users = ((from c in db.Contacto
                                              from gc in db.GrupoContactos
                                              where c.Id_Contacto == gc.CodContacto && c.Inactivo == false
                                              select new UsuarioFonade
                                                  (
                                                  c.Nombres,
                                                  c.Apellidos,
                                                  c.Email, gc.CodGrupo,
                                                  c.Id_Contacto,
                                                  c.CodInstitucion.HasValue ? c.CodInstitucion.Value : -1,
                                                  c.Identificacion,
                                                  c.fechaCreacion.HasValue ? c.fechaCreacion.Value : DateTime.Now,
                                                  c.fechaCambioClave.HasValue ? c.fechaCambioClave.Value : DateTime.Now,
                                                  c.Clave, c.AceptoTerminosYCondiciones.GetValueOrDefault(false),
                                                  c.codOperador
                                                  ))
                                                  .Distinct())
                                                  .ToList();
                return Users;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// visualiza resumen de equipos
        /// </summary>
        /// <param name="rolEmprendedor"></param>
        /// <param name="rolAsesor"></param>
        /// <param name="rolAsesorLider"></param>
        /// <param name="codigoProyecto"></param>
        /// <param name="caso"></param>
        /// <returns></returns>
        public List<MD_Mostrar_resumen_equipoResult> Mostrar_resumen_equipo(int rolEmprendedor, int rolAsesor,
                                                                            int rolAsesorLider, int codigoProyecto,
                                                                            string caso)
        {
            List<MD_Mostrar_resumen_equipoResult> tasks =
                db.MD_Mostrar_resumen_equipo(rolEmprendedor, rolAsesor, rolAsesorLider, codigoProyecto, caso).ToList();
            return tasks;
        }

        /// <summary>
        /// visualiza proyectos a priorizar
        /// </summary>
        /// <param name="CodigoEstado"></param>
        /// <returns></returns>
        public List<MD_MostrarProyectosaPriorizarResult> MostrarProyectosaPriorizar(int CodigoEstado, int codOperador)
        {
            List<MD_MostrarProyectosaPriorizarResult> tasks = db.MD_MostrarProyectosaPriorizar(CodigoEstado, codOperador).ToList();
            return tasks;
        }

        /// <summary>
        /// visualiza actass asignacion de recursos
        /// </summary>
        /// <param name="IdActa"></param>
        /// <param name="caso"></param>
        /// <returns></returns>
        public List<MD_VerActaAsignacionRecursosResult> VerActaAsignacionRecursos(int IdActa, string caso)
        {
            List<MD_VerActaAsignacionRecursosResult> tasks = db.MD_VerActaAsignacionRecursos(IdActa).ToList();
            return tasks;
        }
        
        /// <summary>
        /// imprime actas asignacion
        /// </summary>
        /// <param name="IdActa"></param>
        /// <returns></returns>
        public List<MD_ImprimirActaAsignacionResult> ImprimirActaAsignacion(int IdActa)
        {
            List<MD_ImprimirActaAsignacionResult> tasks = db.MD_ImprimirActaAsignacion(IdActa).ToList();
            return tasks;
        }
        
        /// <summary>
        /// ver listado de actas
        /// </summary>
        /// <returns></returns>
        public List<MD_VerListadoActasResult> VerListadoActas()
        {
            List<MD_VerListadoActasResult> tasks = db.MD_VerListadoActas().ToList();
            return tasks;
        }
        
        /// <summary>
        /// visualiza los listados convocatorias
        /// </summary>
        /// <returns></returns>
        public List<MD_VerListadoConvocatoriasResult> VerListadoConvocatorias()
        {
            List<MD_VerListadoConvocatoriasResult> tasks = db.MD_VerListadoConvocatorias().ToList();
            return tasks;
        }
        
        /// <summary>
        /// muestra proyectos por convocatoria
        /// </summary>
        /// <param name="idConvoct"></param>
        /// <returns></returns>
        public List<MD_MostrarProyectosPorConvocatoriaResult> MostrarProyectosPorConvocatoria(int idConvoct)
        {
            List<MD_MostrarProyectosPorConvocatoriaResult> tasks =
                db.MD_MostrarProyectosPorConvocatoria(idConvoct).ToList();
            return tasks;
        }
        
        /// <summary>
        /// visualizacion reglas convocatoria
        /// </summary>
        public List<MD_VerReglasConvocatoriaResult> VerReglasConvocatoria(int idConvoct)
        {
            List<MD_VerReglasConvocatoriaResult> tasks = db.MD_VerReglasConvocatoria(idConvoct).ToList();
            return tasks;
        }
        /// <summary>
        /// muestra convocatoria criterios priorizacion
        /// </summary>
        public List<MD_Mostrar_ConvocatoriaCriterioPriorizacionResult> MostrarConvocatoriaCriterioPriorizacion(
            int idConvoct)
        {
            List<MD_Mostrar_ConvocatoriaCriterioPriorizacionResult> tasks =
                db.MD_Mostrar_ConvocatoriaCriterioPriorizacion(idConvoct).ToList();
            return tasks;
        }
        /// <summary>
        /// lista de criterios
        /// </summary>        
        public List<MD_Mostrar_ListadoCriteriosResult> Mostrar_ListadoCriterios(int idConvoct)
        {
            List<MD_Mostrar_ListadoCriteriosResult> tasks = db.MD_Mostrar_ListadoCriterios(idConvoct).ToList();
            return tasks;
        }
        /// <summary>
        /// lista catalogo insumo
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="tipoinsumo"></param>
        /// <param name="producto"></param>        
        public List<MD_listarCatalogoInsumoResult> listarCatalogoInsumo(int proyecto, int tipoinsumo, int producto)
        {
            List<MD_listarCatalogoInsumoResult> tasks =
                db.MD_listarCatalogoInsumo(proyecto, tipoinsumo, producto).ToList();
            return tasks;
        }


        public List<MD_txt_insumoResult> txt_insumo()
        {
            List<MD_txt_insumoResult> tasks = db.MD_txt_insumo().ToList();
            return tasks;
        }

        /// <summary>
        /// lista que arroja consulta x usuario grupo e institucion
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="codgrupo"></param>
        /// <param name="institucion"></param>
        /// <param name="accion"></param>
        /// <param name="palabraclave"></param>
        /// <returns></returns>
        public List<MD_ConsultarResult> Consultar(int usuario, int codgrupo, int institucion, string accion,
                                                  string palabraclave)
        {
            List<MD_ConsultarResult> tasks =
                db.MD_Consultar(usuario, codgrupo, institucion, accion, palabraclave).ToList();
            return tasks;
        }
        
        /// <summary>
        /// visualiza integrantes centrales riesgos
        /// </summary>
        /// <param name="codproyecto"></param>
        /// <param name="rolemprendedor"></param>
        /// <param name="codconvocatoria"></param>
        /// <returns></returns>
        public List<MD_MostrarIntegrantesCentralesRiesgosResult> MostrarIntegrantesCentralesRiesgos(int codproyecto,
                                                                                                    int rolemprendedor,
                                                                                                    int codconvocatoria)
        {
            List<MD_MostrarIntegrantesCentralesRiesgosResult> tasks =
                db.MD_MostrarIntegrantesCentralesRiesgos(codproyecto, rolemprendedor, codconvocatoria).ToList();
            return tasks;
        }
        
        /// <summary>
        /// evaluacion rubro proyecto
        /// </summary>
        /// <param name="codproyecto"></param>
        /// <param name="codconvocatoria"></param>
        /// <returns></returns>
        public List<MD_MostrarEvaluacionRubroProyectoResult> MostrarEvaluacionRubroProyecto(int codproyecto,
                                                                                            int codconvocatoria)
        {
            List<MD_MostrarEvaluacionRubroProyectoResult> tasks =
                db.MD_MostrarEvaluacionRubroProyecto(codproyecto, codconvocatoria).ToList();
            return tasks;
        }
        
        /// <summary>
        /// coordinadores de evaluacion
        /// </summary>
        /// <returns></returns>
        public List<MD_VerCoordinadoresDEEvaluacionResult> VerCoordinadoresDeEvaluacion(int? idOperador)
        {
            List<MD_VerCoordinadoresDEEvaluacionResult> tasks = db.MD_VerCoordinadoresDEEvaluacion(idOperador).ToList<MD_VerCoordinadoresDEEvaluacionResult>();
            return tasks;
        }
        
        /// <summary>
        /// visualizador evaluador x proyectos
        /// </summary>
        /// <param name="contacto"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public List<MD_VerEvaluadoresDeCoordinadorResult> VerEvaluadoresDeCoordinador(int contacto, int estado)
        {
            List<MD_VerEvaluadoresDeCoordinadorResult> tasks = db.MD_VerEvaluadoresDeCoordinador(contacto, estado).ToList<MD_VerEvaluadoresDeCoordinadorResult>();
            return tasks;
        }
        
        /// <summary>
        /// proyectos evaluador
        /// </summary>
        /// <param name="contacto"></param>
        /// <param name="evaluacion"></param>
        /// <returns></returns>
        public List<MD_VerProyectosEvaluadorResult> VerProyectosEvaluador(int contacto, int evaluacion)
        {
            List<MD_VerProyectosEvaluadorResult> tasks = db.MD_VerProyectosEvaluador(contacto, evaluacion).ToList<MD_VerProyectosEvaluadorResult>();
            return tasks;
        }
        
        /// <summary>
        /// retorna lista evaluadores
        /// </summary>        
        public List<MD_VerEvaluadoresResult> VerEvaluadores(int? idOperador)
        {
            List<MD_VerEvaluadoresResult> tasks = db.MD_VerEvaluadores(idOperador).ToList<MD_VerEvaluadoresResult>();
            return tasks;
        }

        public List<MD_VerGerenteIntervResult> VerGerenteInterv()
        {
            List<MD_VerGerenteIntervResult> tasks = db.MD_VerGerenteInterv().ToList<MD_VerGerenteIntervResult>();
            return tasks;
        }

        /// <summary>
        /// retorna proyectos evaluacion
        /// </summary>
        public List<MD_cargarProyectosEvalResult> CargarProyectosEval(int contacto, int constRoleval)
        {
            List<MD_cargarProyectosEvalResult> tasks = db.MD_cargarProyectosEval(contacto, constRoleval).ToList<MD_cargarProyectosEvalResult>();
            return tasks;
        }

        /// <summary>
        /// retorna proyectos interventoria
        /// </summary>
        /// <param name="contacto"></param>
        /// <param name="constRolinterv"></param>
        /// <param name="constRolintervLider"></param>        
        public List<MD_cargarProyectosIntervResult> CargarProyectosInterv(int contacto, int constRolinterv, int constRolintervLider)
        {
            List<MD_cargarProyectosIntervResult> tasks = db.MD_cargarProyectosInterv(contacto, constRolinterv, constRolintervLider).ToList<MD_cargarProyectosIntervResult>();
            return tasks;
        }

        /// <summary>
        /// retorna lista coordinador asignado
        /// </summary>
        /// <param name="contacto"></param>        
        public List<MD_VerCoordinadorAsignadoResult> VerCoordinadorAsignado(int contacto)
        {
            List<MD_VerCoordinadorAsignadoResult> tasks = db.MD_VerCoordinadorAsignado(contacto).ToList<MD_VerCoordinadorAsignadoResult>();
            return tasks;
        }

        /// <summary>
        /// retorna lista proyectos x sector
        /// </summary>
        /// <param name="proyecto"></param>        
        public List<MD_cargarProyectosSectorResult> cargarProyectosSector(int proyecto)
        {
            List<MD_cargarProyectosSectorResult> tasks = db.MD_cargarProyectosSector(proyecto).ToList<MD_cargarProyectosSectorResult>();
            return tasks;
        }

        /// <summary>
        /// retorna informes visitas interventoria
        /// </summary>
        /// <param name="CodGrupo"></param>
        /// <param name="IdContacto"></param>        
        public List<MD_InformeVistaInterventoriaResult> VerInformeVisitasInterventoria(int CodGrupo, int IdContacto)
        {
            List<MD_InformeVistaInterventoriaResult> tasks = db.MD_InformeVistaInterventoria(CodGrupo, IdContacto).ToList<MD_InformeVistaInterventoriaResult>();
            return tasks;
        }

        /// retorna lista informe bimensual interventoria
        /// </summary>
        /// <param name="CodGrupo"></param>
        /// <param name="IdContacto"></param>        

        public List<MD_InformeBimensualInterventoriaResult> VerInformeBimensualInterventoria(int CodGrupo, int IdContacto)
        {
            List<MD_InformeBimensualInterventoriaResult> tasks = db.MD_InformeBimensualInterventoria(CodGrupo, IdContacto).ToList<MD_InformeBimensualInterventoriaResult>();
            return tasks;
        }

        /// retorna informe ejecucion
        /// </summary>
        /// <param name="CodGrupo"></param>
        /// <param name="IdContacto"></param>        
        public List<MD_InformeEjecucionInterventoriaResult> VerInformeEjecucionInterventoria(int CodGrupo, int IdContacto)
        {
            List<MD_InformeEjecucionInterventoriaResult> tasks = db.MD_InformeEjecucionInterventoria(CodGrupo, IdContacto).ToList<MD_InformeEjecucionInterventoriaResult>();
            return tasks;
        }

        /// retorna lista consolidado consultoria
        /// </summary>
        /// <param name="CodGrupo"></param>
        /// <param name="IdContacto"></param>        
        public List<MD_InformeConsolidadoInterventoriaResult> VerInformeConsolidadoInterventoria(int CodGrupo, int IdContacto)
        {
            List<MD_InformeConsolidadoInterventoriaResult> tasks = db.MD_InformeConsolidadoInterventoria(CodGrupo, IdContacto).ToList<MD_InformeConsolidadoInterventoriaResult>();
            return tasks;
        }

        /// Cargar proyecto sumario
        /// </summary>
        /// <param name="proyecto"></param>        
        public List<MD_cargarProyectoSumarioActualResult> cargarProyectoSumarioActual(int proyecto)
        {
            List<MD_cargarProyectoSumarioActualResult> tasks = db.MD_cargarProyectoSumarioActual(proyecto).ToList<MD_cargarProyectoSumarioActualResult>();
            return tasks;
        }

        /// visualiza eventos coordinador
        /// </summary>
        /// <param name="contacto"></param>
        public List<MD_VerInterventoresDeCoordinadorResult> VerInterventoresDeCoordinador(int contacto)
        {
            List<MD_VerInterventoresDeCoordinadorResult> tasks = db.MD_VerInterventoresDeCoordinador(contacto).ToList<MD_VerInterventoresDeCoordinadorResult>();
            return tasks;
        }

        /// vissualiza grid proyectos interventor
        /// </summary>
        /// <param name="contacto"></param>
        /// <param name="estado"></param>        
        public List<MD_VerProyectosInterventorResult> VerProyectosInterventor(int contacto, int estado)
        {
            List<MD_VerProyectosInterventorResult> tasks =
                db.MD_VerProyectosInterventor(contacto, estado).ToList();
            return tasks;
        }

        /// <summary>
        /// Obtener infraestructura
        /// </summary>
        /// <param name="codigoProyecto"></param>        
        public List<MD_ObtenerInfraestructuraResult> ObtenerInfraestructura(string codigoProyecto)
        {
            var infraestructura = db.MD_ObtenerInfraestructura(codigoProyecto).ToList();

            return infraestructura;
        }

        /// Obtiene los integrantes iniciativa
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="convocatoria"></param>
        public List<MD_GetIntegrantesIniciativaResult> ObtenerIntegrantesIniciativa(string proyecto, string convocatoria)
        {
            var integrantes = db.MD_GetIntegrantesIniciativa(proyecto, convocatoria).ToList();

            return integrantes;
        }

        /// obtiene los aportes argumentos proyecto y convocatoria
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="convocatoria"></param>        
        public List<MD_GetAportesResult> ObtenerAportes(string proyecto, string convocatoria)
        {
            var aportes = db.MD_GetAportes(proyecto, convocatoria).ToList();

            return aportes;
        }

        /// obtiene los tabs del formulario
        /// </summary>
        /// <param name="proyecto"></param>        
        public int ObtenerTabs(int proyecto)
        {
            var codproyecto = db.MD_obtenerTabs(proyecto).First().codproyecto;

            return codproyecto;
        }
        

        /// Crud proyecto infraestructura 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="proyecto"></param>
        /// <param name="text"></param>        
        public ISingleResult<MD_ObtenerProyectoOperacionInfraestructuraResult> CrudProyectoInfraestructura(int tipo, int proyecto, string text, ref string MensajeDeError)
        {
            ISingleResult<MD_ObtenerProyectoOperacionInfraestructuraResult> tasks = null;
            try
            {
                tasks = db.MD_ObtenerProyectoOperacionInfraestructura(tipo, proyecto, text);
            }
            catch (Exception ex)
            {
                MensajeDeError = string.Format("Se presento un error : {0}, Tipo : {1}", ex.Message, ex.StackTrace);
            }

            return tasks;
        }

        /// obtiene campos evaluaciones observaciones
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="convocatoria"></param>
        /// <param name="idcampo"></param>
        /// <param name="mensajeDeError"></param>        
       public List<MD_ObtenerCamposEvaluacionObservacionesResult> ObtenerCamposEvaluacionObservaciones(int proyecto, int convocatoria, int idcampo, ref string mensajeDeError)
        {
            List<MD_ObtenerCamposEvaluacionObservacionesResult> var = null;

            try
            {
                var = db.MD_ObtenerCamposEvaluacionObservaciones(proyecto, convocatoria, idcampo).ToList();
            }
            catch (Exception ex)
            {
                mensajeDeError = string.Format("Error al consultar los aspectos : {0} ,{1}", ex.Message,
                                               ex.StackTrace);
            }

            return var;
        }

        /// obtiene campos evaluacion observaciones detalle
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyecto"></param>
        /// <param name="convocatoria"></param>
        /// <param name="idcampo"></param>
        /// <param name="mensajeDeError"></param>        
        public List<MD_ObtenerCamposEvaluacionObservacionesHijasResult> ObtenerCamposEvaluacionObservacionesHijas(
            string campo, int proyecto, int convocatoria, int idcampo, ref string mensajeDeError)
        {
            List<MD_ObtenerCamposEvaluacionObservacionesHijasResult> var = null;

            try
            {
                var = db.MD_ObtenerCamposEvaluacionObservacionesHijas(Convert.ToInt32(campo), proyecto, convocatoria, idcampo).ToList();
            }
            catch (Exception ex)
            {
                mensajeDeError = string.Format("Error al consultar los aspectos : {0} ,{1}", ex.Message, ex.StackTrace);
            }

            return var;
        }

        /// obtiene los campos de la hoja de evaluacion
        /// </summary>
        /// <param name="codproyecto"></param>
        /// <param name="mensaje"></param>       
        public List<MD_ObtenerInformacionEvaluacionResult> ObtenercamposEvaluacion(int codproyecto, ref string mensaje)
        {
            List<MD_ObtenerInformacionEvaluacionResult> consulta = null;
            try
            {
                consulta = db.MD_ObtenerInformacionEvaluacion(codproyecto).ToList();
            }
            catch (Exception ex)
            {
                mensaje = string.Format("se presento un error : {0}, de tipo : {1}", ex.Message, ex.StackTrace);
            }

            return consulta;
        }

        /// Obtiene la hoja de Evaluacion
        /// </summary>
        /// <param name="codgrupo"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public List<MD_HojaAvanceEvaluacionResult> HojaEvaluacion(int idOpcion,int idContacto, ref string mensaje)
        {
            List<MD_HojaAvanceEvaluacionResult> consulta = null;
            try
            {
                consulta = db.MD_HojaAvanceEvaluacion(idContacto,idOpcion).ToList();
            }
            catch (Exception ex)
            {
                mensaje = string.Format("se presento un error obteniendo la hoja de avance : {0}, de tipo : {1}", ex.Message, ex.StackTrace);
            }

            return consulta;
        }

        public DataTable ObtenerDataTable(string procedimiento, string typo = "")
        {
            var myDataTable = new DataTable("Controles");
            

            using(var connection = new SqlConnection(_cadena))
            {
                try
                {
                    if (!string.IsNullOrEmpty(procedimiento) && procedimiento != "")
                    {
                        var adapter = new SqlDataAdapter(procedimiento, connection);

                        if (typo == "text")
                        {
                            adapter.SelectCommand.CommandType = CommandType.Text;
                            adapter.SelectCommand.CommandTimeout = 0;
                        }
                        else
                        {
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        }

                        if (Parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(Parameters);
                        }

                        connection.Open();
                        adapter.Fill(myDataTable);
                        adapter.SelectCommand.Connection.Close();
                        Parameters = null;
                    }
                }
                catch (Exception) { throw; }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            
            return myDataTable;
        }

        public DataSet ObtenerDataSet(string procedimiento, string typo = "")
        {

            var myDataSet = new DataSet("Dataset");
            
            using(var connection = new SqlConnection(_cadena))
            {
                try
                {
                    if (!string.IsNullOrEmpty(procedimiento) && procedimiento != "")
                    {
                        var adapter = new SqlDataAdapter(procedimiento, connection);


                        if (typo == "text")
                        {
                            adapter.SelectCommand.CommandType = CommandType.Text;
                        }
                        else
                        {

                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        }

                        if (Parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(Parameters);
                        }

                        adapter.SelectCommand.Connection.Open();
                        adapter.Fill(myDataSet);
                        Parameters = null;
                    }
                }
                catch (Exception exception)
                {
                    Parameters = null;
                    throw new Exception(string.Format("se ha generado un error de tipo: {0}, al tratar de consultar : {1}",
                                                      exception.Message,
                                                      exception.StackTrace));
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return myDataSet;
        }

        public bool Eliminar(string procedimiento, int parametro)
        {
            bool bandera = false;

            using(var connection = new SqlConnection(_cadena))
            {
                try
                {
                    if (!string.IsNullOrEmpty(procedimiento) && procedimiento != "")
                    {
                        var adapter = new SqlDataAdapter(procedimiento.Trim(), connection)
                        {
                            SelectCommand =
                            {
                                CommandType = CommandType.StoredProcedure
                            }
                        };
                        if (Parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(Parameters);
                        }

                        adapter.SelectCommand.Connection.Open();
                        bandera = Convert.ToBoolean(adapter.SelectCommand.ExecuteScalar());
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("se ha generado un error de tipo: {1}, al tratar de eliminar  : {0}",
                                                      exception.Message,
                                                      exception.StackTrace));
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            
            return bandera;
        }

        public bool InsertarDataTable(string procedimiento)
        {
            bool bandera = false;
            using(var connection = new SqlConnection(_cadena))
            {
                try
                {
                    if (!string.IsNullOrEmpty(procedimiento) && procedimiento != "")
                    {
                        var adapter = new SqlDataAdapter(procedimiento.Trim(), connection)
                        {
                            SelectCommand =
                            {
                                CommandType = CommandType.StoredProcedure
                            }
                        };
                        if (Parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(Parameters);
                        }

                        adapter.SelectCommand.Connection.Open();
                        bandera = Convert.ToBoolean(adapter.SelectCommand.ExecuteScalar());
                        adapter.SelectCommand.Connection.Close();
                        Parameters = null;
                    }
                }
                catch (Exception exception)
                {
                    Parameters = null;
                    throw new Exception(
                        string.Format("se ha generado un error al tratar de insertar el excel : {0},  tipo: {1}",
                                      exception.Message,
                                      exception.StackTrace));
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            
            return bandera;
        }


        public object RetornarEscalar(string procedimiento, string tipo)
        {
            bool bandera = false;
            using(var connection = new SqlConnection(_cadena))
            {
                try
                {
                    if (!string.IsNullOrEmpty(procedimiento) && procedimiento != "")
                    {
                        var adapter = new SqlDataAdapter(procedimiento, connection);

                        if (tipo == "text")
                        {
                            adapter.SelectCommand.CommandType = CommandType.Text;
                        }
                        else
                        {

                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        }

                        if (Parameters != null)
                        {
                            adapter.SelectCommand.Parameters.AddRange(Parameters);
                        }

                        adapter.SelectCommand.Connection.Open();
                        var miobject = adapter.SelectCommand.ExecuteScalar();
                        adapter.SelectCommand.Connection.Close();
                        Parameters = null;
                        return miobject;
                    }
                }
                catch (Exception exception)
                {
                    Parameters = null;
                    throw new Exception(
                        string.Format("se ha generado un error al tratar de insertar el excel : {0},  tipo: {1}",
                                      exception.Message,
                                      exception.StackTrace));
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return bandera;
        }


        ///Retorna Info actualizacion de la pagina
         /// </summary>
        /// <param name="codProyecto"></param>
        /// <param name="codConvocatoria"></param>
        /// <param name="idTab"></param>
        /// <returns></returns>
        public IQueryable<InfoActualiza> RetornarInformacionActualizaPagina(int codProyecto, int codConvocatoria, int idTab)
        {
            var consultas = new Consultas();
            var usuActualizo = (from c in consultas.Db.Contacto
                                join tab in consultas.Db.TabEvaluacionProyectos on c.Id_Contacto equals tab.CodContacto
                                where tab.CodProyecto == codProyecto && tab.CodConvocatoria == codConvocatoria &&
                                tab.CodTabEvaluacion == idTab
                                select new InfoActualiza()
                                {
                                    nombres = c.Nombres + " " + c.Apellidos,
                                    fecha = tab.FechaModificacion,
                                    realizado = tab.Realizado
                                });

            return usuActualizo;
        }
        
        /// Reconocimiento postback al retorno de pagina --tabs
        /// </summary>
        /// <param name="codProyecto"></param>
        /// <param name="idTab"></param>        
        public IQueryable<InfoActualiza> RetornarInformacionActualizaPPagina(int codProyecto, int idTab)
        {
            var consultas = new Consultas();
            var usuActualizo = (from t in consultas.db.TabProyectos
                                join c in consultas.db.Contacto on t.CodContacto equals c.Id_Contacto
                                where t.CodProyecto == codProyecto && t.CodTab == idTab
                                select new InfoActualiza()
                                {
                                    nombres = c.Nombres + " " + c.Apellidos,
                                    fecha = t.FechaModificacion,
                                    realizado = t.Realizado
                                });
			

            return usuActualizo;

			
        }

        public class InfoActualiza
        {
            public string nombres { get; set; }
            public DateTime fecha { get; set; }
            public bool realizado { get; set; }
        }

    }
}