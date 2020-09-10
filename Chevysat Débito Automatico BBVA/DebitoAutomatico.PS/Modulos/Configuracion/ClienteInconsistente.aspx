<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="ClienteInconsistente.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.IngresarClienteInconsistente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .style1 {
            width: 7px;
        }

        .auto-style3 {
            width: 296px;
        }

        .auto-style4 {
            width: 301px;
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
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png"
                    Width="16px" ToolTip="Guardar" OnClick="imgBtnGuardar_Click" ValidationGroup="1" />
            </td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="CuadranteBotonImagen"></td>
            <td class="EstiloEtiquetas88">
                <asp:Label ID="lblPrenotificar" runat="server" Text="Prenotificar"></asp:Label></td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnExportar" runat="server" ImageUrl="~/MarcaVisual/iconos/activar_todas.png"
                    Width="16px" ToolTip="Prenotificar" ValidationGroup="3" OnClick="imgBtnExportar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Clientes inconsistentes
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
                                        <td class="EstiloEtiquetas80">
                                            <%--<asp:Button ID="btnContrato" runat="server" Text="Contrato:" />--%>
                                            <asp:Label ID="lblContrato" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbContrato" runat="server" CssClass="BordeControles" MaxLength="12"
                                                Width="110px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbContrato_TextChanged"></asp:TextBox>
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
                                        <td class="EspaciadoCeldaControl">
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
                                        <td class="EstiloEtiquetas80">
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
                                        <td style="width: 20%" class="EstiloEtiquetas80">
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
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlBanco" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                              <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar la entidad bancaria!"
                                                ForeColor="Red" ControlToValidate="ddlBanco" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBanco" runat="server" Enabled="True" TargetControlID="rfvBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                           <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="Label1" runat="server" Text="Débito a partir de:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                              <asp:DropDownList ID="ddlFechaDebito" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvfechadebito" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlFechaDebito" InitialValue="0" ValidationGroup="3"
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
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlTipoCuenta" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                              <asp:RequiredFieldValidator ID="rfvTipoCuenta" runat="server" ErrorMessage="Favor seleccionar el tipo de cuenta!"
                                                ForeColor="Red" ControlToValidate="ddlTipoCuenta" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoCuenta" runat="server" Enabled="True" TargetControlID="rfvTipoCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNumCuenta" runat="server" Text="Número de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNumCuenta" runat="server" CssClass="BordeControles" MaxLength="17"
                                                ValidationGroup="1" Width="130px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNumCuenta" runat="server" ControlToValidate="txbNumCuenta"
                                                ErrorMessage="Favor digitar la Cuenta!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="3">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumCuenta" runat="server" Enabled="True" TargetControlID="rfvNumCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                           <asp:FilteredTextBoxExtender ID="frbNumCuenta" runat="server"
                                                TargetControlID="txbNumCuenta" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblTipoInconsistencia" runat="server" Text="Tipo de Inconsistencia:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlTipoInconsistencia" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                              <asp:RequiredFieldValidator ID="rfvTipoIncosnistencia" runat="server" ErrorMessage="Favor seleccionar el tipo de inconsistencia!"
                                                ForeColor="Red" ControlToValidate="ddlTipoInconsistencia" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoInconsistencia" runat="server" Enabled="True" TargetControlID="rfvTipoIncosnistencia"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbFormaDebito" runat="server" Text="Formato Debito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlFormaDebito" runat="server" CssClass="BordeListas" ValidationGroup="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFormaDebito" runat="server" ErrorMessage="Favor seleccionar el formato de ingreso!"
                                                ForeColor="Red" ControlToValidate="ddlFormaDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFormaDebito" runat="server" Enabled="True" TargetControlID="rfvFormaDebito"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbObservaciones" runat="server" Text="Observaciones:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl" colspan="5">
                                            <asp:TextBox ID="txbObservaciones" runat="server" CssClass="BordeControles" Width="423px"
                                                ValidationGroup="1" MaxLength="100" Height="70px" TextMode="MultiLine"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server" ControlToValidate="txbObservaciones"
                                                ErrorMessage="Favor digitar la Cuenta!" ForeColor="Red" SetFocusOnError="true"
                                                Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceObservaciones" runat="server" Enabled="True" TargetControlID="rfvObservaciones"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                         
                    
      
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTercero" runat="server" Text="Cuenta de Tercero?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style4">
                                            <asp:CheckBox ID="chbTercero" runat="server" AutoPostBack="true" OnCheckedChanged="chbTercero_CheckedChanged" />
                                        </td>
                                        <td colspan="5"></td>
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
                                        </td>
                                        <td colspan="5"></td>
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
                        <td style="width: 130px; text-align: center; vertical-align: bottom"></td>
                        <td class="EspaciadoInicial"></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="SeparadorHorizontal"></td>
                    </tr>
                </table>
                <table style="width: 100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td colspan="2"></td>
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
            <asp:PostBackTrigger ControlID="txbContrato" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
