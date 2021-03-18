using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Datos.Modelos;

namespace Fonade.FONADE.interventoria
{
    public partial class InterIndicadoresGenericos : Negocio.Base_Page
    {
        string CodProyecto;
        string CodEmpresa;
        int contador = 1;

        string txtSQL;

        string _cadenaConex = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        delegate string del(string x, string y);

        public bool mostrarEditar
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    return true;
                else
                    return false;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            var codEmp = HttpContext.Current.Session["CodEmpresa"] ?? consultas.ObtenerDataTable(string.Format(
                "SELECT [id_empresa] FROM [dbo].[Empresa] WHERE [codproyecto] = {0}", CodProyecto), "text").Rows[0][0];
            CodEmpresa = codEmp.ToString();
            // se crea control para ocultar postit y boton guardar para perfil gerente interventor 11/6/2014 Jonathan Aguirre
            if (usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
            { btnGuardar.Visible = false; Post_It1.Visible = false; }

            if (!IsPostBack)
            {
                //Mauricio Arias Olave. "08/05/2014": El botón "Guardar" solo puede ser visible para los interventores.
                if (usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor)
                { btnGuardar.Visible = false; Post_It1.Visible = false; }

                int cant = cargarGrid();

                if (cant > 0)
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        btnGuardar.Visible = true;
                        btnGuardar.Enabled = true;
                    }
                    else
                    {
                        btnGuardar.Visible = false;
                        btnGuardar.Enabled = false;
                    }
                }
                else
                {
                    btnGuardar.Visible = false;
                    btnGuardar.Enabled = false;
                }
            }

            //int txtTab = Constantes.const_in;
            //var tabla = opRoleNeg.UltimaModificacion(CodProyecto, txtTab);
            //if (tabla.Count > 0)
            //{
            //    btnGuardar.Visible = Convert.ToBoolean(tabla[2]);
            //    btnGuardar.Enabled = Convert.ToBoolean(tabla[2]);
            //}

