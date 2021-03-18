using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Web.UI;

namespace Fonade.Clases
{
    /// <summary>
    /// Clase para validar variables y campos
    /// Para validar campos string y numerico :
    /// FieldValidate.ValidateString("Componente", txtComponente.Text, true, 255);
    /// FieldValidate.ValidateNumeric("Valor",txtValor.Text,true);
    /// obtener el valor de variables de sesión de string,Int64(Int16,Int32,Int64).
    /// Uso : Si quiere obtener el valor de una variables session string o int64  
    /// Para string SessionVarValidate.GetString("NombreVariableSesion"), para Int64 SessionVarValidate.GetInt("NombreVariableSesion");
    /// Para comprobar si la variable es valida o no : 
    /// Para String if(variable.equals(String.Empty)) 
    /// Para int if(variable.equals(null)) 
    /// Nota : El valor devuelto por el metodo GetInt es un Int64? nullable.
    /// Autor : Marcel Solera @marztres
    /// Fecha : 24/06/2015
    /// </summary>
    public static class FieldValidate
    {
        /// <summary>
        /// Metodo que obtiene el contexto de la sesión actual, sino existe genera ApplicationExceptión
        /// </summary>
        static HttpSessionState Session
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new ApplicationException("No existe un contexto Http valido, No se puede acceder a la información de aplicación.!");

