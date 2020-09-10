<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.Reportes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 69px;
        }
        .auto-style2 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 69px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
        <asp:UpdatePanel ID="UpdatePanelHerramientas" runat="server">
        <ContentTemplate>
            <table align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="CuadranteBotonImagen">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                            Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
                    </td>
                    <td class="CuadranteBotonImagen">
                    <asp:ImageButton ID="imgBtnReportes" runat="server" ImageUrl="~/MarcaVisual/iconos/activar_sel.png"
                    Width="16px" ToolTip="Reportes"  OnClick="imgBtnReportes_Click"  />
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
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Reportes Debito Automatico
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
                      
                                     <asp:Panel ID="pnlFiltros" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                  

                               
                                  
                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblTipoReporte" runat="server" Text="Tipo de reporte:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                               <asp:DropDownList ID="ddlReporte" AutoPostBack="true" runat="server" CssClass="BordeListas" 
                                                Width="150px" OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged">
                                                      <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="1">Clientes por tipo</asp:ListItem>
                                                <asp:ListItem Value="2">Inconsistencia</asp:ListItem>
                                                   <asp:ListItem Value="3">Historico de transacciones</asp:ListItem>
                                                   <asp:ListItem Value="4">Logs por contrato</asp:ListItem>
                                                   <asp:ListItem Value="5">Logs por archivos</asp:ListItem>
                                                   <asp:ListItem Value="6">Clientes actualizados</asp:ListItem>
                                                   <asp:ListItem Value="7">Envió de correos</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                         <td class="EstiloEtiquetas125Right">
                                     <asp:Label ID="lblContrato" Visible="false" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:TextBox ID="txbContrato" Visible="false" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                               Width="85px"></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="frbContrato" runat="server"
                                                TargetControlID="txbContrato" FilterType="Custom, Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                         <td class="EstiloEtiquetas125Right">
                                            <asp:Label ID="lblBanco" Visible="false" runat="server" Text="Banco:"></asp:Label>
                                        </td>
                                         <td class="EstiloEtiquetas80">
                                        <asp:DropDownList ID="ddlBancoDebito" runat="server" CssClass="BordeListas" Visible="false" Width="150px">
                                           </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            
                                        </td>
                                        
                                    </tr>
                                          <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>                    
                                
                                      <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblFechaInicio" Visible="false" runat="server" Text="Fecha Inicio:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                          <asp:TextBox ID="txbFechaInicio" Visible="false" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                               Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgFechaInicio" Visible="false" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicio" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgFechaInicio" TargetControlID="txbFechaInicio">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meFechaInicio" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaInicio" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                         <td class="EstiloEtiquetas125Right">
                                            <asp:Label ID="lblFechaFin" Visible="false" runat="server" Text="Fecha Fin:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                               <asp:TextBox ID="txbFechaFinal" Visible="false" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                               Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaFinal" Visible="false" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaFinal" TargetControlID="txbFechaFinal">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaFinal" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                        </td>
                                          <td class="EstiloEtiquetas125Right">
                                            <asp:Label ID="lblNombreArchivo" runat="server" Text="Nombre Archivo:" Visible="false"></asp:Label>
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:TextBox ID="txbNombreArchivo" runat="server" CssClass="FuenteDDL" TabIndex="2" Visible="false" Width="150px"></asp:TextBox>                                         
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>
                                         <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            
                                            <asp:Label ID="lblEstadoD" runat="server" Text="Estado Debito:" Visible="false"></asp:Label>
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                       <td class="EstiloEtiquetas80" colspan="9">
                                          
                                           <asp:DropDownList ID="ddlEstadoDebito" runat="server" CssClass="BordeListas" Visible="false" Width="150px">
                                           </asp:DropDownList>
                                          
                                        </td>
                                        
                                                 <tr>
                                                     <td colspan="13" style="height: 10px"></td>
                                    </tr>
                                    
                                    </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFiltros" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
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
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
