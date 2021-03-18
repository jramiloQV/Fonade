using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Text;

namespace Fonade.PlanDeNegocioV2.Formulacion.PlanOperativo
{
    public partial class MetasSociales : Negocio.Base_Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public string codConvocatoria;
        public int txtTab = Constantes.CONST_MetasSocialesV2;
        public bool habilitado = false;
        public Boolean esMiembro;
        public Boolean bRealizado;
        public Boolean PostitVisible
        {
            get
            {
                return esMiembro && !bRealizado;
            }
            set { }
        }
        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, CodigoProyecto); } } }

        public bool EmprendedorFormulacion { get { return (esMiembro && !bRealizado); } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(CodigoProyecto).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = CodigoProyecto;
            Encabezado.CodigoTab = txtTab;

            SetPostIt();

            codConvocatoria = Convert.ToString(HttpContext.Current.Session["codConvocatoria"] ?? "");

            inicioEncabezado(CodigoProyecto.ToString(), codConvocatoria, txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), CodigoProyecto.ToString(), "");

            if (esMiembro == true && !bRealizado)
            { habilitado = true; }

            if (esMiembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            { btnGuardar.Visible = true; }

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();
                CargarTextArea();
                CargarGridEmpleos();
                CargarGridEmprendedores();
                HabilitarCampos();
                HabilitarCampos_Texto();
            }

        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_MetasSocialesV2;
        }

        #region General

        protected void CargarTextArea()
        {
            #region Diego Quiñonez - 17 Diciembre de 2014

            txtPlanRegional.Text = string.Empty;
            txtPlanNacional.Text = string.Empty;
            txtCluster.Text = string.Empty;
            txtEmpleosIndirectos.Text = string.Empty;

            var query = (from p in consultas.Db.ProyectoMetaSocials
                         where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                         select p).FirstOrDefault();

            if (query != null)
            {
                txtPlanRegional.Text = query.PlanRegional;
                txtPlanNacional.Text = query.PlanNacional;
                txtCluster.Text = query.Cluster;

                var sumatoriaEmpleos = consultas.Db.ProyectoMetaSocials.Where(m => m.CodProyecto == Convert.ToInt32(CodigoProyecto) && m.EmpleoIndirecto != null).Count() > 0 ? consultas.Db.ProyectoMetaSocials.Where(m => m.CodProyecto == Convert.ToInt32(CodigoProyecto)).Sum(i => i.EmpleoIndirecto).Value : 0;
                txtEmpleosIndirectos.Text = sumatoriaEmpleos.ToString();
            }

            #endregion

        }

        private void CargarGridEmpleos()
        {
            try
            {
                DataTable respuesta = new DataTable();
                DataTable respuesta2 = new DataTable();
                string consulta =
                " select cast(id_cargo as int) as IdCargo, cast(cargo as varchar(100)) as Cargo, valormensual as ValorMensual, ";
                consulta += " cast(GeneradoPrimerAno as varchar) as GeneradoPrimerAnio, Joven as EsJoven, Desplazado as EsDesplazado, Madre as EsMadre, ";
                consulta += " Minoria as EsMinoria, Recluido as EsRecluido, Desmovilizado as EsDesmovilizado, Discapacitado as EsDiscapacitado,  Desvinculado as EsDesvinculado ";
                consulta += " from  proyectoempleocargo right OUTER JOIN proyectogastospersonal ";
                consulta += "on id_cargo=codcargo where codproyecto= " + CodigoProyecto;

                respuesta = consultas.ObtenerDataTable(consulta, "text");

                string consulta2 = "select " +
                                " cast(id_insumo as int) as IdCargo, cast(nominsumo as varchar(100)) as Cargo, Convert(Numeric, sueldomes) as ValorMensual, cast(GeneradoPrimerAno as varchar)  as GeneradoPrimerAnio, Joven  as EsJoven, " +
                                " Desplazado as EsDesplazado, Madre as EsMadre, Minoria as EsMinoria,Recluido as EsRecluido, Desmovilizado as EsDesmovilizado, " +
                                " Discapacitado as EsDiscapacitado, Desvinculado as EsDesvinculado " +
                                " from proyectoinsumo " +
                                " inner join ProyectoProductoInsumo on id_Insumo = CodInsumo " +
                                " left join proyectoempleomanoobra on id_Insumo = CodManoObra " +
                                " where codTipoInsumo = 2 and CodProyecto = " + CodigoProyecto;

                respuesta2 = consultas.ObtenerDataTable(consulta2, "text");

                //Se crea nueva funcionalidad para enumerar cantidad de empleos a generar
                string consulta3 = "select * from ProyectoMetaSocial where codproyecto=" + CodigoProyecto;

                var empleosGenerar = consultas.ObtenerDataTable(consulta3, "text");

                var kjh = empleosGenerar.Compute("SUM([EmpleoIndirecto])", string.Format("[CodProyecto]={0}", CodigoProyecto)) ?? 0;

                primer_ano.Text = empleosGenerar.Rows.Count.ToString();

                Total_empleos.Text = string.IsNullOrEmpty(kjh.ToString()) ? "0" : kjh.ToString();
                var qry = "select (select COUNT(*) from proyectoinsumo inner join ProyectoProductoInsumo on CodInsumo = Id_Insumo LEFT OUTER JOIN" + " proyectoempleomanoobra  on id_insumo=codmanoobra where codtipoinsumo=2 and codproyecto={0}) + " +
                  "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal  on id_cargo=codcargo where codproyecto={0}) as Conteototal, " +
"(select COUNT(*) from proyectoinsumo  inner join ProyectoProductoInsumo  on CodInsumo = Id_Insumo LEFT OUTER JOIN proyectoempleomanoobra on id_insumo=codmanoobra" + " where codtipoinsumo=2 and codproyecto={0} and GeneradoPrimerAno  is not null and GeneradoPrimerAno!=0) + " +
"(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal on id_cargo=codcargo  where codproyecto={0} and GeneradoPrimerAno is not null" + " and GeneradoPrimerAno!=0) as ConteoAño";
                var xct = new Clases.genericQueries().executeQueryReader(string.Format(qry, CodigoProyecto));
                DataTable dtEmpTtl = new DataTable();
                dtEmpTtl.Load(xct, LoadOption.OverwriteChanges);
                primer_ano.Text = dtEmpTtl.Rows[0]["ConteoAño"].ToString();
                Total_empleos.Text = dtEmpTtl.Rows[0]["Conteototal"].ToString();
                if (respuesta2.Rows.Count > 0)
                {
                    Label_ManoObra.Visible = true;
                }


                gw_Empleos.DataSource = respuesta;
                gw_ManoObra.DataSource = respuesta2;
                gw_Empleos.DataBind();
                gw_ManoObra.DataBind();

            }
            catch (ArgumentException)
            {
                /*gw_Empleos.DataBind();
                gw_ManoObra.DataBind();*/
            }
        }

        private void CargarGridEmprendedores()
        {

            var query = (from p in consultas.Db.ProyectoContactos
                         from c in consultas.Db.Contacto
                         where p.CodContacto == c.Id_Contacto &&
                          p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                          p.CodRol == Constantes.CONST_RolEmprendedor &&
                          p.Inactivo == false
                         orderby c.Nombres, c.Apellidos ascending
                         select new
                         {
                             c.Id_Contacto,
                             nombres = c.Nombres + " " + c.Apellidos,
                             Beneficiario = (p.Beneficiario != null) ? p.Beneficiario : false,
                             Participacion = (p.Participacion != null) ? p.Participacion : 0
                         });

            gw_emprendedores.DataSource = query;
            gw_emprendedores.DataBind();

            for (int i = 0; i < gw_emprendedores.Rows.Count; i++)
            {

                if (gw_emprendedores.DataKeys[i].Value.ToString() != usuario.IdContacto.ToString())
                {
                    ((CheckBox)gw_emprendedores.Rows[i].FindControl("chkBeneficiario")).Checked = true;
                    ((CheckBox)gw_emprendedores.Rows[i].FindControl("chkBeneficiario")).Enabled = false;
                    ((TextBox)gw_emprendedores.Rows[i].FindControl("txtParticipacion")).Visible = false;
                    ((Label)gw_emprendedores.Rows[i].FindControl("lblParticipacion")).Visible = true;

                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string txtSQL = string.Empty;
            if (ValidarFormulario())
            {

                #region Diego Quiñonez - 17 de Diciembre de 2014

                var query = (from p in consultas.Db.ProyectoMetaSocials
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             select p).FirstOrDefault();

                if (query == null)
                {


                    Datos.ProyectoMetaSocial datosNuevos = new ProyectoMetaSocial()
                    {
                        CodProyecto = Convert.ToInt32(CodigoProyecto),
                        PlanNacional = txtPlanNacional.Text,
                        PlanRegional = txtPlanRegional.Text,
                        Cluster = txtCluster.Text,
                        EmpleoIndirecto = !string.IsNullOrEmpty(txtEmpleosIndirectos.Text) ? short.Parse(txtEmpleosIndirectos.Text) : short.Parse("0")
                    };

                    txtSQL = "INSERT INTO ProyectoMetaSocial (CodProyecto, PlanNacional, PlanRegional, Cluster, EmpleoIndirecto) VALUES(" + CodigoProyecto + ",'" + datosNuevos.PlanNacional + "','" + datosNuevos.PlanRegional + "','" + datosNuevos.Cluster + "'," + datosNuevos.EmpleoIndirecto + ")";
                }
                else
                {
                    query.PlanNacional = txtPlanNacional.Text;
                    query.PlanRegional = txtPlanRegional.Text;
                    query.Cluster = txtCluster.Text;
                    query.EmpleoIndirecto = !string.IsNullOrEmpty(txtEmpleosIndirectos.Text) ? short.Parse(txtEmpleosIndirectos.Text) : short.Parse("0");

                    txtSQL = "UPDATE ProyectoMetaSocial SET PlanNacional='" + query.PlanNacional + "',PlanRegional='" + query.PlanRegional + "',Cluster='" + query.Cluster + "',EmpleoIndirecto=" + query.EmpleoIndirecto + " WHERE codproyecto=" + CodigoProyecto;
                }

                ejecutaReader(txtSQL, 2);

                RegistrarEmpleos();
                RegistrarManoObra();
                RegistrarParticipacion();
                consultas.Db.ExecuteCommand(UsuarioActual());
                consultas.Db.SubmitChanges();

                #endregion

            }

            prActualizarTab(txtTab.ToString(), CodigoProyecto.ToString());

            ObtenerDatosUltimaActualizacion();
            CargarTextArea();
            CargarGridEmpleos();
            CargarGridEmprendedores();
            HabilitarCampos();
            HabilitarCampos_Texto();
        }

        protected bool ValidarFormulario()
        {
            if (txtPlanNacional.Text.Trim() == "")
            {
                Alert1.Ver("El Plan Nacional es requerido", true);
                txtPlanNacional.Focus();
                return false;
            }
            if (txtPlanRegional.Text.Trim() == "")
            {
                Alert1.Ver("El Plan Regional es requerido", true);
                txtPlanRegional.Focus();
                return false;
            }
            if (txtCluster.Text.Trim() == "")
            {
                Alert1.Ver("El Plan Regional es requerido", true);
                txtCluster.Focus();
                return false;
            }

            if (txtPlanNacional.Text.Trim().Length > 450)
            {
                Alert1.Ver("El campo excede el tamaño permitido", true);
                txtPlanNacional.Focus();
                return false;
            }
            if (txtPlanRegional.Text.Trim().Length > 450)
            {
                Alert1.Ver("El campo excede el tamaño permitido", true);
                txtPlanRegional.Focus();
                return false;
            }
            if (txtCluster.Text.Trim().Length > 450)
            {
                Alert1.Ver("El campo excede el tamaño permitido", true);
                txtCluster.Focus();
                return false;
            }

            //Diego Quiñonez - 17 de Diciembre de 2014
            if (string.IsNullOrEmpty(txtEmpleosIndirectos.Text))
                txtEmpleosIndirectos.Text = "0";
            else
            {
                short valNum;
                if (!short.TryParse(txtEmpleosIndirectos.Text, out valNum))
                {
                    Alert1.Ver("El campo excede el tamaño permitido", true);
                    txtCluster.Focus();
                    return false;
                }
            }

            for (int i = 0; i < gw_ManoObra.Rows.Count; i++)
            {
                TextBox sueldo = ((TextBox)gw_ManoObra.Rows[i].FindControl("txtSueldo"));
                if (sueldo.Text.Trim() == "")
                {
                    sueldo.Text = "0";
                }
                else
                {
                    decimal valor = 0;
                    if (!decimal.TryParse(sueldo.Text, out valor))
                    {
                        Alert1.Ver("El valor debe ser numérico", true);
                        sueldo.Focus();
                        return false;
                    }
                }
            }

            for (int i = 0; i < gw_emprendedores.Rows.Count; i++)
            {
                TextBox participacion = ((TextBox)gw_emprendedores.Rows[0].FindControl("txtParticipacion"));
                if (participacion.Visible == true)
                {
                    if (participacion.Text.Trim() == "")
                    {
                        participacion.Text = "0";
                    }
                    else
                    {
                        decimal valor = 0;
                        if (!decimal.TryParse(participacion.Text, out valor))
                        {
                            Alert1.Ver("El valor debe ser numérico", true);
                            participacion.Focus();
                            return false;
                        }
                        if (Convert.ToInt32(participacion.Text) > 100 || Convert.ToInt32(participacion.Text) < 0)
                        {
                            Alert1.Ver("Porcentaje no vlaido", true);
                            participacion.Focus();
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        protected void RegistrarEmpleos()
        {
            #region Diego Quiñonez - 17 de Diciembre de 2014
            foreach (GridViewRow gvr in gw_Empleos.Rows)
            {
                #region obtener valores

                var ddlGeneradoPrimerAno = (DropDownList)gvr.FindControl("ddlGeneradoMes");
                var chkJoven = (CheckBox)gvr.FindControl("chkEdad18_24");
                var chkDesplazado = (CheckBox)gvr.FindControl("chkDesplazado");
                var chkMadre = (CheckBox)gvr.FindControl("chkMadreCabeza");
                var chkMinoria = (CheckBox)gvr.FindControl("chkMinoriaEtnica");
                var chkRecluido = (CheckBox)gvr.FindControl("chkRecluidoCarceles");
                var chkDesmovilizado = (CheckBox)gvr.FindControl("chkDesmovilizado");
                var chkDiscapacitado = (CheckBox)gvr.FindControl("chkDiscapacitado");
                var chkDesvinculado = (CheckBox)gvr.FindControl("chkDesvinculado");

                #endregion

                int codCargo = int.Parse(gw_Empleos.DataKeys[gvr.RowIndex].Value.ToString());

                ProyectoEmpleoCargo queryEmpleo = (from p in consultas.Db.ProyectoEmpleoCargos
                                                   where p.CodCargo == codCargo
                                                   select p).FirstOrDefault();


                if (String.IsNullOrEmpty(ddlGeneradoPrimerAno.SelectedValue))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un mes.');", true);
                }
                else
                {
                    if (queryEmpleo == null)
                    {
                        ProyectoEmpleoCargo datoNuevo = new ProyectoEmpleoCargo()
                        {
                            CodCargo = codCargo,
                            GeneradoPrimerAno = byte.Parse(ddlGeneradoPrimerAno.SelectedValue),
                            Joven = chkJoven.Checked,
                            Desplazado = chkDesplazado.Checked,
                            Madre = chkMadre.Checked,
                            Minoria = chkMinoria.Checked,
                            Recluido = chkRecluido.Checked,
                            Desmovilizado = chkDesmovilizado.Checked,
                            Discapacitado = chkDiscapacitado.Checked,
                            Desvinculado = chkDesvinculado.Checked
                        };
                        consultas.Db.ProyectoEmpleoCargos.InsertOnSubmit(datoNuevo);
                    }
                    else
                    {
                        queryEmpleo.GeneradoPrimerAno = byte.Parse(ddlGeneradoPrimerAno.SelectedValue);
                        queryEmpleo.Joven = chkJoven.Checked;
                        queryEmpleo.Desplazado = chkDesplazado.Checked;
                        queryEmpleo.Madre = chkMadre.Checked;
                        queryEmpleo.Minoria = chkMinoria.Checked;
                        queryEmpleo.Recluido = chkRecluido.Checked;
                        queryEmpleo.Desmovilizado = chkDesmovilizado.Checked;
                        queryEmpleo.Discapacitado = chkDiscapacitado.Checked;
                        queryEmpleo.Desvinculado = chkDesvinculado.Checked;
                        consultas.Db.SubmitChanges();
                    }
                }

            }
            #endregion           
        }

        protected void RegistrarManoObra()
        {
            #region Diego Quiñonez - 17 de Diciembre de 2014
            foreach (GridViewRow gvr in gw_ManoObra.Rows)
            {
                #region obtener valores

                var txtSueldo = (TextBox)gvr.FindControl("txtSueldo");
                var ddlGeneradoMes = (DropDownList)gvr.FindControl("ddlGeneradoMes");
                var chkchkEdad18_24 = (CheckBox)gvr.FindControl("chkEdad18_24");
                var chkDesplazado = (CheckBox)gvr.FindControl("chkDesplazado");
                var chkMadreCabeza = (CheckBox)gvr.FindControl("chkMadreCabeza");
                var chkMinoriaEtnica = (CheckBox)gvr.FindControl("chkMinoriaEtnica");
                var chkRecluidoCarceles = (CheckBox)gvr.FindControl("chkRecluidoCarceles");
                var chkDesmovilizado = (CheckBox)gvr.FindControl("chkDesmovilizado");
                var chkDiscapacitado = (CheckBox)gvr.FindControl("chkDiscapacitado");
                var chkDesvinculado = (CheckBox)gvr.FindControl("chkDesvinculado");

                #endregion

                int codInsumo = int.Parse(gw_ManoObra.DataKeys[gvr.RowIndex].Value.ToString());

                ProyectoEmpleoManoObra queryManoObra = (from p in consultas.Db.ProyectoEmpleoManoObras
                                                        where p.CodManoObra == codInsumo
                                                        select p).FirstOrDefault();

                if (String.IsNullOrEmpty(ddlGeneradoMes.SelectedValue))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un mes.');", true);
                }
                else
                {
                    if (queryManoObra == null)
                    {
                        ProyectoEmpleoManoObra NuevoReg = new ProyectoEmpleoManoObra()
                        {
                            CodManoObra = codInsumo,
                            SueldoMes = decimal.Parse(txtSueldo.Text),
                            GeneradoPrimerAno = byte.Parse(ddlGeneradoMes.SelectedValue),
                            Joven = chkchkEdad18_24.Checked,
                            Desplazado = chkDesplazado.Checked,
                            Madre = chkMadreCabeza.Checked,
                            Minoria = chkMinoriaEtnica.Checked,
                            Recluido = chkRecluidoCarceles.Checked,
                            Desmovilizado = chkDesmovilizado.Checked,
                            Discapacitado = chkDiscapacitado.Checked,
                            Desvinculado = chkDesvinculado.Checked
                        };
                        consultas.Db.ProyectoEmpleoManoObras.InsertOnSubmit(NuevoReg);
                        consultas.Db.SubmitChanges();
                    }
                    else
                    {
                        queryManoObra.SueldoMes = decimal.Parse(txtSueldo.Text);
                        queryManoObra.GeneradoPrimerAno = byte.Parse(ddlGeneradoMes.SelectedValue);
                        queryManoObra.Joven = chkchkEdad18_24.Checked;
                        queryManoObra.Desplazado = chkDesplazado.Checked;
                        queryManoObra.Madre = chkMadreCabeza.Checked;
                        queryManoObra.Minoria = chkMinoriaEtnica.Checked;
                        queryManoObra.Recluido = chkRecluidoCarceles.Checked;
                        queryManoObra.Desmovilizado = chkDesmovilizado.Checked;
                        queryManoObra.Discapacitado = chkDiscapacitado.Checked;
                        queryManoObra.Desvinculado = chkDesvinculado.Checked;
                        consultas.Db.SubmitChanges();
                    }
                }
            }
            #endregion

        }

        protected void RegistrarParticipacion()
        {
            //try
            //{
            var queryEmpleo = (from p in consultas.Db.ProyectoContactos
                               where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                               p.CodContacto == usuario.IdContacto &&
                               p.Inactivo == false &&
                               p.FechaFin == null
                               select p).FirstOrDefault();

            if (gw_emprendedores.Rows.Count > 0)
            {
                if (queryEmpleo != null)
                {
                    string participacion = ((TextBox)gw_emprendedores.Rows[0].FindControl("txtParticipacion")).Text;
                    bool benficiario = ((CheckBox)gw_emprendedores.Rows[0].FindControl("chkBeneficiario")).Checked;

                    queryEmpleo.Beneficiario = benficiario;
                    queryEmpleo.Participacion = Convert.ToDouble(participacion);
                    consultas.Db.SubmitChanges();
                }
            }
            //}
            //catch
            //{
            //}

            //consultas.Db.SubmitChanges();
        }

        protected void HabilitarCampos()
        {
            txtPlanRegional.Enabled = habilitado;
            txtPlanNacional.Enabled = habilitado;
            txtEmpleosIndirectos.Enabled = habilitado;
            txtCluster.Enabled = habilitado;

            for (int i = 0; i < gw_Empleos.Rows.Count; i++)
            {

                ((DropDownList)gw_Empleos.Rows[i].FindControl("ddlGeneradoMes")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkEdad18_24")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkDesplazado")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkMadreCabeza")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkMinoriaEtnica")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkRecluidoCarceles")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkDesmovilizado")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkDiscapacitado")).Enabled = habilitado;
                ((CheckBox)gw_Empleos.Rows[i].FindControl("chkDesvinculado")).Enabled = habilitado;
            }


            for (int i = 0; i < gw_ManoObra.Rows.Count; i++)
            {


                ((TextBox)gw_ManoObra.Rows[i].FindControl("txtSueldo")).Enabled = habilitado; ;
                ((DropDownList)gw_ManoObra.Rows[i].FindControl("ddlGeneradoMes")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkEdad18_24")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkDesplazado")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkMadreCabeza")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkMinoriaEtnica")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkRecluidoCarceles")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkDesmovilizado")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkDiscapacitado")).Enabled = habilitado;
                ((CheckBox)gw_ManoObra.Rows[i].FindControl("chkDesvinculado")).Enabled = habilitado;
            }

        }
        #endregion General

        #region Métodos de .

        public void HabilitarCampos_Texto()
        {
            //Inicializar variables.
            bool EsMiembro = false;
            bool bRealizado = true;

            try
            {
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
                bRealizado = esRealizado(txtTab.ToString(), CodigoProyecto.ToString(), "");

                if (!(EsMiembro && !bRealizado) || usuario.CodGrupo != Constantes.CONST_Emprendedor)
                {
                    #region Deshabilitar campos...

                    txtPlanNacional.Enabled = false;
                    txtPlanRegional.Enabled = false;
                    txtCluster.Enabled = false;
                    tabla_1.Attributes.Add("enabled", "disabled");

                    #endregion
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>

        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bRealizado = false;
            bool EsMiembro = false;
            bool EsNuevo = true;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), CodigoProyecto.ToString(), ""); //codConvocatoria);

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(CodigoProyecto, txtTab);

                if (usuActualizo != null)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.SingleOrDefault().nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.SingleOrDefault().fecha); }
                    catch { fecha = DateTime.Today; }
                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); }
                    if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.SingleOrDefault().realizado;
                }


                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(CodigoProyecto.ToString())))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(CodigoProyecto.ToString())))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                    }
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    //tabla_docs.Visible = true;
                }

                if (usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    visibleGuardar = true;
                }
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
            }
        }

        /// <summary>

        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(CodigoProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>

        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            prActualizarTab(txtTab.ToString(), CodigoProyecto.ToString());
            var chkRealizado = (Request.Form.Get("chk_realizado") == "on" ? true : false);
            Marcar(txtTab.ToString(), CodigoProyecto.ToString(), "", chkRealizado);
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "PlanOperMetas";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "PlanOperMetas";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        /// <summary>

        /// 11/09/2014.
        /// Des/habilitar el CheckBox del beneficiario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_emprendedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var chk = e.Row.FindControl("chkBeneficiario") as CheckBox;
                var txt = e.Row.FindControl("txtParticipacion") as TextBox;

                if (chk != null && txt != null)
                { chk.Enabled = habilitado; txt.Enabled = habilitado; }
            }
        }

        protected void gw_Empleos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    ((DropDownList)e.Row.Cells[2].FindControl("ddlGeneradoMes")).Enabled = false;
                    ((CheckBox)e.Row.Cells[3].FindControl("chkEdad18_24")).Enabled = false;
                    ((CheckBox)e.Row.Cells[4].FindControl("chkDesplazado")).Enabled = false;
                    ((CheckBox)e.Row.Cells[5].FindControl("chkMadreCabeza")).Enabled = false;
                    ((CheckBox)e.Row.Cells[6].FindControl("chkMinoriaEtnica")).Enabled = false;
                    ((CheckBox)e.Row.Cells[7].FindControl("chkRecluidoCarceles")).Enabled = false;
                    ((CheckBox)e.Row.Cells[8].FindControl("chkDesmovilizado")).Enabled = false;
                    ((CheckBox)e.Row.Cells[9].FindControl("chkDiscapacitado")).Enabled = false;
                    ((CheckBox)e.Row.Cells[10].FindControl("chkDesvinculado")).Enabled = false;
                }
            }
        }
    }

    public class BORespuestaEmpleos
    {
        public int IdCargo { get; set; }
        public string Cargo { get; set; }
        public decimal ValorMensual { get; set; }
        public int GeneradoPrimerAnio { get; set; }
        public int EsJoven { get; set; }
        public int EsDesplazado { get; set; }
        public int EsMadre { get; set; }
        public int EsMinoria { get; set; }
        public int EsRecluido { get; set; }
        public int EsDesmovilizado { get; set; }
        public int EsDiscapacitado { get; set; }
        public int EsDesvinculado { get; set; }
    }
}