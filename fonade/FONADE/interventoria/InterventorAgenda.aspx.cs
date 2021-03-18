using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.FONADE.interventoria
{
    public partial class InterventorAgenda : Negocio.Base_Page
    {
        String txtSQL = String.Empty;
        String ROL = String.Empty;
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor || ROL == Constantes.CONST_RolInterventorLider.ToString())
            {
                btnNuevaVisita.Visible = true;
                btnNuevaVisita.Enabled = true;
            }
            else
            {
                btnNuevaVisita.Enabled = false;
                btnNuevaVisita.Visible = false;
            }
            if (!IsPostBack)
            {
                ROL = HttpContext.Current.Session["CodRol"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodRol"].ToString()) ? HttpContext.Current.Session["CodRol"].ToString() : "0";
                txtSQL = "UPDATE Visita SET Estado='Realizada' WHERE ID_Visita IN (SELECT Id_Visita FROM Visita WHERE FechaFin < getdate())";

                ejecutaReader(txtSQL, 2);

                txtSQL = "UPDATE Visita SET Estado='Pendiente' WHERE ID_Visita IN (SELECT Id_Visita FROM Visita WHERE FechaFin >= getdate())";

                ejecutaReader(txtSQL, 2);

                txtSQL = "SELECT CodContacto,Rol From EmpresaInterventor,Empresa Where id_empresa=codempresa and CodContacto =" + usuario.IdContacto + " and inactivo=0 and FechaFin is null order by rol desc";

                var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

                if (reader.Rows.Count > 0)
                {
                    if (reader.Rows.Count > 0)
                    {
                        HttpContext.Current.Session["CodRol"] = reader.Rows[0].ItemArray[1].ToString(); //["Rol"].ToString();
                    }
                }
                else
                {
                    HttpContext.Current.Session["CodRol"] = null;
                }
                llenarGrilla();
            }
            
            
        }

        private void llenarGrilla()
        {
            //todo: paginar grid

            var dt2 = (from v in consultas.Db.Visita
                       join emp in consultas.Db.Empresas on v.Id_Empresa equals emp.id_empresa
                       where v.Id_Interventor == usuario.IdContacto
                       orderby emp.razonsocial
                       select new listaVisita
                       {
                           razonsocial = emp.razonsocial,
                           Nit = emp.Nit,
                           Id_Visita = v.Id_Visita,
                           FechaInicio = FieldValidate.getShortFechaConFormato(v.FechaInicio),
                           FechaFin = FieldValidate.getShortFechaConFormato(v.FechaFin),
                           Estado = v.Estado.ToLower()
                       }).ToList();

            gv_agenda.DataSource = dt2;
            gv_agenda.DataBind();
        }
        /// <summary>
        ///Pedro V. Carreño.
        /// 13/11/2014.
        /// Falta el filtro por letra ERROR INT-82 
        /// </summary>
        /// <param name="usuario.IdContacto">Id del contacto: DEBE SER Id de un usuario creado en la aplicaciòn.</param>
        /// <param name="letra">Letra: Letra del alfabeto para filtrar el grid.</param>
        private void llenarGrilla(string letra)
        {            
            txtSQL = @"SELECT Empresa.razonsocial, Empresa.Nit, Visita.Id_Visita, Visita.FechaInicio, Visita.FechaFin, lower(Visita.Estado) as Estado
                        FROM Visita
                        INNER JOIN Empresa ON Visita.Id_Empresa = Empresa.id_empresa
                        WHERE (Visita.Id_Interventor = " + usuario.IdContacto + @")
                        and Empresa.razonsocial like '" + letra + @"%'
                        ORDER BY Empresa.razonsocial";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            gv_agenda.DataSource = dt;
            gv_agenda.DataBind();
        }

        // Pedro V. Carreño  Falta el filtro por letra ERROR INT-82 13/1/2014 - FIN
        protected void gv_agenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id_visita = e.CommandArgument.ToString();

            HttpContext.Current.Session["Id_Visita"] = id_visita;
            HttpContext.Current.Session["Tipo"] = 2;
            // Pedro V. Carreño  Al mirar una visita ya creada No tienen los botones de Borrar y cerrar ERROR INT-80 - 14/11/2014
            Redirect(null, "AdicionarVisita.aspx", "_blank", "width=850,height=230");
        }

        protected void btnNuevaVisita_Click(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
            {
                HttpContext.Current.Session["Id_Visita"] = "0";
                HttpContext.Current.Session["Tipo"] = "1";
                // Pedro V. Carreño  Al mirar una visita ya creada No tienen los botones de Borrar y cerrar ERROR INT-80 - 14/11/2014
                Redirect(null, "AdicionarVisita.aspx", "_blank", "width=850,height=230");
            }
        }

        /// <summary>
        ///Pedro V. Carreño.
        /// 13/11/2014.
        /// Falta el filtro por letra ERROR INT-82 
        /// </summary>
        protected void A_Click(object sender, EventArgs e)
        {
           llenarGrilla("A");
        }

        protected void B_Click(object sender, EventArgs e)
        {
            llenarGrilla("B");
        }

        protected void C_Click(object sender, EventArgs e)
        {
            llenarGrilla("C");
        }

        protected void D_Click(object sender, EventArgs e)
        {
            llenarGrilla("D");
        }

        protected void E_Click(object sender, EventArgs e)
        {
            llenarGrilla("E");
        }

        protected void F_Click(object sender, EventArgs e)
        {
            llenarGrilla("F");
        }

        protected void G_Click(object sender, EventArgs e)
        {
            llenarGrilla("G");
        }

        protected void H_Click(object sender, EventArgs e)
        {
            llenarGrilla("H");
        }

        protected void I_Click(object sender, EventArgs e)
        {
            llenarGrilla("I");
        }

        protected void J_Click(object sender, EventArgs e)
        {
            llenarGrilla("J");
        }

        protected void K_Click(object sender, EventArgs e)
        {
            llenarGrilla("K");
        }

        protected void L_Click(object sender, EventArgs e)
        {
            llenarGrilla("L");
        }

        protected void M_Click(object sender, EventArgs e)
        {
            llenarGrilla("M");
        }

        protected void N_Click(object sender, EventArgs e)
        {
            llenarGrilla("N");
        }

        protected void O_Click(object sender, EventArgs e)
        {
            llenarGrilla("O");
        }

        protected void P_Click(object sender, EventArgs e)
        {
            llenarGrilla("P");
        }

        protected void Q_Click(object sender, EventArgs e)
        {
            llenarGrilla("Q");
        }

        protected void R_Click(object sender, EventArgs e)
        {
            llenarGrilla("R");
        }

        protected void S_Click(object sender, EventArgs e)
        {
            llenarGrilla("S");
        }

        protected void T_Click(object sender, EventArgs e)
        {
            llenarGrilla("T");
        }

        protected void U_Click(object sender, EventArgs e)
        {
            llenarGrilla("U");
        }

        protected void V_Click(object sender, EventArgs e)
        {
            llenarGrilla("V");
        }

        protected void W_Click(object sender, EventArgs e)
        {
            llenarGrilla("W");
        }

        protected void X_Click(object sender, EventArgs e)
        {
            llenarGrilla("X");
        }

        protected void Y_Click(object sender, EventArgs e)
        {
            llenarGrilla("Y");
        }

        protected void Z_Click(object sender, EventArgs e)
        {
            llenarGrilla("Z");
        }
        // Pedro V. Carreño  Falta el filtro por letra ERROR INT-82 13/1/2014 - FIN
    }
}


public class listaVisita
{
    public string razonsocial { get; set; }
    public string Nit { get; set; }
    public int Id_Visita { get; set; }
    public string FechaInicio { get; set; }
    public string FechaFin { get; set; }
    public string Estado { get; set; }
}