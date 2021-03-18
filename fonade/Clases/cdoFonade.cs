using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.UI;

namespace Fonade.Clases{
    /// <summary>
    /// cdoFonadeActas
    /// </summary>    
    public class cdoFonadeActas : IEnumerable<Datos.MD_ActasFormatoResult>
    {
        /// <summary>
        /// Gets or sets a value indicating whether [context proc].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [context proc]; otherwise, <c>false</c>.
        /// </value>
        protected static bool contextProc { get; set; }

        /// <summary>
        /// Lista todas las actas
        /// </summary>
        /// <param name="IdConvocatoria">id de la convocatoria.</param>
        /// <returns></returns>
        public List<Datos.MD_ActasFormatoResult> ListAllActas(int IdConvocatoria){
            Consultas asd = new Consultas();
            asd.Db.DeferredLoadingEnabled = false;
            asd.Db.CommandTimeout = 17000;
            asd.Db.LoadOptions = new System.Data.Linq.DataLoadOptions();
            asd.Db.ObjectTrackingEnabled = false;
            var osj = new Datos.MD_ActasFormatoResult()
            { CodConvocatoria = IdConvocatoria, CodDocumentoFormato = 0, Fecha = DateTime.Now, FechaActa = DateTime.Now,
              Icono = string.Empty, Id_Acta = 0, NomActa = string.Empty, NomDocumentoFormato = string.Empty, URL = string.Empty,
              Comentario = string.Empty, NumActa = string.Empty
            };
            var idb = asd.Db.MD_ActasFormato(IdConvocatoria).Where(a => a.CodConvocatoria == IdConvocatoria).ToList();
            idb.Add(osj);
            idb = idb.OrderBy(n=>n.Id_Acta).ToList();
            asd.Db.Dispose();
            contextProc = false;
            return idb.ToList();
        }

        /// <summary>
        /// Crea nueva acta
        /// </summary>
        /// <param name="CodConvocatoria">id convocatoria.</param>
        /// <param name="CodDocumentoFormato">id formato del documento.</param>
        /// <param name="Comentario">comentario.</param>
        /// <param name="Fecha">fecha.</param>
        /// <param name="FechaActa">fecha del acta.</param>
        /// <param name="Icono">icono.</param>
        /// <param name="Id_Acta"> id del acta.</param>
        /// <param name="NomActa">The nom acta.</param>
        /// <param name="NomDocumentoFormato">nombre documento.</param>
        /// <param name="NumActa">numero del acta.</param>
        /// <param name="URL">URL.</param>
        public void NewActa(string CodConvocatoria, string CodDocumentoFormato, string Comentario, string Fecha, string FechaActa, string Icono, string Id_Acta, string NomActa, string NomDocumentoFormato, string NumActa, string URL){
            if (contextProc) { return; }
            var jyf = new Datos.MD_ActasFormatoResult()
            {
                CodConvocatoria = int.Parse(CodConvocatoria),
                CodDocumentoFormato = byte.Parse(CodDocumentoFormato??"0"),
                Comentario = Comentario,
                Fecha = DateTime.Parse(Fecha),
                FechaActa = DateTime.Parse(FechaActa),
                Icono = Icono??"0",
                Id_Acta = int.Parse(Id_Acta??"0"),
                NomActa = NomActa,
                NomDocumentoFormato = NomDocumentoFormato??"0",
                NumActa = NumActa,
                URL = URL
            };

            
            Consultas asd = new Consultas();
            asd.Db.MD_ActasFormato(0).ToList().Add(jyf);
            asd.Db.SubmitChanges();
            var IdContacto = HttpContext.Current.Request.ServerVariables["cod"]??"0";
            CodDocumentoFormato = HttpContext.Current.Session["Adjuntos"]!=null?((HttpPostedFile[])HttpContext.Current.Session["Adjuntos"]).FirstOrDefault().FileName.Split('.')[1].Insert(0, "."):"5";
            var _CodDocumentoFormato = 
            (int)asd.Db.DocumentoFormatos.Where(u => u.Extension == CodDocumentoFormato).Select(h => h.Id_DocumentoFormato).FirstOrDefault();
            _CodDocumentoFormato = _CodDocumentoFormato > 0 ? _CodDocumentoFormato : 5;
            CodDocumentoFormato = _CodDocumentoFormato.ToString();
            var sqlInsert = "INSERT INTO [dbo].[ConvocatoriaActa] ([NumActa] ,[NomActa] ,[FechaActa] ,[Fecha] ,[URL] ,[CodConvocatoria]" +
                                       ",[CodDocumentoFormato],[CodContacto],[Comentario],[Borrado]) " +
                            "VALUES ({0} ,ƒ{1}ƒ ,CONVERT(DATETIME, ƒ{2}ƒ, 103) ,CONVERT(DATETIME, ƒ{3}ƒ, 103) ,ƒ{4}ƒ ,{5} ,{6} ,{7} ,ƒ{8}ƒ ,{9})";
            sqlInsert = string.Format(sqlInsert, jyf.NumActa, jyf.NomActa, jyf.FechaActa.ToShortDateString(), jyf.Fecha.ToShortDateString(), jyf.URL, jyf.CodConvocatoria, CodDocumentoFormato, IdContacto, jyf.Comentario, new byte());
            sqlInsert = commandFormat(sqlInsert);
            SqlDataSource sqlDs = new SqlDataSource() { ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString, InsertCommand = sqlInsert, UpdateCommandType = SqlDataSourceCommandType.Text, DataSourceMode = SqlDataSourceMode.DataReader };
            sqlDs.Insert();
            sqlDs.Dispose();
            contextProc = true;
        }

