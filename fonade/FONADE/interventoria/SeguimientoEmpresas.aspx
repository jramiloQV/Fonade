<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SeguimientoEmpresas.aspx.cs" Inherits="Fonade.FONADE.interventoria.SeguimientoEmpresas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
     <table class="style10">
        <tr>
            <td>
            <h1>
                <asp:Label ID="lbltitulo" runat="server"  style="font-weight: 700">Empresas</asp:Label>
                </h1>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
        
        <asp:GridView ID="GrvEmpresas" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador" PageSize="50"
          OnSorting="GrvEmpresasSorting"  OnRowCommand="GrvEmpresasRowCommand" 
            OnPageIndexChanging="GrvEmpresasPageIndexChanging" >
            <Columns>
               
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("id_proyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plan de Negocio" SortExpression="NomProyecto" >
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkproyecto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_proyecto") + ";" + Eval("Id_Empresa")  %>' 
                            CommandName="frameset" Text='<%# Eval("NomProyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Empresa" SortExpression="RazonSocial" >
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEmpresa" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_proyecto") + ";" + Eval("Id_Empresa")%>' 
                            CommandName="empresa" Text='<%# Eval("RazonSocial") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
        <br/>
     
        <br/>
     
    </asp:Panel>
    <br />

</asp:Content>
