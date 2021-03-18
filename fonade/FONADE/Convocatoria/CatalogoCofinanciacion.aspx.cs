#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 07 - 2014</Fecha>
// <Archivo>CatalogoCofinanciacion.cs</Archivo>

#endregion

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Fonade
{
    /// <summary>
    /// CatalogoCofinanciacion
    /// </summary>    
    public partial class CatalogoCofinanciacion : Negocio.Base_Page
    {
        #region variables globales y de session

        int CodCiudad;
        string Accion;
        int codConvocatoria;

        double numCofinanciacion = 0;
        int CodDepartamento = 0;
        
        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region recoger datos de session

            CodCiudad = HttpContext.Current.Session["CodCiudad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodCiudad"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodCiudad"].ToString()) : 0;
            codConvocatoria = HttpContext.Current.Session["codConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codConvocatoria"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["codConvocatoria"].ToString()) : 0;
            Accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : string.Empty;
            
            #endregion

            //acciones realizadas si se va a actualizar un item

            switch (Accion)
            {
                case "Editar":
                    lbl_titulo.Text = "MODIFICAR COFINANCIACIÓN";
                    btnaccion.Text = "Actualizar Cofinanciación";

                    var result = (from k in consultas.Db.ConvocatoriaCofinanciacions
                                  from c in consultas.Db.Ciudad
                                  where c.Id_Ciudad == k.CodCiudad
                                  && k.CodConvocatoria == codConvocatoria
                                  && k.CodCiudad == CodCiudad
                                  select new
                                  {
                                      k.Cofinanciacion,
                                      c.CodDepartamento
                                  }).FirstOrDefault();

                    numCofinanciacion = Convert.ToDouble(result.Cofinanciacion);
                    CodDepartamento = result.CodDepartamento;

                    if (!IsPostBack)
                    {
                        //Muestra la informacion para ser modicifada
                        txtcofinanciacion.Text = numCofinanciacion.ToString();
                        try
                        {
                            ddldepartamento.SelectedValue = CodDepartamento.ToString();
                            ddlmunicipio.SelectedValue = CodCiudad.ToString();
                        }
                        catch (Exception) { }
                    }
                    break;
                case "Nuevo":
                    lbl_titulo.Text = "NUEVO COFINANCIACIÓN";
                    btnaccion.Text = "Crear Cofinanciación";
                    break;
            }

            txtcofinanciacion.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
        }

        /// <summary>
        /// Diego Quiñonez
        /// 17 - 07 - 2014
        /// crea o actualiza de acuerdo a lo requerido por la convocatoria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnaccion_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;
            
            int id_municipio;

            if (!string.IsNullOrEmpty(ddldepartamento.SelectedValue) && !string.IsNullOrEmpty(ddlmunicipio.SelectedValue))
            {
                id_municipio = Convert.ToInt32(ddlmunicipio.SelectedValue);

                switch (Accion)
                {
                    case "Editar":
                        var concon = (from cc in consultas.Db.ConvocatoriaCofinanciacions
                                      where cc.CodConvocatoria == codConvocatoria
                                          && cc.CodCiudad == id_municipio
                                      select cc).First();

                        if (concon != null)
                        {
                            concon.Cofinanciacion = Convert.ToDouble(txtcofinanciacion.Text);

                            try
                            {
                                consultas.Db.SubmitChanges();
                            }
                            catch (Exception)
                            {
                                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Se ha producido un error al actualizar el registro');window.close();</script>");
                            }
                        }
                        break;
                    case "Nuevo":
                        try
                        {
                            var concon1 = (from cc in consultas.Db.ConvocatoriaCofinanciacions
                                           where cc.CodConvocatoria == codConvocatoria
                                               && cc.CodCiudad == id_municipio
                                           select cc).First();

                            if (concon1 != null)
                            {
                                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Ya existe una cofinanciación creada para este municipio.');window.close();</script>");
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            Datos.ConvocatoriaCofinanciacion cocof = new Datos.ConvocatoriaCofinanciacion();

                            cocof.CodConvocatoria = codConvocatoria;
                            cocof.CodCiudad = id_municipio;
                            cocof.Cofinanciacion = Convert.ToDouble(txtcofinanciacion.Text);

                            consultas.Db.ConvocatoriaCofinanciacions.InsertOnSubmit(cocof);

                            try
                            {
                                consultas.Db.SubmitChanges();
                            }
                            catch (Exception)
                            {
                                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Se ha producido un error al crear el registro');window.close();</script>");
                            }
                        }
                        break;
                }
                //cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                //return;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;window.close();", true);
            }
            else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('seleccione el departamento y la ciudad');</script>");
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// carga la lista de los departamentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_departamento_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var departamentos = (from d in consultas.Db.departamento
                                 from k in consultas.Db.ConvocatoriaCriterios
                                 from kc in consultas.Db.ConvocatoriaCriterioCiudads
                                 orderby d.NomDepartamento
                                 where k.Id_Criterio == kc.CodCriterio
                                 && (kc.CodDepartamento == d.Id_Departamento || kc.CodDepartamento == 0)
                                 && k.CodConvocatoria == codConvocatoria
                                 && d.CodPais == 1
                                 select new
                                 {
                                     d.Id_Departamento,
                                     d.NomDepartamento
                                 }).Distinct();

            var lista = departamentos.ToList();

            lista.Insert(0, new { Id_Departamento=0,NomDepartamento = "" });

            e.Result = lista;


            //agrega un item vacio a la lista de departamentos
            //ddldepartamento.Items.Insert(0, new ListItem("Seleccione un departamento...", ""));
        }

        /// <summary>
        /// Diego Quiñonez
        /// 17 - 07 - 2014
        /// metodo que dispara el ddldepartamentos
        /// para cargar las ciudades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddldepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlmunicipio.DataBind();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 17 - 07 - 2014
        /// carga la lista de ciudades segun el departamento seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_municipio_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!string.IsNullOrEmpty(ddldepartamento.SelectedValue))
            {
                var municipios = (from c in consultas.Db.Ciudad
                                  from kc in consultas.Db.ConvocatoriaCriterioCiudads
                                  from k in consultas.Db.ConvocatoriaCriterios
                                  where ((kc.CodCiudad == c.Id_Ciudad && kc.CodDepartamento == c.CodDepartamento) ||
                                  (kc.CodCiudad == 0 && kc.CodDepartamento == c.CodDepartamento) ||
                                  (kc.CodCiudad == 0 && kc.CodDepartamento == 0)) &&
                                  k.Id_Criterio == kc.CodCriterio &&
                                  k.CodConvocatoria == codConvocatoria
                                  && c.CodDepartamento == Convert.ToInt32(ddldepartamento.SelectedValue)
                                  select new
                                  {
                                      //k.CodConvocatoria,
                                      c.Id_Ciudad,
                                      c.NomCiudad,
                                      //c.CodDepartamento
                                  }).Distinct();

                e.Result = municipios.ToList();

                //agrega un item vacio a la lista de ciudades
                ddlmunicipio.Items.Insert(0, new ListItem("Seleccione un municipio...", ""));
            }
        }
    }
}