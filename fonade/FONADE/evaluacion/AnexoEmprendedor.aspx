<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AnexoEmprendedor.aspx.cs" Inherits="Fonade.FONADE.evaluacion.AnexoEmprendedor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       .auto-style1 {
            width: 100%;
        }
        #table{
            width:100% !important;
        }
        .sinlinea {
            border: none;
            border-color: none;
            border-collapse: collapse;
        }
        #bodyContentPlace_GV_Anexos{
            width:100% !important;
        }
</style>
    <script type="text/javascript">
        function ocultarMostar(Opt) {
            var obj = document.getElementById("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <table id="table">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:center;">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="DOCUMENTOS ANEXOS"></asp:Label>
                </th>
            </tr>
        </thead>
        <tr style="width:100%;">
            <td><asp:Label ID="L_Destacados" runat="server" Text="Documentos de acreditación"></asp:Label></td>
        </tr>
        <tr style="width:100%;">
            <td><asp:Label ID="L_Conctat" runat="server" Text="Anexos del emprendedor: "></asp:Label></td>
        </tr>
        <tr style="width:100%;">
            <td colspan="2">
                <br />
                <asp:GridView ID="GV_Anexos" runat="server" CssClass="Grilla" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Archivo">
                            <ItemTemplate>
                                <asp:HyperLink ID="HL_Archivo" runat="server" NavigateUrl='<%# Bind("Archivo") %>' Text="" CssClass="sinlinea" Target="_blank">
                                    <asp:Image ID="I_PDF" runat="server" ImageUrl="~/Images/IcoDocPDF.gif" CssClass="sinlinea" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Tipo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Tipo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Descripcion") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </td>
        </tr>
    </table>

</asp:Content>