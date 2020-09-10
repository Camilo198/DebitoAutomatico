<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="LecturaArchivos.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.LecturaArchivos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1
        {
            width: 20px;
            height: 25px;
        }

        .auto-style2
        {
            font-size: 8pt;
            text-align: center;
            vertical-align: middle;
            height: 25px;
        }

        .auto-style3
        {
            width: 15px;
            height: 25px;
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
    <script type="text/javascript">
        window.onsubmit = function () {
            if (Page_IsValid) {
                var updateProgress = $find("<%= UpdateProgress1.ClientID %>");
                window.setTimeout(function () {
                    updateProgress.set_visible(true);
                }, 100);
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function Confirm() {

            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            confirm_value.value = "";
            if (confirm("Esta seguro de enviar los correos?.")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Lectura de Archivos
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
                        <asp:Panel ID="pnlGeneraArchivo" runat="server" ScrollBars="Auto" Height="100%" Width="600px">
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td style="height: 10px" colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbArchivo" runat="server" Text="Seleccione Archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:FileUpload ID="cargarArchivo" runat="server" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:DropDownList ID="ddlBancoDebito" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                OnSelectedIndexChanged="ddlBancoDebito_SelectedIndexChanged" Width="205px" AutoPostBack="false">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBancoDebito" runat="server" ErrorMessage="Favor seleccionar un banco!"
                                                ForeColor="Red" ControlToValidate="ddlBancoDebito" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBancoDebito" runat="server" Enabled="True" TargetControlID="rfvBancoDebito"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="Label1" runat="server" Text="Enviar notificación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:CheckBox ID="chbNotificacion" Checked="true" runat="server" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td style="height: 10px" colspan="5"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceDatos" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlResul" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td style="height: 10px" colspan="5">Archivo cargado </td>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial"></td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbNomArchCarg" runat="server" Text="Nombre:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80"></td>
                                            <td class="EstiloEtiquetas80">&nbsp;</td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbArchCarg" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80"></td>
                                            <td class="EspaciadoFinal"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial"></td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbResulArchivo" runat="server" Text="Registros Cargados:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbTotReg" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">&nbsp;</td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbErroneas" runat="server" Text="Registros con Errores:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="LbRErroneas" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td class="EspaciadoFinal">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="LetraLeyendaColor10" colspan="6">Prenotas </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial"></td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbPAceptada" runat="server" Text="Aceptadas:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="LbPNAceptada" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <%--   <td class="EspaciadoIntermedio">
                                        </td>--%>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbPRechazada" runat="server" Text="Rechazadas:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="LbPNRechazada" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td class="EspaciadoFinal">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="LetraLeyendaColor10" colspan="6">Debitos </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial"></td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbDAceptada" runat="server" Text="Aceptados:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="LbDNAceptada" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">&nbsp;</td>
                                            <%--   <td class="EspaciadoIntermedio">
                                        </td>--%>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lbDRechazada" runat="server" Text="Rechazados:"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="LbDNRechazada" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td class="EspaciadoFinal">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="LetraLeyendaColor10" colspan="6">Total </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial"></td>
                                            <td class="EstiloEtiquetas80">
                                                <asp:Label ID="lblTotal" runat="server" Text="$0"></asp:Label>
                                            </td>
                                            <td class="EstiloEtiquetas80"></td>
                                            <%--   <td class="EspaciadoIntermedio">
                                        </td>--%>
                                            <td class="EstiloEtiquetas80"></td>
                                            <td class="EstiloEtiquetas80"></td>
                                            <td class="EspaciadoFinal"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="height: 10px"></td>
                                        </tr>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceResul" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlResul" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <table width="99%">
                                <tr>
                                    <td class="auto-style1"></td>
                                    <td class="auto-style2">

                                        <asp:Button ID="btnCorreo" runat="server" Height="30px" Text="Correo" Width="122px" Visible="False" />

                                    </td>
                                    <td class="auto-style3"></td>
                                </tr>
                            </table>
                            <br />
                            <table width="99%">
                                <tr>
                                    <td class="EspaciadoInicial"></td>
                                    <td class="EstiloInformacion">
                                        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Height="30px" Width="122px"
                                            OnClick="btnProcesar_Click" Enabled="true" ValidationGroup="1" />
                                    </td>
                                    <td class="EspaciadoFinal"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <!--Prueba-->
            <asp:Panel ID="pnlFiltroCorreo" CssClass="ContenedorDatos" runat="server" Width="850px" Height="650px" Style="display: none;">
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
                                            <asp:TextBox ID="txbContratoB" runat="server" CssClass="BordeControles" MaxLength="100" AutoPostBack="true"
                                                Width="150px" ValidationGroup="3" OnTextChanged="txbContratoB_TextChanged"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbIdentClienteB" runat="server" Text="Respuesta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbRespuesta" runat="server" CssClass="BordeControles" MaxLength="100" AutoPostBack="true"
                                                Width="150px" ValidationGroup="3" OnTextChanged="txbRespuesta_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>

                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="Label2" runat="server" Text="Causal:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbCausal" runat="server" CssClass="BordeControles" MaxLength="100" AutoPostBack="true"
                                                Width="150px" ValidationGroup="3" OnTextChanged="txbCausal_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="Label3" runat="server" Text="Estado:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbEstado" runat="server" CssClass="BordeControles" MaxLength="100" AutoPostBack="true"
                                                Width="150px" ValidationGroup="3" OnTextChanged="txbEstado_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="Label4" runat="server" Text="Tipo Transferencia:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:TextBox ID="txbTipoTranferencia" runat="server" CssClass="BordeControles" MaxLength="100" AutoPostBack="true"
                                                Width="150px" ValidationGroup="3" OnTextChanged="txbTipoTranferencia_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondasB" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlContratoB" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>

                        </td>
                        <td style="width: 130px; text-align: center; vertical-align: bottom">
                            <asp:Button ID="btnBuscar" runat="server" Text="Cargar Lista" Width="110px"
                                UseSubmitBehavior="True" ValidationGroup="3" OnClick="btnBuscar_Click" />
                            <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva Búsqueda" Width="110px" OnClick="btnNuevaBusqueda_Click" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Busqueda" Width="110px"
                                Enabled="false" OnClick="btnLimpiar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="110px"
                                UseSubmitBehavior="false" />
                            <asp:Button runat="server" ID="bttramita" Width="110px" OnClientClick="Confirm()" Text="Enviar Correo" OnClick="btntramitar_Click" />
                        </td>
                        <td class="EspaciadoInicial"></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="SeparadorHorizontal"></td>
                    </tr>
                </table>
                <table style="width: 100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td class="EstiloEtiquetas80">
                            <asp:Button ID="btnSeleccion" runat="server" Text="S/N" Height="27px" Width="42px"
                                OnClick="btnSeleccion_Click" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Get" Height="27px" Width="42px"
                                OnClick="btntramitar_Click" Style="display: none" />
                            <asp:Button ID="btnoEliminar" runat="server" Text="Get" Height="27px" Width="42px"
                                OnClick="btntramita_Click" Style="display: none" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GVCorreo" runat="server" AutoGenerateColumns="False"
                                Width="100%" AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" AllowPaging="True"
                                CssClass="EstiloEtiquetas80" OnPageIndexChanging="GVCorreo_PageIndexChanging" OnPageIndexChanged="GVCorreo_PageIndexChanged" OnSelectedIndexChanged="GVCorreo_SelectedIndexChanged" PageSize="20">
                                <Columns>
                                    <asp:BoundField HeaderText="CONTRATO" DataField="pContrato" />
                                    <asp:BoundField HeaderText="RESPUESTA" DataField="pRespuesta" />
                                    <asp:BoundField HeaderText="CAUSAL" DataField="pCausal" />
                                    <asp:BoundField HeaderText="ESTADO" DataField="pEstado" />
                                    <asp:BoundField HeaderText="TIPO TRANSFERENCIA" DataField="pTipo_transferencia" />
                                    <asp:BoundField HeaderText="ENVIAR CORREO" DataField="pMarca" />
                                    <asp:TemplateField HeaderText="MARCAR">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnAdd" CommandName="Select" runat="server" Height="20px" ImageUrl="~/MarcaVisual/iconos/activar_sel.png" ToolTip="Ver" Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="mpeBusquedaCorreo" runat="server" PopupControlID="pnlFiltroCorreo"
                TargetControlID="btnCorreo" BackgroundCssClass="VentanaModal" CancelControlID="btnCancelar">
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnProcesar" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="bttramita" />
        </Triggers>
    </asp:UpdatePanel>





</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
