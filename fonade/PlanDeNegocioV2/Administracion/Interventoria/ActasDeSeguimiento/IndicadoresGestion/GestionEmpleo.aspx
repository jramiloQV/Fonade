<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="GestionEmpleo.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.GestionEmpleo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 158px;
        }
        .auto-style2 {
            width: 110px;
        }
    </style>


        

</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.1 Gestión en la Generación de Empleo"></asp:Label>
            </h3>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">METAS</h1>
        </div>
        <div>
            <asp:GridView ID="gvMetasEmpleos" runat="server"
                AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="codCargo"
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvMetasEmpleos_PageIndexChanging"
                EmptyDataText="No hay datos para mostrar.">
                <Columns>
                    <asp:BoundField DataField="Unidades" HeaderText="Cantidad" />
                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                    <asp:BoundField DataField="Condicion" HeaderText="Condicion" />
                </Columns>
            </asp:GridView>
        </div>

        <div style="text-align: center;">
            <h2 style="color: #00409b">META TOTAL DE EMPLEOS A GENERAR: 
                <asp:Label ID="lblMetaTotalEmpleos" runat="server" Text="0"></asp:Label></h2>
        </div>
        <hr />
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False" 
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvIndicador_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Visita" HeaderText="Visita" />
                    <asp:BoundField DataField="verificaIndicador" HeaderText="Verificacion" />
                    <asp:BoundField DataField="desarrolloIndicador" HeaderText="Desarollo" />
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="background-color: #00468f;color: white;">
                    <td class="auto-style1">Visita</td>
                    <td class="auto-style2">Verificación</td>
                    <td>Desarrollo</td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align:center">
                        <asp:Label ID="lblnumVisita" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style2" >
                        <asp:TextBox ID="txtVerificacion" runat="server" style="text-align:center"
                            type="number" MaxLength="4" min="0" pattern="^[0-9]+" required></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesarrollo" runat="server" required TextMode="MultiLine"
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
