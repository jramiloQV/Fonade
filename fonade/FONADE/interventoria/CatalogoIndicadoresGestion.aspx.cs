using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoIndicadoresGestion : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 03/07/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 03/07/2014.
        /// Cargar la grilla de indicadores.
        /// </summary>
        private void CargarGrilla()
        {
            //Inicializar variables.
            DataTable tabla = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    txtSQL = " SELECT InterventorIndicadorTMP.id_indicadorinter, InterventorIndicadorTMP.Aspecto, " +
                             " InterventorIndicadorTMP.CodProyecto, InterventorIndicadorTMP.Tarea, " +
                             " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                             " FROM EmpresaInterventor " +
                             " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                             " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                             " INNER JOIN InterventorIndicadorTMP ON Empresa.codproyecto = InterventorIndicadorTMP.CodProyecto " +
                             " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +                             
                             " WHERE (InterventorIndicadorTMP.ChequeoGerente IS NULL) " +
                             " AND (InterventorIndicadorTMP.ChequeoCoordinador IS NULL) " +
                             " AND (EmpresaInterventor.Inactivo = 0) " +
                             " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                             " and (Contacto.codOperador = "+usuario.CodOperador+") " +
                             " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")";
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    txtSQL = " SELECT InterventorIndicadorTMP.id_indicadorinter, InterventorIndicadorTMP.Aspecto, " +
                             " InterventorIndicadorTMP.CodProyecto, InterventorIndicadorTMP.Tarea, " +
                             " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                             " FROM InterventorIndicadorTMP " +
                             " INNER JOIN Empresa ON InterventorIndicadorTMP.CodProyecto = Empresa.codproyecto " +
                             " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                             " INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto " +
                             " WHERE (InterventorIndicadorTMP.ChequeoGerente IS NULL) " +
                             " AND (InterventorIndicadorTMP.ChequeoCoordinador = 1) " +
                             " AND (EmpresaInterventor.Inactivo = 0) " +
                             " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                             " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ")";
                }

                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["dt_data"] = tabla;

                gvindicadoresgestion.DataSource = tabla;
                gvindicadoresgestion.DataBind();

                if (tabla.Rows.Count > 0)
                {
                    if (tabla.Rows.Count == 1)
                    { lbl_pagina.Text = gvindicadoresgestion.Rows.Count + " actividad"; }
                    else
                    { lbl_pagina.Text = gvindicadoresgestion.Rows.Count + " de " + tabla.Rows.Count + " actividades"; }
                }
                else
                { lbl_pagina.Text = "No hay actividades"; }
            }
            catch { }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EDITAR")
            {
                string[] param = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["Accion"] = param[0];
                HttpContext.Current.Session["CodProyecto"] = param[1];
                HttpContext.Current.Session["id_indicadorinter"] = param[2];

                Redirect(null, "CatalogoIndicadorInter.aspx", "_Blank", "width=730,height=585");
            }
        }

        /// <summary>
        /// Paginación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvindicadoresgestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var tabla = HttpContext.Current.Session["dt_data"] as DataTable;

            if (tabla != null)
            {
                gvindicadoresgestion.DataSource = tabla;
                gvindicadoresgestion.PageIndex = e.NewPageIndex;
                gvindicadoresgestion.DataBind();
            }
        }
    }
}