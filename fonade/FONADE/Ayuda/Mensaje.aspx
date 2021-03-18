<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mensaje.aspx.cs" Inherits="Fonade.Ayuda.Mensaje" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #L_Mensaje
        {
            background-color: Blue;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="L_Mensaje" runat="server" Text="" Width="100%" ForeColor="White" Font-Bold="true"></asp:Label>
        <br />
        <asp:Panel ID="P_mensaje1" runat="server" Visible="false">
            <p>
                Defina cantidades de ventas por periodo (mensualmente, trimestralmente, o semestralmente,
                el primer año y los totales por año, para el periodo de evaluación del negocio (este
                periodo es variable acorde con la naturaleza del negocio), teniendo en cuenta las
                demandas estacionales en caso de presentarse, así como los aumentos. Determine los
                ingresos (en $) de acuerdo a la estrategia de precio y a la proyección de unidades
                vendidas. Cada producto debe tener asociado la posición arancelaria de Importación
                /Exportación. La proyección de ventas debe ser producto de un análisis en el que
                se haya utilizado un método de proyección como los relacionados en la caja de selección
                que ofrece el sistema. Explique cual es la fuente de los datos históricos y las
                razones por las que se utilizó el método seleccionado.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje2" runat="server" Visible="false">
            <p>
                TXT_JustificaProyeccion
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje3" runat="server" Visible="false">
            <p>
                Defina si dadas las condiciones del mercado se requiere otorgar crédito a los clientes.
                En caso afirmativo establecer los plazos de la cartera y los porcentajes respecto
                del valor de la venta.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje4" runat="server" Visible="false">
            <p>
                Defina los indicadores de gestión que mas se ajusten al plan de negocio. Los indicadores
                de gestión son la herramienta mas idonea para evaluar el desarrollo del proyecto,
                es el insumo necesario para hacer seguimiento. Un indicador de gestión es por lo
                general una fracción. Es la comparación de una cifra sobre otra de la misma naturaleza,
                por ejemplo, Inversión realizada a X fecha sobre Inversión programada para la misma
                fecha. Se debe crear indicadores de gestión para las variables mas relevantes. No
                es recomendable crear indicadores de gestión para todas las variables.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje5" runat="server" Visible="false">
            <p>
                Presenta para cada uno de los productos, sus componentes de materia prima ó insumos,
                la cantidad de producto ó servicio a producir mensualmente y el costo de la misma.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje6" runat="server" Visible="false">
            <p>
                Presenta para cada uno de los productos ó servicios las ventas esperadas y el ingreso
                por concepto de las misma en forma mensual.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje7" runat="server" Visible="false">
            <p>
                TXT_ConceptosJustificacion
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje8" runat="server" Visible="false">
            <p>
                De manera concreta, pero considerando los aspectos mas relevantes del plan de negocio
                y de la evaluación de este, registre su concepto general. Defina los compromisos
                que debe cumplir el emprendedor y, de ser necesario, fije claramente las condiciones
                que debe cumplir el emprendedor para la entrega de los recursos.
            </p>
        </asp:Panel>
        <asp:Panel ID="P_mensaje9" runat="server" Visible="false">
            <p>
                Basado en el Plan de Producción, identifique, describa y justifique la cantidad
                de cada insumo que se requiere para producir una unidad de producto, registrando
                la información requerida por el sistema.
            </p>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
