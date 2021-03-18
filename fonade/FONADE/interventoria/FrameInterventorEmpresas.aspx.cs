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
using Fonade.Negocio;

namespace Fonade.FONADE.interventoria
{
    public partial class FrameInterventorEmpresas : Base_Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SELECT Id_Sector, NomSector FROM Sector ORDER BY NomSector
                var query = (from s in consultas.Db.Sector
                             orderby s.NomSector
                             select new
                             {
                                 s.Id_Sector,
                                 s.NomSector
                             });

                if(query.Any())
                {
                    dd_Sectores.DataSource = query;
                    dd_Sectores.DataValueField = "Id_Sector";
                    dd_Sectores.DataTextField = "NomSector";

                    dd_Sectores.DataBind();

                    dd_Sectores.Items.Insert(0, new ListItem { Text = "", Value = "[Empty]" });
                    dd_Sectores.Items.Insert(1, new ListItem { Text = "[TODOS]", Value = "[All]" });

                }

                //GetSelectedRecord();
                ObtenerCodSubSector();
            }
        }


        ///// <summary>
        ///// Obtener el registro seleccionado.
        ///// </summary>
        //private void GetSelectedRecord()
        //{
        //    for (int i = 0; i < gv_SubDetalles_interventores.Rows.Count; i++)
        //    {
        //        RadioButton rb = (RadioButton)gv_SubDetalles_interventores.Rows[i].Cells[0].FindControl("rb_interv_lider");

        //        if (rb != null)
        //        {
        //            if (rb.Checked)
        //            {
        //                HiddenField hf = (HiddenField)gv_SubDetalles_interventores.Rows[i].Cells[0].FindControl("HiddenField1");
        //                if (hf != null)
        //                    ViewState["SelectedContact"] = hf.Value;

        //                break;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// nuevo registro
        ///// Establecer el registro seleccionado.
        ///// </summary>
        //private void SetSelectedRecord()
        //{
        //    for (int i = 0; i < gv_SubDetalles_interventores.Rows.Count; i++)
        //    {
        //        RadioButton rb = (RadioButton)gv_SubDetalles_interventores.Rows[i].Cells[0]
        //                                        .FindControl("rb_interv_lider");
        //        if (rb != null)
        //        {
        //            HiddenField hf = (HiddenField)gv_SubDetalles_interventores.Rows[i]
        //                                .Cells[0].FindControl("HiddenField1");
        //            if (hf != null && ViewState["SelectedContact"] != null)
        //            {
        //                if (hf.Value.Equals(ViewState["SelectedContact"].ToString()))
        //                {
        //                    rb.Checked = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        private void ObtenerCodSubSector()
        {
            if (!dd_Sectores.SelectedValue.Equals("[Empty]"))
            {
                var eempresa = (from e in consultas.Db.Empresas
                                join p in consultas.Db.Proyecto on e.codproyecto equals p.Id_Proyecto
                                orderby e.codproyecto ascending
                                where p.CodEstado >= Constantes.CONST_Ejecucion
                                && p.codOperador == usuario.CodOperador
                                select new
                                {
                                    Id_Sector = dd_Sectores.SelectedValue,
                                    e.codproyecto,
                                    razonsocial = e.codproyecto + "-" + e.razonsocial,
                                    p.CodSubSector
                                });

                if (!dd_Sectores.SelectedValue.Equals("[Empty]") && !dd_Sectores.SelectedValue.Equals("[All]"))
                {

                    var result = (from e in eempresa
                                  join ss in consultas.Db.SubSector on e.CodSubSector equals ss.Id_SubSector
                                  orderby e.codproyecto ascending
                                  where ss.CodSector == int.Parse(dd_Sectores.SelectedValue)
                                  select new
                                  {
                                      Id_Sector = dd_Sectores.SelectedValue,
                                      e.codproyecto,
                                      razonsocial = e.razonsocial,
                                      e.CodSubSector
                                  });
                    
                    gv_sectores_encontrados.DataSource = result;
                }
                else {                    
                    gv_sectores_encontrados.DataSource = eempresa;
                }                    
            }

            gv_sectores_encontrados.DataBind();
        }

        protected void dd_Sectores_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnl_seleccione_sector.Visible = false;
            ObtenerCodSubSector();
            //Ocultar los demás campos que podrían estar visibles.
            gv_detalles_interventor.Visible = false;
            gv_interventoresCreados.Visible = false; //Grilla de interventores creados.
        }

        protected void gv_sectores_encontrados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar")
            {
                string palabras = e.CommandArgument.ToString();
                string[] arr_palabras = palabras.Split(';');

                pnl_info.Visible = false;
                lbl_info_detalles_sector.Visible = true;
                gv_detalles_interventor.Visible = true;
                gv_interventoresCreados.Visible = true;

                HttpContext.Current.Session["Cod_Emp_TMP"] = arr_palabras[1];
                CargarInfoEmpresa(arr_palabras[1]);
                HttpContext.Current.Session["codproyecto_rowcommand"] = arr_palabras[1];

                pnl_btn_actualizar.Visible = false;
                btn_Actualizar_Interventores.Visible = false;

                lnkbtn_asignarinterventor.Visible = true;
                gv_SubDetalles_interventores.Visible = false;
            }
        }

        private void CargarInfoEmpresa(String CodProyecto_SectorSeleccionado)
        {
            try
            {
                var eempresa = (from e in consultas.Db.Empresas
								where e.codproyecto == int.Parse(CodProyecto_SectorSeleccionado)
								select new
								{
									e.id_empresa,
									e.razonsocial,
									e.Nit,
									e.ObjetoSocial
								}).FirstOrDefault();

                //Obtener lo siguientes valores y asignarlos a los HiddenFields:
                hdf_IdEmpresa.Value = eempresa.id_empresa.ToString();
                hdf_RazonSocial.Value = eempresa.razonsocial;
                hdf_Nit.Value = eempresa.Nit;
                hdf_ObjSocial.Value = eempresa.ObjetoSocial;

                lbl_info_detalles_sector.Text = "<strong>Empresa para el Plan de Negocio: </strong><br />" +
                                                "<span class='antetitulo'>Razón Social - " + hdf_RazonSocial.Value + "</span><br />" +
                                                "<span class='antetitulo'>Nit - " + hdf_Nit.Value + "</span><br />" +
                                                "<span class='antetitulo'>Objeto Social - " + hdf_ObjSocial.Value + "</span><br/>" +
                                                "<span class='antetitulo'>CIUU - " + dd_Sectores.SelectedItem.Text + "<br/><br/>";

                //Reiniciar la variable.
                string sqlConsulta = "";
                //Segunda consulta.
                sqlConsulta = " SELECT Contacto.Id_Contacto, Contacto.Nombres+ ' ' +  Contacto.Apellidos AS Nombres, Contacto.Email " +
                              " FROM dbo.Contacto INNER JOIN EmpresaInterventor " +
                              " ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto INNER JOIN Empresa " +
                              " ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa" +
                              " WHERE (Empresa.codproyecto = " + CodProyecto_SectorSeleccionado + ")" +
                              " AND (EmpresaInterventor.Rol = 8)" +
                              " AND (EmpresaInterventor.Inactivo = 0)";
							  
                //Asignar resultados a variable DataTable.
                var sql_2 = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Bindear la grilla.
                gv_detalles_interventor.DataSource = sql_2;
                gv_detalles_interventor.DataBind();

                //Llamada al evento para bindear la grilla "este método muestra o no la grilla dependiendo de si tiene datos".
                CargarInterventoresCreados(CodProyecto_SectorSeleccionado);
            }
            catch { string err = ""; }
        }

        
        private void CargarContactos_AsignarInterventor()
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                //Consulta.
                sqlConsulta = " SELECT DISTINCT C.Id_Contacto, C.Nombres, C.Apellidos, C.Inactivo " +
                              " FROM Contacto C INNER JOIN InterventorSector PC " +
                              " ON C.Id_Contacto = PC.CodContacto ";

                if (!dd_Sectores.SelectedValue.Equals("[Empty]") && !dd_Sectores.SelectedValue.Equals("[All]"))
                {
                    sqlConsulta = sqlConsulta + " AND PC.CodSector = " + dd_Sectores.SelectedValue;
                }

                sqlConsulta = sqlConsulta + "INNER JOIN Interventor " +
                              " ON C.Id_Contacto = Interventor.CodContacto " +
                              " WHERE (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                              " and C.codOperador = " + usuario.CodOperador +
                              " ORDER BY C.Nombres, C.Apellidos ";

                //Asignar valores a variable DataTable.
                var sql_subGrilla = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Bindear la grilla.
                gv_SubDetalles_interventores.DataSource = sql_subGrilla;
                gv_SubDetalles_interventores.DataBind();

            }
            catch { string err = ""; }
        }

        private void CargarInterventoresCreados(String CodProyecto_SectorSeleccionado)
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                //Consulta.
                sqlConsulta = " SELECT EmpresaInterventor.* FROM EmpresaInterventor " +
                              " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                              " WHERE " +
                              "     ( " +
                              "         EmpresaInterventor.CodContacto IN " +
                              "         ( " +
                              "         SELECT id_contacto FROM Contacto C " +
                              "         INNER JOIN InterventorSector PC ON C.Id_Contacto = PC.CodContacto " +
                              "         ) " +
                              "     ) " +
                              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventor + ") " +
                              " AND (EmpresaInterventor.Inactivo = 0) " +
                              " AND (Empresa.codproyecto = " + CodProyecto_SectorSeleccionado + ")";

                var tabla_conteo = consultas.ObtenerDataTable(sqlConsulta, "text");


                //Generar consulta para la segunda grilla.
                sqlConsulta = ""; //Se limpia la variable "por si algo"...
                sqlConsulta = " SELECT Contacto.Id_Contacto, Contacto.Nombres + ' ' + Contacto.Apellidos AS DT_FULLNAME, Contacto.Email " +
                              " FROM dbo.Contacto INNER JOIN EmpresaInterventor " +
                              " ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto INNER JOIN Empresa " +
                              " ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                              " WHERE (Empresa.codproyecto = " + CodProyecto_SectorSeleccionado + ") " +
                              " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventor + ") " +
                              " AND (EmpresaInterventor.Inactivo = 0)";

                //Asignar los resultados de la consulta anterior a variable DataTable que será 
                //usada para bindear la grilla.
                var tabla_result = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Bindear la grilla.
                gv_interventoresCreados.Visible = true;
                gv_interventoresCreados.DataSource = tabla_result;
                gv_interventoresCreados.DataBind();
            }
            catch { }
        }

        protected void gv_SubDetalles_interventores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ///Para ejecutar esto, la grilla "gv_SubDetalles_interventores" DEBE TENER DATOS, 
            ///si tiene, debe tener el valor "Inactivo = FALSE / 0", se chequea el CheckBox ...
            ///Ver si también para el RadioButton...

            //Establecer primero el Check... = LISTO!
            //Establecer para el RadioButton.... = LISTO!

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Inicializar variables.
                String consulta_string = "";
                DataTable result_consulta_string = new DataTable();
                var check = e.Row.FindControl("chk_objeto") as CheckBox;
                var radio = e.Row.FindControl("rb_interv_lider") as RadioButton;
                var cntct = e.Row.FindControl("hdf_contacto") as HiddenField;
                var inact = e.Row.FindControl("hdf_inactivo_inter") as HiddenField;
                string cod_proyecto_session = HttpContext.Current.Session["codproyecto_rowcommand"].ToString();

                if (check != null && inact != null && cntct != null && radio != null)
                {
                    if (inact.Value == "False" || inact.Value == "0")
                    {
                        consulta_string = "";
                        consulta_string = " SELECT EmpresaInterventor.CodContacto FROM EmpresaInterventor" +
                                          " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa" +
                                          " WHERE (EmpresaInterventor.CodContacto = " + cntct.Value + ")" +
                                          " AND (EmpresaInterventor.Rol IN (" + Constantes.CONST_RolInterventor + "," + Constantes.CONST_RolInterventorLider + "))" +
                                          " AND (EmpresaInterventor.Inactivo = 0)" +
                                          " AND (Empresa.codproyecto = " + cod_proyecto_session + ")";

                        result_consulta_string = consultas.ObtenerDataTable(consulta_string, "text");

                        if (result_consulta_string.Rows.Count > 0)
                        { check.Checked = true; }

                        result_consulta_string = null;
                        consulta_string = null;

                        consulta_string = "";
                        consulta_string = " SELECT EmpresaInterventor.CodContacto " +
                                          " FROM EmpresaInterventor INNER JOIN Empresa " +
                                          " ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                          " WHERE (EmpresaInterventor.CodContacto = " + cntct.Value + ") " +
                                          " AND (EmpresaInterventor.Rol IN (" + Constantes.CONST_RolInterventorLider + ")) " +
                                          " AND (EmpresaInterventor.Inactivo = 0) " +
                                          " AND (Empresa.codproyecto = " + cod_proyecto_session + ") ";

                        result_consulta_string = consultas.ObtenerDataTable(consulta_string, "text");

                        if (result_consulta_string.Rows.Count > 0)
                        { radio.Checked = true; }

                        result_consulta_string = null;
                        consulta_string = null;
                    }
                }
            }
        }

        protected void lnkbtn_asignarinterventor_Click(object sender, EventArgs e)
        {
            CargarContactos_AsignarInterventor();
            gv_detalles_interventor.Visible = false;
            lnkbtn_asignarinterventor.Visible = false;
            gv_SubDetalles_interventores.Visible = true;
            gv_interventoresCreados.Visible = false; //Grilla de interventores creados.
            pnl_btn_actualizar.Visible = true;
            btn_Actualizar_Interventores.Visible = true;
        }

        protected void gv_SubDetalles_interventores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar_acreditador")
            {
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del producto seleccionado en sesión.
                HttpContext.Current.Session["IdAcreditador_Session"] = valores_command[0];
                HttpContext.Current.Session["NombreAcreditador_Session"] = valores_command[1];

                String sqlConsulta = "";

                //Consultar el CodGrupo del contacto seleccionado.
                sqlConsulta = "SELECT CodGrupo FROM GrupoContacto WHERE CodContacto = " + valores_command[0].ToString();

                //Asignar resultados de la consulta.
                var t = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Obtener el CodGrupo
                HttpContext.Current.Session["CodRol_ActaAcreditacion"] = t.Rows[0]["CodGrupo"].ToString();

                Redirect(null, "../MiPerfil/VerPerfilContacto.aspx", "_blank",
                    "menubar=0,scrollbars=1,width=710,height=430,top=100");
            }
        }

        protected void btn_Actualizar_Interventores_Click(object sender, EventArgs e)
        {
            string txtSQL = "UPDATE EmpresaInterventor SET Fechafin=getDate(), Inactivo=1 WHERE CodEmpresa=" + hdf_IdEmpresa.Value + " AND Rol IN(" + Constantes.CONST_RolInterventor + "," + Constantes.CONST_RolInterventorLider + ")";

            consultas.ObtenerDataTable(txtSQL, "text");

            foreach (GridViewRow fila in gv_SubDetalles_interventores.Rows)
            {
                CheckBox check = (CheckBox)fila.FindControl("chk_objeto");
                RadioButton radio = (RadioButton)fila.FindControl("rb_interv_lider");
                int idContacto_obtenido = Convert.ToInt32(gv_SubDetalles_interventores.DataKeys[fila.RowIndex].Value.ToString());

                if (check.Checked)
                {
                    txtSQL = "INSERT INTO EmpresaInterventor (codempresa,codcontacto,rol, Fechainicio)" +
                                         "VALUES (" + hdf_IdEmpresa.Value + ", " + idContacto_obtenido + ", " + Constantes.CONST_RolInterventor + ",getDate())";

                    consultas.ObtenerDataTable(txtSQL, "text");
                }

                if (radio.Checked)
                {
                    txtSQL = "SELECT * FROM EmpresaInterventor WHERE Inactivo = 0 AND CodContacto= " + idContacto_obtenido + " AND Rol = " + Constantes.CONST_RolInterventor + " and CodEmpresa=" + hdf_IdEmpresa.Value;

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                    {
                        txtSQL = "UPDATE EmpresaInterventor SET CodContacto=" + idContacto_obtenido + ", Rol= " + Constantes.CONST_RolInterventorLider +
                                 "WHERE CodEmpresa=" + dt.Rows[0]["codempresa"] + " AND CodContacto=" + idContacto_obtenido + " AND Rol= " + Constantes.CONST_RolInterventor;

                        consultas.ObtenerDataTable(txtSQL, "text");
                    }
                    else
                    {
                        txtSQL = "UPDATE EmpresaInterventor SET Fechafin=  getDate(), Inactivo=1 WHERE CodEmpresa=" + hdf_IdEmpresa.Value + " AND Rol=" + Constantes.CONST_RolInterventorLider;
                        consultas.ObtenerDataTable(txtSQL, "text");

                        txtSQL = "INSERT INTO EmpresaInterventor (codempresa,codcontacto,rol,Fechainicio)" +
                                 "VALUES (" + hdf_IdEmpresa.Value + ", " + idContacto_obtenido + ", " + Constantes.CONST_RolInterventorLider + ",getDate())";
                        consultas.ObtenerDataTable(txtSQL, "text");
                    }
                }
            }

            //var result = (from ei in consultas.Db.EmpresaInterventors
            //              where ei.CodEmpresa == int.Parse(hdf_IdEmpresa.Value)
            //              && (ei.Rol == Constantes.CONST_RolInterventor || ei.Rol == Constantes.CONST_RolInterventorLider)
            //              select ei);

            //foreach(var temp in result)
            //{
            //    temp.Inactivo = true;
            //    temp.FechaFin = DateTime.Now;

            //    try
            //    {
            //        consultas.Db.SubmitChanges();
            //    }
            //    catch (Exception ex) { }
            //}

            //result = null;

            ////List<EmpresaInterventor> listaEmp = new List<EmpresaInterventor>();

            //foreach (GridViewRow fila in gv_SubDetalles_interventores.Rows)
            //{
            //    CheckBox check = (CheckBox)fila.FindControl("chk_objeto");
            //    RadioButton radio = (RadioButton)fila.FindControl("rb_interv_lider");
            //    int idContacto_obtenido = Convert.ToInt32(gv_SubDetalles_interventores.DataKeys[fila.RowIndex].Value.ToString());

            //    if (check.Checked)
            //    {
            //        var emp = (from ei in consultas.Db.EmpresaInterventors
            //                   where ei.CodContacto == idContacto_obtenido
            //                   && ei.CodEmpresa == int.Parse(hdf_IdEmpresa.Value)
            //                   select ei).FirstOrDefault();

            //        if (emp == null)
            //        {
            //            var emp1 = new EmpresaInterventor();
            //            emp1.CodEmpresa = int.Parse(hdf_IdEmpresa.Value);
            //            emp1.CodContacto = idContacto_obtenido;
            //            emp1.Rol = Constantes.CONST_RolInterventor;
            //            emp1.FechaInicio = DateTime.Now;

            //            //listaEmp.Add(emp1);

            //            try
            //            {
            //                consultas.Db.EmpresaInterventors.InsertOnSubmit(emp1);
            //                emp1 = null;
            //                consultas.Db.SubmitChanges();
            //            }
            //            catch (Exception ex) { }
            //        }
            //        else
            //        {
            //            emp.CodEmpresa = int.Parse(hdf_IdEmpresa.Value);
            //            emp.CodContacto = idContacto_obtenido;
            //            emp.Rol = Constantes.CONST_RolInterventor;
            //            emp.FechaInicio = DateTime.Now;
            //            emp.Inactivo = false;
            //            emp.FechaFin = null;

            //            try
            //            {
            //                consultas.Db.SubmitChanges();
            //                emp = null;
            //            }
            //            catch (Exception ex) { }
            //        }
            //    }

            //    if (radio.Checked)
            //    {
            //        var emp = (from ei in consultas.Db.EmpresaInterventors
            //                   where ei.Inactivo == false
            //                   && ei.CodContacto == idContacto_obtenido
            //                   && ei.Rol == Constantes.CONST_RolInterventor
            //                   && ei.CodEmpresa == int.Parse(hdf_IdEmpresa.Value)
            //                   select ei).FirstOrDefault();

            //        if (emp != null)
            //        {
            //            emp = (from ei in consultas.Db.EmpresaInterventors
            //                   where ei.CodContacto == idContacto_obtenido
            //                   && ei.Rol == Constantes.CONST_RolInterventor
            //                   && ei.CodEmpresa == int.Parse(hdf_IdEmpresa.Value)
            //                   select ei).FirstOrDefault();

            //            emp.CodContacto = idContacto_obtenido;
            //            emp.Rol = Constantes.CONST_RolInterventorLider;

            //            try
            //            {
            //                consultas.Db.SubmitChanges();
            //                emp = null;
            //            }
            //            catch (Exception) { }
            //        }
            //        else
            //        {
            //            emp = (from ei in consultas.Db.EmpresaInterventors
            //                   where ei.Rol == Constantes.CONST_RolInterventorLider
            //                   && ei.CodEmpresa == int.Parse(hdf_IdEmpresa.Value)
            //                   select ei).FirstOrDefault();

            //            emp.Inactivo = true;
            //            emp.FechaFin = DateTime.Now;

            //            try
            //            {
            //                consultas.Db.SubmitChanges();
            //                emp = null;
            //            }
            //            catch (Exception) { }

            //            var emp1 = new EmpresaInterventor();
            //            emp1.CodEmpresa = int.Parse(hdf_IdEmpresa.Value);
            //            emp1.CodContacto = idContacto_obtenido;
            //            emp1.Rol = Constantes.CONST_RolInterventorLider;
            //            emp1.FechaInicio = DateTime.Now;

            //            //listaEmp.ForEach(x =>{
            //            //    if(x.CodContacto == idContacto_obtenido)
            //            //    {
            //            //        x.Rol = Constantes.CONST_RolInterventorLider;
            //            //    }
            //            //});

            //            try
            //            {
            //                consultas.Db.EmpresaInterventors.InsertOnSubmit(emp1);
            //                emp1 = null;
            //                consultas.Db.SubmitChanges();
            //            }
            //            catch (Exception ex) { }
            //        }
            //    }
            //}


            //listaEmp.ForEach(x =>
            //{
            //    try
            //    {
            //        consultas.Db.EmpresaInterventors.InsertOnSubmit(x);
            //        consultas.Db.SubmitChanges();
            //    }
            //    catch (Exception ex) { }
            //});

           

            ////Inicializar variables.
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //SqlCommand cmd = new SqlCommand();
            //String sqlConsulta = "";
            //string correcto;
            //DateTime Fechainicio = DateTime.Today;

            //try
            //{
            //    //Obtener lo siguientes valores y asignarlos a los HiddenFields:
            //    if (hdf_IdEmpresa.Value.Trim() == "")
            //    { return; }

            //    #region Comentarios.
            //    //NOTA: Se modifica lo anterior.
            //    //sqlConsulta = " UPDATE EmpresaInterventor SET Fechafin = '" + DateTime.Today + "', Inactivo = 1 " +
            //    //              " WHERE CodEmpresa = " + hdf_IdEmpresa.Value +
            //    //              " AND Rol IN (" + Constantes.CONST_RolInterventor + "," + Constantes.CONST_RolInterventorLider + ")";

            //    ////Asignar SqlCommand para su ejecución.
            //    //cmd = new SqlCommand(sqlConsulta, conn);

            //    ////Ejecutar SQL.
            //    //correcto = String_EjecutarSQL(conn, cmd);

            //    //if (correcto != "")
            //    //{
            //    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (1).')", true);
            //    //    return;
            //    //} 
            //    #endregion

            //    try
            //    {
            //        //NEW RESULTS:
            //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            //        cmd = new SqlCommand("MD_Update_EmpresaInterventor", con);

            //        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@Fechafin", DateTime.Today);
            //        cmd.Parameters.AddWithValue("@CodEmpresa", hdf_IdEmpresa.Value);
            //        cmd.Parameters.AddWithValue("@CONST_RolInterventor", Constantes.CONST_RolInterventor);
            //        cmd.Parameters.AddWithValue("@CONST_RolInterventorLider", Constantes.CONST_RolInterventorLider);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //        con.Dispose();
            //        cmd.Dispose();
            //    }
            //    catch { }

            //    //Recorrer la grilla para revisar por cada fila los valores a procesar.
            //    foreach (GridViewRow fila in gv_SubDetalles_interventores.Rows)
            //    {
            //        CheckBox check = (CheckBox)fila.FindControl("chk_objeto");
            //        RadioButton radio = (RadioButton)fila.FindControl("rb_interv_lider");
            //        int idContacto_obtenido = Convert.ToInt32(gv_SubDetalles_interventores.DataKeys[fila.RowIndex].Value.ToString());
            //        bool FueChequeado = false;

            //        if (check.Checked == true)
            //        {
            //            #region Inserta los interventores, incluido el lider, aunque primero lo inserta como Interventor.

            //            #region COMENTARIOS!
            //            //sqlConsulta = " INSERT INTO EmpresaInterventor (codempresa, codcontacto, rol, Fechainicio) " +
            //            //              " VALUES (" + hdf_IdEmpresa.Value + ", " + idContacto_obtenido + ", " + Constantes.CONST_RolInterventor + ",'" + Fechainicio + "')";

            //            ////Asignar SqlCommand para su ejecución.
            //            //cmd = new SqlCommand(sqlConsulta, conn);

            //            ////Ejecutar SQL.
            //            //correcto = String_EjecutarSQL(conn, cmd);

            //            //if (correcto != "")
            //            //{
            //            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de interventor (1).')", true);
            //            //    return;
            //            //} 
            //            #endregion

            //            #region Ajustado.
            //            try
            //            {
            //                //NEW RESULTS:
            //                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            //                cmd = new SqlCommand("MD_Update_EmpresaInterventor", con);

            //                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.AddWithValue("@Fechafin", DateTime.Today);
            //                cmd.Parameters.AddWithValue("@CodEmpresa", hdf_IdEmpresa.Value);
            //                cmd.Parameters.AddWithValue("@CONST_RolInterventor", Constantes.CONST_RolInterventor);
            //                cmd.Parameters.AddWithValue("@CONST_RolInterventorLider", Constantes.CONST_RolInterventorLider);
            //                cmd.ExecuteNonQuery();
            //                con.Close();
            //                con.Dispose();
            //                cmd.Dispose();
            //            }
            //            catch { }
            //            #endregion

            //            #endregion
            //        }
            //        if (radio.Checked == true && FueChequeado == false)
            //        {
            //            //Consulta. "En lugar del asterisco (*), se usa el campo que se requiere".
            //            sqlConsulta = " SELECT CodEmpresa FROM EmpresaInterventor WHERE Inactivo = 0 " +
            //                          " AND CodContacto = " + idContacto_obtenido +
            //                          " AND Rol = " + Constantes.CONST_RolInterventor +
            //                          " AND CodEmpresa = " + hdf_IdEmpresa.Value;

            //            //Asignar resultados de la consulta.
            //            var sql_CodEmpresa = consultas.ObtenerDataTable(sqlConsulta, "text");

            //            //Si tiene datos, actualiza, si no, actualiza otro dato e inserta.
            //            if (sql_CodEmpresa.Rows.Count > 0)
            //            {
            //                #region Actualiza el código del Interventor Líder.

            //                //Hago recorrido de datos "paar evitar que de pronto se ignoren datos.
            //                for (int i = 0; i < sql_CodEmpresa.Rows.Count; i++)
            //                {

            //                    #region Actualización del interventor líder (en "EmpresaInterventor").
            //                    sqlConsulta = " UPDATE EmpresaInterventor SET CodContacto = " + idContacto_obtenido + ", " +
            //                                                          " Rol = " + Constantes.CONST_RolInterventorLider +
            //                                                          " WHERE CodEmpresa = " + sql_CodEmpresa.Rows[i]["CodEmpresa"].ToString() +
            //                                                          " AND CodContacto=" + idContacto_obtenido +
            //                                                          " AND Rol= " + Constantes.CONST_RolInterventor;

            //                    //Asignar SqlCommand para su ejecución.
            //                    cmd = new SqlCommand(sqlConsulta, conn);

            //                    //Ejecutar SQL.
            //                    correcto = String_EjecutarSQL(conn, cmd);

            //                    if (correcto != "")
            //                    {
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización de interventor (" + i + ").')", true);
            //                        return;
            //                    }
            //                    #endregion
            //                }

            //                #endregion
            //            }
            //            else
            //            {
            //                #region Inserta un nuevo Interventor Líder a EmpresaInterventor. (Consulta UPDATE)

            //                #region COMENTARIOS!.
            //                //Actualización.
            //                sqlConsulta = "INSERT INTO EmpresaInterventor(CodEmpresa,CodContacto,Rol,FechaInicio,FechaFin,Inactivo)VALUES" +
            //                        "( " + hdf_IdEmpresa.Value +
            //                        "," + idContacto_obtenido +
            //                        "," + Constantes.CONST_RolInterventor +
            //                        ",getDate(),NULL,0)";

            //                //Asignar SqlCommand para su ejecución.
            //                cmd = new SqlCommand(sqlConsulta, conn);

            //                //Ejecutar SQL.
            //                correcto = String_EjecutarSQL(conn, cmd);

            //                if (correcto != "")
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al insertar un nuevo interventor líder (1).')", true);
            //                    return;
            //                } 
            //                #endregion

            //                #region Ajustado.
            //                try
            //                {
            //                    //NEW RESULTS:
            //                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            //                    cmd = new SqlCommand("MD_Update_EmpresaInterventor", con);

            //                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.Parameters.AddWithValue("@Fechafin", DateTime.Today);
            //                    cmd.Parameters.AddWithValue("@CodEmpresa", hdf_IdEmpresa.Value);
            //                    cmd.Parameters.AddWithValue("@CONST_RolInterventor", Constantes.CONST_RolInterventor);
            //                    cmd.Parameters.AddWithValue("@CONST_RolInterventorLider", Constantes.CONST_RolInterventorLider);
            //                    cmd.ExecuteNonQuery();
            //                    con.Close();
            //                    con.Dispose();
            //                    cmd.Dispose();
            //                }
            //                catch { }
            //                #endregion

            //                #endregion

            //                #region Inserción en EmpresaInterventor. (Consulta INSERT)

            //                #region COMENTARIOS!
            //                ////Actualización.
            //                //sqlConsulta = " INSERT INTO EmpresaInterventor (codempresa,codcontacto,rol,Fechainicio) " +
            //                //              " VALUES (" + hdf_IdEmpresa.Value + ", " + idContacto_obtenido + ", " + Constantes.CONST_RolInterventorLider + ",'" + Fechainicio + "')";

            //                ////Asignar SqlCommand para su ejecución.
            //                //cmd = new SqlCommand(sqlConsulta, conn);

            //                ////Ejecutar SQL.
            //                //correcto = String_EjecutarSQL(conn, cmd);

            //                //if (correcto != "")
            //                //{
            //                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al insertar un nuevo interventor líder (2).')", true);
            //                //    return;
            //                //} 
            //                #endregion

            //                #region Ajustado.
            //                try
            //                {
            //                    //NEW RESULTS:
            //                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            //                    cmd = new SqlCommand("MD_Insertar_New_EmpresaInterventor", con);

            //                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.Parameters.AddWithValue("@CodEmpresa", hdf_IdEmpresa.Value);
            //                    cmd.Parameters.AddWithValue("@CodContacto", idContacto_obtenido);
            //                    cmd.Parameters.AddWithValue("@Rol", Constantes.CONST_RolInterventorLider);
            //                    cmd.Parameters.AddWithValue("@FechaInicio", Fechainicio);
            //                    cmd.ExecuteNonQuery();
            //                    con.Close();
            //                    con.Dispose();
            //                    cmd.Dispose();
            //                }
            //                catch { }
            //                #endregion

            //                #endregion
            //            }

            //            //Determina que sólo un registro puede ser interventor líder.
            //            FueChequeado = true;
            //        }
            //    }

                ObtenerCodSubSector();
                pnl_info.Visible = true;
                lbl_info.Visible = true;
                //Se deja la pantalla como al inicio, pero con el DropDownList y resultados de la izquierda visibles.
                lbl_info_detalles_sector.Visible = true;
                CargarInfoEmpresa(HttpContext.Current.Session["Cod_Emp_TMP"].ToString());
                gv_detalles_interventor.Visible = true;
                lnkbtn_asignarinterventor.Visible = true;
                gv_interventoresCreados.Visible = false; //Grilla de interventores creados.
                gv_SubDetalles_interventores.Visible = false;
                btn_Actualizar_Interventores.Visible = false; //Botón oprimido.

            pnl_info.Visible = false;
                lbl_info_detalles_sector.Visible = true;
                gv_detalles_interventor.Visible = true;
                gv_interventoresCreados.Visible = true;

                HttpContext.Current.Session["Cod_Emp_TMP"] = HttpContext.Current.Session["Cod_Emp_TMP"].ToString();
                CargarInfoEmpresa(HttpContext.Current.Session["Cod_Emp_TMP"].ToString());
                HttpContext.Current.Session["codproyecto_rowcommand"] = HttpContext.Current.Session["Cod_Emp_TMP"].ToString();

                pnl_btn_actualizar.Visible = false;
                btn_Actualizar_Interventores.Visible = false;

                lnkbtn_asignarinterventor.Visible = true;
                gv_SubDetalles_interventores.Visible = false;
            //}
            //catch { }
        }
    }
}