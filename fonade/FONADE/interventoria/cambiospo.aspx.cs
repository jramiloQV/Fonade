using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

using Fonade.Negocio;

namespace Fonade.FONADE.interventoria
{
    public partial class cambiospo : Negocio.Base_Page
    {
        public Negocio.Interventoria.CambiosPONegocio ObjCambiosPo = new Negocio.Interventoria.CambiosPONegocio();

        public string CodigoProyecto { get; set; }

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "FONDO EMPRENDER - CAMBIOS A PLANES OPERATIVOS";

            if (!IsPostBack)
            {
                //llenar("");
                //CargarCambios_PO("Seleccione...");
                CargarCambios_PO("");
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/05/2014.
        /// Cargar las grillas acerca de los cambios hechos.
        /// Se usa como reemplazo del método "llenar".
        /// </summary>
        /// <param name="opcion">Valor que determina si carga la información Modificada, Eliminada o Adicionada.</param>
        private void CargarCambios_PO(string opcion, string busquedaPorProyecto = "", string busquedaPorInterventor = "")
        {
            //Inicializar variables.
            String sqlConsulta = "";
            DataTable tabla_PO = new DataTable();
            DataTable tabla_Nomina = new DataTable();
            DataTable tabla_Produccion = new DataTable();
            DataTable tabla_Ventas = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    #region Plan Operativo.

                    if (opcion.Trim() != "")
                    {
                        //Cargar la información de acuerdo a la selección del DropDownList.
                        sqlConsulta = " SELECT distinct ProyectoActividadPOInterventorTMP.Id_Actividad, ProyectoActividadPOInterventorTMP.NomActividad, " +
                                  " ProyectoActividadPOInterventorTMP.CodProyecto, ProyectoActividadPOInterventorTMP.Item,  " +
                                  " ProyectoActividadPOInterventorTMP.Tarea, " +
                                  " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos" +
                                  " , ProyectoActividadPOInterventorTMP.Id " +
                                  " FROM EmpresaInterventor  INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                  " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                  " INNER JOIN ProyectoActividadPOInterventorTMP ON Empresa.codproyecto = ProyectoActividadPOInterventorTMP.CodProyecto " +
                                  " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                  " WHERE (ProyectoActividadPOInterventorTMP.ChequeoGerente IS NULL) " +
                                  " AND (ProyectoActividadPOInterventorTMP.ChequeoCoordinador IS NULL) " +
                                  " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                  " AND (EmpresaInterventor.Inactivo = 0) " +
                                  " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") " +
                                  " AND (Tarea = '" + opcion + "')" +
                                  " and (Contacto.codOperador = " +usuario.CodOperador+ ") " +
                                  busquedaPorProyecto +
                                  busquedaPorInterventor +
                                  " ORDER BY ProyectoActividadPOInterventorTMP.Id ASC ";
                        //" ORDER BY ProyectoActividadPOInterventorTMP.NomActividad ASC ";

                        //Asignar resultados a variable DataTable
                        tabla_PO = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_PO.Columns.IndexOf("editable") == -1) tabla_PO.Columns.Add(new DataColumn("editable"));
                        tabla_PO.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_PO.AcceptChanges();
                        //Bindear la grilla.
                        gvactividadesplanoperativo.DataSource = tabla_PO;
                        gvactividadesplanoperativo.DataBind();
                    }
                    else
                    {
                        //Cargar la grilla sin filtros adicionales.
                        sqlConsulta = " SELECT distinct ProyectoActividadPOInterventorTMP.Id_Actividad, ProyectoActividadPOInterventorTMP.NomActividad, " +
                                  " ProyectoActividadPOInterventorTMP.CodProyecto, ProyectoActividadPOInterventorTMP.Item,  " +
                                  " ProyectoActividadPOInterventorTMP.Tarea, " +
                                  " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                  " , ProyectoActividadPOInterventorTMP.Id" +
                                  " FROM EmpresaInterventor  INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                  " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                  " INNER JOIN ProyectoActividadPOInterventorTMP ON Empresa.codproyecto = ProyectoActividadPOInterventorTMP.CodProyecto " +
                                  " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                  " WHERE (ProyectoActividadPOInterventorTMP.ChequeoGerente IS NULL) " +
                                  " AND (ProyectoActividadPOInterventorTMP.ChequeoCoordinador IS NULL) " +
                                  " AND (EmpresaInterventor.Rol  In( " + Constantes.CONST_RolInterventorLider + "," + Constantes.CONST_RolInterventor + ")) " +
                                  " AND (EmpresaInterventor.Inactivo = 0) " +
                                  " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") AND Id_Actividad <> 0 " +
                                  " and (Contacto.codOperador = "+usuario.CodOperador+") " +
                                  busquedaPorProyecto +
                                  busquedaPorInterventor +
                                  " ORDER BY ProyectoActividadPOInterventorTMP.Id ASC ";
                        //" ORDER BY ProyectoActividadPOInterventorTMP.NomActividad ASC ";

