using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimObligContablesModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaIngresado { get; set; }
        public string estadosFinancieros { get; set; }
        public string librosComerciales { get; set; }
        public string librosContabilidad { get; set; }
        public string conciliacionBancaria { get; set; }
        public string cuentaBancaria { get; set; }
        public string observObligacionContable { get; set; }
       
    }

    public class ActaSeguimObligTributariasModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaIngresado { get; set; }
        public string declaraReteFuente { get; set; }
        public string autorretencionRenta { get; set; }
        public string declaraIva { get; set; }
        public string declaImpConsumo { get; set; }
        public string declaRenta { get; set; }
        public string declaInfoExogena { get; set; }
        public string declaIndustriaComercio { get; set; }
        public string declaRetencionImpIndusComercio { get; set; }
        public string observObligacionTributaria { get; set; }
        

    }


    public class ActaSeguimObligLaboralModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaIngresado { get; set; }
        public string contratosLaborales { get; set; }
        public string pagosNomina { get; set; }
        public string pagoPrestacionesSociales { get; set; }
        public string afiliacionSegSocial { get; set; }
        public string pagoSegSocial { get; set; }
        public string certParafiscalesSegSocial { get; set; }
        public string reglaInternoTrab { get; set; }
        public string sisGestionSegSaludTrabajo { get; set; }
        public string observObligacionLaboral { get; set; }
    }

    public class ActaSeguimObligTramitesModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaIngresado { get; set; }
        public string insCamaraComercio { get; set; }
        public string renovaRegistroMercantil { get; set; }
        public string rut { get; set; }
        public string resolFacturacion { get; set; }
        public string certLibertadTradicion { get; set; }
        public string DocumentoIdoneidad { get; set; }
        public string permisoUsoSuelo { get; set; }
        public string certBomberos { get; set; }
        public string regMarca { get; set; }
        public string otrosPermisos { get; set; }
        public string contratoArrendamiento { get; set; }
        public string observRegistroTramiteLicencia { get; set; }
    }
}
