<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="FrameAsesorProyecto.aspx.cs" Inherits="Fonade.FrameAsesorProyecto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pagina1 {
            width: 30%;
            height: auto;
            border: 1px solid black;
            position: relative;
            float: left;
            overflow: scroll;
            height: 660px;
        }
        
        .pagina2 {
            width: 65%;
            height: auto;
            border: 1px solid black;
            position: relative;
            float: right;
            overflow: scroll;
            height: 620px;
            padding: 20px;
        }

        #pnlPrincipal {
            display: inline-block;
            width: 100%;
        }

        .ContentInfo {
            width: 100%;
        }

        body, #form1 {
            width: 1000px;
            height: 660px;
            background-color:white;
        }

        .asig {
            margin: 0px auto;
            width: 100%;
            text-align: center;
        }

        .seciones {
            padding-top: 25px;
            padding-bottom: 25px;
        }
        .aspNetHidden {
            display: none;
        }
        .seciones div{
            height: inherit !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function CheckOtherIsCheckedByGVID(rb) {
            var isChecked = rb.checked;
            var row = rb.parentNode.parentNode;

            var currentRdbID = rb.id;
            parent = document.getElementById("<%= gvrasignarasesores.ClientID %>");
           var items = parent.getElementsByTagName('input');

           for (i = 0; i < items.length; i++) {
               if (items[i].id != currentRdbID && items[i].type == "radio") {
                   if (items[i].checked) {
                       items[i].checked = false;
                   }
               }
           }
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="background-color: white">
        <div class="pagina1">
            <br />
            <h1 style="text-align:center; background-color: #00468F; color: #FFFFFF;">
                <label>PLANES DE NEGOCIO</label>
            </h1>
            <asp:GridView ID="gv_proyectos" runat="server" AutoGenerateColumns="false" CssClass="Grilla" 
                DataSourceID="ldsproyectos" AllowPaging="True" PageSize="20" 
                OnPageIndexChanging="gv_proyectos_PageIndexChanging" AllowSorting="true" 
                OnRowCreated="gv_proyectos_RowCreated" DataKeyNames="Id_Proyecto" 
                OnRowCommand="gv_proyectos_RowCommand" Width="100%">
                <Columns>
                    <asp:TemplateField SortExpression="NomProyecto">
                        <ItemTemplate>
                            <asp:Button ID="btnproyecto" runat="server" CssClass="boton_Link_Grid" 
                                CommandName="Ver"
                                Text='<%# ">> " + Eval("Id_Proyecto") + " - " + Eval("NomProyecto") %>' 
                                CommandArgument='<%# Eval("Id_Proyecto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgadmiracion" runat="server" ImageUrl="~/Images/admiracion.gif" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblcontactos" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="pagina2">
                    <span class="seciones">
                    <asp:Label ID="lbltitulo" runat="server" Text=""></asp:Label>
                    <asp:GridView ID="gvrasesorlider" runat="server" CssClass="Grilla" Width="90%" 
                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowSorting="true" 
                        DataSourceID="ldsasesorlider">
                        <Columns>
                            <asp:BoundField HeaderText="Asesor Lider" DataField="Nombre" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Email Asesor" SortExpression="Email">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkemail" runat="server" Text='<%# Eval("Email") %>' PostBackUrl='<%# "mailto:" + Eval("Email") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvrasesores" runat="server" AllowSorting="true" AutoGenerateColumns="false" 
                        CssClass="Grilla" DataSourceID="ldsasesores" ShowHeaderWhenEmpty="true" Width="90%">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Asesor" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Email Asesor" SortExpression="Email">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkemail1" runat="server" PostBackUrl='<%# "mailto:" + Eval("Email") %>' Text='<%# Eval("Email") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvrasignarasesores" runat="server" AllowSorting="true" 
                        AutoGenerateColumns="false" CssClass="Grilla" DataKeyNames="Id_Contacto" 
                        DataSourceID="ldsasesoresasignar" OnRowCreated="gvrasignarasesores_RowCreated" 
                        ShowHeaderWhenEmpty="true" Visible="false" Width="90%">
                        <Columns>
                            <asp:TemplateField HeaderText="Asesor" SortExpression="Nombre">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbxasesor" runat="server" Text='<%# Eval("Nombre") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asesor Lider" SortExpression="Nombre">
                                <ItemTemplate>                                    
                                    <asp:RadioButton ID="rbasesorlider" onclick="javascript:CheckOtherIsCheckedByGVID(this);" runat="server" GroupName="lider" Text='<%# Eval("Nombre") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:LinkButton ID="lnkasignacionasesores" runat="server" CssClass="asig" OnClick="lnkasignacionasesores_Click" Text="&gt;&gt; ASIGNACIÓN DE ASESORES &lt;&lt;" Width="100%"></asp:LinkButton>
                    </span>

            <br />
            <asp:Button ID="btnactualizar" runat="server" Text="Actualizar" OnClick="btnactualizar_Click" />
        </div>
    </div>
    <asp:LinqDataSource ID="ldsproyectos" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsproyectos_Selecting" AutoPage="true"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsasesorlider" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsasesorlider_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsasesores" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsasesores_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsasesoresasignar" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsasesoresasignar_Selecting"></asp:LinqDataSource>
    <asp:HiddenField ID="hdproyecto" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnactualizar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="gv_proyectos" EventName="PageIndexChanging" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