        /// <summary>
        ///Eliminar el acta.
        /// </summary>
        /// <param name="CodConvocatoria">id convocatoria.</param>
        /// <param name="CodDocumentoFormato">id documento formato.</param>
        /// <param name="Comentario">comentario.</param>
        /// <param name="Fecha">fecha.</param>
        /// <param name="FechaActa">fecha del acta.</param>
        /// <param name="Icono">icono.</param>
        /// <param name="Id_Acta">id del acta.</param>
        /// <param name="NomActa">nombre del acta.</param>
        /// <param name="NomDocumentoFormato">id documento formato.</param>
        /// <param name="NumActa">numero del acta.</param>
        /// <param name="URL">URL.</param>
        public void DeleteActa(string CodConvocatoria, string CodDocumentoFormato, string Comentario, string Fecha, string FechaActa, string Icono,
                               string Id_Acta, string NomActa, string NomDocumentoFormato, string NumActa, string URL){
            if (contextProc){ return; }
            var jyf = new Datos.MD_ActasFormatoResult(){
                CodConvocatoria = int.Parse(CodConvocatoria),
                CodDocumentoFormato = byte.Parse(CodDocumentoFormato ?? "0"),
                Comentario = Comentario??string.Empty,
                Fecha = DateTime.Parse(Fecha??DateTime.Now.ToShortDateString()),
                FechaActa = DateTime.Parse(FechaActa??DateTime.Now.ToShortDateString()),
                Icono = Icono ?? "0",
                Id_Acta = int.Parse(Id_Acta ?? "0"),
                NomActa = NomActa??string.Empty,
                NomDocumentoFormato = NomDocumentoFormato ?? "0",
                NumActa = NumActa??"0",
                URL = URL??string.Empty
            };
            Consultas asd = new Consultas();
            asd.Db.MD_ActasFormato(0).ToList().Remove(jyf);
            asd.Db.SubmitChanges();
            var sqlInsert = "DELETE FROM [dbo].[ConvocatoriaActa] WHERE [Id_Acta] ß {0} AND [NumActa] ß ƒ{1}ƒ AND [CodConvocatoria] ß {2}";
            sqlInsert = string.Format(sqlInsert, jyf.Id_Acta, jyf.NumActa, jyf.CodConvocatoria);
            sqlInsert = commandFormat(sqlInsert);
            SqlDataSource sqlDs = new SqlDataSource() { ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString, DeleteCommand = sqlInsert, DeleteCommandType = SqlDataSourceCommandType.Text, DataSourceMode = SqlDataSourceMode.DataReader };
            sqlDs.Delete();
            sqlDs.Dispose();
            contextProc = true;
        }

