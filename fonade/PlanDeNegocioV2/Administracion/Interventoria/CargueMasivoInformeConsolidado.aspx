<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CargueMasivoInformeConsolidado.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.CargueMasivoInformeConsolidado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" Text="Cargue Masivo de Informes Consolidados" />
    </h1>
    <hr />
    <div>
        <asp:Label ID="lblRecomendacion" runat="server" Text="Recomendación de la interventoría: "></asp:Label>
        <asp:DropDownList ID="ddlRecomendacion" 
            DataValueField="idRecomendacion" 
        DataTextField="Recomendacion"
            runat="server"></asp:DropDownList>
    </div>
    <div>
        <asp:FileUpload ID="Archivo" runat="server" Width="422px"  />
        <asp:Button ID="btnSubirInformes" runat="server" Text="Subir Informes" OnClick="btnSubirInformes_Click" />  
    </div>
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    </div>
    <div>
        <hr />
        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="false" 
            EmptyDataText="No hay actividades." 
            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" 
            CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" 
            OnRowDataBound="gvResult_RowDataBound" DataKeyNames="MessageColor">
        <Columns>            
            <asp:BoundField HeaderText="Archivo" DataField="Archivo" HtmlEncode="false" />
            <asp:BoundField HeaderText="Mensaje" DataField="Message" HtmlEncode="false" />
            <asp:BoundField HeaderText="Codigo proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Ver archivo" >
                <ItemTemplate>                                                    
                    <asp:HyperLink ID="linkArchivo" runat="server" Target="_blank" NavigateUrl='<%#Eval("Url") %>' Visible='<%# (bool)Eval("ShowUrl")%>' Text ="Ver archivo">                        
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>
</asp:Content>