            RestringirLetras(0);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in gvindicadoresgenericos.Rows)
            {
                TextBox txtNumerador = (TextBox)fila.FindControl("txtNumerador");
                TextBox txtDenominador = (TextBox)fila.FindControl("txtDenominador");
                TextBox txtObservacion = (TextBox)fila.FindControl("txtObservacion");
                var indice = txtNumerador.CssClass.ToString().Split('_');
                var evaluacion = string.Empty;

                float valNumerador = float.Parse(txtNumerador.Text);
                float valDenominador = float.Parse(txtDenominador.Text);
                float division;
                if (valDenominador > 0)
                {
                    division = (valNumerador / valDenominador);
                }
                else
                {
                    division = 0;
                }

                switch (indice[1])
                {
                    // Empleo
                    case "16":
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division > 1)
                            {
                                evaluacion = "Más que Efectivo";
                            }
                            else
                            {
                                if (division >= 0.66 && division <= 1)
                                {
                                    evaluacion = "Efectivo";
                                }
                                else
                                {
                                    evaluacion = "Inefectivo";
                                }
                            }
                        }
                        break;

                    //Presupuesto
                    case "17":
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division >= 0.7)
                            {
                                evaluacion = "Efectivo";
                            }
                            else
                            {
                                evaluacion = "Inefectivo";
                            }
                        }
                        break;

                    //Mercadeo
                    case ("18"):
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division >= 1)
                            {
                                evaluacion = "Eficiente";
                            }
                            else
                            {
                                evaluacion = "Ineficiente";
                            }
                        }
                        break;

                    //Ventas
                    case ("19"):
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division > 1)
                            {
                                evaluacion = "Meta altamente eficiente";
                            }
                            else
                            {
                                if (division >= 0.55 && division <= 1)
                                {
                                    evaluacion = "Meta eficiente";
                                }
                                else
                                {
                                    evaluacion = "Meta Deficiente";
                                }
                            }
                        }
                        break;

                    //Produccion
                    case ("20"):
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division > 1)
                            {
                                evaluacion = "Más que efectivo";
                            }
                            else
                            {
                                if (division >= 0.6 && division <= 1)
                                {
                                    evaluacion = "Efectivo";
                                }
                                else
                                {
                                    evaluacion = "Inefectivo";
                                }
                            }
                        }
                        break;

                    //Comercial
                    case ("21"):
                        if (valDenominador == 0 && valNumerador == 0)
                        {
                            evaluacion = "Sin evaluación";
                        }
                        else
                        {
                            if (division > 1)
                            {
                                evaluacion = "Más que eficiente";
                            }
                            else
                            {
                                if (division >= 0.7 && division <= 1)
                                {
                                    evaluacion = "Eficiente";
                                }
                                else
                                {
                                    evaluacion = "Ineficiente";
                                }
                            }
                        }
                        break;

                }


                if (string.IsNullOrEmpty(txtNumerador.Text)) txtNumerador.Text = "0";
                if (string.IsNullOrEmpty(txtDenominador.Text)) txtDenominador.Text = "0";

                Int32 index = Convert.ToInt32(gvindicadoresgenericos.DataKeys[fila.RowIndex].Value.ToString());

                txtSQL = "Update IndicadorGenerico Set Numerador=" + txtNumerador.Text + ", Evaluacion='" + evaluacion +
                        "', Denominador=" + txtDenominador.Text + ", Observacion = '" + txtObservacion.Text + "' Where Id_IndicadorGenerico=" + index;

                ejecutaReader(txtSQL, 2);
            }
            Response.Redirect("InterIndicadoresGenericos.aspx");
        }

        private void RestringirLetras(int serie)
        {
            try
            {
                GridViewRow fila = gvindicadoresgenericos.Rows[serie];
                TextBox textbox = (TextBox)fila.FindControl("txtNumerador");
                textbox.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                textbox = (TextBox)fila.FindControl("txtDenominador");
                textbox.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                RestringirLetras(serie + 1);
            }
            catch (Exception) { }
        }

        protected void gvindicadoresgenericos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var filas = gvindicadoresgenericos.Rows;

            foreach (GridViewRow fila in filas)
            {
                var txtNum = (TextBox)fila.FindControl("txtNumerador");
                //txtNum.ID = txtNum.ID + "_" + contador;
                txtNum.CssClass = txtNum.ID + "_" + contador;

                var txtDen = (TextBox)fila.FindControl("txtDenominador");
                //txtDen.ID = txtDen.ID + "_" + contador;
                txtDen.CssClass = txtDen.ID + "_" + contador;

                var lblEal = (Label)fila.FindControl("lblEvaluacion");
                lblEal.CssClass = lblEal.ID + "_" + contador;

                contador = contador + 1;
            }
        }


        protected void gvindicadoresgenericos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ((DataControlField)gvindicadoresgenericos.Columns
             .Cast<DataControlField>()
             .Where(fld => fld.HeaderText == "Modificar")
             .SingleOrDefault()).Visible = mostrarEditar;
        }

        protected void gvindicadoresgenericos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Modificar")
            {
                if (e.CommandArgument != null)
                {
                    //TextBox txtNumerador = (TextBox)FindControl("txtNumerador");
                    //TextBox txtNumerador = (TextBox)gvindicadoresgenericos.Rows[4].FindControl("txtNumerador");
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    TextBox txtNumerador = (TextBox)row.FindControl("txtNumerador");

                    string indice = txtNumerador.CssClass.ToString().Split('_')[1];

                    string id = e.CommandArgument.ToString();
                    lblIdIndicador.Text = id;
                    lblIndice.Text = indice;
                    cargarDatosEnModal(Convert.ToInt32(id));
                    ModalEliminarArchivo.Show();
                }
            }
        }

        private void cargarDatosEnModal(int _idIndicador)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {
                var indicador = (from arc in db.IndicadorGenericos
                                 where arc.Id_IndicadorGenerico == _idIndicador
                                 select arc).FirstOrDefault();

                if (indicador != null)
                {
                    lblNombreIndicador.Text = indicador.NombreIndicador;
                    lblIndicadorNumDescripcion.Text = indicador.Descripcion.Split('/')[0];
                    lblIndicadorDenDescripcion.Text = indicador.Descripcion.Split('/')[1];
                    txtIndicadorNumerador.Text = indicador.Numerador.ToString();
                    txtIndicadorDenominador.Text = indicador.Denominador.ToString();
                    lblEvaluacionIndicador.Text = indicador.Evaluacion;
                    txtIndicadorObservacion.Text = indicador.Observacion;
                }

            }
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        private int cargarGrid()
        {
            del myDelegate = (x, y) =>
            {
                if (string.IsNullOrEmpty(x))
                    return "";
                else
                    return x.Split('/')[Convert.ToInt32(y)];
            };

            var result = (from ig in consultas.Db.IndicadorGenericos
                          where ig.CodEmpresa == Convert.ToInt32(CodEmpresa)
                          orderby ig.Id_IndicadorGenerico
                          select new
                          {
                              ig.Id_IndicadorGenerico,
                              ig.NombreIndicador,
                              NueradorDescripcion = myDelegate(ig.Descripcion, "0"),
                              DenominadorDescripcion = myDelegate(ig.Descripcion, "1"),
                              ig.Numerador,
                              ig.Denominador,
                              ig.Evaluacion,
                              ig.Observacion
                          }).Take(6);

            gvindicadoresgenericos.DataSource = result;
            gvindicadoresgenericos.DataBind();

            return result.Count();
        }


        private bool ModificarIndicador(int _idIndicador, float _valNumerador, float _valDenominador
                                            , string _obsersavcion, string _motivo)
        {
            bool modificado = false;

            float division;

            var evaluacion = string.Empty;

            if (_valDenominador > 0)
            {
                division = (_valNumerador / _valDenominador);
            }
            else
            {
                division = 0;
            }

            switch (lblIndice.Text)
            {
                // Empleo
                case "16":
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division > 1)
                        {
                            evaluacion = "Más que Efectivo";
                        }
                        else
                        {
                            if (division >= 0.66 && division <= 1)
                            {
                                evaluacion = "Efectivo";
                            }
                            else
                            {
                                evaluacion = "Inefectivo";
                            }
                        }
                    }
                    break;

                //Presupuesto
                case "17":
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division >= 0.7)
                        {
                            evaluacion = "Efectivo";
                        }
                        else
                        {
                            evaluacion = "Inefectivo";
                        }
                    }
                    break;

                //Mercadeo
                case ("18"):
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division >= 1)
                        {
                            evaluacion = "Eficiente";
                        }
                        else
                        {
                            evaluacion = "Ineficiente";
                        }
                    }
                    break;

                //Ventas
                case ("19"):
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division > 1)
                        {
                            evaluacion = "Meta altamente eficiente";
                        }
                        else
                        {
                            if (division >= 0.55 && division <= 1)
                            {
                                evaluacion = "Meta eficiente";
                            }
                            else
                            {
                                evaluacion = "Meta Deficiente";
                            }
                        }
                    }
                    break;

                //Produccion
                case ("20"):
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division > 1)
                        {
                            evaluacion = "Más que efectivo";
                        }
                        else
                        {
                            if (division >= 0.6 && division <= 1)
                            {
                                evaluacion = "Efectivo";
                            }
                            else
                            {
                                evaluacion = "Inefectivo";
                            }
                        }
                    }
                    break;

                //Comercial
                case ("21"):
                    if (_valDenominador == 0 && _valNumerador == 0)
                    {
                        evaluacion = "Sin evaluación";
                    }
                    else
                    {
                        if (division > 1)
                        {
                            evaluacion = "Más que eficiente";
                        }
                        else
                        {
                            if (division >= 0.7 && division <= 1)
                            {
                                evaluacion = "Eficiente";
                            }
                            else
                            {
                                evaluacion = "Ineficiente";
                            }
                        }
                    }
                    break;

            }

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {

                var query = (from ca in db.IndicadorGenericos
                             where ca.Id_IndicadorGenerico == _idIndicador
                             select ca).FirstOrDefault();
                //Agregamos al historico
                HistoricoIndicadorGenericoModel historicoIndicadorDTO = new HistoricoIndicadorGenericoModel();

                historicoIndicadorDTO.CodContactoCambio = usuario.IdContacto;
                historicoIndicadorDTO.CodEmpresa = query.CodEmpresa;
                historicoIndicadorDTO.Denominador_New = Convert.ToInt32(_valDenominador);
                historicoIndicadorDTO.Denominador_Old = query.Denominador ?? 0;
                historicoIndicadorDTO.Descripcion = query.Descripcion;
                historicoIndicadorDTO.Evaluacion_New = evaluacion;
                historicoIndicadorDTO.Evaluacion_Old = query.Evaluacion;
                historicoIndicadorDTO.FechaCambio = DateTime.Now;
                historicoIndicadorDTO.id_IndicadorGenerico = _idIndicador;
                historicoIndicadorDTO.MotivoCambio = _motivo;
                historicoIndicadorDTO.NombreIndicador = query.NombreIndicador;
                historicoIndicadorDTO.Numerador_New = Convert.ToInt32(_valNumerador);
                historicoIndicadorDTO.Numerador_Old = query.Numerador ?? 0;
                historicoIndicadorDTO.Observacion_New = _obsersavcion;
                historicoIndicadorDTO.Observacion_Old = query.Observacion;

                if (ingresarHistoricoIndicador(historicoIndicadorDTO))
                {
                    //Realizamos el update
                    query.Numerador = Convert.ToInt32(_valNumerador);
                    query.Denominador = Convert.ToInt32(_valDenominador);
                    query.Observacion = _obsersavcion;
                    query.Evaluacion = evaluacion;

                    db.SubmitChanges();

                    cargarGrid();

                    modificado = true;
                }
            }
            return modificado;
        }

        private bool ingresarHistoricoIndicador(HistoricoIndicadorGenericoModel _historicoIndicadorDTO)
        {
            bool ingresado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadenaConex))
            {
                HistoricoIndicadorGenerico historico = new HistoricoIndicadorGenerico {
                    CodContactoCambio = _historicoIndicadorDTO.CodContactoCambio,
                    CodEmpresa = _historicoIndicadorDTO.CodEmpresa,
                    Denominador_New = _historicoIndicadorDTO.Denominador_New,
                    Denominador_Old = _historicoIndicadorDTO.Denominador_Old,
                    Descripcion = _historicoIndicadorDTO.Descripcion,
                    Evaluacion_New = _historicoIndicadorDTO.Evaluacion_New,
                    Evaluacion_Old = _historicoIndicadorDTO.Evaluacion_Old,
                    FechaCambio = _historicoIndicadorDTO.FechaCambio,
                    id_IndicadorGenerico = _historicoIndicadorDTO.id_IndicadorGenerico,
                    MotivoCambio = _historicoIndicadorDTO.MotivoCambio,
                    NombreIndicador = _historicoIndicadorDTO.NombreIndicador,
                    Numerador_New = _historicoIndicadorDTO.Numerador_New,
                    Numerador_Old = _historicoIndicadorDTO.Numerador_Old,
                    Observacion_New = _historicoIndicadorDTO.Observacion_New,
                    Observacion_Old = _historicoIndicadorDTO.Observacion_Old
                };

                db.HistoricoIndicadorGenerico.InsertOnSubmit(historico);
                db.SubmitChanges();
                ingresado = true;
            }

            return ingresado;
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblIdIndicador.Text);
            string motivo = txtMotivoCambio.Text;
            float numerador = Convert.ToInt32(txtIndicadorNumerador.Text);
            float denominador = Convert.ToInt32(txtIndicadorDenominador.Text);
            string observacion = txtIndicadorObservacion.Text;

            if (motivo != "")
            {

                if (ModificarIndicador(id, numerador, denominador, observacion, motivo))
                {
                    txtMotivoCambio.Text = "";
                    Alert("Se actualizó el indicador correctamente.");                    
                }
                else
                {
                    txtMotivoCambio.Text = motivo;
                    Alert("No se logró actualizar el indicador.");
                }
            }
            else
            {
                Alert("El campo motivo es obligatorio.");
            }
        }
    }
}