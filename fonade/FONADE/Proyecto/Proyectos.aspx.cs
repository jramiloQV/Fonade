#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>03 - 03 - 2014</Fecha>
// <Archivo>Proyectos.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;
using Fonade.Negocio;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using Fonade.Negocio.Proyecto;

#endregion

namespace Fonade.Fonade.Proyecto
{
    public partial class Proyectos : Base_Page
    {
        public const int PAGE_SIZE = 20;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        ProyectoController proyectoController = new ProyectoController();


        protected void lds_proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //var query = TodosLosProyectos(usuario.CodOperador);
            //var query = 
            //            from p in consultas.Db.Proyecto
            //            from c in consultas.Db.Ciudads
            //            from d in consultas.Db.departamentos
            //            from i in consultas.Db.Institucions
            //            join cc in consultas.Db.Ciudads on i.CodCiudad equals cc.Id_Ciudad
            //            join dd in consultas.Db.departamentos on cc.CodDepartamento equals dd.Id_Departamento
            //            where c.Id_Ciudad == p.CodCiudad & c.CodDepartamento == d.Id_Departamento & p.CodInstitucion == i.Id_Institucion
            //            orderby p.Id_Proyecto descending
            //            select new ListadoPlanesDeNegocio
            //            {
            //                IdProyecto = p.Id_Proyecto,
            //                NombreProyecto = p.NomProyecto,
            //                CodigoInstitucion = i.Id_Institucion,
            //                CodigoEstado = p.CodEstado,
            //                NombreUnidad = i.NomUnidad,
            //                NombreInstitucion = i.NomInstitucion,
            //                NombreCiudad = c.NomCiudad,
            //                NombreDepartamento = d.NomDepartamento,
            //                idDpto = d.Id_Departamento,
            //                idTipoInstitucion = i.CodTipoInstitucion,
            //                idDptoInst = dd.Id_Departamento,
            //                Estado = (p.Inactivo) ? "Inactivo" : "Activo",
            //                codOperador = p.codOperador
            //            };

            List<ListadoPlanesDeNegocio> query = new List<ListadoPlanesDeNegocio>(); 

            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_AdministradorSistema:
                case Constantes.CONST_AdministradorSena:
                    //query = query.Where(p => p.Estado == "Activo");
                    query = proyectoController.VerTodosLosProyectosActivosADMIN();
                    break;
                case Constantes.CONST_JefeUnidad:
                    //query = query.Where(p => p.CodigoInstitucion == usuario.CodInstitucion);
                    query = proyectoController.VerTodosLosProyectosActivosJefeUnidad(usuario.CodInstitucion);
                    break;
                case Constantes.CONST_Asesor:
                case Constantes.CONST_Emprendedor:
                    //query =
                    //    query.Where(
                    //        v => (consultas.Db.ProyectoContactos.Where(p => p.Proyecto.Id_Proyecto == p.CodProyecto
                    //                                                        && p.CodContacto == usuario.IdContacto
                    //                                                        && p.Inactivo == false).Select(
                    //                                                            t => t.CodProyecto)).Contains(
                    //                                                                v.IdProyecto)
                    //             && v.Estado == "Activo"
                    //             && v.CodigoInstitucion == usuario.CodInstitucion);
                    query = proyectoController.VerTodosLosProyectosEmprendedorOAsesor(usuario.CodInstitucion, usuario.IdContacto);
                    break;
                case Constantes.CONST_Evaluador:
                case Constantes.CONST_CoordinadorEvaluador:
                    //query =
                    //    query.Where(
                    //             v => (consultas.Db.ProyectoContactos
                    //                  .Any(proyectoContacto => v.IdProyecto == proyectoContacto.CodProyecto
                    //                      && proyectoContacto.CodContacto == usuario.IdContacto
                    //                      && proyectoContacto.Inactivo == false))
                    //                  && v.Estado == "Activo"
                    //                  && v.CodigoEstado == Constantes.CONST_Evaluacion)
                    //        .Where(x => x.codOperador == usuario.CodOperador);
                    query = proyectoController.VerTodosLosProyectosEvaluacionCoordinador(usuario.IdContacto, usuario.CodOperador, Constantes.CONST_Evaluacion);
                    break;
                case Constantes.CONST_GerenteEvaluador:
                    //query = query.Where(p => p.Estado == "Activo"
                    //                         && (p.CodigoEstado == Constantes.CONST_Convocatoria
                    //                             || p.CodigoEstado == Constantes.CONST_Evaluacion)
                    //                             && (p.codOperador == usuario.CodOperador));
                    query = proyectoController.VerTodosLosProyectosGerenciaEvaluacion(Constantes.CONST_Evaluacion, usuario.CodOperador);
                    break;
                case Constantes.CONST_LiderRegional:
                    var lider = (from c in consultas.Db.Contacto
                                 where c.Id_Contacto == usuario.IdContacto
                                 select c).FirstOrDefault();
                    //query = query.Where(l => l.idDptoInst == lider.CodTipoAprendiz);
                    query = proyectoController.VerTodosLosProyectosLiderRegional(lider.CodTipoAprendiz);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(ddlbuscar.SelectedValue))
            {
                query = query.Where(t => t.NombreProyecto.StartsWith(ddlbuscar.SelectedValue)).ToList();
            }

