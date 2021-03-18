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
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Fonade.FONADE.MiPerfil
{
    public partial class CambiarClave : Negocio.Base_Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            l_usuariolog.Text = usuario.Nombres + " " + usuario.Apellidos;
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            RegularExpressionValidatora2.ValidationExpression = Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
        }

        protected void Btn_Cancelar_Click(object sender, EventArgs e)
        {

            string st = "window.onbeforeunload = function(e) { alert('The Window is closing!');};";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", st, true);

            Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
            if (clave.DebeCambiar == 1)
            {

                System.Runtime.Caching.MemoryCache.Default.Dispose();
                Session.Abandon();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
            }

        }

        protected void Btn_cambiarClave_Click(object sender, EventArgs e)
        {
            bool validado = false;

            #region Validando longuitud de caracteres digitados.
            if (txt_claveActual.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual digitada no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else
            { validado = true; }
            if (txt_nuevaclave.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else { validado = true; }
            if (txt_confirmaNuevaClave.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La confirmación de la nueva clave no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else { validado = true; }
            #endregion

            if (validado == true)
            {
                validarClaveActual();
            }
        }

        protected void validarClaveActual()
        {
            var query = (from usu in consultas.Db.Contacto
                         where usu.Id_Contacto == usuario.IdContacto
                         select new
                         {
                             clave = usu.Clave,
                         }).FirstOrDefault();
            string ClaveCifradaActual = Utilidades.Encrypt.GetSHA256(txt_claveActual.Text).ToUpper();
            string ClaveCifradaNueva = Utilidades.Encrypt.GetSHA256(txt_nuevaclave.Text).ToUpper();
            if ((ClaveCifradaActual != query.clave.ToUpper()))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
            }
            else
            {
                if (ClaveCifradaActual == ClaveCifradaNueva)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
                }
                else
                {
                    validarClavesUsadas(ClaveCifradaNueva);
                }
            }
        }


        protected void validarClavesUsadas(string cifrado)
        {
            var query = (from pass in consultas.Db.Clave
                         where pass.NomClave == cifrado
                         & pass.codContacto == usuario.IdContacto
                         select new
                         {
                             pass
                         }).Count();

            if (query == 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    string ClaveScriptada = Utilidades.Encrypt.GetSHA256(txt_nuevaclave.Text).ToUpper();
                    SqlCommand cmd = new SqlCommand("MD_CambiarClave", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
                    cmd.Parameters.AddWithValue("@nuevaclave", ClaveScriptada);
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    SqlCommand cmd3 = new SqlCommand(LogAud(usuario.IdContacto, "se realiza cambio de clave"), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();
                    cmd3.Dispose();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Clave cambiada exitosamente!')", true);
                    // ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "RedirectScript", "window.close()", true);


                    //cierra la session al cambio de la sesion
                    System.Runtime.Caching.MemoryCache.Default.Dispose();
                    Session.Abandon();

                    Response.Redirect("~/Account/Login.aspx");
                }
                finally
                {

                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave ha sido utilizada con anterioridad, por favor utilice una clave diferente!')", true);
            }

        }

        #region AntiguoCODE
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //    l_usuariolog.Text = usuario.Nombres + " " + usuario.Apellidos;
        //    l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
        //    RegularExpressionValidatora2.ValidationExpression = Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
        //}

        //protected void Btn_Cancelar_Click(object sender, EventArgs e)
        //{

        //    string st = "window.onbeforeunload = function(e) { alert('The Window is closing!');};";
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", st, true);

        //    Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
        //    if (clave.DebeCambiar == 1)
        //    {

        //        System.Runtime.Caching.MemoryCache.Default.Dispose();
        //        Session.Abandon();
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        //        Response.Redirect("~/Account/Login.aspx");
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        //    }

        //}

        //protected void Btn_cambiarClave_Click(object sender, EventArgs e)
        //{
        //    bool validado = false;

        //    #region Validando longuitud de caracteres digitados.
        //    if (txt_claveActual.Text.Trim().Length > 20)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual digitada no puede tener mas de 20 caracteres.')", true);
        //        validado = false;
        //    }
        //    else
        //    { validado = true; }
        //    if (txt_nuevaclave.Text.Trim().Length > 20)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede tener mas de 20 caracteres.')", true);
        //        validado = false;
        //    }
        //    else { validado = true; }
        //    if (txt_confirmaNuevaClave.Text.Trim().Length > 20)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La confirmación de la nueva clave no puede tener mas de 20 caracteres.')", true);
        //        validado = false;
        //    }
        //    else { validado = true; }
        //    #endregion

        //    if (validado == true)
        //    {
        //        validarClaveActual();
        //    }
        //}

        //protected void validarClaveActual()
        //{
        //    var query = (from usu in consultas.Db.Contacto
        //                 where usu.Id_Contacto == usuario.IdContacto
        //                 select new
        //                 {
        //                     clave = usu.Clave,
        //                 }).FirstOrDefault();
        //    string ClaveCifradaActual = Utilidades.Encrypt.GetSHA256(txt_claveActual.Text);
        //    string ClaveCifradaNueva = Utilidades.Encrypt.GetSHA256(txt_nuevaclave.Text);
        //    if ((ClaveCifradaActual != query.clave) && txt_claveActual.Text != query.clave)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
        //    }
        //    else
        //    {
        //        if (ClaveCifradaActual == txt_nuevaclave.Text)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
        //        }
        //        else
        //        {
        //            validarClavesUsadas(ClaveCifradaNueva);
        //        }
        //    }
        //}


        //protected void validarClavesUsadas(string cifrado)
        //{
        //    var query = (from pass in consultas.Db.Clave
        //                 where pass.NomClave == cifrado
        //                 & pass.codContacto == usuario.IdContacto
        //                 select new
        //                 {
        //                     pass
        //                 }).Count();

        //    if (query == 0)
        //    {
        //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        //        try
        //        {
        //            string ClaveScriptada = Utilidades.Encrypt.GetSHA256(txt_nuevaclave.Text);
        //            SqlCommand cmd = new SqlCommand("MD_CambiarClave", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
        //            cmd.Parameters.AddWithValue("@nuevaclave", ClaveScriptada);
        //            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
        //            SqlCommand cmd3 = new SqlCommand(LogAud(usuario.IdContacto, "se realiza cambio de clave"), con);
        //            con.Open();
        //            cmd2.ExecuteNonQuery();
        //            cmd.ExecuteNonQuery();
        //            cmd3.ExecuteNonQuery();
        //            cmd2.Dispose();
        //            cmd.Dispose();
        //            cmd3.Dispose();
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Clave cambiada exitosamente!')", true);
        //            // ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "RedirectScript", "window.close()", true);


        //            //cierra la session al cambio de la sesion
        //            System.Runtime.Caching.MemoryCache.Default.Dispose();
        //            Session.Abandon();

        //            Response.Redirect("~/Account/Login.aspx");
        //        }
        //        finally
        //        {

        //            con.Close();
        //            con.Dispose();
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave ha sido utilizada con anterioridad, por favor utilice una clave diferente!')", true);
        //    }

        //}

        #endregion
    }
}