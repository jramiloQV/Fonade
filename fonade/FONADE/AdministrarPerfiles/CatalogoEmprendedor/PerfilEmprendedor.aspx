<%@ Page Language="C#"  MasterPageFile="FrameEmprendedor.master" AutoEventWireup="true" CodeBehind="PerfilEmprendedor.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.CatalogoEmprendedor.PerfilEmprendedor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyHolder">
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager> 
    <h1><asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
   
    <asp:Panel  ID="pnl_crearEditar"  runat="server">
    <asp:Table ID="tbl_Convenio" runat="server">
      <asp:TableRow>
            <asp:TableCell>Nombres:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="tb_Convenio" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>Apellidos:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>

          <asp:TableRow>
            <asp:TableCell>Identificación:</asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="ddl_Tipodocumento"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>No:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator5" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>Departamento expedición:</asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList1"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator7" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>

         <asp:TableRow>
            <asp:TableCell>Ciudad expedición:</asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList2"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator8" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            <asp:TableCell>Correo electrónico:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator9" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Género:</asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList3"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator10" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Fecha Nacimiento:</asp:TableCell>
            <asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox4"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="Image1" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender2" PopupButtonID="img_dateInicio"  CssClass="ajax__calendar" TargetControlID="tb_fechaInicio" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Departamento nacimiento:</asp:TableCell><asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList4"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator11" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Ciudad nacimiento:</asp:TableCell><asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList5"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator12" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Teléfono:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox5"  Text="11/01/2006" />
                        
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Nivel de estudio:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="tb_nivelestudio"  Text="11/01/2006" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Programa Realizado:</asp:TableCell><asp:TableCell>
            <asp:TextBox ID="tb_Programarealizado" runat="server"></asp:TextBox>
            <asp:ImageButton ID="imbtn_institucion"  runat="server" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Institución:</asp:TableCell><asp:TableCell>
            <asp:TextBox ID="tb_Institucion"   runat="server"></asp:TextBox>
            <asp:ImageButton ID="imbtn_nivel"   runat="server" />
                        </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Ciudad Institución:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox7"  Text="11/01/2006" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Estado:</asp:TableCell><asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList6"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator13" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fecha Nacimiento:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox8"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="Image2" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender3" PopupButtonID="img_dateInicio"  CssClass="ajax__calendar" TargetControlID="tb_fechaInicio" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fecha Finalización de Materias:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox9"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="Image3" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender4" PopupButtonID="img_dateInicio"  CssClass="ajax__calendar" TargetControlID="tb_fechaInicio" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fecha graduación:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="TextBox10"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="Image4" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender5" PopupButtonID="img_dateInicio"  CssClass="ajax__calendar" TargetControlID="tb_fechaInicio" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>¿Usted tiene alguna condición especial?:</asp:TableCell><asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList7"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator14" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Tipo de Aprendiz:</asp:TableCell><asp:TableCell>
                <asp:DropDownList runat="server" ID="DropDownList8"></asp:DropDownList>
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator15" runat="server" ControlToValidate="tb_Convenio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Descripción:</asp:TableCell><asp:TableCell><asp:TextBox ID="tb_Descripcion" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_Descripcion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fecha inicio:</asp:TableCell><asp:TableCell>
            
            <asp:TextBox runat="server" ID="tb_fechaInicio"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="img_dateInicio" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender1" PopupButtonID="img_dateInicio"  CssClass="ajax__calendar" TargetControlID="tb_fechaInicio" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fecha fin:</asp:TableCell><asp:TableCell>
            <asp:TextBox runat="server" ID="tb_fechaFin"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="img_dateFin" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                        <ajaxtoolkit:calendarextender runat="server"   ID="calExtender2" PopupButtonID="img_dateFin"  CssClass="ajax__calendar" TargetControlID="tb_fechaFin" Format="MMMM d, yy" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Fiduciaria:</asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="ddl_fiduciaria" runat="server">
                </asp:DropDownList> 
                <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddl_fiduciaria" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow></asp:Table><asp:CheckBox  ID="chkbx_terminarPerfil" runat="server"/>
    </asp:Panel>
   </asp:Content>