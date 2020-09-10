<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="IngresarCliente.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.IngresarCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style5 {
            width: 296px;
        }
        .auto-style6 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 167px;
        }
        .auto-style7
        {
            padding-left: 5px;
            height: 16px;
            width: 100%;
            background-color: #88AADA;
            font-size: small;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <asp:UpdatePanel ID="UpdatePanelHerramientas" runat="server">
        <ContentTemplate>
            <table align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="CuadranteBotonImagen">
                        <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                            Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
                    </td>
                    <td class="CuadranteBotonImagen">
                        <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png"
                            Width="16px" ToolTip="Guardar" OnClick="imgBtnGuardar_Click" ValidationGroup="1" />
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="UpdateProgressPanelHerramientas" runat="server" AssociatedUpdatePanelID="UpdatePanelHerramientas">
                <ProgressTemplate>
                    <div class="contenedor">
                        <div class="centrado">
                            <div class="contenido">
                                <asp:Image ID="ImagePanelHerramientas" runat="server" ImageUrl="~/MarcaVisual/iconos/loading.gif"
                                    Height="20px" Width="100px" ImageAlign="Middle" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <script type="text/javascript" language="javascript">

        function habilitarControles() 
        {
            var CampoCuenta = document.getElementById("<%= txbNumCuenta.ClientID %>");

               if ('<%= Session["perfil"] %>' == 5)
                CampoCuenta.type = 'password';
            else
                CampoCuenta.type = 'text';
            //soloNumeros(CampoCuenta.id);
        }

        /* Permite digitar únicamente números en el texto */
        function soloNumeros(control) {
            limitarCaracteres(control, "^[0-9]+$");
        }

        /* Únicamente permite digitar los caracteres que cumplan la expresión regular */
        function limitarCaracteres(control, expresion) {
            document.getElementById(control).addEventListener('keypress', function (event)
            {
                var regex = new RegExp(expresion);
                if (esCaracterEspecial(event)) {
                    return true;
                }
                else {
                    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                    if (!regex.test(key)) {
                        event.preventDefault();
                        return false;
                    }
                }
            }
            );
        }

        function esCaracterEspecial(event) {
            var whichCode = !event.charCode ? event.which : event.charCode;

            if (whichCode == 0) return true;
            if (whichCode == 8) return true;
            if (whichCode == 9) return true;
            if (whichCode == 13) return true;
            if (whichCode == 16) return true;
            if (whichCode == 17) return true;
            if (whichCode == 27) return true;
            return false;
        }
        

     </script>

    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="auto-style7">Ingresar cliente
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo"></td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3"></td>
                </tr>
                <tr>
                    <td style="width: 10px"></td>
                    <td>
                        <asp:Panel ID="pnlCliente" runat="server" ScrollBars="Auto" Height="100%" Width="900px">
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Titular del Contrato
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <%--<asp:Button ID="btnContrato" runat="server" Text="Contrato:" />--%>
                                            <asp:Label ID="lblContrato" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbContrato" runat="server" CssClass="BordeControles" MaxLength="12"
                                                Width="110px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbContrato_TextChanged"></asp:TextBox>
                                            <asp:TextBox ID="txbDigito" runat="server" CssClass="BordeControles" MaxLength="1"
                                                Width="10px" Enabled="False"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                FilterType="Numbers" TargetControlID="txbContrato">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revContrato" runat="server" ControlToValidate="txbContrato"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceContrato" runat="server" Enabled="True" TargetControlID="revContrato"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvContrato" runat="server" ControlToValidate="txbContrato"
                                                ErrorMessage="Favor digitar el Contrato!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceContrato1" runat="server" Enabled="True" TargetControlID="rfvContrato"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="lbNombreCliente" runat="server" Text="Nombre del Cliente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreCliente" runat="server" CssClass="BordeControles" Width="220px"
                                                ValidationGroup="1" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="lbTipoIdentificacionC" runat="server" Text="Tipo de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                           <asp:DropDownList ID="ddlTipoIdentificacionC" runat="server" CssClass="BordeListas"
                                                ValidationGroup="1" Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentificacionC" runat="server" Text="Número de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbIdentificacionC" runat="server" CssClass="BordeControles" MaxLength="50"
                                                ValidationGroup="1" Width="130px" Enabled="False"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftbtxbIndetificacionC" runat="server"
                                                TargetControlID="txbIdentificacionC" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>

                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="lbEstadoCont" runat="server" Text="Estado del Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbEstadoCont" runat="server" CssClass="BordeControles" MaxLength="17"
                                                Width="130px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTipoCliente" runat="server" Text="Tipo de Cliente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbTipoCliente" runat="server" CssClass="BordeControles" MaxLength="17"
                                                Width="130px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <%--                 <asp:Label ID="lbCuotas" runat="server" Text="Cuotas en Mora:"></asp:Label>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <%--                <asp:TextBox ID="txbCuotasM" runat="server" CssClass="BordeControles" Width="130px"
                                                ValidationGroup="1" Enabled="False"></asp:TextBox>--%>
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Datos Débito Automático
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBanco" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlBanco" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlBanco" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBanco" runat="server" Enabled="True" TargetControlID="rfvBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                          <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="Label1" runat="server" Text="Débito a partir del:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                              <asp:DropDownList ID="ddlFechaDebito" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvfechadebito" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlFechaDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vcfechadebito" runat="server" Enabled="True" TargetControlID="rfvfechadebito"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTipoCuenta" runat="server" Text="Tipo de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlTipoCuenta" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoCuenta" runat="server" ErrorMessage="Favor seleccionar Tipo de Cuenta!"
                                                ForeColor="Red" ControlToValidate="ddlTipoCuenta" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoCuenta" runat="server" Enabled="True" TargetControlID="rfvTipoCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNumCuenta" runat="server" Text="Número de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNumCuenta" runat="server" AutopostBack="true" CssClass="BordeControles" MaxLength="16"
                                                ValidationGroup="1" Width="130px" OnTextChanged="txbNumCuenta_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftbNumCuenta" runat="server"
                                                TargetControlID="txbNumCuenta" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revNumCuenta" runat="server" ControlToValidate="txbNumCuenta"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumCuenta" runat="server" Enabled="True" TargetControlID="revNumCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvNumCuenta" runat="server" ControlToValidate="txbNumCuenta"
                                                ErrorMessage="Favor digitar la Cuenta!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumCuenta1" runat="server" Enabled="True" TargetControlID="rfvNumCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>


                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbFormaDebito" runat="server" Text="Formato Ingreso:"></asp:Label>

                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style5">
                                             <asp:DropDownList ID="ddlFormaDebito" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFormaDebito" runat="server" ErrorMessage="Favor seleccionar Tipo de Cuenta!"
                                                ForeColor="Red" ControlToValidate="ddlFormaDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFormaDebito" runat="server" Enabled="True" TargetControlID="rfvFormaDebito"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTercero" Visible="false" runat="server" Text="Cuenta de Tercero?:"></asp:Label>
                                            <%--<asp:Label ID="lbNotificarCelular" runat="server" Text="Notificar Celular?:"></asp:Label>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbTercero" Visible="false" runat="server" AutoPostBack="true" OnCheckedChanged="chbTercero_CheckedChanged" />
                                            <%--<asp:CheckBox ID="chbNotificarCelular" runat="server" AutoPostBack="true" Checked="True" />--%>

                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <%--           <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbFechaInicio" runat="server" Text="Fecha Inicio Débito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbFechaInicial" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                                ValidationGroup="1" Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaInicial" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaInicial" TargetControlID="txbFechaInicial">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaInicial" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="rfvFechaInicial" runat="server" ControlToValidate="txbFechaInicial"
                                                ErrorMessage="Favor digitar o seleccionar del calendariola fecha inicial!" ForeColor="Red"
                                                SetFocusOnError="true" Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaInicial" runat="server" Enabled="True"
                                                HighlightCssClass="Resaltar" TargetControlID="rfvFechaInicial">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbFechaFin" runat="server" Text="Fecha Finalización Débito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbFechaFin" runat="server" CssClass="FuenteDDL" TabIndex="3" ValidationGroup="1"
                                                Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaFin" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txbFechaFin"
                                                ErrorMessage="Favor digitar o seleccionar del calendario la fecha final!" ForeColor="Red"
                                                SetFocusOnError="true" Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaFin" runat="server" Enabled="True" HighlightCssClass="Resaltar"
                                                TargetControlID="rfvFechaFin">
                                            </asp:ValidatorCalloutExtender>

                                            <asp:CalendarExtender ID="ceFechaFin" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaFin" TargetControlID="txbFechaFin">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaFin" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaFin" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbValorMax" runat="server" Text="Valor Máximo a Debitar:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbValorMax" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="130px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNotificarCorreo" runat="server" Text="Notificar Correo?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbNotificarCorreo" runat="server" AutoPostBack="true" Checked="True" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>--%>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceBanco" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBanco" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlTercero" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                Visible="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Titular de la Cuenta
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNombreTercero" runat="server" Text="Nombre del Tercero:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreTercero" runat="server" CssClass="BordeControles" Width="220px"
                                                ValidationGroup="1" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreTercero" runat="server" ErrorMessage="Favor digitar el Nombre del Titular de la Cuenta!"
                                                ForeColor="Red" ControlToValidate="txbNombreTercero" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreTercero" runat="server" Enabled="True"
                                                TargetControlID="rfvNombreTercero" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                PopupPosition="Left" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>

                                            <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="UppercaseLetters"
                                             TargetControlID="txbNombreTercero" ValidChars="*" />--%>

                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTipoIdentificacionT" runat="server" Text="Tipo de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlTipoIdentificacionT" runat="server" CssClass="BordeListas"
                                                ValidationGroup="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentificacionT" runat="server" Text="Número de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbIdentificacionT" runat="server" CssClass="BordeControles" MaxLength="50"
                                                ValidationGroup="1" Width="130px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftbIdentificacionT" runat="server"
                                                TargetControlID="txbIdentificacionT" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="revIdentificacionT" runat="server" ControlToValidate="txbIdentificacionT"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceIdentificacionT" runat="server" Enabled="True"
                                                TargetControlID="revIdentificacionT" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                PopupPosition="Left" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvIdentificacionT" runat="server" ControlToValidate="txbIdentificacionT"
                                                ErrorMessage="Favor digitar la Identificación!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceIdentificacionT1" runat="server" Enabled="True"
                                                TargetControlID="rfvIdentificacionT" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                PopupPosition="Left" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <%--<asp:Label ID="lbCelularT" runat="server" Text="Celular:"></asp:Label>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                         <%--   <asp:TextBox ID="txbCelularT" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="130px"></asp:TextBox>--%>
                                            <%--<asp:FilteredTextBoxExtender ID="ftbCelularT" runat="server"
                                                TargetControlID="txbCelularT" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <%--<asp:Label ID="lbCorreoT" runat="server" Text="Correo:"></asp:Label>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                    <%--        <asp:TextBox ID="txbCorreoT" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="200px"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoT" runat="server" ControlToValidate="txbCorreoT"
                                                ErrorMessage="Formato de Correo Incorrecto!!!" ValidationGroup="1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCorreoT" runat="server" Enabled="True" TargetControlID="revCorreoT"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>--%>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="recTercero" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlTercero" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBusquedaContrato" CssClass="ContenedorDatos" runat="server" Width="600px"
                Height="400px" Style="display: none;">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="BarraSubTitulo">B&uacute;squeda
                        </td>
                    </tr>
                    <tr>
                        <td class="SeparadorSubTitulo"></td>
                    </tr>
                </table>
                <table style="width: 100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="EspaciadoInicial"></td>
                        <td>
                            <asp:Panel ID="pnlContratoB" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbContratoB" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbContratoB" runat="server" CssClass="BordeControles" MaxLength="100"
                                                Width="150px" ValidationGroup="3"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revContratoB" runat="server" ControlToValidate="txbContratoB"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="3">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceContratoB" runat="server" Enabled="True" TargetControlID="revContratoB"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentClienteB" runat="server" Text="Identificación Cliente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbIdentClienteB" runat="server" CssClass="BordeControles" MaxLength="100"
                                                Width="150px" ValidationGroup="3"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revIdentClienteB" runat="server" ControlToValidate="txbIdentClienteB"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="3">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceIdentClienteB" runat="server" Enabled="True"
                                                TargetControlID="revIdentClienteB" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                PopupPosition="Left" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondasB" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlContratoB" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </td>
                        <td style="width: 130px; text-align: center; vertical-align: bottom">
                         
                        </td>
                        <td class="EspaciadoInicial"></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="SeparadorHorizontal"></td>
                    </tr>
                </table>
                <table style="width: 100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td colspan="2">
                        
                        </td>
                    </tr>
                </table>
                <div class="BarraEstado">
                    <table class="EstiloBarraEstado">
                        <tr>
                            <td>
                                <asp:Label ID="lbEstadoBusquedaCliente" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <%--           <asp:ModalPopupExtender ID="mpeBusquedaContrato" runat="server" PopupControlID="pnlBusquedaContrato"
                TargetControlID="btnContrato" BackgroundCssClass="VentanaModal" CancelControlID="btnCancelar">
            </asp:ModalPopupExtender>--%>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upContenido">
                <ProgressTemplate>
                    <div class="contenedor">
                        <div class="centrado">
                            <div class="contenido" style="width: 100px; height: 20px">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/MarcaVisual/iconos/loading.gif"
                                    Height="20px" Width="100px" ImageAlign="Middle" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="txbContrato" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
