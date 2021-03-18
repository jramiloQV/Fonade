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

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// ConvocatoriaReglaSalarios
    /// </summary>    
    public partial class ConvocatoriaReglaSalarios : Negocio.Base_Page
    {
        /// <summary>
        /// The identifier convocatoria
        /// </summary>
        public int idConvocatoria;
        /// <summary>
        /// numero condicion
        /// </summary>
        public int NumeroCondicion;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var _CodConvocatoria = Request.QueryString.GetValues("CodConvocatoria")[0]??"0";
		    var _NoRegla=Request.QueryString.GetValues("NoRegla")[0]??"0";
            idConvocatoria = HttpContext.Current.Session["CodConvocatoria"] == null? Convert.ToInt32(_CodConvocatoria) : Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"].ToString());
            NumeroCondicion = Convert.ToInt32(_NoRegla??"0");
            if (!IsPostBack)
            {                
                l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
                if (NumeroCondicion == 0)
                {
                    lbl_Titulo.Text = void_establecerTitulo("REGLA DE SALARIOS (crear condición)");
                    validarNuevo();
                }
                else 
                {
                    lbl_Titulo.Text = void_establecerTitulo("REGLA DE SALARIOS (modificar condición)");
                    validarModificar();
                }
            }
        }

        /// <summary>
        /// Validar nuevo.
        /// </summary>
        protected void validarNuevo()
        {
            var query = (from cr in consultas.Db.ConvocatoriaReglaSalarios
                         where cr.CodConvocatoria == idConvocatoria
                         select new { cr }).Count();
            int conteo = query + 1;

            l_Numcondicion.Text = conteo.ToString();
            btn_crear.Visible = true;
        }

        /// <summary>
        /// Validar modificar.
        /// </summary>
        protected void validarModificar()
        {
            var query = (from cr in consultas.Db.ConvocatoriaReglaSalarios
                         where cr.CodConvocatoria == idConvocatoria
                         && cr.NoRegla == NumeroCondicion
                         select new
                         {
                             cr,

                         }).FirstOrDefault();
            if (query == null) return;
            l_Numcondicion.Text = query.cr.NoRegla.ToString();
            ddl_condicion.SelectedValue = query.cr.ExpresionLogica;
            txt_numero1.Text = query.cr.EmpleosGenerados1.ToString();
            txt_numero2.Text = query.cr.EmpleosGenerados2.ToString();
            txt_salarios.Text = query.cr.SalariosAPrestar.ToString();
            validarSeleccionado();            
            btn_modificar.Visible = true;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DropDownList1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarSeleccionado();
        }

        /// <summary>
        /// Handles the Click event of the btn_cerrar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }

        /// <summary>
        /// Validar seleccionado.
        /// </summary>
        protected void validarSeleccionado()
        {
            if (ddl_condicion.SelectedValue == "Entre")
            {
                l_y.Visible = true;
                txt_numero2.Visible = true;
            }
            else
            {
                l_y.Visible = false;
                txt_numero2.Visible = false;
            }
        }

        /// <summary>
        /// Inserts or update.
        /// </summary>
        /// <param name="caso">The caso.</param>
        protected void insertUpdate(string caso)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_convocatoria_regla_salarios", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodConvocatoriaR", idConvocatoria);
            cmd.Parameters.AddWithValue("@ExpresionLogicaR", ddl_condicion.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpleosGenerados1R", Convert.ToInt32(txt_numero1.Text));

            if (ddl_condicion.SelectedValue == "Entre")
            {
                cmd.Parameters.AddWithValue("@EmpleosGenerados2R", Convert.ToInt32(txt_numero2.Text));
            }
            else 
            {
                cmd.Parameters.AddWithValue("@EmpleosGenerados2R", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@SalariosAPrestarR", Convert.ToInt32(txt_salarios.Text));
            cmd.Parameters.AddWithValue("@NoReglaR", Convert.ToInt32(l_Numcondicion.Text));
            cmd.Parameters.AddWithValue("@caso", caso);
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();

            if (caso == "Create")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); location.reload();", true);
            }
            else 
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); location.reload();", true);
            }

        }

        /// <summary>
        /// Handles the Click event of the btn_crear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crear_Click(object sender, EventArgs e)
        {
            validarcampos("Create");
        }

        /// <summary>
        /// Validarcamposes the specified caso.
        /// </summary>
        /// <param name="caso">The caso.</param>
        protected void validarcampos(string caso)
        {
            int valida1=0;
            int valida2=0;
            int valida3=0;

            if (ddl_condicion.SelectedValue == "Entre")
            {
                if(txt_salarios.Text=="" || txt_numero2.Text=="" || txt_numero1.Text=="")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Algunos campos están vacíos!')", true);
                }
                else
                {
                    if (int.TryParse(txt_salarios.Text, out valida1) && int.TryParse(txt_numero1.Text, out valida2) && int.TryParse(txt_numero2.Text, out valida3))
                    {
                        insertUpdate(caso);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('funciona!')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Todos los campos deben ser numéricos!')", true);
                    }
                }
            }
            else 
            {
                if (txt_salarios.Text == "" || txt_numero1.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Algunos campos están vacíos!')", true);
                }
                else
                {
                    if (int.TryParse(txt_salarios.Text, out valida1) && int.TryParse(txt_numero1.Text, out valida2))
                    {
                        insertUpdate(caso);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('funciona!')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Todos los campos deben ser numéricos!')", true);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_modificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_modificar_Click(object sender, EventArgs e)
        {
            validarcampos("Update");
        }

    }
}