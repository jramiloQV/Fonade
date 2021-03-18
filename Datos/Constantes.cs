using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos
{
    public static class Constantes
    {
        //        
        //Constantes para el manejo de grupo de usuarios
        //        
        #region Constantes Evaluador

        public const int ConstModeloFinanciero = 1;
        public const int ConstSubIndicadoresFinancieros = 2;
        public const int ConstSubModeloFinanciero = 3;
        public const int ConstTablaEvaluacion = 4;
        public const int ConstSubGenerales = 5;
        public const int ConstSubComerciales = 6;
        public const int ConstSubTecnicos = 7;
        public const int ConstSubOrganizacionales = 8;
        public const int ConstSubFinancieros = 9;
        public const int ConstSubMedioAmbiente = 10;
        public const int ConstInformeConsolidado = 11;
        public const int ConstSubAspectosEvaluados = 12;
        public const int ConstSubObservaciones = 13;
        public const int ConstSubConclusion = 14;
        public const int ConstIndicadoresGestion = 15;
        public const int ConstSubProductosIndicadores = 17;
        public const int ConstSubEvaluacionProyecto = 20;
        public const int ConstSubFlujoCaja = 22;
        public const int ConstSubRiesgosIdentificados = 23;
        public const int ConstSubCentralesRiesgo = 21;
        public const int ConstInformes = 24;
        public const int ConstDesempenoEvaluador = 19;
        public const int ConstPlanOperativoEval = 25;
        public const int ConstSubPlanOperativoEval = 26;
        public const int ConstSubNomina = 27;
        public const int SubProduccion = 28;
        public const int ConstSubVentas = 29;
        public const int ConstHojaAvance = 30;

        #endregion

        // constantes para el windows open -- propiedades

        public static string parametrosOpen = "menubar={0},scrollbars={1},width={2},height={3},top={4}";

        public const int CONST_GerenteAdministrador = 1;
        public const int CONST_AdministradorSistema = 2;
        public const int CONST_AdministradorSena = 3;
        public const int CONST_JefeUnidad = 4;
        public const int CONST_Asesor = 5;
        public const int CONST_Emprendedor = 6;
        public const int CONST_CallCenter = 8;
        public const int CONST_GerenteEvaluador = 9;
        public const int CONST_CoordinadorEvaluador = 10;
        public const int CONST_Evaluador = 11;
        public const int CONST_GerenteInterventor = 12;
        public const int CONST_CoordinadorInterventor = 13;
        public const int CONST_Interventor = 14;
        public const int CONST_Perfil_Fiduciario = 15;
        public const int CONST_LiderRegional = 16;
        public const int CONST_PerfilFiduciaria = 15;
        public const int CONST_PerfilAbogado = 18;
        public const int CONST_PerfilAcreditador = 19;
        public const int CONST_CallCenterOperador = 20;

        //
        //Constantes para el manejo de roles de los estado del proyecto
        //
        public const int CONST_Inscripcion = 1;
        public const int CONST_PlanAprobado = 2;
        public const int CONST_Convocatoria = 3;
        public const int CONST_Evaluacion = 4;
        public const int CONST_AsignacionRecursos = 5;
        public const int CONST_LegalizacionContrato = 6;
        public const int CONST_Ejecucion = 7;
        public const int CONST_EvaluacionIndicadores = 8;
        public const int CONST_Condonacion = 9;
        //
        //Constantes para el manejo de roles de usuario dentro del proyecto
        //
        public const int CONST_RolAsesorLider = 1;
        public const int CONST_RolAsesor = 2;
        public const int CONST_RolEmprendedor = 3;
        public const int CONST_RolEvaluador = 4;
        public const int CONST_RolCoordinadorEvaluador = 5;
        public const int CONST_RolInterventor = 6;
        public const int CONST_RolCoordinadorInterventor = 7;
        public const int CONST_RolInterventorLider = 8;
        public const int CONST_RolAcreditador = 9;
        //
        //Constante para identificar la Unidad Temporal
        //
        public const int CONST_UnidadTemporal = 2;
        //
        //Constantes de tipos de Tareas
        //
        public const int CONST_Reunion = 1;
        public const int CONST_Generica = 2;
        public const int CONST_AsignarAsesor = 3;
        public const int CONST_PostIt = 5;
        public const int CONST_AsignarEvaluador = 8;
        public const int CONST_AsignarCoordinador = 10;
        public const int CONST_AsignarInterventor = 11;
        public const int CONST_AsignarCoordinadorInterventoria = 12;

        //
        //Constantes TABS proyecto
        //
        public const int CONST_Mercado = 12;
        public const int CONST_Operacion = 13;
        public const int CONST_Organizacion = 14;
        public const int CONST_Finanzas = 15;
        public const int CONST_PlanOperativo = 16;
        public const int CONST_Impacto = 17;
        public const int CONST_ResumenEjecutivo = 18;
        public const int CONST_Anexos = 19;
        public const int CONST_InvestigacionMercados = 20;
        public const int CONST_EstrategiasMercado = 21;
        public const int CONST_ProyeccionesVentas = 22;
        public const int CONST_SubOperacion = 23;
        public const int CONST_CostosInsumos = 24;
        public const int CONST_EstrategiaOrganizacional = 25;
        public const int CONST_EstructuraOrganizacional = 26;
        public const int CONST_AspectosLegales = 27;
        public const int CONST_Presupuestos = 28;
        public const int CONST_SubResumenEjecutivo = 29;
        public const int CONST_EquipoTrabajo = 30;
        public const int CONST_Infraestructura = 31;
        public const int CONST_Compras = 32;
        public const int CONST_Ingresos = 33;
        public const int CONST_Egresos = 34;
        public const int CONST_CapitalTrabajo = 35;
        public const int CONST_SubPlanOperativo = 36;
        public const int CONST_SubMetas = 37;

        public const int CONST_Empresa = 38;	//modificar a 38

        public const int CONST_RegistroMercantilInter = 39;		//Se debe eliminar
        public const int CONST_AvanceInter = 40;				//Se debe eliminar
        public const int CONST_IndicadoresGestionInter = 41;	//Se debe eliminar

        //
        //Constantes TABS Interventoria
        //
        public const int CONST_PlanOperativoInter = 1;
        public const int CONST_IndicadoresGestionInter2 = 2;
        public const int CONST_RiesgosInter = 3;
        public const int CONST_ConceptosInter = 4;
        public const int CONST_ContratoInter = 5;
        public const int CONST_PlanOperativoInter2 = 6;
        public const int CONST_NominaInter = 7;
        public const int CONST_ProduccionInter = 8;
        public const int CONST_VentasInter = 9;
        public const int CONST_IndicadoresGen = 10;
        public const int CONST_IndicadoresEsp = 11;

        //
        //Constantes para el manejo de los periodos
        //
        public const int CONST_Mes = 1;
        public const int CONST_Bimestre = 2;
        public const int CONST_Trimestre = 3;
        public const int CONST_Semestre = 4;

        //
        //Constantes para el manejo de Listado de Tipo Inversion  para Nuevas Inversiones (Pestana Finanzaz - Egresos), DropDownlList
        //
        public const string CONST_FondoEmprender = "Fondo Emprender";
        public const string CONST_AporteEmprendedores = "Aporte Emprendedores";
        public const string CONST_RecursosCapital = "Recursos de Capital";
        public const string CONST_IngresosVentas = "Ingresos por ventas";

        //
        //Constantes para el manejo de Listado de Tipo Aporte para Nuevos Aportes (Pestana Finanzaz - Ingreso), DropDownlList
        //
        public const string CONST_Dinero = "Dinero";
        public const string CONST_Bien = "Bien";
        public const string CONST_Servicio = "Servicio";


        //
        //Constantes para el manejo de Listado de Tipo Recruso para Nuevos Recurso (Pestana Finanzaz - Ingreso), DropDownlList
        //
        public const string CONST_Credito = "Crédito";
        public const string CONST_Donancion = "Donación";

        //Constantes para el manejo de evaluador
        public const int CONST_subAportes = 16;



        //Constantes de Acreditacion de Proyectos
        public const int CONST_Registro_y_Asesoria = 1;
        public const int CONST_Asignado_para_acreditacion = 10;
        public const int CONST_Aprobacion_Acreditacion = 11;
        public const int CONST_Aprobacion_No_Acreditacion = 12;
        public const int CONST_Acreditado = 13;
        public const int CONST_No_acreditado = 14;
        public const int CONST_Pendiente = 15;
        public const int CONST_Subsanado = 16;
        public const int CONST_Sena = 1;
        public const int CONST_concejo_directivo = 18;

        //Constantes TABS Evaluación.
        public const int CONST_PlanOperativoEval = 25;
        public const int CONST_subPlanOperativoEval = 26;
        public const int CONST_subNomina = 27;
        public const int CONST_subProduccion = 28;
        public const int CONST_subVentas = 29;
        public const int CONST_HojaAvance = 30;


        public const string CONST_TextoObsNoAcreditado = "NO ACREDITADO:";
        public const string CONST_TextoObsAcreditado = "ACREDITADO:";

        ///pagos

        public const int CONST_EstadoEdicion = 0;
        public const int CONST_EstadoInterventor = 1; //Añadido el 12/04/2014. "Encontrada en el archivo (Pagos.inc)".        
        public const int CONST_EstadoCoordinador = 2;
        public const int CONST_EstadoFiduciaria = 3;
        public const int CONST_EstadoAprobadoFA = 4;
        public const int CONST_EstadoRechazadoFA = 5;
        // constantes interventoria
        public const int CONST_TipoPagoNomina = 2; //Añadido el 12/04/2014. "Encontrada en el archivo (Pagos.inc)".

        //Constantes internas.
        ///Añadido el 15/04/2014. "Encontrada y usada en InterProduccionDer.asp" entre otras páginas para
        ///generar multiplicación de valores y cantidades, generando los resultados en los campos correspondientes.
        public const int CONST_Meses = 12;
        public const int CONST_Fuentes = 2;

        //Constantes TABS Interventoria

        //Mas tabs NO incluídos inicialmente. (añadidos el 03/07/2014).
        public const int CONST_subGenerales = 5;
        public const int CONST_subComerciales = 6;
        public const int CONST_subTecnicos = 7;
        public const int CONST_subOrganizacionales = 8;
        public const int CONST_subFinancieros = 9;
        public const int CONST_subMedioAmbiente = 10;


        //Constantes que no estaban incluídas, se incluyeron el 07/07/2014 desde el archivo "Pagos.inc".

        //Constantes para determinar el tipo de pago: Actividad o Nomina
        public const int CONST_TipoPagoActividad = 1;

        //Constantes para determinar el estado de un pago
        

        //COMENTADAS EN EL CLÁSICO.
        //'Numero de fideicomiso entre Fonade y Fiduagraria
        //'CONST CONST_Fideicomiso = 31204

        //COMENTADAS EN EL CLÁSICO.
        //'Numero de fideicomiso entre Fonade y FiduBogota
        //'CONST CONST_Fideicomiso = 312711
        public const int CONST_Fideicomiso = 3116428;

        //Numero de Encargo Fiduciario para la convocatoria 1
        public const int CONST_EncargoFiduciario1 = 103806;
        
        // Link Manuales
        public const string Const_ManualAcreditador = "Manual_Usuario_Perfil_Acreditacion.pdf";
        public const string Const_ManualAdminFonade = "Manual_Usuario_Perfil_Administrador_Fonade.pdf";
        public const string Const_ManualAsesor = "Manual_Usuario_Perfil_Asesor.pdf";
        public const string Const_ManualCallCenter = "Manual_Usuario_Perfil_Call_Center.pdf";
        public const string Const_ManualCoordInter = "Manual_Usuario_Perfil_Coordinador_de_Interventoria.pdf";
        public const string Const_ManualCoordEval = "Manual_Usuario_Perfil_Coordinador_Evaluador.pdf";
        public const string Const_ManualEmprendedor = "Manual_Usuario_Perfil_Emprendedor.pdf";
        public const string Const_ManualEvaluador = "Manual_Usuario_Perfil_Evaluador.pdf";
        public const string Const_ManualFiducia = "Manual_Usuario_Perfil_Fiduciario.pdf";
        public const string Const_ManualGerenteAdmin = "Manual_Usuario_Perfil_Gerente_Administrador.pdf";
        public const string Const_ManualGerenteEval = "Manual_Usuario_Perfil_Gerente_Evaluador.pdf";
        public const string Const_ManualGerenteInter = "Manual_Usuario_Perfil_Gerente_Interventor.pd";
        public const string Const_ManualInterventor = "Manual_Usuario_Perfil_Interventor.pdf";
        public const string Const_ManualJefeUnidad = "Manual_Usuario_Perfil_JefedeUnidad.pdf";
        public const string Const_ManualLiderRegional = "Manual_Usuario_Perfil_Lider_Regional.pdf";


        //Tabs proyecto de negocio 2017
        public const int CONST_Protagonista = 51;
        public const int CONST_OportunidadMercado = 52;
        public const int CONST_Solucion = 53;
        public const int CONST_Parte1Solucion = 54;
        public const int CONST_Parte2FichaTecnica = 55;
        public const int CONST_DesarolloSolucion = 56;
        public const int CONST_Paso1IngresoCondicionesComerciales = 57;
        public const int CONST_Paso2Proyeccion = 58;
        public const int CONST_Paso3NormatividadCondicionesTecnicas = 59;
        public const int CONST_Paso4Requerimientos = 60;
        public const int CONST_Paso5Produccion = 61;
        public const int CONST_Paso6ProductividadEquipoDeTrabajo = 62;
        public const int CONST_Futurodelnegocio = 63;
        public const int CONST_Estrategias = 64;
        public const int CONST_PeriododeArranqueEImproductivo = 65;
        public const int CONST_Riesgos = 66;
        public const int CONST_ResumenEjecutivoV2 = 67;
        public const int CONST_FinanzasV2 = 68;
        public const int CONST_IngresosV2 = 69;
        public const int CONST_EgresosV2 = 70;
        public const int CONST_CapitalDeTrabajoV2 = 71;
        public const int CONST_PlanOperativoV2 = 72;
        public const int CONST_PlanOperativoV2Hijo = 73;
        public const int CONST_MetasSocialesV2 = 74;
        public const int CONST_AnexosV2 = 75;
        public const int CONST_EmpresaV2 = 76;
        public const int CONST_RegistroMercantil = 77;
        public const int CONST_Avance = 78;
        public const int CONST_PlanOperativoEmpresaV2 = 79;
        public const int CONST_Nomina = 80;
        public const int CONST_Produccion = 81;
        public const int CONST_IndicadoresDeGestion = 82;
        public const int CONST_IndicadoresGenericos = 83;
        public const int CONST_IndicadoresEspecificos = 84;
        public const int CONST_seguimientoV2 = 200;
        public const int CONST_ContratoV2 = 88;
        public const int CONST_CostosAdministrativosV2 = 89;
        public const int CONST_CostosDeProduccionV2 = 90;
        public const int CONST_PlanDeComprasV2 = 91;

        //Fuente de financiación (plan de negocio V2)
        public const int CONST_FondoEmprenderV2 = 1;
        public const int CONST_AporteEmprendedoresV2 = 2;
        public const int CONST_RecursosCapitalV2 = 3;
        public const int CONST_IngresosVentasV2 = 4;


        //Tipo Infraestructura (plan de negocio V2)
        public const int CONST_Infraestructura_Adecuaciones = 9;
        public const int CONST_MaquinariayEquipo = 10;
        public const int CONST_EquiposComuniCompu = 11;
        public const int CONST_MueblesEnseresOtros = 12;
        public const int CONST_Otros = 13;
        public const int CONST_GastoPreoperativos = 14;

        //Versión Plan
        public const int CONST_PlanV1 = 1;
        public const int CONST_PlanV2 = 2;

        //Unidad de tiempo
        public const int CONST_UniTiempoMes = 2;
        public const int CONST_UniTiempoDias = 1;

        //Dedicacion
        public const int CONST_DedicacionCompleta = 0;
        public const int CONST_DedicacionParcial = 1;

        //Tipo Contratación
        public const int CONST_ContratoJornal = 0;
        public const int CONST_ContratoNomina = 1;
        public const int CONST_ContratoPrestacion = 2;


        // Tab evaluación 
        public const int Const_TablaDeEvaluacionV2 = 30;
        public const int Const_DesempenoEvaluadorV2 = 44;
        public const int Const_InformesV2 = 45;
        public const int Const_PlanOperativoEvaluacionV2 = 46;
        public const int Const_EvaluacionFinancieraV2 = 39;
        public const int Const_ConceptoFinalYRecomendacionesV2 = 56;
        public const int Const_DatosGeneralesV2 = 31;
        public const int Const_QuienEsElProtagonistaV2 = 32;
        public const int Const_ExisteOportunidadEnElMercadoV2 = 33;
        public const int Const_CualEsMiSolucionV2 = 34;
        public const int Const_ComoDesarrolloMiSolucionV2 = 35;
        public const int Const_CualEsElFuturoDeMiNegocioV2 = 36;
        public const int Const_QueRiesgosEnfrentoV2 = 37;
        public const int Const_ResumenEjecutivoV2 = 38;                                
        public const int Const_PlanOperativoV2 = 47;
        public const int Const_NominaV2 = 48;
        public const int Const_ProduccionV2 = 49;
        public const int Const_ventasV2 = 62;
        public const int Const_IndicadoresFinancierosEvaluacionV2 = 51;
        public const int Const_CargueModeloFinancieroEvaluacionV2 = 52;
        public const int Const_EvaluacionDelProyectoEvaluacionV2 = 53;
        public const int Const_CentralesDeRiesgoEvaluacionV2 = 54;
        public const int Const_FlujoDeCajaV2 = 55;
        public const int Const_AportesV2 = 57;
        public const int Const_IndicadoresDeGestionYCumplimientoV2 = 58;
        public const int Const_RiesgosIdentificadosYMitigadosV2 = 59;
        public const int Const_ConclusionDeViabilidadV2 = 60;
        public const int Const_IndicadoresDeGestionV2 = 63;


        //Aspectos de evaluación
        public const int Const_AspectoProtagonistaV2 = 69;
        public const int Const_AspectoOportunidadMercadoV2 = 74;
        public const int Const_AspectoCualEsMiSolucionV2 = 87;
        public const int Const_AspectoDesarrolloSolucionV2 = 102;
        public const int Const_AspectoFuturoNegocioV2 = 123;
        public const int Const_AspectoRiesgosV2 = 136;
        public const int Const_AspectoResumenEjecutivoV2 = 140;
        public const int Const_AspectoResumenEjecutivoV2New = 156;
        public const int Const_AspectoResumenEjecutivoV2NewProd = 159;

        //Códigos campos reporte ptje evaluación
        public const int Const_RepA = 71;
        public const int Const_RepB = 73;
        public const int Const_RepC = 76;
        public const int Const_RepD = 77;
        public const int Const_RepE = 79;
        public const int Const_RepF = 80;
        public const int Const_RepG = 81;
        public const int Const_RepH = 82;
        public const int Const_RepI = 84;
        public const int Const_RepJ = 85;
        public const int Const_RepK = 86;
        public const int Const_RepL = 89;
        public const int Const_RepM = 90;
        public const int Const_RepN = 91;
        public const int Const_RepO = 92;
        public const int Const_RepP = 93;
        public const int Const_RepQ = 95;
        public const int Const_RepR = 96;
        public const int Const_RepS = 97;
        public const int Const_RepT = 99;
        public const int Const_RepU = 100;
        public const int Const_RepV = 101;
        public const int Const_RepW = 104;
        public const int Const_RepX = 105;
        public const int Const_RepY = 106;
        public const int Const_RepZ = 107;
        public const int Const_RepAA = 108;
        public const int Const_RepAB = 110;
        public const int Const_RepAC = 111;
        public const int Const_RepAD = 113;
        public const int Const_RepAE = 114;
        public const int Const_RepAF = 115;
        public const int Const_RepAG = 116;
        public const int Const_RepAH = 117;
        public const int Const_RepAI = 118;
        public const int Const_RepAJ = 119;
        public const int Const_RepAK = 121;
        public const int Const_RepAL = 122;
        public const int Const_RepAM = 125;
        public const int Const_RepAN = 126;
        public const int Const_RepAO = 128;
        public const int Const_RepAP = 129;
        public const int Const_RepAQ = 130;
        public const int Const_RepAR = 132;
        public const int Const_RepAS = 133;
        public const int Const_RepAT = 134;
        public const int Const_RepAU = 135;
        public const int Const_RepAV = 138;
        public const int Const_RepAW = 139;
        public const int Const_RepAX = 142;
        public const int Const_RepAY = 143;
        public const int Const_RepAZ = 145;
        public const int Const_RepBA = 146;
        public const int Const_RepBB = 147;
        public const int Const_RepBC = 148;
        public const int Const_RepBD = 149;
        public const int Const_RepBE = 150;
        public const int Const_RepBF = 151;
        public const int Const_RepBG = 152;
        public const int Const_RepBH = 153;
        public const int Const_RepBI = 154;
        public const int Const_RepBJ = 155;
        public const int Const_RepBK = 156;
        public const int Const_RepBL = 157;
        public const int Const_RepBM = 158;
        public const int Const_RepBN = 162;
        public const int Const_RepBO = 163;
        public const int Const_RepBP = 164;

        public const int const_estado_tareaEspecial_cerrada = 1;
        public const int const_estado_tareaEspecial_pendiente = 2;


        //Acta de seguimiento Tipo
        public const int const_actaSeguimientoInicio = 1;

        //OpcionesEstadisticaInterventores

        public const int const_EstadisticaPagos = 1;
        public const int const_EstadisticaAvance = 2;

        //Operadores
        public const int const_OperadorEnterritorio = 1;
        public const int const_OperadorUniversidadNacional = 2;

        //Id's preguntas de evaluacion 
        //¿Se tiene claridad del perfil del cliente y/o consumidor a atender, junto con su ubicación geográfica?
        public const int const_ClaridadPerfilCliente = 71;
        //¿Se ha identificado las necesidades y/o motivaciones a satisfacer del cliente?
        public const int const_NecesidadesCliente = 73;
        //¿Define los aspectos más importantes del mercado?
        public const int const_AspectosMasImportantes = 76;
        //¿Establece el comportamiento de cada uno de los segmentos del mercado?
        public const int const_ComportamientoSegmentoMercado = 77;
        //¿Relaciona la estrategia y actividades de vinculación del proyecto con el ecosistema local de emprendimiento, en términos de participación y promoción de la industria de apoyo a emprendedores?
        public const int const_RelacionaActividadVinculacion = 146;
        //¿Define las principales tendencias que afectan el mercado?
        public const int const_DefineTendenciaMercado = 79;
        //¿Tiene claridad sobre como la situación del sector afecta al negocio?
        public const int const_claridadSituacionSector = 81;
        //¿Establece como participan los diferentes actores en el mercado local?
        public const int const_estableceParticipacionMercado = 82;
        //¿Establece cómo se articula con la Política de desarrollo regional?
        public const int const_politicaDesarrolloRegional = 147;
        //¿Define los principales competidores en el mercado?
        public const int const_defineCompetidresMercado = 84;
        //¿Establece las ventajas y desventajas de los competidores?<br/>  Incluye competidores de productos sustitutos  
        public const int const_estableceVentajasDesventajas = 85;
        //¿Identifica la propuesta de valor que tienen los competidores? 
        public const int const_identificaPropuestaDeValor = 86;
        //¿Identifica como solucionar los problemas y/o necesidades de los clientes?
        public const int const_identificaSolucionarProblemasNecesidades = 89;
        //¿Define claramente el concepto del negocio?
        public const int const_defineConceptoNegocio = 90;
        //¿La propuesta de valor está acorde con las necesidades de los clientes?
        public const int const_acordeNecesidadCliente = 91;
        //¿Establece el aspecto diferenciador del negocio?
        public const int const_aspectoDiferenciadordelNegocio = 149;
        //¿En el plan de negocios se describen las acciones que favorecen la preservación y sostenibilidad del medio ambiente?
        public const int const_describeAccionesMedioAmbiente = 148;
        //¿Explica la metodología utilizada para la validación de mercado?
        public const int const_metodologiaUsadaValidarMercado = 95;
        //¿Muestra los resultados obtenidos de la validación de mercado?
        public const int const_muestraResultadosObtenidos = 96;
        //¿La validación de mercado está acorde con el tipo de negocio y   a los clientes que se quiere llegar?  
        public const int const_validacionMercadoAcorde = 97;
        //¿Evidencia avances técnicos, legales y comerciales en el desarrollo del negocio?
        public const int const_avancesTecnicosLegalesComerciales = 99;
        //¿Cuenta actualmente con un producto mínimo viable validado en el mercado?
        public const int const_productoMinimoViable = 100;
        //¿Menciona su experiencia previa relacionada con el tipo de negocio?
        public const int const_mencionaExperienciaPrevia = 150;
        //¿Define claramente como el negocio generará ingresos y la frecuencia de los mismos?
        public const int const_defineNegocioIngresos = 104;
        //¿Establece condiciones de comercialización acorde con los clientes potenciales?
        public const int const_estableceCondicionesComercializacion = 105;
        //¿Se identifica el canal de comercialización a utilizar para la venta de productos y/o servicios?
        public const int const_identificaCanalComercializacion = 106;
        //¿Se explica la metodología para la fijación de los precios de venta?
        public const int const_explicaMetodologiaFijaPrecios = 107;
        //¿Se identifican acercamientos con los clientes potenciales?
        public const int const_identificaAcercamientosClientes = 108;
        //¿Describe cómo aplica la normatividad que regula al negocio? (Permiso de uso de suelos, licencias de funcionamiento, registros, manejo ambiental, manejo/licencia de aguas, bomberos, entre otros.)
        public const int const_describeNormatividadQueRegulaNegocio = 151;
        //¿Establece los costos y trámites necesarios para cumplir con la normatividad descrita incluyendo la normatividad propia de cada región?
        public const int const_estableceCostosTramites = 152;
        //¿Las condiciones técnicas de la infraestructura y el sitio de operación de negocio están acorde con lo que se requiere según la normatividad?
        public const int const_condicionesInfraestructuraOperacion = 113;
        //¿Se evidencia el cumplimiento de parámetros técnicos conforme al tipo de negocio?
        public const int const_evidenciaCumplimientoParametros = 114;
        //¿Las fichas técnicas describen adecuadamente las características y parámetros requeridos?
        public const int const_fichasTecnicasDescripcion = 115;
        //¿Se describe claramente el proceso de producción, indicando los responsables, tiempos y equipos requeridos para cada una de las actividades?
        public const int const_describeClaramenteProcesoProd = 116;
        //¿La capacidad de producción está acorde con el tamaño propuesto del negocio, partiendo de las ventas proyectadas?
        public const int const_capacidadProduccionAcorde = 117;
        //¿El programa de producción está acorde con la capacidad de producción y las variables técnicas?
        public const int const_programaProduccionAcorde = 118;
        //¿Los requerimientos de materias primas e insumos están acorde con lo que se requiere para la operación del negocio y el plan de producción propuesto?
        public const int const_requerimientosMateriasPrimas = 119;
        //¿Existe complementariedad en los perfiles del equipo de trabajo propuesto y guardan relación con el tipo de negocio?
        public const int const_complementoPerfilTrabajos = 121;
        //¿En el plan de negocios se plantea la contratación de un aprendiz SENA?
        public const int const_contrataAprendizSENA = 153;
        //¿La estructura organizacional propuesta incluyendo cargos y funciones a desempeñar por el personal, es la adecuada para el funcionamiento de la empresa? (tipo de vinculación, proyección en el tiempo de ejecución y sostenibilidad de empleo)
        public const int const_estructuraOrganizacional = 154;
        //Califique la generación de empleo según el siguiente rango: 0 a 3 empleos (1), 4 a 5 empleo (2) y 6 empleos en adelante (3)
        public const int const_generacionEmpleo = 155;
        //¿Se tiene la claridad sobre las actividades, responsables y costos relacionados para llevar a cabo las estrategias de comercialización?
        public const int const_claridadActividades = 125;
        //¿Las actividades propuestas dentro de las estrategias de comercialización están acorde con el mercado objetivo?
        public const int const_actividadesPropuestas = 126;
        //¿Es claro el tiempo improductivo que se requiere para el arranque de la empresa y para empezar a vender de acuerdo al tipo de negocio?
        public const int const_tiempoImproductivoRequerido = 128;
        //¿Se definen con claridad las fuentes de financiación para el periodo improductivo?
        public const int const_fuentesFinanciacionPeriodoImprod = 129;
        //¿Se tiene en cuenta el tiempo, trámites y recursos para la puesta en marcha del negocio conforme al sector productivo?
        public const int const_cuentaTiempoTramitesRecursos = 130;
        //¿El plan de negocios presenta fuente de ingresos sostenibles en el largo plazo?
        public const int const_planNegociosFuenteIngresos = 132;
        //¿La estructura de costos se adecua al modelo de negocios propuesto?
        public const int const_estructuraCostosAdecuaModelo = 133;
        //¿La operación del negocio es rentable en el horizonte de tiempo proyectado?
        public const int const_operacionNegocioRentable = 134;
        //¿Existe claridad sobre la estimación de los márgenes de ganancia por producto?
        public const int const_claridadEstimacionMargen = 135;
        //¿Define y mide de manera coherente los factores externos que pueden afectar la operación del negocio?
        public const int const_DefineCoherenteFactoresExternos = 156;
        //¿Define y mide de manera coherente los factores internos que pueden afectar la operación del negocio?
        public const int const_DefineCoherenteFactoresInternos = 157;
        //¿Explica la forma en que se pueden mitigar los riesgos identificados?
        public const int const_formaMitigarRiesgos = 158;
        //¿Se presentan las principales actividades en el plan operativo que llevara a cabo el negocio?
        public const int const_principalActividadNegocio = 162;
        //¿Para cada una de las actividades se establece su duración y fuente de financiación?
        public const int const_actividadEstableceFinanciacion = 163;
        //¿Se establecen indicadores de seguimiento claros y alcanzables para el primer año? Ejemplo: ventas y generación de empleos
        public const int const_indicadorSeguimientoClaro = 164;
    }
}
