<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoGastoV2.ascx.cs"
    Inherits="Fonade.Controles.CatalogoGastoV2" %>
NUEVO GASTO

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
<table width='900px' border='0' cellspacing='0' cellpadding='3'>
    <tr valign="top">
        <td colspan="2">
            &nbsp;
        </td>
    </tr>
    <tr valign="top">
        <td width='167'  align="Right">
            <b>Descripción:</b>
        </td>
        <td >
                <asp:TextBox ID="txtDescripcion" MaxLength="255" Width="300px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="La descripción es requerida" 
                    ControlToValidate="txtDescripcion" Display="Dynamic" 
                    ValidationGroup="GrupoGastos"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr valign="top">
        <td align="Right">
            <b>Valor:</b>
        </td>
        <td align="left" colspan="3" >
            <asp:TextBox ID="txtValor" CssClass="money" Width="100px" runat="server" Text="0"></asp:TextBox>
        </td>
    </tr>
    <tr valign="top">
        <td colspan="4" align="center">
            <asp:HiddenField ID="hddCodProyecto" runat="server" Value="" />
            <asp:HiddenField ID="hddAccion" runat="server" Value="" />
            <asp:HiddenField ID="hddIdGasto" runat="server" Value="" />
            <asp:HiddenField ID="hddTipo" runat="server" Value="" />
            <asp:Button ID="btnGasto" runat="server" Text="Guardar" OnClick="btnGasto_Click" />
            <asp:Button ID="btnCancelarGasto" runat="server" Text="Cerrar" OnClick="btnCancelarGasto_Click" />
        </td>
    </tr>
</table>
