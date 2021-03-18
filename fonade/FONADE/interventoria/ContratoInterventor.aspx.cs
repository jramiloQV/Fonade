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

namespace Fonade.FONADE.interventoria
{
    public partial class ContratoInterventor : Negocio.Base_Page
    {
        protected int CodContacto;
        protected int CodContrato;

        protected void Page_Load(object sender, EventArgs e)
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);
            CodContrato = Convert.ToInt32(HttpContext.Current.Session["ContratoInterventor"]);

            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            if (!IsPostBack)
            {
                if (CodContrato == 0)
                {
                    CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);

                    var query = (from x in consultas.Db.Contacto
                                 where x.Id_Contacto == CodContacto
                                 select new
                                 {
                                     nombre = x.Nombres + " " + x.Apellidos,
                                 }).FirstOrDefault();

                    lbl_Titulo.Text = void_establecerTitulo("Coordinador de Interventoria: " + query.nombre);
                    l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
                    PanelCrear.Visible = true;
                    txt_fechainicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                   /* lbl_Titulo.Text = void_establecerTitulo("Coordinador de Interventoria");
                    */
                    txt_motivo.Enabled = true;
                }
                else
                {
                    lbl_Titulo.Text = void_establecerTitulo("MODIFICAR CONTRATO");
                    PanelModificar.Visible = true;
                    llenarDatosModificar();
                    txt_motivo.Enabled = true;
                }
            }
        }

        protected void llenarDatosModificar()
        {
            var query = (from x in consultas.Db.InterventorContratos
                         where x.Id_InterventorContrato == CodContrato
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
                
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    SqlCommand cmd = new SqlCommand("MD_InsertUpdateContratoInterventor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodContacto", CodContacto);
                    cmd.Parameters.AddWithValue("@NumContrato", Convert.ToInt32(txt_numero.Text));
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicialSql);
                    cmd.Parameters.AddWithValue("@Motivo", "");
                    cmd.Parameters.AddWithValue("@IdInterventorContrato", 0);
                    cmd.Parameters.AddWithValue("@FechaExpiracion", fechaFinalSql);

                    cmd.Parameters.AddWithValue("@caso", "Create");

                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();

                    //con.Close();
                    //con.Dispose();
                    cmd2.Dispose();
                    cmd.Dispose();
                }
                finally {
                    con.Close();
                    con.Dispose();
                }
                HttpContext.Current.Session["habilitado"] = 1;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Contrato creado exitosamente!');window.opener.location.reload(); window.close();", true);

            }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (txt_motivo.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: Algunos campos están vacíos!')", true);
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
                    var query = (from x in consultas.Db.InterventorContratos
                                 where x.CodContacto == CodContacto
                                &&  x.numContrato == Convert.ToInt32(txt_numero.Text)
                                 select new { x }).Count();
                    if (query != 0)
                    {
                      var fechaInicialSql = fInicialSql.Date.ToString("yyyy-MM-dd HH:mm:ss");
                       var fechaFinalSql = fFinalSql.Date.ToString("yyyy-MM-dd HH:mm:ss");

                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        try
                        {
                            SqlCommand cmd = new SqlCommand("MD_InsertUpdateContratoInterventor", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CodContacto", usuario.IdContacto);
                            cmd.Parameters.AddWithValue("@NumContrato", Convert.ToInt32(txt_numero.Text));
                            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicialSql);
                            cmd.Parameters.AddWithValue("@FechaExpiracion", fechaFinalSql);
                            cmd.Parameters.AddWithValue("@Motivo", txt_motivo.Text);
                            cmd.Parameters.AddWithValue("@IdInterventorContrato", CodContrato);
                            cmd.Parameters.AddWithValue("@caso", "Update");
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            //con.Close();
                            //con.Dispose();
                            cmd2.Dispose();
                            cmd.Dispose();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Contrato modificado exitosamente!'); window.opener.location.reload(); window.close();", true);
                        }
                        finally {
                            con.Close();
                            con.Dispose();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: Ya existe un contrato con el número " + txt_numero.Text + " para este evaluador!')", true);
                    }
                }


            }
        }

        protected void btn_Lista_Click(object sender, EventArgs e)
        {
            Response.Redirect("CatalogoContratoInterventor.aspx");
        }

        protected void btn_Cerrar_Click(object sender, EventArgs e)
        {

        }

 
  
    }
}