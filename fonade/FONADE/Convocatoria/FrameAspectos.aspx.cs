using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// FrameAspectos
    /// </summary>    
    public partial class FrameAspectos : Negocio.Base_Page
    {
        /// <summary>
        /// Gets the identifier version proyecto.
        /// </summary>
        /// <value>
        /// The identifier version proyecto.
        /// </value>
        public int IdVersionProyecto
        {
            get { return Request.QueryString.AllKeys.Contains("IdVersionProyecto") ? int.Parse(Request.QueryString["IdVersionProyecto"]) : 0; }
        }

        string Id_EvalConvocatoriaAS;
        string txtSQL;

        string codPadre;
        string codCampo;

        delegate string del(bool x);

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["Id_EvalConvocatoria"] = Request.QueryString["Id_EvalConvocatoria"];
                codCampo = HttpContext.Current.Session["codCampoAspecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codCampoAspecto"].ToString()) ? HttpContext.Current.Session["codCampoAspecto"].ToString() : "0";
                tv_aspectos.Nodes.Clear();

                crearLista();
            }
            if (IdVersionProyecto.Equals(Datos.Constantes.CONST_PlanV2))
                tv_aspectos.SelectedNodeChanged -= tv_aspectos_SelectedNodeChanged;
        }

        private void crearLista()
        {
            Id_EvalConvocatoriaAS = Session["Id_EvalConvocatoria"].ToString();

			//Se agrega Inactivo = 0 para no cargar los desactivados - William P.L.
			txtSQL = "SELECT id_campo, campo, codConvocatoria FROM Campo C " +
					" LEFT JOIN convocatoriaCampo CC ON C.id_campo = CC.codcampo " +
					" and codConvocatoria = " + Id_EvalConvocatoriaAS +
					" WHERE c.codcampo is null AND c.IdVersionProyecto = " + IdVersionProyecto +
					" and C.Inactivo = 0";

			var dt = consultas.ObtenerDataTable(txtSQL, "text");

            List<Lista1> lista1 = new List<Lista1>();

            foreach (DataRow dr in dt.Rows)
            {
                lista1.Add(new Lista1()
                {
                    id_campo = Convert.ToInt32(dr["id_campo"].ToString()),
                    campo = dr["campo"].ToString(),
                    codConvocatoria = !string.IsNullOrEmpty(dr["codConvocatoria"].ToString()) ? Convert.ToInt32(dr["codConvocatoria"].ToString()) : 0,
                    nivel = 0
                });

                txtSQL = "SELECT id_campo, campo FROM Campo WHERE codCampo = " + dr["id_campo"].ToString();

                var dtH1 = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow drH1 in dtH1.Rows)
                {
                    lista1.Add(new Lista1()
                    {
                        id_campo = Convert.ToInt32(drH1["id_campo"].ToString()),
                        campo = drH1["campo"].ToString(),
                        nivel = 1
                    });

					//Se agrega and C.Inactivo = 0 para no mostrar en la seleccion de aspectos William P.L.
					txtSQL = "SELECT id_campo, campo, codConvocatoria " +
                        "FROM Campo C LEFT JOIN convocatoriaCampo CC ON C.id_campo = CC.codcampo and codConvocatoria = " + Id_EvalConvocatoriaAS + " " +
                        "WHERE C.codCampo = " + drH1["id_campo"].ToString() + " and C.Inactivo = 0";

					var dtH2 = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow drH2 in dtH2.Rows)
                    {
                        lista1.Add(new Lista1()
                        {
                            id_campo = Convert.ToInt32(drH2["id_campo"].ToString()),
                            campo = drH2["campo"].ToString(),
                            codConvocatoria = !string.IsNullOrEmpty(dr["codConvocatoria"].ToString()) ? Convert.ToInt32(dr["codConvocatoria"].ToString()) : 0,
                            nivel = 2
                        });
                    }
                }
            }

            BindTree(lista1);

            tv_aspectos.CollapseAll();

            //chequea todos los nodos para una nueva convocatoria, nuevo plan
            if (lista1.FirstOrDefault().codConvocatoria == 0 && IdVersionProyecto == Datos.Constantes.CONST_PlanV2)
                ChequearTodos();

        }

        void ChequearTodos()
        {
            foreach (TreeNode item in tv_aspectos.Nodes)
            {
                foreach (TreeNode chil in item.ChildNodes)
                {
                    foreach (TreeNode nieto in chil.ChildNodes)
                    {
                        nieto.Checked = true;
                    }
                    chil.Checked = true;
                }
                item.Checked = true;
            }
        }

        private void BindTree(IEnumerable<Lista1> list)
        {
            var nodes = list;

            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.campo, Convert.ToString(node.id_campo));
                if (node.nivel == 0)
                {
                    tv_aspectos.Nodes.Add(newNode);
                    newNode.ShowCheckBox = true;

                    if (node.codConvocatoria != 0)
                    {
                        newNode.Checked = true;
                    }
                }
                else
                {
                    TreeNode nodeAux = tv_aspectos.Nodes[tv_aspectos.Nodes.Count - 1];
                    if (node.nivel == 1)
                    {
                        nodeAux.ChildNodes.Add(newNode);
                    }
                    else
                    {
                        if (node.nivel == 2)
                        {
                            TreeNode nodeUH = nodeAux.ChildNodes[nodeAux.ChildNodes.Count - 1];
                            nodeUH.ChildNodes.Add(newNode);

                            newNode.ShowCheckBox = true;
                            if (node.codConvocatoria != 0)
                            {
                                newNode.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnexpan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnexpan_Click(object sender, EventArgs e)
        {
            if (btnexpan.Text.Equals("Expandir todo"))
            {
                tv_aspectos.ExpandAll();
                btnexpan.Text = "Contraer todo";
            }
            else
            {
                tv_aspectos.CollapseAll();
                btnexpan.Text = "Expandir todo";
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAceptar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Id_EvalConvocatoriaAS = HttpContext.Current.Session["Id_EvalConvocatoria"].ToString();
            foreach (TreeNode nodo in tv_aspectos.Nodes)
            {
                string value = nodo.Value;

                if (nodo.Checked)
                {
                    txtSQL = "SELECT codCampo FROM ConvocatoriaCampo WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo = " + value;

                    //SqlDataReader reader = ejecutaReader(txtSQL, 1);
                    var reader = consultas.ObtenerDataTable(txtSQL, "text");

                    if (reader != null)
                    {
                        if (reader.Rows.Count > 0 && IdVersionProyecto == 2)
                        {
                            txtSQL = "UPDATE ConvocatoriaCampo SET Puntaje=" + GetPuntaje(value) + " WHERE codConvocatoria=" + Id_EvalConvocatoriaAS + " AND codCampo=" + value;
                            ejecutaReader(txtSQL, 2);
                        }
                        else
                        {
                            txtSQL = "INSERT INTO ConvocatoriaCampo (CodConvocatoria, codCampo, Puntaje) VALUES (" + Id_EvalConvocatoriaAS + ", " + value + ", " + GetPuntaje(value) + ")";
                            ejecutaReader(txtSQL, 2);
                        }
                    }
                    else
                    {
                        txtSQL = "INSERT INTO ConvocatoriaCampo (CodConvocatoria, codCampo, Puntaje) VALUES (" + Id_EvalConvocatoriaAS + ", " + value + ", " + GetPuntaje(value) + ")";
                        ejecutaReader(txtSQL, 2);
                    }
                    if (nodo.ChildNodes.Count > 0)
                        foreach (TreeNode item in nodo.ChildNodes)
                        {
                            validarNodo(item);
                        }

                }
                else
                {
                    txtSQL = "DELETE FROM ConvocatoriaCampo WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo = " + value;
                    ejecutaReader(txtSQL, 2);
                    txtSQL = "DELETE FROM ConvocatoriaCampo " +
                    " WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo in (SELECT id_campo FROM Campo WHERE codCampo in (SELECT id_campo FROM Campo WHERE codCampo = " + value + "))";
                    ejecutaReader(txtSQL, 2);
                }
            }
            string OpenerPage = string.Empty;
            OpenerPage = "<script type='text/javascript'> ";
            // OpenerPage += " window.opener.document.getElementById('hidInsumo').value='1';window.close(); ";
            OpenerPage += "opener.location.reload();window.close();";
            OpenerPage += " </script>";
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", OpenerPage);
        }

        string GetPuntaje(string idCampo)
        {
            txtSQL = "SELECT ValorPorDefecto FROM Campo WHERE id_Campo=" + idCampo;
            //SqlDataReader reader = ejecutaReader(txtSQL, 1);
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (IdVersionProyecto == 2)

                return dt.Rows[0][0].ToString();
            else return "0";
        }

        /// <summary>
        /// Validar nodo.
        /// </summary>
        /// <param name="nodo">The nodo.</param>
        public void validarNodo(TreeNode nodo)
        {
            Id_EvalConvocatoriaAS = HttpContext.Current.Session["Id_EvalConvocatoria"].ToString();

            string value = nodo.Value;

            foreach (TreeNode nodo2 in nodo.ChildNodes)
            {
                value = nodo2.Value;
                if (nodo2.Checked)
                {
                    txtSQL = "SELECT codCampo FROM ConvocatoriaCampo WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo = " + value;

                    //SqlDataReader reader = ejecutaReader(txtSQL, 1);
                    var reader = consultas.ObtenerDataTable(txtSQL, "text");

                    if (reader != null)
                    {
                        if (reader.Rows.Count > 0 && IdVersionProyecto == 2)
                        {
                            txtSQL = "UPDATE ConvocatoriaCampo SET Puntaje=" + GetPuntaje(value) + " WHERE codConvocatoria=" + Id_EvalConvocatoriaAS + " AND codCampo=" + value;
                            ejecutaReader(txtSQL, 2);
                        }
                        else
                        {
                            txtSQL = "INSERT INTO ConvocatoriaCampo (CodConvocatoria, codCampo, Puntaje) VALUES (" + Id_EvalConvocatoriaAS + ", " + value + ", " + GetPuntaje(value) + ")";
                            ejecutaReader(txtSQL, 2);
                        }
                    }
                    else
                    {
                        txtSQL = "INSERT INTO ConvocatoriaCampo (CodConvocatoria, codCampo, Puntaje) VALUES (" + Id_EvalConvocatoriaAS + ", " + value + ", " + GetPuntaje(value) + ")";
                        ejecutaReader(txtSQL, 2);
                    }
                    if (nodo2.ChildNodes.Count > 0)
                        validarNodo(nodo2);
                }
                else
                {
                    txtSQL = "DELETE FROM ConvocatoriaCampo WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo = " + value;
                    ejecutaReader(txtSQL, 2);
                    txtSQL = "DELETE FROM ConvocatoriaCampo " +
                    " WHERE codConvocatoria = " + Id_EvalConvocatoriaAS + " AND codCampo in (SELECT id_campo FROM Campo WHERE codCampo in (SELECT id_campo FROM Campo WHERE codCampo = " + value + "))";
                    ejecutaReader(txtSQL, 2);
                }
            }
        }

        /// <summary>
        /// Handles the SelectedNodeChanged event of the tv_aspectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tv_aspectos_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode node = tv_aspectos.SelectedNode;
            // Obtengo el estado del Nodo seleccionado que sera el nodo Padre de esta rutina
            Boolean check = tv_aspectos.SelectedNode.Checked;

            /**********************************************************************/
            //  Valido si esta Check el Nodo para acceder a sus nodos hijos y 
            // chequearlos de acuerdo a la funcionalidad
            // Solo hasta 4 niveles de profundidad
            // Roberto Alvarado 2015/06/02
            /**********************************************************************/
            plnDetallesAspecto.Visible = true;
            pnlVariables.Visible = true;
            pnlAgregar.Visible = false;
            for (int iCnt = 0; iCnt < tv_aspectos.Nodes.Count; iCnt++)
            {
                if ((tv_aspectos.Nodes[iCnt].ChildNodes.Count > 0) && (tv_aspectos.Nodes[iCnt].Checked == check))
                {
                    for (int jCnt = 0; jCnt < tv_aspectos.Nodes[iCnt].ChildNodes.Count; jCnt++)
                    {
                        tv_aspectos.Nodes[iCnt].ChildNodes[jCnt].Checked = check;

                        TreeNodeCollection tnCol = tv_aspectos.Nodes[iCnt].ChildNodes;

                        foreach (TreeNode item in tnCol)
                        {
                            item.Checked = true;
                            if (item.ChildNodes.Count > 0)
                            {
                                for (int i = 0; i < item.ChildNodes.Count; i++)
                                {
                                    item.ChildNodes[i].Checked = check;
                                    if (item.ChildNodes[i].ChildNodes.Count > 0)
                                    {
                                        for (int j = 0; j < item.ChildNodes[i].ChildNodes.Count; j++)
                                        {
                                            item.ChildNodes[i].ChildNodes[j].Checked = check;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            /**********************************************************************/

            string valor = node.Value;
            HttpContext.Current.Session["codCampoAspecto"] = valor;
            seleccionGCCampos(valor);
        }

        private void seleccionGCCampos(string valor)
        {
            HttpContext.Current.Session["codCampoAspecto"] = valor;
            if (!string.IsNullOrEmpty(valor) && !valor.Equals("0"))
            {
                txtSQL = "SELECT codCampo FROM Campo WHERE id_campo = " + valor;
                //SqlDataReader reader = ejecutaReader(txtSQL, 1);
                var reader = consultas.ObtenerDataTable(txtSQL, "text");

                if (reader.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(reader.Rows[0].ItemArray[0].ToString()))
                    {
                        lnkadicionar.Text = "Nueva Variable";
                        lnkadicionar.Visible = true;
                        lnkadicionar.Enabled = true;
                        imgaspectoAgr.Visible = true;
                    }
                    else
                    {
                        txtSQL = "SELECT codCampo FROM Campo WHERE id_campo = " + reader.Rows[0].ItemArray[0].ToString();
                        reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);
                        if (reader.Rows.Count > 0)
                        {
                            if (string.IsNullOrEmpty(reader.Rows[0].ItemArray[0].ToString()))
                            {
                                lnkadicionar.Text = "Nuevo Campo";
                                lnkadicionar.Visible = true;
                                lnkadicionar.Enabled = true;
                                imgaspectoAgr.Visible = true;
                            }
                            else
                            {
                                lnkadicionar.Text = string.Empty;
                                lnkadicionar.Visible = false;
                                lnkadicionar.Enabled = false;
                                imgaspectoAgr.Visible = false;
                            }
                        }
                        else
                        {
                            lnkadicionar.Text = "Nuevo Campo";
                            lnkadicionar.Visible = true;
                            lnkadicionar.Enabled = true;
                            imgaspectoAgr.Visible = true;
                        }
                    }
                }
                else
                {
                    lnkadicionar.Text = "Nueva Variable";
                    lnkadicionar.Visible = true;
                    lnkadicionar.Enabled = true;
                    imgaspectoAgr.Visible = true;
                }
            }

            txtSQL = "SELECT * FROM Campo WHERE id_campo=" + valor;

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                txtdescripcion.Text = dt.Rows[0]["Campo"].ToString();
                if (bool.Parse(dt.Rows[0]["Inactivo"].ToString()))
                {
                    ddlestado.SelectedValue = "1";
                }
                else
                {
                    ddlestado.SelectedValue = "0";
                }
            }

            codPadre = dt.Rows[0]["codCampo"].ToString();
            codCampo = dt.Rows[0]["id_Campo"].ToString();

            del myDelegate = (x) =>
            {
                if (x)
                    return "Inactivo";
                else
                    return "Activo";
            };

            var result = from c in consultas.Db.Campo
                         where c.codCampo == Convert.ToInt32(valor)
                         select new
                         {
                             c.id_Campo,
                             c.Campo1,
                             activo = myDelegate(bool.Parse("" + c.Inactivo))
                         };

            gv_campos.DataSource = result;
            gv_campos.DataBind();
        }

        /// <summary>
        /// Handles the RowCommand event of the gv_campos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gv_campos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ver"))
            {
                string valor = e.CommandArgument.ToString();

                seleccionGCCampos(valor);
            }
            else
            {
                if (e.CommandName.Equals("eliminar"))
                {
                    txtSQL = "DELETE FROM Campo WHERE id_campo = " + e.CommandArgument.ToString();
                    ejecutaReader(txtSQL, 2);

                    tv_aspectos.Nodes.Clear();

                    crearLista();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            codCampo = HttpContext.Current.Session["codCampoAspecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codCampoAspecto"].ToString()) ? HttpContext.Current.Session["codCampoAspecto"].ToString() : "0";
            txtSQL = "SELECT codCampo FROM Campo WHERE Campo = '" + txtdescripcion.Text + "' and id_campo <> " + codCampo;

            var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

            if (reader.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un campo con ese Nombre.')", true);
                //return;
                //if (reader.Read())
                //{

                //}
                //else
                //{
                //    txtSQL = " UPDATE Campo SET Campo = '" + txtdescripcion.Text + "', Inactivo = " + ddlestado.SelectedValue + " WHERE Id_Campo =" + codCampo;
                //    ejecutaReader(txtSQL, 2);
                //}
            }
            else
            {
                txtSQL = " UPDATE Campo SET Campo = '" + txtdescripcion.Text + "', Inactivo = " + ddlestado.SelectedValue + " WHERE Id_Campo =" + codCampo;
                ejecutaReader(txtSQL, 2);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Item actualizado satisfactoriamente!')", true);
            }

            tv_aspectos.Nodes.Clear();

            crearLista();
        }

        /// <summary>
        ///lnkadicionar_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkadicionar_Click(object sender, EventArgs e)
        {
            plnDetallesAspecto.Visible = false;
            pnlVariables.Visible = false;
            pnlAgregar.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the btnAgregarCampo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAgregarCampo_Click(object sender, EventArgs e)
        {
            txtSQL = "SELECT * FROM Campo WHERE Campo = '" + txtnombrecampo.Text + "'";

            //SqlDataReader reader = ejecutaReader(txtSQL, 1);
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un campo con ese Nombre.')", true);
                return;
                //if (reader.Read())
                //{

                //}
                //else
                //{
                //    txtSQL = "INSERT INTO Campo (Campo, codCampo, Inactivo) VALUES ('" + txtnombrecampo.Text + "', " + codCampo + ", " + ddlnuevoactivo.SelectedValue + ")";
                //    ejecutaReader(txtSQL, 2);
                //}
            }
            else
            {
                txtSQL = "INSERT INTO Campo (Campo, codCampo, Inactivo) VALUES ('" + txtnombrecampo.Text + "', " + codCampo + ", " + ddlnuevoactivo.SelectedValue + ")";
                ejecutaReader(txtSQL, 2);
            }

            tv_aspectos.Nodes.Clear();

            crearLista();
        }
    }

    /// <summary>
    /// Lista1
    /// </summary>
    public class Lista1
    {
        /// <summary>
        /// The identifier campo
        /// </summary>
        public int id_campo;
        /// <summary>
        /// The campo
        /// </summary>
        public string campo;
        /// <summary>
        /// The cod convocatoria
        /// </summary>
        public int codConvocatoria;
        /// <summary>
        /// The nivel
        /// </summary>
        public int nivel;
    }
}