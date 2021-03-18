using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Web.UI;
using Fonade.Clases;
using System.Web;
using System.Net.Mail;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// CatalogoUnidadEmprende
    /// </summary>    
    public partial class CatalogoUnidadEmprende : Base_Page
    {
        int idJefe;
        int idUnidad;
        #region Eventos
        void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                lbl_enunciado.Text = "JEFE UNIDAD DE EMPRENDIMIENTO";
                CargarCombos();
                CargarUnidadesEmprendimiento("A");
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkAddUnidad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkAddUnidad_Click(object sender, EventArgs e)
        {
            NuevaUnidad();
        }

        /// <summary>
        /// Handles the Click event of the ImgBtnAdicionar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ImgBtnAdicionar_Click(object sender, ImageClickEventArgs e)
        {
            NuevaUnidad();
        }

        /// <summary>
        /// Handles the Click event of the btn_buscarJefeUnidad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_buscarJefeUnidad_Click(object sender, EventArgs e)
        {
            BuscarJefe();
        }

        /// <summary>
        /// Handles the Click event of the btnAtras control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            AtrasDatosUnidad();
        }

        /// <summary>
        /// Handles the Click event of the btnBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarDatosJefe();
        }

        /// <summary>
        /// Handles the Click event of the btnAtras2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAtras2_Click(object sender, EventArgs e)
        {
            AtrasBuscarJefe();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlDpto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlDpto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCiudades(int.Parse(ddlDpto.SelectedValue), ddlCiudades);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlDptoJefe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void ddlDptoJefe_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCiudades(int.Parse(ddlDptoJefe.SelectedValue), ddlCiudadJefe);
        }

        /// <summary>
        /// Handles the Click event of the btn_crearUnidad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void btn_crearUnidad_Click(object sender, EventArgs e)
        {
            CrearUnidad();
        }

        /// <summary>
        /// Handles the Click event of the btnCrerJefe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void btnCrerJefe_Click(object sender, EventArgs e)
        {
            CreaActualizaJefe();
        }

        /// <summary>
        /// Handles the RowDataBound event of the grvUnidadesEmprendimiento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void grvUnidadesEmprendimiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ModificaDatosGrid(e);
        }

        /// <summary>
        /// Handles the RowCommand event of the grvUnidadesEmprendimiento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs" /> instance containing the event data.</param>
        protected void grvUnidadesEmprendimiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            EditarUnidad(e);
        }

        /// <summary>
        /// Handles the Click event of the btnAtrasPrincipal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void btnAtrasPrincipal_Click(object sender, EventArgs e)
        {
            VolverPrincipal();
        }
        #endregion

        #region Metodos

        #region MetodosCargarGrid        
        /// <summary>
        /// Handles the Click event of the lnkOpcionTodos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkOpcionTodos_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "%";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_A control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_A_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "A";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_B control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_B_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "B";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_C control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_C_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "C";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_D control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_D_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "D";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_E control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_E_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "E";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_F control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_F_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "F";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_G control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_G_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "G";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_H control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_H_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "H";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_I control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_I_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "I";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_J control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_J_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "J";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_K control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_K_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "K";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_L control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_L_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "L";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_M control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_M_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "M";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_N control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_N_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "N";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_O control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_O_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "O";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_P control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_P_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "P";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_Q control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_Q_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "Q";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_R control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_R_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "R";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_S control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_S_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "S";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_T control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_T_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "T";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_U control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_U_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "U";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_V control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_V_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "V";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_W control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_W_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "W";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_X control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_X_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "X";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_Y control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_Y_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "Y";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkbtn_opcion_Z control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkbtn_opcion_Z_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["upper_letter_selected"] = "Z";
            CargarUnidadesEmprendimiento(HttpContext.Current.Session["upper_letter_selected"].ToString());
        }
        #endregion

        private void ModificaDatosGrid(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbl_inst = e.Row.FindControl("lbl_id_inst") as Label; //Id de la institución.
                var lbl_inact = e.Row.FindControl("lbl_estado") as Label; //Contiene el valor de la columan "inactivo".
                //var lblIdUnidad =(Label) e.Row.FindControl("lblIdUnidad");
                var img = e.Row.FindControl("img_btn") as ImageButton;
                int valor_inactivo = 0;

                if (lbl_inst != null && lbl_inact != null)
                {
                    //Convertir el valor del texto del label en un dato operable para la siguiente condición.
                    valor_inactivo = Convert.ToInt32(lbl_inst.Text);

                    if (valor_inactivo != Constantes.CONST_UnidadTemporal)
                    {
                        if (lbl_inact.Text == "False") //0
                        {
                            //Mostrar información.
                            img.ImageUrl = "../../Images/icoBorrar.gif";
                            img.AlternateText = "Desactivar Unidad";
                            lbl_inact.Text = "Activa";
                        }
                        else
                        {
                            img.ImageUrl = "../../Images/icoActivar.gif";
                            img.AlternateText = "Activar Unidad";
                            lbl_inact.Text = "Inactiva";
                            img.CommandName = "actualizar";
                            img.CommandArgument = lbl_inst.Text;
                        }
                    }
                }
            }
        }

        private void CargarUnidadesEmprendimiento(String inicial)
        {
            if(inicial.Equals("%"))
            {
                var uniEmprendimientos = (from inst in consultas.Db.Institucions
                                          join ti in consultas.Db.TipoInstitucions on inst.CodTipoInstitucion equals ti.Id_TipoInstitucion
                                          orderby inst.NomUnidad
                                          select new
                                          {
                                              inst.Id_Institucion,
                                              inst.NomUnidad,
                                              inst.NomInstitucion,
                                              inst.Inactivo,
                                              inst.CodCiudad,
                                              ti.NomTipoInstitucion
                                          }).ToList();

                Session["dtEmpresas"] = uniEmprendimientos;
                grvUnidadesEmprendimiento.DataSource = uniEmprendimientos;
                grvUnidadesEmprendimiento.DataBind();
            }
            else
            {
                var uniEmprendimientos = (from inst in consultas.Db.Institucions
                                          join ti in consultas.Db.TipoInstitucions on inst.CodTipoInstitucion equals ti.Id_TipoInstitucion
                                          where inst.NomUnidad.StartsWith(inicial)
                                          orderby inst.NomUnidad
                                          select new
                                          {
                                              inst.Id_Institucion,
                                              inst.NomUnidad,
                                              inst.NomInstitucion,
                                              inst.Inactivo,
                                              inst.CodCiudad,
                                              ti.NomTipoInstitucion
                                          }).ToList();

                Session["dtEmpresas"] = uniEmprendimientos;
                grvUnidadesEmprendimiento.DataSource = uniEmprendimientos;
                grvUnidadesEmprendimiento.DataBind();
            }
            


            if (pnlDetalles.Visible == true)
            {
                pnlPrincipaal.Visible = false;
            }
            else
            {
                pnlPrincipaal.Visible = true;

            }
        }

        private void NuevaUnidad()
        {
            pnlPrincipaal.Visible = false;
            pnlDetalles.Visible = true;
        }

        private void BuscarJefe()
        {
            dNuevaUnidad.Visible = false;
            dBuscarJefe.Visible = true;
            Session["nroDocJefe"] = txtNumIdentificacion.Text;
            Session["idTipoDoc"] = ddlTipoIdentificacion.SelectedValue;
        }

        private void AtrasDatosUnidad()
        {
            dNuevaUnidad.Visible = true;
            dBuscarJefe.Visible = false;
        }

        private void BuscarDatosJefe()
        {
            if(ddlTipoIdentificacion.SelectedIndex != 0)
            {
                if(!string.IsNullOrEmpty(txtNumIdentificacion.Text))
                {
                    dBuscarJefe.Visible = false;
                    dListaJefes.Visible = true;

                    var contacto = (from c in consultas.Db.Contacto
                                    where c.Identificacion == Convert.ToDouble(txtNumIdentificacion.Text.Replace(".", "").Trim()) &&
                                    c.CodTipoIdentificacion == Convert.ToInt16(ddlTipoIdentificacion.SelectedValue.ToString())
                                    select c).FirstOrDefault();
                    if (contacto != null)
                    {
                        Session["idJefe"] = contacto.Id_Contacto;
                        // Buscar rol
                        var rol = (from gc in consultas.Db.GrupoContactos
                                   join g in consultas.Db.Grupos on gc.CodGrupo equals g.Id_Grupo
                                   where gc.CodContacto == contacto.Id_Contacto
                                   select new
                                   {
                                       NomRol = g.NomGrupo
                                   });
                        lnkUnidadJefe.Text = contacto.Identificacion + " - " + contacto.Nombres + " " + contacto.Apellidos;
                        if (rol.FirstOrDefault() != null)
                        {
                            lblrol.InnerHtml = "Rol: " + rol.FirstOrDefault().NomRol;
                            if (rol.FirstOrDefault().NomRol != "Asesor")
                            {
                                lnkUnidadJefe.OnClientClick = "return false;";
                                lblTextoNoAsigna.InnerHtml = "El usuario ya es " + rol.FirstOrDefault().NomRol + " en otra institución y no puede ser cambiado.";
                            }
                            else
                            {
                                lblTextoNoAsigna.InnerHtml = "El usuario pertenece a otra institución y no puede ser cambiado.";
                            }
                            lblTextoNoAsigna.Visible = true;
                            lnkUnidadJefe.OnClientClick = "return false;";
                            lnkUnidadJefe.PostBackUrl = "javascript:void(0);";
                            btnAtras3.Text = "Realizar otra búsqueda";
                        }
                        else
                        {
                            lblrol.InnerHtml = "Usuario sin rol";
                            lnkUnidadJefe.PostBackUrl = "";
                            lnkUnidadJefe.OnClientClick = "return confirm('Desea cambiar el ROL del usuario?');";
                            lblTextoNoAsigna.Visible = false;
                            btnAtras3.Text = "Atras";
                        }
                    }
                    else
                    {
                        dListaJefes.Visible = false;
                        dDatosJefe.Visible = true;
                        txtTelefonoUnidad.Text = "";
                        txtFax.Text = "";
                        txtWebsite.Text = "";
                        idJefe = 0;
                        Session["idJefe"] = idJefe;
                        txtNombres.Text = "";
                        txtNombres.Enabled = true;
                        txtApellidos.Text = "";
                        txtApellidos.Enabled = true;
                        txtEmail.Text = "";
                        txtEmail.Enabled = true;
                        txtCargo.Text = "";
                        txtTelefono.Text = "";
                        txtFaxJefe.Text = "";
                        //ddlDptoJefe.SelectedIndex = 0;
                        //ddlCiudadJefe.SelectedIndex = 0;
                        btnCrerJefe.Text = "Crear";
                    }
                }
                else
                {
                    txtNumIdentificacion.Focus();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el número de identificacíón a buscar!')", true);
                }
            }
            else
            {
                ddlTipoIdentificacion.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar el tipo de identificación!')", true);
            }
        }

        private void AtrasBuscarJefe()
        {
            dBuscarJefe.Visible = true;
            dListaJefes.Visible = false;
            dDatosJefe.Visible = false;
            if (string.IsNullOrEmpty(Session["nroDocJefe"].ToString()))
            {
                txtNumIdentificacion.Text = "";
                ddlTipoIdentificacion.SelectedIndex = 0;
            }
            //txtNumIdentificacion.Text = "";
            //ddlTipoIdentificacion.SelectedIndex = 0;

        }

        private void CargarCombos()
        {
            //Tipos Unidad
            var tipos = (from t in consultas.Db.TipoInstitucions
                         orderby t.NomTipoInstitucion
                         select t).ToList();
            ddlTipoInstitucion.DataSource = tipos;
            ddlTipoInstitucion.DataValueField = "Id_TipoInstitucion";
            ddlTipoInstitucion.DataTextField = ("NomTipoInstitucion");
            ddlTipoInstitucion.DataBind();
            ddlTipoInstitucion.Items.Insert(0, new ListItem("Seleccione", "0"));

            //Dptos Unidad
            var dptos = (from d in consultas.Db.departamento
                         orderby d.NomDepartamento
                         select d).ToList();
            ddlDpto.DataSource = dptos;
            ddlDpto.DataValueField = "Id_Departamento";
            ddlDpto.DataTextField = "NomDepartamento";
            ddlDpto.DataBind();
            ddlDpto.Items.Insert(0, new ListItem("Seleccione", "0"));

            //Tipos documentos
            var tipodoc = (from td in consultas.Db.TipoIdentificacions
                           orderby td.NomTipoIdentificacion
                           select td).ToList();
            ddlTipoIdentificacion.DataSource = tipodoc;
            ddlTipoIdentificacion.DataValueField = "Id_TipoIdentificacion";
            ddlTipoIdentificacion.DataTextField = "NomTipoIdentificacion";
            ddlTipoIdentificacion.DataBind();
            ddlTipoIdentificacion.Items.Insert(0, new ListItem("Seleccione", "0"));

            ddlEditTipoDocJefe.DataSource = tipodoc;
            ddlEditTipoDocJefe.DataValueField = "Id_TipoIdentificacion";
            ddlEditTipoDocJefe.DataTextField = "NomTipoIdentificacion";
            ddlEditTipoDocJefe.DataBind();
            ddlEditTipoDocJefe.Items.Insert(0, new ListItem("Seleccione", "0"));

            //Dptos Jefe
            ddlDptoJefe.DataSource = dptos;
            ddlDptoJefe.DataValueField = "Id_Departamento";
            ddlDptoJefe.DataTextField = "NomDepartamento";
            ddlDptoJefe.DataBind();
            ddlDptoJefe.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        private void CargarCiudades(int idDpto, DropDownList ddl)
        {
            var ciudades = (from c in consultas.Db.Ciudad
                            where c.CodDepartamento == idDpto
                            orderby c.NomCiudad
                            select c).ToList();
            ddl.DataSource = ciudades;
            ddl.DataValueField = "Id_Ciudad";
            ddl.DataTextField = "NomCiudad";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        private void CrearUnidad()
        {
            if(ddlTipoInstitucion.SelectedIndex != 0)
            {
                if(!string.IsNullOrEmpty(txtNombreUnidad.Text))
                {
                    if(!string.IsNullOrEmpty(txtNombreCentroInstitucion.Text))
                    {
                        if(!string.IsNullOrEmpty(txtNit.Text))
                        {
                            if(ddlDpto.SelectedIndex != 0)
                            {
                                if(ddlCiudades.SelectedIndex !=0)
                                {
                                    if(!string.IsNullOrEmpty(txtDireccion.Text))
                                    {
                                        if(!string.IsNullOrEmpty(txtCriterios.Text))
                                        {
                                            if(!string.IsNullOrEmpty(lblJefeUnidad.Text))
                                            {
                                                switch(btn_crearUnidad.Text)
                                                {
                                                    case "Crear":
                                                        var nuevaUnidad = new Datos.Institucion
                                                        {
                                                            NomInstitucion = txtNombreCentroInstitucion.Text.Trim(),
                                                            NomUnidad = txtNombreUnidad.Text.Trim(),
                                                            Nit = Convert.ToDecimal(txtNit.Text.Trim().Replace(".", "")),
                                                            Direccion = txtDireccion.Text.Trim(),
                                                            Telefono = txtTelefonoUnidad.Text.Trim().Replace(".",""),
                                                            Fax = txtFax.Text.Trim().Replace(".", ""),
                                                            CodCiudad = int.Parse(ddlCiudades.SelectedValue.ToString()),
                                                            Inactivo = false,
                                                            CriteriosSeleccion = txtCriterios.Text.Trim(),
                                                            CodTipoInstitucion = Convert.ToByte(ddlTipoInstitucion.SelectedValue.ToString()),
                                                            WebSite = txtWebsite.Text.Trim()
                                                        };

                                                        consultas.Db.Institucions.InsertOnSubmit(nuevaUnidad);
                                                        consultas.Db.SubmitChanges();
                                                        idUnidad = nuevaUnidad.Id_Institucion;
                                                        Session["idUnidad"] = idUnidad;
                                                        break;
                                                    case "Actualizar":
                                                        var unidadEditar = (from inst in consultas.Db.Institucions
                                                                            where inst.Id_Institucion == int.Parse(Session["idUnidad"].ToString())
                                                                            select inst).FirstOrDefault();
                                                        unidadEditar.NomUnidad = txtNombreUnidad.Text;
                                                        unidadEditar.NomInstitucion = txtNombreCentroInstitucion.Text;
                                                        unidadEditar.Direccion = txtDireccion.Text;
                                                        unidadEditar.Nit = Convert.ToDecimal(txtNit.Text.Trim().Replace(".", ""));
                                                        unidadEditar.Telefono = txtTelefonoUnidad.Text.Replace(".", "");
                                                        unidadEditar.Fax = txtFax.Text.Trim().Replace(".", "");
                                                        unidadEditar.CodCiudad = int.Parse(ddlCiudades.SelectedValue.ToString());
                                                        unidadEditar.CriteriosSeleccion = txtCriterios.Text;
                                                        unidadEditar.WebSite = txtWebsite.Text;
                                                        unidadEditar.MotivoCambioJefe = txtCambioJefe.Text;
                                                        consultas.Db.SubmitChanges();
                                                        idUnidad = unidadEditar.Id_Institucion;
                                                        Session["idUnidad"] = idUnidad;
                                                        break;
                                                    case "Reactivar Unidad":
                                                        var unidadReactivar = (from inst in consultas.Db.Institucions
                                                                            where inst.Id_Institucion == int.Parse(Session["idUnidad"].ToString())
                                                                            select inst).FirstOrDefault();
                                                        unidadReactivar.NomUnidad = txtNombreUnidad.Text;
                                                        unidadReactivar.NomInstitucion = txtNombreCentroInstitucion.Text;
                                                        unidadReactivar.Direccion = txtDireccion.Text;
                                                        unidadReactivar.Nit = Convert.ToDecimal(txtNit.Text.Trim().Replace(".", ""));
                                                        unidadReactivar.Telefono = txtTelefonoUnidad.Text.Replace(".", "");
                                                        unidadReactivar.Fax = txtFax.Text.Trim().Replace(".", "");
                                                        unidadReactivar.CodCiudad = int.Parse(ddlCiudades.SelectedValue.ToString());
                                                        unidadReactivar.CriteriosSeleccion = txtCriterios.Text;
                                                        unidadReactivar.WebSite = txtWebsite.Text;
                                                        unidadReactivar.MotivoCambioJefe = txtCambioJefe.Text;
                                                        unidadReactivar.Inactivo = false;
                                                        consultas.Db.SubmitChanges();
                                                        idUnidad = unidadReactivar.Id_Institucion;
                                                        Session["idUnidad"] = idUnidad;
                                                        btn_crearUnidad.Text = "Crear";
                                                        break;
                                                }
                                                //Actualiza los datos del usuario jefe unidad y lo activa si es necesario
                                                var jefeUnidad = (from c in consultas.Db.Contacto
                                                                  where c.Id_Contacto == int.Parse(Session["idJefe"].ToString())
                                                                  select c).FirstOrDefault();
                                                jefeUnidad.CodInstitucion = idUnidad;
                                                jefeUnidad.Inactivo = false;
                                                jefeUnidad.CodCiudad = int.Parse(ddlCiudades.SelectedValue);
                                                consultas.Db.SubmitChanges();

                                                //Actualiza la tabla institución contacto, por si el usuario es jefe de otra unidad le coloca la fecha fin de la actual unidad
                                                var InstContacto = (from ic in consultas.Db.InstitucionContacto
                                                                    where ic.CodInstitucion == idUnidad 
                                                                    select ic).FirstOrDefault();
                                                if(InstContacto != null)
                                                {
                                                    consultas.Db.InstitucionContacto.DeleteOnSubmit(InstContacto);
                                                    consultas.Db.SubmitChanges();
                                                }
                                                var insContac = new Datos.InstitucionContacto
                                                {
                                                    CodInstitucion = idUnidad,
                                                    CodContacto = int.Parse(Session["idJefe"].ToString()),
                                                    FechaInicio = DateTime.Now.Date
                                                };
                                                consultas.Db.InstitucionContacto.InsertOnSubmit(insContac);
                                                consultas.Db.SubmitChanges();

                                                //Borra la relacion del usuario con cualquier otro grupo, solo puede pertenecer a un grupo
                                                var obj = (from gc in consultas.Db.GrupoContactos where gc.CodContacto == int.Parse(Session["idJefe"].ToString())
                                                               select gc).FirstOrDefault();
                                                if (obj != null)
                                                {
                                                    consultas.Db.GrupoContactos.DeleteOnSubmit(obj);
                                                    consultas.Db.SubmitChanges();
                                                }

                                                //Inserta la relacion entre el contacto jefe de la unidad y el grupo de usuarios
                                                var jefeGrupo = new Datos.GrupoContacto
                                                {
                                                    CodGrupo = Constantes.CONST_JefeUnidad,
                                                    CodContacto = int.Parse(Session["idJefe"].ToString())
                                                };
                                                consultas.Db.GrupoContactos.InsertOnSubmit(jefeGrupo);
                                                consultas.Db.SubmitChanges();

                                                //Si tiene proyectos a su cargo, los deja pendientes de asignacion de asesor.
                                                var txtSQL = "SELECT P.NomProyecto, PC.CodProyecto, PC.CodRol, E.Id_Estado, P.Id_Proyecto FROM ProyectoContacto PC,Proyecto P, Estado E " +
                                                    " WHERE FechaFin is NULL  AND PC.CodProyecto = P.Id_Proyecto AND P.CodEstado = E.Id_estado AND PC.CodContacto = " + Session["idJefe"].ToString();

                                                var dt = consultas.ObtenerDataTable(txtSQL, "text");
                                                if(dt.Rows.Count >0)
                                                {
                                                    foreach(DataRow f in dt.Rows)
                                                    {
                                                        if(int.Parse(f.ItemArray[3].ToString()) < Constantes.CONST_AsignacionRecursos)
                                                        {
                                                            //Si al proyecto no le han asignado recursos pasara a REASIGNACION POR ASIGNACION
                                                            var proyecto = (from p in consultas.Db.Proyecto1s
                                                                            where p.Id_Proyecto == int.Parse(f.ItemArray[4].ToString())
                                                                            select p).FirstOrDefault();
                                                            proyecto.CodInstitucion = Constantes.CONST_UnidadTemporal;
                                                            consultas.Db.SubmitChanges();

                                                            //Actualiza la institucion de los contactos activos del proyecto
                                                            txtSQL = "UPDATE Contacto SET CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                                                " WHERE Id_Contacto IN(SELECT CodContacto FROM ProyectoContacto WHERE FechaFin " +
                                                                " IS NULL AND Inactivo = 0 AND CodProyecto = " + f.ItemArray[4].ToString() + ")";
                                                            ejecutaReader(txtSQL, 2);

                                                            //Asigna tarea a jefe de unidad REASIGNACION POR ASIGNACION, para asignacion de asesor
                                                            var instContac = (from ic in consultas.Db.InstitucionContacto
                                                                              where ic.FechaFin == null && ic.CodInstitucion == Constantes.CONST_UnidadTemporal
                                                                              select ic).ToList();
                                                            if(instContac.Count > 0)
                                                            {
                                                                foreach(var ic in instContac)
                                                                {
                                                                    var nuevaTarea = new AgendarTarea(ic.CodContacto, "Asignar Asesor", "Asignar Asesor a el proyecto " + proyecto.NomProyecto,
                                                                        proyecto.Id_Proyecto.ToString(), 3, "0", false, 1, true, false, usuario.IdContacto, "", "", "Asignar Asesor");

                                                                    nuevaTarea.Agendar();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var proyecto = (from p in consultas.Db.Proyecto1s
                                                                            where p.Id_Proyecto == int.Parse(f.ItemArray[4].ToString())
                                                                            select p).FirstOrDefault();

                                                            //Si al proyecto ya le han asignado recursos pasara a REASIGNACION POR SEGUIMIENTO
                                                            txtSQL = "UPDATE Proyecto SET CodInstitucion = " + Constantes.CONST_UnidadTemporal + " WHERE Id_Proyecto = " + f.ItemArray[4].ToString();
                                                            ejecutaReader(txtSQL, 2);
                                                            
                                                            //Actualiza la institucion de los contactos activos del proyecto
                                                            txtSQL = "UPDATE Contacto SET CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                                                " WHERE Id_Contacto IN(SELECT CodContacto FROM ProyectoContacto WHERE FechaFin " +
                                                                " IS NULL AND Inactivo = 0 AND CodProyecto = " + f.ItemArray[4].ToString() + ")";
                                                            ejecutaReader(txtSQL, 2);

                                                            //Asigna tarea a jefe de unidad REASIGNACION POR ASIGNACION, para asignacion de asesor
                                                            var instContac = (from ic in consultas.Db.InstitucionContacto
                                                                              where ic.FechaFin == null && ic.CodInstitucion == Constantes.CONST_UnidadTemporal
                                                                              select ic).ToList();
                                                            if (instContac.Count > 0)
                                                            {
                                                                foreach (var ic in instContac)
                                                                {
                                                                    var nuevaTarea = new AgendarTarea(ic.CodContacto, "Asignar Asesor", "Asignar Asesor a el proyecto " + proyecto.NomProyecto,
                                                                        f.ItemArray[4].ToString(), 3, "0", false, 1, true, false, usuario.IdContacto, "", "", "Asignar Asesor");

                                                                    nuevaTarea.Agendar();
                                                                }
                                                            }
                                                        }
                                                        txtSQL = "UPDATE ProyectoContacto SET FechaFin = GETDATE(), Inactivo = 1 WHERE CodProyecto = " + f.ItemArray[4].ToString() + " AND CodContacto = " + Session["idJefe"].ToString() + " AND Inactivo = 0";
                                                        ejecutaReader(txtSQL, 2);
                                                    }
                                                }

                                                //Envia email
                                                enviarEmail("Registro a Fondo Emprender", usuario.Email, jefeUnidad.Email, jefeUnidad.Clave);


                                                ResetAllControls(this);
                                                Session["idUnidad"] = null;
                                                Session["idJefe"] = null;
                                                idJefe = 0;
                                                idUnidad = 0;
                                                Response.Redirect("CatalogoUnidadEmprende.aspx");
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un jefe para la unidad a crear!')", true);
                                            }
                                        }
                                        else
                                        {
                                            txtCriterios.Focus();
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar criterios de seleccion!')", true);
                                        }
                                    }
                                    else
                                    {
                                        txtDireccion.Focus();
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar la dirección de la unidad!')", true);
                                    }
                                }
                                else
                                {
                                    ddlCiudades.Focus();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar una ciudad!')", true);
                                }
                            }
                            else
                            {
                                ddlDpto.Focus();
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un departamento!')", true);
                            }
                        }
                        else
                        {
                            txtNit.Focus();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el NIT!')", true);
                        }
                    }
                    else
                    {
                        txtNombreCentroInstitucion.Focus();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el nombre del centro o institución!')", true);
                    }
                }
                else
                {
                    txtNombreUnidad.Focus();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el nombre de la unidad a crear!')", true);
                }
            }
            else
            {
                ddlTipoInstitucion.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un tipo de unidad')", true);
            }
        }


        private bool enviarEmail(string asunto, string emailRemitente, string emailDestinatario, string password)
        {
            try
            {
                string bodyTemplate = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta name=\"viewport\" content=\"width=device-width\"/><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/><title>Fondo Emprender</title><style type=\"text/css\">*,.collapse{padding:0}.btn,.social .soc-btn{text-align:center;font-weight:700}.btn,ul.sidebar li a{text-decoration:none;cursor:pointer}.container,table.footer-wrap{clear:both!important}*{margin:0;font-family:\"Helvetica Neue\",Helvetica,Helvetica,Arial,sans-serif}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100%!important;height:100%}.content table,table.body-wrap,table.footer-wrap,table.head-wrap{width:100%}a{color:#2BA6CB}.btn{color:#FFF;background-color:#666;padding:10px 16px;margin-right:10px;display:inline-block}p.callout{padding:15px;background-color:#ECF8FF;margin-bottom:15px}.callout a{font-weight:700;color:#2BA6CB}table.social{background-color:#ebebeb}.social .soc-btn{padding:3px 7px;font-size:12px;margin-bottom:10px;text-decoration:none;color:#FFF;display:block}a.fb{background-color:#3B5998!important}a.tw{background-color:#1daced!important}a.gp{background-color:#DB4A39!important}a.ms{background-color:#000!important}.sidebar .soc-btn{display:block;width:100%}.header.container table td.logo{padding:15px}.header.container table td.label{padding:15px 15px 15px 0}.footer-wrap .container td.content p{border-top:1px solid #d7d7d7;padding-top:15px;font-size:10px;font-weight:700}h1,h2{font-weight:200}h1,h2,h3,h4,h5,h6{font-family:HelveticaNeue-Light,\"Helvetica Neue Light\",\"Helvetica Neue\",Helvetica,Arial,\"Lucida Grande\",sans-serif;line-height:1.1;margin-bottom:15px;color:#000}h1 small,h2 small,h3 small,h4 small,h5 small,h6 small{font-size:60%;color:#6f6f6f;line-height:0;text-transform:none}h1{font-size:44px}h2{font-size:37px}h3,h4{font-weight:500}h3{font-size:27px}h4{font-size:23px}h5,h6{font-weight:900}h5{font-size:17px}h6,p,ul{font-size:14px}h6{text-transform:uppercase;color:#444}.collapse{margin:0!important}p,ul{margin-bottom:10px;font-weight:400;line-height:1.6}p.lead{font-size:17px}p.last{margin-bottom:0}ul li{margin-left:5px;list-style-position:inside}ul.sidebar li,ul.sidebar li a{display:block;margin:0}ul.sidebar{background:#ebebeb;display:block;list-style-type:none}ul.sidebar li a{color:#666;padding:10px 16px;border-bottom:1px solid #777;border-top:1px solid #FFF}.column tr td,.content{padding:15px}ul.sidebar li a.last{border-bottom-width:0}ul.sidebar li a h1,ul.sidebar li a h2,ul.sidebar li a h3,ul.sidebar li a h4,ul.sidebar li a h5,ul.sidebar li a h6,ul.sidebar li a p{margin-bottom:0!important}.container{display:block!important;max-width:600px!important;margin:0 auto!important}.content{max-width:600px;margin:0 auto;display:block}.column{width:300px;float:left}.column-wrap{padding:0!important;margin:0 auto;max-width:600px!important}.column table{width:100%}.social .column{width:280px;min-width:279px;float:left}.clear{display:block;clear:both}@media only screen and (max-width:600px){a[class=btn]{display:block!important;margin-bottom:10px!important;background-image:none!important;margin-right:0!important}div[class=column]{width:auto!important;float:none!important}table.social div[class=column]{width:auto!important}}</style></head> <body bgcolor=\"#FFFFFF\"><table class=\"head-wrap\" bgcolor=\"#FFFFFF\"><tr><td></td><td class=\"header container\" ><div class=\"content\"><table bgcolor=\"#FFFFFF\"><tr><td><img src=\"{{logo}}\"/></td><td align=\"right\"><h6 class=\"collapse\"></h6></td></tr></table></div></td><td></td></tr></table><table class=\"body-wrap\"><tr><td></td><td class=\"container\" bgcolor=\"#FFFFFF\"><div class=\"content\"><table><tr><td><h3>Hola, Bienvenido a Fondo Emprender</h3><p class=\"lead\"> A continuación encontrara las credenciales para entrar a la plataforma.</p><table align=\"left\" class=\"column\"><tr><td><p class=\"\"><strong> Credenciales de acceso : </strong></p><p>Rol : <strong>{{rol}}</strong><br/>Email : <strong>{{email}}</strong><br/> Clave : <strong>{{clave}}</strong> </p></td></tr></table></td></tr></table></div></td><td></td></tr></table></body></html>";
                string urlLogoFondoEmprender = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

                MailMessage mail;
                mail = new MailMessage();
                mail.To.Add(new MailAddress(emailDestinatario));
                mail.From = new MailAddress(emailRemitente);
                mail.Subject = asunto;
                mail.Body = bodyTemplate.ReplaceWord("{{rol}}", "Jefe de Unidad").ReplaceWord("{{email}}", emailDestinatario).ReplaceWord("{{clave}}", password).ReplaceWord("{{logo}}", urlLogoFondoEmprender);
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtpcorp.com", 25);
                using (client)
                {
                    var usuarioEmail = ConfigurationManager.AppSettings.Get("SMTPUsuario");
                    var passwordEmail = ConfigurationManager.AppSettings.Get("SMTPPassword");
                    client.Credentials = new System.Net.NetworkCredential(usuarioEmail, passwordEmail);
                    client.EnableSsl = true;
                    client.Send(mail);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CreaActualizaJefe()
        {
            switch (btnCrerJefe.Text)
            {
                case "Actualizar":
                    var jefe = (from c in consultas.Db.Contacto
                                where c.CodTipoIdentificacion == Convert.ToInt16(ddlTipoIdentificacion.SelectedValue) &&
                                    c.Identificacion == Convert.ToDouble(txtNumIdentificacion.Text.Replace(".", "").Trim())
                                select c).FirstOrDefault();
                    jefe.CodCiudad = int.Parse(ddlCiudadJefe.SelectedValue.ToString());
                    jefe.Telefono = txtTelefono.Text.Trim().Replace(".", "");
                    jefe.Fax = txtFaxJefe.Text.Trim().Replace(".", "");
                    jefe.Cargo = txtCargo.Text;
                    idJefe = jefe.Id_Contacto;
                    Session["idJefe"] = idJefe;
                    consultas.Db.SubmitChanges();
                    break;
                case "Crear":
                    var nuevoJefe = new Datos.Contacto
                    {
                        Nombres = txtNombres.Text,
                        Apellidos = txtApellidos.Text,
                        Identificacion = Convert.ToDouble(txtNumIdentificacion.Text.Replace(".", "").Trim()),
                        CodTipoIdentificacion = Convert.ToInt16(ddlTipoIdentificacion.SelectedValue.ToString()),
                        Cargo = txtCargo.Text,
                        Email = txtEmail.Text,
                        Telefono = txtTelefono.Text.Trim().Replace(".",""),
                        Fax = txtFaxJefe.Text.Trim().Replace(".",""),
                        CodCiudad = int.Parse(ddlCiudadJefe.SelectedValue),
                        Inactivo = false,
                        Clave = GeneraClave(),
                        fechaCreacion = DateTime.Now.Date,
                        InformacionIncompleta = true
                    };

                    consultas.Db.Contacto.InsertOnSubmit(nuevoJefe);
                    consultas.Db.SubmitChanges();
                    var contactoGrupo = new Datos.GrupoContacto
                   {
                       CodContacto = nuevoJefe.Id_Contacto,
                       CodGrupo = Constantes.CONST_JefeUnidad
                   };
                    consultas.Db.GrupoContactos.InsertOnSubmit(contactoGrupo);
                    consultas.Db.SubmitChanges();
                    idJefe = nuevoJefe.Id_Contacto;
                    Session["idJefe"] = idJefe;
                    lblJefeUnidad.Text = nuevoJefe.Nombres + " " + nuevoJefe.Apellidos;
                    break;
            }
            dDatosJefe.Visible = false;
            dNuevaUnidad.Visible = true;
        }

        private void EditarUnidad(GridViewCommandEventArgs e)
        {
            if(e.CommandName =="editar")
            {
                //Cargar datos en el form para editar
                var argumentos = e.CommandArgument.ToString().Split(';');
                
                var unidad = (from i in consultas.Db.Institucions
                              where i.Id_Institucion == int.Parse(argumentos[0])
                              select i).FirstOrDefault();
                var unidadContacto = (from ic in consultas.Db.InstitucionContacto
                                      where ic.CodInstitucion == unidad.Id_Institucion
                                            && ic.FechaFin.Equals(null)
                                      select ic).FirstOrDefault();
                var jefe = (from c in consultas.Db.Contacto
                            where c.Id_Contacto == unidadContacto.CodContacto
                            select c).FirstOrDefault();

                //datos Unidad
                ddlTipoInstitucion.SelectedValue = unidad.CodTipoInstitucion.ToString();
                txtNombreUnidad.Text = unidad.NomUnidad;
                txtNombreCentroInstitucion.Text = unidad.NomInstitucion;
                txtNit.Text = unidad.Nit.ToString();
                var ciudad = (from c in consultas.Db.Ciudad
                              where c.Id_Ciudad == unidad.CodCiudad
                              select c).FirstOrDefault();
                var dpto = (from d in consultas.Db.departamento
                            where d.Id_Departamento == ciudad.CodDepartamento
                            select d).FirstOrDefault();
                ddlDpto.SelectedValue = dpto.Id_Departamento.ToString();
                CargarCiudades(dpto.Id_Departamento, ddlCiudades);
                ddlCiudades.SelectedValue = ciudad.Id_Ciudad.ToString();
                txtDireccion.Text = unidad.Direccion;
                txtCriterios.Text = unidad.CriteriosSeleccion;
                txtCambioJefe.Text = unidad.MotivoCambioJefe;
                lblJefeUnidad.Text = jefe.Nombres + " " + jefe.Apellidos;
                if (!string.IsNullOrEmpty(btn_cambiarDatosJefe.Text))
                {
                    btn_cambiarDatosJefe.Visible = true;
                    txtEditIdJefe.Text = jefe.Id_Contacto.ToString();
                    txtEditJefeNombres.Text = jefe.Nombres;
                    txtEditJefeApellidos.Text = jefe.Apellidos;
                    ddlEditTipoDocJefe.SelectedValue = jefe.CodTipoIdentificacion.ToString();
                    txteditNumeroId.Text = jefe.Identificacion.ToString();
                    txtEditEmailJefe.Text = jefe.Email;
                }
                txtWebsite.Text = unidad.WebSite;
                txtTelefonoUnidad.Text = unidad.Telefono;
                txtFax.Text = unidad.Fax;
                txtNumIdentificacion.Text = jefe.Identificacion.ToString();
                ddlTipoIdentificacion.SelectedValue = jefe.CodTipoIdentificacion.ToString();
                Session["idUnidad"] = unidad.Id_Institucion;
                Session["idJefe"] = jefe.Id_Contacto;
                idUnidad = unidad.Id_Institucion;
                idJefe = jefe.Id_Contacto;
                pnlPrincipaal.Visible = false;
                pnlDetalles.Visible = true;
                btn_crearUnidad.Text = "Actualizar";
            }
            if(e.CommandName == "eliminar")
            {
                var img = e.CommandSource as ImageButton;
                if(img != null)
                {
                    var argumentos = e.CommandArgument.ToString().Split(';');
                    if(img.ImageUrl.Contains("icoBorrar.gif"))
                    {
                        Session["idUnidad"] = argumentos[0];
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('DesactivarUnidadEmprende.aspx', '_blank', 'menubar=0,scrollbars=1,width=360,height=200,top=100');", true);
                    }
                }
            }
            if(e.CommandName == "actualizar")
            {
                idUnidad = int.Parse(e.CommandArgument.ToString());

                var unidad = (from i in consultas.Db.Institucions
                              where i.Id_Institucion == idUnidad
                              select i).FirstOrDefault();
                var unidadContacto = (from ic in consultas.Db.InstitucionContacto
                                      where ic.CodInstitucion == unidad.Id_Institucion
                                      select ic).FirstOrDefault();
                var jefe = (from c in consultas.Db.Contacto
                            where c.Id_Contacto == unidadContacto.CodContacto
                            select c).FirstOrDefault();

                //datos Unidad
                ddlTipoInstitucion.SelectedValue = unidad.CodTipoInstitucion.ToString();
                txtNombreUnidad.Text = unidad.NomUnidad;
                txtNombreCentroInstitucion.Text = unidad.NomInstitucion;
                txtNit.Text = unidad.Nit.ToString();
                var ciudad = (from c in consultas.Db.Ciudad
                              where c.Id_Ciudad == unidad.CodCiudad
                              select c).FirstOrDefault();
                var dpto = (from d in consultas.Db.departamento
                            where d.Id_Departamento == ciudad.CodDepartamento
                            select d).FirstOrDefault();
                ddlDpto.SelectedValue = dpto.Id_Departamento.ToString();
                CargarCiudades(dpto.Id_Departamento, ddlCiudades);
                ddlCiudades.SelectedValue = ciudad.Id_Ciudad.ToString();
                txtDireccion.Text = unidad.Direccion;
                txtCriterios.Text = unidad.CriteriosSeleccion;
                txtCambioJefe.Text = unidad.MotivoCambioJefe;
                lblJefeUnidad.Text = jefe.Nombres + " " + jefe.Apellidos;
                txtWebsite.Text = unidad.WebSite;
                txtTelefonoUnidad.Text = unidad.Telefono;
                txtFax.Text = unidad.Fax;
                txtNumIdentificacion.Text = jefe.Identificacion.ToString();
                ddlTipoIdentificacion.SelectedValue = jefe.CodTipoIdentificacion.ToString();
                Session["idUnidad"] = unidad.Id_Institucion;
                Session["idJefe"] = jefe.Id_Contacto;
                idUnidad = unidad.Id_Institucion;
                idJefe = jefe.Id_Contacto;
                pnlPrincipaal.Visible = false;
                pnlDetalles.Visible = true;
                btn_crearUnidad.Text = "Reactivar Unidad";
            }
        }

        
        private void VolverPrincipal()
        {
            ResetAllControls(this);
            dNuevaUnidad.Visible = false;
            pnlPrincipaal.Visible = true;
        }

        /// <summary>
        /// Resets all controls.
        /// </summary>
        /// <param name="form">The form.</param>
        public static void ResetAllControls(Control form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    var textBox = (TextBox)control;
                    textBox.Text = "";
                }

                if (control is DropDownList)
                {
                    var comboBox = (DropDownList)control;
                    if (comboBox.Items.Count > 0)
                        comboBox.SelectedIndex = 0;
                }

                if (control is CheckBox)
                {
                    var checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }

                if (control is ListBox)
                {
                    var listBox = (ListBox)control;
                    listBox.ClearSelection();
                }
            }
        }
        #endregion

        /// <summary>
        /// Handles the Click event of the lnkUnidadJefe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkUnidadJefe_Click(object sender, EventArgs e)
        {
            var datos = lnkUnidadJefe.Text.Split('-');
            //Session["idJefe"] = datos[0].Trim();
            var jefe = (from c in consultas.Db.Contacto
                        where c.Identificacion == double.Parse(datos[0].Trim())
                        select c).FirstOrDefault();

            if (jefe != null)
            {
                lblJefeUnidad.Text = jefe.Nombres + " " + jefe.Apellidos;
                dNuevaUnidad.Visible = true;
                dListaJefes.Visible = false;
            }
            txtNumIdentificacion.Text = "";
            
        }

        /// <summary>
        /// Handles the Click event of the btn_cambiarDatosJefe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_cambiarDatosJefe_Click(object sender, EventArgs e)
        {
            dNuevaUnidad.Visible = false;
            deditDatosFeje.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the btnClosedEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnClosedEdit_Click(object sender, EventArgs e)
        {
            dNuevaUnidad.Visible = true;
            deditDatosFeje.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the btnUpdateJefe control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnUpdateJefe_Click(object sender, EventArgs e)
        {
            var jefe = (from c in consultas.Db.Contacto
                        where c.Id_Contacto == int.Parse(txtEditIdJefe.Text)
                        select c).FirstOrDefault();
            jefe.Nombres = txtEditJefeNombres.Text.Trim();
            jefe.Apellidos = txtEditJefeApellidos.Text.Trim();
            jefe.CodTipoIdentificacion = short.Parse(ddlEditTipoDocJefe.SelectedValue);
            jefe.Identificacion = double.Parse(txteditNumeroId.Text.Trim());
            jefe.Email = txtEditEmailJefe.Text.Trim();

            consultas.Db.SubmitChanges();
            lblJefeUnidad.Text = txtEditJefeNombres.Text.Trim() + " " + txtEditJefeApellidos.Text.Trim();
            dNuevaUnidad.Visible = true;
            deditDatosFeje.Visible = false;
        }       

    }
}