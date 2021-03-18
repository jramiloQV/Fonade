<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoFormalizar.aspx.cs" Inherits="Fonade.FONADE.Proyecto.ProyectoFormalizar"  MasterPageFile="~/Master.Master" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    
    <asp:LinqDataSource ID="lds_proyectos" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_proyectos_Selecting" >
    </asp:LinqDataSource>
    <td class="style50"><h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
    <asp:GridView ID="GridViewProyectos"  CssClass="Grilla" runat="server" DataSourceID="lds_proyectos" OnRowCommand="GridViewProyectos_RowCommand"
        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" OnRowDataBound="GridViewProyectos_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Nombre"  SortExpression="nomproyecto">
                <ItemTemplate>
                    <%--<asp:HyperLink ID="hl_proyecto" runat="server" NavigateUrl='<%# "ProyectoFrameSet.aspx?CodProyecto=" + Eval("id_proyecto") %>'
                        ></asp:HyperLink>--%>
                    <asp:Button ID="btnproyecto" runat="server" CommandArgument='<%# Eval("id_proyecto") %>' Text='<%# Eval("nomproyecto") %>' CommandName="Proyecto" CssClass="boton_Link_Grid" />
                 </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ciudad" SortExpression="Lugar">
                <ItemTemplate>
                    <asp:HyperLink ID="hl_ciudad" runat="server" Text='<%# Eval("lugar") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Convocatoria" SortExpression="nomconvocatoria">
                <ItemTemplate>
                    <asp:HyperLink ID="hl_convocatoria" runat="server" Text='<%# Eval("nomconvocatoria") %>'></asp:HyperLink>
                </ItemTemplate> 
            </asp:TemplateField>

            <asp:TemplateField HeaderText=" ">
                <ItemTemplate>
                    <%--<asp:HyperLink ID="hl_adicional" runat="server"  NavigateUrl='<%# "" + Eval("URL") +  Eval("id_proyecto")%>'></asp:HyperLink>--%>
                    <asp:Button ID="btnadicional" runat="server" Text='<%# Eval("Adicional") %>' CommandArgument='<%# "" + Eval("URL") +  Eval("id_proyecto") %>' CssClass="boton_Link_Grid" CommandName="adiocional" />
                    <br />
                    <asp:Button ID="btnAnexos" runat="server" Text="Anexos" CommandArgument='<%# Eval("id_proyecto") %>' CommandName="VerAnexosAcreditacion" />
                </ItemTemplate> 
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
