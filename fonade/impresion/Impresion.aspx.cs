using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;

namespace Fonade.impresion
{
    public partial class Impresion : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //tv_tab.Nodes.Clear();
            if (!IsPostBack)
            {
                //llenarTreeView();
                tv_tab.Attributes.Add("onclick", "postBackByObject()");
                //enlaceproyecto(txt_codProyecto.Text.Trim());
            }
        }

        public void enlaceproyecto(string param)
        {
            String txtSQL = "SELECT id_proyecto, CONVERT(varchar(max), id_proyecto) + ' - ' + nomproyecto nomproyecto, nomciudad, nomdepartamento, inactivo, IdVersionProyecto FROM proyecto , ciudad, departamento WHERE id_ciudad=codciudad and id_departamento = coddepartamento ";
            DataTable datatable = new DataTable();

            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_AdministradorSena:
                    txtSQL = txtSQL + " and inactivo=0 ";
                    break;
                case Constantes.CONST_JefeUnidad:
                    txtSQL = txtSQL + " and codinstitucion = " + usuario.CodInstitucion;
                    break;
                case Constantes.CONST_Asesor:
                    txtSQL = txtSQL + " and inactivo=0 and codinstitucion = " + usuario.CodInstitucion + " and  exists (select codproyecto from proyectocontacto pc  where id_proyecto=codproyecto and pc.codcontacto=" + usuario.IdContacto + " and pc.inactivo=0)";
                    break;
                case Constantes.CONST_Emprendedor:
                    txtSQL = txtSQL + " and inactivo=0 and codinstitucion = " + usuario.CodInstitucion + " and  exists (select codproyecto from proyectocontacto pc  where id_proyecto=codproyecto and pc.codcontacto=" + usuario.IdContacto + " and pc.inactivo=0)";
                    break;
                case Constantes.CONST_Evaluador:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = "+usuario.CodOperador+" and  exists (select codproyecto from proyectocontacto pc  where id_proyecto=codproyecto and pc.codcontacto=" + usuario.IdContacto + " and pc.inactivo=0)";
                    break;
                case Constantes.CONST_CoordinadorEvaluador:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador + "  and  exists (select codproyecto from proyectocontacto pc  where id_proyecto=codproyecto and pc.codcontacto=" + usuario.IdContacto + " and pc.inactivo=0)";
                    break;
                case Constantes.CONST_GerenteEvaluador:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador + " and codestado>=" + Constantes.CONST_Convocatoria;
                    break;
                case Constantes.CONST_Interventor:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador + " and id_proyecto in  (select distinct Codproyecto from empresaInterventor EI, empresa E  where id_empresa=codempresa and EI.inactivo=0 and EI.CodContacto = " + usuario.IdContacto + ")";
                    break;
                case Constantes.CONST_CoordinadorInterventor:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador + " and id_proyecto in  (select distinct Codproyecto from empresaInterventor EI, empresa E  where id_empresa=codempresa and EI.inactivo=0)";
                    break;
                case Constantes.CONST_GerenteInterventor:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador + " and id_proyecto in  (select distinct Codproyecto from empresaInterventor EI, empresa E  where id_empresa=codempresa and EI.inactivo=0)";
                    break;
                case Constantes.CONST_CallCenter:
                    txtSQL = txtSQL + " and inactivo=0";
                    break;
                case Constantes.CONST_CallCenterOperador:
                    txtSQL = txtSQL + " and inactivo=0 and codOperador = " + usuario.CodOperador;
                    break;
            }

            //Si el parámetro "es decir, el código del proyecto" tiene datos, lo agrega a la consulta.
            if (param.Trim() != "") { txtSQL = txtSQL + "  and Id_proyecto = " + param; }

            txtSQL = txtSQL + " order by nomProyecto";

            datatable = consultas.ObtenerDataTable(txtSQL, "text");

            if (datatable.Rows.Count > 0)
            {
                //DDL_proyecto.Items.Clear();

                //DDL_proyecto.Items.Add(new ListItem() { Text = "", Value = "0|0" });

                lblNombrePlanNeg.Text = datatable.Rows[0]["nomproyecto"].ToString().htmlDecode();

                lblBusquedaPlanNegocio.Text = datatable.Rows[0]["id_proyecto"].ToString() + "|" 
                                        + datatable.Rows[0]["IdVersionProyecto"].ToString() 
                                        + "|" + datatable.Rows[0]["nomproyecto"].ToString().htmlDecode();

                //foreach (DataRow row in datatable.Rows)
                //{
                //    ListItem item = new ListItem();
                //    item.Text = row["nomproyecto"].ToString().htmlDecode();
                //    item.Value = row["id_proyecto"].ToString() + "|" + row["IdVersionProyecto"].ToString() + "|" + row["nomproyecto"].ToString().htmlDecode();
                //    DDL_proyecto.Items.Add(item);

                //    lblNombrePlanNeg.Text = item.Text;
                //    lblBusquedaPlanNegocio.Text = item.Value;
                //}

                int version = int.Parse(lblBusquedaPlanNegocio.Text.Split('|')[1]);

                llenarTreeView(version);
            }
            else
            {
                Utilidades.PresentarMsj("No se encontraron resultados en la búsqueda", this, "Alert");
            }

            txt_codProyecto.Text = "";
        }

        private void llenarTreeView(int idversion)
        {
            String txtSQL = "SELECT  isnull(codtab, id_tab) orden, id_tab, nomTab, IdVersionProyecto FROM Tab WHERE isnull(codtab, id_tab) not in(" + Constantes.CONST_PlanOperativo + ", " 
                + Constantes.CONST_Anexos + ", " 
                + Constantes.CONST_PlanOperativoV2 + ", " 
                + Constantes.CONST_AnexosV2 + ", " 
                + Constantes.CONST_EmpresaV2 + ", " 
                + Constantes.CONST_ContratoV2 + "," 
                + Constantes.CONST_Avance + "," 
                + Constantes.CONST_IndicadoresDeGestion+") ORDER BY orden, id_tab";

            DataTable datatable = new DataTable();
            datatable = consultas.ObtenerDataTable(txtSQL, "text");

            List<Tab> lista1 = new List<Tab>();

            foreach (DataRow fila in datatable.Rows)
            {
                int idversiontab = Convert.ToInt32(fila["IdVersionProyecto"].ToString());

                if (idversiontab == idversion)
                {
                    lista1.Add(new Tab()
                    {
                        orden = Convert.ToInt32(fila["orden"].ToString()),
                        id_tab = Convert.ToInt32(fila["id_tab"].ToString()),
                        nomTab = fila["nomTab"].ToString()
                    });

                }
            }

            BindTree(lista1);
        }

        private void BindTree(IEnumerable<Tab> list)
        {
            var nodes = list;

            tv_tab.Nodes.Clear();

            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.nomTab, Convert.ToString(node.id_tab));

                if (node.id_tab != node.orden)
                {
                    TreeNode nodeAux = tv_tab.Nodes[tv_tab.Nodes.Count - 1];
                    nodeAux.ChildNodes.Add(newNode);
                }
                else
                {
                    tv_tab.Nodes.Add(newNode);
                }

                newNode.SelectAction = TreeNodeSelectAction.None;
                newNode.ShowCheckBox = true;
            }
        }

        protected void btnimpresion_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            //string[] datos = DDL_proyecto.SelectedValue.Split('|');
            string[] datos = lblBusquedaPlanNegocio.Text.Split('|');

            if (string.IsNullOrEmpty(datos[0]))
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No ha subido ningun archivo');</script>");
                return;
            }

            if (int.Parse(datos[0]) == 0)
            {
                Utilidades.PresentarMsj("Debe seleccionar un proyecto o buscar un código de proyecto válido", this, "Alert");
                return;
            }

            HttpContext.Current.Session["codProye"] = datos[0];
            HttpContext.Current.Session["listaTap"] = tv_tab;

            if (int.Parse(datos[1]) == Constantes.CONST_PlanV1)
            {
                Response.Redirect("VerImpresion.aspx");
            }
            else
            {
                string[] listaSel = tv_tab.Nodes.OfType<TreeNode>()
                                 .SelectMany(x => new[] { x }.Concat(x.ChildNodes.OfType<TreeNode>()))
                                 .Where(x => x.Checked)
                                 .Select(x => x.Value)
                                 .ToArray();

                if(listaSel.Count() == 0)
                {
                    listaSel = new string[] { "-1" };
                }

                Fonade.Proyecto.Proyectos.Redirect(Response, @"~\PlanDeNegocioV2\ImpresionPlanNegocio\VerImpresion.aspx?ListaSel=" + string.Join(",",listaSel) + "&CodigoProyecto=" + datos[0] + "&NomProyecto=" + datos[2].Replace("\"","").Replace("'",""), "_Blank", string.Empty);
            }

        }

        protected void tv_tab_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            foreach (TreeNode nodeHijo in e.Node.ChildNodes)
            {
                nodeHijo.Checked = e.Node.Checked;
            }
        }

        protected void lnk_buscarProyectos_Click(object sender, EventArgs e)
        {
            int aux = 0;

            if (!int.TryParse(txt_codProyecto.Text.Trim(), out aux))
            {
                Utilidades.PresentarMsj("Debe escribir un código de proyecto válido", this, "Alert");
                return;
            }

            enlaceproyecto(txt_codProyecto.Text.Trim());
        }

        protected void DDL_proyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int version = int.Parse(DDL_proyecto.SelectedValue.Split('|')[1]);

            llenarTreeView(version);
        }
    }

    public class Tab
    {
        public int orden;
        public int id_tab;
        public string nomTab;
    }
}