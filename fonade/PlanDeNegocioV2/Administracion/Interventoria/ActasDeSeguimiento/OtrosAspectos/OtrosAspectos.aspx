<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="OtrosAspectos.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.OtrosAspectos.OtrosAspectos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Otros Aspectos</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 286px;
        }
        .auto-style3 {
            width: 142px;
        }
        .auto-style4 {
            width: 38px;
        }
        .auto-style5 {
            width: 91px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server" 
                    Text="5. OTROS ASPECTOS DEL PLAN DE NEGOCIO"></asp:Label>
            </h1>
        </div>
        <div>
            <h3>5.1 Componente Innovador</h3>
        </div>
        <div style="text-align:center">
            <h2 style="padding-left: 0px;">Descripción Componente Innovador</h2>
        </div>
        <asp:Panel ID="pnlInfoInnovador" runat="server">
            <div style="text-align:  center;">
                <asp:TextBox ID="txtDescripcionInnovador" runat="server" TextMode="MultiLine"
                    style="height: 70px;width: 500px;">
                </asp:TextBox>
            </div>    
            
        </asp:Panel>
        <asp:Panel ID="pnlGridInnovador" runat="server">
            <asp:GridView ID="gvComponenteInnovador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                OnPageIndexChanging="gvComponenteInnovador_PageIndexChanging"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valoracion" HeaderText="Si/No/Parcial" /> 
                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />                   
                </Columns>
            </asp:GridView> 
            <div>
                <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style4">Visita</td>
                     <td class="auto-style5">Si/No/Parcial</td>
                    
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td class="auto-style4" style="text-align:center">
                        <asp:Label ID="lblnumV" runat="server" Text=""></asp:Label>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlValoracionInnovador" runat="server">
                         </asp:DropDownList>
                     </td>                     
                     <td>
                        <asp:TextBox ID="txtObserInnovador" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="97%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="3">
                         <asp:Button ID="btnSaveValorInnovador" runat="server" Height="30px" Text="Guardar" OnClick="btnSaveValorInnovador_Click" />
                     </td>
                 </tr>
             </table> 
            </div>
        </asp:Panel>
         <hr />
        <div>
            <h3>5.2 Componente Ambiental</h3>
        </div>
        <div style="text-align:center">
            <h2 style="padding-left: 0px;">Descripción Componente Ambiental</h2>
        </div>
        <asp:Panel ID="pnlInfoAmbiental" runat="server">
            <div style="text-align:  center;">
                <asp:TextBox ID="txtDescripcionAmbiental" runat="server" TextMode="MultiLine"
                    style="height: 70px;width: 500px;">
                </asp:TextBox>
            </div>            
        </asp:Panel>
        <asp:Panel ID="pnlGridAmbiental" runat="server">
            <asp:GridView ID="gvComponenteAmbiental" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                OnPageIndexChanging="gvComponenteAmbiental_PageIndexChanging"
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valoracion" HeaderText="Si/No/Parcial" /> 
                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />                   
                </Columns>
            </asp:GridView> 
            <div>
                <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style4">Visita</td>
                     <td class="auto-style5">Si/No/Parcial</td>
                    
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td class="auto-style4" style="text-align:center">
                        <asp:Label ID="lblNumVA" runat="server" Text=""></asp:Label>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlValoracionAmbiental" runat="server">
                         </asp:DropDownList>
                     </td>                     
                     <td>
                        <asp:TextBox ID="txtObservAmbiental" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="97%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="3">
                         <asp:Button ID="btnSaveValorAmbiental" runat="server" Height="30px" Text="Guardar" OnClick="btnSaveValorAmbiental_Click"/>
                     </td>
                 </tr>
             </table> 
            </div>
        </asp:Panel>
        <hr />
        <div style="text-align:center">
                <asp:Button ID="btnGuardarDescripciones" runat="server" 
                    Text="Guardar" Height="29px" OnClick="btnGuardarDescripciones_Click" />
            </div>
       
    </form>
</body>
</html>
