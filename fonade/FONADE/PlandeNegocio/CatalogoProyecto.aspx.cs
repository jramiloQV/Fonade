using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using MoreLinq;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;
using System.Data;
using Fonade.Clases;

namespace Fonade.FONADE.PlandeNegocio
{    
	public partial class CatalogoProyecto : Negocio.Base_Page
	{
		/*Actualizacion 17-01-015*/
		int cont = 0;
		bool invalid = false;
		string crearAdmin = "Crear Administrador";
		string crearGerenteEvaluador = "Crear GerenteEvaluador";
		string crearCallCenter = "Crear Call Center";
		string crearGerenteInterventor = "Crear Gerente Interventor";
		string crearPerfilFiduciario = "Crear Perfil Fiduciario";
		public const int PAGE_SIZE = 10;
		DateTime fecha_Validar;
		String CodProgramaRealizado;

		String grupos1;
		int idusuario;//id del usuario seleccionado en la grilla
		string[] codgrupousuario;
		//void_HideControls();
		public class Instituciom
		{
			public int Id_InstitucionEducativa { get; set; }
			public string NomInstitucionEducativa { get; set; }
			public string NomMunicipio { get; set; }
		}

		protected void CamposLectura_Escritura(Control Padre, bool estado)
		{
			if (estado)
			{ tbl_Convenio.CssClass = "Lectura"; }
			else
			{
				tbl_Convenio.CssClass = "Escritura";
			}
			foreach (Control c in Padre.Controls)
			{
				if (c.GetType() == typeof(DropDownList)) ((DropDownList)(c)).Enabled = !estado;
				if (c.GetType() == typeof(TextBox)) ((TextBox)(c)).ReadOnly = !estado;
				if (c.GetType() == typeof(Button)) ((Button)(c)).Enabled = estado;
				CamposLectura_Escritura(c, estado);
			}
		}

		void void_ModificarDatos(int Grupocontacto)
		{
			if (Request.QueryString["CodProyecto"] != null)
			{
				int codigoContacto = Int32.Parse(Request.QueryString["CodProyecto"]);
				var query = (from c in consultas.Db.Proyecto1s
							 where c.Id_Proyecto == codigoContacto
							 select c);

				var queryEeval = (from p in consultas.Db.Proyecto1s
								  from c in consultas.Db.Ciudad
								  from ss in consultas.Db.SubSector
								  from s in consultas.Db.Sector
								  from d in consultas.Db.departamento
								  where c.Id_Ciudad == p.CodCiudad &
								  ss.Id_SubSector == p.CodSubSector &
								  d.Id_Departamento == c.CodDepartamento & s.Id_Sector == ss.CodSector &
								  p.Id_Proyecto == codigoContacto

								  select new
								  {
									  NomProyecto = p.NomProyecto,
									  Sumario = p.Sumario,
									  departamento = d.NomDepartamento,
									  ciudad = c.NomCiudad,
									  sector = s.NomSector,
									  subsector = ss.NomSubSector,
									  idciudad = c.Id_Ciudad,
									  iddepto = d.Id_Departamento,
									  idsector = s.Id_Sector,
									  idsubsector = ss.Id_SubSector
								  }).FirstOrDefault();


				/*
				tb_Descripcion.Text = 
				ddl_depto1.SelectedValue = queryEeval.iddepto.ToString();
				ddl_depto2.SelectedValue = queryEeval.idsector.ToString();
				ddl_ciudad1.SelectedValue = queryEeval.idciudad.ToString();
				ddl_ciudad2.SelectedValue = queryEeval.idsubsector.ToString();*/

				foreach (Proyecto1 p in query)
				{
					if (!String.IsNullOrEmpty(tb_Nombre.Text))
					{
						p.NomProyecto = tb_Nombre.Text;
					}
					else
					{
						p.NomProyecto = queryEeval.NomProyecto;
					}
					if (!String.IsNullOrEmpty(tb_Descripcion.Text))
					{
						p.Sumario = tb_Descripcion.Text;
					}
					else
					{
						p.Sumario = queryEeval.Sumario;
					}
					p.FechaModificacion = DateTime.Today;
					try
					{
						p.CodCiudad = Int32.Parse(ddl_ciudad1.SelectedValue);
					}
					catch (FormatException)
					{
						p.CodCiudad = Int32.Parse(queryEeval.idciudad.ToString());
					}

					try
					{
						p.CodSubSector = Int32.Parse(ddl_ciudad2.SelectedValue);
					}
					catch (FormatException)
					{
						p.CodSubSector = Int32.Parse(queryEeval.idsubsector.ToString());
					}
					p.Latitud = 1;
					p.Longitud = 1;
				}
				// Submit the changes to the database. 
				try
				{
					consultas.Db.SubmitChanges();
					void_show("La información del usuario ha sido actualizado correctamente", true);
				}
				catch
				{ }

			}
		}
		protected int int_CrearDatos(int codusuario)
		{
			int devult = -1;
			SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
			SqlCommand cmd;

			if (String.IsNullOrEmpty(tb_Nombre.Text))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El nombre es requerido')", true);
				return -1;
			}


			var proyectomodelo = (from pgm in consultas.Db.ProyectoGastosModelos
								  select new Datos.ProyectoGastosModelo
								  {
									  Tipo = pgm.Tipo,
									  Descripcion = pgm.Descripcion
								  }
							   ).ToList();
			Proyecto1 Proyecto = new Proyecto1()
		   {
			   NomProyecto = tb_Nombre.Text,
			   FechaCreacion = DateTime.Today,
			   CodTipoProyecto = 1,
			   CodEstado = Constantes.CONST_Inscripcion,
			   CodCiudad = Int32.Parse(ddl_ciudad1.SelectedValue),
			   CodSubSector = Int32.Parse(ddl_ciudad2.SelectedValue),
			   CodContacto = usuario.IdContacto,
			   CodInstitucion = usuario.CodInstitucion
			   ,
			   Sumario = tb_Descripcion.Text
		   };
			try
			{
				String sql = @"Insert into proyecto (nomproyecto,sumario,fechacreacion,codtipoproyecto,codestado,codciudad,codsubsector,codcontacto,codinstitucion)
					 values('" + Proyecto.NomProyecto + "','" + Proyecto.Sumario + "',getdate(),1," + Constantes.CONST_Inscripcion + "," + Proyecto.CodCiudad + "," + Proyecto.CodSubSector + "," + usuario.IdContacto + "," + usuario.CodInstitucion + ")";


				cmd = new SqlCommand(sql, conn);
				try
				{

					conn.Open();
					cmd.ExecuteReader();
					//conn.Close();
				}
				catch (SqlException)
				{ return -1; }
				finally
				{
					conn.Close();
					conn.Dispose();
				}

				var maxidProyecto = (from p in consultas.Db.Proyecto
									 select new
									 {
										 id_Proyecto = p.Id_Proyecto
									 }).OrderByDescending(u => u.id_Proyecto).FirstOrDefault();

				//consultas.Db.Proyecto1s.InsertOnSubmit(Proyecto);


				//consultas.Db.SubmitChanges();

				devult = maxidProyecto.id_Proyecto;

				foreach (ProyectoGastosModelo pgms in proyectomodelo)
				{
					ProyectoGasto proyectogasto = new ProyectoGasto()
					{
						CodProyecto = maxidProyecto.id_Proyecto,
						Descripcion = pgms.Descripcion,
						Valor = 0,
						Tipo = pgms.Tipo,
						Protegido = true,
					};

					sql = @"Insert into proyectogastos (codproyecto,Descripcion,Valor,Tipo,Protegido)
					values('" + proyectogasto.CodProyecto + "','" + proyectogasto.Descripcion + "','" + proyectogasto.Valor + "','" + proyectogasto.Tipo + "','" + proyectogasto.Protegido + "')";
					consultas.Db.ProyectoGastos.InsertOnSubmit(proyectogasto);

					cmd = new SqlCommand(sql, conn);
					try
					{

						conn.Open();
						cmd.ExecuteReader();
						//conn.Close();
					}
					catch (SqlException)
					{ return -1; }
					finally
					{
						conn.Close();
						conn.Dispose();
					}
				};

				//var maxidProyectEreo = (from p in consultas.Db.Proyectos
				//                     select new
				//                     {
				//                         id_Proyecto = p.Id_Proyecto
				//                     }).OrderByDescending(u => u.id_Proyecto).FirstOrDefault();

				//Request.QueryString["CodProyecto"] = "" + maxidProyectEreo;

				
				void_show("el Proyecto " + tb_Nombre.Text + " ha sido agregado", true);
				return maxidProyecto.id_Proyecto;
			}
			catch
			{ return devult; }

		}
		protected void void_show(string texto, bool mostrar)
		{
			lbl_popup.Visible = mostrar;
			lbl_popup.Text = texto;
			mpe1.Enabled = mostrar;
			mpe1.Show();
		}
		/// <summary>
		/// Esconder controles...
		/// </summary>
		/// <param name="accion"></param>
		void void_HideControls(string accion)
		{

			switch (accion)
			{
				case "Crear":
					pnl_Proyectos.Visible = false;
					gv_emprendedor.Visible = false;
					pnl_Emprendedor.Visible = false;
					tc_Emprendedor.Visible = false;
					btn_crearActualizar.Visible = true;
					btn_crearActualizar.Text = "Crear proyecto";
					pnl_crearEditar.Visible = true;
					pnl_infoAcademica.Visible = false;

					break;
				case "Editar":
					pnl_Proyectos.Visible = false;
					btn_crearActualizar.Text = "Actualizar proyecto";
					gv_emprendedor.Visible = true;
					pnl_Emprendedor.Visible = true;
					tc_Emprendedor.Visible = true;
					pnl_crearEditar.Visible = true;
					pnl_infoAcademica.Visible = true;
					pnl_infoAcademica.Visible = false;
					//btn_crearActualizarPrograma.Visible = false;
					Panel2.Visible = false;
					break;
				default:
					gv_emprendedor.Visible = false;
					pnl_Emprendedor.Visible = false;
					tc_Emprendedor.Visible = false;
					pnl_crearEditar.Visible = false;
					pnl_infoAcademica.Visible = false;
					break;
			}

		}
		void void_traerDatos(string[] codgrupo, int codusuario)
		{

			if (Request.QueryString["CodProyecto"] != null)
			{
				int codigoContacto = Int32.Parse(Request.QueryString["CodProyecto"]);
				var query = (from p in consultas.Db.Proyecto1s
							 from c in consultas.Db.Ciudad
							 from ss in consultas.Db.SubSector
							 from s in consultas.Db.Sector
							 from d in consultas.Db.departamento
							 where c.Id_Ciudad == p.CodCiudad &
							 ss.Id_SubSector == p.CodSubSector &
							 d.Id_Departamento == c.CodDepartamento & s.Id_Sector == ss.CodSector &
							 p.Id_Proyecto == codigoContacto

							 select new
							 {
								 NomProyecto = p.NomProyecto,
								 Sumario = p.Sumario,
								 departamento = d.NomDepartamento,
								 ciudad = c.NomCiudad,
								 sector = s.NomSector,
								 subsector = ss.NomSubSector,
								 idciudad = c.Id_Ciudad,
								 iddepto = d.Id_Departamento,
								 idsector = s.Id_Sector,
								 idsubsector = ss.Id_SubSector
							 }).FirstOrDefault();

				tb_Nombre.Text = query.NomProyecto;
				tb_Descripcion.Text = query.Sumario;
				ddl_depto1.SelectedValue = query.iddepto.ToString();
				ddl_depto2.SelectedValue = query.idsector.ToString();

				ddl_depto1_Selected();
				ddl_depto2_Selected();

				ddl_ciudad1.SelectedValue = query.idciudad.ToString();
				ddl_ciudad2.SelectedValue = query.idsubsector.ToString();
				//select nomproyecto, sumario, coddepartamento, codciudad, nomdepartamento, nomciudad, "&_
				//            "codsector, codsubsector, nomsector, nomsubsector, Latitud, Longitud, codestado "&_
				//            "from proyecto, ciudad, subsector, sector, departamento "&_
				//            "where id_ciudad=codciudad and id_subsector=codsubsector "&_
				//            "and id_departamento=coddepartamento and id_sector=codsector and id_proyecto="&CodProyecto

			}


		}
		void void_traerDatosRol(string[] codgrupo, int codusuario)
		{
			//SELECT nombres,apellidos,codtipoidentificacion,identificacion, cargo, email, telefono, fax, codgrupo "&_
			//        "FROM Contacto, GrupoContacto WHERE ID_Contacto=codcontacto and Id_Contacto=" & CodContacto
			//var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions

			//                          select new
			//                          {
			//                              Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
			//                              NomTipoIdentificacion = ti.NomTipoIdentificacion,

			//                          });
			//var Roles = (from g in consultas.Db.Grupos
			//             where codgrupo.Contains(g.Id_Grupo.ToString())
			//             select new
			//             {
			//                 Rol = g.NomGrupo,
			//                 codRol = g.Id_Grupo,

			//             });

			//ddl_identificacion.DataTextField = "NomTipoIdentificacion";
			//ddl_identificacion.DataValueField = "Id_TipoIdentificacion";
			//ddl_identificacion.DataSource = tipoidentificación;
			//ddl_identificacion.DataBind();

			//ddl_perfil.DataTextField = "Rol";
			//ddl_perfil.DataValueField = "codRol";
			//ddl_perfil.DataSource = Roles;
			//ddl_perfil.DataBind();

			//var contacto = (from c in consultas.Db.Contactos
			//                from gc in consultas.Db.GrupoContactos
			//                where c.Id_Contacto == codusuario & c.Id_Contacto == gc.CodContacto
			//                select new
			//                {
			//                    nombres = c.Nombres,
			//                    apellidos = c.Apellidos,
			//                    codtipoidentificacion = c.CodTipoIdentificacion,
			//                    Tipoidentificacion = c.TipoIdentificacion,
			//                    identificacion = c.Identificacion,
			//                    cargo = c.Cargo,
			//                    email = c.Email,
			//                    telefono = c.Telefono,
			//                    fax = c.Fax,
			//                    codgrupo = gc.CodGrupo,
			//                    NomGrupo = gc.Grupo
			//                }).FirstOrDefault();


		}
		void void_ObtenerParametros()
		{
			//object usuario;
			//if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
			//{
			//    grupos1 = Request.QueryString["codGrupo"];
			//    grupos = grupos1.Split(',');
			//}


			if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
			{

				// codgrupousuario = Int32.Parse(Request.QueryString["codGrupo"]);
				if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
				{
					if (Request.QueryString["Accion"].ToString() == "Editar")
					{

						idusuario = Int32.Parse(Request.QueryString["CodProyecto"]);
						void_traerDatos(codgrupousuario, idusuario);//trae la información segun el usuario y el grupo al cual 

					}
					if (Request.QueryString["Accion"].ToString() == "Crear")
					{


					}

				}

			}
		}
		void void_ModificarTextolink()
		{
			//if (grupos.Contains(Constantes.CONST_AdministradorFonade.ToString()) ||
			//   grupos.Contains(Constantes.CONST_AdministradorSena.ToString()))
			//{
			//    AgregarUsuario.Text = crearAdmin;
			//}
			//if (grupos.Contains(Constantes.CONST_GerenteEvaluador.ToString()))
			//{
			//    AgregarUsuario.Text = crearGerenteEvaluador;
			//}
			//if (grupos.Contains(Constantes.CONST_CallCenter.ToString()))
			//{
			//    AgregarUsuario.Text = crearCallCenter;
			//}
			//if (grupos.Contains(Constantes.CONST_GerenteInterventor.ToString()))
			//{
			//    AgregarUsuario.Text = crearGerenteInterventor;
			//}
			//if (grupos.Contains(Constantes.CONST_Perfil_Fiduciario.ToString()))
			//{
			//    AgregarUsuario.Text = crearPerfilFiduciario;
			//}
			//string url = Request.Url.GetLeftPart(UriPartial.Path);
			//url += (Request.QueryString.ToString() == "") ? "?Accion=Crear" : "?" + Request.QueryString.ToString() + "&Accion=Crear";
			//AgregarUsuario.NavigateUrl = url;

		}
		public String[] grupos = { "0" };
		
