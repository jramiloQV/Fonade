using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Fonade.Clases;
using System.Configuration;

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarVisita : Negocio.Base_Page
    {
        string idVisita;
        string tipo;
        string txtSQL;

        DataTable RSVisita;
        DataTable RS;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            idVisita = HttpContext.Current.Session["Id_Visita"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_Visita"].ToString()) ? HttpContext.Current.Session["Id_Visita"].ToString() : "0";
            tipo = HttpContext.Current.Session["Tipo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Tipo"].ToString()) ? HttpContext.Current.Session["Tipo"].ToString() : "0";
            if (!IsPostBack)
            {
                //Cargar DropDownLists.
                CargarDropDownLists();

                if(tipo == "2")
                {
                    btn_agendar.Text = "Actualizar";
                    lblTitulo.InnerText = "Editar visita agendada";
                    btnBorra.Visible = true;
                    CargarDatosVisita();
                }

                //Cargar la ciduad por defecto "como en FONADE clásico".
                CargarCiudad();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Cargar los DropDownLists.
        /// </summary>
        private void CargarDropDownLists()
        {
            var empresas = (from em in consultas.Db.Empresas
                            join p in consultas.Db.Proyecto1s on em.codproyecto equals p.Id_Proyecto
                            join ei in consultas.Db.EmpresaInterventors on em.id_empresa equals ei.CodEmpresa
                            join c in consultas.Db.Ciudad on em.CodCiudad equals c.Id_Ciudad
                            where ei.CodContacto == usuario.IdContacto && ei.Inactivo == false
                            orderby em.razonsocial
                            select new empresa
                            {
                                Nit = em.Nit,
                                RazonSocial = em.razonsocial
                            }).Distinct().OrderBy(x => x.RazonSocial).ToList();

            foreach(var emp in empresas)
            {
                var item = new ListItem(emp.RazonSocial.ToUpper(), emp.Nit);
                //ddlempresa.Items.Add(item);
                DD_Empresas.Items.Add(item);
            }

            //ddlempresa.Items.Insert(0, new ListItem("Seleccione", "0"));
            DD_Empresas.Items.Insert(0, new ListItem("Seleccione", "0"));

            
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Agenadar nueva visita.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_agendar_Click(object sender, EventArgs e)
        {
            
            DateTime? FechaInicio = new DateTime();
            DateTime? FechaFin = new DateTime();

            if(btn_agendar.Text == "Actualizar")
            {
                tipo = "2";
            }

            //Convertir las fecha seleccionadas.
            try
            {
                FechaInicio = Convert.ToDateTime(txtFechaInicio.Text); 
                FechaFin = Convert.ToDateTime(txtFechaFin.Text); 
            }
            catch
            { FechaInicio = null; FechaFin = null; }


            if (tipo == "1")
            {
                //Validaciones
                if (FechaInicio == null || FechaFin == null)
                { return; }

                if (FechaInicio < DateTime.Today)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Imposible Agendar una visita con Fecha Inicial menor que la fecha actual.')", true);
                    return;
                }

                if (FechaFin < FechaInicio)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Fecha de Fin debe ser más reciente que la fecha de Inicio.')", true);
                    return;
                }
                if (TXT_objeto.Text.Trim() == "")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe especificar el Objeto de la Visita!!!.')", true);
                    return;
                }
                if (DD_Empresas.SelectedValue == "" || DD_Empresas.SelectedValue == "0")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Por favor, Seleccione una Empresa.')", true);
                    return;
                }

                // Insercion y envio correo
                var empresa = (from em in consultas.Db.Empresas
                               where em.Nit == DD_Empresas.SelectedValue
                               select em).FirstOrDefault();
                if (empresa != null)
                {
                    var visita = new Datos.Visita
                    {
                        Id_Interventor = usuario.IdContacto,
                        Id_Empresa = empresa.id_empresa,
                        FechaInicio = (DateTime)FechaInicio,
                        FechaFin = (DateTime)FechaFin,
                        Estado = "Pendiente",
                        Objeto = TXT_objeto.Text
                    };
                    consultas.Db.Visita.InsertOnSubmit(visita);
                    consultas.Db.SubmitChanges();

                    string txtFrom = "Fonade";
                    string txtSubject = "Se ha programado una visita!!!";
                    string txtMessage = "<br/><br/> Ha sido Programada una visita a <b>" + DD_Empresas.SelectedItem.Text + "</b>" +
                                        "<br/><br><br>Fecha de Inicio: " + FechaInicio +
                                        "<br/><br>Fecha de Finalización: " + FechaFin +
                                        "<br/><br>Objeto de la visita: " + TXT_objeto.Text.Trim() + " <br/>" +
                                        "<br/><br><br>Gracias por su atención.";

                    //Enviar Mail
                    EnviarMail(txtFrom, txtSubject, txtMessage);

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Nueva visita agendada correctamente.');refreshwindow();", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar la nueva visita.')", true);
                    return;
                }

            }

            if (tipo == "2") // Update
            {
                var visita = (from v in consultas.Db.Visita
                              where v.Id_Visita == int.Parse(idVisita)
                              select v).FirstOrDefault();

                if (visita != null)
                {
                    visita.FechaInicio = (DateTime)FechaInicio;
                    visita.FechaFin = (DateTime)FechaFin;
                    visita.Objeto = TXT_objeto.Text;
                    consultas.Db.SubmitChanges();

                    var datoEmpresa = (from emp in consultas.Db.Empresas
                                       where emp.id_empresa == visita.Id_Empresa
                                       select emp).FirstOrDefault();

                    if (datoEmpresa != null)
                    {
                        //Cuerpo del mensaje.
                        string txtFrom = "Fonade";
                        string txtSubject = "Se ha modificado una visita!!!";
                        string txtMessage = "<br/><br/>La visita programada a <b>" + datoEmpresa.razonsocial.ToUpper() + "</b> ha sido modificada de la siguiente manera: " +
                                            "<br/><br><br>Fecha de Inicio: " + txtFechaInicio.Text +
                                            "<br/><br>Fecha de Finalización: " + txtFechaFin.Text +
                                            "<br/><br>Objeto de la visita: " + TXT_objeto.Text.Trim() + "<br/>" +
                                            "<br/><br><br>Gracias por su atención.";

                        //Enviar mensaje.
                        EnviarMail(txtFrom, txtSubject, txtMessage);

                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Visita agendada modificada correctamente.');refreshwindow();", true);
                    }
                }

            }

        }

        /// <summary>
        /// SeelctedIndexChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DD_Empresas_SelectedIndexChanged(object sender, EventArgs e)
        { CargarCiudad(); }

        /// <summary>
        /// Establecer el texto del TextBox "TXT_ciudad" de acuerdo a la selección del 
        /// DropDownList "DD_Empresas".
        /// </summary>
        private void CargarCiudad()
        {
            var ciudad = new Datos.Ciudad();
            if(DD_Empresas.SelectedIndex != 0)
            {
                ciudad = (from c in consultas.Db.Ciudad
                              join em in consultas.Db.Empresas on c.Id_Ciudad equals em.CodCiudad
                              where em.Nit == DD_Empresas.SelectedValue
                              select c).FirstOrDefault();

                TXT_ciudad.Text = ciudad.NomCiudad.ToUpper();
                TXT_nit.Text = DD_Empresas.SelectedValue;
            }
        }

        private void CargarDatosVisita()
        {
            var datosVisita = (from v in consultas.Db.Visita
                               where v.Id_Visita == int.Parse(idVisita)
                               select v).FirstOrDefault();
            var empresa = (from emp in consultas.Db.Empresas
                           where emp.id_empresa == datosVisita.Id_Empresa
                           select emp).FirstOrDefault();
            var ciudad = (from c in consultas.Db.Ciudad
                          where c.Id_Ciudad == empresa.CodCiudad
                          select c).FirstOrDefault();

            DD_Empresas.SelectedValue = empresa.Nit;
            DD_Empresas.Enabled = false;
            TXT_nit.Text = empresa.Nit;
            TXT_nit.ReadOnly = true;
            TXT_ciudad.Text = ciudad.NomCiudad.ToUpper();
            TXT_ciudad.ReadOnly = true;
            TXT_objeto.Text = datosVisita.Objeto;

            var parametros = "'" + datosVisita.FechaInicio.Year.ToString() + "-" + datosVisita.FechaInicio.Month.ToString().PadLeft(2, '0') + "-" + datosVisita.FechaInicio.Day.ToString().PadLeft(2, '0') + ";";
            parametros += datosVisita.FechaFin.Year.ToString() + "-" + datosVisita.FechaFin.Month.ToString().PadLeft(2, '0') + "-" + datosVisita.FechaFin.Day.ToString().PadLeft(2, '0') + "'";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SetDate(" + parametros + ")", true);
        }

        /// <summary>
        /// Colocar en mayúscula la primera letra del parámetro.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Enviar Email "método de FONADE clásico".
        /// </summary>
        /// <param name="txtFrom">De:</param>
        /// <param name="txtSubject">Para:</param>
        /// <param name="txtMessage">Cuerpo del mensaje.</param>
        private void EnviarMail(string txtFrom, string txtSubject, string txtMessage)
        {
            //Inicializar variables.
            DataTable RSContacto = new DataTable();
            DataTable RS = new DataTable();

            try
            {
                //Consulta.
                txtSQL = "Select nombres, apellidos, identificacion from contacto where id_contacto = " + usuario.IdContacto;

                //Asignar resultados de la consulta a variable DataTable
                RSContacto = consultas.ObtenerDataTable(txtSQL, "text");

                //Información de los emprendedores de la Empresa.
                txtSQL = " SELECT Empresa.razonsocial, Empresa.codproyecto, ProyectoContacto.CodContacto " +
                         " FROM Empresa INNER JOIN ProyectoContacto ON Empresa.codproyecto = ProyectoContacto.CodProyecto " +
                         " WHERE (ProyectoContacto.CodRol = " + Datos.Constantes.CONST_RolEmprendedor + ") " +
                         " AND (ProyectoContacto.Inactivo = 0) " +
                         " AND (Empresa.Nit = '" + DD_Empresas.SelectedValue + "')";

                //Asignar resultados de la consulta anterior a variable DataTable.
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Recorrer filas del DataTable anterior para generar tareas pendientes.
                for (int i = 0; i < RS.Rows.Count; i++)
                {
                    string Mensaje = "<br/><br/><br/>" + RSContacto.Rows[0]["nombres"].ToString() + " " + RSContacto.Rows[0]["apellidos"].ToString() +
                                     "<br/>C.C. " + RSContacto.Rows[0]["identificacion"].ToString();

                    //Instancia de la clase "Tarea".
                    AgendarTarea tarea = new AgendarTarea(Int32.Parse(RSContacto.Rows[0]["identificacion"].ToString()), "Visita de Interventoría",
                         Mensaje, RS.Rows[i]["CodProyecto"].ToString(), Datos.Constantes.CONST_Generica, "0", true, 1, true, false,
                         usuario.IdContacto, "", "", "Adicionar Visita");
                    //Agendar tarea.
                    tarea.Agendar();
                }
            }
            catch { }
        }

        protected void btnBorra_Click(object sender, EventArgs e)
        {
            var visita = (from v in consultas.Db.Visita
                          where v.Id_Visita == int.Parse(idVisita)
                          select v).FirstOrDefault();
            consultas.Db.Visita.DeleteOnSubmit(visita);
            consultas.Db.SubmitChanges();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Visita eliminada correctamente.');refreshwindow();", true);
        }
    }
}

public class empresa
{
    public string Nit { get; set; }
    public string RazonSocial { get; set; }
}