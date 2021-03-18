#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>17 - 07 - 2014</Fecha>
// <Archivo>CatalogoCriterio.cs</Archivo>

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
    /// CatalogoCriterio
    /// </summary>    
    public partial class CatalogoCriterio : Negocio.Base_Page
    {
        #region variables globales y de session

        delegate string del(string x, string y, int z);

        string Accion;
        int codConvocatoria;
        int codCriterio;

        string NomCriterio;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 17 - 07 - 2014
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region recoger datos de session

            codCriterio = HttpContext.Current.Session["codCriterio"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codCriterio"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["codCriterio"].ToString()) : 0;
            codConvocatoria = HttpContext.Current.Session["codConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codConvocatoria"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["codConvocatoria"].ToString()) : 0;
            Accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : string.Empty;

            #endregion

            #region limpiar items de los ListBox

            if (!IsPostBack)
            {
                lbx_ciudades.Items.Clear();
                lbx_ciudadesseleccionadas.Items.Clear();
                lbx_sectores.Items.Clear();
                lbx_sectoresseleccionados.Items.Clear();
            }

            #endregion

            switch (Accion)
            {
                case "Editar":

                    NomCriterio = (from cc in consultas.Db.ConvocatoriaCriterios
                                   where cc.Id_Criterio == codCriterio
                                   select cc.NomCriterio).FirstOrDefault();

                    lbl_titulo.Text = "MODIFICAR CRITERIO";
                    btnaccion.Text = "Actualizar Criterio";

                    if(!IsPostBack)
                        txtnombrecriterio.Text = NomCriterio;

                    break;
                case "Nuevo":
                    lbl_titulo.Text = "NUEVO CRITERIO";
                    btnaccion.Text = "Crear Criterio";
                    break;
            }
        }

        #region cargar datos        
        /// <summary>
        /// Handles the Selecting event of the lds_departamento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_departamento_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //txtSQL="SELECT * FROM Departamento WHERE codPais=1 ORDER BY nomDepartamento"
            var departamentos = (from d in consultas.Db.departamento
                                 orderby d.NomDepartamento
                                 where d.CodPais == 1
                                 select new
                                 {
                                     d.Id_Departamento,
                                     d.NomDepartamento
                                 });

            var lista = departamentos.ToList();

            lista.Insert(0, new { Id_Departamento = 0, NomDepartamento = " (Todo el pais) " });

            e.Result = lista;
        }
        /// <summary>
        /// Handles the Selecting event of the lds_ciudades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_ciudades_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!string.IsNullOrEmpty(ddldepartamento.SelectedValue))
            {
                var ciudades = (from c in consultas.Db.Ciudad
                                orderby c.CodDepartamento, c.NomCiudad
                                where c.CodDepartamento == Convert.ToInt32(ddldepartamento.SelectedValue)
                                select new
                                {
                                    Id_Ciudad = c.Id_Ciudad + ";" + c.CodDepartamento,
                                    c.NomCiudad
                                });

                var lista = ciudades.ToList();

                lista.Insert(0, new { Id_Ciudad = string.Empty, NomCiudad = " (Todos los Municipios) " });

                e.Result = lista;
            }
        }

        /// <summary>
        /// Handles the Selecting event of the lds_ciudadesseleccionadas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_ciudadesseleccionadas_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            del delegado = (string x, string y, int z) =>
            {
                string valor = string.Empty;

                if (z == 1)
                {
                    if (string.IsNullOrEmpty(x))
                        valor += " (Todo el pais) ";
                    else
                        valor += x;

                    if (string.IsNullOrEmpty(y))
                        valor += " (Todos los Municipios) ";
                    else
                        valor += y;
                }
                return valor;
            };

            var ciudades = (from k in consultas.Db.ConvocatoriaCriterioCiudads
                            join d in consultas.Db.departamento on k.CodDepartamento equals d.Id_Departamento into r1
                            from c1 in r1.DefaultIfEmpty()
                            join c in consultas.Db.Ciudad on k.CodCiudad equals c.Id_Ciudad into r2
                            from c2 in r2.DefaultIfEmpty()
                            where k.CodCriterio == codCriterio
                            select new
                            {
                                Id_Ciudad = k.CodCiudad + ";" + k.CodDepartamento,
                                NomCiudad = delegado(c1.NomDepartamento, c2.NomCiudad, 1)
                            });

            e.Result = ciudades.ToList();
        }

        /// <summary>
        /// Handles the Selecting event of the lds_sectores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_sectores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var sectores = (from s in consultas.Db.Sector
                            orderby s.NomSector
                            select new
                            {
                                s.Id_Sector,
                                s.NomSector
                            });

            var lista = sectores.ToList();

            lista.Insert(0, new { Id_Sector = 0, NomSector = " (Todos los Sectores) " });

            e.Result = lista;
        }

        /// <summary>
        /// Handles the Selecting event of the lds_sectoresseleccionados control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_sectoresseleccionados_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            del delegado = (string x, string y, int z) =>
            {
                string valor = string.Empty;

                if (z == 1)
                {
                    if (string.IsNullOrEmpty(x))
                        valor += " (Todos los sectores) ";
                    else
                        valor += x;
                }
                return valor;
            };

            var secortes = (from k in consultas.Db.ConvocatoriaCriterioSectors
                            join s in consultas.Db.Sector on k.CodSector equals s.Id_Sector into r1
                            from c1 in r1.DefaultIfEmpty()
                            orderby c1.NomSector
                            where k.CodCriterio == codCriterio
                            select new
                            {
                                Id_Sector = c1.Id_Sector.ToString(),
                                nomSector = delegado(c1.NomSector, "", 1)
                            });

            e.Result = secortes.ToList();
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddldepartamento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddldepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbx_ciudades.DataBind();
        }

        #region ambito geografico

        /// <summary>
        /// Handles the Click event of the btn_agregarciudad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_agregarciudad_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbx_ciudades.Items)
                {
                    if (item.Selected)
                    {
                        if(!verificarCiudad(item))
                            lbx_ciudadesseleccionadas.Items.Add(item);
                    }
                }
            }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// Handles the Click event of the btn_quitarciudad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_quitarciudad_Click(object sender, EventArgs e) { eliminarItemCiudad(); }

        private void eliminarItemCiudad()
        {
            try
            {
                foreach (ListItem item in lbx_ciudadesseleccionadas.Items)
                {
                    if (item.Selected)
                    {
                        lbx_ciudadesseleccionadas.Items.Remove(item);
                    }
                }
            }
            catch (InvalidOperationException) { eliminarItemCiudad(); }
        }

        private bool verificarCiudad(ListItem item)
        {
            bool existe = false;

            foreach (ListItem li in lbx_ciudadesseleccionadas.Items)
            {
                if (li.Value.Equals(item.Value) && li.Text.Equals(item.Text))
                    return true;
            }

            return existe;
        }

        #endregion

        #region ambito economico        
        /// <summary>
        /// Handles the Click event of the btn_agregarsector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_agregarsector_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbx_sectores.Items)
                {
                    if (item.Selected)
                    {
                        if (!verificaSector(item))
                            lbx_sectoresseleccionados.Items.Add(item);
                    }
                }
            }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// Handles the Click event of the btn_quitarsector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_quitarsector_Click(object sender, EventArgs e) { eliminarItemSector(); }

        private void eliminarItemSector()
        {
            try
            {
                foreach (ListItem item in lbx_sectoresseleccionados.Items)
                {
                    if (item.Selected)
                    {
                        lbx_sectoresseleccionados.Items.Remove(item);
                    }
                }
            }
            catch (InvalidOperationException) { eliminarItemSector(); }
        }

        private bool verificaSector(ListItem item)
        {
            bool existe = false;

            foreach (ListItem li in lbx_sectoresseleccionados.Items)
            {
                if (li.Value.Equals(item.Value) && li.Text.Equals(item.Text))
                    return true;
            }

            return existe;
        }

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 17 - 07 - 2014
        /// evento disparado en el boton crear o actualizar
        /// valida si se a seleccionado algun ambito de las dos categorias
        /// de lo contrario retorna
        /// en caso de seguir efectua la accion crear o actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnaccion_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            if (lbx_ciudadesseleccionadas.Items.Count <= 0)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El Criterio Ciudad Es Requerido');window.close();</script>");
                return;
            }

            if (lbx_sectoresseleccionados.Items.Count <= 0)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El Criterio Sector Es Requerido');window.close();</script>");
                return;
            }

            switch (Accion)
            {
                case "Editar":
                    try
                    {
                        /*var criterio = (from c in consultas.Db.ConvocatoriaCriterios
                                        where c.CodConvocatoria == codConvocatoria
                                        && c.NomCriterio == txtnombrecriterio.Text
                                        && c.Id_Criterio != codCriterio
                                        select c).FirstOrDefault();

                        
                        if (criterio != null)
                        {
                            
                        }
                        */
                        String txtSQL = " select * from ConvocatoriaCriterio where NomCriterio = '" + txtnombrecriterio.Text + "'" +
                                          " And Id_Criterio = " + codCriterio.ToString();

                        var tabla_inicial = consultas.ObtenerDataTable(txtSQL, "text");

                        if(  tabla_inicial.Rows.Count > 0)
                            {
                                Actualizar();
                            }
                        else {
                            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No existe un Criterio con ese Nombre.');window.close();</script>"); 
                        }
                    }
                    catch (InvalidOperationException) { Actualizar(); }
                    break;
                case "Nuevo":
                    try
                    {
                        var concri = (from cc in consultas.Db.ConvocatoriaCriterios
                                      where cc.NomCriterio == txtnombrecriterio.Text
                                      && cc.CodConvocatoria == codConvocatoria
                                      select cc).FirstOrDefault();

                        if (concri == null)
                        {
                            crear();
                        }
                        else
                        {
                            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Ya existe un Criterio con ese Nombre.');window.close();</script>");
                        }
                    }
                    catch (Exception ){
                        throw;
                    }
                    break;
            } // Cierre del switch
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.history.go(-1);window.close();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;window.close();", true);
        }

        /// <summary>
        /// Diego Quiñonez
        /// 18 - 07 - 2014
        /// crea el criterio
        /// </summary>
        private void crear()
        {
            ClientScriptManager cm = this.ClientScript;

            Datos.ConvocatoriaCriterio criterio = new Datos.ConvocatoriaCriterio();

            criterio.CodConvocatoria = codConvocatoria;
            criterio.NomCriterio = txtnombrecriterio.Text;

            consultas.Db.ConvocatoriaCriterios.InsertOnSubmit(criterio);

            try
            {
                consultas.Db.SubmitChanges();

                var criterio1 = (from c in consultas.Db.ConvocatoriaCriterios
                                where c.CodConvocatoria == codConvocatoria
                                && c.NomCriterio == txtnombrecriterio.Text
                                && c.Id_Criterio != codCriterio
                                select c).FirstOrDefault();

                if (criterio1 != null)
                {
                    codCriterio = criterio1.Id_Criterio;
                    accionActualizarYCrear();
                }
            }
            catch (Exception)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Se ha producido un error al crear el registro');window.close();</script>");
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 18 - 07 - 2014
        /// actualiza el criterio
        /// </summary>
        private void Actualizar()
        {
            ClientScriptManager cm = this.ClientScript;

            var criterio = (from c in consultas.Db.ConvocatoriaCriterios
                             where c.CodConvocatoria == codConvocatoria
                             && c.Id_Criterio == codCriterio
                             select c).First();

            criterio.NomCriterio = txtnombrecriterio.Text;
            try
            {
                consultas.Db.SubmitChanges();
                accionActualizarYCrear();
            }
            catch (Exception)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Se ha producido un error al actualizar el registro');window.close();</script>");
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 18 - 07 - 2014
        /// elimina los ambitos de las ciudades y sectores asociados al criterio
        /// si no existe el criterio no efectua ningna accion
        /// y registra los ambitos seleccionados de los ListBox para ser guardados
        /// en base al criterio creado o actualizado
        /// </summary>
        private void accionActualizarYCrear()
        {
            var sectores = (from c in consultas.Db.ConvocatoriaCriterioSectors
                            where c.CodCriterio == codCriterio
                            select c);

            consultas.Db.ConvocatoriaCriterioSectors.DeleteAllOnSubmit(sectores);

            consultas.Db.SubmitChanges();

            var ciudades = (from c in consultas.Db.ConvocatoriaCriterioCiudads
                            where c.CodCriterio == codCriterio
                            select c);

            consultas.Db.ConvocatoriaCriterioCiudads.DeleteAllOnSubmit(ciudades);

            consultas.Db.SubmitChanges();

            foreach (ListItem ls in lbx_ciudadesseleccionadas.Items)
            {
                Datos.ConvocatoriaCriterioCiudad ciu = new Datos.ConvocatoriaCriterioCiudad();

                ciu.CodCriterio = codCriterio;
                ciu.CodCiudad = !string.IsNullOrEmpty(ls.Value) ? Convert.ToInt32(ls.Value.Split(';')[0]) : 0;
                ciu.CodDepartamento = !string.IsNullOrEmpty(ls.Value) ? Convert.ToInt32(ls.Value.Split(';')[1]) : 0;

                consultas.Db.ConvocatoriaCriterioCiudads.InsertOnSubmit(ciu);

                consultas.Db.SubmitChanges();
            }

            foreach (ListItem ls in lbx_sectoresseleccionados.Items)
            {
                Datos.ConvocatoriaCriterioSector ciu = new Datos.ConvocatoriaCriterioSector();

                ciu.CodCriterio = codCriterio;

                if (!string.IsNullOrEmpty(ls.Value) && !ls.Value.Equals("0"))
                    ciu.CodSector = Convert.ToInt32(ls.Value);

                //ciu.CodSector = !string.IsNullOrEmpty(ls.Value) && !ls.Value.Equals("0") ? Convert.ToInt32(ls.Value) : 0;

                consultas.Db.ConvocatoriaCriterioSectors.InsertOnSubmit(ciu);

                consultas.Db.SubmitChanges();
            }
        }
    }
}