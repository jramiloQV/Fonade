<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="AdicionarProyectoActa.aspx.cs" Inherits="Fonade.FONADE.evaluacion.AdicionarProyectoActa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <br/>
    <table class="style10" style="margin-left: 30px" align="center">
        <tr>
            <td>
            <h1 align="center">
                <asp:Label ID="lbltitulo" runat="server" Text="Adicionar Planes De Negocio Al Acta" style="font-weight: 700"/>
                </h1>
            </td>
        </tr>
    </table>
    <br/>
    <div   align="center">
        <asp:GridView ID="GrvProyectoActas" runat="server" Width="40%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
            OnPageIndexChanging="GrvActasPageIndexChanging" 
            OnSorting="GrvProyectoActasSorting" EmptyDataText="No se encontraro Datos">
            <Columns>
                
                


                <asp:TemplateField HeaderText="Incluir">
                    <ItemTemplate>
                       <asp:CheckBox runat="server" ID="cidproyecto"/>
                        <asp:Label runat="server" ID="idproyecto" Visible="False" Text='<%# Eval("id_proyecto") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
         
                <asp:BoundField DataField="nomproyecto" HeaderText="Nombre" SortExpression="nomproyecto" />
               
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
        <p> <asp:Button runat="server" Text="Adicionar" onclick="Unnamed1_Click"/> </p>
        <p> &nbsp;</p>
    </div>
</asp:Content>
