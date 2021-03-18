using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Fiduciaria
{
    public class FiduciariaBLL
    {
        static string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public static PagoBeneficiarioModel getBeneficiario(int _codPago, int? _codOperador)
        {
            PagoBeneficiarioModel pagoBeneficiario = new PagoBeneficiarioModel();

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                pagoBeneficiario = (from p in db.PagoActividad
                                    join pr in db.Proyecto 
                                        on p.CodProyecto equals pr.Id_Proyecto                                   
                                    join ben in db.PagoBeneficiarios 
                                        on p.CodPagoBeneficiario equals ben.Id_PagoBeneficiario
                                    join ti in db.TipoIdentificacions
                                       on ben.CodTipoIdentificacion equals ti.Id_TipoIdentificacion
                                    join banco in db.PagoBancos
                                        on ben.CodPagoBanco equals banco.Id_Banco
                                    where p.Id_PagoActividad == _codPago
                                    && pr.codOperador == _codOperador
                                    select new PagoBeneficiarioModel{ 
                                        codPago = p.Id_PagoActividad,
                                        pagoActividad = p.NomPagoActividad,
                                        codProyecto = p.CodProyecto,
                                        nomProyecto = pr.NomProyecto,

                                        tipoIdentificacionBen = ti.NomTipoIdentificacion,
                                        IdentificacionBeneficiario = ben.NumIdentificacion,
                                        NombresBeneficiario = ben.Nombre + " "  +ben.Apellido,
                                        RazonSocialBeneficiario = ben.RazonSocial,
                                        BancoBeneficiario = banco.NomBanco,
                                        NumcuentaBeneficiario = ben.NumCuenta
                                    }).FirstOrDefault();
            }

            return pagoBeneficiario;
        }

        
    }

    public class PagoBeneficiarioModel
    {
        public int codPago { get; set; }
        public string pagoActividad { get; set; }        
        public int? codProyecto { get; set; }
        public string nomProyecto { get; set; }

        public string tipoIdentificacionBen { get; set; }
        public string IdentificacionBeneficiario { get; set; }
        public string NombresBeneficiario { get; set; }
        public string RazonSocialBeneficiario { get; set; }
        public string BancoBeneficiario { get; set; }
        public string NumcuentaBeneficiario { get; set; }
    }
}
