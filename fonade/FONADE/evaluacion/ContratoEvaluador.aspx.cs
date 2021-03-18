using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Globalization;

namespace Fonade.FONADE.evaluacion
{
    public partial class ContratoEvaluador : Negocio.Base_Page
    {
        protected int CodContacto;
        protected int CodContrato;
        /// <summary>
        /// clase que actualiza la entidad ContratoEvaluador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoEvaluador"]);
            CodContrato = Convert.ToInt32(HttpContext.Current.Session["ContratoEvaluador"]);

            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            if (!IsPostBack)
            {
                if (CodContrato == 0)
                {
                    lbl_Titulo.Text = void_establecerTitulo("CREAR CONTRATO");
                    PanelCrear.Visible = true;
                    txt_fechainicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    lbl_Titulo.Text = void_establecerTitulo("MODIFICAR CONTRATO");
                    PanelModificar.Visible = true;
                    llenarDatosModificar();
                }
            }
        }

        protected void llenarDatosModificar()
        {
            var query = (from x in consultas.Db.EvaluadorContratos
                         where x.Id_EvaluadorContrato == CodContrato
                         select new { x }).FirstOrDefault();

            lnumero.Text = query.x.numContrato.ToString();
            lfechainicio.Text = query.x.FechaInicio.ToString("dd/MM/yyyy");
            txt_fechafin.Text = ((DateTime)query.x.FechaExpiracion).ToString("dd/MM/yyyy");
            txt_motivo.Text = query.x.Motivo;
        }

        protected void btn_Crear_Click(object sender, EventArgs e)
        {
            if (txt_numero.Text == "" || txt_meses.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: Algunos campos están vacíos!')", true);
            }
            else
            {
                DateTime fInicialSql = DateTime.ParseExact(txt_fechainicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime fFinalSql = fInicialSql.AddMonths(Convert.ToInt32(txt_meses.Text));

                var fechaInicialSql = fInicialSql.Date.ToString("yyyy-MM-dd HH:mm:ss");
                var fechaFinalSql = fFinalSql.Date.ToString("yyyy-MM-dd HH:mm:ss");

                
                EvaluadorContrato evaContrato = new EvaluadorContrato();
                evaContrato.CodContacto = CodContacto;
                evaContrato.numContrato = int.Parse(txt_numero.Text);
                evaContrato.FechaInicio = DateTime.Parse(fechaInicialSql);
                evaContrato.FechaExpiracion = DateTime.Parse(fechaFinalSql);


                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();

               
                try
                {
                    consultas.Db.EvaluadorContratos.InsertOnSubmit(evaContrato);
                    consultas.Db.SubmitChanges();
                }
                catch (Exception) { throw; }


                
                con.Close();
                con.Dispose();
                cmd2.Dispose();
          

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Contrato creado exitosamente!'); window.opener.location.reload(); window.close();", true);
            }

        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_motivo.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: El campo Motivo es requerido!')", true);
            }
            else
            {
                DateTime fInicialSql = DateTime.ParseExact(lfechainicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime fFinalSql = DateTime.ParseExact(txt_fechafin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (fFinalSql <= fInicialSql)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: La fecha de expiración debe ser mayor a la fecha de inicio!')", true);
                }
                else
                {
                    var fechaInicialSql = fInicialSql.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    var fechaFinalSql = fFinalSql.Date.ToString("yyyy-MM-dd HH:mm:ss");

          
                    EvaluadorContrato evaContrato = (from ec in consultas.Db.EvaluadorContratos
                                                     where ec.Id_EvaluadorContrato == CodContrato
                                                     select ec).FirstOrDefault();

                    if (evaContrato != null)
                    {
                        evaContrato.FechaInicio = DateTime.Parse(fechaInicialSql);
                        evaContrato.FechaExpiracion = DateTime.Parse(fechaFinalSql);
                        evaContrato.Motivo = txt_motivo.Text;
                    }


                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                 
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                   
                    try
                    {
                        consultas.Db.SubmitChanges();
                    }
                    catch (Exception) { throw; }


                    con.Close();
                    con.Dispose();
                    cmd2.Dispose();
                 
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Contrato modificado exitosamente!'); window.opener.location.reload(); window.close();", true);

            
                }
            }
        }
    }
}