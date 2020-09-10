<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="AdministrarMensajes.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.AdministrarMensajes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 185px;
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
                    Width="16px" ToolTip="Guardar" OnClick="imgBtnGuardar_Click" />
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
                    <td class="BarraSubTitulo">Administrar Mensajes
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
                        <asp:Panel ID="pnlBanco" runat="server" ScrollBars="Auto" Height="605px" Width="1006px">

                            <asp:Panel ID="pnlFTP" CssClass="PanelBordesRedondos" runat="server" Width="99%" Height="550px">
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
                                        <td class="auto-style1">
                                            <asp:Label ID="lblTipoContrato" runat="server" Text="Tipo de contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                                   <asp:DropDownList ID="ddlTipoContrato" AutoPostBack="true" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px" OnSelectedIndexChanged="ddlTipoContrato_SelectedIndexChanged">
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="S">Suscriptor</asp:ListItem>
                                                <asp:ListItem Value="A">Adjudicado</asp:ListItem>
                                                <asp:ListItem Value="G">Ganador</asp:ListItem>
                                                <asp:ListItem Value="C">Cuotas Por Devolver</asp:ListItem>
                                            </asp:DropDownList>
                                             
                                        </td>
                                        <td class="EspaciadoCeldaControl" colspan="2">
                                 
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                      <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblEstadoDebito" runat="server" Text="Estado debito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                                 <asp:DropDownList ID="ddlEstadoD" AutoPostBack="true" runat="server" Width="150px" CssClass="BordeListas" OnSelectedIndexChanged="ddlEstadoD_SelectedIndexChanged">
                                                     </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            
                                        </td>
                                        <td style="text-align: right">
                                            
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                     <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                     <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblMotivo" runat="server" Text="Motivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                                 <asp:DropDownList ID="ddlMotivo" AutoPostBack="true" runat="server" Width="150px" CssClass="BordeListas" OnSelectedIndexChanged="ddlMotivo_SelectedIndexChanged">
                                                      <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="1">Inscripción</asp:ListItem>
                                                <asp:ListItem Value="2">Modificación</asp:ListItem>
                                                <asp:ListItem Value="3">Respuesta Aceptada</asp:ListItem>
                                                <asp:ListItem Value="4">Respuesta Rechazada</asp:ListItem>
                                                     </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            
                                        </td>
                                        <td style="text-align: right">
                                            
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                      <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                        <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblAsunto" runat="server" Text="Asunto:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">

                                            <asp:TextBox ID="txbAsunto" runat="server" CssClass="BordeControles" 
                                                Width="450px" ValidationGroup="1" AutoPostBack="true"></asp:TextBox>
                                                     
                                        </td>
                                        <td>
                                   
                                        </td>
                                        <td style="text-align: right">
                                 
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                     
                                    </tr>
                                       
                                      <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                     </tr>
                                        <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style1">
                                            <asp:Label ID="Label1" runat="server" Text="Cuerpo del mensaje:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                   <asp:TextBox ID="txbMensaje" runat="server" CssClass="BordeControles" Width="666px"
                                         MaxLength="100" Height="358px" TextMode="MultiLine"></asp:TextBox>

                                        </td>
                                        <td>

                                        </td>
                                        <td style="text-align: right">
                                 
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6"></td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            
                            <br />
                  <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFTP" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
 
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
