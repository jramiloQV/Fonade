<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdicionarReintegro.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.AdicionarReintegro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .auto-style3 {
            width: 197px;
        }
    </style>
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de realizar este reintegro ?');
        }
    </script>
    <%--<script src="../../../../Scripts/jquery-1.11.1.min.js"></script>    
    <script type="text/javascript" src="../../../../Scripts/jquery.number.min.js"></script>    
    <script>
         $(function () {
             $('.money').number(true, 2);
         });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Reintegros" runat="server" ID="lblMainTitle" Visible="true" />
            </h1            
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3"> <asp:Label ID="lblTitle1" Font-Bold="true" runat="server" Text="Código reintegro"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle2" Font-Bold="true" runat="server" Text="Fecha reintegro"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle3" Font-Bold="true" runat="server" Text="Valor reintegro"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle4" Font-Bold="true" runat="server" Text="Valor pago post reintegro"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle5" Font-Bold="true" runat="server" Text="Presupuesto actual"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle6" Font-Bold="true" runat="server" Text="Presupuesto con reintegro"></asp:Label></td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtCodigoReintegro" AutoPostBack="false" Text="" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td class="auto-style3">                                            
                        <asp:TextBox ID="txtFechaReintegro" runat="server" BackColor="White" Width="60%" />                            
                        <asp:Image ID="btnDatePicker" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarfechaI" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDatePicker" TargetControlID="txtFechaReintegro" />                                        
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtValorReintegro" CssClass="money" AutoPostBack="true" OnTextChanged="txtCodigoReintegro_TextChanged" Text="" runat="server" Width="80%"></asp:TextBox>
                    </td>
                    <td><asp:Label ID="lblValorPagoPostReintegro" runat="server" Text="0"></asp:Label></td>
                    <td><asp:Label ID="lblPresupuestoVigente" runat="server" Text="0"></asp:Label></td>
                    <td><asp:Label ID="lblPresupuestoConReintegro" runat="server" Text="0"></asp:Label></td>
                </tr>                  
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:Label ID="lblTitle7" Font-Bold="true" runat="server" Text="Observación"></asp:Label>
                        <br />     
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Width="100%" Height="50px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:Label ID="lblTitle8" Font-Bold="true" runat="server" Text="Informe de reintegro"></asp:Label>
                        <br />                        
                        <asp:FileUpload ID="fuArchivo" runat="server" Width="422px" />                        
                    </td>                    
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                    </td>
                    <td >
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Reintegros/Reintegros.aspx" ></asp:Button>            
                    </td>
                    <td >
                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar reintegro" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>            
                    </td>
                </tr>               
            </table>
            <br />
            <h1>
                <asp:Label ID="lblTitle9" Text="Historial de reintegros" runat="server" Visible="true" />               
            </h1            
            <br />

            <asp:GridView ID="gvReintegros" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvReintegros_RowCommand" >
                <Columns>            
                    <asp:BoundField HeaderText="Código reintegro" DataField="CodigoReintegro" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Valor reintegro" DataField="ValorReintegroWithFormat" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Valor de pago con reintegro" DataField="ValorPagoConReintegroWithFormat" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Presupuesto sin reintegro" DataField="PresupuestoSinReintegroWithFormat" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Presupuesto con reintegro" DataField="PresupuestoConReintegroWithFormat" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha reintegro" DataField="FechaReintegroWithFormat" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="NombreContacto" HtmlEncode="false" />                                        
                    <asp:TemplateField HeaderText="Ver informe" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("ArchivoInforme") %>'>
                                <asp:Image ID="img_Url" runat="server" ImageUrl="~/Images/IcoDocNormal.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
            
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.AdicionarReintegro"
                    SelectMethod="GetReintegros" >
                <SelectParameters> 
                    <asp:QueryStringParameter Name="codigo" Type="String" DefaultValue="0" QueryStringField="codigo" />                    
                </SelectParameters>  
        </asp:ObjectDataSource> 
        </ContentTemplate>   
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdicionar"  />     
            <asp:PostBackTrigger ControlID="gvReintegros" />
        </Triggers>
    </asp:UpdatePanel>    
</asp:Content>
