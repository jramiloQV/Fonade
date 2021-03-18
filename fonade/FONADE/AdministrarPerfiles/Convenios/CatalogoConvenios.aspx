<%@ Page Language="C#" MasterPageFile="~/Master.master" Title="Fonade-Administrar Convenios" AutoEventWireup="true" CodeBehind="CatalogoConvenios.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.Convenios.CatalogoConvenios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:LinqDataSource ID="lds_Convenios" runat="server"
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false"
        OnSelecting="lds_Convenios_Selecting">
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <h1>
        <asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
    <asp:Panel ID="pnl_Convenios" runat="server">
        <asp:HyperLink ID="AgregarConvenio" NavigateUrl="~/FONADE/AdministrarPerfiles/Convenios/CatalogoConvenios.aspx?Accion=Crear" runat="server">
 <img alt="" src="../../../Images/icoAdicionarUsuario.gif" />
 Agregar Convenios</asp:HyperLink>
        <asp:GridView ID="gv_Convenios" runat="server" Width="100%" AutoGenerateColumns="False"
            DataKeyNames="" CssClass="Grilla" AllowPaging="false"
            DataSourceID="lds_Convenios"
            AllowSorting="True" OnRowDataBound="gv_Convenios_RowDataBound"
            OnDataBound="gv_Convenios_DataBound"
            OnRowCreated="gv_Convenios_RowCreated"
            OnPageIndexChanging="gv_Convenios_PageIndexChanged">

            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>

                        <asp:ImageButton ID="btn_Inactivar" CommandArgument='<%# Bind("Id_convenio")%>' OnCommand="btn_Inactivar_click" runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="true" />
                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btn_Inactivar">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btn_Inactivar" OkControlID="btnYes"
                            CancelControlID="btnNo" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                            <div class="header">
                                Confirmación
                            </div>
                            <div class="body">
                                Esta seguro de borrar este registro?
                            </div>
                            <div class="footer" align="right">
                                <asp:Button ID="btnYes" runat="server" Text="Sí" />
                                <asp:Button ID="btnNo" runat="server" Text="No" />
                            </div>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Convenio">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Convenio" runat="server" NavigateUrl='<%# "CatalogoConvenios.aspx?Accion=Editar&CodCriterio="+ Eval("Id_convenio") %>' Text='<%# Eval("nomconvenio")%>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Fecha Inicio" DataField="FechaInicio" SortExpression="FechaInicio" DataFormatString="{0:dd-MM-yyyy}" />
                <asp:BoundField HeaderText="Fecha Fin" DataField="FechaFin" SortExpression="FechaFin" DataFormatString="{0:dd-MM-yyyy}" />
                <asp:BoundField HeaderText="Email Fiduciaria" DataField="EmailFiduciaria" SortExpression="EmailFiduciaria" />
                <asp:BoundField HeaderText="Operador" DataField="operador" SortExpression="operador" />


            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_crearEditar" runat="server">
        <asp:Table ID="tbl_Convenio" runat="server">
            <asp:TableRow>
                <asp:TableCell>Convenio:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_Convenio" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Descripción:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_Descripcion" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_Descripcion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <%--FechaInicial--%>
                <asp:TableCell>Fecha inicio:</asp:TableCell>

                <asp:TableCell>

                    <asp:TextBox runat="server" ID="tb_fechaInicio" Text="" />
                    <asp:Image runat="server" ID="img_dateInicio" AlternateText="cal2"
                        ImageUrl="~/images/icomodificar.gif" />
                    <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender1"
                        OnClientDateSelectionChanged="fechaIniMenor"
                        PopupButtonID="img_dateInicio" CssClass="ajax__calendar"
                        TargetControlID="tb_fechaInicio"
                        Format="dd/MM/yyyy" />
                </asp:TableCell></asp:TableRow><asp:TableRow>

                <%--Fecha Final--%>
                <asp:TableCell>Fecha fin:</asp:TableCell><asp:TableCell>
                    <asp:TextBox runat="server" ID="tb_fechaFin" Text="" />
                    <asp:Image runat="server" ID="img_dateFin" AlternateText="cal2"
                        ImageUrl="~/images/icomodificar.gif" />



                    <ajaxToolkit:CalendarExtender runat="server" ID="calExtender2"
                        OnClientDateSelectionChanged="fechaFinMayor"
                        PopupButtonID="img_dateFin" CssClass="ajax__calendar"
                        TargetControlID="tb_fechaFin"
                        Format="dd/MM/yyyy" ClientIDMode="Inherit" />


                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Operador:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddlOperador" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged">
                    </asp:DropDownList>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Fiduciaria:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddl_fiduciaria" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddl_fiduciaria" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow></asp:Table><%--Boton para actualizar--%><asp:Button ID="btn_crearActualizar"
            OnClick="btn_crearActualizar_onclick"
            runat="server" Text="Actualizar" />

        <ajaxToolkit:ConfirmButtonExtender Enabled="false" ID="cbe1" runat="server" DisplayModalPopupID="mpe1" TargetControlID="btn_crearActualizar">
        </ajaxToolkit:ConfirmButtonExtender>
        <ajaxToolkit:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlPopup1" TargetControlID="btn_crearActualizar" OkControlID="btnYes"
            BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlPopup1" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header">
                Confirmación </div><div class="body">
                <asp:Label ID="lbl_popup" runat="server"></asp:Label></div><div class="footer" align="right">
                <asp:Button ID="btnYes" runat="server" Text="Aceptar" />

            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server"></asp:Label><script type="text/javascript">

                                                                                       function fechaIniMenor(sender, args) {

                                                                                           var startDate = document.getElementById("<%=tb_fechaInicio.ClientID%>").value.split('/');
                                                                                           var endDate = document.getElementById("<%=tb_fechaFin.ClientID%>").value.split('/');

                                                                                           var FechaInicial = new Date(startDate[2] + '-' + startDate[1] + '-' + startDate[0]);
                                                                                           var FechaFin = new Date(endDate[2] + '-' + endDate[1] + '-' + endDate[0]);

                                                                                           if (FechaInicial > FechaFin) {
                                                                                               alert("La fecha inicial no puede ser mayor que la final.");

                                                                                               document.getElementById("<%=tb_fechaInicio.ClientID%>").value = document.getElementById("<%=tb_fechaFin.ClientID%>").value;
                return false;
            }

        }


        function fechaFinMayor(sender, args) {

            var startDate = document.getElementById("<%=tb_fechaInicio.ClientID%>").value.split('/');
            var endDate = document.getElementById("<%=tb_fechaFin.ClientID%>").value.split('/');

            var FechaInicial = new Date(startDate[2] + '-' + startDate[1] + '-' + startDate[0]);
            var FechaFin = new Date(endDate[2] + '-' + endDate[1] + '-' + endDate[0]);

            if (FechaInicial > FechaFin) {
                alert("La fecha Final no puede ser menor que la Inicial.");

                document.getElementById("<%=tb_fechaFin.ClientID%>").value = document.getElementById("<%=tb_fechaInicio.ClientID%>").value;
                return false;
            }

        }
    </script></asp:Content>