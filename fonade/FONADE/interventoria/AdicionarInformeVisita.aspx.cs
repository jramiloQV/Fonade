using Fonade.Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarInformeVisita : Negocio.Base_Page
    {
        #region Variables globales.
        /// <summary>
        /// Id del informe seleccionado "para consultar la información del informe seleccionado.
        /// </summary>
        string idInforme;
        DataTable dt_informe;
        DataTable RSMedio;
        DataTable infoDeta;
        /// <summary>
        /// Código de la empresa seleccionada para generarle el informe.
        /// </summary>
        String CodEmpresa_NuevoInforme;
        /// <summary>
        /// Nombre de la empresa seleccionada para crearle el nuevo informe.
        /// </summary>
        String Nombre_Empresa;
        /// <summary>
        /// Creado y enviado desde "InformeVisitaInter.aspx" para disponer el formulario para crerar nuevos informes.
        /// </summary>
        String EsNuevo;
        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                #region Cargar la fecha actual en los campos correspondientes.
                DateTime fecha_actual = DateTime.Today;
                c_fecha_r.SelectedDate = fecha_actual;
                c_fecha_s.SelectedDate = fecha_actual;
                txtDate.Text = fecha_actual.ToString("dd/MM/yyyy");
                txtDate2.Text = fecha_actual.ToString("dd/MM/yyyy");
                #endregion

                //Consultar el Id y el nombre de la empresa seleccionada para generar un nuevo reporte.
                CodEmpresa_NuevoInforme = HttpContext.Current.Session["InformeIdVisita"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["InformeIdVisita"].ToString()) ? HttpContext.Current.Session["InformeIdVisita"].ToString() : "0";
                Nombre_Empresa = HttpContext.Current.Session["Nombre_Empresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Nombre_Empresa"].ToString()) ? HttpContext.Current.Session["Nombre_Empresa"].ToString() : "";
                EsNuevo = HttpContext.Current.Session["Nuevo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Nuevo"].ToString()) ? HttpContext.Current.Session["Nuevo"].ToString() : "False";

                //Determinar si se crea o se actualiza el informe.
                if (EsNuevo != "False")
                {
                    #region Crear nuevo informe.

                    //Cargar los DropDownLists.
                    lenarTabla(true);

                    //Asignar valores.
                    txtempresa.Text = Nombre_Empresa;

                    //Crear nuevo informe "Cargar el nombre de la empresa seleccionada y valores determinados.
                    btn_creaar.Text = "Ingresar Informe";
                    btn_eliminar.Visible = false;
                    #endregion
                }
                else
                {
                    #region Consultar informe seleccionado.
                    //Se ha seleccionado un informe para consultarle su información.
                    idInforme = HttpContext.Current.Session["InformeIdVisita"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["InformeIdVisita"].ToString()) ? HttpContext.Current.Session["InformeIdVisita"].ToString() : "0";

                    //Si no tiene datos, tuvo que haber un error.
                    if (idInforme == "0")
                    { Response.Redirect("InformeVisitaInter.aspx"); } //return;
                    else
                    {
                        //Consultar la información:
                        DatosEntrada(Convert.ToInt32(idInforme));
                        infoPlan(Convert.ToInt32(idInforme));
                        lenarTabla(false);
                    }
                    #endregion
                }
            }

            //Atributos agregados a controles.
            medio1.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            medio2.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            medio3.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            medio4.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");

            #region Comentarios.
            //if (!IsPostBack)
            //{
            //    DatosEntrada();

            //    infoPlan();

            //    lenarTabla();
            //}

            //medio1.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            //medio2.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            //medio3.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            //medio4.Attributes.Add("onkeypress", "javascript: return ValidNum(event);"); 
            #endregion
        }

        #region Métodos de consulta de informe seleccionado.

        /// <summary>
        /// Ejecutar consultas, asignando valores a variables "dt_informe" y "RSMedio".
        /// </summary>
        /// <param name="id_informe">Id del informe seleccionado.</param>
        private void DatosEntrada(Int32 id_informe)
        {
            string txtSQL = "Select * from InformeVisitaInterventoria i, Empresa e where i.CodEmpresa = e.Id_Empresa and id_informe = " + id_informe;
            dt_informe = consultas.ObtenerDataTable(txtSQL, "text");
            //txtSQL = "Select Id_MedioDeTransporte, Valor from InformeMedioTransporte, MedioDeTransporte Where Id_MedioDeTransporte = CodMedioTransporte and CodInforme = " + id_informe;
            // Se ajusta la consulta para que los datos obtenidos se trabajen en orden
            // Alex Flautero - Noviembre 19 de 2014
            txtSQL = "Select b.Id_MedioDeTransporte, a.Valor from InformeMedioTransporte a inner join MedioDeTransporte b on a.CodMedioTransporte = b.Id_MedioDeTransporte ";
            txtSQL = txtSQL + " Where a.CodInforme = "+ id_informe.ToString();
            txtSQL = txtSQL + " order by b.Id_MedioDeTransporte asc";
            RSMedio = consultas.ObtenerDataTable(txtSQL, "text");
        }

        /// <summary>
        /// Cargar la información del detalle seleccionado, asignando el resultado de 
        /// la consulta a la variable "infoDeta".
        /// </summary>
        /// <param name="id_informe">Id del informe seleccionado.</param>
        private DataTable infoPlan(Int32 id_informe)
        {
            consultas.Parameters = new[] {
                new SqlParameter { 
                    ParameterName = "@CodGrupo",
                    Value = usuario.CodGrupo
                },
                new SqlParameter { 
                    ParameterName = "@CodUsuario",
                    Value = usuario.IdContacto
                },
                new SqlParameter { 
                    ParameterName = "@CodInforme",
                    Value = id_informe
                }
            };

            return infoDeta = consultas.ObtenerDataTable("MD_ReporteEmpresa");
        }

        /// <summary>
        /// Llenar tabla...
        /// </summary>
        /// <param name="esNuevo">TRUE = Sólo cargar los DropDownLists. // FALSE = Cargar todo, está consultando un informe.</param>
        private void lenarTabla(bool esNuevo)
        {
            if (esNuevo == true)
            {
                #region Cargar DropDownLists.

                var departamentos = from d in consultas.Db.departamento

                                    orderby d.NomDepartamento ascending
                                    select new
                                    {
                                        Id_Departamento = d.Id_Departamento,
                                        NomDepartamento = d.NomDepartamento
                                    };

                ddl_dedorigen.DataSource = departamentos;
                ddl_dedorigen.DataTextField = "NomDepartamento";
                ddl_dedorigen.DataValueField = "Id_Departamento";
                ddl_dedorigen.DataBind();

                ddl_deddestino.DataSource = departamentos;
                ddl_deddestino.DataTextField = "NomDepartamento";
                ddl_deddestino.DataValueField = "Id_Departamento";
                ddl_deddestino.DataBind();

                #endregion
            }
            else
            {
                #region Consultar la información del informe seleccionado.
                Double valor = 0;
                infoDeta = infoPlan(Int32.Parse(idInforme)); //Consultar la información.
                txtinforme.Text = dt_informe.Rows[0]["NombreInforme"].ToString();
                txtempresa.Text = dt_informe.Rows[0]["razonsocial"].ToString();

                if (dt_informe.Rows[0]["Estado"].ToString().Equals("1"))
                    txtinforme.Enabled = false;

                if (infoDeta.Rows.Count > 0)               
                    lblnit.Text = infoDeta.Rows[0]["nit"].ToString();
                
                var departamentos = from d in consultas.Db.departamento

                                    orderby d.NomDepartamento ascending
                                    select new
                                    {
                                        Id_Departamento = d.Id_Departamento,
                                        NomDepartamento = d.NomDepartamento
                                    };

                ddl_dedorigen.DataSource = departamentos;
                ddl_dedorigen.DataTextField = "NomDepartamento";
                ddl_dedorigen.DataValueField = "Id_Departamento";
                ddl_dedorigen.DataBind();

                ddl_deddestino.DataSource = departamentos;
                ddl_deddestino.DataTextField = "NomDepartamento";
                ddl_deddestino.DataValueField = "Id_Departamento";
                ddl_deddestino.DataBind();

                string txtSQL = "Select c1.CodDepartamento as dpto_o, c2.CodDepartamento as dpto_d from Ciudad c1, Ciudad c2 Where c1.Id_Ciudad = " + dt_informe.Rows[0]["CodCiudadOrigen"].ToString() + " and c2.Id_Ciudad = " + dt_informe.Rows[0]["CodCiudadDestino"].ToString();

                var dataDep = consultas.ObtenerDataTable(txtSQL, "text");

                try
                {
                    ddl_dedorigen.SelectedValue = dataDep.Rows[0]["dpto_o"].ToString();
                    ddl_dedorigenllenar(Int32.Parse(dataDep.Rows[0]["dpto_o"].ToString()));
                    ddl_ciuorigen.SelectedValue = dt_informe.Rows[0]["CodCiudadOrigen"].ToString();

                    ddl_deddestino.SelectedValue = dataDep.Rows[0]["dpto_d"].ToString();
                    ddl_deddestinollenar(Int32.Parse(dataDep.Rows[0]["dpto_d"].ToString()));
                    ddl_ciudestino.SelectedValue = dt_informe.Rows[0]["CodCiudadDestino"].ToString();
                }
                catch (FormatException) { }

                for (int i = 0; i < RSMedio.Rows.Count; i++)
                {
                    switch (Convert.ToInt32(RSMedio.Rows[i]["Id_MedioDeTransporte"].ToString()))
                    {
                        #region Formateados los valores por Mauricio Arias Olave.
                        case 1:
                            medio1.Text = RSMedio.Rows[i]["Valor"].ToString();
                            try { valor = Convert.ToDouble(medio1.Text); medio1.Text = valor.ToString(); }
                            catch { medio1.Text = RSMedio.Rows[i]["Valor"].ToString(); }
                            if (dt_informe.Rows[0]["Estado"].ToString().Equals("1")) medio1.Enabled = false;
                            break;
                        case 2:
                            medio2.Text = RSMedio.Rows[i]["Valor"].ToString();
                            try { valor = Convert.ToDouble(medio2.Text); medio2.Text = valor.ToString(); }
                            catch { medio2.Text = RSMedio.Rows[i]["Valor"].ToString(); }
                            if (dt_informe.Rows[0]["Estado"].ToString().Equals("1")) medio2.Enabled = false;
                            break;
                        case 3:
                            medio3.Text = RSMedio.Rows[i]["Valor"].ToString();
                            try { valor = Convert.ToDouble(medio3.Text); medio3.Text = valor.ToString(); }
                            catch { medio3.Text = RSMedio.Rows[i]["Valor"].ToString(); }
                            if (dt_informe.Rows[0]["Estado"].ToString().Equals("1")) medio3.Enabled = false;
                            break;
                        case 4:
                            medio4.Text = RSMedio.Rows[i]["Valor"].ToString();
                            try { valor = Convert.ToDouble(medio4.Text); medio4.Text = valor.ToString(); }
                            catch { medio4.Text = RSMedio.Rows[i]["Valor"].ToString(); }
                            if (dt_informe.Rows[0]["Estado"].ToString().Equals("1")) medio4.Enabled = false;
                            break;
                        #endregion
                    }
                }

                c_fecha_s.SelectedDate = DateTime.Parse(dt_informe.Rows[0]["FechaSalida"].ToString()); c_fecha_s.DataBind();
                c_fecha_r.SelectedDate = DateTime.Parse(dt_informe.Rows[0]["FechaRegreso"].ToString()); c_fecha_r.DataBind();

                txtDate.Text = dt_informe.Rows[0]["FechaSalida"].ToString();
                txtDate2.Text = dt_informe.Rows[0]["FechaRegreso"].ToString();

                tb_info_tecnica.Text = dt_informe.Rows[0]["InformacionTecnica"].ToString();
                tb_info_financiera.Text = dt_informe.Rows[0]["InformacionFinanciera"].ToString();

                if (!string.IsNullOrEmpty(idInforme))
                {
                    btn_creaar.Text = "Actualizar";
                    btn_eliminar.Text = "Borrar";
                }
                else
                {
                    btn_creaar.Text = "Ingresar Informe";
                }
                #endregion
            }
        }

        #endregion

        #region Métodos generales.

        protected void ddl_dedorigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_dedorigenllenar(Int32.Parse(ddl_dedorigen.SelectedValue));
        }

        private void ddl_dedorigenllenar(int id)
        {
            try
            {
                if (ddl_dedorigen.SelectedValue != "9999999999")
                {
                    var municipios = (from c in consultas.Db.Ciudad
                                      where c.CodDepartamento == id
                                      orderby c.NomCiudad ascending
                                      select new
                                      {
                                          NomCiudad = c.NomCiudad,
                                          Id_Ciudad = c.Id_Ciudad
                                      });
                    ddl_ciuorigen.DataSource = municipios;
                    ddl_ciuorigen.DataTextField = "NomCiudad";
                    ddl_ciuorigen.DataValueField = "ID_Ciudad";
                    ddl_ciuorigen.DataBind();
                }
            }
            catch (FormatException) { }
            catch (ArgumentOutOfRangeException) { }
            catch (SqlException) { }
        }

        protected void ddl_deddestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_deddestinollenar(Int32.Parse(ddl_deddestino.SelectedValue));
        }

        private void ddl_deddestinollenar(int id)
        {
            try
            {
                if (ddl_deddestino.SelectedValue != "9999999999")
                {
                    var municipios = (from c in consultas.Db.Ciudad
                                      where c.CodDepartamento == id
                                      orderby c.NomCiudad ascending
                                      select new
                                      {
                                          NomCiudad = c.NomCiudad,
                                          Id_Ciudad = c.Id_Ciudad
                                      });
                    ddl_ciudestino.DataSource = municipios;
                    ddl_ciudestino.DataTextField = "NomCiudad";
                    ddl_ciudestino.DataValueField = "ID_Ciudad";
                    ddl_ciudestino.DataBind();
                }
            }
            catch (FormatException) { }
            catch (ArgumentOutOfRangeException) { }
        }

        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (btn_eliminar.Text.Equals("Borrar"))
            {
                idInforme = Session["InformeIdVisita"].ToString();

                var infoVisita = (from iv in consultas.Db.InformeVisitaInterventoria
                                  where iv.Id_Informe == int.Parse(idInforme)
                                  select iv).FirstOrDefault();

                var lstMedios = (from vt in consultas.Db.InformeMedioTransporte
                                 where vt.CodInforme == infoVisita.Id_Informe
                                 select vt).ToList();
                consultas.Db.InformeMedioTransporte.DeleteAllOnSubmit(lstMedios);
                consultas.Db.SubmitChanges();

                consultas.Db.InformeVisitaInterventoria.DeleteOnSubmit(infoVisita);
                consultas.Db.SubmitChanges();

                Response.Redirect("InformeVisitaInter.aspx");
            }
        }

        #endregion

        /// <summary>
        /// Si el texto del botón es "Ingresar Informe", creará un informe nuevo, de lo contrario
        /// actualizaciá o eliminará el informe seleccionado, al final del proceso, redirigirá al 
        /// usuario a la página de informes de visita.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_creaar_Click(object sender, EventArgs e)
        {
            if (btn_creaar.Text == "Ingresar Informe") //Insert
            {
                //Variable temporal que contendrá el resultado devuelto por el método "ValidarSeleccion()".
                string temp_string = ValidarSeleccion();

                //Si la variable contiene datos, significa que NO pasó las validaciones.
                if (temp_string != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + temp_string + "')", true);
                    temp_string = null;
                    return;
                }
                else
                {
                    //De lo contrario, creará el informe de visita.

                    decimal valorViatico;
                    string txtSql;
                    var idEmpresa = Session["InformeIdVisita"].ToString();
                    var FechaSalida = Convert.ToDateTime(txtDate.Text);
                    var FechaRegreso = Convert.ToDateTime(txtDate2.Text);
                    var interventor = (from i in consultas.Db.Interventor1s where i.CodContacto == usuario.IdContacto
                                           select i).FirstOrDefault();

                    txtSql = "SELECT Valor FROM Viatico  WHERE ((SELECT Salario FROM Interventor WHERE (CodContacto = "+ usuario.IdContacto + "))";
                    txtSql += " BETWEEN LimiteInferior AND LimiteSuperior and LimiteSuperior != -1)";

                    var dt = consultas.ObtenerDataTable(txtSql, "text");
                    if(dt.Rows.Count > 0)
                    {
                        var valorvia = dt.Rows[0].ItemArray[0].ToString().Split(',');
                        valorViatico = decimal.Parse(valorvia[0]);
                    }
                    else
                    {
                        txtSql = "SELECT Valor FROM Viatico  WHERE ((SELECT Salario FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + "))";
                        txtSql += " >= LimiteInferior and LimiteSuperior = -1)";
                        var dt2 = consultas.ObtenerDataTable(txtSql, "text");
                        valorViatico = decimal.Parse(dt2.Rows[0].ItemArray[0].ToString());
                    }

                    

                    if (valorViatico > 0)
                    {
                        //var valorViatico = Convert.ToInt32(viaticos.Valor);
                        var informeVisita = new Datos.InformeVisitaInterventoria
                        {
                            NombreInforme = txtinforme.Text.ToUpper(),
                            CodCiudadOrigen = int.Parse(ddl_ciuorigen.SelectedValue),
                            CodCiudadDestino = int.Parse(ddl_ciudestino.SelectedValue),
                            FechaSalida = FechaSalida,
                            FechaRegreso = FechaRegreso,
                            CodEmpresa = int.Parse(idEmpresa),
                            CodInterventor = usuario.IdContacto,
                            InformacionTecnica = tb_info_tecnica.Text,
                            InformacionFinanciera = tb_info_financiera.Text,
                            FechaInforme = DateTime.Now.Date,
                            Estado = false
                        };

                        consultas.Db.InformeVisitaInterventoria.InsertOnSubmit(informeVisita);
                        consultas.Db.SubmitChanges();

                        var diasDiferencia = (informeVisita.FechaRegreso - informeVisita.FechaSalida).Days + 1;

                        var mediosTrasp = (from mt in consultas.Db.MedioDeTransporte
                                           select mt).ToList();

                        var valorTrasnporte = 0;
                        foreach(var medio in mediosTrasp)
                        {
                            switch (medio.Id_MedioDeTransporte)
                            {
                                case 1:
                                    if(!string.IsNullOrEmpty(medio1.Text.Trim()) || medio1.Text.Trim() != "0" )
                                    {
                                        var objInformeMedioTrasnp = new Datos.InformeMedioTransporte();
                                        objInformeMedioTrasnp.CodInforme = informeVisita.Id_Informe;
                                        objInformeMedioTrasnp.CodMedioTransporte = medio.Id_MedioDeTransporte;
                                        objInformeMedioTrasnp.Valor = int.Parse(medio1.Text.Trim());
                                        consultas.Db.InformeMedioTransporte.InsertOnSubmit(objInformeMedioTrasnp);
                                        consultas.Db.SubmitChanges();
                                        valorTrasnporte += int.Parse(medio1.Text.Trim());
                                    }
                                    break;
                                case 2:
                                    if (!string.IsNullOrEmpty(medio2.Text.Trim()) || medio2.Text.Trim() != "0")
                                    {
                                        var objInformeMedioTrasnp = new Datos.InformeMedioTransporte();
                                        objInformeMedioTrasnp.CodInforme = informeVisita.Id_Informe;
                                        objInformeMedioTrasnp.CodMedioTransporte = medio.Id_MedioDeTransporte;
                                        objInformeMedioTrasnp.Valor = int.Parse(medio2.Text.Trim());
                                        consultas.Db.InformeMedioTransporte.InsertOnSubmit(objInformeMedioTrasnp);
                                        consultas.Db.SubmitChanges();
                                        valorTrasnporte += int.Parse(medio2.Text.Trim());
                                    }
                                    break;
                                case 3:
                                    if (!string.IsNullOrEmpty(medio3.Text.Trim()) || medio3.Text.Trim() != "0")
                                    {
                                        var objInformeMedioTrasnp = new Datos.InformeMedioTransporte();
                                        objInformeMedioTrasnp.CodInforme = informeVisita.Id_Informe;
                                        objInformeMedioTrasnp.CodMedioTransporte = medio.Id_MedioDeTransporte;
                                        objInformeMedioTrasnp.Valor = int.Parse(medio3.Text.Trim());
                                        consultas.Db.InformeMedioTransporte.InsertOnSubmit(objInformeMedioTrasnp);
                                        consultas.Db.SubmitChanges();
                                        valorTrasnporte += int.Parse(medio3.Text.Trim());
                                    }
                                    break;
                                case 4:
                                    if (!string.IsNullOrEmpty(medio4.Text.Trim()) || medio4.Text.Trim() != "0")
                                    {
                                        var objInformeMedioTrasnp = new Datos.InformeMedioTransporte();
                                        objInformeMedioTrasnp.CodInforme = informeVisita.Id_Informe;
                                        objInformeMedioTrasnp.CodMedioTransporte = medio.Id_MedioDeTransporte;
                                        objInformeMedioTrasnp.Valor = int.Parse(medio4.Text.Trim());
                                        consultas.Db.InformeMedioTransporte.InsertOnSubmit(objInformeMedioTrasnp);
                                        consultas.Db.SubmitChanges();
                                        valorTrasnporte += int.Parse(medio4.Text.Trim());
                                    }
                                    break;
                            }
                        }
                        informeVisita.CostoVisita = (decimal)(valorViatico * diasDiferencia) + valorTrasnporte;
                        consultas.Db.SubmitChanges();
                        Session["InformeIdVisita"] = null;
                        Session["Nombre_Empresa"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Informe de visita guardado satisfactoriamente!')", true);
                        Response.Redirect("InformeVisitaInter.aspx");
                    }
                }
            }
            else
            {
                //update
                var objInformeVisita = (from iv in consultas.Db.InformeVisitaInterventoria
                                        where iv.Id_Informe == int.Parse(Session["InformeIdVisita"].ToString())
                                        select iv).FirstOrDefault();

                //var fecha = DateTime.Now.Date;

                var fechaSalida = Convert.ToDateTime(txtDate.Text).Date;
                var fechaRegreso = Convert.ToDateTime(txtDate2.Text).Date;

                if (fechaRegreso < fechaSalida)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha de Salida no puede ser más reciente que la Fecha de Regreso!!!')", true);
                    return;
                }

                objInformeVisita.NombreInforme = txtinforme.Text.Trim();
                objInformeVisita.CodCiudadOrigen = int.Parse(ddl_ciuorigen.SelectedValue);
                objInformeVisita.CodCiudadDestino = int.Parse(ddl_ciudestino.SelectedValue);
                objInformeVisita.FechaSalida = fechaSalida.Date;
                objInformeVisita.FechaRegreso = fechaRegreso.Date;
                objInformeVisita.InformacionTecnica = tb_info_tecnica.Text;
                objInformeVisita.InformacionFinanciera = tb_info_financiera.Text;

                consultas.Db.SubmitChanges();

                // Costo viajes
                var interventor = (from i in consultas.Db.Interventor1s
                                   where i.CodContacto == objInformeVisita.CodInterventor // usuario.IdContacto
                                   select i).FirstOrDefault();
                var diasDiferencia = (objInformeVisita.FechaRegreso - objInformeVisita.FechaSalida).Days;

                var mediosTrasp = (from mt in consultas.Db.MedioDeTransporte
                                   select mt).ToList();

                var valorTrasnporte = 0;

                var listInformeMedioTrasnp = (from im in consultas.Db.InformeMedioTransporte
                                             where im.CodInforme == objInformeVisita.Id_Informe
                                             select im).ToList();

                var valorViatico = (from v in consultas.Db.Viatico
                                    where (v.LimiteInferior < interventor.Salario && v.LimiteSuperior > interventor.Salario) && v.LimiteSuperior != -1
                                    select v.Valor).FirstOrDefault();
                var valor = string.Empty;

                foreach (var medio in mediosTrasp)
                {
                    var infoMedio = listInformeMedioTrasnp.FirstOrDefault(x => x.CodMedioTransporte == medio.Id_MedioDeTransporte);
                    if(infoMedio != null)
                    {
                        valor = Page.Request.Form["ctl00$bodyContentPlace$medio" + infoMedio.CodMedioTransporte].ToString();
                        infoMedio.Valor = int.Parse(valor.Trim());
                        valorTrasnporte += int.Parse(valor.Trim());
                        consultas.Db.SubmitChanges();
                    }
                    else
                    {
                        var txtValor = Page.Request.Form["ctl00$bodyContentPlace$medio" + medio.Id_MedioDeTransporte].ToString();
                        var objInformeMedioTrasnp = new Datos.InformeMedioTransporte();
                        objInformeMedioTrasnp.CodInforme = objInformeVisita.Id_Informe;
                        objInformeMedioTrasnp.CodMedioTransporte = medio.Id_MedioDeTransporte;
                        objInformeMedioTrasnp.Valor = int.Parse(txtValor.Trim());
                        consultas.Db.InformeMedioTransporte.InsertOnSubmit(objInformeMedioTrasnp);
                        consultas.Db.SubmitChanges();
                        valorTrasnporte += int.Parse(txtValor.Trim());
                    }
                }
                objInformeVisita.CostoVisita = (decimal)(valorViatico * diasDiferencia) + valorTrasnporte;
                consultas.Db.SubmitChanges();
                Response.Redirect("InformeVisitaInter.aspx");
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 22/05/2014.
        /// Validar "nuevamente" la selección de departamento y ciudad origen.
        /// </summary>
        /// <returns>String con datos = NO debe continuar. // String vacío: Continuar.</returns>
        private string ValidarSeleccion()
        {
            try
            {
                if (ddl_ciuorigen.SelectedValue == "" || ddl_ciuorigen.SelectedValue == null)
                {
                    return "Debe seleccionar la ciudad de origen.";
                }
                if (ddl_dedorigen.SelectedValue == "" || ddl_dedorigen.SelectedValue == null)
                {
                    return "Debe seleccionar el departamento de origen.";
                }

                return "";
            }
            catch { return "ERROR"; }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("InformeVisitaInter.aspx");
        }
    }
}