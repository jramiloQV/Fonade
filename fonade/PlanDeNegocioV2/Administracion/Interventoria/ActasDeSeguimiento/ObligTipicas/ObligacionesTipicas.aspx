<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="ObligacionesTipicas.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.ObligTipicas.ObligacionesTipicas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Obligaciones Tipicas</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 57px;
        }
        .auto-style2 {
            width: 108px;
        }
        .auto-style3 {
            width: 125px;
        }
        .auto-style4 {
            width: 136px;
        }
        .auto-style5 {
            width: 90px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="4. VERIFICAR OBLIGACIONES TÍPICAS PARA LOS COMERCIANTES"></asp:Label>
            </h1>
        </div>
         <div>
             <h2>4.1 Verificar el Cumplimiento de las obligaciones Contables</h2>
            <asp:GridView ID="gvObligacionContable" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                OnPageIndexChanging="gvObligacionContable_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="estadosFinancieros" HeaderText="Estado Financiero" />
                    <asp:BoundField DataField="librosComerciales" HeaderText="Libros Comerciales" />                   
                    <asp:BoundField DataField="librosContabilidad" HeaderText="Libros de Contabilidad" />                   
                    <asp:BoundField DataField="conciliacionBancaria" HeaderText="Conciliaciones Bancarias" />                   
                    <asp:BoundField DataField="cuentaBancaria" HeaderText="Cuenta Bancaria" />                   
                    <asp:BoundField DataField="observObligacionContable" HeaderText="Observaciones" />                   
                </Columns>
            </asp:GridView> 
             <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style1">Visita</td>
                     <td class="auto-style2">Estado Financiero</td>
                     <td class="auto-style2">Libros Comerciales</td>
                     <td class="auto-style3">Libros de Contabilidad</td>
                     <td class="auto-style4">Conciliaciones Bancarias</td>
                     <td class="auto-style5">Cuenta Bancaria</td>
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td class="auto-style1" style="text-align:center">
                        <asp:Label ID="lblnumV" runat="server" Text=""></asp:Label>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlEstadoFinanciero" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlLibrosComerciales" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style3">
                         <asp:DropDownList ID="ddlLibrosContabilidad" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style4">
                         <asp:DropDownList ID="ddlConciliacionesBancarias" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlCuentaBancaria" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td>
                        <asp:TextBox ID="txtObservacionContable" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="97%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="7">
                         <asp:Button ID="btnGuardarOblContable" runat="server" Height="30px" Text="Guardar" OnClick="btnGuardarOblContable_Click" />
                     </td>
                 </tr>
             </table> 
        </div>
       
        <hr />
        <div>
            <h2>4.2 Verificar el Cumplimiento de las Obligaciones Tributarias</h2>
            <asp:GridView ID="gvObligacionTributaria" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                OnPageIndexChanging="gvObligacionTributaria_PageIndexChanging"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="declaraReteFuente" HeaderText="Declaraciones Retención en la Fuente" />
                    <asp:BoundField DataField="autorretencionRenta" HeaderText="Autorretención Renta" />                   
                    <asp:BoundField DataField="declaraIva" HeaderText="Decla. de IVA" />                   
                    <asp:BoundField DataField="declaImpConsumo" HeaderText="Decla. de Impuesto al Consumo" />                   
                    <asp:BoundField DataField="declaRenta" HeaderText="Decla. de Renta" />                   
                    <asp:BoundField DataField="declaInfoExogena" HeaderText="Decla. Info Exogena" />   
                    <asp:BoundField DataField="declaIndustriaComercio" HeaderText="Decla. Industria y C/cio" /> 
                    <asp:BoundField DataField="declaRetencionImpIndusComercio" 
                        HeaderText="Decla. Retención Impuesto Industria y C/cio" />  
                    <asp:BoundField DataField="observObligacionTributaria" HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>    
            <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td class="auto-style1">Visita</td>
                     <td class="auto-style2">Declaraciones Retención en la Fuente</td>
                     <td class="auto-style2">Autorretención Renta</td>
                     <td class="auto-style3">Decla. de IVA</td>
                     <td class="auto-style4">Decla. de Impuesto al Consumo</td>
                     <td class="auto-style5">Decla. de Renta</td>
                     <td class="auto-style5">Decla. Info Exogena</td>
                     <td class="auto-style5">Decla. Industria y C/cio</td>
                     <td class="auto-style5">Decla. Retención Impuesto Industria y C/cio</td>
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td class="auto-style1" style="text-align:center">
                        <asp:Label ID="lblNumVT" runat="server" Text=""></asp:Label>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlDeclaReteFuente" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlAutoRenta" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style3">
                         <asp:DropDownList ID="ddlDeclaIVA" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style4">
                         <asp:DropDownList ID="ddlDeclaImpConsumo" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlDeclaRenta" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlDeclaExogena" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlDeclaIndustria" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlDeclaReteImpIndustria" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td>
                        <asp:TextBox ID="txtObservTributaria" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="95%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="10">
                         <asp:Button ID="btnGuardarOblTributaria" runat="server" Height="30px" Text="Guardar" OnClick="btnGuardarOblTributaria_Click" />
                     </td>
                 </tr>
             </table> 
        </div>
        <hr />
        <div>
            <h2>4.3 Verificar el Cumplimiento de las Obligaciones Laborales</h2>
            <asp:GridView ID="gvObligacionLaboral" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                OnPageIndexChanging="gvObligacionLaboral_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>                    
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="contratosLaborales" HeaderText="Contratos Laborales" />
                    <asp:BoundField DataField="pagosNomina" HeaderText="Pagos de Nomina" />                   
                    <asp:BoundField DataField="pagoPrestacionesSociales" HeaderText="Pago de Prestaciones sociales" />     
                    <asp:BoundField DataField="afiliacionSegSocial" HeaderText="Afiliacion Seguridad Social" />                   
                    <asp:BoundField DataField="pagoSegSocial" HeaderText="Pagos Seguridad Social" />                   
                    <asp:BoundField DataField="certParafiscalesSegSocial" 
                        HeaderText="Certificado Paz y Salvo de Parafiscales y Seg. Social" />                   
                    <asp:BoundField DataField="reglaInternoTrab" HeaderText="Reglamento Interno de Trabajo" />   
                    <asp:BoundField DataField="sisGestionSegSaludTrabajo" 
                        HeaderText="Sis. Gest. de Seg. y Salud en el Trabajo" /> 
                      
                    <asp:BoundField DataField="observObligacionLaboral" HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>     
             <table style="width: 100%;">
                
                 <tr style="background-color: #00468f;color: white;">
                     <td>Visita</td>
                     <td class="auto-style2">Contratos Laborales</td>
                     <td class="auto-style2">Pagos de Nomina</td>
                     <td class="auto-style2">Pago de Prestaciones Sociales</td>
                     <td class="auto-style3">Afiliacion Seguridad Social</td>
                     <td class="auto-style4">Pagos Seguridad Social</td>
                     <td class="auto-style5">Certificado Paz y Salvo de Parafiscales y Seg. Social</td>
                     <td class="auto-style5">Reglamento Interno de Trabajo</td>
                     <td class="auto-style5">Sis. Gest. de Seg. y Salud en el Trabajo</td>
                     
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td style="text-align:center">
                        <asp:Label ID="lblNumVL" runat="server" Text=""></asp:Label>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlContratoLaboral" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlPagoNomina" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlPagoPrestaciones" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style3">
                         <asp:DropDownList ID="ddlAfilSegSocial" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style4">
                         <asp:DropDownList ID="ddlPagoSegSocial" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlCertParafiscal" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlRegIntTrabajo" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlGestSegSalud" runat="server">
                         </asp:DropDownList>
                     </td>
                     
                     <td>
                        <asp:TextBox ID="txtObservacionLaboral" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="95%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="9">
                         <asp:Button ID="btnGuardarOblLaboral" runat="server" Height="30px" Text="Guardar" OnClick="btnGuardarOblLaboral_Click" />
                     </td>
                 </tr>
             </table> 
        </div>
        <hr />
        <div>
            <h2>4.4 Verificar Registros, Trámites y Licencias</h2>
            <asp:GridView ID="gvRegistrosTramitesLicencias" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se ha registrado indicador."
                OnPageIndexChanging="gvRegistrosTramitesLicencias_PageIndexChanging"
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="insCamaraComercio" 
                        HeaderText="Inscripción Cámara de C/cio" />
                    <asp:BoundField DataField="renovaRegistroMercantil" 
                        HeaderText="Renovación de Registro Mercantil" />                   
                    <asp:BoundField DataField="rut" HeaderText="RUT" />                   
                    <asp:BoundField DataField="resolFacturacion" HeaderText="Resolución Facturación" />                   
                    <asp:BoundField DataField="certLibertadTradicion" 
                        HeaderText="Certificado de Libertad y Tradición" />                   
                    <asp:BoundField DataField="DocumentoIdoneidad" 
                        HeaderText="Documento de Idoneidad" />   
                    <asp:BoundField DataField="permisoUsoSuelo" 
                        HeaderText="Permiso Uso de Suelo" /> 
                    <asp:BoundField DataField="certBomberos" HeaderText="Certificado de Bomberos" /> 
                    <asp:BoundField DataField="regMarca" HeaderText="Reg. de Marca" /> 
                    <asp:BoundField DataField="otrosPermisos" HeaderText="Otros Permisos" /> 
                    <asp:BoundField DataField="contratoArrendamiento" HeaderText="Contrato de Arrendamiento" /> 
                    <asp:BoundField DataField="observRegistroTramiteLicencia" 
                        HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>       
             <table style="width: 100%;">
                 
                 <tr style="background-color: #00468f;color: white;">
                     <td>Visita</td>
                     <td class="auto-style2">Inscripción Cámara de C/cio</td>
                     <td class="auto-style2">Renovación de Registro Mercantil</td>
                     <td class="auto-style3">RUT</td>
                     <td class="auto-style4">Resolución Facturación</td>
                     <td class="auto-style5">Certificado de Libertad y Tradición</td>
                     <td class="auto-style5">Documento de Idoneidad</td>
                     <td class="auto-style5">Permiso Uso de Suelo</td>
                     
                     <td class="auto-style5">Certificado de Bomberos</td>
                     
                     <td class="auto-style5">Reg. de Marca</td>
                     
                     <td class="auto-style5">Otros Permisos</td>

                     <td class="auto-style5">Contrato de Arrendamiento</td>
                     
                     <td>Observaciones</td>
                 </tr>
                 <tr>
                     <td style="text-align:center">
                        <asp:Label ID="lblNumVR" runat="server"></asp:Label>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlInsCamara" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style2">
                         <asp:DropDownList ID="ddlRenMercantil" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style3">
                         <asp:DropDownList ID="ddlRUT" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style4">
                         <asp:DropDownList ID="ddlResFacturacion" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlCertLibertad" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlAvalEmprendimiento" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlPermisoSuelo" runat="server">
                         </asp:DropDownList>
                     </td>
                     
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlCertBomberos" runat="server">
                         </asp:DropDownList>
                     </td>
                     
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlRegMarca" runat="server">
                         </asp:DropDownList>
                     </td>
                     
                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlOtrosPermisos" runat="server">
                         </asp:DropDownList>
                     </td>

                     <td class="auto-style5">
                         <asp:DropDownList ID="ddlContratoArrendamiento" runat="server">
                         </asp:DropDownList>
                     </td>
                     
                     <td>
                        <asp:TextBox ID="txtObservTramites" runat="server" TextMode="MultiLine"
                            MaxLength ="10000" Width="95%"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align:center" colspan="12">
                         <asp:Button ID="btnGuardarOblTramites" runat="server" Height="30px" Text="Guardar" OnClick="btnGuardarOblTramites_Click" />
                     </td>
                 </tr>
             </table> 
        </div>
        <hr />
    </form>
</body>
</html>
