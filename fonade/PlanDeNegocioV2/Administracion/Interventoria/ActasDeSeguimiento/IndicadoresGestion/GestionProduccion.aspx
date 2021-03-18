<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="GestionProduccion.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.GestionProduccion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 326px;
        }
        .auto-style2 {
            width: 177px;
        }
        .auto-style3 {
            width: 97px;
        }
        .auto-style4 {
            width: 133px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.5 Gestión en Producción"></asp:Label>
            </h3>
        </div>
        <div>
            <h2>Producto mas representativo: <asp:Label ID="lblProductoMasRep" runat="server" Text="Producto"></asp:Label></h2>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">METAS</h1>
        </div>
         <div>
            <asp:GridView ID="gvMetasProduccion" runat="server"
                AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="Id_Producto"
                OnPageIndexChanging="gvMetasProduccion_PageIndexChanging"
                AllowPaging="True" ForeColor="#666666" Width="100%"
                EmptyDataText="No hay datos para mostrar.">
                <Columns>
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />                    
                    <asp:BoundField DataField="NomProducto" HeaderText="Producto o Servicio" />
                </Columns>
            </asp:GridView>
             <asp:Label ID="lblUnidadMedida" runat="server" Text="" Visible="false"></asp:Label>            
             <div style="text-align: center">*Meta propuesta con el producto más Representativo/Significativo
            </div>
        </div>
         <hr />
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                OnPageIndexChanging="gvIndicador_PageIndexChanging"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Visita" HeaderText="Visita" />
                    <asp:BoundField DataField="cantidadMedida" HeaderText="Cantidad" />                    
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                </Columns>
            </asp:GridView>
            <div style="text-align:center">Tabla del indicador de cumplimiento de la meta propuesta</div>
        </div>
        <hr />
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="background-color: #00468f;color: white;">
                    <td class="auto-style3">Visita</td>
                    <td class="auto-style2">Cantidad</td>
                    <td class="auto-style4">Unidad de Medida</td>
                    <td>Descripción</td>
                </tr>
                <tr>
                    <td class="auto-style3" style="text-align:center">
                        <asp:Label ID="lblnumVisita" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style2" >
                        <asp:TextBox ID="txtCantidad" runat="server" style="text-align:center"
                            type="number" MaxLength="4" min="0" pattern="^[0-9]+" required></asp:TextBox>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblMedida" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="500" required 
                            TextMode="MultiLine" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:center">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</body>
</html>
