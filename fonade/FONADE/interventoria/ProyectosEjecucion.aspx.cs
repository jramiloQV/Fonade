using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class ProyectosEjecucion : Negocio.Base_Page
    {
        String txtSQL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page.Title = "FONDO EMPRENDER - Proyectos en Ejecución";
                txtidproyecto.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                CargarProyectosEnEjecucion("");
            }
        }

        protected void gv_proyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ProyetoFrame"))
            {
                string[] palabras = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["CodProyecto"] = palabras[0];
                HttpContext.Current.Session["CodEmpresa"] = palabras[1];
                Response.Redirect("../Proyecto/ProyectoFrameSet.aspx");
            }
            else
            {
                if (e.CommandName.Equals("SeguimientoFrame"))
                {
                    string[] param = e.CommandArgument.ToString().Split(';');

                    HttpContext.Current.Session["CodProyecto"] = param[0];
                    HttpContext.Current.Session["CodEmpresa"] = param[1];


                    Response.Redirect("SeguimientoFrameSet.aspx");
                }
                else
                {
                    if (e.CommandName.Equals("mailtoEnviar"))
                    {
                        Response.Redirect("mailto:" + e.CommandArgument);
                    }
                }
            }
        }

        protected void btnbuscar_Click(object sender, EventArgs e)
        {
            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            //{
            //    var result = from p in consultas.Db.MD_ProyectosEjecucion()
            //                 where p.CodCoordinador == usuario.IdContacto
            //                 select p;

            //    if (!string.IsNullOrEmpty(txtidproyecto.Text))
            //    {
            //        result = result.Where(p => p.Id_Proyecto == Convert.ToInt32(txtidproyecto.Text));
            //    }

            //    if (!string.IsNullOrEmpty(txtnomempresa.Text))
            //    {
            //        result = result.Where(p => p.NomEmpresa.ToString().ToLower().Contains(txtnomempresa.Text.ToString().ToLower()));
            //    }

            //    if (!string.IsNullOrEmpty(txtnomproyecto.Text))
            //    {
            //        result = result.Where(p => p.NomProyecto.ToString().ToLower().Contains(txtnomproyecto.Text.ToString().ToLower()));
            //    }

            //    gv_proyectos.DataSource = result;
            //    gv_proyectos.DataBind();


            //}

            //else if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            //{
            //    var result = from p in consultas.Db.MD_ProyectosEjecucion()
            //                 select p;

            //    if (!string.IsNullOrEmpty(txtidproyecto.Text))
            //    {
            //        result = result.Where(p => p.Id_Proyecto == Convert.ToInt32(txtidproyecto.Text));
            //    }

            //    if (!string.IsNullOrEmpty(txtnomempresa.Text))
            //    {
            //        result = result.Where(p => p.NomEmpresa.ToString().ToLower().Contains(txtnomempresa.Text.ToString().ToLower()));
            //    }

            //    if (!string.IsNullOrEmpty(txtnomproyecto.Text))
            //    {
            //        result = result.Where(p => p.NomProyecto.ToString().ToLower().Contains(txtnomproyecto.Text.ToString().ToLower()));
            //    }

            //    gv_proyectos.DataSource = result;
            //    gv_proyectos.DataBind();

            //}

            String txt_sql_param = "";
         
            if (txtidproyecto.Text.Trim() != "" || txtnomempresa.Text.Trim() != "" || txtnomproyecto.Text.Trim() != "")
            {

                if (!string.IsNullOrEmpty(txtidproyecto.Text))
                { txt_sql_param = "  AND Proyecto.Id_Proyecto = " + txtidproyecto.Text.Trim(); }


                if (!string.IsNullOrEmpty(txtnomempresa.Text))
                { txt_sql_param = " AND Empresa.RazonSocial like '%" + txtnomempresa.Text.Trim() + "%'"; }


                if (!string.IsNullOrEmpty(txtnomproyecto.Text))
                { txt_sql_param = "  AND Proyecto.NomProyecto like '%" + txtnomproyecto.Text.Trim() + "%'"; }


                CargarProyectosEnEjecucion(txt_sql_param);
            }
            else
            {

                txt_sql_param = "";
                CargarProyectosEnEjecucion(txt_sql_param);
            }
        }

        protected void gv_proyectos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                gv_proyectos.PageIndex = e.NewPageIndex;
                gv_proyectos.DataSource = dt;
                gv_proyectos.DataBind();
            }
        }

        private void CargarProyectosEnEjecucion(String txt_sql)
        {
            DataTable tabla = new DataTable();

            try
            {
                if (txt_sql.Trim() == "")
                {
                    
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        txtSQL = " SELECT Proyecto.Id_Proyecto, Proyecto.NomProyecto, Empresa.Id_Empresa," +
                                 "Empresa.RazonSocial AS NomEmpresa, Interventor.CodContacto," +
                                 "Contacto_1.Nombres + ' ' + Contacto_1.Apellidos AS NomCoordinador," +
                                 "Contacto.Nombres + ' ' + Contacto.Apellidos AS NomInterventor," +
                                 "Contacto.Email AS EmailInterventor, Contacto_1.Email AS EmailCoordinador" +
                                 ",(select top 1 convocatoria.NomConvocatoria from EvaluacionObservacion inner join convocatoria on EvaluacionObservacion.CodConvocatoria = convocatoria.Id_Convocatoria where EvaluacionObservacion.CodProyecto = Proyecto.Id_Proyecto order by 1 desc) Convocatoria "+
                                 " FROM Proyecto INNER JOIN Empresa ON Proyecto.Id_Proyecto = Empresa.codproyecto" +
                                 " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa" +
                                 " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto" +
                                 " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto" +
                                 " INNER JOIN Contacto Contacto_1 ON Interventor.CodCoordinador = Contacto_1.Id_Contacto" +
                                 " WHERE (Proyecto.CodEstado = 7)" +
                                 " AND (EmpresaInterventor.Rol = 8)" +
                                 " AND (EmpresaInterventor.Inactivo = 0)" +
                                 " AND Interventor.CodCoordinador = " + usuario.IdContacto +
                                 " AND Proyecto.codOperador = " + usuario.CodOperador +
                                 " ORDER BY Proyecto.Id_Proyecto DESC";
                    }
                    else
                    {
                        
                        txtSQL = " SELECT Proyecto.Id_Proyecto, Proyecto.NomProyecto, Empresa.Id_Empresa, Empresa.RazonSocial AS NomEmpresa, Interventor.CodContacto," +
                                   "Contacto_1.Nombres + ' ' + Contacto_1.Apellidos AS NomCoordinador," +
                                   "Contacto.Nombres + ' ' + Contacto.Apellidos AS NomInterventor," +
                                   "Contacto.Email AS EmailInterventor, Contacto_1.Email AS EmailCoordinador" +
                                   ",(select top 1 convocatoria.NomConvocatoria from EvaluacionObservacion inner join convocatoria on EvaluacionObservacion.CodConvocatoria = convocatoria.Id_Convocatoria where EvaluacionObservacion.CodProyecto = Proyecto.Id_Proyecto order by 1 desc) Convocatoria " +
                                   " FROM Proyecto INNER JOIN Empresa ON Proyecto.Id_Proyecto = Empresa.codproyecto" +
                                   " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa" +
                                   " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto" +
                                   " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto" +
                                   " INNER JOIN Contacto Contacto_1 ON Interventor.CodCoordinador = Contacto_1.Id_Contacto" +
                                   " WHERE (Proyecto.CodEstado = 7)" +
                                   " AND Proyecto.codOperador = " + usuario.CodOperador +
                                   " AND (EmpresaInterventor.Rol = 8)" +
                                   " AND (EmpresaInterventor.Inactivo = 0)" +
                                   " ORDER BY Proyecto.Id_Proyecto";

                    }
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        txtSQL = " SELECT Proyecto.Id_Proyecto,Proyecto.NomProyecto,Empresa.Id_Empresa,";
                        txtSQL = txtSQL + "Empresa.RazonSocial AS NomEmpresa, Interventor.CodContacto,";
                        txtSQL = txtSQL + "Contacto_1.Nombres + ' ' + Contacto_1.Apellidos AS NomCoordinador,";
                        txtSQL = txtSQL + "Contacto.Nombres + ' ' + Contacto.Apellidos AS NomInterventor,";
                        txtSQL = txtSQL + "Contacto.Email AS EmailInterventor, Contacto_1.Email AS EmailCoordinador ";
                        txtSQL = txtSQL + ",(select top 1 convocatoria.NomConvocatoria from EvaluacionObservacion inner join convocatoria on EvaluacionObservacion.CodConvocatoria = convocatoria.Id_Convocatoria where EvaluacionObservacion.CodProyecto = Proyecto.Id_Proyecto order by 1 desc) Convocatoria ";
                        txtSQL = txtSQL + " FROM Proyecto INNER JOIN Empresa ON Proyecto.Id_Proyecto = Empresa.codproyecto ";
                        txtSQL = txtSQL + " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa ";
                        txtSQL = txtSQL + " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto ";
                        txtSQL = txtSQL + " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto ";
                        txtSQL = txtSQL + " INNER JOIN Contacto Contacto_1 ON Interventor.CodCoordinador = Contacto_1.Id_Contacto ";
                        txtSQL = txtSQL + " WHERE (Proyecto.CodEstado = 7) ";
                        txtSQL = txtSQL + " AND (EmpresaInterventor.Rol = 8) ";
                        txtSQL = txtSQL + " AND (EmpresaInterventor.Inactivo = 0) ";
                        txtSQL = txtSQL + " AND Proyecto.codOperador = " + usuario.CodOperador;
                        txtSQL = txtSQL + " AND Interventor.CodCoordinador = " + usuario.IdContacto +" "+ txt_sql;
                        txtSQL = txtSQL + " ORDER BY Proyecto.Id_Proyecto ";
                    }
                    else
                    {
                        txtSQL = "SELECT Proyecto.Id_Proyecto, Proyecto.NomProyecto, Empresa.Id_Empresa," +
                                 "Empresa.RazonSocial AS NomEmpresa, Interventor.CodContacto," +
                                 "Contacto_1.Nombres + ' ' + Contacto_1.Apellidos AS NomCoordinador," +
                                 "Contacto.Nombres + ' ' + Contacto.Apellidos AS NomInterventor," +
                                 "Contacto.Email AS EmailInterventor, Contacto_1.Email AS EmailCoordinador" +
                                 ",(select top 1 convocatoria.NomConvocatoria from EvaluacionObservacion inner join convocatoria on EvaluacionObservacion.CodConvocatoria = convocatoria.Id_Convocatoria where EvaluacionObservacion.CodProyecto = Proyecto.Id_Proyecto order by 1 desc) Convocatoria " +
                                 "  FROM Proyecto INNER JOIN Empresa ON Proyecto.Id_Proyecto = Empresa.codproyecto " +
                                 "  INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                 "  INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                 "  INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                 "  INNER JOIN Contacto Contacto_1 ON Interventor.CodCoordinador = Contacto_1.Id_Contacto " +
                                 "  WHERE (Proyecto.CodEstado = 7) " +
                                 "  AND Proyecto.codOperador = " + usuario.CodOperador +
                                 "  AND (EmpresaInterventor.Rol = 8) " +
                                 "  AND (EmpresaInterventor.Inactivo = 0) " + txt_sql +
                                 "  ORDER BY Proyecto.Id_Proyecto ";
                    }
                }
                tabla = consultas.ObtenerDataTable(txtSQL, "text");
                HttpContext.Current.Session["dtEmpresas"] = tabla;
                gv_proyectos.DataSource = tabla;
                gv_proyectos.DataBind();
            }
            catch (Exception ex)
            { string a = ex.Message; }
        }
    }
}