        /// <summary>
        /// Commands the format.
        /// </summary>
        /// <param name="SQLCommand">The SQL command.</param>
        /// <returns></returns>
        protected string commandFormat(string SQLCommand){
            SQLCommand = SQLCommand.Replace(char.Parse("'"), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("%"), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("="), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("ß"), char.Parse("="));
            SQLCommand = SQLCommand.Replace(char.Parse("Ø"), char.Parse("%"));
            SQLCommand = SQLCommand.Replace(char.Parse("Ÿ"), char.Parse("+"));
            SQLCommand = SQLCommand.Replace(char.Parse("ƒ"), char.Parse("'"));
            return SQLCommand;
        }

        /// <summary>
        /// Handles the RowCommand event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        public void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// Devuelve un enumerador que itera a través de la colección.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Datos.MD_ActasFormatoResult> GetEnumerator()
        {
            return new List<Datos.MD_ActasFormatoResult>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new List<Datos.MD_ActasFormatoResult>().GetEnumerator();
        }
    }

    /// <summary>
    /// genericQueries
    /// </summary>
    /// <seealso cref="System.Web.HttpSessionStateBase" />
    public class genericQueries : HttpSessionStateBase {

        /// <summary>
        /// Obtiene o establece el _usr.
        /// </summary>
        /// <value>
        /// usr.
        /// </value>
        public static Dictionary<string, Account.FonadeUser>_usr { get; set; }

        /// <summary>
        /// Obtiene o establece el formato.
        /// </summary>
        /// <value>
        /// The indeed formatted.
        /// </value>
        public byte indeedFormatted { get; set; }

        /// <summary>
        /// valida si el usuario pertenece al proyecto.
        /// </summary>
        /// <param name="Id_Contacto">The identifier contacto.</param>
        /// <param name="CodProyecto">The cod proyecto.</param>
        /// <returns></returns>
        public bool ValidateUserCode(int Id_Contacto, Object CodProyecto)
        {
            if (CodProyecto == null)
                return false;

                var codigoProyecto = CodProyecto.ToString();
                var appSetting = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                var rolId = System.Configuration.ConfigurationManager.AppSettings["IdRol"]??"0";
                var cntx = new Consultas().Db;
                var proyectoContacto = cntx.Contacto.Where(y => y.ProyectoContacto.Where(p => p.CodRol == int.Parse(rolId)).Count() > 0).Where(c => c.Id_Contacto == Id_Contacto && c.ProyectoContacto.Select(t => t.CodProyecto == int.Parse(codigoProyecto)).Count() > 0).Count() > 0;
                var contactoProyecto = cntx.ProyectoContactos.Where(n => n.Contacto.Id_Contacto == Id_Contacto && n.CodProyecto == int.Parse(codigoProyecto)).Select(i => i.CodRol == int.Parse(rolId)).ToList();
                var numeroContacto = contactoProyecto.Count() > 0;
                return numeroContacto;
        }

        /// <summary>
        /// Gets or sets the index provider.
        /// </summary>
        /// <value>
        /// The index provider.
        /// </value>
        public int idxProvider { get; set; }

        /// <summary>
        /// Gets or sets the rw SND.
        /// </summary>
        /// <value>
        /// The rw SND.
        /// </value>
        public int rwSnd { get; protected set; }

        /// <summary>
        /// Gets or sets the query override.
        /// </summary>
        /// <value>
        /// The query override.
        /// </value>
        public static string queryOverride { get; set; }

