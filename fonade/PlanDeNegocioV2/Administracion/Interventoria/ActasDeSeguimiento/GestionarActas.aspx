<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Master.Master" 
    AutoEventWireup="true" 
    CodeBehind="GestionarActas.aspx.cs"     
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.GestionarActas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Gestión de actas de seguimiento"></asp:Label>
    <br />
    <div style="text-align: right; margin-right: 10px;">
        <asp:LinkButton ID="LinkButton1" Text="Volver a consultar proyectos" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/ProyectosAsignados.aspx" runat="server"></asp:LinkButton>
    </div>
    <div style="text-align: left;">
        <asp:LinkButton ID="lnkNew" Text="Adicionar acta de incio" runat="server" OnClick="lnkNew_Click"></asp:LinkButton>
        <asp:LinkButton ID="lnkAddActaSeguimiento" Text="Adicionar Nueva Acta de Seguimiento" Visible="false"
            runat="server" OnClick="lnkAddActaSeguimiento_Click"></asp:LinkButton>
    </div>
    <br />
    <br />
    <h1 style="text-align: center;">
        <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Trazabilidad de acta de inicio y actas de seguimiento de interventoria"></asp:Label>
    </h1>
    <%--<input type="button" value="Imprimir" onclick="window.print();"/>--%>
    
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="false" AutoGenerateColumns="False"
        EmptyDataText="No existen actas de seguimiento." Width="98%" BorderWidth="0"
        CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
        ShowHeaderWhenEmpty="true" DataSourceID="data" OnRowCommand="gvMain_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Acta">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id") +";"+Eval("IdProyecto") %>'
                        CommandName="Ver" CausesValidation="False" Text='<%# Eval("Nombre") %>' runat="server" Enabled='<%# !(bool)Eval("Publicado") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Número" DataField="Numero" HtmlEncode="false" />
            <asp:BoundField HeaderText="Fecha de elaboración" DataField="FechaCreacionWithFormat" HtmlEncode="false" />
            <asp:BoundField HeaderText="Fecha de publicación" DataField="FechaPublicacionWithFormat" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Publicada">
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgHasUpdates" Height="16" Width="17" ImageUrl="~/Images/check.png" Visible='<%# (bool)Eval("Publicado") %>' />
                    <asp:Image runat="server" ID="Image1" Height="16" Width="17" ImageUrl="~/Images/icocanceladas.gif" Visible='<%# !(bool)Eval("Publicado") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ver Borrador">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkGenerarActaBorrador" runat="server" CausesValidation="false" CommandName="GenerarActaBorrador" 
                        CommandArgument='<%# Eval("Id") %>' Enabled='<%# !(bool)Eval("Publicado") %>'>
                        <asp:Image runat="server" ID="imgGetActaBorrador" Height="16" Width="13" ImageUrl="~/Images/DocBorrador.png" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Generar acta">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkGenerarActa" runat="server" CausesValidation="false" CommandName="GenerarActa" CommandArgument='<%# Eval("Id") %>' Enabled='<%# (bool)Eval("Publicado") %>'>
                        <asp:Image runat="server" ID="imgGetActa" Height="16" Width="13" ImageUrl="~/Images/IcoBMPDoc.jpg" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cargar acta">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCargarActa" runat="server" CausesValidation="false" CommandName="CargarActa" CommandArgument='<%# Eval("Id") %>' Enabled='<%# (bool)Eval("Publicado") %>'>
                        <asp:Image runat="server" ID="imgUploadActa" Height="23" Width="15" ImageUrl="~/Images/icoClick.gif" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descargar">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDescargarActa" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("ArchivoActa") %>' Enabled='<%# (bool)Eval("HasActaCharged") %>'>
                        <asp:Image runat="server" ID="imgDownloadActa" Height="16" Width="16" ImageUrl="~/Images/IcoDocPDF.gif" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
    <br />
    <br />
    <h1 style="text-align: center;">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Verificación de indicadores y metas"></asp:Label>
    </h1>
    <br />
    <%--DataSourceID="dataV2"--%>
    <asp:GridView ID="gvIndicadores" runat="server"
        AllowPaging="false"
        AutoGenerateColumns="False"
        EmptyDataText="No hay datos para mostrar."
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla"
        HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:BoundField HeaderText="Visita" DataField="Visita" HtmlEncode="false" />
            <asp:BoundField HeaderText="Empleo" DataField="Cargos" HtmlEncode="false" />
            <asp:BoundField HeaderText="Ejecución presupuestal" DataField="PresupuestoWithFormat" HtmlEncode="false" />
            <asp:BoundField HeaderText="Mercadeo" DataField="Mercadeo" HtmlEncode="false" />
            <asp:BoundField HeaderText="NBI" DataField="Nbi" HtmlEncode="false" />
            <asp:BoundField HeaderText="Contrapartidas" DataField="Contrapartidas" HtmlEncode="false" />
            <asp:BoundField HeaderText="Producción" DataField="Produccion" HtmlEncode="false" />
            <asp:BoundField HeaderText="Ventas" DataField="VentasWithFormat" HtmlEncode="false" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>



    <asp:ObjectDataSource
        ID="data"
        runat="server"
        TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.GestionarActas"
        SelectMethod="GetActas">
        <SelectParameters>
            <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codigo" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--<asp:ObjectDataSource
        ID="dataV2"
        runat="server"
        TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.GestionarActas"
        SelectMethod="GetIndicadores">
        <SelectParameters>
            <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codigo" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>

    <table class="Grilla" cellspacing="1" cellpadding="4" rules="all"
        id="idTablaEjecucion" style="border-width: 0px; width: 98%;">
        <tbody>
            <tr align="left">
                <th scope="col" colspan="8">% Ejecución</th>
            </tr>
            <tr align="left">
                <th scope="col">Visita</th>
                <th scope="col">Empleo</th>
                <th scope="col">Ejecución presupuestal</th>
                <th scope="col">Mercadeo</th>
                <th scope="col">GIMINBI</th>
                <th scope="col">Contrapartidas</th>
                <th scope="col">Producción</th>
                <th scope="col">Ventas</th>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEjecEmpleo0" runat="server" Font-Bold="True" Font-Size="Small" Text="%"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecEmpleo" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecPresupuesto" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecMercadeo" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecIDH" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecContrapartidas" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecProduccion" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEjecVentas" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#009900" Text=""></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <div>
        <asp:Label ID="Label4" runat="server" Visible="false"
            Text="*El IDH, se presenta concatenado con su respectivo porcentaje de ejecución"></asp:Label>
        
    </div>
</asp:Content>
