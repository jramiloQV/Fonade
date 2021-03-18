<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FiltrosUsuario2.aspx.cs" Inherits="Fonade.FONADE.Administracion.FiltrosUsuario2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #bodyContentPlace_gv_ResultadosBusqueda{
            width:100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="BUSCAR USUARIO" />
    </h1>

    <asp:Panel ID="pnlPrincipal" runat="server" DefaultButton="btn_Buscar">
        <table>
            <tr>
                <td style="width: 16%; height: 46%;" valign="top">
                    <asp:Label ID="Label1" Text="Nombre(s):" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_Nombres" runat="server" />
                </td>
                <td style="width: 15%; height: 46%;" valign="top">
                    <asp:Label ID="Label2" Text="Apellido(s):" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_Apellidos" runat="server" />
                </td>
                <td style="width: 13%; height: 46%;" valign="top">
                    <asp:Label ID="Label3" Text="Email:" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_Email" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 16%; height: 46%;" valign="top">
                    <asp:Label ID="Label4" Text="Identificación:" runat="server"  />
                    <br />
                    <asp:TextBox ID="txt_Identificacion" runat="server" onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
                </td>
                <td style="width: 15%; height: 46%;" valign="top">
                    <asp:Label ID="Label5" Text="Nombre Plan:" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_NombrePlan" runat="server" />
                </td>
                <td style="width: 15%; height: 46%;" valign="top">
                    <asp:Label ID="Label6" Text="Número Plan:" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_NumeroPlan" runat="server" />
                </td>
            </tr>
        </table>
        <div style="width:100%">
           <div style="text-align: right;">
              <asp:Button ID="btn_Buscar" Text="Buscar" runat="server" OnClick="btn_Buscar_Click" />
           </div>
        </div>
        <div style="vertical-align: bottom; width: 100%; height: 100%; overflow: scroll; display: block">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Grilla" DataKeyNames="Id_Contacto" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ViewStateMode="Enabled" Width="90%">
                <Columns>
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/editar.png" SelectText="" ShowSelectButton="True" />
                    <asp:BoundField DataField="Id_Contacto" HeaderText="Codigo" InsertVisible="False" ReadOnly="True" SortExpression="Id_Contacto" />
                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" SortExpression="Nombres" />
                    <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" SortExpression="Apellidos" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" SortExpression="Identificacion" />
                    <asp:BoundField DataField="NomProyecto" HeaderText="Proyecto" ReadOnly="True" SortExpression="NomProyecto" />
                    <asp:BoundField DataField="NomGrupo" HeaderText="Rol" SortExpression="NomGrupo" />
                    <asp:BoundField DataField="Id_Proyecto" HeaderText="Id_Proyecto" ReadOnly="True" SortExpression="Id_Proyecto" Visible="False" />
                    <asp:BoundField DataField="Id_Grupo" HeaderText="Id_Grupo" ReadOnly="True" SortExpression="Id_Grupo" Visible="False" />
                </Columns>
            </asp:GridView>
        </div>
        <%--Grilla // Terminar.--%>
    </asp:Panel>
</asp:Content>
