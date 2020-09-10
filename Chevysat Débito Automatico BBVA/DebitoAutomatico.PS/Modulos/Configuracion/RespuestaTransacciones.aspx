<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="RespuestaTransacciones.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.RespuestaTransacciones" %>

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
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Ingresar Respuestas de Transacciones
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
                        <asp:Panel ID="pnlRespuestasTransacciones" runat="server" ScrollBars="Auto" Height="100%" Width="900px">
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Banco
                                        </td>
                                    </tr>
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
                                            <asp:TextBox ID="txbCodigoBanco" runat="server" CssClass="BordeControles" MaxLength="3"
                                                Width="30px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbCodigoBanco_TextChanged"></asp:TextBox>
                                                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                FilterType="Numbers" TargetControlID="txbCodigoBanco">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rfvCodigoBanco" runat="server" ErrorMessage="Favor digitar el Codigo del Banco!"
                                                ForeColor="Red" ControlToValidate="txbCodigoBanco" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigoBanco" runat="server" Enabled="True" TargetControlID="rfvCodigoBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbNombreBanco" runat="server" Text="Nombre del Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreBanco" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="150px" ValidationGroup="1" Enabled="False"></asp:TextBox>
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
                            <asp:Panel ID="pnlRespuestas" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Respuestas Transacciones
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbCodigo" runat="server" Text="Código de Respuesta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigo" runat="server" CssClass="BordeControles" MaxLength="10"
                                                Width="145px" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ErrorMessage="Favor digitar el Codigo de la Respuesta!"
                                                ForeColor="Red" ControlToValidate="txbCodigo" ValidationGroup="1" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigo" runat="server" Enabled="True" TargetControlID="rfvCodigo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lbRespuesta" runat="server" Text="Respuesta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbRespuesta" runat="server" CssClass="BordeControles" MaxLength="200"
                                                Width="150px" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRespuesta" runat="server" ErrorMessage="Favor digitar la Respuesta del Banco!"
                                                ForeColor="Red" ControlToValidate="txbRespuesta" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceRespuesta" runat="server" Enabled="True" TargetControlID="rfvRespuesta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoP" runat="server" Text="Estado Prenota:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlEstadoP" runat="server" CssClass="BordeListas">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoD" runat="server" Text="Estado Débito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoD" runat="server" CssClass="BordeListas">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbEstadoRespuesta" runat="server" Text="Estado Respuesta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="espaciado">
                                            <asp:DropDownList ID="ddlEstadoRespuesta" runat="server" CssClass="BordeListas" Width="145px">
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="ACEPTADO">ACEPTADO</asp:ListItem>
                                                <asp:ListItem Value="RECHAZADO">RECHAZADO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEstadoRespuesta" runat="server" ErrorMessage="Favor seleccionar un Estado!"
                                                ForeColor="Red" ControlToValidate="ddlEstadoRespuesta" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceEstadoRespuesta" runat="server" Enabled="True" TargetControlID="rfvEstadoRespuesta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="LEnvioC" runat="server" Text="Envio Correo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="DDLEnvioC" runat="server" CssClass="BordeListas">
                                               <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="SI">SI</asp:ListItem>
                                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvDDLEnvioC" runat="server" ErrorMessage="Favor seleccionar un Estado!"
                                                ForeColor="Red" ControlToValidate="DDLEnvioC" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="rfvDDLEnvioC"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="3" style="text-align: right">
                                            <asp:ImageButton ID="imgBtnAddField" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                Width="16px" ValidationGroup="1" OnClick="imgBtnAddField_Click" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlRespuestas" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvRespuestas" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" PageSize="9" Width="97%"
                                OnRowCommand="gvRespuestas_RowCommand" CssClass="EstiloEtiquetas80"
                                OnPageIndexChanging="gvRespuestas_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Editar" ImageUrl="~/MarcaVisual/iconos/editar.png"
                                        Text="Editar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                        Text="Eliminar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle Width="7px" CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CODIGO" HeaderText="Código Respuesta"></asp:BoundField>
                                    <asp:BoundField DataField="RESPUESTA" HeaderText="Descripción">
                                         <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Id_prenota" DataField="ID_ESTADO_PRENOTA" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO_PRENOTA" HeaderText="Prenota en proceso" >
                                         <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                    <asp:BoundField HeaderText="Id_debito" DataField="ID_ESTADO_DEBITO" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO_DEBITO" HeaderText="Debito en proceso" >
                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="ESTADO_RESPUESTA" HeaderText="Estado Respuesta"/> 
                                    <asp:BoundField DataField="ENVIO_CORREO" HeaderText="Envio Correo">
                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />                                  
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" HorizontalAlign="Center" />
                            </asp:GridView>
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
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                FilterType="Numbers" TargetControlID="txbCodigoBancoB">
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
