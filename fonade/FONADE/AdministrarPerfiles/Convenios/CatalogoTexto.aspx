<%@ Page  MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="CatalogoTexto.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.Convenios.CatalogoTexto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        var rdrct = function () { document.location = 'CatalogoTexto.aspx'; }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:LinqDataSource ID="lds_Anexos" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_Anexos_Selecting"  >        
 </asp:LinqDataSource>

 <h1><asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
   <asp:Panel  ID="pnl_Anexos" runat="server">
     <asp:GridView ID="gv_Anexos" runat="server" Width="100%" AutoGenerateColumns="False"
        DataKeyNames="" CssClass="Grilla" AllowPaging="false" 
        DataSourceID="lds_Anexos" 
        AllowSorting="True" onrowdatabound="gv_Anexos_RowDataBound" 
        OnDataBound="gv_Anexos_DataBound" 
        OnRowCreated ="gv_Anexos_RowCreated"       
        OnPageIndexChanging="gv_Anexos_PageIndexChanged">
          
        <Columns>
            <asp:TemplateField HeaderText="Texto" SortExpression="Anexo">
                <ItemTemplate>
                    <asp:HyperLink ID="hl_Anexo" runat="server" NavigateUrl='<%# "CatalogoTexto.aspx?Accion=Editar&CodAnexo="+ Eval("Id_anexo") %>' Text='<%# Eval("nomtexto")%>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>                      
        </Columns>
    </asp:GridView>
    </asp:Panel>
    <asp:Panel  ID="pnl_crearEditar"  runat="server">
    <asp:Table ID="tbl_Anexo" runat="server" >
      <asp:TableRow>
            <asp:TableCell Style="width:30%;"><asp:Label ID="lbl_tituloanexo" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="tb_Descripción" TextMode="MultiLine" runat="server" Style="width:400px;height:200px;"></asp:TextBox>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Descripción" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
            </asp:TableCell>
        </asp:TableRow>   
        
    </asp:Table>
    <div style="width:100%;text-align:right;">
        <asp:Button ID="btn_crearActualizar"    OnClick="btn_crearActualizar_onclick"  runat="server" Text="Actualizar" />
    </div>
     
                <ajaxToolkit:ConfirmButtonExtender Enabled=false ID="cbe1" runat="server" DisplayModalPopupID="mpe1" TargetControlID="btn_crearActualizar">
                    </ajaxToolkit:ConfirmButtonExtender>
                    <ajaxToolkit:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlPopup1"  TargetControlID="btn_crearActualizar" OkControlID = "btnYes"
                    BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                     <asp:Panel ID="pnlPopup1" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        Confirmación
                    </div>
                    <div class="body">
                        <asp:Label ID="lbl_popup" runat="server"></asp:Label>
                    </div>
                    <div class="footer" align="right">
                        <asp:Button ID="btnYes" runat="server" Text="Aceptar" />
                        
                    </div>
                </asp:Panel>
    </asp:Panel>
        <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server"></asp:Label>
</asp:Content>