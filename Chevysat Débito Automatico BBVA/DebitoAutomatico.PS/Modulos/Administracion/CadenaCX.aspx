<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="CadenaCX.aspx.cs" Inherits="DebitoAutomatico.PS.Modulo.Administracion.CadenaCX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
 <script language="javascript" type="text/javascript">
     function validarCaracteres(textAreaControl, maxTam) {
         if (textAreaControl.value.length > maxTam) {
             textAreaControl.value = textAreaControl.value.substring(0, maxTam);
             alert("Debe ingresar hasta un máximo de " + maxTam + " carácteres!!!");
         }
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
<table align="left" cellpadding="0" cellspacing="0">
        <tr>
            
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png"
                    Width="16px" ToolTip="Guardar" ValidationGroup="1" OnClick="imgBtnGuardar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Configuración conexion
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
                                        <td class="LetraLeyendaColor" colspan="9">Parametrización
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="lbCadenaCX" runat="server" Font-Size="X-Small" Text="Cadena de Conexión:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                                        <asp:TextBox ID="txbCadenaCX" runat="server" CssClass="EstiloTextoMultilinea" Height="50px"
                                                    TextMode="MultiLine" Width="400px"></asp:TextBox>
                                            
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
     <tr>
                                        <td class="EspaciadoInicial"></td>

                                        <td class="auto-style6">
                                            <asp:Label ID="Label4" runat="server" Font-Size="X-Small" Text="Conexión Sico:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">

<asp:TextBox ID="txbCadenaSico" runat="server" CssClass="EstiloTextoMultilinea" Height="50px"
                                                    TextMode="MultiLine" Width="400px"></asp:TextBox>
                                            
                                        </td>
                                        <td colspan="5"></td>
                               </tr>                          
                      
                                      <tr>
                                        <td class="EspaciadoInicial"></td>

                                        <td class="auto-style6">
                                            <asp:Label ID="Label1" runat="server" Font-Size="X-Small" Text="Ruta Ftp:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                                        <asp:TextBox ID="txbRutaFtp" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="110px" ></asp:TextBox>
                                            
                                        </td>
                                        <td colspan="5"></td>
                               </tr>
                                 
                                      <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="Label2" runat="server" Font-Size="X-Small" Text="Usuario Ftp:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                                        <asp:TextBox ID="txbUserFtp" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="110px" ></asp:TextBox>
                                            
                                        </td>
                                        <td colspan="5"></td>
                               </tr>
                                      <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="Label3" runat="server" Font-Size="X-Small" Text="Password Ftp:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                                        <asp:TextBox ID="txbPassFtp" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="110px" ></asp:TextBox>
                                            
                                        </td>
                                        <td colspan="5"></td>
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
                            
                            
                            <br />

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
        <Triggers>
            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>