            e.Arguments.TotalRowCount = query.Count();
            //e.Result = query.ToList();
            e.Result = query;
        }

        protected void gw_proyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                {
                    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CodigoEstado")) == Constantes.CONST_Inscripcion)
                    {
                        if (DataBinder.Eval(e.Row.DataItem, "Estado").ToString().ToLower() == "activo")
                        {
                            (e.Row.Cells[0].FindControl("ibtn_Inactivar")).Visible = true;
                        }
                        else
                        {
                            (e.Row.Cells[0].FindControl("ibtn_Activar")).Visible = true;
                        }
                    }
                }

                if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador ||
                    usuario.CodGrupo == Constantes.CONST_Evaluador
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    var query = (from cp in consultas.Db.ConvocatoriaProyectos
                                 from c in consultas.Db.Convocatoria
                                 where c.Id_Convocatoria == cp.CodConvocatoria
                                       &&
                                       cp.CodProyecto == Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IdProyecto"))
                                 select new { cp.CodConvocatoria, c.NomConvocatoria, cp.Fecha }).OrderByDescending(
                                     t => t.Fecha).Take(1);

                    //Make an adition
                    if (query.FirstOrDefault() != null)
                        ((LinkButton)e.Row.Cells[0].FindControl("hl_evaluacion")).CommandArgument =
                            string.Format("{0},{1}", DataBinder.Eval(e.Row.DataItem, "IdProyecto"), query.FirstOrDefault().CodConvocatoria);

                    if (query.FirstOrDefault() != null)
                        ((LinkButton)e.Row.Cells[0].FindControl("hl_evaluacion_hp")).CommandArgument =
                            string.Format("{0},{1}", DataBinder.Eval(e.Row.DataItem, "IdProyecto"), query.FirstOrDefault().CodConvocatoria);


                    var lblnombreC = e.Row.Cells[0].FindControl("lbconvocatoria") as Label;
                    var lblevaluador = e.Row.Cells[0].FindControl("levaluador_") as Label;

                    if (query.FirstOrDefault() != null)
                        lblnombreC.Text = query.FirstOrDefault().NomConvocatoria;
                    int codproyect = (int)DataBinder.Eval(e.Row.DataItem, "IdProyecto");
                    lblevaluador.Text = CargarEvaluadores(codproyect);
                    var lblavalado = e.Row.Cells[0].FindControl("lblavalado") as Label;
                    if (CargarAvalado(codproyect) != 0)
                    {
                        lblavalado.Text = "<img src='../../Images/chulo.gif' />";
                    }
                    else
                    {
                        lblavalado.Text = string.Empty;
                    }
                }


                //Unidad Reasignar
                if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema ||
                    usuario.CodGrupo == Constantes.CONST_AdministradorSena)
                {
                    if (usuario.CodInstitucion != Constantes.CONST_UnidadTemporal)
                    {
                        ((LinkButton)e.Row.Cells[0].FindControl("lbtn_reasignar")).Enabled = false;
                    }
                }

                //inactivo
                if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "Estado").ToString().ToLower() == "activo")
                    {
                        LinkButton lkb = ((LinkButton)e.Row.Cells[0].FindControl("lbtn_inactivo"));
                        lkb.Text = "activo";
                        lkb.Enabled = false;
                    }
                }

                if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                {
                    var lnkUnidad = (LinkButton)e.Row.FindControl("lbtn_reasignar");
                    var lnkEstado = (LinkButton)e.Row.FindControl("lbtn_inactivo");
                    var nomProyecto = (LinkButton)e.Row.FindControl("hl_proyecto");
                    lnkEstado.Enabled = false;
                    lnkUnidad.Enabled = false;
                    var idTipoUnidad = int.Parse(gw_proyectos.DataKeys[e.Row.RowIndex].Values[1].ToString());
                    if (idTipoUnidad == 3)
                    {
                        nomProyecto.Enabled = false;
                    }
                }

                //
            }
        }

        public string CargarEvaluadores(int codproyecto)
        {
            string evaluador = string.Empty;
            if (codproyecto != 0)
            {
                var query = consultas.Db.Contacto.Join(consultas.Db.ProyectoContactos,
                                                        (c => c.Id_Contacto),
                                                        (p => p.CodContacto),
                                                        (c, p) => new
                                                        {
                                                            Evaluador = c.Nombres + " " + c.Apellidos,
                                                            PInactivo = p.Inactivo,
                                                            CInactivo = c.Inactivo,
                                                            Codproyect = p.CodProyecto,
                                                            Rols = p.CodRol
                                                        }).Where(
                                                                          r =>
                                                                          r.PInactivo == false &&
                                                                          r.Rols == Constantes.CONST_RolEvaluador &&
                                                                          r.Codproyect == codproyecto &&
                                                                          r.CInactivo == false);


                if (query.Any())
                {
                    evaluador = Enumerable.Aggregate(query, evaluador, (valor, items) => valor + items.Evaluador);
                }
            }


            return evaluador;
        }

        public int CargarAvalado(int codproyecto)
        {
            int proyecto = 0;
            if (codproyecto != 0)
            {
                proyecto = consultas.ObtenerTabs(codproyecto);
            }

            return proyecto;
        }

        protected void gw_proyectos_DataBound(object sender, EventArgs e)
        {
            //Iterar según el rol del usuario en sesión.
            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_GerenteEvaluador:

                    gw_proyectos.Columns[2].Visible = false; //evaluar
                    gw_proyectos.Columns[3].Visible = false; //ciudad
                    gw_proyectos.Columns[4].Visible = false; //unidad
                    gw_proyectos.Columns[5].Visible = false; //estado
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //evaluar

                    break;

                case Constantes.CONST_CoordinadorEvaluador:

                    gw_proyectos.Columns[2].Visible = false; //evaluar
                    gw_proyectos.Columns[4].Visible = false; //unidad
                    gw_proyectos.Columns[5].Visible = false; //estado

                    break;
                case Constantes.CONST_Evaluador:

                    gw_proyectos.Columns[2].Visible = false; // ciudad
                    gw_proyectos.Columns[4].Visible = false; // evaluador
                    gw_proyectos.Columns[5].Visible = false; //avalado
                    gw_proyectos.Columns[6].Visible = false; //evaluar
                    gw_proyectos.Columns[7].Visible = false; //evaluar
                    break;


                case Constantes.CONST_JefeUnidad:
                    gw_proyectos.Columns[2].Visible = false; // evaluador
                    gw_proyectos.Columns[4].Visible = false; //avalado
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //avalado
                    gw_proyectos.Columns[9].Visible = false; //evaluar
                    break;

                case Constantes.CONST_Asesor:

                    gw_proyectos.Columns[2].Visible = false; // evaluador
                    gw_proyectos.Columns[4].Visible = false; //avalado
                    gw_proyectos.Columns[5].Visible = false; // evaluador
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //avalado
                    gw_proyectos.Columns[9].Visible = false; //evaluar

                    break;
                case Constantes.CONST_AdministradorSistema:
                    gw_proyectos.Columns[2].Visible = false; // evaluador
                    gw_proyectos.Columns[5].Visible = false;
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //avalado
                    gw_proyectos.Columns[9].Visible = false; //avalado
                    break;

                case Constantes.CONST_CallCenterOperador:
                case Constantes.CONST_CallCenter:
                    gw_proyectos.Columns[2].Visible = false; //evaluar
                    gw_proyectos.Columns[3].Visible = true; //ciudad
                    //Ocultar el resto de las columnas.
                    gw_proyectos.Columns[4].Visible = false; //unidad
                    gw_proyectos.Columns[5].Visible = false; //estado
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //evaluar
                    gw_proyectos.Columns[9].Visible = false; //avalado
                    break;

                case Constantes.CONST_LiderRegional:
                    gw_proyectos.Columns[2].Visible = false;
                    gw_proyectos.Columns[9].Visible = false;
                    gw_proyectos.Columns[8].Visible = false;
                    gw_proyectos.Columns[7].Visible = false;
                    gw_proyectos.Columns[6].Visible = false;
                    break;
                default:
                    gw_proyectos.Columns[5].Visible = false;
                    gw_proyectos.Columns[6].Visible = false; // evaluador
                    gw_proyectos.Columns[7].Visible = false; //avalado
                    gw_proyectos.Columns[8].Visible = false; //avalado
                    gw_proyectos.Columns[9].Visible = false; //avalado
                    break;
            }
        }

        protected void gw_proyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var IdProyecto = e.CommandArgument.ToString();
            string[] variables = IdProyecto.Split(',');

            switch (e.CommandName)
            {
                case "Evaluacion":
                    HttpContext.Current.Session["CodProyecto"] = variables[0];
                    HttpContext.Current.Session["CodConvocatoria"] = variables[1];
                    HttpContext.Current.Session["HistorialEvaluacion"] = null;
                    Response.Redirect("../evaluacion/EvaluacionFrameSet.aspx");
                    break;
                case "InActivar":
                    CargarFormularioInActivarProyecto(variables[0]);
                    break;
                case "Activar":
                    ActivarProyecto(variables[0]);
                    break;
                case "Evaluacions":
                    HttpContext.Current.Session["CodProyecto"] = variables[0];
                    HttpContext.Current.Session["CodConvocatoria"] = 0;
                    HttpContext.Current.Session["HistorialEvaluacion"] = null;
                    Response.Redirect("../evaluacion/EvaluacionFrameSet.aspx");
                    break;
                case "Frameset":
                    HttpContext.Current.Session["CodProyecto"] = e.CommandArgument;
                    Response.Redirect("ProyectoFrameSet.aspx");
                    break;
                case "ventanaInactivo":
                    LinkButton lnkBtn = e.CommandSource as LinkButton;

                    if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                    {
                        if (lnkBtn.Text == "Inactivo")
                        {
                            HttpContext.Current.Session["CodProyecto"] = lnkBtn.CommandArgument;
                            Redirect(null, "InactivarProyecto.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=260,top=100");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected void ActivarProyecto(string IdProyecto)
        {
            var query = (from p in consultas.Db.Proyecto
                         where p.Id_Proyecto == Convert.ToInt32(IdProyecto) &&
                               p.CodInstitucion == usuario.CodInstitucion
                         select p).First();
            query.Inactivo = false;
            query.MotivoDesactivacion = null;
            consultas.Db.SubmitChanges();

            var queryNombre = (from p in consultas.Db.Proyecto
                               where p.Id_Proyecto == Convert.ToInt32(IdProyecto)
                               select new { p.NomProyecto }).First();
            string nombreProyecto = queryNombre.NomProyecto;
            AgendarTarea agenda = new AgendarTarea(usuario.IdContacto, "Asignar Asesor",
                                                   "Asignar Asesor a el proyecto " + nombreProyecto, IdProyecto, 3, "0",
                                                   false,
                                                   1, true, false, usuario.IdContacto, "CodProyecto=" + IdProyecto, "",
                                                   "Asignar Asesor");
            agenda.Agendar();
        }

        protected void CargarFormularioInActivarProyecto(string IdProyecto)
        {
            pnlPrincipal.Visible = false;
            pnlInActivar.Visible = true;
            btnInActivar.Visible = true;
            hddIdProyecto.Value = IdProyecto;

            var query = (from p in consultas.Db.Proyecto
                         where p.Id_Proyecto == Convert.ToInt32(IdProyecto)
                         select new { p.MotivoDesactivacion, p.Inactivo, p.NomProyecto }).First();
            txtMotivoInactivacion.Text = query.MotivoDesactivacion;
            lblTitulo.Text = "MOTIVO INACTIVACION";
            if (query.Inactivo == false)
            {
                btnInActivar.Visible = true;
                lblTitulo.Text = "INACTIVAR PROYECTO " + query.NomProyecto.ToUpper() + "";
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlInActivar.Visible = false;
            hddIdProyecto.Value = "";
            txtMotivoInactivacion.Text = "";
        }

        protected void btnInActivar_Click(object sender, EventArgs e)
        {
            string idProyecto = hddIdProyecto.Value;

            if (String.IsNullOrEmpty(txtMotivoInactivacion.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar un motivo de inactivación de no mas de 300 caracteres.')", true);
                return;
            }
            try
            {
                //Inactivar proyecto
                consultas.Db.ExecuteCommand(
                    "update proyecto set fechadesactivacion=getdate(), MotivoDesactivacion={0}, inactivo=1 where id_proyecto={1}",
                    txtMotivoInactivacion.Text, Convert.ToInt32(idProyecto));

                //Inactivar usuarios emprendedores del proyecto  
                string modifica = "update contacto set inactivo=1 where id_contacto in ";
                modifica += " (select p.codcontacto from proyectocontacto p, grupocontacto g ";
                modifica += " where p.codcontacto=g.codcontacto and codgrupo={0}";
                modifica += " and codproyecto={1} and inactivo=0) ";
                consultas.Db.ExecuteCommand(modifica, Constantes.CONST_Emprendedor, Convert.ToInt32(idProyecto));

                //Inactivar usuarios emprendedores. Los asesores no se inactivan porque pueden tener otros proyectos
                string elimina = "delete from grupocontacto where codgrupo={0} and codcontacto in ";
                elimina += "(select codcontacto from proyectocontacto where codproyecto={1} and inactivo=0)";
                consultas.Db.ExecuteCommand(elimina, Constantes.CONST_Emprendedor, Convert.ToInt32(idProyecto));

                //Inactivar usuarios dentro del proyecto
                consultas.Db.ExecuteCommand(
                    "update proyectocontacto set inactivo=1, fechafin=getdate() where inactivo=0 and codproyecto={1}",
                    Convert.ToInt32(idProyecto));

                //Cerrar Tareas Pendientes relacionadas con el proyecto
                string modificaTarea =
                    "update tareausuariorepeticion set respuesta = 'Cerrada por Inactivacion proyecto', fechacierre=getdate() where codtareausuario in ";
                modificaTarea += "(select id_tareausuario from tareausuario where codproyecto={0}) and fechacierre is null";
                consultas.Db.ExecuteCommand(modificaTarea, Convert.ToInt32(idProyecto));

                pnlPrincipal.Visible = true;
                pnlInActivar.Visible = false;
                hddIdProyecto.Value = "";
                txtMotivoInactivacion.Text = "";
            }
            catch (FormatException) { }
            catch (Exception) { }
            gw_proyectos.DataBind();
            Response.Redirect("Proyectos.aspx");
        }

        protected void ibtn_Activar_Click(object sender, ImageClickEventArgs e)
        {

            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVProyecto = gw_proyectos.Rows[indicefila];
            Int64 CodProyecto = Int64.Parse(gw_proyectos.DataKeys[GVProyecto.RowIndex].Value.ToString());

            ActivarEIncativar(CodProyecto);
        }

        protected void lbtn_inactivo_Click(object sender, EventArgs e)
        {

            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVProyecto = gw_proyectos.Rows[indicefila];
            Int64 CodProyecto = Int64.Parse(gw_proyectos.DataKeys[GVProyecto.RowIndex].Value.ToString());

            ActivarEIncativar(CodProyecto);
        }

        private void ActivarEIncativar(Int64 CodProyecto)
        {
            String txtSQL = "update proyecto set inactivo=0, motivodesactivacion=NULL where id_proyecto=" + CodProyecto + " and codinstitucion=" + usuario.CodInstitucion;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(txtSQL, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                ActivarProyecto("" + CodProyecto);

                gw_proyectos.DataBind();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void ddlbuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            gw_proyectos.DataBind();
        }

        protected void gw_proyectos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gw_proyectos.PageIndex = e.NewPageIndex;
            gw_proyectos.DataBind();
        }

        protected void gw_proyectos_Sorting(object sender, GridViewSortEventArgs e)
        {
        }
    }

    
}