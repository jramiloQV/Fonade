<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Producto.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Solucion.Producto" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Fondo emprender - Ficha técnica </title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>   
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>    
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px !important;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .parentContainer {
            width: 100%;
            height: 650px;
            overflow-x: hidden;
            overflow-y: visible;
        }

        .childContainer {
            width: 100%;
            height: auto;
        }

        html, body, div, iframe {            
        }
    </style>      
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        
        <table style="width:1000px; border=0;" cellspacing='0' cellpadding='3'>
            <tr style="vertical-align:top">
                <td width="350px;" align="right">
                </td>
                <td >
                    <h1>
                        <asp:Label Text="Adicionar producto" runat="server" ID="lblCreate" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />  
                        <asp:Label Text="Actualizar producto" runat="server" ID="lblUpdate" Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />                             
                    </h1>
                </td>               
            </tr>   
                                             
            <tr style="vertical-align:top">
                <td align="right">
                    <b>Producto específico:</b>
                    <br />
                    <label> (Denominación común del bien o servico) </label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombreProducto" runat="server" Width="300" />
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b>Nombre comercial:</b>
                    <br />
                    <label> (Denominación comercial que se propone) </label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombreComercial" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b>Unidad de medida :</b>
                    <br />
                    <label> (Unidad de medida a través de la cual se comercializará el bien o servicio a ofrecer, Ejemplo : Kilogramo, toneladas, paquete de 12 unidades, horas de consultoría,ect.  ) </label>
                </td>
                <td>
                    <asp:TextBox ID="txtUnidadMedida" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b>Descripción general :</b>
                    <br />
                    <label> (Descripción de las características técnicas del bien o servicio) </label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcionGeneral" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b> Condiciones especiales :</b>
                    <br />
                    <label> 
                        (Describa las advertencias o condiciones especiales de almacenamiento o uso del producto / servicio)
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="txtCondicionesEspeciales" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b> Composición :</b>
                    <br />
                    <label> 
                        (Descripción de la composición del producto)
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="txtComposicion" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td align="right">
                    <b>  Otros, ¿ Cuál ? :</b>
                    <br />
                    <label> 
                        (Campo de texto no obligatorio, de longitud máxima 250 caracteres, tipo de dato alfanumérico y permite el ingreso de caracteres especiales.)
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="txtOtros" runat="server" Height="50" Width="300" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr style="vertical-align:top">
                <td colspan="2">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" >Sucedio un error inesperado, intentalo de nuevo.</asp:Label>
                </td>
            </tr>
            <tr style="vertical-align:top">
                <td>
                </td>
                <td >
                    <asp:Button ID="btnCreate" runat="server" Text="Guardar" align="center" OnClick="btnCreate_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
                    <asp:Button ID="btnUpdate" runat="server" Text="Guardar" align="center" OnClick="btnUpdate_Click" Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" align="center" OnClick="btnCancel_Click" Visible="false" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
