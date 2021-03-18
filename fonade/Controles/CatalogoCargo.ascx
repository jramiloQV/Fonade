<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoCargo.ascx.cs"
    Inherits="Fonade.Controles.CatalogoCargo" %>
NUEVO CARGO
<script type="text/javascript" src="/../Scripts/jquery-1.7.2.min.js"></script>
<script type="text/javascript" src="/../Scripts/jquery.number.min.js"></script>
<script>        
    function validarNro(e) {
        var key = e.which
        switch (true) {
            case key <= 44:
                e.preventDefault();
                break
            case key == 13:
                e.preventDefault();
            case key > 57:
                e.preventDefault();
        }
    };
    
    $(function () {
        $('.money').number(true, 2);
    });
</script>
<table width='1000px' border='0' cellspacing='0' cellpadding='3'>
    <tr valign="top">
        <td width='167' align="Right">
            <b>Cargo:</b>
        </td>
        <td>
            <asp:TextBox ID="txtCargo" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtCargo" Display="Dynamic" 
                ErrorMessage="El cargo del usuario es requerido" 
                ValidationGroup="ValidadorCargo"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Dedicación:</b>
        </td>
        <td align="left" class="TitDestacado">
            <asp:DropDownList ID="ddlDedicacion" runat="server" AppendDataBoundItems="True">
                <asp:ListItem Selected="True"></asp:ListItem>
                <asp:ListItem Value="Completa" Text="Completa"></asp:ListItem>
                <asp:ListItem Value="Parcial" Text="Parcial"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Tipo de Contratación:</b>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlTipoContratacion" runat="server" AppendDataBoundItems="True">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem Value="Fija" Text="Fija"></asp:ListItem>
                <asp:ListItem Value="Temporal" Text="Temporal"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Valor Mensual:</b>
        </td>
        <td align="left">
            <asp:TextBox ID="txtValorMensual" runat="server" CssClass="money" Width="119px" Text="0" Height="16px"></asp:TextBox>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Valor Anual:</b>
        </td>
        <td align="left" class="TitDestacado">
            <asp:TextBox ID="txtValorAnual" runat="server" Width="118px" CssClass="money" Text="0" Height="16px"></asp:TextBox>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Otros Gastos:</b>
        </td>
        <td align="left">
            <asp:TextBox ID="txtOtrosGastos" runat="server" MaxLength="20" CssClass="money" Width="117px" Text="0" Height="16px"></asp:TextBox>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Observación:</b>
        </td>
        <td>
            <asp:TextBox ID="txtObservacion" runat="server" MaxLength="20" Width="410px" TextMode="MultiLine"
                Height="100px"></asp:TextBox>
        </td>
    </tr>
    <tr valign="top">
        <td colspan="2" align="center">
            <asp:HiddenField ID="hddCodProyecto" runat="server" Value="" />
            <asp:HiddenField ID="hddAccion" runat="server" Value="" />
            <asp:HiddenField ID="hddIdCargo" runat="server" Value="" />
            <asp:Button ID="btnCargo" runat="server" Text="Crear" OnClick="btnCargo_Click" 
                ValidationGroup="ValidadorCargo" />
            <asp:Button ID="btnCancelarCargo" runat="server" Text="Cerrar" OnClick="btnCancelarCargo_Click" />
        </td>
    </tr>
</table>
