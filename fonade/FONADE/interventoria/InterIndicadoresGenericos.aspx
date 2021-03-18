<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterIndicadoresGenericos.aspx.cs" Inherits="Fonade.FONADE.interventoria.InterIndicadoresGenericos"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <script type="text/ecmascript">
        function url() {
            open("agregarRiesgo.aspx", "Agregar Riesgo", "width=800,height=600");
        }
    </script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla != 13);
        }
    </script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .sinlinea {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }

        table {
            width: 100%;
        }

        .auto-style5 {
            width: 208px;
        }
        .auto-style6 {
            width: 210px;
        }
        .auto-style7 {
            width: 116px;
        }
        .auto-style8 {
            width: 69px;
        }
        .auto-style9 {
            width: 127px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div>
            <br />
            <table class="style1">
                <tr>
                    <td>
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Indicadores Genéricos', texto: 'IndicadoresInter'});">
                                <img alt="help_Objetivos" border="0" src="../../Images/imgAyuda.gif" />
                            </div>
                            <div>
                                &nbsp; <strong>Indicadores Genéricos</strong>
                            </div>
                        </div>
                        <br />
                    </td>
                    <td>
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="IndicadoresInter" _txtTab="1" _mostrarPost="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvindicadoresgenericos" runat="server" CssClass="Grilla" AutoGenerateColumns="false"
                            EmptyDataText="No se han establecido Indicadores Genéricos para esta empresa."
                            DataKeyNames="Id_IndicadorGenerico" OnRowDataBound="gvindicadoresgenericos_RowDataBound" 
                            OnRowCreated="gvindicadoresgenericos_RowCreated" OnRowCommand="gvindicadoresgenericos_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="Id_IndicadorGenerico" Visible="false" />

                                <asp:TemplateField HeaderText="Modificar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/editar.png"
                                            Width="20px"
                                            CommandName="Modificar" CommandArgument='<%# Eval("Id_IndicadorGenerico") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Nombre del Indicador" DataField="NombreIndicador" />

                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:Label ID="lblNumDescripcion" runat="server" Text='<%# Eval("NueradorDescripcion") %>'></asp:Label>
                                            <hr />
                                            <asp:Label ID="lblDenDescripcion" runat="server" Text='<%# Eval("DenominadorDescripcion") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:TextBox ID="txtNumerador" runat="server" Text='<%# Eval("Numerador") %>' OnChange='javascript:Evaluar(this);' ClientIDMode="Static"></asp:TextBox>
                                            <hr />
                                            <asp:TextBox ID="txtDenominador" runat="server" Text='<%# Eval("Denominador") %>' OnChange='javascript:Evaluar(this);' ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Evaluación">
                                    <ItemTemplate>
                                        <%--<span id="lblEvaluacion" name="lblEvaluacion" runat="server" ><%# Eval("Evaluacion") %></span>--%>
                                        <asp:Label ID="lblEvaluacion" runat="server" Text='<%# Eval("Evaluacion") %>' ClientIDMode="Static" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Evaluación" DataField="Evaluacion" />--%>
                                <asp:TemplateField HeaderText="Observación">
                                    <ItemTemplate>
                                        <div style="width: 250px; text-align: center;">
                                            <asp:TextBox ID="txtObservacion" runat="server" Text='<%# Eval("Observacion") %>' TextMode="MultiLine" Width="250px" Height="150px" MaxLength="8000"></asp:TextBox>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2">
                        <br />
                        <br />
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>

        </div>

        <asp:Label ID="lblModalEliminar" runat="server" Text=""></asp:Label>
        <!--MODAL EDITAR COMPROMISO-->

        <asp:ModalPopupExtender ID="ModalEliminarArchivo" runat="server"
            CancelControlID="btnCerrarModalEdit"
            TargetControlID="lblModalEliminar" PopupControlID="pnlEliminarArchivo"
            PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <!--Style="display: none"-->
        <asp:Panel ID="pnlEliminarArchivo" runat="server" Style="display: none"
            Width="700px" Height="330px" BackColor="White">
            <div class="EliminarArchivoPopup" style="overflow: auto;">
                <%--Popup--%>
                <div class="Controls" style="text-align: right; height: 0%">
                    <input type="submit" value="X" id="btnCerrarModalEdit" />
                </div>
                <div class="PopupBody" style="max-height: 500px;">

                    <asp:Label ID="lblIdIndicador" runat="server" Text="Label" ForeColor="White"></asp:Label>
                     <asp:Label ID="lblIndice" runat="server" Text="Label" ForeColor="White"></asp:Label>
                    <div id="cuerpoEliminarArchivo" style="height: 0%; padding-left: 20px;">

                        <h1>Modificar Indicador</h1>
                        <hr />
                        <h3>Nombre Indicador: </h3>
                        <h2>
                            <asp:Label ID="lblNombreIndicador" runat="server" Text="Nombre Indicador"></asp:Label></h2>

                        <table class="Grilla" style="max-width:80%">
                            <tr>                                                                
                                <th scope="col" class="auto-style5">Descripción</th>
                                <th scope="col" class="auto-style8">&nbsp;</th>
                                <th scope="col" class="auto-style7">Evaluación</th>
                                <th scope="col">Observación</th>
                            </tr>
                            <tr>
                                <td class="auto-style5">
                                    <div style="text-align: center;">
                                        <asp:Label ID="lblIndicadorNumDescripcion" runat="server" Text=""></asp:Label>
                                        <hr />
                                        <asp:Label ID="lblIndicadorDenDescripcion" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                                <td class="auto-style8">
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txtIndicadorNumerador" runat="server" Text="" 
                                           
                                            OnChange='javascript:Evaluar(this);' ClientIDMode="Static"></asp:TextBox>
                                        <hr/>
                                        <asp:TextBox ID="txtIndicadorDenominador" runat="server" Text="" 
                                            
                                            OnChange='javascript:Evaluar(this);' ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="auto-style6">
                                    <div style="text-align: center;">
                                        <asp:Label ID="lblEvaluacionIndicador" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 250px; text-align: center;">
                                            <asp:TextBox ID="txtIndicadorObservacion" runat="server" Text="" TextMode="MultiLine" Width="250px" Height="150px" MaxLength="8000"></asp:TextBox>
                                        </div>
                                </td>
                            </tr>                           
                        </table>

                        <h3>Motivo de cambio del indicador (Obligatorio): </h3>
                        <asp:TextBox ID="txtMotivoCambio" runat="server"
                            TextMode="MultiLine" Style="width: 520px; margin: 0px; height: 120px;"></asp:TextBox>
                        <div style="text-align: center; height: 0%;">
                            <asp:Button ID="btnModificarIndicador" runat="server"
                                Text="Modificar Indicador" OnClick="btnModificar_Click"/>                            
                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>


    </form>
    <script type="text/javascript">
        function Evaluar(objeto) {
            var indice;
            var nomClase;
            var objForm = document.forms["form1"];
            if (isNaN(objeto.value)) {
                alert("Debe ingresar un número");
                objeto.value = "";
            }
            nomClase = objeto.className.split('_');
            indice = nomClase[1]; // objeto.name.substring(objeto.class.length - 1, objeto.name.length);
            var classNumerador = 'txtNumerador_' + indice;
            var classDenominador = 'txtDenominador_' + indice;
            var classEvalua = 'lblEvaluacion_' + indice;
            var numerador = document.getElementsByClassName(classNumerador);
            var denominador = document.getElementsByClassName(classDenominador);
            var lblEvalua = document.getElementsByClassName(classEvalua);
            //"document.frmIndicador.numerador" + indice + ".value.length"

            if (numerador[0].value != '' && denominador[0].value != '') {
                var Division = parseInt(numerador[0].value) / parseInt(denominador[0].value);
                var Valdenominador = denominador[0].value;
                var Valnumerador = numerador[0].value;

                switch (indice) {
                    //Empleo
                    case "16":
                        if (Valdenominador == 0 && Valnumerador == 0) {
                            lblEvalua[0].innerHTML = "Sin evaluación";
                            lblEvalua[0].innerText = "Sin evaluación";
                        }
                        else {
                            if (Division > 1) {
                                lblEvalua[0].innerHTML = "Más que Efectivo";
                                lblEvalua[0].innerText = "Más que Efectivo";
                            }
                            else {
                                if (Division >= 66 / 100 && Division <= 1) {
                                    lblEvalua[0].innerHTML = "Efectivo";
                                    lblEvalua[0].innerText = "Efectivo";
                                }

                                else {
                                    lblEvalua[0].innerHTML = "Inefectivo";
                                    lblEvalua[0].innerText = "Inefectivo";
                                }
                            }
                        }

                        break;

                    //Presupuesto
                    case "17":
                        if (Valdenominador == 0 && Valnumerador == 0) { lblEvalua[0].innerHTML = "Sin evaluación"; }
                        else {
                            if (Division >= 70 / 100) { lblEvalua[0].innerHTML = "Efectivo"; }
                            else { lblEvalua[0].innerHTML = "Inefectivo"; }
                        }
                        break;

                    //Mercadeo
                    case "18":
                        if (Valdenominador == 0 && Valnumerador == 0) { lblEvalua[0].innerHTML = "Sin evaluación"; }
                        else {
                            if (Division >= 1) { lblEvalua[0].innerHTML = "Eficiente"; }
                            else { lblEvalua[0].innerHTML = "Ineficiente"; }
                        }
                        break;

                    //Ventas
                    case "19":
                        if (Valdenominador == 0 && Valnumerador == 0) { lblEvalua[0].innerHTML = "Sin evaluación"; }
                        else {
                            if (Division > 1) { lblEvalua[0].innerHTML = "Meta altamente eficiente"; }
                            else {
                                if (Division >= 55 / 100 && Division <= 1) { lblEvalua[0].innerHTML = "Meta eficiente"; }
                                else {
                                    lblEvalua[0].innerHTML = "Meta Deficiente";

                                }
                            }
                        }
                        break;

                    //Produccion
                    case "20":
                        if (Valdenominador == 0 && Valnumerador == 0) { lblEvalua[0].innerHTML = "Sin evaluación"; }
                        else {
                            if (Division > 1) { lblEvalua[0].innerHTML = "Más que efectivo"; }
                            else {
                                if (Division >= 60 / 100 && Division <= 1) { lblEvalua[0].innerHTML = "Efectivo"; }
                                else { lblEvalua[0].innerHTML = "Inefectivo"; }
                            }
                        }
                        break;

                    //Comercial
                    case "21":
                        if (Valdenominador == 0 && Valnumerador == 0) { lblEvalua[0].innerHTML = "Sin evaluación"; }
                        else {
                            if (Division > 1) { lblEvalua[0].innerHTML = "Más que eficiente"; }
                            else {
                                if (Division >= 70 / 100 && Division <= 1) { lblEvalua[0].innerHTML = "Eficiente"; }
                                else { lblEvalua[0].innerHTML = "Ineficiente"; }
                            }
                        }
                        break;

                }
            }
            else {
                if (eval("document.frmIndicador.numerador" + indice + ".value.length") == 0 || eval("document.frmIndicador.denominador" + indice + ".value.length") == 0) {
                    for (var x = 0; x < objForm.elements.length; x++) {
                        if (lblEvalua[0].name == "Evaluacion" + indice) { lblEvalua[0].innerHTML = ""; }
                    }
                }
            }
        }
    </script>
</body>
</html>
