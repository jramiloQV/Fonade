<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="NotificacionesEnviadas.aspx.cs" Inherits="Fonade.FONADE.evaluacion.NotificacionesEnviadas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
  <table style="width: 60%" align="center">
        <thead>
            <tr>
                <th style="background-color:#00468f; text-align:left">
                    <asp:Label ID="titulo" runat="server" ForeColor="White" Text="NOTIFICACION ENVIADAS"/>
                </th>
                <th style="background-color:#00468f; text-align:left">
                    <asp:Label ID="lfecha" runat="server" ForeColor="White"/>
                </th>
            </tr>
        </thead>
        <tbody>
        <tr align="center">
          <td align="center" colspan="2">
                
                <asp:GridView ID="GrvNotificaciones" Width="100%" runat="server" 
                    AutoGenerateColumns="False"  AllowPaging="True" CssClass="Grilla" EmptyDataText="NO EXISTEN NOTIFICACIONES"
                    onrowdatabound="GrvNotificaciones_RowDataBound" 
                    onpageindexchanging="GrvNotificaciones_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NOMBRE" HeaderText="Emprendedor"  />
                         <asp:BoundField DataField="NOMCC" HeaderText="Emprendedores CC" />
                         <asp:TemplateField HeaderText="Rol">
                             <ItemTemplate>
                                 <asp:Label ID="lrol" runat="server" Text='<%# Bind("ROL") %>'/>
                             </ItemTemplate>
                        </asp:TemplateField>
                          <asp:BoundField DataField="Email" HeaderText="Mensaje" />
                           <asp:BoundField DataField="NOMESTADO" HeaderText="Estado" />
                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" />
                    </Columns>
                </asp:GridView>
               </td>
        </tr>
         <tr>
          <td align="right" colspan="2">
              <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" 
                  onclick="btnCerrar_Click" />
          </td>
        </tr>
        </tbody>
    </table>
</asp:Content>