		public string caption { get; set; }
		public string serializacion { get; set; }

		public void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				tb_nodocperfil.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");

				string Accion = "";
				if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
				{ Accion = Request.QueryString["Accion"].ToString(); }

				void_HideControls(Accion);

				lbl_Titulo.Text = void_establecerTitulo(grupos, Accion, "Proyecto");

				var nivelestudio = from ne in consultas.Db.NivelEstudios
								   orderby ne.Id_NivelEstudio
								   select new
								   {
									   Id_NivelEstudio = ne.Id_NivelEstudio,
									   NomNivelEstudio = ne.NomNivelEstudio
								   };

				var departamentos = from d in consultas.Db.departamento

									orderby d.NomDepartamento ascending
									select new
									{
										id_Departamento = d.Id_Departamento,
										nomDepartamento = d.NomDepartamento
									};

				var tipoaprendiz = from ta in consultas.Db.TipoAprendizs
								   select new
								   {
									   id_TIPOAPRENDIZ = ta.Id_TipoAprendiz,
									   NomTIPOAPRENDIZ = ta.NomTipoAprendiz
								   }
									  ;
				var sector = from s in consultas.Db.Sector

							 orderby s.NomSector ascending
							 select new
							 {
								 id_Sector = s.Id_Sector,
								 nomSector = s.NomSector
							 };

