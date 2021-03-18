using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividadRechazo : Negocio.Base_Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Title = "Solicitudes de Pago Enviadas a Fiduciaria";

                if (!IsPostBack)
                { CargarGrilla(usuario.CodGrupo); }
            }
            catch { }

            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                lnk_excel.Visible = true;
                lnk_excel.Enabled = true;
            }
        }

        
        private void CargarGrilla(Int32 CodGrupo_Usuario)
        {
            //Inicializar variables:
            String sqlConsulta = "";
            DataTable result = new DataTable();

            try
            {
                switch (CodGrupo_Usuario)
                {
                    case Constantes.CONST_CoordinadorInterventor:
                        sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, " +
                                     " PagoActividad.CantidadDinero " +
                                     " FROM PagoActividad " +
                                     " INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto  " +
                                     " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                     " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                     " inner join contacto ci on interventor.CodContacto = ci.Id_Contacto "+
                                     " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + ") " +
                                     " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                     " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                     " AND (EmpresaInterventor.Inactivo = 0)" +
                                     " and ci.codOperador = " + usuario.CodOperador;

                        //Bindear la grilla.
                        result = null;
                        result = consultas.ObtenerDataTable(sqlConsulta, "text");
                        HttpContext.Current.Session["dtEmpresas"] = result;
                        gv_pagosactividad.DataSource = result;
                        gv_pagosactividad.DataBind();
                        break;
                    case Constantes.CONST_GerenteInterventor:
                        sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, " +
                                      " Empresa.razonsocial, PagoActividad.CantidadDinero  " +
                                      " FROM PagoActividad INNER JOIN Empresa " +
                                      " ON PagoActividad.CodProyecto = Empresa.codproyecto  " +
                                      " inner join proyecto p on PagoActividad.CodProyecto = p.Id_Proyecto " +
                                      " WHERE (PagoActividad.Estado = " + Constantes.CONST_EstadoFiduciaria + ") " +
                                      " and p.codOperador =  "+ usuario.CodOperador +
                                      " ORDER BY PagoActividad.Id_PagoActividad";
                        //Bindear la grilla.
                        result = null;
                        result = consultas.ObtenerDataTable(sqlConsulta, "text");
                        HttpContext.Current.Session["dtEmpresas"] = result;
                        gv_pagosactividad.DataSource = result;
                        gv_pagosactividad.DataBind();
                        break;

                    default:
                        break;
                }

                #region Código anterior comentado.
                //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                //{
                //    txtidproyecto.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                //    if (!IsPostBack)
                //    {
                //        var result = from ap in consultas.Db.MD_ReportPagosActividad(usuario.CodGrupo, usuario.IdContacto) orderby ap.Id_PagoActividad select ap;

                //        if (result != null)
                //        {
                //            HttpContext.Current.Session["dtEmpresas"] = result;
                //            gv_pagosactividad.DataSource = result;
                //            gv_pagosactividad.DataBind();
                //        }
                //    }
                //} 
                #endregion
            }
            catch { }
        }

        
        protected void gv_pagosactividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PagosActividad")
            {
                HttpContext.Current.Session["Id_PagoActividadRechazo"] = e.CommandArgument;
                Redirect(null, "PagoRechazado.aspx", "_Blank", "width=500,height=400");
            }
        }

        
        protected void btnbuscar_Click(object sender, EventArgs e)
        {
            #region Versión de Mauricio Arias Olave.

            //Inicializar variables.
            String sqlConsulta = "";
            DataTable result = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    #region Ejecutar la consulta que verá el Coordinador Interventor.

                    if (txtidproyecto.Text.Trim() != "" && txtnomproyecto.Text.Trim() != "")
                    {
                        #region Buscar por los dos parámetros.

                        sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                      " 	   PagoActividad.CantidadDinero " +
                                      " FROM PagoActividad INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                      " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " inner join contacto ci on interventor.CodContacto = ci.Id_Contacto " +
                                      " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + " ) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                      " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                      " AND (EmpresaInterventor.Inactivo = 0)" +
                                      " AND PagoActividad.Id_PagoActividad = " + txtidproyecto.Text.Trim() +
                                      " AND Empresa.razonsocial like '%" + txtnomproyecto.Text.Trim() + "%'" +
                                      " and ci.codOperador = " + usuario.CodOperador +
                                      " ORDER BY PagoActividad.Id_PagoActividad ";

                        //Bindear la grilla.
                        result = null;
                        result = consultas.ObtenerDataTable(sqlConsulta, "text");
                        HttpContext.Current.Session["dtEmpresas"] = result;
                        gv_pagosactividad.DataSource = result;
                        gv_pagosactividad.DataBind();

                        #endregion
                    }
                    else
                    {
                        #region Buscar por Id o Nombre.
                        if (txtidproyecto.Text.Trim() != "" || txtnomproyecto.Text.Trim() != "")
                        {
                            if (txtidproyecto.Text.Trim() != "")
                            {
                                #region Buscar por el Id_Proyecto.

                                sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                              " 	   PagoActividad.CantidadDinero " +
                                              " FROM PagoActividad INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                              " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                              " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                              "  inner join contacto ci on interventor.CodContacto = ci.Id_Contacto " + 
                                              " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + " ) " +
                                              " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                              " AND (EmpresaInterventor.Inactivo = 0)" +
                                              " AND PagoActividad.Id_PagoActividad = " + txtidproyecto.Text.Trim() +
                                               " and ci.codOperador = " + usuario.CodOperador +
                                              " ORDER BY PagoActividad.Id_PagoActividad ";

                                //Bindear la grilla.
                                result = null;
                                result = consultas.ObtenerDataTable(sqlConsulta, "text");
                                HttpContext.Current.Session["dtEmpresas"] = result;
                                gv_pagosactividad.DataSource = result;
                                gv_pagosactividad.DataBind();
                                #endregion
                            }
                            if (txtnomproyecto.Text.Trim() != "")
                            {
                                #region Buscar por el nombre del proyecto.

                                sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                              " 	   PagoActividad.CantidadDinero " +
                                              " FROM PagoActividad INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                              " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                              " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                              "  inner join contacto ci on interventor.CodContacto = ci.Id_Contacto " +
                                              " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + " ) " +
                                              " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                              " AND (EmpresaInterventor.Inactivo = 0)" +
                                              " AND Empresa.razonsocial like '%" + txtnomproyecto.Text.Trim() + "%'" +
                                              " and ci.codOperador = " + usuario.CodOperador +
                                              " ORDER BY PagoActividad.Id_PagoActividad ";



                                //Bindear la grilla.
                                result = null;
                                result = consultas.ObtenerDataTable(sqlConsulta, "text");
                                HttpContext.Current.Session["dtEmpresas"] = result;
                                gv_pagosactividad.DataSource = result;
                                gv_pagosactividad.DataBind();
                                #endregion
                            }
                        }
                        else
                        {
                            //Cargar la grilla normalmente.
                            CargarGrilla(usuario.CodGrupo);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Ejecutar la consulta que verá el Gerente Interventor.

                    if (txtidproyecto.Text.Trim() != "" && txtnomproyecto.Text.Trim() != "")
                    {
                        #region Buscar por los dos parámetros.
                        sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                      " PagoActividad.CantidadDinero " +
                                      " FROM PagoActividad INNER JOIN Empresa " +
                                      " ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                      " inner join proyecto p on PagoActividad.CodProyecto = p.Id_Proyecto " +
                                      " WHERE (PagoActividad.Estado = " + Constantes.CONST_EstadoFiduciaria + ") " +
                                      " AND PagoActividad.Id_PagoActividad = " + txtidproyecto.Text.Trim() +
                                      " AND Empresa.razonsocial like '%" + txtnomproyecto.Text.Trim() + "%'" +
                                      " and p.codOperador = " +usuario.CodOperador +
                                      " ORDER BY PagoActividad.Id_PagoActividad ";

                        //Bindear la grilla.
                        result = null;
                        result = consultas.ObtenerDataTable(sqlConsulta, "text");
                        HttpContext.Current.Session["dtEmpresas"] = result;
                        gv_pagosactividad.DataSource = result;
                        gv_pagosactividad.DataBind();
                        #endregion
                    }
                    else
                    {
                        if (txtidproyecto.Text.Trim() != "" || txtnomproyecto.Text.Trim() != "")
                        {
                            if (txtidproyecto.Text.Trim() != "")
                            {
                                #region Buscar por el Id_Proyecto.
                                //sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, " +
                                //              " PagoActividad.CantidadDinero " +
                                //              " FROM PagoActividad INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                //              " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                //              " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                //              " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + ") " +
                                //              " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                //              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                //              " AND (EmpresaInterventor.Inactivo = 0)" +
                                //              " AND PagoActividad.Id_PagoActividad = " + txtidproyecto.Text.Trim() +
                                //              " ORDER BY PagoActividad.Id_PagoActividad ";


                                sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                              " PagoActividad.CantidadDinero " +
                                              " FROM PagoActividad INNER JOIN Empresa " +
                                              " ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                              " inner join proyecto p on PagoActividad.CodProyecto = p.Id_Proyecto " +
                                              " WHERE (PagoActividad.Estado = " + Constantes.CONST_EstadoFiduciaria + ") " +
                                              " AND PagoActividad.Id_PagoActividad = " + txtidproyecto.Text.Trim() +
                                              " and p.codOperador = " + usuario.CodOperador +
                                              " ORDER BY PagoActividad.Id_PagoActividad ";

                                //Bindear la grilla.
                                result = null;
                                result = consultas.ObtenerDataTable(sqlConsulta, "text");
                                HttpContext.Current.Session["dtEmpresas"] = result;
                                gv_pagosactividad.DataSource = result;
                                gv_pagosactividad.DataBind();
                                #endregion
                            }
                            if (txtnomproyecto.Text.Trim() != "")
                            {
                                #region Buscar por el nombre del proyecto.
                                //sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, " +
                                //              " PagoActividad.CantidadDinero " +
                                //              " FROM PagoActividad INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                //              " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                //              " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                //              " WHERE (PagoActividad.Estado > " + Constantes.CONST_EstadoFiduciaria + ") " +
                                //              " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                //              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                //              " AND (EmpresaInterventor.Inactivo = 0)" +
                                //              " AND Empresa.razonsocial like '%" + txtnomproyecto.Text.Trim() + "%'" +
                                //              " ORDER BY PagoActividad.Id_PagoActividad ";

                                sqlConsulta = " SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial,  " +
                                              " PagoActividad.CantidadDinero " +
                                              " FROM PagoActividad INNER JOIN Empresa " +
                                              " ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                                               " inner join proyecto p on PagoActividad.CodProyecto = p.Id_Proyecto " +
                                              " WHERE (PagoActividad.Estado = " + Constantes.CONST_EstadoFiduciaria + ") " +
                                              " AND Empresa.razonsocial like '%" + txtnomproyecto.Text.Trim() + "%'" +
                                              " and p.codOperador = " + usuario.CodOperador +
                                              " ORDER BY PagoActividad.Id_PagoActividad ";

                                //Bindear la grilla.
                                result = null;
                                result = consultas.ObtenerDataTable(sqlConsulta, "text");
                                HttpContext.Current.Session["dtEmpresas"] = result;
                                gv_pagosactividad.DataSource = result;
                                gv_pagosactividad.DataBind();
                                #endregion
                            }
                        }
                        else
                        {
                            //Cargar la grilla normalmente.
                            CargarGrilla(usuario.CodGrupo);
                        }
                    }
                    #endregion
                }
            }
            catch { }

            #endregion

            #region Código anterior comentado.
            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            //{
            //    var result = from ap in consultas.Db.MD_ReportPagosActividad(usuario.CodGrupo, usuario.IdContacto) select ap;

            //    if (!string.IsNullOrEmpty(txtidproyecto.Text))
            //    {
            //        result = result.Where(ap => ap.Id_PagoActividad == Convert.ToInt32(txtidproyecto.Text));
            //    }

            //    if (!string.IsNullOrEmpty(txtnomproyecto.Text))
            //    {
            //        result = result.Where(ap => ap.razonsocial.ToString().ToLower().Contains(txtnomproyecto.Text.ToString().ToLower()));
            //    }

            //    result = result.OrderBy(ap => ap.Id_PagoActividad);

            //    gv_pagosactividad.DataSource = result;
            //    gv_pagosactividad.DataBind();
            //} 
            #endregion
        }

       
        protected void gv_pagosactividad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_pagosactividad.PageIndex = e.NewPageIndex;
            CargarGrilla(usuario.CodGrupo);
        }

        
        protected void gv_pagosactividad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbl_fecha = e.Row.FindControl("lblfecha") as Label;
                var lbl_valor = e.Row.FindControl("lblvalor") as Label;
                DateTime fecha = new DateTime();

                if (lbl_valor != null && lbl_fecha != null)
                {
                    #region Formatear valor moneda.
                    try
                    {
                        Double valor = Convert.ToDouble(lbl_valor.Text);
                        lbl_valor.Text = valor.ToString();
                    }
                    catch { }
                    #endregion

                    #region Formatear valor fecha.

                    try
                    {
                        //Establecer fecha obtenida del campo de texto en variable.
                        fecha = Convert.ToDateTime(lbl_fecha.Text);

                        //Obtener la hora en minúscula.
                        string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

                        //Mostrar fecha total.
                        lbl_fecha.Text = fecha.ToString("dd/MM/yyyy") + " " + hora + ".";
                    }
                    catch { }

                    #endregion
                }
            }
        }

        protected void lnk_excel_Click(object sender, EventArgs e)
        {
            Redirect(null, "SubirRespuestaPagos.aspx", "_Blank", "width=700,height=500");
        }
    }
}