#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Archivo>BuscarProyecto.cs</Archivo>

#endregion

using Datos;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{

    class BusquedaProeycto
    {
        public int Id_Proyecto { get; set; }
        public string NomProyecto { get; set; }
        public int? idOperador { get; set; }
        public string Operador { get; set; }
    }
    /// <summary>
    /// BuscarProyecto
    /// </summary>    
    public partial class BuscarProyecto : Negocio.Base_Page
    {
        /// <summary>
        /// Diego Quiñonez
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //agrega la propiedad de validacion al textbox
            //con el fin que solo deje ingresar numeros en el
            txtnoproyecto.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
        }

        /// <summary>
        /// Diego Quiñonez
        /// metodo asociado al boton buscar
        /// se encarga de buscar un proyecto de acuerdo al id o a alguna letra o palabra que este contenida dentro del nombre del proyecto
        /// en caso de no coincidir muestra toda la lista de proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnbuscar_Click(object sender, EventArgs e)
        {
            List<BusquedaProeycto> resul = new List<BusquedaProeycto>();

            if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
            {
                //devuelove todos los proyectos de acuerdo a un estado
                resul = (from p in consultas.Db.Proyecto
                         where p.CodEstado == 7
                         select new BusquedaProeycto
                         {
                             Id_Proyecto = p.Id_Proyecto,
                             NomProyecto = p.NomProyecto,
                             idOperador = p.codOperador
                         }).ToList();
            }
            else
            {
                resul = (from p in consultas.Db.Proyecto
                         where p.CodEstado == 7
                         && p.codOperador == usuario.CodOperador
                         select new BusquedaProeycto
                         {
                             Id_Proyecto = p.Id_Proyecto,
                             NomProyecto = p.NomProyecto,
                             idOperador = p.codOperador
                         }).ToList();
            }



            //si busca por nombre lo filtra
            if (!string.IsNullOrEmpty(txtProyecto.Text)) 
                resul = resul.Where(p => p.NomProyecto.ToUpper().Contains(txtProyecto.Text.ToUpper())).ToList();

            //si busca por id lo filtra
            if (!string.IsNullOrEmpty(txtnoproyecto.Text))
                resul = resul.Where(p => p.Id_Proyecto == Convert.ToInt32(txtnoproyecto.Text)).ToList();

            if (resul.Count > 0)
            {
                foreach (var r in resul)
                {
                    if(r.idOperador!=null)
                    r.Operador = operadorController.getOperador(r.idOperador).NombreOperador;
                }
            }


            gvproyectos.DataSource = resul;
            gvproyectos.DataBind();
        }

        OperadorController operadorController = new OperadorController();

        /// <summary>
        /// Diego Quiñonez
        /// evento asociado a la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvproyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //recoge los parametros de la fila de la grilla desde la que se alla disparado el evento
            string[] param = e.CommandArgument.ToString().Split(';');

            //guarda en session los parametros para ser utilizados
            //en la pagina de llamada
            HttpContext.Current.Session["CodigoProyecto"] = Convert.ToInt64(param[0]);
            HttpContext.Current.Session["NombreProyecto"] = param[1];

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;", true);
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);

        }
    }
}