        /// <summary>
        /// obtiene el id contacto jefe unidad.
        /// </summary>
        /// <param name="Institucion">id institucion.</param>
        /// <returns></returns>
        public int getId_ContactoJefeUnidad(int Institucion){
            var esz = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CodGrupo"].ToString());

            var sqlSelect = string.Format("SELECT InstitucionContacto.CodContacto FROM InstitucionContacto INNER JOIN " + 
                                          "GrupoContacto ON InstitucionContacto.CodContacto = GrupoContacto.CodContacto " +
                                          "WHERE (InstitucionContacto.FechaFin IS NULL) AND (GrupoContacto.CodGrupo = {0}) " +
                                          "AND (InstitucionContacto.CodInstitucion = {1})", esz, Institucion);
            SqlDataSource sqlds = new SqlDataSource(){ 
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString,
                DataSourceMode = SqlDataSourceMode.DataReader, SelectCommand = sqlSelect, SelectCommandType = SqlDataSourceCommandType.Text };
            var thn = sqlds.Select(new System.Web.UI.DataSourceSelectArguments());
            if(((System.Data.SqlClient.SqlDataReader)thn).Read()) return ((System.Data.SqlClient.SqlDataReader)thn).GetInt32(0);
            return 0;
        }

        /// <summary>
        /// obtiene el interventor del proyecto.
        /// </summary>
        /// <param name="IdProyecto">id del proyecto.</param>
        /// <returns></returns>
        public Datos.Contacto getInterventorProyecto(int IdProyecto){
            //var tareaUsuario = new Consultas().Db.TareaUsuarios.Where(c => c.Contacto.GrupoContactos.Select(g => g.CodGrupo == Constantes.CONST_GerenteInterventor).FirstOrDefault() != null && c.CodProyecto == IdProyecto);
            var tareaUsuario = new Consultas().Db.TareaUsuarios.Where(c => c.Contacto.GrupoContacto.Select(g => g.CodGrupo == Constantes.CONST_GerenteInterventor).FirstOrDefault() != false && c.CodProyecto == IdProyecto);
            var grupoContacto = tareaUsuario.Where(f => f.Contacto.GrupoContacto.Select(c => c.CodGrupo).FirstOrDefault() == Constantes.CONST_Interventor).Select(u => u.Contacto).FirstOrDefault();
            var pryIntrv = grupoContacto;
            return pryIntrv ?? new Datos.Contacto();
        }

        /// <summary>
        /// obtiene el gerente interventor del proyecto.
        /// </summary>
        /// <param name="IdProyecto">id proyecto.</param>
        /// <returns></returns>
        public Datos.Contacto getGerenteInterventorProyecto(int IdProyecto, int? _codOperador){
            //var tareaUsuario = new Consultas().Db.TareaUsuarios.Where(c => c.Contacto.GrupoContactos.Select(g => g.CodGrupo == Constantes.CONST_GerenteInterventor).FirstOrDefault() != null && c.CodProyecto == IdProyecto);
            var tareaUsuario = new Consultas().Db.TareaUsuarios
                                    .Where(c => c.Contacto.GrupoContacto
                                                .Select(g => g.CodGrupo == Constantes.CONST_GerenteInterventor)
                                    .FirstOrDefault() != false && c.CodProyecto == IdProyecto);
            var grupoContacto = tareaUsuario
                                    .Where(f => f.Contacto.GrupoContacto
                                                .Select(c => c.CodGrupo)
                                    .FirstOrDefault() == Constantes.CONST_GerenteInterventor)                                    
                                    .Select(u=>u.Contacto)
                                    .Where(u=>u.codOperador == _codOperador)
                                    .FirstOrDefault();
            var gerIntrv = grupoContacto;
            return gerIntrv??new Datos.Contacto();
        }

        /// <summary>
        /// Commands the format.
        /// </summary>
        /// <param name="SQLCommand">The SQL command.</param>
        /// <returns></returns>
        protected string commandFormat(string SQLCommand)
        {
            SQLCommand = SQLCommand.Replace(char.Parse("'"), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("%"), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("="), char.Parse(" "));
            SQLCommand = SQLCommand.Replace(char.Parse("ß"), char.Parse("="));
            SQLCommand = SQLCommand.Replace(char.Parse("Ø"), char.Parse("%"));
            SQLCommand = SQLCommand.Replace(char.Parse("Ÿ"), char.Parse("+"));
            SQLCommand = SQLCommand.Replace(char.Parse("ƒ"), char.Parse("'"));
            return SQLCommand;
        }

