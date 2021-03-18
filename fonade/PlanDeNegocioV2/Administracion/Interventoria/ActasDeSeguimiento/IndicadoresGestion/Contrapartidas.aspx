<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="Contrapartidas.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.Contrapartidas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .auto-style9 {
            height: 22px;
            width: 192px;
        }
        .auto-style13 {
            height: 22px;
            width: 11px;
        }
        .auto-style15 {
            height: 22px;
        }
        .auto-style16 {
            height: 22px;
            width: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.4 Contrapartidas"></asp:Label>
            </h3>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">META CONTRAPARTIDAS: <asp:Label ID="lblContrapartidas" runat="server" Text="0"></asp:Label></h1>
        </div>
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvIndicador_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="cantContrapartida" HeaderText="Cantidad Contrapartidas" />
                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />                   
                </Columns>
            </asp:GridView>
             <div style="text-align: center">*Solo se incluyen las contrapartidas con sus respectivos avales subidos en plataforma.
 En la descripción se pueden enunciar los que presenten evidencias y los que están pendientes de avales.
            </div>
        </div>
        <hr />
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <table class="auto-style2">
                <tr>
                    <td class="auto-style13">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style16"></td>
                    <td class="auto-style9"></td>
                </tr>
                <tr style="background-color: #00468f;color: white;">
                    <td class="auto-style13">Visita</td>
                    <td class="auto-style16">Cantidad Contrapartidas</td>
                    <td class="auto-style9">Descripción</td>
                </tr>
                <tr>
                    <td class="auto-style13" style="text-align:center">
                        <asp:Label ID="lblnumVisita" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style16" style="text-align:center">
                        <asp:TextBox ID="txtContrapartidas" runat="server" style="text-align:center"
                            type="number" MaxLength="4" min="0" pattern="^[0-9]+" required></asp:TextBox>
                    </td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txtDescripcion" runat="server" required TextMode="MultiLine"
                            MaxLength ="10000" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:center">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
