<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoBeneficiario.aspx.cs"
    Inherits="Fonade.FONADE.Beneficiario.CatalogoBeneficiario" MasterPageFile="~/Master.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <%--<style type="text/css">
        .style10
        {
            width: 21px;
        }
        .style11
        {
            width: 39px;
        }
        .style12
        {
            width: 275px;
        }
        .style13
        {
            width: 359px;
        }
        .style14
        {
            width: 18px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <%--<asp:LinqDataSource ID="lds_beneficiarios" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_beneficiarios_Selecting">
    </asp:LinqDataSource>--%>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Width="98%">
        <ContentTemplate>
            <table id="tabla_normal" runat="server" width="98%">
                <tr>
                    <td colspan="3">
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700"></asp:Label></h1>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="Img_AgregarBenef" runat="server" ImageUrl="~/Images/agregar.png"
                            ImageAlign="AbsBottom" OnClick="Img_AgregarBenef_Click" />
                        <asp:HyperLink ID="h_agregarbenef" runat="server" Style="font-weight: 700" NavigateUrl="Beneficiario.aspx?LoadCode=0">Adicionar Beneficiario</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gv_verBeneficiarios" CssClass="Grilla" runat="server" 
                            AllowPaging="false" AutoGenerateColumns="false" Width="100%" EmptyDataText="Aún no ha registrado ningún beneficiario."
                            OnDataBound="gv_verBeneficiarios_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminarbenef" CommandArgument='<%# Bind("Id_PagoBeneficiario")%>'
                                            OnCommand="Eliminar_benef" Visible='<%# !((int)Eval("CodPagoBeneficiario") == 0) %>'
                                            runat="server" ImageUrl="/Images/icoBorrar.gif" />
                                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnEliminarbenef"
                                            ConfirmText="Desea eliminar este beneficiario?" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="l_nombre" runat="server" Text='<%#"" + Eval("Nombre") + " " + Eval("Apellido") %>'
                                            NavigateUrl='<%# "Beneficiario.aspx?LoadCode=" + Eval("Id_PagoBeneficiario") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Correo Electrónico">
                                    <ItemTemplate>
                                        <asp:Label ID="l_email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ciudad">
                                    <ItemTemplate>
                                        <asp:Label ID="l_ciudad" runat="server" Text='<%#"" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=""></asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="l_conteoBenef" runat="server" Text="0 beneficiarios registrados"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table id="tabla_default" runat="server" width="95%" border="1" cellpadding="0" cellspacing="0"
                bordercolor="#4e77af" visible="false">
                <tr>
                    <td align="center" valign="top" width="98%">
                        <br />
                        <br />
                        SU PROYECTO DEBE ESTAR EN EJECUCIÓN PARA PODER REALIZAR REGISTRO DE BENEFICIARIOS.
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
