using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.AdministrarPerfiles.CatalogoEmprendedor
{
    /// <summary>
    /// InformacionAcademica
    /// </summary>    
    public partial class InformacionAcademica : Negocio.Base_Page
    {
        /// <summary>
        /// The page size
        /// </summary>
        public const int PAGE_SIZE = 10;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the DataBound event of the gv_infoac control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_infoac_DataBound(object sender, EventArgs e)
        {

            //gv_Proyectos.Columns[2].Visible = false;
            //string codigosGrupo;
            //String[] grupos2 = { "0" };
            //String grupos3;
            //if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
            //{

            //    grupos3 = Request.QueryString["codGrupo"];
            //    grupos2 = grupos1.Split(',');
            //    if (grupos2.Contains(Constantes.CONST_AdministradorFonade.ToString()) | grupos2.Contains(Constantes.CONST_AdministradorSena.ToString()))
            //    {
            //        gv_administradores.Columns[3].Visible = true;

            //    }
            //    else
            //    {
            //        gv_administradores.Columns[3].Visible = false;
            //    }
            //}

        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_infoac control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_infoac_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //string Conteo = "";
            ////Conteo = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "cuantos"));
            //ImageButton imbtton = new ImageButton();
            //imbtton = ((ImageButton)e.Row.Cells[0].FindControl("btn_Inactivar"));
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (Conteo == "0")
            //    {
            //        imbtton.Visible = true;

            //    }
            //    else
            //    {

            //        imbtton.Visible = false;

            //    }
            //}


        }

        /// <summary>
        /// Handles the Selecting event of the lds_infoac control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_infoac_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var infoac = from pa in consultas.Db.ProgramaAcademicos
                         join ie in consultas.Db.InstitucionEducativas on pa.CodInstitucionEducativa equals ie.Id_InstitucionEducativa
                         join c in consultas.Db.Ciudad on pa.CodCiudad equals c.Id_Ciudad

                         select new
                         {
                             Id_Programa = pa.Id_ProgramaAcademico,
                             NomProgramaAcademico = pa.NomProgramaAcademico,
                             NomInstitucionEducativa = ie.NomInstitucionEducativa,
                             NomCiudad = c.NomCiudad
                         };

            if (e.Arguments.TotalRowCount == 0)
            {
                Lbl_Resultados.Visible = true;
                Lbl_Resultados.Text = "No hay datos en esta consulta";
            }
            else
            {
                Lbl_Resultados.Visible = false;
            }
            e.Arguments.TotalRowCount = infoac.Count();
            infoac = infoac.Skip(gv_infoac.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);
            e.Result = infoac.ToList();

        }

        /// <summary>
        /// Handles the RowCreated event of the gv_infoac control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_infoac_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        // buscar el link del header
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null && gv_infoac.SortExpression == lnk.CommandArgument)
                        {
                            // inicializar nueva imagen
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            // url de la imagen dinamicamente
                            img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_infoac.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
                            // a ñadir el espacio de la imagen
                            tc.Controls.Add(new LiteralControl(" "));
                            tc.Controls.Add(img);

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the OnSelectedIndexChanged1 event of the ddl_depto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddl_depto_OnSelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddl_depto1.SelectedValue != "9999999999")
            {
                var municipios = (from c in consultas.Db.Ciudad
                                  where c.CodDepartamento == Int32.Parse(ddl_depto1.SelectedValue)
                                  orderby c.NomCiudad ascending
                                  select new
                                  {
                                      Ciudad = c.NomCiudad,
                                      ID_Ciudad = c.Id_Ciudad
                                  });
                ddl_ciudad1.DataSource = municipios;
                ddl_ciudad1.DataTextField = "Ciudad";
                ddl_ciudad1.DataValueField = "ID_Ciudad";
                ddl_ciudad1.DataBind();
                ddl_ciudad1.Items.Insert(0, new ListItem("(Todos los Municipios)", "9999999999"));
            }
        }

        /// <summary>
        /// Handles the OnSelectedIndexChanged2 event of the ddl_depto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddl_depto_OnSelectedIndexChanged2(object sender, EventArgs e)
        {
            if (ddl_depto2.SelectedValue != "9999999999")
            {
                var subsectores = (from s in consultas.Db.SubSector
                                   where s.CodSector == Int32.Parse(ddl_depto2.SelectedValue)
                                   orderby s.NomSubSector
                                   select new
                                   {
                                       Sumsector = s.NomSubSector,
                                       Id_subsector = s.Id_SubSector
                                   });
                ddl_ciudad2.DataSource = subsectores;
                ddl_ciudad2.DataTextField = "Sumsector";
                ddl_ciudad2.DataValueField = "Id_subsector";
                ddl_ciudad2.DataBind();
                ddl_ciudad2.Items.Insert(0, new ListItem("(Todos los Subsectores)", "9999999999"));
            }
        }
    }
}