<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="Notas.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Notas.Notas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notas</title>
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
                    Text="NOTAS"></asp:Label>
            </h1>
        </div>
        <asp:GridView ID="gvNotas" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se han registrado notas."
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvNota_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="Notas" HeaderText="Nota" />                     
                </Columns>
            </asp:GridView>
           <div>
                <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style1">Visita</td>                                       
                     <td style="text-align:center">Nota</td>
                 </tr>
                 <tr>
                     <td class="auto-style1" style="text-align:center">
                        <asp:Label ID="lblnumV" runat="server" Text=""></asp:Label>
                     </td>                                       
                     <td>
                        <asp:TextBox ID="txtNota" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="97%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="3">
                         <asp:Button ID="btnGuardar" runat="server" Height="30px" 
                             Text="Guardar" OnClick="btnGuardar_Click" />
                     </td>
                 </tr>
             </table> 
            </div>
    </form>
</body>
</html>
