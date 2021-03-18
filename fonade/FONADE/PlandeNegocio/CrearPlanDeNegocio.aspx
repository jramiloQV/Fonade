<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="CrearPlanDeNegocio.aspx.cs" Inherits="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro que desea eliminar el emprendedor de este plan de negocio ?');
        }      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <h1>
                <asp:Label Text="Crear plan de negocio" runat="server" ID="lblTituloCrearPlan" Visible="false" />
                <asp:Label Text="Actualizar plan de negocio" runat="server" ID="lblTituloActualizarPlan" Visible="false" /> 
            </h1>
            <br />
            <asp:Table ID="formPlanDeNegocio" runat="server">
                <asp:TableRow>
                    <asp:TableCell> Nombre: </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNombre" runat="server" Width="400px" Enabled="false" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell>Descripción:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Width="400px" Height="100px" runat="server" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>                
                    <asp:TableCell><asp:Label ID="LabelLugar" runat="server" Text="¿En dónde se localizará la empresa?" Width="150"></asp:Label></asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="cmbDepartamento" runat="server" Width="400px" DataSourceID="dataDepartamento" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre"/>
                        <br />
                        <asp:DropDownList ID="cmbCiudad" runat="server" Width="400px" DataSourceID="dataCiudad" DataTextField="Nombre" DataValueField="Id" />   
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell><asp:Label ID="LabelSector" runat="server" Text="¿En que sector se encuentra clasificado el proyecto a desarrollar?" Width="150"></asp:Label></asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="cmbSector" runat="server" Width="400px" DataSourceID="dataSector" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                        <br />
                        <asp:DropDownList ID="cmbSubSector" runat="server" Width="400px" DataSourceID="dataSubSector" DataTextField="Nombre" DataValueField="Id" />                                                
                    </asp:TableCell></asp:TableRow></asp:Table><br /><asp:Label ID="lblError" Text="Sucedio un error inesperado" Visible="False" runat="server" ForeColor="Red" />
            <br />

            <asp:ImageButton ID="imageAdicionEmprendedor" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" Visible="false" OnClick="imageAdicionEmprendedor_Click" /><asp:LinkButton ID="linkAdicionarEmprendedor" runat="server" Text=" Adicionar emprendedor" Visible="false" OnClick="linkAdicionarEmprendedor_Click" /><br />
            <br />
            <asp:GridView ID="gvEmprendedores" runat="server" Width="100%" AutoGenerateColumns="False" Visible="false"
                CssClass="Grilla" AllowPaging="false" AllowSorting="false" EmptyDataText="No existen emprendedores para este plan de negocio." OnRowCommand="gvEmprendedores_RowCommand" ><Columns>
                    <asp:TemplateField HeaderText="Eliminar" Visible ="false" >
                        <ItemTemplate>
                            <asp:LinkButton ID="linkEliminar" CommandArgument='<%# Eval("CodigoProyecto") +";"+ Eval("Id") %>' runat="server" CommandName="deleteEmprendedor" OnClientClick="return alerta();" >
                                <img src="/Images/icoBorrar.gif" alt="Eliminar emprendedor" />
                            </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Nombres" >
                        <ItemTemplate>
                            <asp:LinkButton ID="linkEmprendedor" CommandArgument='<%# Eval("CodigoProyecto") +";"+ Eval("Id") %>'
                                Text='<%# Eval("NombreCompleto")%>' runat="server" CommandName="updateEmprendedor" Enabled='<%# (bool)Eval("AllowUpdate")%>' />                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" >
                        <ItemTemplate>
                            <asp:HyperLink ID="linkEmail" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                                Text='<%# Eval("Email") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:updateprogress id="UpdateProgress1" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true" >
                                <progresstemplate>
                                    <div class="form-group center-block">                                                                 
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <label class="control-label"> <b>Procesando información</b> </label><img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" /></div></div></progresstemplate></asp:updateprogress><asp:Button ID="btnCrearPlanDeNegocio" runat="server" Text="Crear plan de negocio" OnClick="btnCrearPlanDeNegocio_Click" Visible="False" />
            <asp:Button ID="btnActualizarPlanDeNegocio" runat="server" Text="Actualizar plan de negocio" OnClick="btnActualizarPlanDeNegocio_Click" Visible="False" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/FONADE/PlandeNegocio/PlanDeNegocio.aspx" Visible="true" />
            
            <asp:ObjectDataSource 
                ID="dataCiudad" 
                runat="server" 
                SelectMethod="getCiudades" 
                TypeName="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio">
                <SelectParameters><asp:ControlParameter ControlID="cmbDepartamento" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource 
                    ID="dataDepartamento"
                    runat="server"
                    TypeName="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio"
                    SelectMethod="getDepartamentos"
                    >
            </asp:ObjectDataSource>

            <asp:ObjectDataSource 
                    ID="dataSector"
                    runat="server"
                    TypeName="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio"
                    SelectMethod="getSectores"
                    >
            </asp:ObjectDataSource>

            <asp:ObjectDataSource 
                ID="dataSubSector" 
                runat="server" 
                SelectMethod="getSubSectores" 
                TypeName="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio">
                <SelectParameters><asp:ControlParameter ControlID="cmbSector" DefaultValue="0"  Name="CodigoSector" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource
                    ID="dataEmprendedores"
                    runat="server"
                    
                    TypeName="Fonade.FONADE.PlandeNegocio.CrearPlanDeNegocio"
                    SelectMethod="getEmprendedores"
                    >
            </asp:ObjectDataSource>

            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
