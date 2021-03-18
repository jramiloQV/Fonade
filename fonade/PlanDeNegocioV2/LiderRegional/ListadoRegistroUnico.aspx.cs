using Datos;
using Fonade.Negocio.PlanDeNegocioV2.LiderRegional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.LiderRegional
{
    public partial class ListadoRegistroUnico : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarDLL(usuario.CodGrupo);
                cargarGrid(usuario.CodGrupo);
            }

        }

        private void cargarDLL(int _codGrupo)
        {

            if (_codGrupo == Constantes.CONST_AdministradorSistema ||
                _codGrupo == Constantes.CONST_CallCenter ||
                _codGrupo == Constantes.CONST_CallCenterOperador)
            {
                cargarDropDLCentroDesarrollo();
            }
            else if (_codGrupo == Constantes.CONST_LiderRegional)
            {
                cargarDropDLCentroDesarrolloFiltrarLiderRegional(usuario.IdContacto);
            }

        }

        private void cargarGrid(int _codGrupo)
        {
            if (_codGrupo == Constantes.CONST_AdministradorSistema ||
                _codGrupo == Constantes.CONST_CallCenter ||
                _codGrupo == Constantes.CONST_CallCenterOperador)
            {
                cargarGrid();
            }
            else if (_codGrupo == Constantes.CONST_LiderRegional)
            {
                cargarGridLiderRegional(usuario.IdContacto);
            }
        }

        private void cargarDropDLCentroDesarrollo()
        {
            List<ListItem> listItems = new List<ListItem>();

            List<ListItem> centrosDesarrollo = LiderRegionalBLL.getItemCentrosDesarrollo();

            ListItem item = new ListItem
            {
                Value = "0",
                Text = "Seleccione..."
            };

            listItems.Add(item);
            listItems.AddRange(centrosDesarrollo);

            ddlCentroDesarrollo.DataTextField = "Text";
            ddlCentroDesarrollo.DataValueField = "Value";
            ddlCentroDesarrollo.DataSource = listItems;
            ddlCentroDesarrollo.DataBind();
        }

        private void cargarDropDLCentroDesarrolloFiltrarLiderRegional(int _codusuario)
        {
            List<ListItem> listItems = new List<ListItem>();

            List<ListItem> centrosDesarrollo = LiderRegionalBLL.getItemCentrosDesarrolloXLider(_codusuario);

            ListItem item = new ListItem
            {
                Value = "0",
                Text = "Seleccione..."
            };

            listItems.Add(item);
            listItems.AddRange(centrosDesarrollo);

            ddlCentroDesarrollo.DataTextField = "Text";
            ddlCentroDesarrollo.DataValueField = "Value";
            ddlCentroDesarrollo.DataSource = listItems;
            ddlCentroDesarrollo.DataBind();
        }

        private void cargarGrid()
        {
            List<LiderRegionalRUDTO> datos = LiderRegionalBLL.getDatosRegistroUnico();
            Session["DatosRU"] = datos;
            gvRegistros.DataSource = datos;
            gvRegistros.DataBind();
        }
        
        private void cargarGridLiderRegional(int _codUsuario)
        {
            List<LiderRegionalRUDTO> datos = LiderRegionalBLL.getDatosRegistroUnicoXLiderRegional(_codUsuario);
            Session["DatosRU"] = datos;
            gvRegistros.DataSource = datos;
            gvRegistros.DataBind();
        }

        private void filtrar(int _codCentro)
        {
            List<LiderRegionalRUDTO> datos = LiderRegionalBLL.getDatosRegistroUnicoFiltrarCentro(_codCentro);
            Session["DatosRU"] = datos;
            gvRegistros.DataSource = datos;
            gvRegistros.DataBind();
        }

        protected void gvRegistros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRegistros.PageIndex = e.NewPageIndex;
            gvRegistros.DataSource = (List<LiderRegionalRUDTO>)Session["DatosRU"];
            gvRegistros.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (ddlCentroDesarrollo.SelectedValue.ToString() == "0")
            {
                cargarGrid(usuario.CodGrupo);
            }
            else
            {
                filtrar(Convert.ToInt32(ddlCentroDesarrollo.SelectedValue));
            }
        }
    }
}