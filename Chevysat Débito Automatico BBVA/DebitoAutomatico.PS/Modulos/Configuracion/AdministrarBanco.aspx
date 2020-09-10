<%@ Page Title="Débito Automático" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="AdministrarBanco.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.AdministrarBanco" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
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
            <td class="CuadranteBotonImagen">
                <%--<asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                    Width="16px" ToolTip="Eliminar" OnClick="imgBtnEliminar_Click" Style="height: 16px" />
                <asp:ConfirmButtonExtender ID="cbeEliminar" runat="server" TargetControlID="imgBtnEliminar"
                    ConfirmText="Esta seguro de eliminar el registro?">
                </asp:ConfirmButtonExtender>--%>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Ingresar Banco
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
                        <asp:Panel ID="pnlBanco" runat="server" ScrollBars="Auto" Height="610px" Width="800px">
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Button ID="btnCodigoBanco" runat="server" Text="Código Banco:" />
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigoBanco" runat="server" CssClass="BordeControles" MaxLength="5"
                                                Width="30px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbCodigoBanco_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigoBanco" runat="server" ErrorMessage="Favor digitar el Codigo del Banco!"
                                                ForeColor="Red" ControlToValidate="txbCodigoBanco" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigoBanco" runat="server" Enabled="True" TargetControlID="rfvCodigoBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:FilteredTextBoxExtender ID="frbtxbCodigoBanco" runat="server"
                                                TargetControlID="txbCodigoBanco" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNombreBanco" runat="server" Text="Nombre del Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreBanco" runat="server" CssClass="BordeControles" MaxLength="50" AutoPostBack="true"
                                                Width="150px" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreBanco" runat="server" ErrorMessage="Favor digitar el Nombre del Banco!"
                                                ForeColor="Red" ControlToValidate="txbNombreBanco" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreBanco" runat="server" Enabled="True" TargetControlID="rfvNombreBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblNit" runat="server" Visible="false" Text="Nit del Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNit" runat="server" Visible="false" CssClass="BordeControles" MaxLength="30" Width="150px"></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="revNit" runat="server" ValidationGroup="1"
                                                ControlToValidate="txbRemitente" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvNit" runat="server" ErrorMessage="Favor digitar el Nit!"
                                                ForeColor="Red" ControlToValidate="txbNit" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNit" runat="server" Enabled="True" TargetControlID="revNit"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:ValidatorCalloutExtender ID="vceNit2" runat="server" Enabled="True"
                                                TargetControlID="rfvNit" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:FilteredTextBoxExtender ID="ftbtxbNit" runat="server"
                                                TargetControlID="txbNit" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80"></td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl"></td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoE" runat="server" Text="Se Encuentra Activo?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbEstaActivo" runat="server" AutoPostBack="True" OnCheckedChanged="chbEstaActivo_CheckedChanged" />

                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDe" runat="server" Text="Es banco Débito Automático?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbBancoDeb" runat="server" AutoPostBack="True" OnCheckedChanged="chbBancoDeb_CheckedChanged" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblDebitoParcial" runat="server" Text="Debito Parcial:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbDebitoParcial" runat="server" AutoPostBack="True" OnCheckedChanged="chbDebitoParcial_CheckedChanged" />
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lblMonto" runat="server" Visible ="false" Text="Valor maximo:"></asp:Label>

                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbValorParcial" Visible="false" runat="server" CssClass="BordeControles" AutoPostBack="true" MaxLength="30" Width="150px" OnTextChanged="txbValorParcial_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txbValorParcial"  ValidChars="," FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFTP" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlFTP" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="6">Correos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="LblRemitente" runat="server" Text="Correo Remitente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl" colspan="2">
                                            <asp:TextBox ID="txbRemitente" CssClass="BordeControles" runat="server" Width="250px"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revRemitente" runat="server" ValidationGroup="1"
                                                ControlToValidate="txbRemitente" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvRemitente" runat="server" ErrorMessage="Favor digitar el Remitente!"
                                                ForeColor="Red" ControlToValidate="txbRemitente" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceRemitente" runat="server" Enabled="True" TargetControlID="revRemitente"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                TargetControlID="rfvRemitente" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                       <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="LbCorreoEnvio" runat="server" Text="Correos Para:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbCorreoEnvio" CssClass="BordeControles" Enabled="True" runat="server"
                                                Width="250px" MaxLength="200" ValidationGroup="3"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoEnvio" runat="server" ValidationGroup="3"
                                                ControlToValidate="txbCorreoEnvio" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCorreoEnvio" runat="server" Enabled="True" TargetControlID="revCorreoEnvio"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                Width="16px" OnClick="imgAgregarCorreoEnvio_Click" ValidationGroup="3" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                        <td>
                                            <asp:ListBox ID="LtbCorreoEnvio" runat="server" Width="400px" CssClass="Bordes" Height="64px"></asp:ListBox>
                                        </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton ID="imgBorrarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                Width="16px" OnClick="imgBorrarCorreoEnvio_Click" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbCorreoControl" runat="server" Text="Correos con Copia:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCorreoControl" CssClass="BordeControles" runat="server" Width="250px"
                                                MaxLength="100" ValidationGroup="2"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoControl" runat="server" ControlToValidate="txbCorreoControl"
                                                ErrorMessage="Formato de Correo Incorrecto!!!" ValidationGroup="2" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCorreoControl" runat="server" Enabled="True"
                                                TargetControlID="revCorreoControl" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                Width="16px" OnClick="imgAgregarCorreoControl_Click" ValidationGroup="2" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                        <td>
                                            <asp:ListBox ID="LtbCorreoControl" runat="server" Width="400px" CssClass="Bordes"
                                                Height="64px"></asp:ListBox>
                                        </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton ID="imgBorrarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                Width="16px" OnClick="imgBorrarCorreoControl_Click" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                 
                                    <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />

                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBusquedaBanco" CssClass="ContenedorDatos" runat="server" Width="600px"
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
                            <asp:Panel ID="pnlDatosB" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbCodigoBanco" runat="server" Text="Código Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbCodigoBancoB" runat="server" CssClass="BordeControles" MaxLength="3"
                                                Width="30px"></asp:TextBox>
                                              <asp:FilteredTextBoxExtender ID="ftetxbCodigoBancoB" runat="server"
                                                TargetControlID="txbCodigoBancoB" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNombreBancoB" runat="server" Text="Nombre del Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbNombreBancoB" runat="server" CssClass="BordeControles" MaxLength="100"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondasB" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatosB" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </td>
                        <td style="width: 130px; text-align: center; vertical-align: bottom">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btnBuscar_Click"
                                UseSubmitBehavior="True" />
                            <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva Búsqueda" Width="110px"
                                OnClick="btnNuevaBusqueda_Click" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Resultados" Width="110px"
                                OnClick="btnLimpiar_Click" Enabled="false" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="110px" OnClick="btnCancelar_Click"
                                UseSubmitBehavior="false" />
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
                            <asp:GridView ID="gvBusquedaBanco" runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" OnRowCommand="gvBusquedaBanco_RowCommand" OnRowDataBound="gvBusquedaBanco_RowDataBound" AllowPaging="True"
                                OnPageIndexChanging="gvBusquedaBanco_PageIndexChanging" PageSize="9" CssClass="EstiloEtiquetas80">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/MarcaVisual/iconos/aceptar.png" CommandName="sel"
                                        Text="Seleccionar">
                                        <ItemStyle Width="50px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:BoundField ItemStyle-Width="80px" HeaderText="Código Banco" DataField="pCodigo"></asp:BoundField>
                                    <asp:BoundField HeaderText="Nombre Banco" DataField="pNombre"></asp:BoundField>
                                    <asp:BoundField HeaderText="Estado" DataField="pActivo"></asp:BoundField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
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
            <asp:ModalPopupExtender ID="mpeBusquedaBanco" runat="server" PopupControlID="pnlBusquedaBanco"
                TargetControlID="btnCodigoBanco" BackgroundCssClass="VentanaModal" CancelControlID="btnCancelar">
            </asp:ModalPopupExtender>
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
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
