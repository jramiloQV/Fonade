<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="VerReintegro.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.VerReintegro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

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
            
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/FONADE/interventoria/SeguimientoPptal.aspx" ></asp:Button>            

        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.VerReintegro"
                    SelectMethod="GetReintegros" >
                <SelectParameters> 
                    <asp:QueryStringParameter Name="codigo" Type="String" DefaultValue="0" QueryStringField="codigo" />                    
                </SelectParameters>  
        </asp:ObjectDataSource> 
</asp:Content>
