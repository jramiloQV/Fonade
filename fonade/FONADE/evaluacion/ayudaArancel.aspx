<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ayudaArancel.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ayudaArancel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table class="auto-style1">
        <tr>
            <td align="center">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Ayuda de Posición Arancelaria"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center"><span style="color: rgb(51, 51, 51); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: -webkit-center; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; display: inline !important; float: none;">Ingrese palabra clave a buscar y a continuación haga click<span class="Apple-converted-space">&nbsp;</span></span><br style="color: rgb(51, 51, 51); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: -webkit-center; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px;" />
                <span style="color: rgb(51, 51, 51); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: -webkit-center; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; display: inline !important; float: none;">sobre el código deseado.</span></td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="TB_Codigo" runat="server" Width="235px"></asp:TextBox>
                <asp:LinkButton ID="lnk_buscar" runat="server" OnClick="lnk_buscar_Click">Buscar</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:RadioButtonList ID="RB_Buscar" runat="server">
                    <asp:ListItem Value="1" Selected="True">Número Posición</asp:ListItem>
                    <asp:ListItem Value="2">Descripción</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Descripcion"  CssClass="Grilla" AllowPaging="true" PageSize="20"   OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo">
                            <ItemTemplate>
                                <asp:LinkButton ID="HL_Direccionar" runat="server" Text='<%# Eval("PosicionArancelaria") %>' OnClick="HL_Direccionar_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