                return HttpContext.Current.Session;
            }
        }

        static string[] pats3 = { "é", "É", "á", "Á", "í", "Í", "ó", "Ó", "ú", "Ú", "ñ", "Ñ", " " };
        static string[] repl3 = { "e", "E", "a", "A", "i", "I", "o", "O", "u", "U", "n", "N", "" };
        static Dictionary<string, string> _var = null;
        static Dictionary<string, string> dict
        {
            get
            {
                if (_var == null)
                {
                    _var = pats3.Zip(repl3, (k, v) => new { Key = k, Value = v }).ToDictionary(o => o.Key, o => o.Value);
                }

                return _var;
            }
        }

        /// <summary>
        /// Obtener una variable de tipo Integer (Int16,Int32,Int64).
        /// </summary>
        /// <param name="SesssionVarName"> nombre de variable de sesión a obtener. </param>
        /// <returns> Int64? nullable o null sino existe. </returns>
        public static Int64? GetSessionInt(string SesssionVarName)
        {
            Int64? sessionVarInteger = Get<Int64>(SesssionVarName);
            return sessionVarInteger == null ? null : sessionVarInteger;
        }

        /// <summary>
        /// Obtener una variable de tipo string.
        /// </summary>
        /// <param name="SesssionVarName">nombre de variable de sesión a obtener.</param>
        /// <returns></returns>
        public static string GetSessionString(string SesssionVarName)
        {
            string sessionVarString = Get<string>(SesssionVarName);
            return sessionVarString == null ? string.Empty : sessionVarString;
        }

        /// <summary>
        /// Obtener el valor de una variable de session independiente de su tipo.
        /// </summary>
        /// <typeparam name="T"> Tipo de parametro de retorno, ej : Getstring("VarName"),GetInt64("VarName")  </typeparam>
        /// <param name="SesssionVarName"> nombre de variable de sesión a obtener.</param>
        /// <returns> Valor de variable de sesion o null sino existe.</returns>
        public static T Get<T>(string SesssionVarName)
        {
            if (Session[SesssionVarName] == null)
                return default(T);
            else
                return (T)Session[SesssionVarName];
        }

       
        /// <summary>
        /// Valida un string
        /// </summary>
        /// <param name="fieldName">Nombre del campo.</param>
        /// <param name="fieldData"> Datos del campo ej : txtNombre.text.</param>
        /// <param name="required"> ¿ El campo es requerido ? Opcional.</param>
        /// <param name="maxLength">Tamaño maximo del campo. Opcional.</param>
        /// <param name="validateValidEmail">if set to <c>true</c> [validate valid email].</param>
        /// <param name="zeroIsNull">if set to <c>true</c> [zero is null].</param>
        /// <param name="validateSpecialCharacters">if set to <c>true</c> [validate special characters].</param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public static void ValidateString(string fieldName ,string fieldData, bool required = true , int maxLength = 0, bool validateValidEmail = false, bool zeroIsNull = false, bool validateSpecialCharacters = false) 
        {
            StringBuilder messageError = new StringBuilder();

            if (String.IsNullOrEmpty(fieldData) && required)
                throw new ApplicationException(messageError.AppendFormat(" El campo {0} es obligatorio y no puede estar vacío.", fieldName).ToString());

            if (fieldData.Length > maxLength && maxLength != 0)
                throw new ApplicationException(messageError.AppendFormat("El {0} debe ser máximo de {1} caracteres.", fieldName,maxLength.ToString() ).ToString());
            
            if(!isValidEmail(fieldData) &&  validateValidEmail)
                throw new ApplicationException(messageError.AppendFormat(" El campo {0} no es un correo electrónico válido.", fieldName).ToString());

            if (zeroIsNull && fieldData.Equals("0"))
                throw new ApplicationException(messageError.AppendFormat(" El campo {0} es obligatorio.", fieldName).ToString());

            if (validateSpecialCharacters && HasSpecialChar(fieldData))
                throw new ApplicationException(messageError.AppendFormat(" El campo {0} no debe contener caracteres espciales.", fieldName).ToString());

        }

        /// <summary>
        /// Valida un campo numerico
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="fieldData">Datos del campo ej : txtValor.text</param>
        /// <param name="required">¿ El campo es requerido ?</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public static void ValidateNumeric( string fieldName ,string fieldData, bool required = true, Int64? maxValue = null)
        {
            StringBuilder messageError = new StringBuilder();

            if (String.IsNullOrEmpty(fieldData) && !required)
                return;

            if (String.IsNullOrEmpty(fieldData) && required)
                throw new ApplicationException(messageError.AppendFormat(" El campo {0} es obligatorio y no puede estar vacío.", fieldName).ToString());

            Int64 numero;
            if (!Int64.TryParse(fieldData.Replace(",", "").Replace(".", ""), out numero))
                throw new ApplicationException(messageError.AppendFormat(" El {0} debe ser numérico.", fieldName).ToString());
            
            if(maxValue != null && numero > maxValue)
                throw new ApplicationException(messageError.AppendFormat(" El {0} supera el maximo numero posible "+ Int32.MaxValue + ".", fieldName).ToString());
        }

        /// <summary>
        /// valida el formato money.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="ShowMoneySymbol">if set to <c>true</c> [show money symbol].</param>
        /// <returns></returns>
        public static string moneyFormat( Decimal valor, Boolean ShowMoneySymbol = true) 
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : ""; 
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        /// <summary>
        /// valida el formato money
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="ShowMoneySymbol">if set to <c>true</c> [show money symbol].</param>
        /// <returns></returns>
        public static string moneyFormat(Double valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        /// <summary>
        /// obtiene la fecha con formato.
        /// </summary>
        /// <param name="fechaSinFormato">fecha sin formato.</param>
        /// <param name="showTime">if set to <c>true</c> [show time].</param>
        /// <returns></returns>
        public static string getFechaConFormato(this DateTime fechaSinFormato, Boolean showTime = false) {

            string diaSemana = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fechaSinFormato.DayOfWeek);
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaSinFormato.Month);
            string hora = showTime ? " a las " + fechaSinFormato.ToString("hh:mm") + " " + fechaSinFormato.ToString("tt", CultureInfo.CurrentCulture) : string.Empty;
            string fechaConFormato = diaSemana + " " +  fechaSinFormato.Day + " de " + mes + " de " + fechaSinFormato.Year + hora;
            
            return fechaConFormato;
        }

        /// <summary>
        ///obtiene la fecha abreviada con formato.
        /// </summary>
        /// <param name="fechaSinFormato">fecha sin formato.</param>
        /// <param name="showTime">if set to <c>true</c> [show time].</param>
        /// <returns></returns>
        public static string getFechaAbreviadaConFormato(this DateTime fechaSinFormato, Boolean showTime = false)
        {
            string diaSemana = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fechaSinFormato.DayOfWeek);
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaSinFormato.Month);
            string hora = showTime ? " " + fechaSinFormato.ToString("hh:mm") + " " + fechaSinFormato.ToString("tt", CultureInfo.CurrentCulture) : string.Empty;

            string fechaConFormato = fechaSinFormato.Day + " " + mes + " " + fechaSinFormato.Year + hora;

            return fechaConFormato;
        }

        /// <summary>
        /// obtiene la fecha corta con formato.
        /// </summary>
        /// <param name="fechaSinFormato">The fecha sin formato.</param>
        /// <returns></returns>
        public static string getShortFechaConFormato( DateTime fechaSinFormato)
        {

            string diaSemana = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fechaSinFormato.DayOfWeek);
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaSinFormato.Month).Substring(0,3);
            string fechaConFormato = fechaSinFormato.Day + " de " + mes + " de " + fechaSinFormato.Year;

            return fechaConFormato;
        }

        /// <summary>
        /// obtiene la fecha corta.
        /// </summary>
        /// <param name="fechaSinFormato">fecha sin formato.</param>
        /// <returns></returns>
        public static string GetShotDateOnlyNumbers( DateTime fechaSinFormato)
        {

            var dia = fechaSinFormato.Day;
            var mes = fechaSinFormato.Month;
            var fechaConFormato = dia + "/" + mes + "/" + fechaSinFormato.Year;

            return fechaConFormato;
        }

        /// <summary>
        /// Gets the short date only numbers with underscore.
        /// </summary>
        /// <param name="fechaSinFormato">The fecha sin formato.</param>
        /// <returns></returns>
        public static string GetShortDateOnlyNumbersWithUnderscore( this DateTime fechaSinFormato)
        {

            var dia = fechaSinFormato.Day;
            var mes = fechaSinFormato.Month;
            var fechaConFormato = dia + "_" + mes + "_" + fechaSinFormato.Year + "_" + fechaSinFormato.ToString("hh_mm");

            return fechaConFormato;
        }

        /// <summary>
        /// Metodo que devuelvo un string con caracteres especiales de html
        /// codificado para guadarlo en la base de datos
        /// </summary>
        /// <param name="field"> String con caracteres especiales </param>
        /// <returns> String codificado para guardar en base de datos </returns>
        public static string htmlEncode(this string field)
        {
            return System.Net.WebUtility.HtmlEncode(field);
        }

        /// <summary>
        /// Metodo que devuelvo un string codificado con caracteres especiales de html
        /// </summary>
        /// <param name="field"> String con caracteres especiales </param>
        /// <returns> String codificado con caracteres especiales de html </returns>
        public static string htmlDecode(this string field)
        {
            return System.Net.WebUtility.HtmlDecode(field);
        }

        /// <summary>
        /// Valida si es un email valido
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>Si es o no un email valido</returns>
        public static bool isValidEmail(string email)
        {            
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Valida si una fecha es mayor a otra
        /// </summary>
        /// <param name="fieldNameDateMenor">Nombre campo fecha menor ej: Fecha de graduación</param>
        /// <param name="fechaMenor">Fecha menor</param>
        /// <param name="fieldNameDateMayor"> Nombre del campo fecha mayor ej : Fecha de hoy, Fecha de finalización </param>
        /// <param name="fechaMayor">Fecha mayor</param>
        public static void ValidateIsDateMayor(string fieldNameDateMenor, DateTime fechaMenor,string fieldNameDateMayor , DateTime fechaMayor)
        {
            StringBuilder messageError = new StringBuilder();
            
            if (isFechaMayor(fechaMenor,fechaMayor))
                throw new ApplicationException(messageError.AppendFormat("La {0} debe ser menor a {1} .", fieldNameDateMenor, fieldNameDateMayor).ToString());
        }

        /// <summary>
        /// Valida si una fecha es menor a otra fecha
        /// </summary>        
        /// <returns>Si es o no un email valido</returns>
        public static bool isFechaMayor(DateTime fechaMenor, DateTime fechaMayor)
        {
            return fechaMenor.Date > fechaMayor.Date;
        }

        
        /// <summary>
        /// Valida si una fecha control esta entre el rango de otras dos
        /// </summary>
        /// <param name="fieldNameDateMenor">Nombre campo fecha menor ej: Fecha de graduación.</param>
        /// <param name="fechaMenor">Fecha menor.</param>
        /// <param name="fieldNameDateValidar">Nombre del campo fecha mayor ej : Fecha de hoy, Fecha de finalización.</param>
        /// <param name="fechaAValidar">The fecha a validar.</param>
        /// <param name="fieldNameDateMayor">The field name date mayor.</param>
        /// <param name="fechaMayor">Fecha mayor.</param>
        /// <exception cref="ApplicationException"></exception>
        public static void ValidateIsFechaEntreRango(string fieldNameDateMenor, DateTime fechaMenor, string fieldNameDateValidar, DateTime fechaAValidar, string fieldNameDateMayor, DateTime fechaMayor)
        {
            StringBuilder messageError = new StringBuilder();

            if (!isFechaEntreRango(fechaMenor,fechaAValidar, fechaMayor))
                throw new ApplicationException(messageError.AppendFormat("La {0} debe ser mayor a {1} y menor que {2}", fieldNameDateValidar, fieldNameDateMenor, fieldNameDateMayor).ToString());
        }

        /// <summary>
        /// Valida si una fecha control esta entre el rango de otras dos 
        /// </summary>                
        public static bool isFechaEntreRango(DateTime fechaMenor,DateTime fechaAValidar , DateTime fechaMayor)
        {
            return (fechaAValidar.Date > fechaMenor.Date) && (fechaAValidar.Date < fechaMayor.Date);
        }

        /// <summary>
        /// Metodo de extensión para templazar palabras en un string
        /// </summary>                
        public static string ReplaceWord(this string text, string oldWord, string newWord)
        {
            return text.Replace(oldWord, newWord);
        }

        /// <summary>
        /// Elimina los caracteres especiales
        /// </summary>
        /// <param name="urlText">The URL text.</param>
        /// <param name="addDateIdentifier">if set to <c>true</c> [add date identifier].</param>
        /// <returns></returns>
        public static string cleanSpecialChar(this string urlText, Boolean addDateIdentifier = false)
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

        /// <summary>
        /// Reemplazar tildes
        /// </summary>
        /// <param name="texto">texto.</param>
        /// <returns></returns>
        public static string remplazarTilde(this string texto)
        {
            string result = texto.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            return result;
        }

        /// <summary>
        /// Remover acentos
        /// </summary>
        /// <param name="text">text.</param>
        /// <returns></returns>
        public static string RemoveAccent(this string text)
        { 
            string pattern = String.Join("|", dict.Keys.Select(k => k));
            string result = Regex.Replace(text, pattern, m => dict[m.Value]);

            return result;
        }

        /// <summary>
        /// Redirects the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="url">The URL.</param>
        /// <param name="target">The target.</param>
        /// <param name="windowFeatures">The window features.</param>
        /// <exception cref="InvalidOperationException">Cannot redirect to new window outside Page context.</exception>
        public static void Redirect(HttpResponse response, string url, string target, string windowFeatures)
        {

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }

        }

        /// <summary>
        /// Determines whether [has special character] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [has special character] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
    }
}