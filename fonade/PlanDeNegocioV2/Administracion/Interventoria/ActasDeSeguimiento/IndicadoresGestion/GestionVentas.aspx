<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="GestionVentas.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.GestionVentas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 296px;
        }

        .auto-style2 {
            width: 91px;
        }

        .auto-style3 {
            width: 647px;
        }

        .auto-style4 {
            width: 136px;
        }
    </style>

    <script type="text/javascript">
        function format(input) {
            var num = input.value.replace(/\./g, '');
            if (!isNaN(num) ||
                ((num.indexOf(',') != -1) && comas(num))) {
                num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
                num = num.split('').reverse().join('').replace(/^[\.]/, '');
                input.value = num;
            }

            else {
                alert('Solo se permiten numeros');
                //input.value = input.value.replace(/[^\d\.]*/g, '');
            }
        }

        function comas(entrada) {
            var contador = 0;
            for (var i = 0; i < entrada.length; i++)
                if (entrada[i] == ',')
                {
                    contador++;
                    if (contador == 2)
                        return false;
                }

            return true;
        }
     </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.6 Gestión en Ventas"></asp:Label>
            </h3>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">META VENTAS:
                <asp:Label ID="lblMetaVentas" runat="server" Text="0"></asp:Label></h1>
        </div>
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id"
                OnPageIndexChanging="gvIndicador_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valorFormato" HeaderText="valor" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                </Columns>
            </asp:GridView>
            <div style="text-align:center">
                *Se incluyen las ventas que estén facturadas, contabilizadas, declaradas y reportadas en la plataforma
            </div>
        </div>
        <hr />
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="background-color: #00468f; color: white;">
                    <td class="auto-style2">Visita</td>
                    <td class="auto-style4">Valor</td>
                    <td>Descripción</td>
                </tr>
                <tr>
                    <td class="auto-style2" style="text-align: center">
                        <asp:Label ID="lblnumVisita" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtCantidad" runat="server" Style="text-align: center"
                           MaxLength="12" required onkeyup="format(this)"></asp:TextBox>    
                      <!-- <input type="text" onkeyup="format(this)" onchange="format(this)">-->
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescripcionEvento" runat="server" required TextMode="MultiLine"
                            MaxLength="10000" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="3">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
