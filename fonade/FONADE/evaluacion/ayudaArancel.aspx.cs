using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// ayudaArancel
    /// </summary>
    
    public partial class ayudaArancel : Negocio.Base_Page
    {
        DataTable datatable = new DataTable();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Resultadoes this instance.
        /// </summary>
        /// <returns></returns>
        public DataTable resultado()
        {
            return datatable;
        }

        /// <summary>
        /// Handles the DataBinding event of the HL_Direccionar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void HL_Direccionar_DataBinding(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// busqueda de posicion arancelaria
        /// </summary>
        /// <param name="consulta"></param>
        private void Buscar(int consulta)
        {
      
            string txtSQL = string.Empty;

            switch(consulta)
            {
                case 1:
                    txtSQL = "SELECT DISTINCT [PosicionArancelaria],[Descripcion]  FROM [PosicionArancelaria]  WHERE [PosicionArancelaria] LIKE '%" + TB_Codigo.Text + "%';";
                    break;
                case 2:
                    txtSQL = "SELECT DISTINCT [PosicionArancelaria],[Descripcion]  FROM [PosicionArancelaria]  WHERE [Descripcion] LIKE '%" + TB_Codigo.Text + "%';";
                    break;
            }
            
            datatable = consultas.ObtenerDataTable(txtSQL, "text");

            GridView1.DataSource = datatable;
            GridView1.DataBind();
     
        }

        /// <summary>
        /// Handles the Click event of the lnk_buscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnk_buscar_Click(object sender, EventArgs e)
        {
            Buscar(int.Parse(RB_Buscar.SelectedValue));
        }


        /// <summary>
        /// Handles the Click event of the HL_Direccionar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void HL_Direccionar_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScriptManager cm = this.ClientScript;

                var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
                GridViewRow GVInventario = GridView1.Rows[indicefila];

                LinkButton TBCantidades = (LinkButton)GVInventario.FindControl("HL_Direccionar");
                String codigo = TBCantidades.Text;
                String desccripcion = GridView1.DataKeys[GVInventario.RowIndex].Value.ToString();

                HttpContext.Current.Session["txtcodigo"] = codigo;
                HttpContext.Current.Session["desccripcion"] = desccripcion;

                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Int32 page = e.NewPageIndex;
            GridView1.PageIndex = page;
            Buscar(int.Parse(RB_Buscar.SelectedValue));
        }


    }
}