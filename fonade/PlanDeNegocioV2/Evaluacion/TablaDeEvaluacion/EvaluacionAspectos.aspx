<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionAspectos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.EvaluacionAspectos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        #itemPlaceholderContainer
        {
            width: 754px;
        }
    </style>      
    
    <script type="text/javascript">                                                                           
        
        $(document).ready(function () {            
            $('.each').on('change', function () {
                var listas = $(":selected");
                var total = 0;
                for (var i = 0; i < listas.length; i++) {
                    total += parseInt(listas[i].value);
                }

                $(".objetivo").html(total);
            });
        });        
    </script>      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                                                   
                <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />
                <br />
                <table>
                    <tr>
                        <td style="width: 50%">
                            <div class="help_container">
                                <div id="imagen1" runat="server">
                                    <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" onclick="textoAyuda({titulo: 'Aspectos Evaluados', texto: 'Aspectos Evaluados'});" />
                                    <strong>Aspectos Evaluados </strong>
                                </div>
                                <div id="imagen2" runat="server" style="display: none">
                                    <a onclick="textoAyuda({titulo: 'Aspectos Evaluados', texto: 'AspectoMAmbiente'});">
                                        <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></a>
                                    <strong>Aspectos Evaluados </strong>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="div_Post_It" runat="server" visible="false">
                                <uc1:Post_It runat="server" ID="Post_It" _txtCampo="" _txtTab="1" _mostrarPost="false"/>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="overflow: hidden;">
                    <asp:DataList ID="DltEvaluacion" runat="server" Width="100%" BackColor="White" BorderStyle="None"
                        BorderWidth="0px" CellPadding="4" ForeColor="Black" OnItemDataBound="DltEvaluacion_ItemDataBound">
                        <FooterTemplate>
                            <table id="Table5" runat="server" bgcolor="#00468F" class="Grilla" width="100%">
                                <tr>
                                    <td align="right" style="width: 77%; background-color: #00468F;">
                                        <strong style="color: #FFFFFF">Puntaje Obtenido:</strong>&nbsp;
                                    </td>
                                    <td align="center" style="width: 10%; background-color: #00468F;">
                                        <strong>
                                            <asp:Label ID="lpuntajeObtenido" runat="server" CssClass="objetivo" ForeColor="White" />
                                        </strong>
                                    </td>
                                </tr>
                            </table>
                        </FooterTemplate>
                        <HeaderTemplate>
                            <table id="itemPlaceholderContainer" width="100%" runat="server" class="Grilla" frame="none">
                                <tr id="Tr2" runat="server">
                                    <th id="Th2" style="width: 77%" runat="server">
                                        Campo
                                    </th>
                                    <th id="Th3" runat="server" style="width: 10%" align="center">
                                        Puntaje
                                    </th>
                                    <th id="Th4" runat="server" style="width: 10%">
                                        Máximo
                                    </th>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="100%" runat="server">
                                <tr>
                                    <td style="border-width: thin; border-top-style: none; border-color: #94ADCD; border-bottom-style: solid">
                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("Orden") %>' Font-Size="Small"
                                            ForeColor="#174696" />
                                        <asp:Label ID="campoid" runat="server" Style="display: none" Text='<%# Eval("CampoId") %>' />
                                    </td>
                                </tr>
                            </table>
                            <table id="itemPlaceholderContainer0" width="100%" runat="server">
                                <tr>
                                    <td>
                                        Observaciones:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="validarLargo" runat="server" ID="txtobservaciones" TextMode="MultiLine"
                                            Height="103px" Width="648px" MaxLength="1000" />
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" width="100%" runat="server">
                                <tr>
                                    <td style="width: 77%" align="justify" width="100%">
                                        <asp:DataList ID="DtlHijos" runat="server" OnItemDataBound="DtlHijos_ItemDataBound" 
                                            Width="100%" >
                                            <ItemTemplate>
                                                <table id="Table4" runat="server" width="100%">
                                                    <tr>
                                                        <td align="justify" style="width: 77%">
                                                            <asp:Label ID="lCampo" runat="server" Text='<%# Eval("Campo") %>' />
                                                        </td>
                                                        <td align="center" style="width: 10%">
														
                                                            <asp:DropDownList ID="Ddlpuntaje" runat="server" CssClass="each" Width="80%">
                                                                <asp:ListItem>0</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lbl_Ddlpuntaje" Text="" runat="server" Visible="false" />
                                                            <asp:DropDownList ID="DdlpuntajeMedio" runat="server" AutoPostBack="false" Visible="False"
                                                                Width="80%">
                                                    
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lbl_DdlpuntajeMedio" Text="" runat="server" Visible="false" />
                                                        </td>
                                                        <td align="center" style="width: 10%">
                                                            <asp:Label ID="lblmaximo" runat="server" Text='<%# Eval("Maximo") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="justify" style="width: 77%">
                                                            <asp:Label ID="lAsignado" runat="server" Font-Size="Small" Text='<%# Eval("Asignado") %>'
                                                                Visible="False" />
                                                            <asp:Label ID="lidVariable" runat="server" Font-Size="Small" Text='<%# Eval("idVariable") %>'
                                                                Visible="False" />
                                                            <asp:Label ID="idcampo" runat="server" Font-Size="Small" Text='<%# Eval("id_campo") %>'
                                                                Visible="False" />
                                                        </td>
                                                        <td align="center" style="width: 10%">
                                                            &nbsp;
                                                        </td>
                                                        <td align="center" style="width: 10%">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <strong>
                                    <asp:Label ID="lTotal" runat="server" ForeColor="White" Visible="False" />
                                </strong>                                
                                <asp:Button runat="server" ID="update" Text="Actualizar" CssClass="btnUpdate" OnClick="update_Click" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </div>        
                
                <script type="text/javascript">   
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_endRequest(function () {
                        $('.each').on('change', function () {
                            var listas = $(":selected");
                            var total = 0;
                            for (var i = 0; i < listas.length; i++) {
                                total += parseInt(listas[i].value);
                            }

                            $(".objetivo").html(total);
                        });

                        var listas = $(":selected");
                        var total = 0;
                        for (var i = 0; i < listas.length; i++) {
                            total += parseInt(listas[i].value);
                        }

                        $(".objetivo").html(total);
                                               
                    });


                </script>        
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>