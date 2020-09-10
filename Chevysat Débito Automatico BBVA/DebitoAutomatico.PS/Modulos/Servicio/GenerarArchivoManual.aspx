<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="GenerarArchivoManual.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.GenerarArchivoManual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 284px;
        }
        .auto-style2 {
            width: 284px;
            margin-left: 40px;
        }
        .auto-style3 {
            width: 285px;
            margin-left: 40px;
        }
        .auto-style4 {
            width: 257px;
            margin-left: 40px;
        }
        .auto-style5 {
            width: 238px;
        }
        .auto-style7 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 13%;
        }
        .auto-style8 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 174px;
        }
        .auto-style9 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                    Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Generar Archivo Manual
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
                                         <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lblContrato" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
                                            <asp:TextBox ID="txbContrato" runat="server" CssClass="BordeControles" MaxLength="10"
                                                Width="130px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbContrato_TextChanged"></asp:TextBox>
                                                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                FilterType="Numbers" TargetControlID="txbContrato">
                                            </asp:FilteredTextBoxExtender>
                                           
                                             <asp:TextBox ID="txbDigito" runat="server" CssClass="BordeControles" MaxLength="1"
                                                Width="10px" Enabled="False"></asp:TextBox>
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
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNombreCliente" runat="server" Text="Nombre del Cliente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
                                            <asp:TextBox ID="txbNombreCliente" runat="server" CssClass="BordeControles" Width="220px"
                                                ValidationGroup="1" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTipoIdentificacionC" runat="server" Text="Tipo de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
                                            <asp:DropDownList ID="ddlTipoIdentificacionC" runat="server" CssClass="BordeListas"
                                                ValidationGroup="1" Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentificacionC" runat="server" Text="Número de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl" style="left:auto">
                                            <asp:TextBox ID="txbIdentificacionC" runat="server" CssClass="BordeControles" MaxLength="50"
                                                ValidationGroup="1" Width="130px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                  
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoCont" runat="server" Text="Estado del Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
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
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%">
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
                                        <td class="auto-style8">
                                            <asp:Label ID="lbBanco" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddlBanco" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                                          <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="Label1" runat="server" Text="Débito a partir del:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                              <asp:DropDownList ID="ddlFechaDebito" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                     Enabled="False">
                                            </asp:DropDownList>
                                        
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style8">
                                            <asp:Label ID="lbTipoCuenta" runat="server" Text="Tipo de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddlTipoCuenta" runat="server"  CssClass="BordeListas" ValidationGroup="1"
                                                Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNumCuenta" runat="server" Text="Número de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNumCuenta" runat="server"  CssClass="BordeControles" MaxLength="50"
                                                ValidationGroup="1" Width="130px" Enabled="False"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revNumCuenta" runat="server" ControlToValidate="txbNumCuenta"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumCuenta" runat="server" Enabled="True" TargetControlID="revNumCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvNumCuenta" runat="server" ControlToValidate="txbNumCuenta"
                                                ErrorMessage="Favor digitar la Cuenta!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1"> </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumCuenta1" runat="server" Enabled="True" TargetControlID="rfvNumCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="lbTercero" runat="server" Text="Cuenta de Tercero?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style2">
                                            <asp:CheckBox ID="chbTercero" runat="server" AutoPostBack="true" Enabled="False" />
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoD" runat="server" Text="Estado Débito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlEstadoD" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                       <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblValorDebitar" Visible="false" runat="server" Text="Valor a Debitar:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style2">
                                              <asp:TextBox ID="txbValorD" Visible="false" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="130px"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revValorD" runat="server" ControlToValidate="txbValorD"
                                                ErrorMessage="Formato Incorrecto!!!" ForeColor="Red" ValidationExpression="^\d+$"
                                                ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceValorD" runat="server" Enabled="True" TargetControlID="revValorD"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvValorD" runat="server" ControlToValidate="txbValorD"
                                                ErrorMessage="Favor digitar el valor a debitar!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceValorD1" runat="server" Enabled="True" TargetControlID="rfvValorD"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                                         <asp:ImageButton Visible="false" ID="imgBuscar" AutoPostBack="true" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/buscar.png"
                                                Width="20px" ValidationGroup="5" OnClick="imgBuscar_Click"/>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                  
             
                   
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
                                        <td class="auto-style9">
                                            <asp:Label ID="lbNombreTercero" runat="server" Text="Nombre del Tercero:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
                                            <asp:TextBox ID="txbNombreTercero" runat="server" CssClass="BordeControles" Width="220px"
                                                ValidationGroup="1" Style='text-transform: uppercase' Enabled="False" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreTercero" runat="server" ErrorMessage="Favor digitar el Nombre del Titular de la Cuenta!"
                                                ForeColor="Red" ControlToValidate="txbNombreTercero" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreTercero" runat="server" Enabled="True"
                                                TargetControlID="rfvNombreTercero" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                PopupPosition="Left" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style9">
                                            <asp:Label ID="lbTipoIdentificacionT" runat="server" Text="Tipo de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style3">
                                            <asp:DropDownList ID="ddlTipoIdentificacionT" runat="server" CssClass="BordeListas"
                                                ValidationGroup="1" Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentificacionT" runat="server" Text="Número de Identificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbIdentificacionT" runat="server" CssClass="BordeControles" MaxLength="50"
                                                ValidationGroup="1" Width="130px" Enabled="False"></asp:TextBox>
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
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="recTercero" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlTercero" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlDebito" CssClass="PanelBordesRedondos" runat="server" Width="99%" Visible="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                            <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lblFechaDebito" runat="server" Text="Fecha Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txbFechaProceso" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                                ValidationGroup="1" Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaDebito" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaDebito" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaDebito" TargetControlID="txbFechaProceso">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaDebito" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaProceso" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="rfvFechaDebito" runat="server" ControlToValidate="txbFechaProceso"
                                                ErrorMessage="Favor digitar o seleccionar del calendariola fecha inicial!" ForeColor="Red"
                                                SetFocusOnError="true" Text="*" ValidationGroup="2"> </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaDebito" runat="server" Enabled="True" HighlightCssClass="Resaltar"
                                                TargetControlID="rfvFechaDebito">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                                <td class="auto-style7">
                                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar Contrato" Height="20px"
                                                Width="122px" Enabled="False" OnClick="btnAgregar_Click" ValidationGroup="1" />

                                                </td>
                                                     <td class="EspaciadoCeldaControl">
                                              
                                        </td>
                                               
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlBancoDebito" runat="server" CssClass="BordeListas" ValidationGroup="2"
                                                OnSelectedIndexChanged="ddlBancoDebito_SelectedIndexChanged" Width="205px" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlBancoDebito" InitialValue="0" ValidationGroup="2"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBanco" runat="server" Enabled="True" TargetControlID="rfvBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="2" align="right">
                                            
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceDebito" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDebito" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                             <asp:Panel ID="pnlGrilla" CssClass="PanelBordesRedondos" runat="server" Width="99%" Visible="false" > 
                            <table width="99%">
                                <tr>
                                        <td class="LetraLeyendaColor" colspan="3">Contratos Manuales:
                                                    <asp:Label ID="lbNManual" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                <tr>
                                    <td class="EspaciadoInicial"></td>
                                    <td align="center" style="height: 110px;  overflow: auto;">
                                        <asp:GridView ID="gvDebitos" runat="server" AutoGenerateColumns="False" Width="80%"
                                            AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                            GridLines="None" OnRowCommand="gvDebitos_RowCommand" CssClass="EstiloEtiquetas81">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                    Text="Eliminar">
                                                    <ItemStyle Width="30px" />
                                                    <ControlStyle Width="16px" />
                                                </asp:ButtonField>
                                                <asp:BoundField HeaderText="CONTRATO" DataField="pContrato">
                                                    <ControlStyle Width="200px" />
                                                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="NOMBRE DEL CLIENTE" DataField="pNombre"></asp:BoundField>
                                                <asp:BoundField HeaderText="NÚMERO DE CUENTA" DataField="pNumeroCuenta"></asp:BoundField>
                                                   <asp:BoundField HeaderText="Estado" DataField="pEstadoCliente" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                <asp:BoundField HeaderText="TIPO DE CUENTA" DataField="pTipoCuenta"></asp:BoundField>
                                                <asp:BoundField HeaderText="VALOR" DataField="pValor">
                                                    <ControlStyle Width="100px" />
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="BANCO" DataField="pBanco">
                                                      <ControlStyle Width="200px" />
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle HorizontalAlign="Center" />


                                                </asp:BoundField>
                                              
                                            
                                            </Columns>
                                            <HeaderStyle BackColor="#C5C5C6" />
                                            <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                            <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                            <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                                        </asp:GridView>
                                    </td>
                                    <td class="EspaciadoFinal"></td>
                                </tr>
                                <tr>
                                    <td class="EspaciadoInicial"></td>
                                    <td class="EstiloInformacion">
                                        <asp:Button ID="btnGenerarArchivo" runat="server" Text="Generar Archivo" Height="30px"
                                            Width="122px" ValidationGroup="2" OnClick="btnGenerarArchivo_Click" />
                                    </td>
                                    <td class="EspaciadoFinal"></td>
                                </tr>
                                <tr>
                                   <td style="height: 10px" colspan="9">

                                </tr>
                            </table>
                                 </asp:Panel>
                             <asp:RoundedCornersExtender ID="rcePnlGrilla" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlGrilla" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBusquedaContrato" CssClass="ContenedorDatos" runat="server" Width="600px"
                Height="400px" Style="display:none;">
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
            <asp:PostBackTrigger ControlID="btnAgregar" />
            <%--<asp:PostBackTrigger ControlID="btnGenerarArchivo" />--%>
            <asp:PostBackTrigger ControlID="txbContrato" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
