<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="EstadoEmpresa.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.EstadoEmpresa.EstadoEmpresa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estado Empresa</title>
     <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server" 
                    Text="7. ESTADO DE LA EMPRESA"></asp:Label>
            </h1>
        </div>
        <asp:GridView ID="gvEstadoEmpresa" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvEstadoEmpresa_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />                     
                </Columns>
            </asp:GridView>
           <div>
                <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style1">Visita</td>                                       
                     <td style="text-align:center">Descripción</td>
                 </tr>
                 <tr>
                     <td class="auto-style1" style="text-align:center">
                        <asp:Label ID="lblnumV" runat="server" Text=""></asp:Label>
                     </td>                                       
                     <td>
                        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="97%" required></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="3">
                         <asp:Button ID="btnGuardar" runat="server" Height="30px" Text="Guardar" OnClick="btnGuardar_Click" />
                     </td>
                 </tr>
             </table> 
            </div>
    </form>
</body>
</html>
