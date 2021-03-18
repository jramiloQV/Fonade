#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>19 - 06 - 2014</Fecha>
// <Archivo>ProyectoOffline.cs</Archivo>

#endregion

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Fonade.FONADE.Offline
{
    public partial class ProyectoOffline : Negocio.Base_Page
    {
        int CodProyecto;//variable para el control del proyecto del emprendedor Fonade

        /// <summary>
        /// Diego Quiñonez
        /// 19 - 06 - 2014
        /// motodo de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CodProyecto = cargueData(usuario.IdContacto);
            }
            catch (FormatException) { return; }
            catch (ArgumentNullException) { return; }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 19 - 06 - 2014
        /// funcion que retorna el codigo del proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int cargueData(int id)
        {
            var result = (from pc in consultas.Db.ProyectoContactos
                          where pc.Inactivo == false
                          && pc.CodContacto == id
                          select pc.CodProyecto).First();

            return result;
        }

        /// <summary>
        /// Diego Quiñonez
        /// 19 - 06 - 2014
        /// metodo que retorna y monta en la grilla
        /// la informacion y los documentos BackUp
        /// del proyecto por cvada emprendedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_proyectosoffline_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from df in consultas.Db.DocumentoFormatos
                          from t in consultas.Db.Tabs
                          join dc in consultas.Db.Documentos on t.Id_Tab equals dc.CodTab
                          where df.Id_DocumentoFormato == dc.CodDocumentoFormato
                          && dc.Borrado == false
                          && dc.CodProyecto == cargueData(usuario.IdContacto)
                          && (dc.CodDocumentoFormato == 19 || dc.CodDocumentoFormato == 17)
                          orderby dc.NomDocumento
                          select new
                          {
                              dc.Id_Documento,
                              dc.NomDocumento,
                              dc.Fecha,
                              dc.URL,
                              df.NomDocumentoFormato,
                              df.Icono,
                              dc.CodDocumentoFormato,
                              t.NomTab
                          });

            if (result.Any())
                e.Result = result.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 19 - 06 - 2014
        /// permite el paginado del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvr_proyectooffline_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvr_proyectooffline.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// Diego Quiñonez
        /// 19 - 06 - 2014
        /// acciones del gridview
        /// sobre cada uno de los documentos
        /// que se cargan en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvr_proyectooffline_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("BorrarDocumento"))
            {
                #region desactivar documento

                var documento = (from dc in consultas.Db.Documentos
                                 where dc.Id_Documento == Convert.ToInt32(e.CommandArgument.ToString())
                                 select dc).First();

                documento.Borrado = true;

                consultas.Db.SubmitChanges();

                var result = (from dc in consultas.Db.Documentos
                              where dc.Borrado == true
                              && dc.Id_Documento == Convert.ToInt32(e.CommandArgument.ToString())
                              select dc.URL).Distinct().FirstOrDefault();

                try
                {
                    System.IO.File.Delete(result);
                }
                catch (System.IO.IOException) { }

                #endregion
            }
            else
            {
                if (e.CommandName.Equals("DescargarDocumento"))
                {
                    #region downlodascript
                    Response.Buffer = true;
                    Response.Clear();

                    string strFileName = e.CommandArgument.ToString().Split(';')[0];
                    string strFile = strFileName.Substring(strFileName.Length, strFileName.Length - strFileName.LastIndexOf(@"\"));
                    string strFileType = e.CommandArgument.ToString().Split(';')[1];
                    if (strFileType == "")
                    {
                        strFileType = "application/download";
                    }

                    var files = System.IO.Directory.GetFiles(Server.MapPath("E:\\ftproot\\sales"));
                    #endregion
                }
                else
                {
                    if (e.CommandName.Equals("ProcesaCarga"))
                    {
                        procesaCarga(e.CommandArgument.ToString());
                    }
                }
            }
        }


        /// <summary>
        /// en construccion...........
        /// utlima modificacion 19 - 06 - 2014
        /// no borrar por falta de funcionalidad
        /// </summary>
        /// <param name="url"></param>
        private void procesaCarga(string url)
        {
            ClientScriptManager cm = this.ClientScript;

            string id = cargueData(usuario.IdContacto).ToString();
            string fechaArchivo = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
            
            string txtCarpeta = "Proyecto_" + id;
            string SysNombreArchivo = "ProyectoOffLine" + id + " " + fechaArchivo + ".xml";
            string RutaHttpDestino = "\\Documentos\\FonadeDocumentos\\ProyectosOffline" + "\\" + txtCarpeta + "\\" + SysNombreArchivo;

            if (string.IsNullOrEmpty(url))
                return;
            else
            {
                string NombreArchivo = System.IO.Path.GetFileName(url);

                System.IO.FileInfo fi = new System.IO.FileInfo(NombreArchivo);
                string Tamano = fi.Length.ToString();

                try
                {
                    if (!System.IO.Directory.Exists("\\FonadeDocumentos\\ProyectosOffline" + "\\" + txtCarpeta))
                        System.IO.Directory.CreateDirectory("\\FonadeDocumentos\\ProyectosOffline" + "\\" + txtCarpeta);
                }
                catch (System.IO.IOException)
                {
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Error No se pudo crear la carpeta : " + txtCarpeta + "');</script>");
                    return;
                }

                Datos.Documento document = new Datos.Documento();

                document.CodProyecto = Convert.ToInt32(id);
                document.NomDocumento = "ArchivoOffline";
                document.CodDocumentoFormato = 17;
                document.URL = RutaHttpDestino;
                document.Comentario = "Backup Off-Line";
                document.Fecha = DateTime.Now;
                document.CodContacto = usuario.IdContacto;

                consultas.Db.Documentos.InsertOnSubmit(document);

                try
                {
                    consultas.Db.SubmitChanges();
                }
                catch (LinqDataSourceValidationException) { return; }
                catch (Exception)
                {
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No se realizo el registro correctamente');</script>");
                    return;
                }
            }
        }
    }
}