				var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions
										  select new
										  {
											  Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
											  NomTipoIdentificacion = ti.NomTipoIdentificacion
										  });

				var CiudadPrograma = (from c in consultas.Db.Ciudad
									  orderby c.NomCiudad ascending //orderby c.Id_Ciudad ascending
									  select new
									  {
										  Id_Ciudad = c.Id_Ciudad,
										  NomCiudad = c.NomCiudad
									  });

				var instituciones = (from ins in consultas.Db.InstitucionEducativas
									 where ins.NomInstitucionEducativa != "" || ins.NomInstitucionEducativa != null
									 orderby ins.NomInstitucionEducativa ascending
									 select new
									 {
										 ins.NomInstitucionEducativa,
										 ins.Id_InstitucionEducativa
									 });

				#region NO BORRAR! COMENTADO.

				////Inicializar variables.

				//DataTable instituciones = new DataTable();
				//String txtSQL = "";
				//String txtOrdenamiento = "";
				//String txtTituloPagina = "";
				//String TextoAdicionalTitulo = "";
				//int txtParametroConsulta = 0;
				//String txtNomCampoPoseeCodigo = "";
				//String txtNomCampoPoseeDescripcion = "";
				//String txtCondicion = "";

				//txtOrdenamiento = " ORDER BY 3";
				//txtTituloPagina = "INSTITUCIONES EDUCATIVAS  " + TextoAdicionalTitulo;
				//txtSQL = "SELECT PA.ID_PROGRAMAACADEMICO, PA.NOMPROGRAMAACADEMICO, IE.NOMINSTITUCIONEDUCATIVA, IE.ID_INSTITUCIONEDUCATIVA, C.NOMCIUDAD FROM PROGRAMAACADEMICO PA JOIN INSTITUCIONEDUCATIVA IE ON (IE.ID_INSTITUCIONEDUCATIVA = PA.CODINSTITUCIONEDUCATIVA) JOIN CIUDAD C ON (C.ID_CIUDAD= PA.CODCIUDAD) ";
				//txtSQL = "SELECT PA.ID_PROGRAMAACADEMICO, PA.NOMPROGRAMAACADEMICO, IE.NOMINSTITUCIONEDUCATIVA, IE.ID_INSTITUCIONEDUCATIVA, C.NOMCIUDAD FROM PROGRAMAACADEMICO PA JOIN INSTITUCIONEDUCATIVA IE ON (IE.ID_INSTITUCIONEDUCATIVA = PA.CODINSTITUCIONEDUCATIVA) JOIN CIUDAD C ON (C.ID_CIUDAD= PA.CODCIUDAD) ";
				//if (txtParametroConsulta != 0) { txtCondicion = " WHERE PA.CODNIVELESTUDIO = " + txtParametroConsulta; }

				//txtSQL = txtSQL + txtCondicion + txtOrdenamiento;

				//instituciones = consultas.ObtenerDataTable(txtSQL, "text");

				////foreach (DataRow row in dt.Rows)
				////{
				////    ListItem item = new ListItem();
				////    txtNomCampoPoseeCodigo = "ID_PROGRAMAACADEMICO";
				////    txtNomCampoPoseeDescripcion = "NOMPROGRAMAACADEMICO";
				////}

				#endregion

				ddl_Tipodocumentoperfil.DataTextField = "NomTipoIdentificacion";
				ddl_Tipodocumentoperfil.DataValueField = "Id_TipoIdentificacion";
				ddl_Tipodocumentoperfil.DataSource = tipoidentificación;
				ddl_Tipodocumentoperfil.DataBind();
				ddl_Tipodocumentoperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

				ddl_tipoaprendizperfil.DataTextField = "NomTIPOAPRENDIZ";
				ddl_tipoaprendizperfil.DataValueField = "id_TIPOAPRENDIZ";
				ddl_tipoaprendizperfil.DataSource = tipoaprendiz;
				ddl_tipoaprendizperfil.DataBind();
				ddl_tipoaprendizperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

				//SelDptoExpedicion.DataSource = departamentos;
				//SelDptoExpedicion.DataTextField = "nomDepartamento";
				//SelDptoExpedicion.DataValueField = "id_Departamento";
				//SelDptoExpedicion.DataBind();

				ddl_deptonacimientoperfil.DataSource = departamentos;
				ddl_deptonacimientoperfil.DataTextField = "nomDepartamento";
				ddl_deptonacimientoperfil.DataValueField = "id_Departamento";
				ddl_deptonacimientoperfil.DataBind();
				ddl_deptonacimientoperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

				//ddl_deptonacimientoperfil_Selected();

				ddl_depto1.DataSource = departamentos;
				ddl_depto1.DataTextField = "nomDepartamento";
				ddl_depto1.DataValueField = "id_Departamento";
				ddl_depto1.DataBind();

				ddl_depto1_Selected();

				ddl_depto2.DataSource = sector;
				ddl_depto2.DataTextField = "nomSector";
				ddl_depto2.DataValueField = "id_Sector";
				ddl_depto2.DataBind();

				ddl_depto2_Selected();

				ddl_deptoexpedicionperfil.DataSource = departamentos;
				ddl_deptoexpedicionperfil.DataTextField = "nomDepartamento";
				ddl_deptoexpedicionperfil.DataValueField = "id_Departamento";
				ddl_deptoexpedicionperfil.DataBind();
				ddl_deptoexpedicionperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

				ddl_deptoexpedicionperfil_OnSelected();

				ddl_nivelestudioperfil.DataSource = nivelestudio;
				ddl_nivelestudioperfil.DataTextField = "NomNivelEstudio";
				ddl_nivelestudioperfil.DataValueField = "Id_NivelEstudio";
				ddl_nivelestudioperfil.DataBind();
				ddl_nivelestudioperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

				void_ObtenerParametros();
			}
			ddl_estadoperfil.Attributes.Add("onchange", "controlFechas()");
			tb_fechaInicioperfil.Attributes.Add("onchange", "txtItm(event.target)");
			id_fechafinalizacion.Attributes.Add("onchange", "txtItm(event.target)");
			tb_fechagraduacionperfil.Attributes.Add("onchange", "txtItm(event.target)");
			var rfv = Request.Form["__EVENTTARGET"];
			if (IsPostBack && (rfv.IndexOf("Button1") > -1 || rfv.IndexOf("Button3") > -1 || rfv.IndexOf("hl_Emprendedor") > -1 || rfv.IndexOf("sd") > -1)){
				if (Equals(Request.Form["__EVENTTARGET"], "Button3")){
					Button3_Click1(sender, e);
				}
				if (Equals(Request.Form["__EVENTTARGET"], "Button1")){
					tb_crearPrograma_onclick(sender, e);
				}
				tstBtn.Text = Request.Form["__EVENTTARGET"].IndexOf("hl_Emprendedor") > -1 ? "Actualizar" : "Guardar";
				tstBtn.OnClientClick = Request.Form["__EVENTTARGET"].IndexOf("hl_Emprendedor") > -1 ? "__doPostBack('Button1','tb_crearPrograma_onclick')" : "__doPostBack('Button3','Button3_Click1')";
				serializacion = Request.Form["__EVENTTARGET"].IndexOf("hl_Emprendedor") > -1 ? "__doPostBack('Button1','tb_crearPrograma_onclick')" : "__doPostBack('Button3','Button3_Click1')";
			}
		}

		protected void gv_Proyectos_DataBound(object sender, EventArgs e)
		{

			//gv_Proyectos.Columns[2].Visible = false;
			//string codigosGrupo;
			//String[] grupos2 = { "0" };
			//String grupos3;
			//if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
			//{

			//    grupos3 = Request.QueryString["codGrupo"];
			//    grupos2 = grupos1.Split(',');
			//    if (grupos2.Contains(Constantes.CONST_AdministradorFonade.ToString()) | grupos2.Contains(Constantes.CONST_AdministradorSena.ToString()))
			//    {
			//        gv_administradores.Columns[3].Visible = true;

			//    }
			//    else
			//    {
			//        gv_administradores.Columns[3].Visible = false;
			//    }
			//}

		}
		protected void gv_Proyectos_RowDataBound(object sender, GridViewRowEventArgs e)
		{

			//string Conteo = "";
			////Conteo = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "cuantos"));
			//ImageButton imbtton = new ImageButton();
			//imbtton = ((ImageButton)e.Row.Cells[0].FindControl("btn_Inactivar"));
			//if (e.Row.RowType == DataControlRowType.DataRow)
			//{
			//    if (Conteo == "0")
			//    {
			//        imbtton.Visible = true;

			//    }
			//    else
			//    {

			//        imbtton.Visible = false;

			//    }
			//}


		}
		protected void gv_Proyectos_PageIndexChanged(object sender, GridViewPageEventArgs e)
		{

		}
		protected void lds_Proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{

			var proyectos = from p in consultas.Db.Proyecto
							from c in consultas.Db.Ciudad
							from dp in consultas.Db.departamento
							where c.Id_Ciudad == p.CodCiudad &
									p.CodContacto == usuario.IdContacto &
									dp.Id_Departamento == c.CodDepartamento &
									p.Inactivo == false & p.CodInstitucion == usuario.CodInstitucion
							orderby p.NomProyecto
							select new
							{
								Ciudad = c.NomCiudad + " ( " + dp.NomDepartamento + " ) ",
								Proyecto = p.NomProyecto,
								Id_proyecto = p.Id_Proyecto
							};
			if (e.Arguments.TotalRowCount == 0)
			{
				Lbl_Resultados.Visible = true;
				Lbl_Resultados.Text = "No hay datos en esta consulta";
			}
			else
			{
				Lbl_Resultados.Visible = false;
			}

			e.Result = proyectos.ToList();

		}
		protected void lds_Emprendedor_Selecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			int codproyecto = Int32.Parse(Request.QueryString["CodProyecto"]??"0");
			var Emprendedores = from pc in consultas.Db.ProyectoContactos
								from c in consultas.Db.Contacto

								where c.Id_Contacto == pc.CodContacto &
									   pc.Inactivo == false &
									   pc.CodRol == Constantes.CONST_RolEmprendedor &
									   pc.CodProyecto == codproyecto
								orderby c.Nombres
								select new
								{
									id_contacto = c.Id_Contacto,
									Nombre = c.Nombres + " " + c.Apellidos,
									Email = c.Email,
									CodProyecto = codproyecto
								};
			if (e.Arguments.TotalRowCount == 0)
			{
				Lbl_Resultados.Visible = true;
				Lbl_Resultados.Text = "No hay datos en esta consulta";
			}
			else
			{
				Lbl_Resultados.Visible = false;
			}

			e.Result = Emprendedores.ToList();
		}

		protected void btn_crearActualizar_onclick(object sender, EventArgs e)
		{

			int codigoContacto = 0;
			if (!String.IsNullOrEmpty(Request.QueryString["ID_Proyecto"]))
			{
				codigoContacto = Int32.Parse(Request.QueryString["ID_Proyecto"]);
			}
			string accion = Request.QueryString["Accion"].ToString();
			switch (accion)
			{
				case "Crear":
					codigoContacto = int_CrearDatos(codigoContacto);
					if (codigoContacto != -1)
					{
						#region Diego Quiñonez - 11 de Diciembre de 2014

						//var tgh = new AgendarTarea(usuario.IdContacto, "Registro Plan de Negocio", "Registro Plan de Negocio", codigoContacto.ToString(), 1, "0", true, 0, true, true, usuario.IdContacto, string.Empty, string.Empty, string.Empty);
						//tgh.Agendar();


						//var uhb = new Clases.genericQueries().getId_ContactoJefeUnidad(usuario.CodInstitucion);

						var uhb = (from ic in consultas.Db.InstitucionContacto where ic.FechaFin == null && ic.CodInstitucion == usuario.CodInstitucion select ic.CodContacto).FirstOrDefault();

						#endregion

						var nbv = new AgendarTarea(uhb, "Registro Plan de Negocio", "Registro Plan de Negocio", codigoContacto.ToString(), 1, "0", true, 0,
													true, true, usuario.IdContacto, string.Empty, string.Empty, string.Empty);
						nbv.Agendar();
						
						string destUrl = string.Format("{0}://{1}{2}/", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
						if (destUrl.EndsWith("/"))
							destUrl = destUrl.TrimEnd(new char[] { '/' });
						if (!string.IsNullOrEmpty(Request.QueryString["Accion"]))
						{
							destUrl = string.Format("{0}?Accion={1}&CodProyecto={2}", destUrl, "Editar", codigoContacto);
							Response.Redirect(destUrl);
						}
					}
					break;
				case "Editar":
					void_ModificarDatos(codigoContacto);
					string destUrl1 = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
					Response.Redirect(destUrl1);
					break;
				default: break;
			}

		}
		protected void gv_Proyectos_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.Header)
			{
				foreach (TableCell tc in e.Row.Cells)
				{
					if (tc.HasControls())
					{
						// buscar el link del header
						LinkButton lnk = (LinkButton)tc.Controls[0];
						if (lnk != null && gv_Proyectos.SortExpression == lnk.CommandArgument)
						{
							// inicializar nueva imagen
							System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
							// url de la imagen dinamicamente
							img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_Proyectos.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
							// a ñadir el espacio de la imagen
							tc.Controls.Add(new LiteralControl(" "));
							tc.Controls.Add(img);

						}
					}
				}
			}
		}
		protected void ddl_depto_OnSelectedIndexChanged1(object sender, EventArgs e)
		{
			ddl_depto_OnSelectedI1();
		}

		private void ddl_depto_OnSelectedI1()
		{
			if (ddl_depto1.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_depto1.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });
				ddl_ciudad1.DataSource = municipios;
				ddl_ciudad1.DataTextField = "Ciudad";
				ddl_ciudad1.DataValueField = "ID_Ciudad";
				ddl_ciudad1.DataBind();
			}
		}

		protected void ddl_depto_OnSelectedIndexChanged2(object sender, EventArgs e)
		{
			ddl_depto_OnSelectedI();
		}

		private void ddl_depto_OnSelectedI()
		{
			if (ddl_depto2.SelectedValue != "")
			{
				var subsectores = (from s in consultas.Db.SubSector
								   where s.CodSector == Int32.Parse(ddl_depto2.SelectedValue)
								   orderby s.NomSubSector
								   select new
								   {
									   Sumsector = s.NomSubSector,
									   Id_subsector = s.Id_SubSector
								   });
				ddl_ciudad2.DataSource = subsectores;
				ddl_ciudad2.DataTextField = "Sumsector";
				ddl_ciudad2.DataValueField = "Id_subsector";
				ddl_ciudad2.DataBind();
			}
		}

		protected void hl_Emprendedor_click(object sender, CommandEventArgs e)
		{
			//RequiredFieldValidator1.Enabled = false;
			//RequiredFieldValidator2.Enabled = false;
		}

		protected void ddl_depto_OnSelectedIndexChanged11(object sender, EventArgs e)
		{
			ddl_depto_OnSelected1();
		}

		private void ddl_depto_OnSelected1()
		{
			if (ddl_depto1.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_depto1.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });
				ddl_ciudad1.DataSource = municipios;
				ddl_ciudad1.DataTextField = "Ciudad";
				ddl_ciudad1.DataValueField = "ID_Ciudad";
				ddl_ciudad1.DataBind();
			}
		}

		protected void ddl_depto_OnSelectedIndexChanged22(object sender, EventArgs e)
		{
			ddl_depto_OnSelected();
		}

		private void ddl_depto_OnSelected()
		{
			if (ddl_depto2.SelectedValue != "")
			{
				var subsectores = (from s in consultas.Db.SubSector
								   where s.CodSector == Int32.Parse(ddl_depto2.SelectedValue)
								   orderby s.NomSubSector
								   select new
								   {
									   Sumsector = s.NomSubSector,
									   Id_subsector = s.Id_SubSector
								   });
				ddl_ciudad2.DataSource = subsectores;
				ddl_ciudad2.DataTextField = "Sumsector";
				ddl_ciudad2.DataValueField = "Id_subsector";
				ddl_ciudad2.DataBind();
			}
		}

		protected void ddl_deptoexpedicionperfil_OnSelectedIndexChanged(object sender, EventArgs e)
		{

			ddl_deptoexpedicionperfil_OnSelected();
		}

		private void ddl_deptoexpedicionperfil_OnSelected()
		{
			if (ddl_deptoexpedicionperfil.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_deptoexpedicionperfil.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });

				ddl_ciudadexpedicionperfil.DataSource = municipios;
				ddl_ciudadexpedicionperfil.DataTextField = "Ciudad";
				ddl_ciudadexpedicionperfil.DataValueField = "ID_Ciudad";
				ddl_ciudadexpedicionperfil.DataBind();
				ddl_ciudadexpedicionperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
			}
		}

		protected void ddl_deptoprograma_onselectitemchanged(object sender, EventArgs e)
		{
			ddl_deptoprograma_onselect();
		}

		protected void SelDptoExpedicion_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			string txtSQL = "SELECT * FROM Ciudad WHERE CodDepartamento = " + SelDptoExpedicion.SelectedValue;
			var dt = consultas.ObtenerDataTable(txtSQL, "text");

			SelMunExpedicion.Items.Clear();

			foreach (DataRow row in dt.Rows)
			{
				ListItem item = new ListItem();
				item.Value = row["ID_Ciudad"].ToString();
				item.Text = row["NomCiudad"].ToString();
				SelMunExpedicion.Items.Add(item);
			}

			txtSQL = null;
			dt = null;
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// SelectedIndexChanged del Departamento CREACIÓN DEL PROGRAMA ACADÉMICO.
		/// </summary>
		private void ddl_deptoprograma_onselect()
		{
			#region LINQ comentado.
			//var municipios = (from c in consultas.Db.Ciudads
			//                  where c.CodDepartamento == Int32.Parse(SelDptoExpedicion.SelectedValue)
			//                  orderby c.NomCiudad ascending
			//                  select new
			//                  {
			//                      Ciudad = c.NomCiudad,
			//                      ID_Ciudad = c.Id_Ciudad
			//                  }); 
			#endregion

			string txtSQL = "SELECT * FROM Ciudad WHERE CodDepartamento = " + SelDptoExpedicion.SelectedValue;
			var dt = consultas.ObtenerDataTable(txtSQL, "text");

			SelMunExpedicion.Items.Clear();

			foreach (DataRow row in dt.Rows)
			{
				ListItem item = new ListItem();
				item.Value = row["ID_Ciudad"].ToString();
				item.Text = row["NomCiudad"].ToString();
				SelMunExpedicion.Items.Add(item);
			}

			txtSQL = null;
			dt = null;
		}

		protected void ddl_deptonacimientoperfil_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			ddl_deptonacimientoperfil_OnSelected();

		}

		private void ddl_deptonacimientoperfil_OnSelected()
		{
			if (ddl_deptonacimientoperfil.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_deptonacimientoperfil.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });

				ddl_ciudadonacimientoperfil.DataSource = municipios;
				ddl_ciudadonacimientoperfil.DataTextField = "Ciudad";
				ddl_ciudadonacimientoperfil.DataValueField = "ID_Ciudad";
				ddl_ciudadonacimientoperfil.DataBind();
			}
		}

		protected void btn_CrearEmprendedor_onclick(object sender, EventArgs e)
		{
			//pnl_infoAcademica.Visible = true;
			gv_emprendedor.Visible = true;
			pnl_infoAcademica.Visible = true;

			pnl_crearEditar.Visible = false;
			tc_Emprendedor.Visible = true;
			pnl_infoAcademica.Visible = true;
		}

		protected void btn_InstitucionEducativa_onclick(object sender, CommandEventArgs e)
		{
			String proramainstitucion;
			proramainstitucion = traerinfoacademica(e.CommandArgument.ToString());
			tb_Programarealizadoperfil.Text = proramainstitucion.Split(';')[0];
			tb_Institucionperfil.Text = proramainstitucion.Split(';')[1];
		}

		protected string traerinfoacademica(string infoacademica)
		{
			//SELECT CE.TITULOOBTENIDO,CE.INSTITUCION,CE.CODCIUDAD,CE.CODNIVELESTUDIO,CE.CODPROGRAMAACADEMICO,
			//CE.FINALIZADO,CE.FECHAINICIO,CE.FECHAFINMATERIAS,CE.FECHAGRADO,CE.FECHAULTIMOCORTE,CE.SEMESTRESCURSADOS,
			//C.NOMCIUDAD,PA.NOMPROGRAMAACADEMICO FROM CONTACTOESTUDIO CE JOIN
			//CIUDAD C ON (C.ID_CIUDAD = CE.CODCIUDAD) JOIN PROGRAMAACADEMICO PA ON
			//(PA.ID_PROGRAMAACADEMICO = CE.CODPROGRAMAACADEMICO) WHERE CODCONTACTO =" & fnRequest("CodContacto") & " AND 
			//FLAGINGRESADOASESOR =1"
			string programa;
			string institucion;

			var infoac = (from pa in consultas.Db.ProgramaAcademicos
						  join ie in consultas.Db.InstitucionEducativas on pa.CodInstitucionEducativa equals ie.Id_InstitucionEducativa
						  join c in consultas.Db.Ciudad on pa.CodCiudad equals c.Id_Ciudad
						  where pa.Id_ProgramaAcademico == Int32.Parse(infoacademica.ToString())

						  select new
						  {
							  NomProgramaAcademico = pa.NomProgramaAcademico,
							  NomInstitucionEducativa = ie.NomInstitucionEducativa,
						  }).FirstOrDefault();
			programa = infoac.NomProgramaAcademico;
			institucion = infoac.NomInstitucionEducativa;
			tp_perfil.Visible = true;
			tp_infoacademica.Visible = true;
			tc_Emprendedor.ActiveTab = tp_perfil;
			return (programa + ";" + institucion);
		}

		protected void agregarprogama_onclick(object sender, EventArgs e)
		{
			gv_institucion.Visible = false;
			//Ocultar el espacio para CREAR el programa académico.
			Panel2.Visible = false;
		}

		protected void tb_crearPrograma_onclick(object sender, EventArgs e)
		{
			//Int64 contactoid;
			objectEmcap Emprendedor = new objectEmcap();

			#region vvariable
			Emprendedor._nombreE = tb_nombreperfil.Text;
			Emprendedor._apellidoE = tb_apellidoperfil.Text;
			Emprendedor._TipoIdentificacionE = ddl_Tipodocumentoperfil.SelectedValue.ToString();
			Emprendedor._noIdentificacion1 = tb_nodocperfil.Text;
			Emprendedor._departamentExpedicion1 = ddl_deptoexpedicionperfil.SelectedValue.ToString();
			Emprendedor._ciudadExpedicionE1 = ddl_ciudadexpedicionperfil.SelectedValue.ToString();
			Emprendedor._emailE1 = tb_correoperfil.Text;
			Emprendedor._generoE1 = ddl_generoperfil.SelectedValue.ToString();
			Emprendedor._fechaNaciE1 = DateTime.Parse(tb_fechanacimiento.Text, System.Globalization.CultureInfo.CreateSpecificCulture("es-LT"), DateTimeStyles.AdjustToUniversal);
			Emprendedor._departamentoNacimientoE = ddl_deptonacimientoperfil.SelectedValue.ToString();
			Emprendedor._ciudadnacimientoE = ddl_ciudadonacimientoperfil.SelectedValue.ToString();
			Emprendedor._telefonoE1 = tb_telefonoperfil.Text;
			Emprendedor._nivelEstudioE = ddl_nivelestudioperfil.SelectedValue.ToString();
			Emprendedor._tituloObtenidoE = tb_Programarealizadoperfil.Text;
			Emprendedor._institucionE = tb_Institucionperfil.Text;
			Emprendedor._ciudadIE = tb_ciudadinstitucionperfil.Text;//pendiente control
			Emprendedor._estadoestudiosE = ddl_estadoperfil.SelectedValue.ToString();
			Emprendedor._fechainicioE = DateTime.Parse(tb_fechaInicioperfil.Text, System.Globalization.CultureInfo.CreateSpecificCulture("es-LT"), DateTimeStyles.AdjustToUniversal).ToShortDateString();
			if(Emprendedor._estadoestudiosE == "1")
			{
				Emprendedor._fechafinalizacionMateriaE = DateTime.Parse(id_fechafinalizacion.Text, System.Globalization.CultureInfo.CreateSpecificCulture("es-LT"), DateTimeStyles.AdjustToUniversal).ToShortDateString();
				Emprendedor._FechaGraduacionE = DateTime.Parse(tb_fechagraduacionperfil.Text, System.Globalization.CultureInfo.CreateSpecificCulture("es-LT"), DateTimeStyles.AdjustToUniversal).ToShortDateString();
			}
			else
			{
				Emprendedor._fechafinalizacionMateriaE = "";
				Emprendedor._FechaGraduacionE = "";
			}
			
			//Emprendedor._fehcaGradoE = tb_fechagrado.Text;
			Emprendedor._condicionEspecialE = ddl_condicionespecialperfil.SelectedValue.ToString();
			Emprendedor._tipoEmprendizE = Int32.Parse(ddl_tipoaprendizperfil.SelectedValue);

			tb_fechanacimiento.Text = Emprendedor._fechaNaciE1.ToShortDateString();
			tb_fechaInicioperfil.Text = Emprendedor._fechainicioE;
			id_fechafinalizacion.Text = Emprendedor._fechafinalizacionMateriaE;
			tb_fechagraduacionperfil.Text = Emprendedor._FechaGraduacionE;

			#endregion


			#region validarEntradas
			if (String.IsNullOrEmpty(Emprendedor._nombreE))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Nombre es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._apellidoE))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo apellido es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._TipoIdentificacionE))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo tipo de identificacion es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._noIdentificacion1))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo No de Identificacion es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._departamentExpedicion1))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Departamento de Expedicion es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._ciudadExpedicionE1))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Ciudad de Expedicion es requerido')", true);
				return;
			}

			if (!DateTime.TryParse(Emprendedor._fechaNaciE1.ToString(), out fecha_Validar) || String.IsNullOrEmpty(Emprendedor._fechaNaciE1.ToString()))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo fecha de nacimiento es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._departamentoNacimientoE))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo departamento de nacimiento es requerido')", true);
				return;
			}

			if (String.IsNullOrEmpty(Emprendedor._ciudadnacimientoE))
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo ciudad de nacimiento es requerido')", true);
				return;
			}

			#endregion

			#region validarexpresiones

			Int64 valida = 0;

			if (!Int64.TryParse(Emprendedor._noIdentificacion1, out valida))
			{
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo numero de identificacion tiene que ser numero')", true);
					return;
				}
			}

			if (!string.IsNullOrEmpty(Emprendedor._emailE1))
			{
				if (!IsValidEmail(Emprendedor._emailE1))
				{
					{
						ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El correo no es valido')", true);
						return;
					}
				}
			}

			if (!string.IsNullOrEmpty(Emprendedor._telefonoE1))
			{
				if (!Int64.TryParse(Emprendedor._telefonoE1, out valida))
				{
					{
						ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo telefono tiene que ser numero')", true);
						return;
					}
				}
			}


			#endregion

			string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

			using (var con = new SqlConnection(conexionStr))
			{
				using (var com = con.CreateCommand())
				{
					com.CommandText = "MD_ActualizarContactoEmprendedor";
					com.CommandType = System.Data.CommandType.StoredProcedure;
					com.Parameters.AddWithValue("@nombreContacto", Emprendedor._nombreE);
					com.Parameters.AddWithValue("@apellidoContacto", Emprendedor._apellidoE);
					com.Parameters.AddWithValue("@CodTipoIdentificacion", Emprendedor._TipoIdentificacionE);
					com.Parameters.AddWithValue("@Identificacion", Emprendedor._noIdentificacion1);
					com.Parameters.AddWithValue("@Genero", Emprendedor._generoE1);
					com.Parameters.AddWithValue("@FechaNacimiento", Emprendedor._fechaNaciE1);
					com.Parameters.AddWithValue("@Email", Emprendedor._emailE1);
					com.Parameters.AddWithValue("@Telefono", Emprendedor._telefonoE1);
					com.Parameters.AddWithValue("@CodCiudad", Emprendedor._ciudadnacimientoE);
					com.Parameters.AddWithValue("@LugarExpedicionDI", Emprendedor._ciudadExpedicionE1);
					// Validar que no guarde espacios en blanco
					try
					{
						con.Open();
						com.ExecuteReader();
					}
					catch (Exception ex)
					{
						throw new Exception(ex.Message);
					}
					finally
					{
					  com.Dispose();
						con.Close();
						con.Dispose();
						gv_emprendedor.DataBind();
						string redirePagina = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
						Response.Redirect(redirePagina + "?Accion=Editar&CodProyecto=" + Request.QueryString["CodProyecto"].ToString());
					}
				}
			}
		}


		protected void btn_Inactivar_click(object sender, CommandEventArgs e)
		{


			int codproyecto = 0;
			if (Request.QueryString["Codproyecto"] != null)
			{
				codproyecto = Int32.Parse(Request.QueryString["Codproyecto"]);
			}
			int conteo;
			var query = from tu in consultas.Db.TareaUsuarios
						from tur in consultas.Db.TareaUsuarioRepeticions
						where tu.Id_TareaUsuario == tur.CodTareaUsuario & tur.FechaCierre == null
							   & tu.CodContacto == Int32.Parse(e.CommandArgument.ToString())
						select new
						{
							IdTareaUsuario = tu.Id_TareaUsuario
						};

			conteo = query.Count();
			if (conteo == 0)
			{
				var query2 = (from gc in
								  consultas.Db.GrupoContactos
							  where gc.CodContacto == Int32.Parse(e.CommandArgument.ToString())
							  select gc
								 );
				var query3 = from pc in consultas.Db.ProyectoContactos
							 where pc.CodContacto == Int32.Parse(e.CommandArgument.ToString()) &
							   pc.CodProyecto == codproyecto &
							   pc.CodRol == Constantes.CONST_RolEmprendedor & pc.Inactivo == false
							 select pc;
				var query4 = from c in consultas.Db.Contacto
							 where c.Id_Contacto == Int32.Parse(e.CommandArgument.ToString())
							 select c;
				foreach (ProyectoContacto pc in query3)
				{
					pc.Inactivo = true;

					pc.FechaFin = DateTime.Today;
				}

				try
				{
					consultas.Db.SubmitChanges();
					void_show("La información del usuario ha sido actualizado correctamente", true);

				}
				catch
				{ }

				foreach (Contacto c in query4)
				{
					c.Inactivo = true;
				}

				try
				{
					consultas.Db.SubmitChanges();
					void_show("La información del usuario ha sido actualizado correctamente", true);

				}
				catch
				{ }
				consultas.Db.GrupoContactos.DeleteAllOnSubmit(query2);
				consultas.Db.SubmitChanges();
				Lbl_Resultados.Visible = true;
				Lbl_Resultados.Text = e.CommandArgument.ToString();
			}
			else
			{
				Lbl_Resultados.Visible = true;
				Lbl_Resultados.Text = "este usuario tiene tareas pendientes";
			}

			gv_emprendedor.DataBind();


		}

		protected void btn_aceptarperfil_Click(object sender, EventArgs e)
		{

		}

		public bool IsValidEmail(string strIn)
		{
			return

			Regex.IsMatch(strIn, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
					  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase);
		}

		private void insertarContactoEstudio(objectEmcap Emprendedor, Int64 contactoid)
		{
			var ejecutaReader = new Clases.genericQueries();
			String txtAnioTitulo;
			String coProgramaAcademico = "";
			String codCiudad = "";

			if (Emprendedor._estadoestudiosE.Equals("1"))
			{
				if (ddl_estadoperfil.SelectedValue =="1")
				{
					txtAnioTitulo = Emprendedor._FechaGraduacionE.Split('/')[2];
				}
				else
				{
					txtAnioTitulo = null;
				}
				
			}
			else
			{
				txtAnioTitulo = "NULL";
			}

			SqlDataReader reader = ejecutaReader.executeQueryReader(@"SELECT Id_ProgramaAcademico FROM ProgramaAcademico
													  INNER JOIN InstitucionEducativa ON CodInstitucionEducativa = Id_InstitucionEducativa
													  WHERE [NomProgramaAcademico] = '" + Emprendedor._tituloObtenidoE + @"'
													  AND CodCiudad = (SELECT Id_Ciudad FROM Ciudad WHERE NomCiudad = '" + Emprendedor._ciudadIE + "')");

			establecer(ref coProgramaAcademico, reader);

			reader = ejecutaReader.executeQueryReader("SELECT Id_Ciudad FROM Ciudad WHERE NomCiudad = '" + Emprendedor._ciudadIE + "'");

			establecer(ref codCiudad, reader);

			txtAnioTitulo = txtAnioTitulo == "NULL" ? string.Empty : txtAnioTitulo;

			String txtSQL = @"INSERT INTO CONTACTOESTUDIO
							(CODCONTACTO,CODPROGRAMAACADEMICO, TITULOOBTENIDO, ANOTITULO, INSTITUCION, CODCIUDAD,
							CODNIVELESTUDIO,FINALIZADO,FECHAINICIO,FECHAFINMATERIAS,FECHAGRADO,FECHAULTIMOCORTE,
						   SEMESTRESCURSADOS,FlagIngresadoAsesor,FECHACREACION) VALUES (" + @"
							'" + contactoid + "','" + coProgramaAcademico + "','" + Emprendedor._tituloObtenidoE + "','" + txtAnioTitulo + "','" + Emprendedor._institucionE + "','" + codCiudad + "','" +
							Emprendedor._nivelEstudioE + "','" + Emprendedor._estadoestudiosE + "',convert(datetime,'" + Emprendedor._fechainicioE + "', 103),convert(datetime,'" + Emprendedor._fechafinalizacionMateriaE + "', 103),convert(datetime,'" + Emprendedor._FechaGraduacionE + "', 103),NULL,NULL,'1',GETDATE())";
			ejecutaReader.executeQueryReader(txtSQL, 1);
		}

		private void establecer(ref String valor, SqlDataReader reader)
		{
			if (reader != null)
			{
				if (reader.Read())
					valor = reader[0].ToString();
				else
					valor = "";
			}
			else
				valor = "";
		}

		private void enviarPorEmail(String toTxt, String asuntoTxt, String mensajeTxt)
		{
			string To;
			string Subject;
			string Body;

			MailMessage mail;
			//Attachment Data;

			if (!(toTxt.Trim() == ""))
			{
				To = toTxt;
				Subject = asuntoTxt;
				Body = mensajeTxt;

				mail = new MailMessage();
				mail.To.Add(new MailAddress(To));
				mail.From = new MailAddress(usuario.Email);
				mail.Subject = Subject;
				mail.Body = Body;
				mail.IsBodyHtml = false;

				//if (!(archivoAdjTxt.Text.Trim() == ""))
				//{
				//    Data = new Attachment(archivoAdjTxt.Text, MediaTypeNames.Application.Octet);
				//    mail.Attachments.Add(Data);
				//}

				SmtpClient client = new SmtpClient("smtpcorp.com", 25);
				using (client)
				{
					//variables de configuracion de prueba

					var SMTPUsuario = ConfigurationManager.AppSettings.Get("SMTPUsuario");
					var SMTPPassword = ConfigurationManager.AppSettings.Get("SMTPPassword");

					client.Credentials = new System.Net.NetworkCredential(SMTPUsuario, SMTPPassword);
					client.EnableSsl = true;
					client.Send(mail);
				}
				//System.Windows.Forms.MessageBox.Show("", "Correcto", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// Agregar Emprendedor "mostrando y ocultando datos".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_CrearEmprendedor_Click(object sender, EventArgs e)
		{
			tc_Emprendedor.ActiveTabIndex = 0;
			limpiarCampos();

			#region Vaciar los campos y remover la selección de los dropdownlist.

			tb_apellidoperfil.Text = "";
			tb_correoperfil.Text = "";
			tb_nodocperfil.Text = "";
			tb_fechanacimiento.Text = "";
			tb_nombreperfil.Text = "";
			tb_telefonoperfil.Text = "";

			//foreach (ListItem item in ddl_Tipodocumentoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_Tipodocumentoperfil.SelectedIndex = 0;

			//foreach (ListItem item in ddl_generoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_generoperfil.SelectedIndex = 0;

			//foreach (ListItem item in ddl_deptoexpedicionperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_deptoexpedicionperfil.SelectedIndex = 0;

			//foreach (ListItem item in ddl_ciudadexpedicionperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			//ddl_ciudadexpedicionperfil.SelectedIndex = 0;

			//foreach (ListItem item in ddl_deptonacimientoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_deptonacimientoperfil.SelectedIndex = 0;

			//foreach (ListItem item in ddl_ciudadonacimientoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			//ddl_ciudadonacimientoperfil.SelectedIndex = 0;

			tb_ciudadinstitucionperfil.Text = "";
			id_fechafinalizacion.Text = "";
			tb_fechagraduacionperfil.Text = "";
			tb_fechaInicioperfil.Text = "";

			//foreach (ListItem item in ddl_nivelestudioperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_nivelestudioperfil.SelectedIndex = 0;

			tb_Programarealizadoperfil.Text = "";

			tb_Institucionperfil.Text = "";

			//foreach (ListItem item in ddl_estadoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }
			ddl_estadoperfil.SelectedValue = "-";

			tb_fechaInicioperfil.Text = "";

			// RJ Calendarextender4.SelectedDate = DateTime.Today;
			id_fechafinalizacion.Text = "";

			// RJ Calendarextender3.SelectedDate = DateTime.Today;
			tb_fechagraduacionperfil.Text = "";

			foreach (ListItem item in ddl_condicionespecialperfil.Items)
			{ if (item.Selected) { item.Selected = false; break; } }

			foreach (ListItem item in ddl_tipoaprendizperfil.Items)
			{ if (item.Selected) { item.Selected = false; break; } }

			#endregion

			gv_emprendedor.Visible = true;
			pnl_infoAcademica.Visible = true;

			pnl_crearEditar.Visible = false;
			tc_Emprendedor.Visible = true;

			CargarCiudades_BusquedaInstituciones();
			HttpContext.Current.Session["txtTipoConsulta"] = "programa";
			tstBtn.Text = "Guardar";
			tstBtn.OnClientClick = "__doPostBack('Button3','Button3_Click1')";

			if(!filafecha.Visible)
			{
				filafecha.Visible = true;
				filafecha2.Visible = true;
			}

			tb_fechaInicioperfil.Text = "";
			id_fechafinalizacion.Text = "";
			tb_fechagraduacionperfil.Text = "";

			ClearControlsForm();
		}

		private void ClearControlsForm()
		{
			tb_apellidoperfil.Text = "";
			tb_correoperfil.Text = "";
			tb_nodocperfil.Text = "";
			tb_fechanacimiento.Text = "";
			tb_nombreperfil.Text = "";
			tb_telefonoperfil.Text = "";
			tb_fechaInicioperfil.Text = "";
			id_fechafinalizacion.Text = "";
			tb_fechagraduacionperfil.Text = "";
			tb_ciudadinstitucionperfil.Text = "";
			tb_Programarealizadoperfil.Text = "";
			tb_Institucionperfil.Text = "";
			ddl_estadoperfil.SelectedValue = "-";

			ddl_Tipodocumentoperfil.SelectedIndex = 0;
			ddl_generoperfil.SelectedIndex = 0;
			ddl_deptoexpedicionperfil.SelectedIndex = 0;
			ddl_ciudadexpedicionperfil.DataSource = null;
			ddl_ciudadexpedicionperfil.DataBind();
			ddl_deptonacimientoperfil.SelectedIndex = 0;
			ddl_ciudadonacimientoperfil.DataSource = null;
			ddl_ciudadonacimientoperfil.DataBind();
			ddl_nivelestudioperfil.SelectedIndex = 0;
			
		}

		/// <summary>
		/// Agregar Emprendedor "mostrando y ocultando datos".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void image_i_Click(object sender, ImageClickEventArgs e)
		{
			tc_Emprendedor.ActiveTabIndex = 0;
			limpiarCampos();

			#region Vaciar los campos y remover la selección de los dropdownlist.

			tb_apellidoperfil.Text = "";
			tb_correoperfil.Text = "";
			tb_nodocperfil.Text = "";
			tb_fechanacimiento.Text = "";
			tb_nombreperfil.Text = "";
			tb_telefonoperfil.Text = "";
			tb_fechaInicioperfil.Text = "";
			id_fechafinalizacion.Text = "";
			tb_fechagraduacionperfil.Text = "";

			//foreach (ListItem item in ddl_Tipodocumentoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_generoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_deptoexpedicionperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_ciudadexpedicionperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_deptonacimientoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_ciudadonacimientoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			tb_ciudadinstitucionperfil.Text = "";
			id_fechafinalizacion.Text = "";
			tb_fechagraduacionperfil.Text = "";
			tb_fechaInicioperfil.Text = "";

			//foreach (ListItem item in ddl_nivelestudioperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			tb_Programarealizadoperfil.Text = "";

			tb_Institucionperfil.Text = "";

			//foreach (ListItem item in ddl_estadoperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			tb_fechaInicioperfil.Text = "";

			Calendarextender4.SelectedDate = DateTime.Today;
			id_fechafinalizacion.Text = "";

			Calendarextender3.SelectedDate = DateTime.Today;
			tb_fechagraduacionperfil.Text = "";

			//foreach (ListItem item in ddl_condicionespecialperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			//foreach (ListItem item in ddl_tipoaprendizperfil.Items)
			//{ if (item.Selected) { item.Selected = false; break; } }

			#endregion

			gv_emprendedor.Visible = true;
			pnl_infoAcademica.Visible = true;

			pnl_crearEditar.Visible = false;
			tc_Emprendedor.Visible = true;
			
			tstBtn.Text = "Guardar";
			tstBtn.OnClientClick = "__doPostBack('Button3','Button3_Click1')";

			ClearControlsForm();
		}

		//protected void btnYes_Click(object sender, EventArgs e)
		//{
		//    string destUrl1 = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
		//    Response.Redirect(destUrl1);
		//}

		private void limpiarCampos()
		{
			foreach (Control control in tbl_Convenio.Controls)
			{
				try
				{
					((TextBox)control).Text = "";
				}
				catch (Exception) { }
			}

			ClearControlsForm();
		}

		/// <summary>
		/// Crear el emprendedor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>        
		protected void Button3_Click1(object sender, EventArgs e)
		{
			if(cont == 0)
			{
				string redirePagina = string.Empty;
				//Inicializar variables.
				Int64 contactoid;
				objectEmcap Emprendedor = new objectEmcap();
				String ciudad = "";

				#region Asignar valores a los atributos de la clase "objectEmcap".

				Emprendedor._nombreE = tb_nombreperfil.Text;
				Emprendedor._apellidoE = tb_apellidoperfil.Text;
				Emprendedor._TipoIdentificacionE = ddl_Tipodocumentoperfil.SelectedValue.ToString();
				Emprendedor._noIdentificacion1 = tb_nodocperfil.Text;
				Emprendedor._departamentExpedicion1 = ddl_deptoexpedicionperfil.SelectedValue.ToString();
				Emprendedor._ciudadExpedicionE1 = ddl_ciudadexpedicionperfil.SelectedValue.ToString();
				Emprendedor._emailE1 = tb_correoperfil.Text;
				Emprendedor._generoE1 = ddl_generoperfil.SelectedValue.ToString();
				Emprendedor._fechaNaciE1 = DateTime.Parse(tb_fechanacimiento.Text, System.Globalization.DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal);
				Emprendedor._departamentoNacimientoE = ddl_deptonacimientoperfil.SelectedValue.ToString();
				Emprendedor._ciudadnacimientoE = ddl_ciudadonacimientoperfil.SelectedValue.ToString();
				Emprendedor._telefonoE1 = tb_telefonoperfil.Text;
				Emprendedor._nivelEstudioE = ddl_nivelestudioperfil.SelectedValue.ToString();
				Emprendedor._tituloObtenidoE = tb_Programarealizadoperfil.Text;
				Emprendedor._institucionE = tb_Institucionperfil.Text;
				Emprendedor._ciudadIE = tb_ciudadinstitucionperfil.Text;//pendiente control
				Emprendedor._estadoestudiosE = ddl_estadoperfil.SelectedValue.ToString();
				//Emprendedor._fechainicioE = DateTime.Parse(tb_fechaInicioperfil.Text, System.Globalization.DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal).ToShortDateString();
				Emprendedor._fechainicioE = tb_fechaInicioperfil.Text;
				//Emprendedor._fechafinalizacionMateriaE = DateTime.Parse(id_fechafinalizacion.Text, System.Globalization.DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal).ToShortDateString();
				Emprendedor._fechafinalizacionMateriaE = id_fechafinalizacion.Text;
				//Emprendedor._FechaGraduacionE = DateTime.Parse(tb_fechagraduacionperfil.Text, System.Globalization.DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal).ToShortDateString();
				Emprendedor._FechaGraduacionE = tb_fechagraduacionperfil.Text;
				//Emprendedor._fehcaGradoE = tb_fechagrado.Text;
				Emprendedor._condicionEspecialE = ddl_condicionespecialperfil.SelectedValue.ToString();
				Emprendedor._tipoEmprendizE = Int32.Parse(ddl_tipoaprendizperfil.SelectedValue);

				//tb_fechanacimiento.Text = Emprendedor._fechaNaciE1.ToShortDateString();
				//tb_fechaInicioperfil.Text = Emprendedor._fechainicioE;
				//id_fechafinalizacion.Text = Emprendedor._fechafinalizacionMateriaE;
				//tb_fechagraduacionperfil.Text = Emprendedor._FechaGraduacionE;


				#endregion

				#region Validar entradas.

				if (String.IsNullOrEmpty(Emprendedor._nombreE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Nombre es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._apellidoE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo apellido es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._TipoIdentificacionE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo tipo de identificacion es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._noIdentificacion1))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo No de Identificacion es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._departamentExpedicion1))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Departamento de Expedicion es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._ciudadExpedicionE1))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo Ciudad de Expedicion es requerido')", true);
					return;
				}

				if (!DateTime.TryParse(Emprendedor._fechaNaciE1.ToString(), out fecha_Validar) || String.IsNullOrEmpty(Emprendedor._fechaNaciE1.ToString()))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo fecha de nacimiento es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._departamentoNacimientoE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo departamento de nacimiento es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._ciudadnacimientoE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo ciudad de nacimiento es requerido')", true);
					return;
				}

				if (String.IsNullOrEmpty(Emprendedor._tituloObtenidoE))
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo programa realizado es requerido')", true);
					return;
				}

				#endregion

				#region validarexpresiones

				Int64 valida = 0;
				//DateTime fechaValida;

				if (!Int64.TryParse(Emprendedor._noIdentificacion1, out valida))
				{
					{
						ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo numero de identificacion tiene que ser numero')", true);
						return;
					}
				}

				if (!string.IsNullOrEmpty(Emprendedor._emailE1))
				{
					if (!IsValidEmail(Emprendedor._emailE1))
					{
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El correo no es valido')", true);
							return;
						}
					}
				}

				if (!string.IsNullOrEmpty(Emprendedor._telefonoE1))
				{
					if (!Int64.TryParse(Emprendedor._telefonoE1, out valida))
					{
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo telefono de identificacion tiene que ser numero')", true);
							return;
						}
					}
				}

				#region Desarrollo anterior comentado.

				/*
			foreach (Control control in tbl_Convenio.Controls)
			{
				if (control is TextBox)
				{
					if (((TextBox)control).ID.Equals("tb_fechanacimiento") ||
						((TextBox)control).ID.Equals("id_fechafinalizacion") ||
						((TextBox)control).ID.Equals("tb_fechagraduacionperfil") ||
						((TextBox)control).ID.Equals("tb_fechagrado") ||
						((TextBox)control).ID.Equals("tb_fechaInicioperfil"))
					{
						if (!String.IsNullOrEmpty(((TextBox)control).Text))
						{
							if (!DateTime.TryParse(((TextBox)control).Text, out fechaValida))
							{
								{
									ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('la fecha no tiene el formato completo')", true);
									return;
								}
							}
						}
					}
				}
			}
*/
				#endregion

				//if (!DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechanacimiento")).Text, out fechaValida)  || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechaInicioperfil")).Text, out fechaValida) || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechanacimiento")).Text, out fechaValida)){
				//    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('la fecha no tiene el formato completo')", true);
				//    return;
				//}

				//if(ddl_estadoperfil.SelectedValue != "1")
				//{
				//    if (!DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechanacimiento")).Text, out fechaValida) || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("id_fechafinalizacion")).Text, out fechaValida) || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechagraduacionperfil")).Text, out fechaValida) || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechaInicioperfil")).Text, out fechaValida) || !DateTime.TryParse(((TextBox)tbl_Convenio.FindControl("tb_fechanacimiento")).Text, out fechaValida))
				//    {
				//        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('la fecha no tiene el formato completo')", true);
				//        return;
				//    }
				//}

				#endregion

				#region Desarrollo anterior comentado.

				//            String txtSQL = "select *  from contacto where email = '" + Emprendedor._emailE1 + "' AND Email != '' " + " or identificacion=" + Emprendedor._noIdentificacion1;

				//            SqlDataReader reader = ejecutaReader(txtSQL, 1);
				//            if (reader != null)
				//            {
				//                if (reader.Read())
				//                {
				//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('ya hay un usuario registrado con el mismo email o numero de identificacion')", true);
				//                    return;
				//                }
				//                else
				//                {
				//                    if (Emprendedor._condicionEspecialE.Equals("0"))
				//                    {
				//                        Emprendedor._tipoEmprendizE = "NULL";
				//                    }
				//                    else
				//                    {
				//                        Emprendedor._tipoEmprendizE = "'" + Emprendedor._tipoEmprendizE + "'";
				//                    }

				//                    String ciudad = "";
				//                    //if (string.IsNullOrEmpty(HttpContext.Current.Session["idInstitucionEvalRe"].ToString()))
				//                    //{
				//                    txtSQL = "SELECT Id_Institucion FROM Institucion WHERE NomInstitucion = '" + Emprendedor._institucionE + "' AND CodCiudad = (SELECT Id_Ciudad FROM Ciudad WHERE NomCiudad = '" + Emprendedor._ciudadIE + "')";

				//                    reader = ejecutaReader(txtSQL, 1);

				//                    establecer(ref ciudad, reader);
				//                    //}
				//                    //else
				//                    if (string.IsNullOrEmpty(ciudad))
				//                    {
				//                        ciudad = "NULL";
				//                    }

				//                    txtSQL = @"Insert into contacto(nombres,apellidos,CodTipoIdentificacion" +
				//                    ",Identificacion,Email,Clave,CodInstitucion, CodTipoAprendiz, Genero,FechaNacimiento,CodCiudad,Telefono,LugarExpedicionDI) " + @"
				//                    values ('" + Emprendedor._nombreE + "','" + Emprendedor._apellidoE + "'," + Emprendedor._TipoIdentificacionE + @"
				//                    ," + Emprendedor._noIdentificacion1 + ",'" + Emprendedor._emailE1 + "','" + "Password1" + "'," + ciudad + "," + Emprendedor._tipoEmprendizE + ", " + @"
				//                    '" + Emprendedor._generoE1 + "','" + Emprendedor._fechaNaciE1 + "','" + Emprendedor._ciudadnacimientoE + "','" + Emprendedor._telefonoE1 + "','" + Emprendedor._ciudadExpedicionE1 + "')";

				//                    ejecutaReader(txtSQL, 2);

				//                    txtSQL = "Select Id_Contacto from contacto where email = '" + Emprendedor._emailE1 + "'";
				//                    reader = ejecutaReader(txtSQL, 1);

				//                    reader.Read();

				//                    contactoid = Int64.Parse(reader[0].ToString());

				//                    txtSQL = "INSERT INTO GrupoContacto(CodContacto,CodGrupo) " + "VALUES(" + contactoid + "," + Constantes.CONST_Emprendedor + ")";
				//                    ejecutaReader(txtSQL, 2);

				//                    txtSQL = "INSERT INTO ProyectoContacto(CodContacto,CodProyecto,CodRol,FechaInicio) " + "VALUES(" + contactoid + "," + Request["CodProyecto"].ToString() + "," + Constantes.CONST_RolEmprendedor + ",getdate())";
				//                    ejecutaReader(txtSQL, 2);

				//                    insertarContactoEstudio(Emprendedor, contactoid);

				//                    String mensaje = "";

				//                    mensaje += "{{Rol}} Emprendedor, \n";
				//                    mensaje += "{{Email}} " + Emprendedor._emailE1 + ", \n";
				//                    mensaje += "{{Clave}} Password1, \n";
				//                    mensaje += "Fondo Emprender \n";
				//                    mensaje += "Registro a Fondo Emprender \n";

				//                    try
				//                    {
				//                        enviarPorEmail(Emprendedor._emailE1, "Catálogo de Emprendedor", mensaje);
				//                    }
				//                    catch (Exception err)
				//                    {
				//                        System.Windows.Forms.MessageBox.Show("" + err.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				//                    }
				//                    finally
				//                    {
				//                        limpiarCampos();
				//                        gv_emprendedor.Visible = true;

				//                        pnl_crearEditar.Visible = true;
				//                        tc_Emprendedor.Visible = false;
				//                        pnl_infoAcademica.Visible = false;

				//                        gv_emprendedor.DataBind();
				//                        string redirePagina = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
				//                        Response.Redirect(redirePagina + "?Accion=Editar&CodProyecto=" + Request.QueryString["CodProyecto"].ToString());
				//                    }
				//                }
				//            }

				#endregion

				#region Versión de Mauricio Arias Olave.

				//Consultar para verificar si ya existe un usuario con el mismo Email y/o número de identificación.

				/*
				 * EN EL METODO INICIALMENTE DEFINIDO SE PRESENTABAN ERRORES DE ACCESO AL OBJETO DE ACCESO A DATOS
				 * SE IMPLEMENTO UNA FUNCIONALIDAD CON EL MISMO COMPORTAMIENTO PERMITIENDO ACCESO A LA INSTANCIA DE RETORNO
				 */

				var ejecutaReader = new Clases.genericQueries();

				String txtSQL = string.Format("select *  from contacto where email = '{0}' AND Email != SPACE(1) or identificacion={1} AND Inactivo = 0", Emprendedor._emailE1, Emprendedor._noIdentificacion1);
				SqlDataReader reader = ejecutaReader.executeQueryReader(txtSQL);

				if (!reader.IsClosed)
				{
					if (reader.Read())
					{
						if(cont == 0)
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario registrado con el mismo email o número de identificación.')", true);
							return;
						}
					}
					else
					{
						if (Emprendedor._condicionEspecialE.Equals("0"))
							Emprendedor._tipoEmprendizE = 0;

						#region Establecer datos...

						txtSQL = "SELECT Id_Institucion FROM Institucion WHERE NomInstitucion = '" + Emprendedor._institucionE + "' AND CodCiudad = (SELECT Id_Ciudad FROM Ciudad WHERE NomCiudad = '" + Emprendedor._ciudadIE + "')";
						reader = ejecutaReader.executeQueryReader(txtSQL);
						establecer(ref ciudad, reader);

						#endregion

						if (string.IsNullOrEmpty(ciudad)) { ciudad = "0"; } //null

						#region Insertar emprendedor.

						#region COMENTADO "problemas con datetime".

						//                    txtSQL = @"Insert into contacto(nombres,apellidos,CodTipoIdentificacion" +
						//                                ",Identificacion,Email,Clave,CodInstitucion, CodTipoAprendiz, Genero,FechaNacimiento,CodCiudad,Telefono,LugarExpedicionDI) " + @"
						//                    values ('" + Emprendedor._nombreE + "','" + Emprendedor._apellidoE + "'," + Emprendedor._TipoIdentificacionE + @"
						//                    ," + Emprendedor._noIdentificacion1 + ",'" + Emprendedor._emailE1 + "','" + "Password1" + "'," + ciudad + "," + Emprendedor._tipoEmprendizE + ", " + @"
						//                    '" + Emprendedor._generoE1 + "','" + Emprendedor._fechaNaciE1 + "','" + Emprendedor._ciudadnacimientoE + "','" + Emprendedor._telefonoE1 + "','" + Emprendedor._ciudadExpedicionE1 + "')";

						//                    ejecutaReader(txtSQL, 2); 

						#endregion
						SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
						try
						{							
							SqlCommand cmd = new SqlCommand("MD_CrearEmprendedor_mod", con);

							if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Nombres", Emprendedor._nombreE);
							cmd.Parameters.AddWithValue("@Apellidos", Emprendedor._apellidoE);
							cmd.Parameters.AddWithValue("@CodTipoIdentificacion", Int32.Parse(Emprendedor._TipoIdentificacionE));
							cmd.Parameters.AddWithValue("@Identificacion", Convert.ToInt64(Emprendedor._noIdentificacion1));
							cmd.Parameters.AddWithValue("@Email", Emprendedor._emailE1);
							cmd.Parameters.AddWithValue("@Clave", "Password1");
							cmd.Parameters.AddWithValue("@CodInstitucion", usuario.CodInstitucion);
							cmd.Parameters.AddWithValue("@CodTipoAprendiz", Emprendedor._tipoEmprendizE);
							cmd.Parameters.AddWithValue("@Genero", Emprendedor._generoE1);
							cmd.Parameters.AddWithValue("@FechaNacimiento", Emprendedor._fechaNaciE1);
							cmd.Parameters.AddWithValue("@CodCiudad", Emprendedor._ciudadnacimientoE);
							cmd.Parameters.AddWithValue("@Telefono", Emprendedor._telefonoE1);
							cmd.Parameters.AddWithValue("@LugarExpedicionDI", Emprendedor._ciudadExpedicionE1);
							cont = (int)cmd.ExecuteScalar();
							//con.Close();
							//con.Dispose();
							cmd.Dispose();


						}
						catch
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Avis: Ocurrio un error al guardar los datos..!')", true);
							return;
						}
						finally {
							con.Close();
							con.Dispose();
						}
						#endregion

						#region Consultar el ID del emprendedor recién ingresado.

						txtSQL = "Select Id_Contacto from contacto where email = '" + Emprendedor._emailE1 + "'";
						var dt = consultas.ObtenerDataTable(txtSQL, "text");
						if (dt.Rows.Count > 0) { contactoid = Int64.Parse(dt.Rows[0]["Id_Contacto"].ToString()); }
						else
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo ingresar el emprendedor.')", true);
							return;
						}


						#endregion

						txtSQL = "INSERT INTO GrupoContacto(CodContacto,CodGrupo) " + "VALUES(" + contactoid + "," + Constantes.CONST_Emprendedor + ")";
						ejecutaReader.executeQueryReader(txtSQL, 1);

						txtSQL = "INSERT INTO ProyectoContacto(CodContacto,CodProyecto,CodRol,FechaInicio) " + "VALUES(" + contactoid + "," + Request["CodProyecto"].ToString() + "," + Constantes.CONST_RolEmprendedor + ",getdate())";
						ejecutaReader.executeQueryReader(txtSQL, 1);

						insertarContactoEstudio(Emprendedor, contactoid);

						String mensaje = "";

						mensaje += "{{Rol}} <b><i>Emprendedor<b><i> \n";
						mensaje += "{{Email}} <b><i>" + Emprendedor._emailE1 + "<b><i> \n";
						mensaje += "{{Clave}} <b><i>Password1<b><i> \n";
						mensaje += "Fondo Emprender \n";
						mensaje += "Registro a Fondo Emprender \n";

						try
						{
							enviarPorEmail(Emprendedor._emailE1, "Catálogo de Emprendedor", mensaje);
						}
						catch (Exception err)
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en el envío del mensaje de notificación')", true);
						}
						finally
						{
							limpiarCampos();
							gv_emprendedor.Visible = true;

							pnl_crearEditar.Visible = true;
							tc_Emprendedor.Visible = false;
							pnl_infoAcademica.Visible = false;

							gv_emprendedor.DataBind();
							//redirePagina = string.Format("{0}://{1}{2}?Accion=Editar&CodProyecto={3}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath, Request.QueryString["CodProyecto"]);
							//redirePagina = Request.Url.PathAndQuery.Insert(0, "~");
						}
					}
				}
				//try { Response.Redirect(redirePagina); }
				//finally { Server.TransferRequest(redirePagina, true); }
				#endregion
				ClearControlsForm();
				//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "location.reload();", true);
				Response.Redirect("CatalogoProyecto.aspx?Accion=" + Request.QueryString["Accion"] + "&CodProyecto=" + Request.QueryString["CodProyecto"]);

			}
			else
			{
				cont = 1;
			}
		}

		protected void ddl_depto1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddl_depto1_Selected();
		}

		private void ddl_depto1_Selected()
		{
			if (ddl_depto1.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_depto1.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });
				ddl_ciudad1.DataSource = municipios;
				ddl_ciudad1.DataTextField = "Ciudad";
				ddl_ciudad1.DataValueField = "ID_Ciudad";
				ddl_ciudad1.DataBind();
			}
		}

		protected void ddl_depto2_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddl_depto2_Selected();
		}

		private void ddl_depto2_Selected()
		{
			if (ddl_depto2.SelectedValue != "")
			{
				var subsectores = (from s in consultas.Db.SubSector
								   where s.CodSector == Int32.Parse(ddl_depto2.SelectedValue)
								   orderby s.NomSubSector
								   select new
								   {
									   Sumsector = s.NomSubSector,
									   Id_subsector = s.Id_SubSector
								   });
				ddl_ciudad2.DataSource = subsectores;
				ddl_ciudad2.DataTextField = "Sumsector";
				ddl_ciudad2.DataValueField = "Id_subsector";
				ddl_ciudad2.DataBind();
			}
		}

		protected void ddl_deptoexpedicionperfil_SelectedIndexChanged(object sender, EventArgs e)
		{
			ddl_deptoexpedicionperfil_Selected();
		}

		private void ddl_deptoexpedicionperfil_Selected()
		{
			if (ddl_deptoexpedicionperfil.SelectedValue != "")
			{
				var municipios = (from c in consultas.Db.Ciudad
								  where c.CodDepartamento == Int32.Parse(ddl_deptoexpedicionperfil.SelectedValue)
								  orderby c.NomCiudad ascending
								  select new
								  {
									  Ciudad = c.NomCiudad,
									  ID_Ciudad = c.Id_Ciudad
								  });

				ddl_ciudadexpedicionperfil.DataSource = municipios;
				ddl_ciudadexpedicionperfil.DataTextField = "Ciudad";
				ddl_ciudadexpedicionperfil.DataValueField = "ID_Ciudad";
				ddl_ciudadexpedicionperfil.DataBind();
				ddl_ciudadexpedicionperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
			}
		}

		/// <summary>
		/// Selección del departamento y carga de ciudades correspondientes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddl_deptonacimientoperfil_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddl_deptonacimientoperfil.SelectedValue != "")
			{
				try
				{
					var dt = consultas.ObtenerDataTable("SELECT * FROM Ciudad WHERE CodDepartamento = " + ddl_deptonacimientoperfil.SelectedValue, "text");

					ddl_ciudadonacimientoperfil.Items.Clear();

					foreach (DataRow row in dt.Rows)
					{
						ListItem item = new ListItem();
						item.Text = row["NomCiudad"].ToString();
						item.Value = row["Id_Ciudad"].ToString();
						ddl_ciudadonacimientoperfil.Items.Add(item);
					}
					ddl_ciudadonacimientoperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
				}
				catch { }
			}
		}

		protected void btnCrearPrograma_Click(object sender, EventArgs e)
		{
			string id_institucion;
			string NombrePrograma;
			string id_ciudadPrograma;
			string txtSQL;

			id_institucion = SelInstitucion.SelectedValue;
			NombrePrograma = txtNomPrograma.Text;
			id_ciudadPrograma = SelDptoExpedicion.SelectedValue;

			if (string.IsNullOrEmpty(id_institucion))
			{
				txtSQL = "UPDATE PARAMETRO SET VALOR= (CAST(VALOR AS INT) +1) WHERE NOMPARAMETRO = 'SecuenciaNuevaInstitucion' ";
				ejecutaReader(txtSQL, 2);
				txtSQL = "SELECT VALOR FROM PARAMETRO WHERE NOMPARAMETRO = 'SecuenciaNuevaInstitucion'";
				var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

				if (reader.Rows.Count > 0)
				{
					txtSQL = "INSERT INTO INSTITUCIONEDUCATIVA VALUES(" + reader.Rows[0].ItemArray[0].ToString() + ",'" + NombrePrograma + "')";
					ejecutaReader(txtSQL, 2);

					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente')", true);
					Panel2.Visible = false;
				}
			}
			else
			{
				txtSQL = "UPDATE PARAMETRO SET VALOR= (CAST(VALOR AS INT) +1) WHERE NOMPARAMETRO = 'SecuenciaNuevoProgramaAcademico' ";
				ejecutaReader(txtSQL, 2);
				txtSQL = "SELECT VALOR FROM PARAMETRO WHERE NOMPARAMETRO = 'SecuenciaNuevoProgramaAcademico'";

				var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

				if (reader.Rows.Count > 0)
				{
					txtSQL = "INSERT INTO PROGRAMAACADEMICO VALUES(" + reader.Rows[0].ItemArray[0].ToString() + ",'" + NombrePrograma + "','N/A'," + id_institucion + ",'ACTIVO','N/A','','',1," + id_ciudadPrograma + ")";
					ejecutaReader(txtSQL, 2);

					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente')", true);
					Panel2.Visible = false;
				}
			}
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Cargar la GRILLA de instituciones y/o programas académicos (según el parámetro txtTipoConsulta) 
		/// y así seleccionar dicha información.
		/// </summary>
		/// <param name="txtTipoConsulta">Tipo de consulta: DEBE SER "programa" o "institucion".</param>
		/// <param name="txtletra">parámetro.</param>
		private void CargarInstituciones(string txtTipoConsulta, string txtletra)
		{
			//Inicializar variables.
			String txtSQL = "";
			int numero = 0;
			DataTable rsConsulta = new DataTable();
			String CodNivelEstudio;
			String txtCondicion = "";
			String txtBuscar = txtBusqueda.Text.Trim();
			String txtciudad = SelCiudad.SelectedValue;
			String txtOrdenamiento = " ORDER BY 3";
			DropDownList dd_codnivel = new DropDownList();

			try
			{
				//Revisar si se construye la consulta para el PROGRAMA o la INSTITUCIÓN.
				switch (txtTipoConsulta)
				{
					case "programa":
						#region Establecer consulta para PROGRAMA.

						txtSQL = "SELECT PA.ID_PROGRAMAACADEMICO, PA.NOMPROGRAMAACADEMICO, IE.NOMINSTITUCIONEDUCATIVA, C.NOMCIUDAD FROM PROGRAMAACADEMICO PA JOIN INSTITUCIONEDUCATIVA IE ON (IE.ID_INSTITUCIONEDUCATIVA = PA.CODINSTITUCIONEDUCATIVA) JOIN CIUDAD C ON (C.ID_CIUDAD= PA.CODCIUDAD)";

						//Nivel de estudio.
						dd_codnivel = tc_Emprendedor.Tabs[0].FindControl("ddl_nivelestudioperfil") as DropDownList;
						CodNivelEstudio = dd_codnivel.SelectedValue;

						//Agregar el nivel de estudio para la consulta.
						if (Int32.TryParse(txtletra, out numero))
						{ txtCondicion = " WHERE PA.CODNIVELESTUDIO = " + CodNivelEstudio; }

						if (txtletra != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							txtCondicion = txtCondicion + " PA.NOMPROGRAMAACADEMICO LIKE '" + txtletra + "%'";
						}

						if (txtBuscar != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							if (txtBuscar.Contains("'")) { txtBuscar = txtBuscar.Replace("'", ""); }

							txtCondicion = txtCondicion + " PA.NOMPROGRAMAACADEMICO LIKE '%" + txtBuscar + "%'";
						}

						if (txtciudad != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							if (txtciudad.Contains("'")) { txtciudad = txtciudad.Replace("'", ""); }

							txtCondicion = txtCondicion + " c.Id_ciudad = " + txtciudad + " ";
						}

						txtSQL = (txtSQL + txtCondicion).Insert((txtSQL.Length + txtCondicion.Length), " ORDER BY NOMCIUDAD ASC ");// + txtOrdenamiento;
						//txtNomCampoPoseeCodigo = "ID_PROGRAMAACADEMICO"
						//txtNomCampoPoseeDescripcion = "NOMPROGRAMAACADEMICO" 

						#endregion
						break;
					case "institucion":
						#region Establecer consulta para INSTITUCIÓN.

						txtOrdenamiento = " ORDER BY 3";

						txtSQL = "SELECT PA.ID_PROGRAMAACADEMICO, PA.NOMPROGRAMAACADEMICO, IE.NOMINSTITUCIONEDUCATIVA, C.NOMCIUDAD FROM PROGRAMAACADEMICO PA JOIN INSTITUCIONEDUCATIVA IE ON (IE.ID_INSTITUCIONEDUCATIVA = PA.CODINSTITUCIONEDUCATIVA) JOIN CIUDAD C ON (C.ID_CIUDAD= PA.CODCIUDAD) ";

						//Nivel de estudio.
						dd_codnivel = tc_Emprendedor.Tabs[0].FindControl("ddl_nivelestudioperfil") as DropDownList;
						CodNivelEstudio = dd_codnivel.SelectedValue;

						//Agregar el nivel de estudio para la consulta.
						if (Int32.TryParse(txtletra, out numero))
						{ txtCondicion = " WHERE PA.CODNIVELESTUDIO = " + CodNivelEstudio; }

						if (txtletra != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							txtCondicion = txtCondicion + " PA.NOMPROGRAMAACADEMICO LIKE '" + txtletra + "%'";
						}

						if (txtBuscar != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							if (txtBuscar.Contains("'")) { txtBuscar = txtBuscar.Replace("'", ""); }

							txtCondicion = txtCondicion + " PA.NOMPROGRAMAACADEMICO LIKE '%" + txtBuscar + "%'";
						}

						if (txtciudad != "")
						{
							if (txtCondicion.Length > 0)
								txtCondicion = txtCondicion + " AND ";
							else
								txtCondicion = " WHERE";

							if (txtciudad.Contains("'")) { txtciudad = txtciudad.Replace("'", ""); }

							txtCondicion = txtCondicion + " c.Id_ciudad = " + txtciudad + " ";
						}

						txtSQL = txtSQL + txtCondicion;// + txtOrdenamiento;
						//txtNomCampoPoseeCodigo = "ID_PROGRAMAACADEMICO"
						//txtNomCampoPoseeDescripcion = "NOMPROGRAMAACADEMICO"

						#endregion
						break;
					default:
						break;
				}

				rsConsulta = consultas.ObtenerDataTable(txtSQL, "text");
				HttpContext.Current.Session["ProgramasAcademicos"] = rsConsulta;

				gv_institucion.DataSource = rsConsulta;
				gv_institucion.DataBind();
			}
			catch { }
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Ocultar el panel de creación de programa académico.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_Ocultar_Click(object sender, EventArgs e)
		{
			txtOtraInstitucion.Text = "";
			txtNomPrograma.Text = "";
			Panel2.Visible = false;
			pnl_Convenios.Visible = true;
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// RowCommand de la grilla de instituciones para seleccionar
		/// el programa académico del emprendedor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gv_institucion_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "seleccionar_1":
				case "seleccionar_2":
				case "seleccionar_3":

					try
					{
						//Dividir los valores.
						string[] id_and_name = e.CommandArgument.ToString().Split(';');

						//Colocar el nombre de la institucion y los datos relacionados en la otra pestaña.

						//ID del programa seleccionado, para así contuinuar con el flujo.
						HttpContext.Current.Session["CodProgramaAcademico"] = id_and_name[0];

						//Encuentra y asigna los valores a los campos de la otra pestaña.
						TextBox txt_prog = tc_Emprendedor.Tabs[0].FindControl("tb_Programarealizadoperfil") as TextBox;
						TextBox txt_inst = tc_Emprendedor.Tabs[0].FindControl("tb_Institucionperfil") as TextBox;
						TextBox txt_ciud = tc_Emprendedor.Tabs[0].FindControl("tb_ciudadinstitucionperfil") as TextBox;

						if (txt_prog != null)
							txt_prog.Text = id_and_name[1]; //txtNomPrograma.Text;
						if (txt_inst != null)
							txt_inst.Text = id_and_name[3];//SelInstitucion.SelectedItem.Text;
						if (txt_ciud != null)
							txt_ciud.Text = id_and_name[2];//SelCiudad.SelectedItem.Text;

						//Mostrar la otra pestaña con los valores seleccionados.
						tc_Emprendedor.ActiveTabIndex = 0; //1
					}
					catch { }

					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Crear programa académico.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_CrearPrograma_Click(object sender, EventArgs e)
		{
			//Inicializar variables.
			SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
			SqlCommand cmd = new SqlCommand();
			String str = "";
			String txtSQL = "";
			Int32 CodProgramaAcademico = 1;
			String CodInstitucion = "1";
			String CodNivelEstudio = "1";
			String txtCiudadInstitucion = "111";
			bool savedData = false;

			try
			{
				//Validar si se puede crear el nuevo programa.
				str = validarNuevoPrograma();

				if (str == "")
				{
					//Consultar Id_ProgramaAcademico.
					txtSQL = "SELECT TOP 1 Id_ProgramaAcademico FROM ProgramaAcademico ORDER BY Id_ProgramaAcademico DESC";

					var dt = consultas.ObtenerDataTable(txtSQL, "text");
					if (dt.Rows.Count > 0)
					{ CodProgramaAcademico = Int32.Parse(dt.Rows[0]["Id_ProgramaAcademico"].ToString()) + 1; }
					dt = null;

					//Código de la institución.
					CodInstitucion = SelInstitucion.SelectedValue;

					//Nivel de estudio.
					DropDownList dd_codnivel = tc_Emprendedor.Tabs[0].FindControl("ddl_nivelestudioperfil") as DropDownList;
					CodNivelEstudio = dd_codnivel.SelectedValue;

					//Ciudad.
					//txtCiudadInstitucion = SelCiudad.SelectedValue;
					txtCiudadInstitucion = SelMunExpedicion.SelectedValue;

					//Crear el programa académico.
					txtSQL = "INSERT INTO PROGRAMAACADEMICO VALUES(" + CodProgramaAcademico + ",'" + txtNomPrograma.Text.Trim() + "','N/A'," + CodInstitucion + ",'ACTIVO','N/A','',''," + CodNivelEstudio + "," + txtCiudadInstitucion + ")";

					//Ejecutar consulta
					cmd = new SqlCommand(txtSQL, conn);
					savedData = EjecutarSQL(conn, cmd);

					if (savedData)
					{
						//Este es el ID que se CREÓ, se usará para contuinuar con el flujo.
						HttpContext.Current.Session["CodProgramaAcademico"] = CodProgramaAcademico;

						try
						{
							//Encuentra y asigna los valores a los campos de la otra pestaña.
							TextBox txt_prog = tc_Emprendedor.Tabs[0].FindControl("tb_Programarealizadoperfil") as TextBox;
							TextBox txt_inst = tc_Emprendedor.Tabs[0].FindControl("tb_Institucionperfil") as TextBox;
							TextBox txt_ciud = tc_Emprendedor.Tabs[0].FindControl("tb_ciudadinstitucionperfil") as TextBox;

							txt_prog.Text = txtNomPrograma.Text;
							txt_inst.Text = SelInstitucion.SelectedItem.Text;
							txt_ciud.Text = SelMunExpedicion.SelectedItem.Text;
						}
						catch { }

						//Mostrar la otra pestaña con los valores seleccionados.
						tc_Emprendedor.ActiveTabIndex = 0;
					}
					else
					{
						ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el programa académico.')", true);
						return;
					}
				}
				else
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + str + "')", true);
					return;
				}
			}
			catch
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el programa académico.')", true);
				return;
			}
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Validar cuando se cree el nuevo programa académico.
		/// </summary>
		/// <returns>string vacío = puede continaur // con datos = error.</returns>
		private string validarNuevoPrograma()
		{
			//Inicializar variable.
			String str = "";

			try
			{
				if (SelInstitucion.SelectedValue == "" || SelInstitucion.SelectedValue == null) { str = Texto("TXT_INSTITUCION"); }
				if (txtNomPrograma.Text.Trim() == "") { str = Texto("TXT_PROGRAMA_REQ"); }
				if (SelMunExpedicion.SelectedValue == "" || SelMunExpedicion.SelectedValue == null) { str = Texto("TXT_CIUDADINSTITUCION_REQ"); }

				return str;
			}
			catch { return "Error"; }
		}

		#region Métodos de selección del abecedario.

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "%" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_todos_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "%";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "A" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_A_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "A";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "B" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_B_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "B";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "C" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_C_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "C";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "D" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_D_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "D";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "E" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_E_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "E";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "F" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_F_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "F";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "G" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_G_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "G";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "H" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_H_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "H";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "I" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_I_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "I";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "J" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_J_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "J";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "K" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_K_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "K";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "L" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_L_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "L";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "M" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_M_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "M";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "N" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_N_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "N";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "O" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_O_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "O";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "P" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_P_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "P";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "Q" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_Q_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "Q";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "R" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_R_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "R";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "S" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_S_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "S";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "T" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_T_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "T";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "U" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_U_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "U";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "V" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_V_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "V";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "W" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_W_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "W";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "X" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_X_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "X";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "Y" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_Y_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "Y";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// Al seleccionar este LinkButton, se establecerá el valor "Z" a
		/// la variable de sesión "upper_letter_selected".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkbtn_opcion_Z_Click(object sender, EventArgs e)
		{
			HttpContext.Current.Session["upper_letter_selected"] = "Z";
			CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), HttpContext.Current.Session["upper_letter_selected"].ToString());
		}

		#endregion

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Buscar programa académico, al llamar al método y de acuerdo con lo digitado
		/// en la caja de texto.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_Buscar_Programa_Click(object sender, EventArgs e)
		{
			if (txtBusqueda.Text.Trim() != "")
			{ CargarInstituciones(HttpContext.Current.Session["txtTipoConsulta"].ToString(), txtBusqueda.Text.Trim()); }
			else
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe Ingresar un criterio de búsqueda.')", true);
				return;
			}
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 09/09/2014.
		/// Paginación de la grilla de programas académicos.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gv_institucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			var dt = HttpContext.Current.Session["ProgramasAcademicos"] as DataTable;

			if (dt != null)
			{
				gv_institucion.PageIndex = e.NewPageIndex;
				gv_institucion.DataSource = dt;
				gv_institucion.DataBind();
			}
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Obtener el nombre de la ciudad "tal como se hace en FONADE clásico".
		/// </summary>
		/// <param name="pCodCiudad">Código de la ciudad.</param>
		/// <returns>string</returns>
		private string getNomCiudad(string pCodCiudad)
		{
			//Inicializar variables.
			string lSentencia;
			DataTable rs = new DataTable();

			try
			{
				lSentencia = "SELECT NOMCIUDAD FROM CIUDAD WHERE ID_CIUDAD=" + pCodCiudad;
				rs = consultas.ObtenerDataTable(lSentencia, "text");

				if (rs.Rows.Count > 0) { return rs.Rows[0]["NOMCIUDAD"].ToString(); } else { return ""; }
			}
			catch { return ""; }
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Obtener el nombre de la institución "tal como se hace en FONADE clásico".
		/// </summary>
		/// <param name="pCodInstitucion">Código de la institución.</param>
		/// <returns>string</returns>
		private string getNomInstitucion(string pCodInstitucion)
		{
			//Inicializar variables.
			string lSentencia;
			DataTable rs = new DataTable();

			try
			{
				lSentencia = "SELECT NOMINSTITUCIONEDUCATIVA FROM INSTITUCIONEDUCATIVA WHERE ID_INSTITUCIONEDUCATIVA=" + pCodInstitucion;
				rs = consultas.ObtenerDataTable(lSentencia, "text");

				if (rs.Rows.Count > 0) { return rs.Rows[0]["NOMINSTITUCIONEDUCATIVA"].ToString(); } else { return ""; }
			}
			catch { return ""; }
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Cargar las ciudades "lista desplegable para la búsqueda y selección de instituciones.
		/// </summary>
		private void CargarCiudades_BusquedaInstituciones()
		{
			//Inicializar variables.
			String txtSQL;

			try
			{
				txtSQL = "SELECT id_Ciudad, NomCiudad From Ciudad ORDER BY NomCiudad";
				var dt = consultas.ObtenerDataTable(txtSQL, "text");

				//Limpiar ítems.
				SelCiudad.Items.Clear();

				//Agergado el valor por defecto  la lista de ciudades.
				ListItem itemDefault = new ListItem();
				itemDefault.Value = "";
				itemDefault.Text = "Seleccione Ciudad";
				SelCiudad.Items.Add(itemDefault);

				foreach (DataRow row in dt.Rows)
				{
					ListItem item = new ListItem();
					item.Value = row["id_Ciudad"].ToString();
					item.Text = row["NomCiudad"].ToString();
					SelCiudad.Items.Add(item);
				}
			}
			catch { }
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Mostrar la siguiente pestaña para adicionar la institución (por PROGRAMA).
		/// </summary>
		protected void imbtn_institucion_Click(object sender, ImageClickEventArgs e)
		{
			HttpContext.Current.Session["txtTipoConsulta"] = "programa";
			tc_Emprendedor.ActiveTabIndex = 1;
			CargarCiudades_BusquedaInstituciones();
			CargarInstituciones("programa", "");
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Mostrar la siguiente pestaña para adicionar la institución (por INSTITUCIÓN).
		/// </summary>
		protected void imbtn_nivel_Click(object sender, ImageClickEventArgs e)
		{
			HttpContext.Current.Session["txtTipoConsulta"] = "institucion";
			tc_Emprendedor.ActiveTabIndex = 1;
			CargarCiudades_BusquedaInstituciones();
			CargarInstituciones("institucion", "");
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Crear programa académico (para ello, se muestran los campos requeridos).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkBtn_CrearPrograma_Click(object sender, EventArgs e)
		{
			pnl_Convenios.Visible = false;
			Panel2.Visible = true;
			String txtSQL = "";

			#region Cargar las instituciones para poder crear el programa académico.

			try
			{
				//Variables internas.
				DropDownList dd_codnivel = new DropDownList();
				String CodNivel = "";
				dd_codnivel = tc_Emprendedor.Tabs[0].FindControl("ddl_nivelestudioperfil") as DropDownList;
				CodNivel = dd_codnivel.SelectedValue;
				DataTable rsNivelEducativo = new DataTable();

				txtSQL = "SELECT DISTINCT IE.id_InstitucionEducativa, IE.NomInstitucionEducativa, (SELECT TOP 1 NOMMUNICIPIO + ' (' + NOMDEPARTAMENTO+')'  FROM PROGRAMAACADEMICO WHERE CODINSTITUCIONEDUCATIVA=IE.ID_INSTITUCIONEDUCATIVA)'NOMMUNICIPIO' From InstitucionEducativa IE JOIN PROGRAMAACADEMICO PA ON (PA.CODINSTITUCIONEDUCATIVA = IE.ID_INSTITUCIONEDUCATIVA) WHERE PA.CODNIVELESTUDIO = " + CodNivel + " AND PA.NOMMUNICIPIO <>'' ORDER BY IE.NomInstitucionEducativa";
				rsNivelEducativo = consultas.ObtenerDataTable(txtSQL, "text");

				SelInstitucion.Items.Clear();

				foreach (DataRow row in rsNivelEducativo.Rows)
				{
					ListItem item = new ListItem();
					item.Value = row["id_InstitucionEducativa"].ToString();
					item.Text = row["NomInstitucionEducativa"].ToString() + "  -  " + row["NOMMUNICIPIO"].ToString();
					SelInstitucion.Items.Add(item);
				}

				//Destruir variables.
				txtSQL = null;
				dd_codnivel = null;
				CodNivel = null;
				rsNivelEducativo = null;
			}
			catch { }

			#endregion

			#region Cargar Ciudades y departamentos.

			#region Ciudad.

			txtSQL = "SELECT id_Ciudad, NomCiudad From Ciudad ORDER BY NomCiudad";
			var dt = consultas.ObtenerDataTable(txtSQL, "text");

			//Limpiar ítems.
			SelMunExpedicion.Items.Clear();

			//Agergado el valor por defecto  la lista de ciudades.
			ListItem itemDefault = new ListItem();
			itemDefault.Value = "";
			itemDefault.Text = "Seleccione Ciudad";
			SelMunExpedicion.Items.Add(itemDefault);

			foreach (DataRow row in dt.Rows)
			{
				ListItem item = new ListItem();
				item.Value = row["id_Ciudad"].ToString();
				item.Text = row["NomCiudad"].ToString();
				SelMunExpedicion.Items.Add(item);
			}

			#endregion

			#region Departamento.

			txtSQL = "SELECT * From Departamento ORDER BY NomDepartamento";
			dt = consultas.ObtenerDataTable(txtSQL, "text");

			//Limpiar ítems.
			SelDptoExpedicion.Items.Clear();

			//Agergado el valor por defecto  la lista de ciudades.
			itemDefault = new ListItem();
			itemDefault.Value = "";
			itemDefault.Text = "Seleccione Departamento";
			SelDptoExpedicion.Items.Add(itemDefault);

			foreach (DataRow row in dt.Rows)
			{
				ListItem item = new ListItem();
				item.Value = row["id_Departamento"].ToString();
				item.Text = row["NomDepartamento"].ToString();
				SelDptoExpedicion.Items.Add(item);
			}

			#endregion

			#endregion

			txtOtraInstitucion.Text = "";
			txtNomPrograma.Text = "";
		}

		/// <summary>
		/// Mostrar la página actual y el total de páginas.
		/// http://stackoverflow.com/questions/21325878/row-command-event-of-gridview-is-not-working-in-ajax-tab-container
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gv_institucion_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			#region Ver recurso.

			//if (e.Row.RowType == DataControlRowType.DataRow)
			//{
			//    LinkButton lnk_1 = e.Row.FindControl("lnk_nom_programa_ac") as LinkButton;
			//    LinkButton lnk_2 = e.Row.FindControl("lnk_inst_educativa") as LinkButton;
			//    LinkButton lnk_3 = e.Row.FindControl("lnk_inst_educativa") as LinkButton;

			//    if (lnk_1 != null)
			//        ToolkitScriptManager1.RegisterPostBackControl(lnk_1);
			//    if (lnk_2 != null)
			//        ToolkitScriptManager1.RegisterPostBackControl(lnk_2);
			//    if (lnk_3 != null)
			//        ToolkitScriptManager1.RegisterPostBackControl(lnk_3);
			//}

			#endregion

			// Calculate the current page number.
			int currentPage = gv_institucion.PageIndex + 1;

			// Update the Label control with the current page information.
			lbl_Pagina.Text = "Página " + currentPage.ToString() + " de " + gv_institucion.PageCount.ToString();
		}

		/// <summary>
		/// Mauricio Arias Olave.
		/// 10/09/2014.
		/// Mostrar la primera pestaña.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_volver_Click(object sender, EventArgs e)
		{ tc_Emprendedor.ActiveTabIndex = 0; }

		/// <summary>
		/// Editar y/o borrar Emprendedor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gv_emprendedor_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			//Inicializar variables.
			String CodContacto = "";
			String txtSQL = "";
			String CodProyecto = "";
			String mCodNivelEstudio = "";
			String mTituloObtenido = "";
			String mInstitucion = "";
			String mCodCiudadInstitucion = "";
			String mCodProgramaRealizado = "";
			String mEstudioFinalizado = "";
			String mFechaGrado = "";
			String mFechaUltimoCorte = "";
			String mSemestresCursados = "";
			String mNomCiudadInstitucion = "";
			String mFechaInicio = "";
			String mFechaFinMateria = "";
			string[] parametros;
			
			switch (e.CommandName)
			{
				case "Editar":
					//Editar datos emprendedor
					//Dividir los valores.
					parametros = e.CommandArgument.ToString().Split(';');

					//Asignar valores a las variables.
					CodContacto = parametros[1];
					CodProyecto = parametros[0];

					#region Administrar campos.

					//limpiarCampos();
					gv_emprendedor.Visible = true;
					pnl_infoAcademica.Visible = true;

					pnl_crearEditar.Visible = false;
					tc_Emprendedor.Visible = true;
					pnl_infoAcademica.Visible = true;

					#endregion

					#region Cargar la información del emprendedor seleccionado.

					txtSQL = " SELECT nombres,apellidos,codtipoidentificacion,identificacion, cargo, email, telefono, fax, codgrupo, CodTipoAprendiz,Genero,FechaNacimiento,CodCiudad,Telefono,LugarExpedicionDI " +
							 " FROM Contacto, GrupoContacto WHERE ID_Contacto=codcontacto and Id_Contacto=" + CodContacto;

					var RsUsuario = consultas.ObtenerDataTable(txtSQL, "text");

					if (RsUsuario.Rows.Count > 0)
					{
						tb_apellidoperfil.Text = RsUsuario.Rows[0]["apellidos"].ToString();
						tb_correoperfil.Text = RsUsuario.Rows[0]["email"].ToString();
						tb_nodocperfil.Text = RsUsuario.Rows[0]["identificacion"].ToString();
						tb_fechanacimiento.Text = DateTime.Parse(RsUsuario.Rows[0]["FechaNacimiento"].ToString()).ToString("dd/MM/yyyy");
						tb_nombreperfil.Text = RsUsuario.Rows[0]["nombres"].ToString();
						tb_telefonoperfil.Text = RsUsuario.Rows[0]["telefono"].ToString();
						String lCodTipoAprendiz = RsUsuario.Rows[0]["CodTipoAprendiz"].ToString();

						//Cargar mas datos.
						ddl_Tipodocumentoperfil.SelectedValue = RsUsuario.Rows[0]["codtipoidentificacion"].ToString();
						ddl_generoperfil.SelectedValue = RsUsuario.Rows[0]["Genero"].ToString();

						#region Ciudad y Departamento de expedición.

						#region Establecer ciudad.

						var rsCiudades = consultas.ObtenerDataTable("SELECT * FROM Ciudad WHERE Id_Ciudad = '" + RsUsuario.Rows[0]["LugarExpedicionDI"].ToString() + "' ORDER BY NomCiudad", "text");
						ddl_ciudadexpedicionperfil.Items.Clear();

						foreach (DataRow row in rsCiudades.Rows)
						{
							ListItem item = new ListItem();
							item.Value = row["Id_Ciudad"].ToString();
							item.Text = row["NomCiudad"].ToString();
							if (RsUsuario.Rows[0]["CodCiudad"].ToString() == item.Value) { item.Selected = true; }
							ddl_ciudadexpedicionperfil.Items.Add(item);
						}
						ddl_ciudadexpedicionperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

						#endregion

						#region Establecer departamentos.

						if (rsCiudades.Rows.Count > 0)
						{
							var rsDeptos = consultas.ObtenerDataTable("SELECT * FROM Departamento ORDER BY NomDepartamento", "text");
							ddl_deptoexpedicionperfil.Items.Clear();

							foreach (DataRow row in rsDeptos.Rows)
							{
								ListItem item = new ListItem();
								item.Value = row["Id_Departamento"].ToString();
								item.Text = row["NomDepartamento"].ToString();
								if (rsCiudades.Rows[0]["CodDepartamento"].ToString() == item.Value) { item.Selected = true; }
								ddl_deptoexpedicionperfil.Items.Add(item);
							}
							// Codigo RJ
							ddl_deptoexpedicionperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
							ddl_ciudadexpedicionperfil.DataSource = null;
							ddl_ciudadexpedicionperfil.DataBind();
						}

						#endregion

						#endregion

						#region Ciudad y Departamento de nacimiento.

						#region Establecer ciudad.

						rsCiudades = new DataTable();
						rsCiudades = consultas.ObtenerDataTable("SELECT * FROM Ciudad WHERE Id_Ciudad = " + RsUsuario.Rows[0]["CodCiudad"].ToString() + " ORDER BY NomCiudad", "text");
						ddl_ciudadonacimientoperfil.Items.Clear();

						foreach (DataRow row in rsCiudades.Rows)
						{
							ListItem item = new ListItem();
							item.Value = row["Id_Ciudad"].ToString();
							item.Text = row["NomCiudad"].ToString();
							if (RsUsuario.Rows[0]["CodCiudad"].ToString() == item.Value) { item.Selected = true; }
							ddl_ciudadonacimientoperfil.Items.Add(item);
						}

						#endregion

						#region Establecer departamentos.

						if (rsCiudades.Rows.Count > 0)
						{
							var rsDeptos = consultas.ObtenerDataTable("SELECT * FROM Departamento ORDER BY NomDepartamento", "text");
							ddl_deptonacimientoperfil.Items.Clear();

							foreach (DataRow row in rsDeptos.Rows)
							{
								ListItem item = new ListItem();
								item.Value = row["Id_Departamento"].ToString();
								item.Text = row["NomDepartamento"].ToString();
								if (rsCiudades.Rows[0]["CodDepartamento"].ToString() == item.Value) { item.Selected = true; }
								ddl_deptonacimientoperfil.Items.Add(item);
							}
							ddl_deptonacimientoperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
						}

						#endregion

						#endregion

						//Se incluye la información del Nivel de estudio
						txtSQL = "SELECT CE.TITULOOBTENIDO,CE.INSTITUCION,CE.CODCIUDAD,CE.CODNIVELESTUDIO,CE.CODPROGRAMAACADEMICO,CE.FINALIZADO,CE.FECHAINICIO,CE.FECHAFINMATERIAS,CE.FECHAGRADO,CE.FECHAULTIMOCORTE,CE.SEMESTRESCURSADOS,C.NOMCIUDAD,PA.NOMPROGRAMAACADEMICO FROM CONTACTOESTUDIO CE JOIN CIUDAD C ON (C.ID_CIUDAD = CE.CODCIUDAD) JOIN PROGRAMAACADEMICO PA ON (PA.ID_PROGRAMAACADEMICO = CE.CODPROGRAMAACADEMICO) WHERE CODCONTACTO =" + CodContacto + " AND FLAGINGRESADOASESOR =1";

						var rsContactoEstudio = consultas.ObtenerDataTable(txtSQL, "text");

						if (rsContactoEstudio.Rows.Count > 0)
						{
							mCodNivelEstudio = rsContactoEstudio.Rows[0]["CODNIVELESTUDIO"].ToString();
							mTituloObtenido = rsContactoEstudio.Rows[0]["TITULOOBTENIDO"].ToString();
							mInstitucion = rsContactoEstudio.Rows[0]["INSTITUCION"].ToString();
							mCodCiudadInstitucion = rsContactoEstudio.Rows[0]["CODCIUDAD"].ToString();
							mCodProgramaRealizado = rsContactoEstudio.Rows[0]["CODPROGRAMAACADEMICO"].ToString();
							mEstudioFinalizado = rsContactoEstudio.Rows[0]["FINALIZADO"].ToString();
							var fechaGrado = Convert.ToDateTime(rsContactoEstudio.Rows[0]["FECHAGRADO"].ToString()).Date;
							if(fechaGrado.Year != 1900)
							{
								mFechaGrado = rsContactoEstudio.Rows[0]["FECHAGRADO"].ToString();
							}
							else
							{
								mFechaGrado = "";
							}
							mFechaUltimoCorte = rsContactoEstudio.Rows[0]["FECHAULTIMOCORTE"].ToString();
							mSemestresCursados = rsContactoEstudio.Rows[0]["SEMESTRESCURSADOS"].ToString();
							mNomCiudadInstitucion = rsContactoEstudio.Rows[0]["NOMCIUDAD"].ToString();
							mFechaInicio = rsContactoEstudio.Rows[0]["FECHAINICIO"].ToString();
							var fechaFinMateria = Convert.ToDateTime(rsContactoEstudio.Rows[0]["FECHAFINMATERIAS"].ToString()).Date;
							if(fechaFinMateria.Year != 1900)
							{
								mFechaFinMateria = rsContactoEstudio.Rows[0]["FECHAFINMATERIAS"].ToString();
							}
							else
							{
								mFechaFinMateria = "";
							}
							

							//Datos en los campos de texto...
							tb_ciudadinstitucionperfil.Text = rsContactoEstudio.Rows[0]["NOMCIUDAD"].ToString();
							id_fechafinalizacion.Text = rsContactoEstudio.Rows[0]["FECHAFINMATERIAS"].ToString();
							tb_fechagraduacionperfil.Text = rsContactoEstudio.Rows[0]["FECHAGRADO"].ToString();
							tb_fechaInicioperfil.Text = rsContactoEstudio.Rows[0]["FECHAINICIO"].ToString();
							ddl_nivelestudioperfil.SelectedValue = rsContactoEstudio.Rows[0]["CODNIVELESTUDIO"].ToString();
							tb_Programarealizadoperfil.Text = rsContactoEstudio.Rows[0]["NOMPROGRAMAACADEMICO"].ToString();

							txtSQL = "SELECT Id_NivelEstudio, NomNivelEstudio From NivelEstudio ORDER BY Id_NivelEstudio";
							var rsNivelEstudio = consultas.ObtenerDataTable(txtSQL, "text");

							ddl_nivelestudioperfil.Items.Clear();

							foreach (DataRow row in rsNivelEstudio.Rows)
							{
								ListItem item = new ListItem();
								item.Value = row["Id_NivelEstudio"].ToString();
								item.Text = row["NomNivelEstudio"].ToString();
								if (mCodNivelEstudio == item.Value) { item.Selected = true; }
								ddl_nivelestudioperfil.Items.Add(item);
							}
							ddl_nivelestudioperfil.Items.Insert(0, new ListItem("Seleccione", "0"));

							//Programa realizado.
							tb_Programarealizadoperfil.Text = mTituloObtenido;
							CodProgramaRealizado = mCodProgramaRealizado;
							tb_Institucionperfil.Text = mInstitucion;
							tb_ciudadinstitucionperfil.Text = mNomCiudadInstitucion;

							//Finalizado o estudiando actualmente.
							ddl_estadoperfil.SelectedValue = mEstudioFinalizado;
							if (mEstudioFinalizado == "0")
							{
								filafecha.Visible = false;
								filafecha2.Visible = false;
							}
							else
							{
								filafecha.Visible = true;
								filafecha2.Visible = true;
							}
							//if (mEstudioFinalizado == "1")
							//{ ddl_estadoperfil.Items[0].Selected = true; }
							//else { ddl_estadoperfil.Items[1].Selected = true; }

							//Fecha de inicio.
							try
							{
								if (DateTime.TryParse(mFechaInicio, out fecha_Validar))
								{
									Calendarextender1.SelectedDate = DateTime.Parse(mFechaInicio);
									tb_fechaInicioperfil.Text = DateTime.Parse(mFechaInicio).ToString("dd/MM/yyyy");
								}
								else { tb_fechaInicioperfil.Text = ""; }
							}
							catch { tb_fechaInicioperfil.Text = ""; }

							//Fecha de fin matetias.
							try
							{
								if (!string.IsNullOrEmpty(mFechaFinMateria))
								{
									if (DateTime.TryParse(mFechaFinMateria, out fecha_Validar))
									{
										Calendarextender4.SelectedDate = DateTime.Parse(mFechaFinMateria);
										id_fechafinalizacion.Text = DateTime.Parse(mFechaFinMateria).ToString("dd/MM/yyyy");
									}
									else { id_fechafinalizacion.Text = ""; }
								}
								
							}
							catch { id_fechafinalizacion.Text = ""; }

							//Fecha de graduación.
							try
							{
								if(!string.IsNullOrEmpty(mFechaGrado))
								{
									if (DateTime.TryParse(mFechaGrado, out fecha_Validar))
									{
										Calendarextender3.SelectedDate = DateTime.Parse(mFechaGrado);
										tb_fechagraduacionperfil.Text = DateTime.Parse(mFechaGrado).ToString("dd/MM/yyyy");
									}
									else { tb_fechagraduacionperfil.Text = ""; }
								}
								
							}
							catch { tb_fechagraduacionperfil.Text = ""; }

							//Fecha finalización etapa lectiva del programa o curso...

							//Semestre actual...

							//Condición especial.
							
							//int test = 0;
							//if (Int32.TryParse(lCodTipoAprendiz, out test))
							//{ ddl_condicionespecialperfil.Items[1].Selected = true; }
							//else { ddl_condicionespecialperfil.Items[0].Selected = true; }

							txtSQL = "SELECT id_TIPOAPRENDIZ, NomTIPOAPRENDIZ From TIPOAPRENDIZ ORDER BY NomTIPOAPRENDIZ";
							var rsDeps = consultas.ObtenerDataTable(txtSQL, "text");

							ddl_tipoaprendizperfil.Items.Clear();

							foreach (DataRow row in rsDeps.Rows)
							{
								ListItem item = new ListItem();
								item.Value = row["id_TIPOAPRENDIZ"].ToString();
								item.Text = row["NomTIPOAPRENDIZ"].ToString();
								//if (lCodTipoAprendiz == item.Value) { item.Selected = true; }
								ddl_tipoaprendizperfil.Items.Add(item);
							}
							ddl_tipoaprendizperfil.Items.Insert(0, new ListItem("Seleccione", "0"));
							//ddl_condicionespecialperfil.SelectedValue = lCodTipoAprendiz;
							ddl_tipoaprendizperfil.SelectedValue = lCodTipoAprendiz;
						}
					}
					#endregion

					break;
				case "Borrar":

					#region Borrar el emprendedor del proyecto seleccionado.

					try
					{
						//Dividir los valores.
						parametros = e.CommandArgument.ToString().Split(';');

						//Asignar valores a las variables.
						CodProyecto = parametros[0];
						CodContacto = parametros[1];
						//CodProyecto = Request["CodProyecto"];

						#region Eliminar el emprendedor seleccionado.

						//Revisar si tiene tareas pendientes
						//txtSQL = " select count(id_tareausuario) as cuantos from tareausuario,tareausuariorepeticion " +
						//         " where id_tareausuario=codtareausuario and  fechacierre is null and codcontacto=" + CodContacto;

						//var RS = consultas.ObtenerDataTable(txtSQL, "text");

						//if (RS.Rows.Count > 0)
						//{
							//if (Int32.Parse(RS.Rows[0]["cuantos"].ToString()) > 0)
							//{
								//Quitarle al contacto los permisos
								txtSQL = "Delete from grupocontacto where codcontacto=" + CodContacto;
								ejecutaReader(txtSQL, 2);

								//Inactivar el usuario dentro del proyecto
								txtSQL = " update proyectocontacto set inactivo=1, FechaFin=getdate() " +
										 " where  codcontacto=" + CodContacto + " and codproyecto=" + CodProyecto + " and " +
										 " codrol = " + Constantes.CONST_RolEmprendedor + " and inactivo=0";
								ejecutaReader(txtSQL, 2);

								//'Inactivar el contacto
								txtSQL = "update contacto set inactivo=1 where id_contacto=" + CodContacto;
								ejecutaReader(txtSQL, 2);

								//Al final debe mostrar la grilla de emprendedores y ocutlando lso demás campos.
								limpiarCampos();
								gv_emprendedor.Visible = true;
								pnl_crearEditar.Visible = true;
								tc_Emprendedor.Visible = false;
								pnl_infoAcademica.Visible = false;
								gv_emprendedor.DataBind();
								string redirePagina = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath);
								Response.Redirect(redirePagina + "?Accion=Editar&CodProyecto=" + Request.QueryString["CodProyecto"].ToString());
							//}
						//    else
						//    {
						//        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puede borrar el usuario porque tiene tareas pendientes abiertas.')", true);
						//        return;
						//    }
						//}

						#endregion
					}
					catch { }

					#endregion

					break;
				default:
					break;
					
			}
			tstBtn.Text = CodContacto != null && CodProyecto != null ? "Actualizar" : "Guardar";
			tstBtn.OnClientClick = CodContacto == null && CodProyecto == null ? "__doPostBack('Button3','Button3_Click1')" : "__doPostBack('Button1','tb_crearPrograma_onclick')";
			serializacion = CodContacto == null && CodProyecto == null ? "__doPostBack('Button3','Button3_Click1')" : "__doPostBack('Button1','tb_crearPrograma_onclick')";
		}

		protected void tstBtn_Click(object sender, EventArgs e)
		{

		}
	}

	#region Clase objectEmcap.

	public class objectEmcap
	{
		public objectEmcap()
		{

		}

		String nombreE;

		public String _nombreE
		{
			get { return nombreE; }
			set { nombreE = value; }
		}
		String apellidoE;

		public String _apellidoE
		{
			get { return apellidoE; }
			set { apellidoE = value; }
		}
		String tipoIdentificacionE;

		public String _TipoIdentificacionE
		{
			get { return tipoIdentificacionE; }
			set { tipoIdentificacionE = value; }
		}
		String NoIdentificacion;

		public String _noIdentificacion1
		{
			get { return NoIdentificacion; }
			set { NoIdentificacion = value; }
		}
		String DepartamentExpedicion;

		public String _departamentExpedicion1
		{
			get { return DepartamentExpedicion; }
			set { DepartamentExpedicion = value; }
		}
		String CiudadExpedicionE;

		public String _ciudadExpedicionE1
		{
			get { return CiudadExpedicionE; }
			set { CiudadExpedicionE = value; }
		}
		String EmailE;

		public String _emailE1
		{
			get { return EmailE; }
			set { EmailE = value; }
		}
		String GeneroE;

		public String _generoE1
		{
			get { return GeneroE; }
			set { GeneroE = value; }
		}
		DateTime FechaNaciE;

		public DateTime _fechaNaciE1
		{
			get { return FechaNaciE; }
			set { FechaNaciE = value; }
		}
		String departamentoNacimientoE;

		public String _departamentoNacimientoE
		{
			get { return departamentoNacimientoE; }
			set { departamentoNacimientoE = value; }
		}
		String ciudadnacimientoE;

		public String _ciudadnacimientoE
		{
			get { return ciudadnacimientoE; }
			set { ciudadnacimientoE = value; }
		}
		String TelefonoE;

		public String _telefonoE1
		{
			get { return TelefonoE; }
			set { TelefonoE = value; }
		}
		String nivelEstudioE;

		public String _nivelEstudioE
		{
			get { return nivelEstudioE; }
			set { nivelEstudioE = value; }
		}
		String tituloObtenidoE;

		public String _tituloObtenidoE
		{
			get { return tituloObtenidoE; }
			set { tituloObtenidoE = value; }
		}
		String institucionE;

		public String _institucionE
		{
			get { return institucionE; }
			set { institucionE = value; }
		}
		String ciudadIE;//pendiente control

		public String _ciudadIE
		{
			get { return ciudadIE; }
			set { ciudadIE = value; }
		}
		String estadoestudiosE;

		public String _estadoestudiosE
		{
			get { return estadoestudiosE; }
			set { estadoestudiosE = value; }
		}
		String fechainicioE;

		public String _fechainicioE
		{
			get { return fechainicioE; }
			set { fechainicioE = value; }
		}
		String fechafinalizacionMateriaE;

		public String _fechafinalizacionMateriaE
		{
			get { return fechafinalizacionMateriaE; }
			set { fechafinalizacionMateriaE = value; }
		}
		String fechaGraduacionE;

		public String _FechaGraduacionE
		{
			get { return fechaGraduacionE; }
			set { fechaGraduacionE = value; }
		}
		String fehcaGradoE;

		public String _fehcaGradoE
		{
			get { return fehcaGradoE; }
			set { fehcaGradoE = value; }
		}
		String condicionEspecialE;

		public String _condicionEspecialE
		{
			get { return condicionEspecialE; }
			set { condicionEspecialE = value; }
		}
		Int32 tipoEmprendizE;

		public Int32 _tipoEmprendizE
		{
			get { return tipoEmprendizE; }
			set { tipoEmprendizE = value; }
		}

		String nombreprograma;

		public String _nombreprograma
		{
			get { return nombreprograma; }
			set { nombreprograma = value; }
		}
	}

	#endregion
}