        /// <summary>
        /// Executes the query reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="execType">Type of the execute.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public System.Data.SqlClient.SqlDataReader executeQueryReader(string query, byte execType = 0, System.Data.SqlClient.SqlCommand command = null){
            query = query ?? queryOverride;
            if (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(queryOverride)) return null;
            if(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["check4"]??"0")==3)setLogAppEntry(query);
            Consultas asd = new Consultas();
            var sqlInstruction = query;
     
            sqlInstruction = string.Format(sqlInstruction);
            sqlInstruction = indeedFormatted!=0? commandFormat(sqlInstruction):sqlInstruction;
            SqlDataSource sqlDs = new SqlDataSource(){
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString,
                SelectCommand = execType == 0 ? sqlInstruction : string.Empty,
                SelectCommandType = command == null ? SqlDataSourceCommandType.Text : SqlDataSourceCommandType.StoredProcedure,
                InsertCommand = execType == 1 ? sqlInstruction : string.Empty,
                InsertCommandType = command == null ? SqlDataSourceCommandType.Text : SqlDataSourceCommandType.StoredProcedure,
                UpdateCommand = execType == 2 ? sqlInstruction : string.Empty,
                UpdateCommandType = command == null ? SqlDataSourceCommandType.Text : SqlDataSourceCommandType.StoredProcedure,
                DeleteCommand = execType==3?sqlInstruction:string.Empty, 
                DeleteCommandType = command ==null?SqlDataSourceCommandType.Text:SqlDataSourceCommandType.StoredProcedure, 
                DataSourceMode = execType==0? SqlDataSourceMode.DataReader: SqlDataSourceMode.DataSet,
                ConflictDetection = System.Web.UI.ConflictOptions.OverwriteChanges
            };
            SqlDataReader reader = null;
            try { 
                switch (execType){
                    case 0: { reader = ((SqlDataReader)sqlDs.Select(new System.Web.UI.DataSourceSelectArguments())); break; }
                    case 1: { rwSnd = sqlDs.Insert(); break; }
                    case 2: { rwSnd = sqlDs.Update(); break; }
                    case 3: { rwSnd = sqlDs.Delete(); break; }
                    default: { break; }
                }
            }
            catch(Exception xcpt){
                setLogAppEntry(xcpt.Message + '\n' +  xcpt.StackTrace + '\n' + xcpt.InnerException!=null?xcpt.Source :xcpt.InnerException.Message);
            }
            sqlDs.Dispose();
            return reader;
        }

        /// <summary>
        /// obtiene los items proyecto mercado proyeccion ventas.
        /// </summary>
        /// <param name="CodProyecto">id proyecto.</param>
        /// <returns></returns>
        public List<Datos.ProyectoMercadoProyeccionVenta> getItemsProyectoMercadoProyeccionVentas(int CodProyecto){
            var lst = new Consultas().Db.ProyectoMercadoProyeccionVentas.Where(p => p.CodProyecto == CodProyecto).ToList();
            return lst??new List<ProyectoMercadoProyeccionVenta>();
        }

        /// <summary>
        /// Sets the log application entry.
        /// </summary>
        /// <param name="_param">The parameter.</param>
        protected void setLogAppEntry(string _param){
            System.Diagnostics.EventLog appLog =
                new System.Diagnostics.EventLog();                        
        }

        /// <summary>
        /// Records to log.
        /// </summary>
        /// <param name="_param">The parameter.</param>
        public static void recordToLog(string _param){
            if (!string.IsNullOrEmpty(_param)) new genericQueries().setLogAppEntry(_param);
            _param = string.Empty;
        }

    }
}