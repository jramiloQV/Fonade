<%@ Page Language="C#" MasterPageFile="FrameEmprendedor.master" CodeBehind="InformacionAcademica.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.CatalogoEmprendedor.InformacionAcademica" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyHolder">

    <asp:LinqDataSource ID="lds_infoac" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_infoac_Selecting"  >        
 </asp:LinqDataSource>

 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager> 
       
 <h1><asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
  <asp:Panel  ID="pnl_Convenios" runat="server">
 Buscar: <asp:TextBox runat="server" ID="a"></asp:TextBox>
    Ciudad:<asp:DropDownList ID="ddl1" runat="server"></asp:DropDownList>
   <asp:Button ID="btn1" Text="Buscar"  runat="server" />

   
   <asp:HyperLink ID="AgregarConvenio"    NavigateUrl="~/FONADE/AdministrarPerfiles/Convenios/CatalogoConvenios.aspx?Accion=Crear" runat="server">
 <img alt="" src="../../../Images/icoAdicionarUsuario.gif" />
 Agregar Programa Académico</asp:HyperLink>
     <asp:GridView ID="gv_infoac" runat="server" Width="100%" AutoGenerateColumns="False"
        DataKeyNames="" CssClass="Grilla" AllowPaging="true" 
        DataSourceID="lds_infoac" 
        AllowSorting="True" onrowdatabound="gv_infoac_RowDataBound" 
        OnDataBound="gv_infoac_DataBound" 
        OnRowCreated ="gv_infoac_RowCreated" 
         PageSize='<%# PAGE_SIZE %>'
               
        >
          
        <Columns>   
            <asp:TemplateField HeaderText="Programa Académico" SortExpression="Convenio">
                <ItemTemplate>
                    <asp:Button CssClass="boton_Link"  ID="btn_ProgramaAcademico" runat="server" Text='<%# Eval("NomProgramaAcademico")%>'></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="Institución Educativa" SortExpression="Convenio">
                <ItemTemplate>
                    <asp:Button CssClass="boton_Link" ID="btn_InstitucionEducativa" runat="server"   Text='<%# Eval("NomInstitucionEducativa")%>'></asp:Button>
                </ItemTemplate>
            </asp:TemplateField> 
             <asp:TemplateField HeaderText="Ciudad" SortExpression="Ciudad">
                <ItemTemplate>
                    <asp:Button  CssClass="boton_Link" ID="btn_Ciudad" runat="server"  Text='<%# Eval("NomCiudad")%>'></asp:Button>
                </ItemTemplate>
            </asp:TemplateField> 
        </Columns>
    </asp:GridView>
    </asp:Panel>
    <asp:Panel  ID="pnl_crearEditar"  runat="server">
    <asp:Table ID="tbl_Institucion" runat="server">
      <asp:TableRow>
            <asp:TableCell>Institución:</asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="ddl_institucion" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_institucion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Nombre del Programa:</asp:TableCell>
            <asp:TableCell> <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownList1" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            <asp:TableCell>Departamento de institución:</asp:TableCell>
            
            <asp:TableCell>
            
            <asp:UpdatePanel runat="server" id="UpdatePanel" updatemode="Conditional">
            <Triggers>
                        <asp:AsyncPostBackTrigger controlid="ddl_depto1" eventname="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                 <ajaxToolkit:ComboBox 
                  
                        ID="ddl_depto1" 
                        runat="server" 
                        AutoPostBack="true" 
                        DropDownStyle="DropDownList" 
                        AutoCompleteMode="SuggestAppend" 
                        CaseSensitive="False" 
                        CssClass="AjaxToolkitStyle" 
                        ItemInsertLocation="Append"
                        OnSelectedIndexChanged="ddl_depto_OnSelectedIndexChanged1"
                         Enabled="true">
                        
                 </ajaxToolkit:ComboBox>
                         <asp:DropDownList ID="ddl_ciudad1" AutoPostBack="true" runat="server">
                            <asp:ListItem Value="" Text="(Todos los Municipios)" Selected></asp:ListItem>
                         </asp:DropDownList>
            </ContentTemplate>
            </asp:UpdatePanel> 
            </asp:TableCell></asp:TableRow><asp:TableRow>
            </asp:TableRow><asp:TableRow>
            <asp:TableCell>Ciudad de la institución:</asp:TableCell><asp:TableCell>
             <asp:UpdatePanel runat="server" id="UpdatePanel1" updatemode="Conditional">
            <Triggers>
                        <asp:AsyncPostBackTrigger controlid="ddl_depto2" eventname="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
            <ajaxToolkit:ComboBox 
                ID="ddl_depto2" 
                runat="server" 
                AutoPostBack="true" 
                DropDownStyle="DropDownList" 
                AutoCompleteMode="SuggestAppend" 
                CaseSensitive="False" 
                CssClass="AjaxToolkitStyle" 
                ItemInsertLocation="Append"
                OnSelectedIndexChanged="ddl_depto_OnSelectedIndexChanged2"
                 Enabled="true">
                 </ajaxToolkit:ComboBox>
               
                <asp:DropDownList ID="ddl_ciudad2"  AutoPostBack="true" runat="server">
                            <asp:ListItem Value="" Text="(Todos los Municipios)" Selected></asp:ListItem>
                </asp:DropDownList>
            </ContentTemplate>
            </asp:UpdatePanel >
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddl_depto1" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddl_ciudad2" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow>
        
    </asp:Table>
     <asp:Button ID="btn_crearActualizar"    runat="server" Text="Actualizar" />
     
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
