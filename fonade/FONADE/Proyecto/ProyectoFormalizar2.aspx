<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoFormalizar2.aspx.cs" Inherits="Fonade.FONADE.Proyecto.ProyectoFormalizar2"  MasterPageFile="~/Master.Master"%>
   
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    
    <asp:LinqDataSource ID="lds_proyectos" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_proyectos_Selecting" >
    </asp:LinqDataSource>
   <asp:LinqDataSource ID="LinqDataSourceEmp" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_proyectos_SelectingEmprend" >
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="LinqDataSourceEmpleos" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_proyectos_SelectingEmpleo" >
    </asp:LinqDataSource>
     <script type="text/javascript">
          function ValidaSeleccion(source, arguments) {
             var CardNumber = arguments.Value;
             if (CardNumber == 0) {
                 arguments.IsValid = false;
                 return;
             }
         }
    </script>
        <table width="98%" border="0">
          <tr>
            <td class="style50"><h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
	    </td>
            <td align="right">
                &nbsp;</td>
          </tr>
        </table>

        <!--------------------------------->
        <table class="style9">
          <tr>
            <td  colspan="4" bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td  colspan="4">&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->
        <table class="style1" >
            <tr>
              <td align="left" class="style7">&nbsp;</td>
              <td align="left" class="style10">Id.:</td>
              <td width="31%" align="left" class="style3">
                  <asp:Label  ID="LabelID" runat="server" Text=""></asp:Label>
                </td>
              <td align="left" class="style10">Fecha de Creación:</td>
              <td width="28%" align="left" class="style3">
                  <asp:Label ID="LabelFechaCreac" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
              <td align="left" >&nbsp;</td>
              <td align="left" class="style10">Nombre:</td>
              <td align="left" class="style3">
                  <asp:Label ID="LabelNombre" runat="server" Text=""></asp:Label>
                </td>
              <td align="left" class="style10">Estado:</td>
              <td align="left" class="style3">
                  <asp:Label ID="LabelEstado" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" >&nbsp;</td>
                <td align="left" class="style10">Tipo de Proyecto:</td>
                <td align="left" class="style3">
                    <asp:Label ID="LabelTipoP" runat="server" Text=""></asp:Label>
                </td>
                <td align="left" class="style10">Ciudad:</td>
                <td align="left" >
                    <asp:Label ID="LabelCiudad" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" >&nbsp;</td>
                <td align="left" class="style10">Sector:</td>
                <td colspan="3" align="left" class="style3">
                    <asp:Label ID="LabelSector" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">&nbsp;</td>
                <td align="left" class="style10">Sumario:</td>
                <td colspan="3" align="left" >
                    <asp:Label ID="LabelSumario" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table >
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->
        <table class="style1" style="width:98%;float:left">
          <tr>
            <td align="left" class="style10">Unidad:</td>
            <td class="style3">
                <asp:Label ID="LabelUnidad2" runat="server" Text=""></asp:Label>
            </td>
            <td align="right" class="style10">Institución: <asp:Label ID="LabelInsti2" runat="server" Text=""></asp:Label></td>
          </tr>
          <tr>
            <td align="left" class="style10">Jefe:</td>
            <td colspan="3" class="style3">
                <asp:Label ID="LabelJefe2" runat="server" Text=""></asp:Label>
            </td>
          </tr>
          <tr>
            <td  align="left" class="style10">Identificación:</td>
            <td colspan="3" class="style3">
                <asp:Label ID="LabelIdent2" runat="server" Text=""></asp:Label>
            </td>
          </tr>
        </table>
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style10">Asesores</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->

            <asp:GridView ID="GridViewProyectos"  CssClass="Grilla" runat="server" DataSourceID="lds_proyectos" 
            AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
            Height="30px" Width="98%">
                <Columns>
                    <asp:TemplateField HeaderText="">
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rol">
                        <ItemTemplate>
                            <asp:Label ID="hl_Rol" runat="server" Text='<%# Eval("nombreQ2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <asp:Label ID="hl_Nombre" runat="server" Text='<%# "" + Eval("nombresQ2") + " " + Eval("apellidosQ2")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->
        <table class="style1">
          <tr>
            <td align="left" class="style10" style="font-weight:bold;font-size:14px;">Plan Nacional</td>
          </tr>
          <tr>
            <td align="left" class="style3">
                <asp:Label ID="LabelPN3" runat="server" Text=""></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="style10" style="font-weight:bold;font-size:14px;">Plan Regional</td>
          </tr>
          <tr>
            <td align="left" class="style3">
                <asp:Label ID="LabelPR3" runat="server" Text=""></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="style10" style="font-weight:bold;font-size:14px;">Clúster</td>
          </tr>
          <tr>
            <td align="left" class="style3">
                <asp:Label ID="LabelCluster3" runat="server" Text=""></asp:Label>
            </td>
          </tr>
          
        </table>
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style10">Empleos</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
                <!--------------------------------->
    <table width='98%'  bgcolor="#00468F" style="color: #FFFFFF">
								<tr>
									<td class='Titulotabla'>Empleos Directos</td>
									<td colspan=2 class='Titulotabla'>&nbsp;</td>
									<td class='Titulotabla'>Jovenes</td>
									<td colspan=7 class='Titulotabla' align=center>Población Vulnerable</td>
								</tr>
        </table>
            <asp:GridView ID="GridViewEmpleos"  CssClass="Grilla" runat="server" DataSourceID="LinqDataSourceEmpleos" 
            AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
            Height="30px" Width="98%">
                <Columns>
                    <asp:TemplateField HeaderText="">
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargo">
                        <ItemTemplate>
                            <asp:Label ID="Tx_CargoQ3" runat="server" Text='<%# Eval("cargoQ3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sueldo mensual">
                        <ItemTemplate>
                            <asp:Label ID="Tx_SueldQ3" runat="server" Text='<%# Eval("valormensualQ3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Generado en el primer Año">
                        <ItemTemplate>
                            <asp:Label ID="Tx_PrimerAñoQ3" runat="server" Text='<%# Eval("GeneradoPrimerAnoQ3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Entre 18 y 24 años">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkjovenQ3" runat="server" Checked='<%# Eval("JovenQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Desplazado por la violencia">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkDesplQ3" runat="server" Checked='<%# Eval("DesplazadoQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Madre cabeza de familia">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkMadrQ3" runat="server" Checked='<%# Eval("MadreQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Minoría étnica">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkMinorQ3" runat="server" Checked='<%# Eval("MinoriaQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recluido en cárceles INPEC">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkrecluQ3" runat="server" Checked='<%# Eval("RecluidoQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Desmovilizado o reinsertado">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkdesmovQ3" runat="server" Checked='<%# Eval("DesmovilizadoQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discapacitado">
                        <ItemTemplate>
                           <asp:CheckBox ID="checkdiscq3" runat="server" Checked='<%# Eval("DiscapacitadoQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Desvinculado de entidades del estado">
                        <ItemTemplate >
                            <asp:CheckBox ID="checkdesvQ3" runat="server" Checked='<%# Eval("DesvinculadoQ3")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style10">Emprendedores</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->
            <asp:GridView ID="GridViewEmprendedor"  CssClass="Grilla" runat="server" DataSourceID="LinqDataSourceEmp" 
            AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
            Height="30px" Width="98%">
                <Columns>
                    <asp:TemplateField HeaderText="">
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <asp:Label ID="Tx_NombreQ4" runat="server" Text='<%# "" + Eval("NombresQ4") + " " + Eval("ApellidosQ4") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Beneficiario Fonade">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkbeneficiario" runat="server" Checked='<%# Eval("beneficiarioQ4") == null ? false : Eval("beneficiarioQ4") %>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Participación Accionaria">
                        <ItemTemplate >
                            <asp:Label ID="Tx_partaccion" runat="server" Text='<%# Eval("participacionQ4") == null ? "0" : Eval("participacionQ4") + " %"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        <!--------------------------------->
        <table class="style9">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td bgcolor="#00468F">&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <!--------------------------------->
         <table class="style1">
          <tr>
            <td align="left" class="style10"><!--Aval--></td>
          </tr>
          <tr>
            <td align="left" class="style3">
                <!--
               <asp:TextBox ID="Txt_Aval" runat="server" Text="" MaxLength="500" TextMode="MultiLine" Width="400px">
                </asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        BackColor="White" ControlToValidate="Txt_Aval" 
                        ErrorMessage="* Este campo está vacío y es obligatorio" 
                        style="font-size: small; color: #FF3300;">
                </asp:RequiredFieldValidator>-->
            </td>
          </tr>
          <tr>
            <td align="left" class="style10"  width="21%">Convocatoria:   
                 <asp:DropDownList ID="DropDownListConvoct" runat="server" Height="25px" Width="312px" >
                 </asp:DropDownList>
