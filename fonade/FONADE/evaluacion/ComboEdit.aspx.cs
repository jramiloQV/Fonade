using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class ComboEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
            else 
            {
                alternateToAction();
            }
           
        }
        private void alternateToAction()
        {
            switch (hidCombo.Value)
            {
                case "0":
                   CargPosiscionarancelaria();

                    break;
                case "1":
                    Grabar();
                    break;
            }
        }

        /// <summary>
        /// cargar posicion arancelaria
        /// </summary>
        private void CargPosiscionarancelaria()
        {
            cb_identifier.Value = "Se cargara la posición arancelaria";
            StringBuilder ListaElementos = new StringBuilder();
            ListaElementos.Append("<a>8205409000 Los demás destornilladores, de metales comunes.</a>");
            ListaElementos.Append("<a>1512210000 Aceites de algodón en bruto, incluso sin el gosipol.</a>");
            ListaElementos.Append("<a>2402201000 Cigarrillos de tabaco negro.</a>");
            ListaElementos.Append("<a>3904109000 Los demás policloruros de vinilo.</a>");
            ListaElementos.Append("<a>3905190000 Los demás polímeros de acetato de vinilo</a>");
            ListaElementos.Append("<a>2929903000 Ciclamato de sodio (DCI).</a>");
            hidCombo.Value = "1";
            cmbArancelaria.InnerHtml=ListaElementos.ToString();
        }
        private void Grabar()
        {
                  cb_identifier.Value = "Se hizo clic en el boton guardar";
                  hidCombo.Value = "0";
        }
    }
}