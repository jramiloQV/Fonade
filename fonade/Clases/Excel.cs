using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
namespace Fonade.Clases
{
    /// <summary>
    /// Excel
    /// </summary>
    public class Excel
    {
        /// <summary>
        /// devuelve informacion de un excel dentro de un datatable
        /// </summary>
        /// <param name="archivo">The archivo.</param>
        /// <param name="NomHoja">The nom hoja.</param>
        /// <returns></returns>
        public static DataTable recogerExcel(string archivo, string NomHoja)
        {
            DataTable dt = new DataTable();

            OleDbConnection conexion = null; 
            OleDbDataAdapter dataAdapter = null;
            string consultaHojaExcel = "Select * from [" + NomHoja + "$]";
            string cadenaConexionArchivoExcel = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";//cadena que permite realizar una conexion con un libro excel 

                conexion = new OleDbConnection(cadenaConexionArchivoExcel);
                conexion.Open();
                dataAdapter = new OleDbDataAdapter(consultaHojaExcel, conexion);

                dataAdapter.Fill(dt);
                conexion.Close();
                conexion.Dispose();
            
                if (conexion != null)
                    conexion.Close();
            
            return dt;
        }

        /// <summary>
        /// se valida de acuerdo a la extension del excel
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string pathName, string sheetName)
        {
            DataTable tbContainer = new DataTable();
            string strConn = string.Empty;
            if (string.IsNullOrEmpty(sheetName)) { sheetName = "Sheet1"; }
            var file = new FileInfo(pathName);
            if (!file.Exists) { 
                throw new Exception("Error, file doesn't exists!"); 
            }
            string extension = file.Extension;
            switch (extension)
            {
                case ".xls":
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                    break;
                case ".xlsx":
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                    break;
                default:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                    break;
            }
            OleDbConnection cnnxls = new OleDbConnection(strConn);
            OleDbDataAdapter oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
            DataSet ds = new DataSet();
            oda.Fill(tbContainer);
            return tbContainer;
        }


    }
}