<%--                 <asp:RequiredFieldValidator ID="RequiredFieldValidator435" runat="server" 
                        BackColor="White" ControlToValidate="DropDownListConvoct" 
                        ErrorMessage="* No hay convocatorias vigentes" 
                        style="font-size: small; color: #FF3300;">
                </asp:RequiredFieldValidator>--%>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="* Seleccione un valor diferente para el campo Convocatoria"  BackColor="White" ControlToValidate="DropDownListConvoct" 
                                style="font-size: small; color: #FF3300;" ClientValidationFunction="ValidaSeleccion"></asp:CustomValidator>
             </td>
          </tr>
          <tr>
            <td align="left" class="style10">Observaciones</td>
          </tr>
          <tr>
            <td align="left" class="style3">
                <asp:TextBox ID="txt_Observaciones" runat="server" Text="" MaxLength="500" TextMode="MultiLine" Width="400px"></asp:TextBox>
                   <!-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        BackColor="White" ControlToValidate="txt_Observaciones" 
                        ErrorMessage="* Este campo está vacío y es obligatorio" 
                        style="font-size: small; color: #FF3300;">
                    </asp:RequiredFieldValidator>-->
            </td>
          </tr>
        </table>
        <!--------------------------------->
        <table class="style9" >
          <tr>
            <td class="auto-style2">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td align="center" class="auto-style2">
                <asp:Button ID="BotónFormalizar" runat="server" Text="Formalizar proyecto"  onclick="ButtonGuardar_Click" />
            </td>
            <td align="center">
                 <asp:Button ID="BotonAnexos" runat="server" Text="Generar Anexos" Visible="false" CausesValidation="false" onclick="BotonAnexos_Click" />
            </td>
            <td align="center">
                 <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" Visible="true" PostBackUrl="~/FONADE/Proyecto/ProyectoFormalizar.aspx" />

            </td>
          </tr>
         </table>
        <br />
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2 {
            width: 394px;
        }
    </style>
</asp:Content>