                        //Asignar resultados a variable DataTable
                        tabla_PO = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_PO.Columns.IndexOf("editable") == -1) tabla_PO.Columns.Add(new DataColumn("editable"));
                        tabla_PO.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_PO.AcceptChanges();
                        //Bindear la grilla.
                        gvactividadesplanoperativo.DataSource = tabla_PO;
                        gvactividadesplanoperativo.DataBind();
                    }

                    #endregion

                    #region Nómina.
                    if (opcion.Trim() != "")
                    {
                        //Cargar la información de acuerdo a la selección del DropDownList.
                        sqlConsulta = " SELECT distinct InterventorNominaTMP.Id_Nomina, InterventorNominaTMP.Cargo, " +
                                      " InterventorNominaTMP.CodProyecto, InterventorNominaTMP.Tipo, InterventorNominaTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor  INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorNominaTMP ON Empresa.codproyecto = InterventorNominaTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorNominaTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorNominaTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol  In( " + Constantes.CONST_RolInterventorLider + "," + Constantes.CONST_RolInterventor + ")) " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " and (Contacto.codOperador = "+usuario.CodOperador+") " +
                                      " AND (Tarea = '" + opcion + "')" +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorNominaTMP.Id_Nomina ";

                        //Asignar resultados a variable DataTable
                        tabla_Nomina = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Nomina.Columns.IndexOf("editable") == -1) tabla_Nomina.Columns.Add(new DataColumn("editable"));
                        tabla_Nomina.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Nomina.AcceptChanges();
                        //Bindear la grilla.
                        gvcargosnomina.DataSource = tabla_Nomina;
                        gvcargosnomina.DataBind();
                    }
                    else
                    {
                        //Cargar la grilla sin filtros adicionales.
                        sqlConsulta = " SELECT distinct InterventorNominaTMP.Id_Nomina, InterventorNominaTMP.Cargo, " +
                                      " InterventorNominaTMP.CodProyecto, InterventorNominaTMP.Tipo, InterventorNominaTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor  INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorNominaTMP ON Empresa.codproyecto = InterventorNominaTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorNominaTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorNominaTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol  In( " + Constantes.CONST_RolInterventorLider + "," + Constantes.CONST_RolInterventor + ")) " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorNominaTMP.Id_Nomina ";

                        //Asignar resultados a variable DataTable
                        tabla_Nomina = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Nomina.Columns.IndexOf("editable") == -1) tabla_Nomina.Columns.Add(new DataColumn("editable"));
                        tabla_Nomina.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Nomina.AcceptChanges();
                        //Bindear la grilla.
                        gvcargosnomina.DataSource = tabla_Nomina;
                        gvcargosnomina.DataBind();
                    }

                    #endregion

                    #region Producción.

                    if (opcion.Trim() != "")
                    {
                        //Cargar la información de acuerdo a la selección del DropDownList.
                        sqlConsulta = " SELECT distinct InterventorProduccionTMP.Id_Produccion, InterventorProduccionTMP.NomProducto, " +
                                      " InterventorProduccionTMP.CodProyecto, InterventorProduccionTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorProduccionTMP ON Empresa.codproyecto = InterventorProduccionTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorProduccionTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorProduccionTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol  In( " + Constantes.CONST_RolInterventorLider + "," + Constantes.CONST_RolInterventor + ")) " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " AND (Tarea = '" + opcion + "')" +
                                      " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorProduccionTMP.Id_Produccion ";

                        //Asignar resultados a variable DataTable
                        tabla_Produccion = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Produccion.Columns.IndexOf("editable") == -1) tabla_Produccion.Columns.Add(new DataColumn("editable"));
                        tabla_Produccion.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Produccion.AcceptChanges();
                        //Bindear la grilla.
                        gvproductosproduccion.DataSource = tabla_Produccion;
                        gvproductosproduccion.DataBind();
                    }
                    else
                    {
                        //Cargar la información sin filtros adicionales.
                        sqlConsulta = " SELECT distinct InterventorProduccionTMP.Id_Produccion, InterventorProduccionTMP.NomProducto, " +
                                      " InterventorProduccionTMP.CodProyecto, InterventorProduccionTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorProduccionTMP ON Empresa.codproyecto = InterventorProduccionTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorProduccionTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorProduccionTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol  In( " + Constantes.CONST_RolInterventorLider + "," + Constantes.CONST_RolInterventor + ")) " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorProduccionTMP.Id_Produccion ";

                        //Asignar resultados a variable DataTable
                        tabla_Produccion = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Produccion.Columns.IndexOf("editable") == -1) tabla_Produccion.Columns.Add(new DataColumn("editable"));
                        tabla_Produccion.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Produccion.AcceptChanges();
                        //Bindear la grilla.
                        gvproductosproduccion.DataSource = tabla_Produccion;
                        gvproductosproduccion.DataBind();
                    }

                    #endregion

                    #region Ventas.

                    if (opcion.Trim() != "")
                    {
                        //Cargar la información de acuerdo a la selección del DropDownList.
                        sqlConsulta = " SELECT distinct InterventorVentasTMP.Id_Ventas, InterventorVentasTMP.NomProducto, " +
                                      " InterventorVentasTMP.CodProyecto, InterventorVentasTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorVentasTMP ON Empresa.codproyecto = InterventorVentasTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorVentasTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorVentasTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " AND (Tarea = '" + opcion + "')" +
                                      " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorVentasTMP.Id_Ventas ";

                        //Asignar resultados a variable DataTable
                        tabla_Ventas = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Ventas.Columns.IndexOf("editable") == -1) tabla_Ventas.Columns.Add(new DataColumn("editable"));
                        tabla_Ventas.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Ventas.AcceptChanges();
                        //Bindear la grilla.
                        gvproductosventas.DataSource = tabla_Ventas;
                        gvproductosventas.DataBind();
                    }
                    else
                    {
                        //Cargar la información sin filtros adicionales.
                        sqlConsulta = " SELECT distinct InterventorVentasTMP.Id_Ventas, InterventorVentasTMP.NomProducto, " +
                                      " InterventorVentasTMP.CodProyecto, InterventorVentasTMP.Tarea, " +
                                      " Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos " +
                                      " FROM EmpresaInterventor INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                      " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                      " INNER JOIN InterventorVentasTMP ON Empresa.codproyecto = InterventorVentasTMP.CodProyecto " +
                                      " INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto " +
                                      " WHERE (InterventorVentasTMP.ChequeoGerente IS NULL) " +
                                      " AND (InterventorVentasTMP.ChequeoCoordinador IS NULL) " +
                                      " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                      " AND (EmpresaInterventor.Inactivo = 0) " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                      " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                                      busquedaPorProyecto +
                                      busquedaPorInterventor +
                                      " ORDER BY InterventorVentasTMP.Id_Ventas ";

                        //Asignar resultados a variable DataTable
                        tabla_Ventas = consultas.ObtenerDataTable(sqlConsulta, "text");
                        var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                        if (tabla_Ventas.Columns.IndexOf("editable") == -1) tabla_Ventas.Columns.Add(new DataColumn("editable"));
                        tabla_Ventas.Columns["editable"].Expression = string.Format("{0}", editable);
                        tabla_Ventas.AcceptChanges();
                        //Bindear la grilla.
                        gvproductosventas.DataSource = tabla_Ventas;
                        gvproductosventas.DataBind();
                    }
                    Bloquear_columnas_chequeo();
                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
                    var cxz = Constantes.CONST_RolInterventorLider;
                    var qry = string.Empty;   
                    var dtQry = new DataTable();
                    #region ACTIVIDADES DEL PLAN OPERATIVO
                        qry = "SELECT distinct ProyectoActividadPOInterventorTMP.Id_Actividad, ProyectoActividadPOInterventorTMP.NomActividad, " +   "ProyectoActividadPOInterventorTMP.CodProyecto, " + 
                          "ProyectoActividadPOInterventorTMP.Item,  ProyectoActividadPOInterventorTMP.Tarea, Empresa.razonsocial, " + 
                          "Contacto.Nombres, Contacto.Apellidos " +
                          ", ProyectoActividadPOInterventorTMP.Id " + 
                          "FROM ProyectoActividadPOInterventorTMP  " +
                          "INNER JOIN Empresa ON ProyectoActividadPOInterventorTMP.CodProyecto = Empresa.codproyecto  " + 
                          "INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa  " + 
                          "INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto  " + 
                          "WHERE (ProyectoActividadPOInterventorTMP.ChequeoGerente IS NULL)  " + 
                          "AND (ProyectoActividadPOInterventorTMP.ChequeoCoordinador = 1)  " + 
                          "AND (EmpresaInterventor.Inactivo = 0) {0}" + 
                          "AND (EmpresaInterventor.Rol IN({1},6)) " +
                          " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                          busquedaPorProyecto +
                          busquedaPorInterventor +
                          "order by ProyectoActividadPOInterventorTMP.Id asc";
                        qry = string.Format(qry, (editable) ? string.Format("and ProyectoActividadPOInterventorTMP.Tarea='{0}' ", opcion) : string.Empty, cxz);
                        dtQry.Load(new Clases.genericQueries().executeQueryReader(qry), LoadOption.OverwriteChanges);
                        dtQry.Columns.Add(new DataColumn("editable"));
                        dtQry.Columns["editable"].Expression = string.Format("{0}",editable);
                        gvactividadesplanoperativo.DataSource = dtQry;
                        gvactividadesplanoperativo.DataBind();
                    #endregion

                    #region CARGOS DE NOMINA
                        dtQry = new DataTable();
                        qry = "SELECT distinct InterventorNominaTMP.Id_Nomina, InterventorNominaTMP.Cargo,  InterventorNominaTMP.CodProyecto, InterventorNominaTMP.Tipo,  " + 
                        "InterventorNominaTMP.Tarea, Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos  " +
                        "FROM InterventorNominaTMP  " +
                        " INNER JOIN Empresa ON InterventorNominaTMP.CodProyecto = Empresa.codproyecto  " +
                        " INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa  " +
                        "INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto  " + 
                        "WHERE (InterventorNominaTMP.ChequeoGerente IS NULL)  " + 
                        "AND (InterventorNominaTMP.ChequeoCoordinador = 1)  " + 
                        "AND (EmpresaInterventor.Inactivo = 0) {0} " +
                        "AND (EmpresaInterventor.Rol IN({1},6)) " +
                        " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                        busquedaPorProyecto +
                        busquedaPorInterventor +
                        "order by InterventorNominaTMP.Id_Nomina desc";
                        qry = string.Format(qry, (editable) ? string.Format("and InterventorNominaTMP.Tarea='{0}' " , opcion) : string.Empty, cxz);
                        dtQry.Load(new Clases.genericQueries().executeQueryReader(qry), LoadOption.OverwriteChanges);
                        dtQry.Columns.Add(new DataColumn("editable"));
                        dtQry.Columns["editable"].Expression = string.Format("{0}", editable);
                        gvcargosnomina.DataSource = dtQry;
                        gvcargosnomina.DataBind();
                    #endregion

                    #region PRODUCTOS EN PRODUCCIÓN
                        dtQry = new DataTable();
                        qry = "SELECT distinct InterventorProduccionTMP.Id_Produccion, InterventorProduccionTMP.NomProducto,  InterventorProduccionTMP.CodProyecto,  " + 
                        "InterventorProduccionTMP.Tarea, Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos  " +
                        "FROM InterventorProduccionTMP  " +
                        "INNER JOIN Empresa ON InterventorProduccionTMP.CodProyecto = Empresa.codproyecto  " + 
                        "INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa  " +
                        "INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto  " + 
                        "WHERE (InterventorProduccionTMP.ChequeoGerente IS NULL)  " +
                        "AND (InterventorProduccionTMP.ChequeoCoordinador = 1)  " +
                        "AND (EmpresaInterventor.Inactivo = 0)  {0}" +
                        "AND (EmpresaInterventor.Rol IN({1},6)) " +
                        " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                        busquedaPorProyecto +
                        busquedaPorInterventor +
                        "order by InterventorProduccionTMP.Id_Produccion ";
                        qry = string.Format(qry, (editable) ? string.Format("and InterventorProduccionTMP.Tarea='{0}' ", opcion) : string.Empty, cxz);
                        dtQry.Load(new Clases.genericQueries().executeQueryReader(qry), LoadOption.OverwriteChanges);
                        dtQry.Columns.Add(new DataColumn("editable"));
                        dtQry.Columns["editable"].Expression = string.Format("{0}", editable);
                        gvproductosproduccion.DataSource = dtQry;
                        gvproductosproduccion.DataBind();
                    #endregion

                    #region PRODUCTOS EN VENTAS
                        dtQry = new DataTable();
                        qry = "SELECT distinct InterventorVentasTMP.Id_Ventas, InterventorVentasTMP.NomProducto,  InterventorVentasTMP.CodProyecto,  " + 
                        "InterventorVentasTMP.Tarea, Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos  " + 
                        "FROM InterventorVentasTMP  " + 
                        "INNER JOIN Empresa ON InterventorVentasTMP.CodProyecto = Empresa.codproyecto  " + 
                        "INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa  " + 
                        "INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto  " + 
                        "WHERE (InterventorVentasTMP.ChequeoGerente IS NULL)  " + 
                        "AND (InterventorVentasTMP.ChequeoCoordinador = 1)  " +
                        "AND (EmpresaInterventor.Inactivo = 0) {0} " +
                        "AND (EmpresaInterventor.Rol IN({1},6)) " +
                        " and (Contacto.codOperador = " + usuario.CodOperador + ") " +
                        busquedaPorProyecto +
                        busquedaPorInterventor +
                        "order by InterventorVentasTMP.Id_Ventas ";
                        qry = string.Format(qry, (editable) ? string.Format("and InterventorVentasTMP.Tarea='{0}' ", opcion) : string.Empty, cxz);
                        dtQry.Load(new Clases.genericQueries().executeQueryReader(qry), LoadOption.OverwriteChanges);
                        dtQry.Columns.Add(new DataColumn("editable"));
                        dtQry.Columns["editable"].Expression = string.Format("{0}", editable);
                        gvproductosventas.DataSource = dtQry;
                        gvproductosventas.DataBind();
                    #endregion
                }
            }
            catch(Exception)
            {
                //throw;
            }
        }

         public void Bloquear_columnas_chequeo()
         {

                foreach (GridViewRow fila in gvactividadesplanoperativo.Rows)
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                    if (gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[0].ToString() == "Seleccione...")
                    {
                        box.Enabled = false;
                    }
                    else
                    {
                        box.Enabled = true;
                    }
                }
                foreach (GridViewRow fila in gvcargosnomina.Rows)
                {
                    CheckBox box = (CheckBox)fila.FindControl("chcknomina");
                    if (gvcargosnomina.DataKeys[fila.RowIndex].Values[0].ToString() == "Seleccione...")
                    {
                        box.Enabled = false;
                    }
                    else
                    {
                        box.Enabled = true;
                    }
                }

                foreach (GridViewRow fila in gvproductosproduccion.Rows)
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckproduccion");
                    if (gvproductosproduccion.DataKeys[fila.RowIndex].Values[0].ToString() == "Seleccione...")
                    {
                        box.Enabled = false;
                    }
                    else
                    {
                        box.Enabled = true;
                    }
                }

                foreach (GridViewRow fila in gvproductosventas.Rows)
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckventas");
                    if (gvproductosventas.DataKeys[fila.RowIndex].Values[0].ToString() == "Seleccione...")
                    {
                        box.Enabled = false;
                    }
                    else
                    {
                        box.Enabled = true;
                    }
                }
         }

        private void llenar(string opc)
        {
            //var gvoperativo = consultas.Db.MD_CambiosPlanOperativo(usuario.IdContacto, usuario.CodGrupo, "OPERATIVO").OrderBy(po => po.NomActividad);

            //var gvnomina = consultas.Db.MD_CambiosPlanOperativo(usuario.IdContacto, usuario.CodGrupo, "NOMINA").OrderBy(no => no.Tarea);

            //var gvproduccion = consultas.Db.MD_CambiosPlanOperativo(usuario.IdContacto, usuario.CodGrupo, "PRODUCCION").OrderBy(pr => pr.Tarea);

            //var gvventas = consultas.Db.MD_CambiosPlanOperativo(usuario.IdContacto, usuario.CodGrupo, "VENTAS").OrderBy(vn => vn.Tarea);

            consultas.Parameters = new[]{
                new SqlParameter{
                    ParameterName = "@CodUsuario",
                    Value = usuario.IdContacto
                },
                new SqlParameter{
                    ParameterName = "@CodGrupo",
                    Value = usuario.CodGrupo
                },
                new SqlParameter{
                    ParameterName = "@opcion",
                    Value = "OPERATIVO"
                }
            };

            var gvoperativo = consultas.ObtenerDataTable("MD_CambiosPlanOperativo", "StoredProcedure");

            consultas.Parameters = new[]{
                new SqlParameter{
                    ParameterName = "@CodUsuario",
                    Value = usuario.IdContacto
                },
                new SqlParameter{
                    ParameterName = "@CodGrupo",
                    Value = usuario.CodGrupo
                },
                new SqlParameter{
                    ParameterName = "@opcion",
                    Value = "NOMINA"
                }
            };

            var gvnomina = consultas.ObtenerDataTable("MD_CambiosPlanOperativo", "StoredProcedure");

            consultas.Parameters = new[]{
                new SqlParameter{
                    ParameterName = "@CodUsuario",
                    Value = usuario.IdContacto
                },
                new SqlParameter{
                    ParameterName = "@CodGrupo",
                    Value = usuario.CodGrupo
                },
                new SqlParameter{
                    ParameterName = "@opcion",
                    Value = "PRODUCCION"
                }
            };

            var gvproduccion = consultas.ObtenerDataTable("MD_CambiosPlanOperativo", "StoredProcedure");

            consultas.Parameters = new[]{
                new SqlParameter{
                    ParameterName = "@CodUsuario",
                    Value = usuario.IdContacto
                },
                new SqlParameter{
                    ParameterName = "@CodGrupo",
                    Value = usuario.CodGrupo
                },
                new SqlParameter{
                    ParameterName = "@opcion",
                    Value = "VENTAS"
                }
            };

            var gvventas = consultas.ObtenerDataTable("MD_CambiosPlanOperativo", "StoredProcedure");

            var editable = !string.IsNullOrEmpty(ddlfiltro.SelectedValue);
            if (gvoperativo.Columns.IndexOf("editable") == -1) gvoperativo.Columns.Add(new DataColumn("editable"));
            gvoperativo.Columns["editable"].Expression = string.Format("{0}", editable);
            gvoperativo.AcceptChanges();

            if (gvnomina.Columns.IndexOf("editable") == -1) gvnomina.Columns.Add(new DataColumn("editable"));
            gvnomina.Columns["editable"].Expression = string.Format("{0}", editable);
            gvnomina.AcceptChanges();

            if (gvproduccion.Columns.IndexOf("editable") == -1) gvproduccion.Columns.Add(new DataColumn("editable"));
            gvproduccion.Columns["editable"].Expression = string.Format("{0}", editable);
            gvproduccion.AcceptChanges();

            if (gvventas.Columns.IndexOf("editable") == -1) gvventas.Columns.Add(new DataColumn("editable"));
            gvventas.Columns["editable"].Expression = string.Format("{0}", editable);
            gvventas.AcceptChanges();

            if (!string.IsNullOrEmpty(opc))
            {
                string filtro = "Tipo_Solicitud = '" + opc + "'";

                var gvoperativo1 = gvoperativo.Select(filtro);
                var gvnomina1 = gvnomina.Select(filtro);
                var gvproduccion1 = gvproduccion.Select(filtro);
                var gvventas1 = gvventas.Select(filtro);

                gvactividadesplanoperativo.DataSource = gvoperativo1;
                gvactividadesplanoperativo.DataBind();
                gvcargosnomina.DataSource = gvnomina1;
                gvcargosnomina.DataBind();
                gvproductosproduccion.DataSource = gvproduccion1;
                gvproductosproduccion.DataBind();
                gvproductosventas.DataSource = gvventas1;
                gvproductosventas.DataBind();
            }
            else
            {
                var gvoperativo1 = gvoperativo;
                var gvnomina1 = gvnomina;
                var gvproduccion1 = gvproduccion;
                var gvventas1 = gvventas;

                gvactividadesplanoperativo.DataSource = gvoperativo1;
                gvactividadesplanoperativo.DataBind();
                gvcargosnomina.DataSource = gvnomina1;
                gvcargosnomina.DataBind();
                gvproductosproduccion.DataSource = gvproduccion1;
                gvproductosproduccion.DataBind();
                gvproductosventas.DataSource = gvventas1;
                gvproductosventas.DataBind();
            }

            desCkec();
        }

        private void desCkec()
        {
            foreach (GridViewRow fila in gvactividadesplanoperativo.Rows)
            {
                if (gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                    box.Enabled = true;
                }
            }
            foreach (GridViewRow fila in gvcargosnomina.Rows)
            {
                if (gvcargosnomina.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chcknomina");
                    box.Enabled = true;
                }
            }

            foreach (GridViewRow fila in gvproductosproduccion.Rows)
            {
                if (gvproductosproduccion.DataKeys[fila.RowIndex].Value.ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckproduccion");
                    box.Enabled = true;
                }
            }

            foreach (GridViewRow fila in gvproductosventas.Rows)
            {
                if (gvproductosventas.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckventas");
                    box.Enabled = true;
                }
            }
        }

        // Se ajusta el metodo para que realice la selección de todas las tareas a Borrar.
        // Ajuste final Diciembre 11 de 2014
        // Alex Flautero 
        protected void chectodos_CheckedChanged(object sender, EventArgs e)
        {
            // Filtro para evitar que valla al resto de grillas y avise al usuariuo que solo es funcional eligiendo "Borrar"
            if (ddlfiltro.SelectedItem.Text == "Seleccione..." || ddlfiltro.SelectedItem.Text == "Adicionar" || ddlfiltro.SelectedItem.Text == "Modificar")
            { 
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Esta opción solo es funcional en el filtro Borrar')", true);
                chectodos.Checked = false;
                return;
            }
            
            // Cambia valor del check para los datos de Actividades del plan de Negocio
            foreach (GridViewRow fila in gvactividadesplanoperativo.Rows)
            {
                if (gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                    box.Checked = chectodos.Checked;
                }
            }

            // Cambia valor del check para los datos de Cargos de Nómina
            foreach (GridViewRow fila in gvcargosnomina.Rows)
            {
                if (gvcargosnomina.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chcknomina");
                        box.Checked = chectodos.Checked;
                }
            }

            // Cambia valor del check para los datos de Productos en producción
            foreach (GridViewRow fila in gvproductosproduccion.Rows)
            {
                if (gvproductosproduccion.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckproduccion");
                        box.Checked = chectodos.Checked;
                }
            }

            // Cambia valor del check para los datos de Productos en Ventas
            foreach (GridViewRow fila in gvproductosventas.Rows)
            {
                if (gvproductosventas.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                {
                    CheckBox box = (CheckBox)fila.FindControl("chckventas");
//                    if (box.Enabled && ddlfiltro.SelectedItem.Text == "Borrar")
                        box.Checked = chectodos.Checked;
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/05/2014.
        /// RowCommand de Plan Operativo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvactividadesplanoperativo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] param = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["Accion"] = param[0];
            HttpContext.Current.Session["CodProyecto"] = param[1];
            HttpContext.Current.Session["CodActividad"] = param[2];

            //NEW Session. (Indica que al cargar este valor, ciertos valores en "CatalogoActividadPO" se volverán visibles).
            HttpContext.Current.Session["Detalles_CambiosPO_PO"] = "PO";

            Redirect(null, "../Evaluacion/CatalogoActividadPO.aspx", "_Blank", "scrollbars=1,width=950,height=650");
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/05/2014.
        /// RowCommand de Nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcargosnomina_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] param = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["Accion"] = param[0];
            HttpContext.Current.Session["CodProyecto"] = param[1];
            HttpContext.Current.Session["CodNomina"] = param[2];

            //NEW Session. (Indica que al cargar este valor, ciertos valores en "CatalogoInterventorTMP" se volverán visibles).
            HttpContext.Current.Session["Detalles_CambiosPO_NO"] = "NO";

            Redirect(null, "../Evaluacion/CatalogoInterventorTMP.aspx", "_Blank", "scrollbars=1,width=950,height=600");
            //Redirect(null, "CatalogoInterventorTMP.aspx", "_Blank", "width=730,height=585");
        }

        protected void gvproductosproduccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] param = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["Accion"] = param[0];
            HttpContext.Current.Session["CodProyecto"] = param[1];
            HttpContext.Current.Session["CodProducto"] = param[2];

            //NEW Session. (Indica que al cargar este valor, ciertos valores en "CatalogoProduccionTMP" se volverán visibles).
            HttpContext.Current.Session["Detalles_CambiosPO_PO"] = "NO";

            Redirect(null, "../Evaluacion/CatalogoProduccionTMP.aspx", "_Blank", "scrollbars=1,width=730,height=585");
        }

        protected void gvproductosventas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] param = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["Accion"] = param[0];
            HttpContext.Current.Session["CodProyecto"] = param[1];
            HttpContext.Current.Session["CodProducto"] = param[2];

            //NEW Session. (Indica que al cargar este valor, ciertos valores en "CatalogoVentasTMP" se volverán visibles).
            HttpContext.Current.Session["Detalles_CambiosPO_VO"] = "NO";

            Redirect(null, "../Evaluacion/CatalogoVentasTMP.aspx", "_Blank", "scrollbars=1,width=730,height=585");
        }

        /// <summary>
        /// Establecer los check en 1 para indicar que todos seran borrados.
        /// </summary> Se aajusto para que filtrara los datos a Borrar
        /// Noviembre 2014 - Alex Flautero 
        protected void ddlfiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlfiltro.SelectedValue == "Borrar")
            {
                chectodos.Enabled = true;
                CargarCambios_PO("Borrar");
            }
            else
            {
                //chectodos.Enabled = false;
                chectodos.Checked = false;
                CargarCambios_PO(ddlfiltro.SelectedValue);
            }
        }

        #region Metodo que elimina los datos relacionados con los proyectos seleccionados
        /// <summary> Metodo que elimina los datos relacionados con los proyectos seleccionados
        /// <returns> void </returns>
        /// <remarks>2014/10/26 Alex Flautero</remarks>
        protected void lnkelemeir_Click(object sender, EventArgs e)
        {
            int ID_Proyecto = 0, ID_Nomina = 0, Id_Produccion = 0;
            int eliminado = 0, cantidad = 0, CambiaTareaaBorrar = 0;
            String NomProducto = String.Empty;

            if (ddlfiltro.SelectedValue == "Borrar")
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    //Se envían las tareas de tipo Borrar al gerente interventor
                    foreach (GridViewRow fila in gvactividadesplanoperativo.Rows)
                    {
                        if (gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[1].ToString());
                            String NomActividad = gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[2].ToString();
                            CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                            if (box.Checked == true)
                            {
                                CambiaTareaaBorrar = ObjCambiosPo.Eliminar_ProyectoActividadPOInterventorTMP(ID_Proyecto, NomActividad);
                                if (CambiaTareaaBorrar != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }

                    foreach (GridViewRow fila in gvcargosnomina.Rows)
                    {
                        if (gvcargosnomina.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvcargosnomina.DataKeys[fila.RowIndex].Values[1].ToString());
                            ID_Nomina = int.Parse(gvcargosnomina.DataKeys[fila.RowIndex].Values[2].ToString());
                            CheckBox box = (CheckBox)fila.FindControl("chcknomina");
                            if (box.Checked == true)
                            {
                                CambiaTareaaBorrar = ObjCambiosPo.EnviaTareaBorrarNominaTMP_A_GerenteInterventor(ID_Proyecto, ID_Nomina);
                                if (CambiaTareaaBorrar != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }


                    foreach (GridViewRow fila in gvproductosproduccion.Rows)
                    {
                        if (gvproductosproduccion.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvproductosproduccion.DataKeys[fila.RowIndex].Values[1].ToString());
                            Id_Produccion = int.Parse(gvproductosproduccion.DataKeys[fila.RowIndex].Values[2].ToString());
                            CheckBox box = (CheckBox)fila.FindControl("chckproduccion");
                            if (box.Checked == true)
                            {
                                CambiaTareaaBorrar = ObjCambiosPo.EnviaTareaBorrarProduccionTMP_A_GerenteInterventor(ID_Proyecto, Id_Produccion);
                                if (CambiaTareaaBorrar != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }

                    foreach (GridViewRow fila in gvproductosventas.Rows)
                    {

                        if (gvproductosventas.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvproductosventas.DataKeys[fila.RowIndex].Values[1].ToString());
                            NomProducto = gvproductosventas.DataKeys[fila.RowIndex].Values[2].ToString();
                            CheckBox box = (CheckBox)fila.FindControl("chckventas");
                            if (box.Checked == true)
                            {
                                CambiaTareaaBorrar = ObjCambiosPo.EnviaTareaBorrarVentasTMP_A_GerenteInterventor(ID_Proyecto, NomProducto);
                                if (eliminado != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }
                }

                // Se eliminan definitivamente las actividades del plan operativo   - GERENTE INTERVENTOR
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    foreach (GridViewRow fila in gvactividadesplanoperativo.Rows)
                    {
                        if (gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[1].ToString());
                            String NomActividad = gvactividadesplanoperativo.DataKeys[fila.RowIndex].Values[2].ToString();
                            CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                            if (box.Checked == true)
                            {
                                eliminado = ObjCambiosPo.Eliminar_ProyectoActividadPOInterventorTMP(ID_Proyecto, NomActividad);
                                if (eliminado != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }

                    foreach (GridViewRow fila in gvcargosnomina.Rows)
                    {
                        if (gvcargosnomina.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvcargosnomina.DataKeys[fila.RowIndex].Values[1].ToString());
                            CheckBox box = (CheckBox)fila.FindControl("chcknomina");
                            if (box.Checked == true)
                            {
                                eliminado = ObjCambiosPo.Eliminar_InterventorNominaTMP(ID_Proyecto);
                                if (eliminado != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }


                    foreach (GridViewRow fila in gvproductosproduccion.Rows)
                    {
                        if (gvproductosproduccion.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvproductosproduccion.DataKeys[fila.RowIndex].Values[1].ToString());
                            CheckBox box = (CheckBox)fila.FindControl("chckproduccion");
                            if (box.Checked == true)
                            {
                                eliminado = ObjCambiosPo.Eliminar_InterventorProduccionTMP(ID_Proyecto);
                                if (eliminado != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;
                            }
                        }
                    }

                    foreach (GridViewRow fila in gvproductosventas.Rows)
                    {

                        if (gvproductosventas.DataKeys[fila.RowIndex].Values[0].ToString() == "Borrar")
                        {
                            ID_Proyecto = int.Parse(gvproductosventas.DataKeys[fila.RowIndex].Values[1].ToString());
                            CheckBox box = (CheckBox)fila.FindControl("chckventas");
                            if (box.Checked == true)
                            {
                                eliminado = ObjCambiosPo.Eliminar_InterventorVentasTMP(ID_Proyecto);
                                if (eliminado != 0) { cantidad = cantidad + 1; }
                                box.Checked = false;

                            }
                        }
                    }
                }
                CargarCambios_PO("Borrar");

                if (cantidad == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No existen tareas seleccionadas para Borrar'); document.location.reload();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La transacción fué realizada con éxito'); document.location.reload();", true);
                    CargarCambios_PO("Borrar");
                    chectodos.Checked = false;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar Borrar para modificar la tarea'); document.location.reload();", true);
            }
        }
        #endregion

        protected void btnBuscarProyecto_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 numero;
                if (Int64.TryParse(txtBuscarProyecto.Text, out numero))
                {
                    string busquedaPorProyecto = " AND empresa.codproyecto = " + numero.ToString() + " ";
                    CargarCambios_PO(ddlfiltro.SelectedValue, busquedaPorProyecto);
                }
                else
                {
                    CargarCambios_PO(ddlfiltro.SelectedValue);
                }                                  
            }
            catch (Exception)
            {
                CargarCambios_PO(ddlfiltro.SelectedValue);
            }                
        }

        protected void btnBuscarPorInterventor_Click(object sender, EventArgs e)
        {
            try
            {                
                if (!string.IsNullOrEmpty(txtBuscarPorInterventor.Text))
                {
                    string busquedaPorInterventor = " AND (Contacto.Nombres + ' ' + Contacto.Apellidos like '%"+ txtBuscarPorInterventor.Text + "%') ";
                    CargarCambios_PO(ddlfiltro.SelectedValue,"", busquedaPorInterventor);
                }
                else
                {
                    CargarCambios_PO(ddlfiltro.SelectedValue);
                }
            }
            catch (Exception)
            {
                CargarCambios_PO(ddlfiltro.SelectedValue);
            }
        }
    }
}