using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fonade.Negocio.Utility
{

    /// <summary>
    /// Clase de utilidades
    /// By @marztres
    /// </summary>
    public static class FonadeUtility
    {
        /// <summary>
        /// Metodo que devuelve un string con caracteres especiales de html
        /// codificado para guadarlo en la base de datos
        /// </summary>
        /// <param name="field"> String con caracteres especiales </param>
        /// <returns> String codificado para guardar en base de datos </returns>
        public static string htmlEncode(this string field)
        {
            return System.Net.WebUtility.HtmlEncode(field);
        }

        /// <summary>
        /// Metodo que devuelve un string codificado con caracteres especiales de html
        /// </summary>
        /// <param name="field"> String con caracteres especiales </param>
        /// <returns> String codificado con caracteres especiales de html </returns>
        public static string htmlDecode(this string field)
        {
            return System.Net.WebUtility.HtmlDecode(field);
        }

        /// <summary>
        /// Metodo que devuelve un string con caracteres especiales reemplazados  
        /// </summary>        
        /// <param name="addDateIdentifier"> Añade caracteres de datetime ej : nombre+220120161222</param>
        /// <returns> String con caracteres especiales removidos  </returns>
        public static string cleanSpecialChars(this string urlText, Boolean addDateIdentifier = false )
        { 
            List<string> urlParts = new List<string>();
            string rt = "";
            Regex r = new Regex(@"[a-z]+", RegexOptions.IgnoreCase);
            foreach (Match m in r.Matches(urlText))
            {
              urlParts.Add(m.Value);
            }
            for (int i = 0; i < urlParts.Count; i++)
            {
            rt = rt + urlParts[i];
            rt = rt + "_";
            }

            if (addDateIdentifier)
            {
                rt += DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString(); 
            }

            return rt;
        }

        public static string moneyFormat(this Decimal valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        public static string moneyFormat(this Double valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        public static string moneyFormat(this long valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        public static string getFechaAbreviadaConFormato(this DateTime fechaSinFormato, Boolean showTime = false)
        {
            string diaSemana = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fechaSinFormato.DayOfWeek);
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaSinFormato.Month);
            string hora = showTime ? " " + fechaSinFormato.ToString("hh:mm") + " " + fechaSinFormato.ToString("tt", CultureInfo.CurrentCulture) : string.Empty;

            string fechaConFormato = fechaSinFormato.Day + " " + mes + " " + fechaSinFormato.Year + hora;

            return fechaConFormato;
        }        
    }
}
