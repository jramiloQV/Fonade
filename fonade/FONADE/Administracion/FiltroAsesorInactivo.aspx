<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="FiltroAsesorInactivo.aspx.cs" Inherits="Fonade.FONADE.Administracion.FiltroAsesorInactivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="ASESORES INACTIVOS - BUSCAR" />
    </h1>
    <%--<p>
        <asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
            Width="35%" Text="BUSCAR USUARIO" />
        <asp:Label ID="lbl_Interventor" runat="server" Width="40%" />
        <asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
    </p>--%>
    <asp:Panel ID="pnlPrincipal" runat="server">
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
                    <asp:Label ID="Label4" Text="Identificación:" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_Identificacion" runat="server" onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="text-align: center;">
                        <asp:Button ID="btn_Buscar" Text="Buscar" runat="server" OnClick="btn_Buscar_Click" />
                    </div>
                </td>
            </tr>
        </table>
        <%--Grilla // Terminar.--%>
        <asp:GridView ID="gv_ResultadosBusqueda" runat="server" AutoGenerateColumns="false"
            OnRowCommand="gv_ResultadosBusqueda_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Id_Contacto" DataField="Id_Contacto" Visible="false" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn_Nombres" runat="server" ForeColor="Red" CausesValidation="False"
                            CommandArgument='<%# Eval("Id_Contacto") %>' CommandName="mostrar" Text='<%#Eval("Nombres")%>'
                            Style="text-decoration: none;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn_Apellidos" runat="server" ForeColor="Red" CausesValidation="False"
                            CommandArgument='<%# Eval("Id_Contacto") %>' CommandName="mostrar" Text='<%#Eval("Apellidos")%>'
                            Style="text-decoration: none;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn_Email" runat="server" ForeColor="Red" CausesValidation="False"
                            CommandArgument='<%# Eval("Id_Contacto") %>' CommandName="mostrar" Text='<%#Eval("Email")%>'
                            Style="text-decoration: none;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn_Identificacion" runat="server" ForeColor="Red" CausesValidation="False"
                            CommandArgument='<%# Eval("Id_Contacto") %>' CommandName="mostrar" Text='<%#Eval("Identificacion")%>'
                            Style="text-decoration: none;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtn_NomInstitucion" runat="server" ForeColor="Red" CausesValidation="False"
                            CommandArgument='<%# Eval("Id_Contacto") %>' CommandName="mostrar" Text='<%#Eval("NomInstitucion")%>'
                            Style="text-decoration: none;" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
