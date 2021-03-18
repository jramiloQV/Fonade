#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>10 - 06 - 2014</Fecha>
// <Archivo>Consultas.cs</Archivo>

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using Datos;

using Datos.DataType;
using System.Xml;
using System.Data;
using Fonade.Account;
using System.Configuration;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.MiPerfil
{
    public partial class Consultas : Negocio.Base_Page
    {
        public const int PAGE_SIZE = 10;
        public int tiporol1;
        public int tiporol2;
        public int tiporol3;
        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;
        /// <summary>
        /// Conteo de registros obtenidos de la consulta de planes de negocio por palabra.
        /// </summary>
        Int32 conteo_grvMain;

        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        public bool mostrarAbrirEmpresa
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    return true;
                else
                    return false;
            }
            set { }
        }

        private string mostrarPlanOEmpresa = "plan";
        public string MostrarPlanOEmpresa
        {
            get {                          
                 return mostrarPlanOEmpresa;                
            }
            set {
                mostrarPlanOEmpresa = value;
            }
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDllOperador(int? _codOperador)
        {
            List<OperadorModel> operadores = new List<OperadorModel>();

            if (_codOperador == null) //Todos los operadores
            {
                operadores = operadorController.getAllOperador();

                ddloperador.DataSource = operadores;
                ddloperador.DataTextField = "NombreOperador";
                ddloperador.DataValueField = "idOperador";
                ddloperador.DataBind();
                ddloperador.Items.Insert(0, new ListItem("(Todos los Operadores)", ""));
            }
            else
            {
                operadores.Add(operadorController.getOperador(_codOperador));

                ddloperador.DataSource = operadores;
                ddloperador.DataTextField = "NombreOperador";
                ddloperador.DataValueField = "idOperador";
                ddloperador.DataBind();
            }
        }

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btn_AbrirEmpresa.Visible = mostrarAbrirEmpresa;

                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    AsesoresPanel.Visible = false;
                    tb_codigo.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                    if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema)
                    {
                        vercoor1.Visible = false;
                        try { vercoor1.Attributes.Add("", "disabled"); /*vercoor1.Enabled = false;*/ }
                        catch { }
                        vercoor2.Visible = false;
                        try { vercoor2.Attributes.Add("", "disabled");/*vercoor2.Enabled = false;*/ }
                        catch { }
                    }
                    if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "move")
                    {
                        tb_asesor.Text = lb_asesores.SelectedItem.Text;

                    }

                    cargarDllOperador(usuario.CodOperador);

                    var departamentos = from d in consultas.Db.departamento
                                        where d.CodPais == 1
                                        orderby d.NomDepartamento ascending
                                        select new
                                        {
                                            id_Departamento = d.Id_Departamento,
                                            nomDepartamento = d.NomDepartamento
                                        };

                    ddl_departamento.DataSource = departamentos;
                    ddl_departamento.DataTextField = "nomDepartamento";
                    ddl_departamento.DataValueField = "id_Departamento";
                    ddl_departamento.DataBind();
                    ddl_departamento.Items.Insert(0, new ListItem("(Todos los Departamentos)", ""));
                    //if(usuario.CodGrupo == 16)
                    //{
                    //    var lider = (from c in consultas.Db.Contactos
                    //                 where c.Id_Contacto == usuario.IdContacto
                    //                 select c).FirstOrDefault();
                    //    ddl_departamento.SelectedValue = lider.CodTipoAprendiz.ToString();
                    //    ddl_departamento.Enabled = false;

                    //    var ciudades = (from c in consultas.Db.Ciudads
                    //                    where c.CodDepartamento == lider.CodTipoAprendiz
                    //                    select c).ToList();
                    //    ddl_municipio.DataSource = ciudades;
                    //    ddl_municipio.DataValueField = "Id_Ciudad";
                    //    ddl_municipio.DataTextField = "NomCiudad";
                    //    ddl_municipio.DataBind();
                    //}

                    var estados = from es in consultas.Db.Estados
                                  orderby es.NomEstado
                                  select new
                                  {
                                      Id_Estado = es.Id_Estado,
                                      NomEstado = es.NomEstado
                                  };
                    lb_estados.DataSource = estados.ToList();
                    lb_estados.DataTextField = "NomEstado";
                    lb_estados.DataValueField = "Id_Estado";
                    lb_estados.DataBind();
                    lb_estados.Items.Insert(0, new ListItem("                              ", ""));

                    List<Institucion> unidad;
                    if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                    {
                        var contacto = (from c in consultas.Db.Contacto
                                        where c.Id_Contacto == usuario.IdContacto
                                        select c).FirstOrDefault();

                        unidad = (from i in consultas.Db.Institucions
                                  join c in consultas.Db.Ciudad on i.CodCiudad equals c.Id_Ciudad
                                  join d in consultas.Db.departamento on c.CodDepartamento equals d.Id_Departamento
                                  where d.Id_Departamento == contacto.CodTipoAprendiz && i.Inactivo == false
                                  orderby i.NomUnidad
                                  select i).ToList();

                    }
                    else
                    {
                        unidad = (from i in consultas.Db.Institucions
                                  where i.Inactivo == false
                                  orderby i.NomUnidad
                                  select i).ToList();
                    }

                    if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                    {
                        unidad.Where(i => i.Id_Institucion == usuario.CodInstitucion);
                    }

                    lb_unidadEmprendimiento.DataSource = unidad.ToList();
                    lb_unidadEmprendimiento.DataTextField = "NomUnidad";
                    lb_unidadEmprendimiento.DataValueField = "Id_Institucion";
                    lb_unidadEmprendimiento.DataBind();
                    lb_unidadEmprendimiento.Items.Insert(0, new ListItem("                              ", ""));

                    var sectores = from s in consultas.Db.Sector

                                   orderby s.NomSector
                                   select new
                                   {
                                       Id_Sector = s.Id_Sector,
                                       NomSector = s.NomSector
                                   };
                    lb_sector.DataSource = sectores.ToList();
                    lb_sector.DataTextField = "NomSector";
                    lb_sector.DataValueField = "Id_Sector";
                    lb_sector.DataBind();
                    lb_sector.Items.Insert(0, new ListItem("                              ", ""));
                    lbl_Titulo.Text = void_establecerTitulo("CONSULTAS");

                    try
                    {
                        if (!string.IsNullOrEmpty(HttpContext.Current.Session["consultarMaster"].ToString()))
                        {
                            tb_porPalabra.Text = HttpContext.Current.Session["consultarMaster"].ToString();
                            HttpContext.Current.Session["consultarMaster"] = null;
                            grdMain.DataSource = null;
                            grdMain.DataBind();
                        }
                    }
                    catch (Exception ex) { errorMessageDetail = ex.Message; }
                    if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema 
                        || usuario.CodGrupo == Constantes.CONST_AdministradorSena 
                        || usuario.CodGrupo == Constantes.CONST_JefeUnidad
                        || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                        || usuario.CodGrupo == Constantes.CONST_CallCenter)
                    {
                        tr_part_I.Visible = true;
                        tr_part_II.Visible = true;
                        tr_part_III.Visible = true;

                        if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema 
                            || usuario.CodGrupo == Constantes.CONST_AdministradorSena 
                            || usuario.CodGrupo == Constantes.CONST_CallCenter
                            || usuario.CodGrupo == Constantes.CONST_CallCenterOperador)
                        { vercoor1.Visible = true; vercoor2.Visible = true; }
                    }

                    if (usuario.CodGrupo == Constantes.CONST_JefeUnidad 
                        || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                        || usuario.CodGrupo == Constantes.CONST_AdministradorSistema
                        || usuario.CodGrupo == Constantes.CONST_CallCenter) //Agregado el Rol "Call Center".
                    {
                        Panel1.Visible = true;
                        Panel2.Visible = false;
                        Panel3.Visible = false;
                        Panel4.Visible = false;
                        pnlpanel6.Visible = false;
                        pnlpanel5.Visible = false;
                        pnlInfoResultados.Visible = false;
                        if (usuario.CodGrupo == 8)
                        {
                            vercoor1.Visible = true;
                            vercoor2.Visible = true;
                        }

                    }

                    AsesoresPanel.Visible = true;
                    UpdatePanel1.Visible = true;
                }
            }
        }

        /// <summary>
        /// Buscar por palabra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_buscar_onclick(object sender, EventArgs e)
        {
            DataTable rstProyecto = new DataTable();
            if (tb_porPalabra.Text.Trim().Length<3)
            {
                lblMensajeBusqPorPalabra.Visible = true;
            }
            else
            {
                try
                {
                    if (usuario.CodGrupo != Constantes.CONST_LiderRegional)
                    {
                        txtSQL = " SET LANGUAGE 'Spanish'"
                                +" SELECT P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad"
	                            +" ,D.NomDepartamento"
	                            +" , cast(day(P.FechaCreacion) as varchar(2)) + ' '"
                                +" + cast(DATENAME(month, CAST(P.FechaCreacion AS datetime)) as varchar(3)) + ' '"
                                +" + cast(year(P.FechaCreacion) as varchar(4)) N_Fecha"
	                            +" , I.nomUnidad + ' (' + I.nomInstitucion + ')' Unidad"
	                            +" ,(Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) CodConvocatoria"
                                +" ,("
                                +" Select NomConvocatoria from Convocatoria where Id_Convocatoria ="
                                +" (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto)"
	                            +" ) as N_NomConvocatoria"
                                +" FROM Proyecto P with(nolock), SubSector S with(nolock)"
		                        +" , Estado E with(nolock), Ciudad C with(nolock)"
		                        +" , Departamento D with(nolock), Institucion I with(nolock)"
                                +" WHERE"
                                + " (P.NomProyecto like '%" + tb_porPalabra.Text.Trim() + "%')"
                                + " AND S.Id_SubSector = P.CodSubSector"
                                +" AND I.Id_Institucion = P.CodInstitucion"
                                +" AND E.Id_estado = P.CodEstado"
                                +" AND C.Id_Ciudad = P.CodCiudad"
                                +" AND C.CodDepartamento = D.Id_Departamento"
                                ;

                        if (usuario.CodOperador != null)
                        {
                            txtSQL = txtSQL + " and P.codOperador = " + usuario.CodOperador;                            
                        }

                        txtSQL = txtSQL + " and p.inactivo = 0; ";

                        //txtSQL = " SELECT P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad, D.NomDepartamento, P.FechaCreacion, I.nomUnidad + ' ('+I.nomInstitucion+')' Unidad " +
                        //     " ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria" +
                        //     " FROM Proyecto P , SubSector S, Estado E, Ciudad C, Departamento D, Institucion I " +
                        //     " WHERE " +
                        //     " (P.NomProyecto like '%" + tb_porPalabra.Text.Trim() + "%' " +
                        //     "  OR P.Sumario like '%" + tb_porPalabra.Text.Trim() + "%' " +
                        //     "  OR S.NomSubSector like '%" + tb_porPalabra.Text.Trim() + "%' " +
                        //     "  OR D.NomDepartamento like '%" + tb_porPalabra.Text.Trim() + "%' " +
                        //     "  OR C.NomCiudad like '%" + tb_porPalabra.Text.Trim() + "%') " +
                        //     " AND S.Id_SubSector = P.CodSubSector " +
                        //     " AND I.Id_Institucion = P.CodInstitucion " +
                        //     " AND E.Id_estado = P.CodEstado " +
                        //     " AND C.Id_Ciudad = P.CodCiudad " +
                        //     " AND C.CodDepartamento = D.Id_Departamento " +
                        //     " and p.inactivo=0";
                    }
                    else
                    {
                        var lider = (from c in consultas.Db.Contacto
                                     where c.Id_Contacto == usuario.IdContacto
                                     select c).FirstOrDefault();

                        txtSQL = "  SET LANGUAGE 'Spanish'"
                                +" SELECT P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad"
	                            +" ,D.NomDepartamento"
	                            +" , cast(day(P.FechaCreacion) as varchar(2)) + ' '"
                                +" + cast(DATENAME(month, CAST(P.FechaCreacion AS datetime)) as varchar(3)) + ' '"
                                +" + cast(year(P.FechaCreacion) as varchar(4)) N_Fecha"
	                            +" , I.nomUnidad + ' (' + I.nomInstitucion + ')' Unidad"
	                            +" ,(Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) CodConvocatoria"
                                +" ,("
                                +" Select NomConvocatoria from Convocatoria where Id_Convocatoria ="
                                +" (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto)"
	                            +" ) as N_NomConvocatoria"
                                +" FROM Proyecto P with(nolock), SubSector S with(nolock)"
		                        +" , Estado E with(nolock), Ciudad C with(nolock)"
		                        +" , Departamento D with(nolock), Institucion I with(nolock)"
                                +" WHERE"
                                + " (P.NomProyecto like '%"+tb_porPalabra.Text.Trim()+"%')"
                                + " AND S.Id_SubSector = P.CodSubSector"
                                +" AND I.Id_Institucion = P.CodInstitucion"
                                +" AND E.Id_estado = P.CodEstado"
                                +" AND C.Id_Ciudad = P.CodCiudad"
                                +" AND C.CodDepartamento = D.Id_Departamento"
                                + " and c.CodDepartamento = " + lider.CodTipoAprendiz + ""
                                + " and p.inactivo = 0; ";

                        //txtSQL = " Select P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad,";
                        //txtSQL += " D.NomDepartamento, P.FechaCreacion, I.nomUnidad + ' ('+I.nomInstitucion+')' Unidad  ,";
                        //txtSQL += " (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria";
                        //txtSQL += " from Proyecto p Inner Join Institucion i on i.Id_Institucion = p.CodInstitucion ";
                        //txtSQL += "Inner join Estado e on e.id_estado = p.codestado Inner join SubSector s on s.Id_SubSector = p.CodSubSector ";
                        //txtSQL += "Inner Join Ciudad c on c.Id_Ciudad = i.codCiudad Inner Join departamento D on d.id_departamento = c.codDepartamento ";
                        //txtSQL += "Where (P.NomProyecto like '%" + tb_porPalabra.Text.Trim() + "%'   OR P.Sumario like '%" + tb_porPalabra.Text.Trim() + "%'   OR S.NomSubSector like '%" + tb_porPalabra.Text.Trim() + "%') and c.CodDepartamento = " + lider.CodTipoAprendiz + "  and p.inactivo=0";
                    }

                    rstProyecto = consultas.ObtenerDataTable(txtSQL, "text");
                    conteo_grvMain = rstProyecto.Rows.Count;

                    //rstProyecto.Columns.Add("N_NomConvocatoria", typeof(System.String));
                    //rstProyecto.Columns.Add("N_Fecha", typeof(System.String));
                    //foreach (DataRow dr in rstProyecto.Rows)
                    //{
                    //    dr["N_NomConvocatoria"] = getNomConvocatoria(dr["CodConvocatoria"].ToString());

                    //    try { dr["N_Fecha"] = DateTime.Parse(dr["FechaCreacion"].ToString()).ToString(" d MMM yyyy"); }
                    //    catch { dr["N_Fecha"] = dr["FechaCreacion"].ToString(); }
                    //}

                    HttpContext.Current.Session["tablaConsultas_palabra"] = rstProyecto;

                    grdMain.EnableSortingAndPagingCallbacks = false;

                    grdMain.DataSource = rstProyecto;
                    grdMain.DataBind();
                    try { lblResults.Text = "Se han encontrado " + conteo_grvMain + " Planes de negocio de <br/> buscando ''" + tb_porPalabra.Text + "''"; }
                    catch { lblResults.Text = "Se han encontrado planes de <br/> negocio buscando ''"; }

                    Panel3.Visible = false;
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    pnlInfoResultados.Visible = true;
                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                AsesoresPanel.Visible = false;
            }
        }

        protected void hpl_nueva_onclick(object sender, EventArgs e)
        {
            LimpiarControles();
            Panel1.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            pnlInfoResultados.Visible = false;
            pnlpanel5.Visible = false;
            pnlpanel6.Visible = false;

            if (usuario.CodGrupo == 8)
            {
                vercoor1.Visible = true;
                vercoor2.Visible = true;
            }
            AsesoresPanel.Visible = true;
        }

        protected void btn_buscarAsesor_onclick(object sender, EventArgs e)
        {
            lb_asesores.Visible = true;
            lb_asesores.DataBind();
            lb_asesores.Attributes.Add("onchange", "setIdx()");
        }

        protected void btn_BusquedaAvanzada_onclick(object sender, EventArgs e)
        {
            MostrarPlanOEmpresa = "plan";
            BuscarProyecto();
        }

        private void BuscarProyecto()
        {
            //try
            //{
            DataTable rstProyecto = new DataTable();
            bool tieneAsesor;

            string txtSQLTemp = "";
            //string txtSQLTemp1 = "";

            string txtCodigo = "";
            string CodSector = "";
            string CodEstado = "";
            string CodUnidad = "";
            string CodContacto = "";
            string CodCiudad = "";
            string CodDepartamento = "";
            bool bolIncluye = CheckBox1.Checked;

            bool busquedaXcodigo = tb_codigo.Text.Trim() != "" ? true : false;

            if (busquedaXcodigo)
            {
                txtCodigo = tb_codigo.Text.Trim();
                txtSQL = txtSQL + " AND Id_Proyecto = " + txtCodigo;
            }
            else
            {
                if (lb_sector.SelectedValue != "")
                {
                    CodSector = lb_sector.SelectedValue;
                    txtSQL = txtSQL + " AND SC.Id_Sector IN (" + CodSector + ")";
                }

                if (lb_estados.SelectedValue != "")
                {
                    CodEstado = lb_estados.SelectedValue;
                    txtSQL = txtSQL + " AND E.Id_Estado IN(" + CodEstado + ")";
                }

                if (lb_unidadEmprendimiento.SelectedValue != "")
                {
                    CodUnidad = lb_unidadEmprendimiento.SelectedValue;
                    txtSQL = txtSQL + " AND P.CodInstitucion IN(" + CodUnidad + ")";
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                    {
                        var contacto = (from c in consultas.Db.Contacto
                                        where c.Id_Contacto == usuario.IdContacto
                                        select c).FirstOrDefault();
                        var ciudades = (from c in consultas.Db.Ciudad
                                        where c.CodDepartamento == contacto.CodTipoAprendiz
                                        select c).ToList();

                        var idCiudades = string.Empty;

                        if (ciudades.Count() > 0)
                        {
                            foreach (var ciudad in ciudades)
                            {
                                idCiudades += ciudad.Id_Ciudad.ToString() + ",";
                            }
                        }
                        idCiudades = idCiudades.Remove(idCiudades.Length - 1);

                        txtSQL = txtSQL + " AND I.CodCiudad IN(" + idCiudades + ")";
                    }
                }

                if (ddl_municipio.SelectedValue != "")
                {
                    CodCiudad = ddl_municipio.SelectedValue;
                    txtSQL = txtSQL + " AND P.CodCiudad IN(" + CodCiudad + ")";
                }

                if (ddl_departamento.SelectedValue != "")
                {
                    CodDepartamento = ddl_departamento.SelectedValue;
                    txtSQL = txtSQL + " AND CP.CodDepartamento IN(" + CodDepartamento + ")";
                }

            }

            if (!string.IsNullOrEmpty(hdf_CodContacto.Value))
            {
                CodContacto = hdf_CodContacto.Value;
            }

            if (CodContacto != "")
            {

                txtSQL = txtSQL + " AND PC.CodContacto = " + CodContacto;

                txtSQLTemp = " SELECT Id_Proyecto, NomProyecto, Sumario, S.NomSubSector, E.NomEstado"
                            + " , CP.NomCiudad + ', ' + DP.NomDepartamento NomCiudad"
                            + " , DP.NomDepartamento"
                            + " , NomUnidad + ' (' + NomInstitucion + ')' NomUnidad"
                            + " , NomInstitucion"
                            + " , CI.NomCiudad + ',' + DI.NomDepartamento as CiudadUnidad"
                            + " , A.Nombres + ' ' + A.Apellidos Asesor"
                            + " , case PC.CodRol when 1 then 'Lider' else '' end Lider"
                            + " ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto"
                            + " where CodProyecto = P.Id_Proyecto) as CodConvocatoria"
                            + " , I.CodTipoInstitucion"
                            + " ,("
                            + " Select NomConvocatoria from Convocatoria where Id_Convocatoria ="
                            + " (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto)"
                            + " ) as N_NomConvocatoria"
                            + " FROM Proyecto P with(nolock), SubSector S with(nolock)"
                            + " , Estado E with(nolock), Ciudad CP with(nolock), Departamento DP with(nolock)"
                            + " , Sector SC with(nolock), Institucion I with(nolock)"
                            + " , ProyectoContacto PC with(nolock), Contacto A with(nolock)"
                            + " , Ciudad CI with(nolock), Departamento DI with(nolock)"
                            + " WHERE S.Id_SubSector = P.CodSubSector " + txtSQL
                            + " AND PC.CodProyecto = P.Id_Proyecto"
                            + " AND PC.CodRol IN(1,2)"
                            + " AND PC.Codcontacto = A.id_contacto"
                            + " AND PC.Inactivo = 0"
                            + " AND P.CodSubSector = S.Id_SubSector"
                            + " AND SC.Id_Sector = S.CodSector"
                            + " AND E.Id_Estado = P.CodEstado"
                            + " AND CP.Id_Ciudad = P.CodCiudad"
                            + " AND CP.CodDepartamento = DP.Id_Departamento"
                            + " AND CI.Id_Ciudad = I.CodCiudad"
                            + " AND CI.CodDepartamento = DI.Id_Departamento"
                            + " AND P.CodInstitucion = Id_Institucion"
                            + " AND P.inactivo = 0; ";

                //txtSQLTemp = " SELECT Id_Proyecto, NomProyecto, Sumario, S.NomSubSector, E.NomEstado, CP.NomCiudad, DP.NomDepartamento, Nomunidad, NomInstitucion, CI.NomCiudad +','+ DI.NomDepartamento as CiudadUnidad, A.Nombres + ' ' + A.Apellidos Asesor, case PC.CodRol when 1 then 'Lider' else '' end Lider " +
                //             " ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria, I.CodTipoInstitucion" +
                //             " FROM Proyecto P, SubSector S, Estado E, Ciudad CP, Departamento DP, Sector SC, Institucion I, ProyectoContacto PC, Contacto A, Ciudad CI, Departamento DI " +
                //             " WHERE S.Id_SubSector = P.CodSubSector " + txtSQL +
                //             " AND PC.CodProyecto = P.Id_Proyecto" +
                //             " AND PC.CodRol IN(1,2)" +
                //             " AND PC.Codcontacto=A.id_contacto" +
                //             " AND PC.Inactivo = 0 " +
                //             " AND P.CodSubSector = S.Id_SubSector" +
                //             " AND SC.Id_Sector = S.CodSector" +
                //             " AND E.Id_Estado = P.CodEstado " +
                //             " AND CP.Id_Ciudad = P.CodCiudad " +
                //             " AND CP.CodDepartamento = DP.Id_Departamento" +
                //             " AND CI.Id_Ciudad = I.CodCiudad " +
                //             " AND CI.CodDepartamento = DI.Id_Departamento" +
                //             " AND P.CodInstitucion = Id_Institucion " +
                //             " AND P.inactivo=0";


                tieneAsesor = true;
            }
            else
            {
                txtSQLTemp = " SELECT Id_Proyecto, NomProyecto, Sumario, S.NomSubSector, E.NomEstado"
                            + " , CP.NomCiudad + ', ' + DP.NomDepartamento NomCiudad"
                            + " , DP.NomDepartamento"
                            + " , NomUnidad + ' (' + NomInstitucion + ')' NomUnidad"
                            + " , NomInstitucion"
                            + " , CI.NomCiudad + ',' + DI.NomDepartamento as CiudadUnidad"
                            + " ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria"
                            + " , '' AS[Asesor] , '' AS[Lider], I.CodTipoInstitucion"
                            + " ,("
                            + " Select NomConvocatoria from Convocatoria where Id_Convocatoria ="
                            + " (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto)"
                            + " ) as N_NomConvocatoria"
                            + " , o.NombreOperador "
                            + " FROM Proyecto P with(nolock)" 
                            + " left join operador o with(nolock) on o.idOperador = P.codOperador "
                            + ", SubSector S with(nolock)"
                            + " , Estado E with(nolock), Ciudad CP with(nolock)"
                            + " , Departamento DP with(nolock), Sector SC with(nolock)"
                            + " , Institucion I with(nolock), Ciudad CI with(nolock), Departamento DI with(nolock)"
                            
                            + " WHERE S.Id_SubSector = P.CodSubSector" + txtSQL
                            + " AND S.codsector = sc.Id_Sector"
                            //+ " AND o.idOperador = P.codOperador"
                            + " AND SC.Id_Sector = S.CodSector"
                            + " AND E.Id_Estado = P.CodEstado"
                            + " AND CP.Id_Ciudad = P.CodCiudad"
                            + " AND CP.CodDepartamento = DP.Id_Departamento"
                            + " AND CI.Id_Ciudad = I.CodCiudad"
                            + " AND CI.CodDepartamento = DI.Id_Departamento"
                            + " AND P.CodInstitucion = Id_Institucion"
                            + " AND P.inactivo = 0 ";


                //txtSQLTemp = " SELECT Id_Proyecto, NomProyecto, Sumario, S.NomSubSector, E.NomEstado, CP.NomCiudad, DP.NomDepartamento, NomUnidad, NomInstitucion, CI.NomCiudad +','+ DI.NomDepartamento as CiudadUnidad  " +
                //                     ",  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria" +
                //                     " , '' AS [Asesor] , '' AS [Lider], I.CodTipoInstitucion " +
                //                     " FROM Proyecto P, SubSector S, Estado E, Ciudad CP, Departamento DP, Sector SC, Institucion I, Ciudad CI, Departamento DI" +
                //                     " WHERE S.Id_SubSector = P.CodSubSector " + txtSQL +
                //                     " AND  S.codsector = sc.Id_Sector " +
                //                     " AND SC.Id_Sector = S.CodSector" +
                //                     " AND E.Id_Estado = P.CodEstado " +
                //                     " AND CP.Id_Ciudad = P.CodCiudad " +
                //                     " AND CP.CodDepartamento = DP.Id_Departamento" +
                //                     " AND CI.Id_Ciudad = I.CodCiudad " +
                //                     " AND CI.CodDepartamento = DI.Id_Departamento" +
                //                     " AND P.CodInstitucion = Id_Institucion" +
                //                     " AND P.inactivo=0";


                tieneAsesor = false;

            }

            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {
                txtSQLTemp = txtSQLTemp + " AND P.CodInstitucion = " + usuario.CodInstitucion;
            }

            if (usuario.CodOperador != null)
            {
                txtSQLTemp = txtSQLTemp + " and P.codOperador = " + usuario.CodOperador;
            }

            if (ddloperador.SelectedValue != "")
            {
                txtSQLTemp = txtSQLTemp + " and P.codOperador = " + ddloperador.SelectedValue;
            }

            rstProyecto = consultas.ObtenerDataTable(txtSQLTemp, "text");

            this.gv_busquedaavanzada.Columns[6].Visible = bolIncluye;

            if (CodContacto != "" || tieneAsesor)
            {
                this.gv_busquedaavanzada.Columns[8].Visible = true;
                this.gv_busquedaavanzada.Columns[9].Visible = true;
            }
            else
            {
                this.gv_busquedaavanzada.Columns[8].Visible = false;
                this.gv_busquedaavanzada.Columns[9].Visible = false;
            }

            if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema)
            {
                this.gv_busquedaavanzada.Columns[10].Visible = false;
            } //7
            //rstProyecto.Columns.Add("N_NomConvocatoria", typeof(System.String));
            //foreach (DataRow dr in rstProyecto.Rows)
            //{
            //    dr["NomCiudad"] = dr["NomCiudad"].ToString() + ", " + dr["NomDepartamento"].ToString();

            //    dr["NomUnidad"] = dr["NomUnidad"].ToString() + " (" + dr["NomInstitucion"].ToString() + ")";

            //    dr["N_NomConvocatoria"] = getNomConvocatoria(dr["CodConvocatoria"].ToString());
            //}

            HttpContext.Current.Session["tablaConsultas"] = rstProyecto;
            gv_busquedaavanzada.PageSize = 10;
            gv_busquedaavanzada.DataSource = rstProyecto;
            gv_busquedaavanzada.DataBind();

            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = true;
            lblResults.Text = "Se han encontrado " + rstProyecto.Rows.Count + " Planes de negocio de <br/> acuerdo a los criterios seleccionados";
            if (rstProyecto.Rows.Count == 0)
            {
                lbl_NumeroPaginas.Visible = false;
            }
            pnlInfoResultados.Visible = true;
            //}
            //catch
            //{
            //    Panel1.Visible = true;
            //    Panel2.Visible = true;
            //    Panel3.Visible = true;
            //    Panel4.Visible = false;
            //    pnlInfoResultados.Visible = false;
            //    lblResults.Text = "Se han encontrado planes de <br/> negocio buscando ''";
            //}

            AsesoresPanel.Visible = false;
        }

        /// <summary>
        /// Buscar por Emprendedores y/o asesores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tb_cedulaPalabra_onclick(object sender, EventArgs e)
        {
            if (tb_cedulaPalabra.Text.Trim().Length<3)
            {
                lblMensajeBusqEmprendedor.Visible = true;
            }
            else
            {

                DataTable rstContacto = new DataTable();
                string CodTipoContacto = RadioButtonList1.SelectedValue;

                txtSQL = " SELECT P.Id_Proyecto, P.NomProyecto, T.NomTipoIdentificacion, C.Identificacion"
                        + " , C.Id_contacto, C.Nombres, C.Apellidos, C.Email, R.Id_Rol, R.Nombre,"
                        + " nomUnidad + ' (' + nomInstitucion + ')' nomInstitucion, nomTipoInstitucion"
                        + " , (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria"
                        + " ,("
                        + " Select NomConvocatoria from Convocatoria where Id_Convocatoria ="
                        + " (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto)"
                        + " ) as N_NomConvocatoria"
                        + " FROM Contacto C with(nolock), TipoIdentificacion T with(nolock), Proyecto P with(nolock)"
                        + " , ProyectoContacto PC with(nolock), Rol R with(nolock), Institucion I with(nolock), TipoInstitucion TI with(nolock)"
                        + " WHERE C.CodTipoIdentificacion = T.Id_TipoIdentificacion"
                        + " AND PC.CodContacto = C.Id_Contacto"
                        + " AND C.CodInstitucion = I.Id_Institucion"
                        + " AND I.CodTipoInstitucion = TI.Id_TipoInstitucion"
                        + " AND PC.Inactivo = 0 AND P.inactivo = 0"
                        + " AND P.Id_Proyecto = PC.CodProyecto"
                        + " AND PC.CodRol = R.Id_Rol AND PC.codRol in (" + CodTipoContacto + ") AND ";

                //txtSQL = "SELECT P.Id_Proyecto, P.NomProyecto, T.NomTipoIdentificacion, C.Identificacion, C.Id_contacto, C.Nombres, C.Apellidos, C.Email, R.Id_Rol, R.Nombre, " +
                //    "nomUnidad+' ('+nomInstitucion+')' nomInstitucion, nomTipoInstitucion, (Select Max(CodConvocatoria)  from ConvocatoriaProyecto " +
                //    "where CodProyecto = P.Id_Proyecto) as CodConvocatoria FROM Contacto C, TipoIdentificacion T, Proyecto P, ProyectoContacto PC, Rol R, Institucion I, TipoInstitucion TI " +
                //    "WHERE C.CodTipoIdentificacion = T.Id_TipoIdentificacion AND PC.CodContacto = C.Id_Contacto AND C.CodInstitucion = I.Id_Institucion " +
                //    "AND I.CodTipoInstitucion = TI.Id_TipoInstitucion AND PC.Inactivo = 0 AND P.inactivo=0 AND P.Id_Proyecto = PC.CodProyecto " +
                //    "AND PC.CodRol = R.Id_Rol AND PC.codRol in (" + CodTipoContacto + ") AND ";

                long numero = 0;
                var canConvert = long.TryParse(tb_cedulaPalabra.Text.Trim(), out numero);
                if (canConvert)
                {
                    txtSQL += "C.Identificacion = " + numero;
                }
                else
                {
                    txtSQL += "(C.Nombres LIKE '%" + tb_cedulaPalabra.Text.Trim() + "%' OR C.Apellidos LIKE '%" + tb_cedulaPalabra.Text.Trim() + "%' )";
                }


                if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                { txtSQL = txtSQL + " AND P.CodInstitucion = " + usuario.CodInstitucion; }
                txtSQL = txtSQL.Insert(txtSQL.Length, '\n' + "ORDER BY P.Id_Proyecto");
                rstContacto = consultas.ObtenerDataTable(txtSQL, "text");
                conteo_grvMain = rstContacto.Rows.Count;

                if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema) { this.gv_busquedaporrol.Columns[9].Visible = false; }

                //rstContacto.Columns.Add("N_NomConvocatoria", typeof(System.String));
                //foreach (DataRow dr in rstContacto.Rows)
                //{
                //    dr["N_NomConvocatoria"] = getNomConvocatoria(dr["CodConvocatoria"].ToString());
                //}

                HttpContext.Current.Session["tablaConsultas_Rol"] = rstContacto;

                gv_busquedaporrol.DataSource = rstContacto;
                gv_busquedaporrol.DataBind();

                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = true;
                pnlInfoResultados.Visible = true;
                lblResults.Text = string.Format("Se han encontrado {0}  usuarios {1} buscando {2}", conteo_grvMain, '\n', tb_cedulaPalabra.Text);
                lbl_NumeroPaginas.Text = conteo_grvMain > 0 ? lbl_NumeroPaginas.Text.Replace("3976", (conteo_grvMain / 10).ToString()) : "página 0 de 0";
                AsesoresPanel.Visible = false;
            }
        }

        protected void ddl_departamento_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_departamento.SelectedIndex != 0)
            {
                var municipios = (from c in consultas.Db.Ciudad
                                  where c.CodDepartamento == Int32.Parse(ddl_departamento.SelectedValue)
                                  orderby c.NomCiudad ascending
                                  select new
                                  {
                                      Ciudad = c.NomCiudad,
                                      ID_Ciudad = c.Id_Ciudad
                                  });
                ddl_municipio.DataSource = municipios;
                ddl_municipio.DataTextField = "Ciudad";
                ddl_municipio.DataValueField = "ID_Ciudad";
                ddl_municipio.DataBind();
                ddl_municipio.Items.Insert(0, new ListItem("(Todos los Municipios)", ""));
            }
            else
            {
                ddl_municipio.SelectedIndex = 0;
            }
        }

        protected void lds_Consultarporrol_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int resultadoConversion;
            bool esnumero = Int32.TryParse(tb_cedulaPalabra.Text, out resultadoConversion);
            if (esnumero) { }
            else { resultadoConversion = 0; }

            var contactos = (from c in consultas.Db.Contacto
                             from t in consultas.Db.TipoIdentificacions
                             from p in consultas.Db.Proyecto
                             from pc in consultas.Db.ProyectoContactos
                             from r in consultas.Db.Rols
                             from i in consultas.Db.Institucions
                             from ti in consultas.Db.TipoInstitucions
                             orderby c.Id_Contacto
                             where (
                                 c.CodTipoIdentificacion == t.Id_TipoIdentificacion &
                                  pc.CodContacto == c.Id_Contacto &
                                  c.CodInstitucion == i.Id_Institucion &
                                  i.CodTipoInstitucion == ti.Id_TipoInstitucion
                                  & pc.Inactivo == false & p.Inactivo == false
                                  & p.Id_Proyecto == pc.CodProyecto
                                  & (pc.CodRol == tiporol1 || pc.CodRol == tiporol2 || pc.CodRol == tiporol3)
                                  & pc.CodRol == r.Id_Rol & (c.Nombres.Contains(tb_cedulaPalabra.Text) || c.Apellidos.Contains(tb_cedulaPalabra.Text) || c.Identificacion == resultadoConversion))

                             select new
                             {
                                 Id_Proyecto = p.Id_Proyecto,
                                 codRol = r.Id_Rol,
                                 Unidad = i.NomUnidad,
                                 codUnidad = i.Id_Institucion,
                                 NomProyecto = p.NomProyecto,
                                 NomTipoIdentificacion = t.NomTipoIdentificacion,
                                 Identificacion = c.Identificacion,
                                 Id_contacto = c.Id_Contacto,
                                 Nombres = c.Nombres + " " + c.Apellidos,
                                 Apellidos = c.Apellidos,
                                 Email = c.Email,
                                 Id_Rol = r.Id_Rol,
                                 Rol = r.Nombre,
                                 Nombre = r.Nombre,
                                 NomTipoInstitucion = ti.NomTipoInstitucion
                             });

            var conteo = (from c in consultas.Db.Contacto
                          from t in consultas.Db.TipoIdentificacions
                          from p in consultas.Db.Proyecto
                          from pc in consultas.Db.ProyectoContactos
                          from r in consultas.Db.Rols
                          from i in consultas.Db.Institucions
                          from ti in consultas.Db.TipoInstitucions
                          orderby c.Id_Contacto
                          where (
                              c.CodTipoIdentificacion == t.Id_TipoIdentificacion &
                              pc.CodContacto == c.Id_Contacto &
                              c.CodInstitucion == i.Id_Institucion &
                              i.CodTipoInstitucion == ti.Id_TipoInstitucion
                              & pc.Inactivo == false & p.Inactivo == false
                              & p.Id_Proyecto == pc.CodProyecto
                              & (pc.CodRol == tiporol1 || pc.CodRol == tiporol2 || pc.CodRol == tiporol3)
                              & pc.CodRol == r.Id_Rol & (c.Nombres.Contains(tb_cedulaPalabra.Text) || c.Apellidos.Contains(tb_cedulaPalabra.Text) || c.Identificacion == resultadoConversion))
                          select new
                          {
                              Id_Proyecto = p.Id_Proyecto,
                              codRol = r.Id_Rol
                          }).Count();

            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {
                contactos = contactos.Where(x => x.codUnidad == usuario.CodInstitucion);
            }

            double numero;
            bool validaelnumero;

            validaelnumero = Double.TryParse(tb_cedulaPalabra.Text, out numero);

            if (validaelnumero)
                contactos = contactos.Where(re => re.Identificacion == Convert.ToDouble(tb_cedulaPalabra.Text));
            else
            {
                contactos = contactos.Where(re => (re.Nombres + " " + re.Apellidos).Contains(tb_cedulaPalabra.Text));
            }

            e.Arguments.TotalRowCount = conteo;
            conteo_grvMain = conteo;
            contactos = contactos.Skip(grdMain.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);
            e.Result = contactos;

            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = true;
            lblResults.Text = "Se han encontrado " + conteo + " Planes de <br/> negocio buscando '" + tb_porPalabra.Text + "'";
            pnlInfoResultados.Visible = true;
        }

        /// <summary>
        /// LINQ de grvMain...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_Consultar_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var Consulta = from P in consultas.Consultar(usuario.IdContacto, usuario.CodGrupo, usuario.CodInstitucion, "palabra", tb_porPalabra.Text)
                               select P;

                e.Arguments.TotalRowCount = Consulta.Count();

                try
                { Consulta = Consulta.Skip(grdMain.PageIndex * PAGE_SIZE).Take(PAGE_SIZE).ToList(); }
                catch (Exception ex) { errorMessageDetail = ex.Message; }

                e.Result = Consulta;

                Panel3.Visible = false;
                Panel1.Visible = false;
                Panel2.Visible = true;
                pnlInfoResultados.Visible = true;
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        /// <summary>
        /// LINQ de la consulta avanzada...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_Consultaravanzada_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var consultaav1 = (from p in consultas.Db.Proyecto
                               from s in consultas.Db.SubSector
                               from es in consultas.Db.Estados
                               from cp in consultas.Db.Ciudad
                               from dp in consultas.Db.departamento
                               from sc in consultas.Db.Sector
                               from i in consultas.Db.Institucions
                               from pc in consultas.Db.ProyectoContactos
                               from c in consultas.Db.Contacto
                               from ci in consultas.Db.Ciudad
                               from di in consultas.Db.departamento
                               where (s.Id_SubSector == p.CodSubSector & pc.CodProyecto == p.Id_Proyecto & (pc.CodRol == 1 | pc.CodRol == 2)
                                     & pc.CodContacto == c.Id_Contacto
                                     & pc.Inactivo == false
                                     & p.CodSubSector == s.Id_SubSector
                                     & sc.Id_Sector == s.CodSector
                                     & es.Id_Estado == p.CodEstado
                                     & cp.Id_Ciudad == p.CodCiudad
                                     & cp.CodDepartamento == dp.Id_Departamento
                                     & ci.Id_Ciudad == i.CodCiudad
                                     & ci.CodDepartamento == di.Id_Departamento
                                     & p.CodInstitucion == i.Id_Institucion
                                     & p.Inactivo == false)
                               select new
                               {
                                   Id_Proyecto = p.Id_Proyecto,
                                   NomSubSector = s.NomSubSector,
                                   NomCiudad = cp.NomCiudad,
                                   NomProyecto = p.NomProyecto,
                                   CiudadUnidad = ci.NomCiudad + ',' + di.NomDepartamento,
                                   Nomunidad = i.NomUnidad,
                                   NomEstado = es.NomEstado,

                                   CodSector = sc.Id_Sector,
                                   CodEstados = es.Id_Estado,
                                   CodInstitucion = p.CodInstitucion,
                                   CodCiudad = p.CodCiudad,
                                   CodDepartamento = cp.CodDepartamento,
                                   CodContacto = pc.CodContacto,

                                   Sumario = p.Sumario,
                                   NomDepartamento = dp.NomDepartamento,
                                   NomInstitucion = i.NomInstitucion,
                                   Asesor = c.Nombres + ' ' + c.Apellidos,
                                   CodRol = pc.CodRol
                               }).Distinct();

            if (!string.IsNullOrEmpty(tb_codigo.Text))
            {
                consultaav1 = consultaav1.Where(x => x.Id_Proyecto == Int32.Parse(tb_codigo.Text)).Take(1);

            }

            if (!string.IsNullOrEmpty(lb_sector.SelectedValue) && !lb_sector.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodSector.ToString().Contains(lb_sector.SelectedValue));
            }

            if (!string.IsNullOrEmpty(lb_estados.SelectedValue) && !lb_estados.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodEstados.ToString().Contains(lb_estados.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ddl_municipio.SelectedValue) && !ddl_municipio.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodCiudad.ToString().Contains(ddl_municipio.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ddl_departamento.SelectedValue) && !ddl_departamento.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodDepartamento.ToString().Contains(ddl_departamento.SelectedValue));
            }

            if (!string.IsNullOrEmpty(lb_unidadEmprendimiento.SelectedValue) && !lb_unidadEmprendimiento.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodInstitucion.ToString().Contains(lb_unidadEmprendimiento.SelectedValue));
            }

            if (!string.IsNullOrEmpty(lb_asesores.SelectedValue) && !lb_asesores.SelectedValue.Equals(""))
            {
                consultaav1 = consultaav1.Where(x => x.CodContacto == Int32.Parse(lb_asesores.SelectedValue));
            }

            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {
                consultaav1.Where(y => usuario.CodInstitucion == y.CodInstitucion);
            }

            var lista = consultaav1.OrderBy(y => y.Id_Proyecto).ToList();

            int idpro = 0;

            try
            {
                foreach (var res in lista)
                {
                    if (idpro != res.Id_Proyecto)
                        idpro = res.Id_Proyecto;
                    else
                        lista.Remove(res);
                }
            }
            catch (InvalidOperationException) { }

            consultaav1 = lista.AsQueryable();

            e.Arguments.TotalRowCount = consultaav1.Count();
            consultaav1 = consultaav1.Skip(gv_busquedaavanzada.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);
            e.Result = consultaav1;

            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = true;
            pnlInfoResultados.Visible = true;
        }

        protected void btnbuscarcedulaopalabra_Click(object sender, EventArgs e)
        {
            if (tctcedulaopalabra.Text.Length<3)
            {
                lblMensajeBusqJefeUnidad.Visible = true;
            }
            else
            {
                
                var sesult = (from ju in consultas.Db.MD_Consultarjefe()                              
                              select new
                              {
                                  ju.NomTipoIdentificacion,
                                  ju.Identificacion,
                                  Nombre = ju.Nombres + " " + ju.Apellidos,
                                  ju.Email,
                                  Unidad = ju.NomUnidad + " (" + ju.NomInstitucion + ")",
                                  ciudad = ju.NomCiudad + " (" + ju.NomDepartamento + ")"
                              });

                if (!string.IsNullOrEmpty(tctcedulaopalabra.Text))
                {
                    Int32 number1 = 0;
                    bool canConvert = Int32.TryParse(tctcedulaopalabra.Text, out number1);

                    if (canConvert)
                        sesult = sesult.Where(ju => ju.Identificacion == number1);
                    else
                        sesult = sesult.Where(ju => ju.Nombre.ToString().ToLower().Contains(tctcedulaopalabra.Text.ToLower()));
                }

                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                pnlpanel5.Visible = true;
                pnlInfoResultados.Visible = true;
                var rfv = 1000;
                HttpContext.Current.Session["result_contacto"] = sesult;
                gvcontacto.DataSource = sesult;
                gvcontacto.DataBind();
                gvcontacto.PageSize = rfv;
                AsesoresPanel.Visible = false;
            }
        }

        protected void btnbuscarcedulaopalabra0_Click(object sender, EventArgs e)
        {
            if (txtpalabra.Text.Length<3)
            {
                lblMensajeBusqUnidadEmp.Visible = true;
            }
            else
            {

                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                pnlpanel5.Visible = false;
                pnlpanel6.Visible = true;
                pnlInfoResultados.Visible = true;

                //var result = from ue in consultas.Db.MD_Consultar_UnidadesdeEmprendimiento(byte.Parse(radiobutonunidades.SelectedValue), txtpalabra.Text)
                //             select new
                //             {
                //                 ue.NomTipoInstitucion,
                //                 unidad = ue.NomUnidad + " (" + ue.NomTipoInstitucion + ")",
                //                 ciudad = ue.NomCiudad + " (" + ue.NomDepartamento + ")",
                //                 Nombre = ue.Nombres + " " + ue.Apellidos,
                //                 ue.Email,
                //                 ue.Telefono
                //             };

                int[] ids = new int[3];
                var tipoInst = radiobutonunidades.SelectedValue;
                switch (tipoInst)
                {
                    case "1":
                        ids[0] = 1;
                        ids[1] = 0;
                        ids[2] = 0;
                        break;
                    case "3":
                        ids[0] = 3;
                        ids[1] = 0;
                        ids[2] = 0;
                        break;
                    case "2":
                        ids[0] = 1;
                        ids[1] = 2;
                        ids[2] = 3;
                        break;
                }

                var result = (from ti in consultas.Db.TipoInstitucions
                              join i in consultas.Db.Institucions on ti.Id_TipoInstitucion equals i.CodTipoInstitucion
                              join c in consultas.Db.Ciudad on i.CodCiudad equals c.Id_Ciudad
                              join d in consultas.Db.departamento on c.CodDepartamento equals d.Id_Departamento
                              join ic in consultas.Db.InstitucionContacto on i.Id_Institucion equals ic.CodInstitucion
                              join co in consultas.Db.Contacto on ic.CodContacto equals co.Id_Contacto
                              where ic.FechaFin == null && i.Inactivo == false && ids.Contains(i.CodTipoInstitucion) &&
                              (i.NomInstitucion.Contains(txtpalabra.Text.Trim()) || i.NomUnidad.Contains(txtpalabra.Text.Trim()))
                              select new
                              {
                                  ti.NomTipoInstitucion,
                                  unidad = i.NomUnidad + " (" + ti.Id_TipoInstitucion + ")",
                                  ciudad = c.NomCiudad + " (" + d.NomDepartamento + ")",
                                  nombre = co.Nombres + " " + co.Apellidos,
                                  co.Email,
                                  co.Telefono
                              });

                HttpContext.Current.Session["data_gridview1"] = result;
                GridView1.DataSource = result;
                GridView1.DataBind();
                GridView1.AllowPaging = true;
                GridView1.PageSize = 10;
                var cant = result.Count();
                lblResults.Text = "Se han encontrado " + cant + "  unidades de emprendimiento <br/> buscando ''" + txtpalabra.Text + "''";
                txtpalabra.Text = "";
                lbl_NumeroPaginas.Text = "Pagina 1 de " + GridView1.PageCount;
                AsesoresPanel.Visible = false;
                UpdatePanel1.Visible = false;
            }
        }

        protected void lnk_Limpiar_Click(object sender, EventArgs e)
        { tb_asesor.Text = ""; hdf_CodContacto.Value = ""; /*lb_asesores.Items.Clear(); lb_asesores.Visible = false;*/ }

        protected void gv_busquedaavanzada_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("plan"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx");
            }
            if (e.CommandName.Equals("empresa"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();

                HttpContext.Current.Session["CodEmpresa"] = buscarCodEmpresa(Convert.ToInt32(e.CommandArgument.ToString()));

                Response.Redirect("~/FONADE/interventoria/SeguimientoFrameSet.aspx");
            }
        }

        protected void grdMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("plan"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx");
            }
            if (e.CommandName.Equals("empresa"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();

                HttpContext.Current.Session["CodEmpresa"] = buscarCodEmpresa(Convert.ToInt32(e.CommandArgument.ToString()));

                Response.Redirect("~/FONADE/interventoria/SeguimientoFrameSet.aspx");                               
            }
        }
        string _cadenaConex = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private int buscarCodEmpresa(int _codproyecto)
        {
            int codempresa = 0;

            using (FonadeDBDataContext db= new FonadeDBDataContext(_cadenaConex))
            {
                codempresa = (from e in db.Empresas
                              where e.codproyecto == _codproyecto
                              select e.id_empresa).FirstOrDefault();
            }

            return codempresa;
        }

        protected void gv_busquedaporrol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("planes"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx");
            }
            else
            {
                if (e.CommandArgument.Equals("email"))
                {
                    HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx");
                }
            }
        }

        /// <summary>
        /// Paginación del GridView de búsqueda avanzada...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaavanzada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas"] as DataTable;

            if (dt != null)
            {
                gv_busquedaavanzada.PageIndex = e.NewPageIndex;
                gv_busquedaavanzada.DataSource = dt;
                gv_busquedaavanzada.DataBind();
            }
        }

        /// <summary>
        /// Obtener el nombre de la convocatoria "tal como se hace en FONADE clásico".
        /// </summary>
        /// <param name="pCodConvocatoria">Código de la convocatoria.</param>
        /// <returns>string</returns>
        private string getNomConvocatoria(string pCodConvocatoria)
        {
            string lSentencia;
            DataTable rs = new DataTable();

            try
            {
                lSentencia = "Select NomConvocatoria from Convocatoria where Id_Convocatoria = '" + pCodConvocatoria + "'";
                rs = consultas.ObtenerDataTable(lSentencia, "text");

                if (rs.Rows.Count > 0) { return rs.Rows[0]["NomConvocatoria"].ToString(); } else { return ""; }
            }
            catch { return ""; }
        }

        /// <summary>
        /// Paginación de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas_palabra"] as DataTable;

            if (dt != null)
            {
                grdMain.PageIndex = e.NewPageIndex;
                grdMain.DataSource = dt;
                grdMain.DataBind();
            }
        }

        /// <summary>
        /// Se debe enviar la información de la tabla en uan variable se sesión
        /// para poder sortearlo.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            var sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;

                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        /// <summary>
        /// Sortear la grilla "grdMain".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMain_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas_palabra"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                grdMain.DataSource = dt;
                grdMain.DataBind();
            }
        }

        /// <summary>
        /// Paginación de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaporrol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas_Rol"] as DataTable;

            if (dt != null)
            {
                gv_busquedaporrol.PageIndex = e.NewPageIndex;
                gv_busquedaporrol.DataSource = dt;
                gv_busquedaporrol.DataBind();
            }
        }

        protected void numRegPorPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializar variables.
            var dt = new DataTable();
            Int32 PlanesPorPagina = 10;

            try
            {
                //Convertir el valor para aplicarlo a la grilla visible.
                PlanesPorPagina = Int32.Parse(numRegPorPagina.SelectedValue);

                if (grdMain.Visible)
                {
                    dt = HttpContext.Current.Session["tablaConsultas_palabra"] as DataTable;

                    if (dt != null)
                    {
                        grdMain.PageSize = PlanesPorPagina;
                        grdMain.DataSource = dt;
                        grdMain.DataBind();
                    }
                }

                if (gv_busquedaporrol.Visible)
                {
                    dt = HttpContext.Current.Session["tablaConsultas_Rol"] as DataTable;

                    if (dt != null)
                    {
                        gv_busquedaporrol.PageSize = PlanesPorPagina;
                        gv_busquedaporrol.DataSource = dt;
                        gv_busquedaporrol.DataBind();
                    }
                }

                if (gv_busquedaavanzada.Visible)
                {
                    dt = HttpContext.Current.Session["tablaConsultas"] as DataTable;

                    if (dt != null)
                    {
                        gv_busquedaavanzada.PageSize = PlanesPorPagina;
                        gv_busquedaavanzada.DataSource = dt;
                        gv_busquedaavanzada.DataBind();
                    }
                }
            }
            catch { }

            if (HttpContext.Current.Session["data_gridview1"] != null)
            {
                var dtt = HttpContext.Current.Session["data_gridview1"];
                GridView1.PageSize = int.Parse(numRegPorPagina.SelectedValue);
                GridView1.DataSource = dtt;
                GridView1.DataBind();
                GridView1.PageIndex = 0;
                lbl_NumeroPaginas.Text = "Pagina " + (GridView1.PageIndex + 1) + " de " + GridView1.PageCount;
                UpdatePanel1.Visible = false;
            }
        }

        /// <summary>
        /// Mostrar el número de páginas de la grilla "grdMain".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Calculate the current page number.
            int currentPage = grdMain.PageIndex + 1;

            // Update the Label control with the current page information.
            lbl_NumeroPaginas.Text = "Página " + currentPage.ToString() + " de " + grdMain.PageCount.ToString();
        }

        /// <summary>
        /// Mostrar el número de páginas de la grilla "gv_busquedaporrol".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaporrol_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Calculate the current page number.
            int currentPage = gv_busquedaporrol.PageIndex + 1;

            // Update the Label control with the current page information.
            lbl_NumeroPaginas.Text = "Página " + currentPage.ToString() + " de " + gv_busquedaporrol.PageCount.ToString();
        }

        /// <summary>
        /// Mostrar el número de páginas de la grilla "gv_busquedaavanzada".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaavanzada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Calculate the current page number.
            int currentPage = gv_busquedaavanzada.PageIndex + 1;

            // Update the Label control with the current page information.
            lbl_NumeroPaginas.Text = "Página " + currentPage.ToString() + " de " + gv_busquedaavanzada.PageCount.ToString();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                {
                    var nomProyecto = (LinkButton)e.Row.FindControl("btnidproyecto");
                    var idTipoUnidad = int.Parse(gv_busquedaavanzada.DataKeys[e.Row.RowIndex].Values[1].ToString());
                    if (idTipoUnidad == 3)
                    {
                        nomProyecto.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Ordernas los resultados ascendente o descendentemente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcontacto_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["result_contacto"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvcontacto.DataSource = dt;
                gvcontacto.DataBind();
            }
        }

        /// <summary>
        /// Ordernas los resultados ascendente o descendentemente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["data_gridview1"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// Sortear la grilla "gv_busquedaporrol".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaporrol_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas_Rol"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_busquedaporrol.DataSource = dt;
                gv_busquedaporrol.DataBind();
            }
        }

        /// <summary>
        /// Sortear la grilla "gv_busquedaavanzada".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_busquedaavanzada_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaConsultas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_busquedaavanzada.DataSource = dt;
                gv_busquedaavanzada.DataBind();
            }
        }

        /// <summary>
        /// Retorna una lista de asesores
        /// los bindea en un ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_asesores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            IEnumerable<ListAsesores> result = (consultas.Db.ExecuteQuery<ListAsesores>("select c.Id_Contacto, str(c.Identificacion) + ' - ' + c.Nombres + ' ' + c.Apellidos Nombre from Contacto c, GrupoContacto gc where c.Id_Contacto = gc.CodContacto and gc.CodGrupo = " + Constantes.CONST_Asesor + " order by c.Nombres, c.Apellidos"));

            if (!string.IsNullOrEmpty(tb_asesor.Text))
            {
                result = result.Where(x => x.Nombre.ToLower().Contains(tb_asesor.Text.ToLower()));
            }

            e.Result = result.ToList();
        }

        protected void tb_asesor_TextChanged(object sender, EventArgs e)
        {
            lb_asesores.Visible = true;
            lb_asesores.DataBind();
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            ImgBtn_MPE.Hide();
        }

        private void LimpiarControles()
        {
            tb_porPalabra.Text = "";
            tb_codigo.Text = "";
            ddl_departamento.SelectedIndex = 0;
            ddl_municipio.Items.Clear();
            lb_sector.SelectedIndex = 0;
            lb_estados.SelectedIndex = 0;
            lb_unidadEmprendimiento.SelectedIndex = 0;
            txtBuscarAsesor.Text = "";
            AsesorNmLabel.Text = "";
            CheckBox1.Checked = false;
        }

        protected void gvcontacto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            var dtt = HttpContext.Current.Session["data_gridview1"];
            GridView1.PageSize = int.Parse(numRegPorPagina.SelectedValue);
            GridView1.DataSource = dtt;
            GridView1.DataBind();
            UpdatePanel1.Visible = false;
            lbl_NumeroPaginas.Text = "Pagina " + (e.NewPageIndex + 1) + " de " + GridView1.PageCount;
        }

        protected void btn_AbrirEmpresa_Click(object sender, EventArgs e)
        {
            MostrarPlanOEmpresa = "empresa";
            BuscarProyecto();
        }
    }

    class ListAsesores
    {
        private int _Id_Contacto;

        public int Id_Contacto
        {
            get { return _Id_Contacto; }
            set { _Id_Contacto = value; }
        }

        private string _Nombre;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
    }
}

