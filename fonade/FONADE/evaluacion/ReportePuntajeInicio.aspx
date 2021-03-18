<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ReportePuntajeInicio.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReportePuntajeInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700" Text="Reporte de puntaje de Evaluación" />
                </h1>
            </td>
        </tr>
    </table>
    <div>
        <asp:GridView ID="GrvConvocatorias" runat="server" Width="80%" AutoGenerateColumns="False"
            CssClass="Grilla" OnRowCommand="GrvConvocatorias_RowCommand" EmptyDataText="No se encontraron Datos"
            AllowPaging="True" OnPageIndexChanging="GrvConvocatorias_PageIndexChanging" PageSize ="100">
            <Columns>
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <asp:LinkButton   ID="lnkdescargar" style="cursor: pointer"   CommandName="descargar"  Text="Excel"
                                          CommandArgument='<%# Eval("Id_Convocatoria") %>' runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id_Convocatoria" HeaderText="Id" />
                <asp:TemplateField HeaderText="Nombres">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkdetallado" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Convocatoria") %>'
                            CommandName="detallado" Text='<%# Eval("NomConvocatoria") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
    </div>
 
</asp:Content>
