using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class IndicadorActasSeguimModelDTO
    {
        public int Visita { get; set; }
        public int Cargos { get; set; }
        public double Presupuesto { get; set; }
        public String PresupuestoWithFormat
        {
            get
            {
                return moneyFormat(Presupuesto, true);
            }
            set
            {
            }
        }

        public int Mercadeo { get; set; }
        public double Idh { get; set; }
        public int Contrapartidas { get; set; }
        public string Produccion { get; set; }
        public Decimal Ventas { get; set; }
        public String VentasWithFormat
        {
            get
            {
                return moneyFormat(Ventas, true);
            }
            set
            {
            }
        }

        public static string moneyFormat(Double valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }

        public static string moneyFormat(Decimal valor, Boolean ShowMoneySymbol = true)
        {
            String moneySymbol = ShowMoneySymbol ? "$ " : "";
            String valorFormateado = valor.ToString("0,0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");

            return !String.IsNullOrEmpty(valorFormateado) ? moneySymbol + valorFormateado : "0";
        }
    }
}
