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

namespace Fonade.FONADE.interventoria
{
    public partial class VerInterventoresCoordinador : Negocio.Base_Page
    {


        protected int CodContacto;

        public DataTable dtInterventores;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                ProcesoCoordInterventor();
                GridView1.Visible = true;
                GridView2.Visible = false;
            }
            else
            {
                ProcesoGerenteIner();
                GridView1.Visible = false;
                GridView2.Visible = true;
            }
        }

        protected string EmpresasByInterventor(string id)
        {
            var resp = "-";
            var txtSql = "SELECT DISTINCT UPPER(RazonSocial) FROM EmpresaInterventor, Empresa " +
                "WHERE Id_Empresa = CodEmpresa AND Inactivo = 0  AND CodContacto = " + id;
            var dt = consultas.ObtenerDataTable(txtSql, "text");

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow f in dt.Rows)
                {
                    resp += f.ItemArray[0].ToString().ToUpper() + "<br />-";
                }
                resp = resp.TrimEnd('-');
            }
            else
            {
                resp = " ";
            }

            return resp;
        }

        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }

        private void ProcesoCoordInterventor()
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);

            var query = (from x in consultas.Db.Contacto
                         where x.Id_Contacto == CodContacto
                         select new
                         {
                             nombre = x.Nombres + " " + x.Apellidos,
                         }).FirstOrDefault();

            lbl_Titulo.Text = "EMPRESAS PARA: " + query.nombre; // void_establecerTitulo();
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            var empresas = (from ei in consultas.Db.EmpresaInterventors
                            join r in consultas.Db.Rols on ei.Rol equals r.Id_Rol
                            join em in consultas.Db.Empresas on ei.CodEmpresa equals em.id_empresa
                            where ei.Inactivo == false && (ei.Rol == Constantes.CONST_RolInterventor || ei.Rol == Constantes.CONST_RolInterventorLider) && ei.CodContacto == CodContacto
                            select new
                            {
                                em.razonsocial,
                                rol = r.Nombre,
                                ei.FechaInicio
                            }).ToList();

            GridView1.DataSource = empresas;
            GridView1.DataBind();
        }

        private void ProcesoGerenteIner()
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);

            var query = (from x in consultas.Db.Contacto
                         where x.Id_Contacto == CodContacto
                         select new
                         {
                             nombre = x.Nombres + " " + x.Apellidos,
                         }).FirstOrDefault();

            lbl_Titulo.Text = "INTERVENTORES PARA: " + query.nombre; 
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            var txtSQL = "SELECT c.Id_Contacto Id, c.Nombres + ' ' + c.Apellidos Nombres FROM Contacto c " +
                "INNER JOIN Interventor i ON c.Id_Contacto = i.CodContacto " +
                "WHERE  i.CodCoordinador = " + CodContacto;

            var interventor = consultas.ObtenerDataTable(txtSQL, "text");

            GridView2.DataSource = interventor;
            GridView2.DataBind();
        }
    }
}