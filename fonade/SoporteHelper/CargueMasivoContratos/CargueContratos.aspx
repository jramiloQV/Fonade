<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CargueContratos.aspx.cs" Inherits="Fonade.SoporteHelper.CargueMasivoContratos.CargueContratos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <!--Cargue masivo de contratos-->
    <fieldset style="Width:90%">
        <legend>Cargue de documentos (Sin notificación)</legend>
        <div>
            <asp:FileUpload ID="Archivo" runat="server" Width="100%" />
            <asp:Button ID="SubirArchivo" runat="server" Text="Subir documentos" OnClick="SubirArchivo_Click" />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
    <!--Cargue masivo de informes consolidados-->
    <fieldset style="Width:90%">
        <legend>Cargue masivo de informes consolidados (Se notifica: emprendedor)</legend>
        <div>
            <div>
                <asp:Label ID="lblRecomendacion" runat="server" Text="Recomendación de la interventoría: "></asp:Label>
                <asp:DropDownList ID="ddlRecomendacion"
                    DataValueField="idRecomendacion"
                    DataTextField="Recomendacion"
                    runat="server">
                </asp:DropDownList>
            </div>
            <div>
                <asp:FileUpload ID="FUInformes" runat="server"  Width="100%" />
                <asp:Button ID="btnSubirInformes" runat="server" Text="Subir Informes" OnClick="btnSubirInformes_Click" />
            </div>            
            <asp:Label ID="lblErrorInforme" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
    <!--Cargue de  Contratos de Cooperación-->
    <fieldset style="Width:90%">
        <legend>Cargue de  contratos de cooperación (Se notifica: emprendedor, Asesor, Interventor)</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUContratosCooperacion" runat="server"  Width="100%" />
                <asp:Button ID="btnSubirContratos" runat="server" Text="Subir Contratos" OnClick="btnSubirContratos_Click" />
            </div>            
            <asp:Label ID="lblErrorSubirContratosCooperacion" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
     <!--cargue de contratos de cooperación firmados -->
    <fieldset style="Width:90%">
        <legend>Cargue de contratos de cooperación firmados (Se notifica: emprendedor)</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUContratosCooperacionFirmados" runat="server" Width="100%" />
                <asp:Button ID="btnContratosCooperacionFirmados" runat="server" 
                    Text="Subir Contratos Firmados" OnClick="btnContratosCooperacionFirmados_Click" />
            </div>            
            <asp:Label ID="lblErrorContratosCooperacionFirmados" runat="server" 
                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>

    <!--Cargue de Actas de Liquidación-->
    <fieldset style="Width:90%">
        <legend>Cargue de actas de liquidación (Se notifica: emprendedor, Asesor, Interventor)</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUSubirActasLiquidacion" runat="server"  Width="100%" />
                <asp:Button ID="btnSubirActasLiquidacion" runat="server" 
                    Text="Subir Actas Liquidacion" OnClick="btnSubirActasLiquidacion_Click" />
            </div>            
            <asp:Label ID="lblErrorSubirActasLiquidacion" runat="server" 
                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
    <!--Prorrogas de Contratos -->
    <fieldset style="Width:90%">
        <legend>Prorrogas de contratos (Se notifica: emprendedor, Asesor, Interventor)</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUProrrogaContratos" runat="server" Width="100%" />
                <asp:Button ID="btnProrrogaContrato" runat="server" 
                    Text="Subir Prorrogas Contrato" OnClick="btnProrrogaContrato_Click" />
            </div>            
            <asp:Label ID="lblErrorProrrogaContrato" runat="server" 
                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
    <!--Cargue de Actas de Terminación -->
    <fieldset style="Width:90%">
        <legend>Cargue de actas de terminación (Se notifica: emprendedor, Asesor, Interventor)</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUActasTerminacion" runat="server" Width="100%" />
                <asp:Button ID="btnActasTerminacion" runat="server" 
                    Text="Subir Actas Terminacion" OnClick="btnActasTerminacion_Click" />
            </div>            
            <asp:Label ID="lblErrorActasTerminacion" runat="server" 
                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>

    
    <!--otros documentos
    <fieldset style="Width:90%; visibility:hidden">
        <legend>Otros documentos</legend>
        <div>            
            <div>
                <asp:FileUpload ID="FUOtrosDocumentos" runat="server"  Width="100%" />
                <asp:Button ID="btnOtrosDocumentos" runat="server" 
                    Text="Subir Otros Documentos" OnClick="btnOtrosDocumentos_Click" />
            </div>            
            <asp:Label ID="lblErrorOtrosDocumentos" runat="server" 
                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        </div>
    </fieldset>
         -->
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Listado de resultado"></asp:Label>
    <br />
    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay actividades." Width="90%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" OnRowDataBound="gvResult_RowDataBound" DataKeyNames="MessageColor">
        <Columns>
            <asp:BoundField HeaderText="Archivo" DataField="Archivo" HtmlEncode="false" />
            <asp:BoundField HeaderText="Mensaje" DataField="Message" HtmlEncode="false" />
            <asp:BoundField HeaderText="Codigo proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Ver archivo">
                <ItemTemplate>
                    <asp:HyperLink ID="linkArchivo" runat="server" Target="_blank" NavigateUrl='<%#Eval("Url") %>' Visible='<%# (bool)Eval("ShowUrl")%>' Text="Ver archivo">                        
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
</asp:Content>
