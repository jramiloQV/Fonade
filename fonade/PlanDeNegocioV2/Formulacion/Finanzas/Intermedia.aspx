<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="Intermedia.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.Intermedia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <%--<link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />        --%>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>   
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>    
    <script type="text/javascript">
        function cerrarVentana()
        {
            window.opener.location.reload();
            window.parent.opener.focus();
            window.close();
        }
    </script>
    <style type="text/css">
                        
        body,html
        {
            background-image: none !important;
        }            
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label ID="lblTitulo" runat="server" Text="">

        </asp:Label>
    </h1>
    <br />
    <br />    

    <asp:DropDownList ID="ddlTipoInsumo" runat="server" Height="21px" Width="300px" OnSelectedIndexChanged="ddlTipoInsumo_SelectedIndexChanged" AutoPostBack="true" />
    <br />
    <br />
    <input id="hidInsumo" name="hidInsumo" type="hidden" value="1" />
    
    <asp:ImageButton ID="imgBtnAgregarInsumo" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="imgBtnAgregarInsumo_Click" />
    &nbsp;
    <asp:LinkButton ID="lnkAgregarInsumo" runat="server" Text="Adicionar Insumo" OnClick="lnkAgregarInsumo_Click">
    </asp:LinkButton>

    <asp:LinqDataSource ID="lds_ProyectoInsumo" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_ProyectoInsumo_Selecting">
    </asp:LinqDataSource>
    <asp:GridView ID="gvrProyectoInsumo" runat="server" AutoGenerateColumns="false" CssClass="Grilla" DataSourceID="lds_ProyectoInsumo" DataKeyNames="id_Insumo" OnRowCommand="gvrProyectoInsumo_RowCommand">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="imgBorrarInsumo" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandName="borrar" CommandArgument='<%# Eval("Id_Insumo") %>' OnClientClick="return confirm('Al borrar un insumo: este desaparecerá de los productos que lo requieren\nTambien se eliminará la proyeccion de compras del isnumo\n\n ¿Esta seguro que desea borrar el insumo seleccionado?')" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chk_insumo" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Nombre Tipo Insumo" DataField="NomTipoInsumo" />
            <asp:TemplateField HeaderText="Nombre">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEditarInsumo" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id_Insumo") %>' Text='<%# Eval("nomInsumo") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Presentacion" HeaderText="Presentacion" />
            <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
        </Columns>
    </asp:GridView>
    <br />
    <br />
    <div>
        <span style="width: 150px;  text-align: left;">
            <asp:Button ID="btnAgrgarAProducto" runat="server" OnClick="btnAgrgarAProducto_Click" Text="Agregar al producto" />
        </span>
        <span style="width: 150px;  text-align: right;">
            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClick="btnCerrar_Click" />
        </span>
    </div>
</asp:Content>
