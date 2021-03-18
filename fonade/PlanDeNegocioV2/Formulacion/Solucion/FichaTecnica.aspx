<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FichaTecnica.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Solucion.FichaTecnica" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="controlHelp" TagName="Help" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>

<!DOCTYPE html >
<html style="overflow-x: hidden;">
<head runat="server">
    <title>Ficha tecnica </title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
        <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
       <script src="../../../Scripts/common.js"></script>
    <style type="text/css">
        #tc_proyectos_body {
            height : 100% !important;
        } 

        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px !important;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .parentContainer {
            width: 100%;
            height: 650px;
            overflow-x: hidden;
            overflow-y: visible;
        }

        .childContainer {
            width: 100%;
            height: auto;
        }

        html, body, div, iframe {
        }
    </style>

    <script type="text/javascript">
        function alerta() {
            return confirm('¿ Está seguro de eliminar el registro, ya que este puede estar siendo usado en otra sección de su plan de negocio ?');
        }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">

        <controlEncabezado:Encabezado ID="Encabezado" runat="server" />

        <div style="position: relative; left: 740px; width: 160px;">
            <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="FichaTecnica" />
        </div>

        <div style="text-align: center">
           <h1>III. ¿Cuál es mi Solución?</h1>
        </div>

        <br />
        <controlHelp:Help runat="server" ID="helpFichaTecnica" Mensaje="FichaTecnica" Titulo="8. Elabore la ficha técnica para cada uno de los productos (Bienes o servicios) que componen su portafolio:" />
        <h3>Nota : La información consignada en la ficha técnica dependerá del tipo de bien o servicio a ofrecer, 
                   y el emprendedor podrá ampliar esta información a su consideración.                   
        </h3>
        <br />
        <asp:ImageButton ID="imgAdd" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
        <asp:LinkButton ID="lnkAdd" runat="server" Text=" Adicionar producto" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClick="lnkAdd_Click" />
        <br />
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false">Sucedio un error inesperado, intentalo de nuevo.</asp:Label>

        <asp:GridView ID="gvProductos" runat="server" Width="820px" AutoGenerateColumns="False" CssClass="Grilla" AllowPaging="true" AllowSorting="false" EmptyDataText="No existen productos para este plan de negocio." DataSourceID="dataProductos" OnRowCommand="gvProductos_RowCommand" PageSize="5">
            <Columns>
                <asp:TemplateField HeaderText="Eliminar">
                    <ItemTemplate>
                        <asp:LinkButton ID="imgborrar" runat="server" CommandArgument='<%# Eval("Id_Producto") %>' CommandName="eliminar" CausesValidation="false" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClientClick="return alerta();">
                            <asp:Image ID="btnBorrar" ImageUrl="../../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;" ToolTip="Eliminar acta" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre especifico">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Producto")%>' CommandName="actualizar" Text='<%# Eval("NomProducto") %>' Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Nombre comercial" DataField="NombreComercial" HtmlEncode="false" />
                <asp:BoundField HeaderText="Unidad de medida" DataField="UnidadMedida" HtmlEncode="false" />
                <asp:BoundField HeaderText="Descripción" DataField="DescripcionGeneral" HtmlEncode="false" />
                <asp:BoundField HeaderText="Condicones especiales" DataField="CondicionesEspeciales" HtmlEncode="false" />
                <asp:BoundField HeaderText="Composición" DataField="Composicion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Otros" DataField="Otros" HtmlEncode="false" />
            </Columns>
        </asp:GridView>

        <asp:ObjectDataSource
            ID="dataProductos"
            runat="server"
            TypeName="Fonade.PlanDeNegocioV2.Formulacion.Solucion.FichaTecnica"
            SelectMethod="Get"
            SelectCountMethod="CountProductos"
            MaximumRowsParameterName="maxRows"
            StartRowIndexParameterName="startIndex"
            EnablePaging="true">
            <SelectParameters>
                <asp:QueryStringParameter Name="codigoProyecto" QueryStringField="codproyecto" DefaultValue="0" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
