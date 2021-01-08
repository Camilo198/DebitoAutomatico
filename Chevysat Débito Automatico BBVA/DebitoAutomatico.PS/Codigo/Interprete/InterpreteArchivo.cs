using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.IO;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.LN.Utilidades;
using DebitoAutomatico.EN.Definicion;

using DebitoAutomatico.PS.Codigo.Correo;
using DebitoAutomatico.PS.Codigo.Ftp;
using System.Globalization;
using DebitoAutomatico.LN.Consulta;
using System.Configuration;

using Renci.SshNet;
using SSH;
using System.Web.UI.WebControls;
using DebitoAutomatico.LN;
using System.Linq;

namespace DebitoAutomatico.PS.Codigo.Interprete
{

    public class InterpreteArchivo
    {

        #region DEFINICION DE VARIABLES

        //ESTRUCTURA DEL ARCHIVO RESPUESTA BANCO
        //*************************************************************************************
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> BListaLinea1EA { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> BListaLinea2EL { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> BListaLinea3DT { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> BListaLinea4CL { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> BListaLinea5CA { get; set; }
        String BLineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String BLineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String BLineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE
        String BLineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE       
        String BLineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO       
        //*************************************************************************************

        //ESTRUCTURA DEL ARCHIVO ASOBANCARIA
        //*************************************************************************************

        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> AListaLinea3DT { get; set; }
        String ALineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE

        //*************************************************************************************

        //CONTIENE LA INFORMACION DE LAS PRENOTAS Y DEBITOS (EXITOSOS Y ERRONEOS)
        //*************************************************************************************
        Int32 prenotaOK = 0;
        Int32 prenotaERROR = 0;
        Int32 debitoOK = 0;
        Int32 debitoERROR = 0;
        Int32 lineasErroneas = 0; // ESTA VARIABLE CUENTA LOS CONTRATOS NO ENCONTRADOS, CAMBIOS EN
        //PRENOTAS Y DEBITOS NO REALIZADOS (EJ: CAMBIAR EL ESTADO DEL CLIENTE DE PRENOTA A DEBITO)
        //*************************************************************************************

        DataTable tablaCE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA DE LOS CAMPOS_EQUIVALENCIAS
        DataTable tablaE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA EQUIVALENCIAS

        ArrayList idASumar = new ArrayList(); //CONTIENE LOS ID'S DE LOS CAMPOS QUE SE VAN A SUMAR      
        Dictionary<int, double> contadores = new Dictionary<int, double>();

        DataSet ds0 = new DataSet(); // DATASET CON LINEAS DE ENCABEZADO Y CONTROL
        DataSet ds1 = new DataSet(); // DATASET CON PRENOTAS
        DataSet ds2 = new DataSet(); // DATASET CON DEBITOS
        DataSet ds3 = new DataSet(); // DATASET CON DEBITOS OK, SE UTILIZA PARA CREAR ARCHIVO SICO

        DataTable dt0 = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();

        DataTable detalle = new DataTable();//TABLA QUE CONTIENE LA INFORMACION SOBRE PRENOTAS Y DEBITOS
        DataTable DebitoXFecha = new DataTable();
        DataTable PrenotaXFecha = new DataTable();

        ArrayList retornoMetodo = new ArrayList();
        EnviarCorreo enviar = new EnviarCorreo();
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO SICO
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA LOS ARCHIVOS CON RESULTADOS Y FALLAS
        String DirectorioSico = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA LOS ARCHIVOS SICO QUE SE CARGA A SICO
        String DirectorioHistorico = String.Empty;
        String FtpSico = String.Empty; // CONTIENE LA RUTA DEL FTP DONDE VAN A QUEDAR LOS ARCHIVOS QUE SE CARGAN EN LA INTERFACE DE SICO
        String tipoArchivo = String.Empty;

        HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
        static StreamWriter sw;
        String ConsecutivoArchivo; // CONTIENE UN CONSECUTIVO QUE SE USA PARA CONTROLAR CUANTAS VECES SE HA GENERADO EL ARCHIVO EN EL DIA
        String Fecha;
        int registrosLote = 0;
        int valorarchivo = 0;
        ArrayList LimitesSuperior = new ArrayList();
        int ciclo = 0;
        String CuentaPrincipal = String.Empty;//Fiducia Principal Chevyplan
        Int32 TipoDeCuenta = 0;
        String UsaFtp = String.Empty;
        String PassFtp = String.Empty;
        String exporasico, RutaEpicor, RutaSico, comando;

        string UsuFTPSystem = ConfigurationManager.AppSettings["LoginSystem"].ToString();
        string PassFTPSystem = ConfigurationManager.AppSettings["passwordSystem"].ToString();

        #endregion
        String LugarPago = String.Empty;
        public DateTime FeModificacion;
        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion.

        public ArrayList ServicioInterprete(Int32 IdBanco, String Usuario, List<String> lineasArchivo, String RutaArchivo, String NombreArchivoCargado,
                                             String FechaProcesoArchivo, bool procesar, String RutaFtp, String UserFtp, String PasswordFtp, bool notificacion)
        {
            #region CONFIGURACION BANCO

            Banco objB = new Banco();
            objB.pId = IdBanco;
            List<EN.Tablas.Banco> listaB = new BancoLN().consultarBanco(objB);
            if (listaB.Count > 0)
            {
                objB = listaB[0];
            }

            DataSet dsLupa = new DataSet();
            BancoLN objBancoLN = new BancoLN();
            dsLupa = objBancoLN.consultarLupa(objB);

            if (dsLupa.Tables[0].Rows.Count > 0)
            {
                LugarPago = dsLupa.Tables[0].Rows[0]["LugarPago"].ToString();
            }

            Rutas objRuta = new Rutas();
            Rutas objRutaH = new Rutas();
            Rutas objRutaSico = new Rutas();

            String mes = DateTime.Now.ToString("MMMM").ToUpper();
            String año = DateTime.Now.Year.ToString();
            RutasLN objRutaLN = new RutasLN();

            objRuta.pId = objB.pIdErrores;
            objRutaH.pId = objB.pIdHistorico;
            objRutaSico.pId = objB.pIdPagos;

            String path = objRutaLN.consultarRuta(objRuta)[0].pRuta + año + "\\" + mes + "\\";
            Directorio = path;

            String patHist = objRutaLN.consultarRuta(objRutaH)[0].pRuta + año + "\\" + mes + "\\";
            DirectorioHistorico = patHist;

            DirectorioSico = objRutaLN.consultarRuta(objRutaSico)[0].pRuta + año + "\\" + mes + "\\";

            FtpSico = RutaFtp;
            UsaFtp = UserFtp;
            PassFtp = PasswordFtp;

            //OBTENER LOS CORREOS DE CONTROL DEL PROCESO Y LOS DESTINATARIOS
            string[] CorreoControl = objB.pCorreoControl.Split(';');
            ArrayList correoC = new ArrayList();
            correoC.AddRange(CorreoControl);
            string[] CorreoEnvio = objB.pCorreoEnvio.Split(';');
            ArrayList correoE = new ArrayList();
            correoE.AddRange(CorreoEnvio);

            #endregion


            Logs objL = new Logs();
            EnviarCorreo Correo = new EnviarCorreo();

            #region OBTENER PRENOTAS Y DEBITOS DEL SISTEMA
            DataSet DsDebitos = new DataSet();
            DataTable tablaDebitos = new DataTable();
            DsDebitos = new DatosDebitoLN().consultar("5,8");//TRAE TODOS LOS CLIENTES EN ESTADO DEBITO EN PROCESO Y DEBITADO(PARA PODER PROCESAR CAMBIOS DE BIEN)
            tablaDebitos = DsDebitos.Tables[0];

            DataSet DsPrenotas = new DataSet();
            DataTable tablaPrenotas = new DataTable();
            DsPrenotas = new DatosDebitoLN().consultar("2,3,4");//TRAE TODOS LOS CLIENTES EN ESTADO PRENOTA EN PROCESO, PRENOTA RECHAZADA, DEBITO
            tablaPrenotas = DsPrenotas.Tables[0];

            #endregion

            tipoArchivo = "Respuesta Banco";

            //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE SICO SI NO EXISTE
            if (!Directory.Exists(Directorio))
            {
                System.IO.Directory.CreateDirectory(Directorio);
            }
            if (!Directory.Exists(DirectorioSico))
            {
                System.IO.Directory.CreateDirectory(DirectorioSico);
            }
            if (!Directory.Exists(DirectorioHistorico))
            {
                System.IO.Directory.CreateDirectory(DirectorioHistorico);
            }

            #region CREAR COLUMNAS DE LA TABLA CON LOS DATOS PARA GUARDAR LAS LINEAS EN UNA BD
            LineasArmadasdt.Columns.Add("fecha");
            LineasArmadasdt.Columns.Add("codigoBanco");
            LineasArmadasdt.Columns.Add("consecutivo");
            LineasArmadasdt.Columns.Add("lineaArmada");
            #endregion

            #region OBTENER ESTRUCTURA DEL BANCO
            BListaLinea1EA = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
            BListaLinea2EL = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
            BListaLinea3DT = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
            BListaLinea4CL = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
            BListaLinea5CA = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();

            BListaLinea1EA.AddRange(consultarEstructura("1EA", "RBN", Convert.ToInt32(objB.pId)));
            BListaLinea2EL.AddRange(consultarEstructura("2EL", "RBN", Convert.ToInt32(objB.pId)));
            BListaLinea3DT.AddRange(consultarEstructura("3DT", "RBN", Convert.ToInt32(objB.pId)));
            BListaLinea4CL.AddRange(consultarEstructura("4CL", "RBN", Convert.ToInt32(objB.pId)));
            BListaLinea5CA.AddRange(consultarEstructura("5CA", "RBN", Convert.ToInt32(objB.pId)));
            #endregion

            //CREAR COLUMNAS PARA GUARDAR LINEAS DEL ARCHIVO
            //*************************************************************************************
            dt0.Columns.Add("linea");
            dt1.Columns.Add("linea");
            dt1.Columns.Add("estado");
            dt2.Columns.Add("linea");
            dt2.Columns.Add("estado");
            dt3.Columns.Add("fecha");
            dt3.Columns.Add("linea");
            //*************************************************************************************

            Int32 inicio = 0;

            Int32[,] camposBanco = new int[9, 3];//ARREGLO PARA GUARDAR POSICION INICIAL, LONGITUD DE
            //UN CAMPO DEL ARCHIVO BANCO E ID DE LA ESTRUCTURA DEL CAMPO.
            //FILA 1 = Estado
            //FILA 2 = Identificacion
            //FILA 3 = Valor
            //FILA 4 = NCuenta
            //FILA 5 = Fecha Recaudo
            //FILA 6 = Contrato
            //FILA 7 = Banco Debitado
            //FILA 8 = Causal de rechazo
            //FILA 9 = NCuenta 2 BBVA
            int a = 0;
            try
            {



                #region OBTENER UBICACION DE Estado,Identificacion,Valor,NCuenta,Fecha,Contrato,Banco
                foreach (DebitoAutomatico.EN.Tablas.EstructuraArchivo objBan in BListaLinea3DT)
                {
                    if (objBan.pNombre.Equals("Estado"))//CODIGO RESPUESTA DEL BANCO
                    {
                        camposBanco[0, 0] = inicio;
                        camposBanco[0, 1] = objBan.pLongitud.Value;
                        camposBanco[0, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Identificacion"))//IDENTIFICACION DEL CLIENTE
                    {
                        camposBanco[1, 0] = inicio;
                        camposBanco[1, 1] = objBan.pLongitud.Value;
                        camposBanco[1, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Valor a debitar"))//SI TIENE UN VALOR PARA DIFERENCIAR PRENOTA O DEBITO
                    {
                        camposBanco[2, 0] = inicio;
                        camposBanco[2, 1] = objBan.pLongitud.Value;
                        camposBanco[2, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Cuenta destino"))//NUMERO DE LA CUENTA DEL CLIENTE
                    {
                        camposBanco[3, 0] = inicio;
                        camposBanco[3, 1] = objBan.pLongitud.Value;
                        camposBanco[3, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Fecha del debito"))//FECHA DEL DEBITO
                    {
                        camposBanco[4, 0] = inicio;
                        camposBanco[4, 1] = objBan.pLongitud.Value;
                        camposBanco[4, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Contrato"))//NUMERO DE CONTRATO DEL CLIENTE
                    {
                        camposBanco[5, 0] = inicio;
                        camposBanco[5, 1] = objBan.pLongitud.Value;
                        camposBanco[5, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Banco Debitado"))//NUMERO DEL BANCO
                    {
                        camposBanco[6, 0] = inicio;
                        camposBanco[6, 1] = objBan.pLongitud.Value;
                        camposBanco[6, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Causal de rechazo"))//CAUSAL RECHAZO
                    {
                        camposBanco[7, 0] = inicio;
                        camposBanco[7, 1] = objBan.pLongitud.Value;
                        camposBanco[7, 2] = objBan.pId.Value;
                    }
                    if (objBan.pNombre.Equals("Número de cuenta NACHAM/ tarjeta receptora"))//Cuenta 2 BBVA
                    {
                        camposBanco[8, 0] = inicio;
                        camposBanco[8, 1] = objBan.pLongitud.Value;
                        camposBanco[8, 2] = objBan.pId.Value;
                    }

                    inicio = inicio + objBan.pLongitud.Value;
                }
                #endregion

                if (lineasArchivo.Count > 0)
                {
                    //EN ESTE CICLO SE SEPARA LAS LINEAS DE PRENOTA Y LAS DE DEBITO
                    for (int i = 0; i < lineasArchivo.Count; i++)
                    {
                        if (lineasArchivo[i] != null)
                        {
                            if (BListaLinea1EA.Count != 0 && i == 0)
                            {
                                DataRow dr0 = dt0.NewRow();
                                dr0["linea"] = lineasArchivo[i];
                                dt0.Rows.Add(dr0);
                                continue;
                            }

                            if (BListaLinea2EL.Count != 0 && i == 1)
                            {
                                DataRow dr0 = dt0.NewRow();
                                dr0["linea"] = lineasArchivo[i];
                                dt0.Rows.Add(dr0);
                                continue;
                            }

                            if (BListaLinea4CL.Count != 0 && i == (lineasArchivo.Count - 2))
                            {
                                DataRow dr0 = dt0.NewRow();
                                dr0["linea"] = lineasArchivo[i];
                                dt0.Rows.Add(dr0);
                                continue;
                            }

                            if (BListaLinea5CA.Count != 0 && i == (lineasArchivo.Count - 1))
                            {
                                DataRow dr0 = dt0.NewRow();
                                dr0["linea"] = lineasArchivo[i];
                                dt0.Rows.Add(dr0);
                                continue;
                            }

                            if (Convert.ToInt64(lineasArchivo[i].Substring(camposBanco[2, 0], camposBanco[2, 1])) == 0)
                            {
                                //INCLUIR LINEA DE CONSULTA DE HISTORIAL
                                DataRow dr1 = dt1.NewRow();
                                dr1["linea"] = lineasArchivo[i];
                                dr1["estado"] = "";
                                dt1.Rows.Add(dr1);
                            }
                            else
                            {
                                //INCLUIR LINEA DE CONSULTA DE HISTORIAL
                                DataRow dr2 = dt2.NewRow();
                                dr2["linea"] = lineasArchivo[i];
                                dr2["estado"] = "";
                                dt2.Rows.Add(dr2);
                            }
                        }
                    }
                    ds0.Tables.Add(dt0);
                    ds1.Tables.Add(dt1);
                    ds2.Tables.Add(dt2);

                }
                else
                {
                    objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                    objL.pUsuario = Usuario;
                    objL.pDetalle = objB.pNombre + ", " + tipoArchivo + " : No se encontraron registros para procesar";
                    objL.pTipoArchivo = "SIC";
                    objL.pTipoProceso = "GEN";
                    objL.pIdBanco = objB.pId.Value;
                    new LogsLN().insertar(objL);
                    retornoMetodo.Add("No se encontraron registros para procesar");
                    retornoMetodo.Add(prenotaOK);
                    retornoMetodo.Add(prenotaERROR);
                    retornoMetodo.Add(debitoOK);
                    retornoMetodo.Add(debitoERROR);
                    retornoMetodo.Add(lineasErroneas);
                    retornoMetodo.Add(0);
                    retornoMetodo.Add(ds1.Tables[0].Rows.Count + ds2.Tables[0].Rows.Count);
                    return retornoMetodo;
                }
            }
            catch (Exception ex)
            {
                int b = a;
                throw ex;
            }

            try
            {

                HistorialProcesoUsuarioLN objConsultaPrenotaDebitoXFecha = new HistorialProcesoUsuarioLN();

                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    PrenotaXFecha = objConsultaPrenotaDebitoXFecha.consultarPrenotaXFecha(IdBanco == 27 ? ds1.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(6, 2) + ds1.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(4, 2) + ds1.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(0, 4) : Convert.ToDateTime(valorCampo(ds1.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]), Convert.ToInt32(camposBanco[4, 2]))).ToString("dd/MM/yyyy"), objB.pNombre);

                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    DebitoXFecha = objConsultaPrenotaDebitoXFecha.consultarDebitoXFecha(IdBanco == 27 ? ds2.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(6, 2) + ds2.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(4, 2) + ds2.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]).Substring(0, 4) : Convert.ToDateTime(valorCampo(ds2.Tables[0].Rows[0].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]), Convert.ToInt32(camposBanco[4, 2]))).ToString("dd/MM/yyyy"), objB.pNombre);

                InterpreteRespuesta(tablaPrenotas, camposBanco, objB, Usuario, NombreArchivoCargado, FechaProcesoArchivo, procesar,
                                         ds1, "PRENOTA", ref prenotaOK, ref prenotaERROR, 1, PrenotaXFecha, notificacion);

                InterpreteRespuesta(tablaDebitos, camposBanco, objB, Usuario, NombreArchivoCargado, FechaProcesoArchivo, procesar,
                                         ds2, "DEBITO", ref debitoOK, ref debitoERROR, 1, DebitoXFecha, notificacion);

                if (procesar == true)
                {
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        LimitesSuperior = obtenerLimites(ds3);
                        for (int i = 0; i < LimitesSuperior.Count; i++)
                        {
                            escribirArchivo(objB, Convert.ToInt16(LimitesSuperior[i]), ds3, camposBanco, LugarPago);

                            LineasArmadasdt.Clear();

                            //SE GUARDA EL ESTADO DEL PROCESO
                            objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                            objL.pUsuario = Usuario;
                            objL.pDetalle = objB.pNombre + " : Archivo " + tipoArchivo + " con fecha de transaccion " + Convert.ToDateTime(ds3.Tables[0].Rows[i][0].ToString()).ToString("dd/MM/yyyy") + " fue generado correctamente";
                            objL.pTipoArchivo = "SIC";
                            objL.pTipoProceso = "GEN";
                            objL.pIdBanco = objB.pId.Value;
                            new LogsLN().insertar(objL);
                            nombreArchivo = String.Empty;
                            registrosLote = 0;
                            contadores = new Dictionary<int, double>();
                        }
                        ciclo = 0;
                        LimitesSuperior.Clear();
                    }
                }


                if (lineasErroneas > 0)
                {//CREAR ACRHIVO PLANO PARA VOLVER A PROCESARLO

                    sw = new StreamWriter(Directorio + "ARCHIVO_FALLAS_" + Convert.ToString(DateTime.Now.ToString("ddMMyyyy")) +
                                             "_" + writeMilitaryTime(DateTime.Now) + ".txt", false);

                    if (ds0.Tables.Count > 0 || ds0.Tables[0].Rows.Count > 0)
                    {
                        if (BListaLinea1EA.Count != 0)
                        {
                            sw.WriteLine(ds0.Tables[0].Rows[0].ItemArray[0].ToString());
                        }
                        if (BListaLinea2EL.Count != 0)
                        {
                            sw.WriteLine(ds0.Tables[0].Rows[1].ToString());
                        }
                    }

                    if (ds1.Tables.Count > 0 || ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow renglon in ds1.Tables[0].Rows)
                        {
                            sw.WriteLine(renglon[0].ToString() + renglon[1].ToString());
                        }
                    }
                    if (ds2.Tables.Count > 0 || ds2.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow renglon in ds2.Tables[0].Rows)
                        {
                            sw.WriteLine(renglon[0].ToString() + renglon[1].ToString());
                        }
                    }

                    if (ds0.Tables.Count > 0 || ds0.Tables[0].Rows.Count > 0)
                    {
                        if (BListaLinea4CL.Count != 0)
                        {
                            sw.WriteLine(ds0.Tables[0].Rows[2].ToString());
                        }

                        if (BListaLinea5CA.Count != 0)
                        {
                            sw.WriteLine(ds0.Tables[0].Rows[3].ToString());
                        }
                    }
                    sw.Close();
                }
                else
                {

                    string RutaOrigen = System.IO.Path.Combine(RutaArchivo);

                    string RutaDestino = System.IO.Path.Combine(DirectorioHistorico + NombreArchivoCargado + "_" + Convert.ToString(DateTime.Now.ToString("ddMMyyyy")) +
                                             "_" + writeMilitaryTime(DateTime.Now) + ".txt");
                    System.IO.File.Move(RutaOrigen, RutaDestino);
                }

                retornoMetodo.Add("Proceso de Lectura de Archivo se ejecuto con exito!!");
                retornoMetodo.Add(prenotaOK);
                retornoMetodo.Add(prenotaERROR);
                retornoMetodo.Add(debitoOK);
                retornoMetodo.Add(debitoERROR);
                retornoMetodo.Add(lineasErroneas);
                retornoMetodo.Add(valorarchivo);
                retornoMetodo.Add(ds1.Tables[0].Rows.Count + ds2.Tables[0].Rows.Count);
                return retornoMetodo;
            }
            catch (Exception ex)
            {
                if (correoC.Count > 0)
                    //    Correo.enviarNotificacionesError((String[])correoC.ToArray(typeof(String)), objB.pRemitente, ex.Message, tipoArchivo);
                    objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = objB.pNombre + ", " + tipoArchivo + " : " + ex.Message;
                objL.pTipoArchivo = "SIC";
                objL.pTipoProceso = "GEN";
                objL.pIdBanco = objB.pId.Value;
                new LogsLN().insertar(objL);
                retornoMetodo.Add(ex.Message);
                retornoMetodo.Add(prenotaOK);
                retornoMetodo.Add(prenotaERROR);
                retornoMetodo.Add(debitoOK);
                retornoMetodo.Add(debitoERROR);
                retornoMetodo.Add(lineasErroneas);
                retornoMetodo.Add(0);
                retornoMetodo.Add(ds1.Tables[0].Rows.Count + ds2.Tables[0].Rows.Count);
                return retornoMetodo;
            }

        }

        public void llenarTablaParaActualizados(String fecha, String CodigoBanco, String Consecutivo, String Linea)
        {
            DataRow dr1 = LineasArmadasdt.NewRow();
            dr1["fecha"] = fecha;
            dr1["codigoBanco"] = CodigoBanco;
            dr1["consecutivo"] = Consecutivo;
            dr1["lineaArmada"] = Linea;
            LineasArmadasdt.Rows.Add(dr1);
        }

        private string escribirArchivo(Banco objBanco, int Limite, DataSet tablaDebitos, Int32[,] camposBanco, String LugarPago)
        {
            SSHConect Conexion = new SSHConect(); // Aplicar los pagos en SICO
            
            try
            {

                #region OBTENER ESTRUCTURA SICO

                AListaLinea3DT = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();

                AListaLinea3DT.AddRange(consultarEstructura("3DT", "SIC", 0));

                #endregion

                tablaCE = new CamposEquivalenciasLN().consultarCampoEquivalencias("SIC", "0");
                tablaE = new EquivalenciasLN().consultarEquivalenciasXTipoArchivo("SIC");


                String FechaTransaccion = tablaDebitos.Tables[0].Rows[ciclo].ItemArray[0].ToString().Trim();
                String FechaSico = Convert.ToDateTime(tablaDebitos.Tables[0].Rows[ciclo].ItemArray[0].ToString().Trim()).ToString("dd/MM/yyyy");
                String Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";
                //SE CREA EL NOMBRE DEL ARCHIVO SEGUN LOS PARAMETROS SICO
                nombreArchivo = String.Concat(LugarPago.PadLeft(3, '0'),
                    FechaSico.Substring(8, 2), //Año
                    FechaSico.Substring(3, 2), //Mes
                    FechaSico.Substring(0, 2), //Dia
                    Hora);

                sw = new StreamWriter(DirectorioSico + nombreArchivo, false);

                Fecha = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));

                //SE ADICIONA UN CONSECUTIVO SEGUN EL NUMERO DE ARCHIVO QUE EXISTAN DEL MISMO DIA                
                int numColumnas = 0;
                HistorialArchivosLN columnas = new HistorialArchivosLN();
                numColumnas = columnas.consultarConsecutivoXBanco(objBanco.pCodigo, "SIC", Fecha).Rows.Count;

                if (numColumnas == 0)
                    ConsecutivoArchivo = "A";
                else
                    ConsecutivoArchivo = consecutivo(numColumnas);

                ArrayList line3DT = new ArrayList();

                #region Armar Linea Detalle

                for (int i = ciclo; i < ciclo + Limite; i++)
                {
                    line3DT.Add("3DT");
                    line3DT.Add(FechaTransaccion);
                    line3DT.Add(valorCampo(tablaDebitos.Tables[0].Rows[i].ItemArray[1].ToString().Substring(camposBanco[2, 0], camposBanco[2, 1]), Convert.ToInt32(camposBanco[2, 2])).TrimStart('0'));//Valor
                    line3DT.Add(valorCampo(tablaDebitos.Tables[0].Rows[i].ItemArray[1].ToString().Substring(camposBanco[5, 0], camposBanco[5, 1]), Convert.ToInt32(camposBanco[5, 2])).TrimStart('0'));//Contrato

                    ALineaArmada3DT = armarLineas(line3DT, AListaLinea3DT);
                    sw.WriteLine(ALineaArmada3DT);
                    llenarTablaParaActualizados(Fecha, objBanco.pCodigo, ConsecutivoArchivo, ALineaArmada3DT);
                    line3DT.Clear();
                    registrosLote += 1;
                    valorarchivo = valorarchivo + Convert.ToInt32(valorCampo(tablaDebitos.Tables[0].Rows[i].ItemArray[1].ToString().Substring(camposBanco[2, 0], camposBanco[2, 1]), Convert.ToInt32(camposBanco[2, 2])).TrimStart('0'));
                    ALineaArmada3DT = String.Empty;
                }


                double valor = Convert.ToDouble(valorarchivo);
                string resultsuma = Convert.ToString(valorarchivo);
                resultsuma = valor.ToString("0,0", CultureInfo.InvariantCulture);
                resultsuma = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);

                #endregion

                #region Reporte pagos
                RptPagosEN pagosEN = new RptPagosEN();
                PagosRptLN pagosLN = new PagosRptLN();
                List<String[,]> CodigoArchivos = new List<string[,]>();
                string[,] parametro;
                String error_mensaje;
                IList<RptPagosEN> arrPagos = null;
                String resQueryLog = "";

                pagosEN.fechaModificacionArch = this.FeModificacion;
                pagosEN.fechaProceso = Convert.ToDateTime(FechaTransaccion);
                pagosEN.fechaPago = DateTime.Now.ToString(); // Fecha
                pagosEN.codigoBanco = Convert.ToInt32(LugarPago);
                pagosEN.cantPagosArchivo = registrosLote;
                pagosEN.valorMontoArchivo = valorarchivo;
                pagosEN.cantPagosReacudo = registrosLote;

                CodigoArchivos = pagosLN.ConsultaParteFijaLN("F1", "D", LugarPago);
                //pagosEN.parteFija = parteFijaAbstracta;
                if (CodigoArchivos.Count > 0)
                {
                    parametro = CodigoArchivos[0];
                    pagosEN.parteFija = parametro[1, 1].ToString();
                }
                
                arrPagos = pagosLN.ConsultarPagoDebitoLN(pagosEN);
                if (arrPagos.Count > 0) //Si existe
                {
                    try
                    {
                        int contArch = 0;
                        int contRec = 0;
                        int contSicoCon = 0;
                        int contSicoInc = 0;
                        double contVlrSicoCon = 0;
                        double contVlrSicoIncon = 0;
                        double contVlrArch = 0;
                        foreach (var item in arrPagos)
                        {
                            contVlrArch += item.valorMontoArchivo;
                            contArch += item.cantPagosArchivo;
                            contRec += item.cantPagosReacudo;
                            contSicoCon += item.cantPagosSicoCon;
                            contSicoInc += item.cantPagosSicoInc;
                            contVlrSicoCon += item.valorMontoSicoCon;
                            contVlrSicoIncon += item.valorMontoSicoInc;
                        }
                        pagosEN.cantPagosArchivo = contArch + pagosEN.cantPagosArchivo;
                        pagosEN.cantPagosReacudo = contArch + pagosEN.cantPagosArchivo;
                        pagosEN.cantPagosSicoCon = contSicoCon + pagosEN.cantPagosSicoCon;
                        pagosEN.cantPagosSicoInc = contSicoInc + pagosEN.cantPagosSicoInc;
                        pagosEN.valorMontoArchivo = contVlrArch + pagosEN.valorMontoArchivo;
                        pagosEN.valorMontoSicoCon = contVlrSicoCon + pagosEN.valorMontoSicoCon;
                        pagosEN.valorMontoSicoInc = contVlrSicoIncon + pagosEN.valorMontoSicoInc;

                        int result = Convert.ToInt32(pagosLN.actualizarPagoDebitoLN(pagosEN));
                        if (result == 0)
                        {
                            error_mensaje = "Error en la actualización Monto Archivo banco: " +
                                pagosEN.codigoBanco + " " + pagosEN.fechaPago;

                            resQueryLog = pagosLN.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (resQueryLog == "1")
                            {
                                pagosLN.actualizaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                pagosLN.insertaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            resQueryLog = String.Empty;

                            error_mensaje = String.Empty;
                        }
                    }
                    catch (Exception e)
                    {
                        resQueryLog = pagosLN.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (resQueryLog == "1")
                        {
                            pagosLN.actualizaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            pagosLN.insertaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        resQueryLog = String.Empty;
                    }
                }
                else
                {
                    try
                    {
                        int resultado = Convert.ToInt32(pagosLN.insertarPagoDebitoLN(pagosEN));
                        if (resultado == 0)
                        {
                            error_mensaje = "Error en la inserción Monto Archivo banco: banco: " +
                                pagosEN.codigoBanco + " " + pagosEN.fechaPago;

                            resQueryLog = pagosLN.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (resQueryLog == "1")
                            {
                                pagosLN.actualizaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                pagosLN.insertaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            resQueryLog = String.Empty;

                            error_mensaje = String.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        resQueryLog = pagosLN.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (resQueryLog == "1")
                        {
                            pagosLN.actualizaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            pagosLN.insertaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        resQueryLog = String.Empty;
                    }
                }
                #endregion

                sw.Close();

                #region ENVIAR CORREO Y EXPORTAR ARCHIVO AL FTP DE SICO

                try
                {   
                    String Mensaje = enviar.EnvioMail("", "PAGOS MASIVOS PARA DÉBITO AUTOMÁTICO DEL BANCO " + objBanco.pNombre,
                    "Buen día, \n\n" +
                    "A continuación se envia un detallado de los pagos procesados a traves del banco " + objBanco.pNombre +
                    " para débito automatico generado el "
                    + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString() + " a las "
                    + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + " \n\n" +
                    "1. Pagos por aplicar: " + debitoOK + " registros.\n" +
                    "2. Fecha de aplicación: " + FechaSico + "\n" +
                    "3. Nombre archivo SICO: " + nombreArchivo + "\n\n" +
                    "4. Valor total $ " + resultsuma + "\n\n" +
                    "El archivo de SICO lo encontrara en la ruta: " + DirectorioSico + ". \n\n" +
                    "La información contenida en este E-mail es confidencial y solo puede ser utilizada por el individuo ó la compañia " +
                    "a la cual esta dirigido. Si no es el receptor autorizado, cualquier retención, difusión, distribución ó copia " +
                    "de este mensaje es prohibida y será sancionada por la ley. Si por error recibe este mensaje, " +
                    "favor reenviarlo al remitente y borrar el mensaje recibido inmediatamente.", objBanco.pCorreoEnvio, objBanco.pRemitente, objBanco.pCorreoControl);

                    ///SAU: Aplicar pago en SICO
                    MoverAFtp objFtp = new MoverAFtp();
                    exporasico = objFtp.enviarFtp(nombreArchivo, FtpSico, DirectorioSico + nombreArchivo, UsaFtp, PassFtp);
                   
                    if (exporasico == "OK")
                    {
                        //Se encarga de aplicar directamente en SICO
                        ServMetodosSICO.ServMetodosSICO smsico = new ServMetodosSICO.ServMetodosSICO();
                        // Lectura de constantes 
                        ParametrosEN parametrosEN = new ParametrosEN();
                        ParametrosLN parametrosLN = new ParametrosLN();
                        // comando = NombreComando + NombrePrograma + " " + nombreArchivo;
                        try
                        {
                            parametrosEN.NombreParametro = "APLICACION_SICO";
                            parametrosEN.Tipo = parametrosEN.Descripcion = parametrosEN.ValorParametro = "";
                            List<ParametrosEN> listaParamSico = parametrosLN.ConsultarParametrosLN(parametrosEN).ToList();
                            if (listaParamSico != null)
                            {
                                if (listaParamSico.Count > 0)
                                {
                                    String Aplicar = listaParamSico[0].ValorParametro;
                                    if (Aplicar == "SI")
                                    {
                                        //Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                                        smsico.Proappaau(nombreArchivo);
                                    }

                                }
                                else
                                {
                                    smsico.Proappaau(nombreArchivo);
                                }
                            }
                            else
                            {
                                smsico.Proappaau(nombreArchivo);
                            }
                            
                           // Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                        }
                        catch (Exception ex)
                        {
                            Mensaje = enviar.EnvioMail(" ", "OCURRIO UN ERROR AL Aplicar Los Pagos en Sico en la Libreria SSHConect " + objBanco.pCodigo + "\n\n",
                                  "Se presento un error al aplicar los pagos en la libreria de Sico. Por favor validar." + ex.ToString(),
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                        }
                        
                        System.Threading.Thread.Sleep(5000);//Espera para aplicación de pago

                        //Almacena pagos consistentes e inconsistentes de SICO      
                        WcfUtilidades Util = new WcfUtilidades();
                        pagosLN.almacenaRegistroSicoLN(Util, nombreArchivo, UsuFTPSystem, PassFTPSystem,
                                                    pagosEN);
                    }
                    else
                    {
                        Mensaje = enviar.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + nombreArchivo + " AL FTP DE SICO DEL BANCO " + objBanco.pNombre, "Buen día, \n\n" +
                          "Se presento un error al crear el archivo a SICO. Por favor validar." + exporasico,
                         ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                         ConfigurationManager.AppSettings["CorreoCC"].ToString());

                        String msg_err = "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + nombreArchivo + " AL FTP DE SICO DEL BANCO " + objBanco.pNombre + " " + exporasico;

                        resQueryLog = pagosLN.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (resQueryLog == "1")
                        {
                            pagosLN.actualizaLogErroresLN("DA: " + msg_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            pagosLN.insertaLogErroresLN("DA: " + msg_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        resQueryLog = String.Empty;
                    }

                    return resultsuma;
                }
                catch
                {
                    sw.Close();
                    File.Delete(DirectorioSico + nombreArchivo);
                    throw new System.Exception("Ocurrio un error al crear archivo plano");
                }
                #endregion

            }
            catch
            {
                sw.Close();
                File.Delete(DirectorioSico + nombreArchivo);
                throw new System.Exception("Ocurrio un error al crear archivo plano");
            }
        }

        //SE ARMA CADA LINEA DE PRENOTA SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineas(ArrayList lineaDatos, List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> lineasSico)
        {
            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;
            foreach (DebitoAutomatico.EN.Tablas.EstructuraArchivo objEst in lineasSico)
            {
                valor = String.Empty;
                if (objEst.pCaracterRelleno.Equals("CR"))
                    caracterRelleno = '0';
                else
                    caracterRelleno = ' ';
                if (objEst.pValorPorDefecto == true)
                {
                    linea += rellenarCampo(objEst.pValor, objEst.pAlineacion, Convert.ToInt32(objEst.pLongitud), caracterRelleno);
                }
                else
                {
                    switch (Convert.ToString(lineaDatos[0]))
                    {
                        case "1EA":
                            break;
                        case "2EL":
                            break;
                        case "3DT":
                            if (objEst.pNombre.Equals("FECHA"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[1])));
                            else if (objEst.pNombre.Equals("OFIC"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            else if (objEst.pNombre.Equals("TIPO-MOV"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            else if (objEst.pNombre.Equals("VALOR"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[2])));
                            else if (objEst.pNombre.Equals("VALOR-CHE"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            else if (objEst.pNombre.Equals("CUPO"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[3])));
                            else if (objEst.pNombre.Equals("DIGITO"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            else if (objEst.pNombre.Equals("HORA"))
                                valor = armarCampo(objEst, String.Empty);
                            break;
                        case "4CL":
                            break;
                        case "5CA":
                            break;
                    }
                    linea += rellenarCampo(valor, objEst.pAlineacion, Convert.ToInt32(objEst.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE RELLENAN LOS CAMPOS DEL ARCHIVO SICO CON CEROS O ESPACIOS SEGUN EL PARAMETRO 
        private String rellenarCampo(String campo, String alineacion, int longitud, char caracterRelleno)
        {
            if (campo.Length <= longitud)
            {
                if (alineacion.Equals("der"))
                {
                    campo = campo.PadLeft(longitud, caracterRelleno);
                }
                else if (alineacion.Equals("izq"))
                {
                    campo = campo.PadRight(longitud, caracterRelleno);
                }
            }
            else
            {
                campo = campo.Substring(0, longitud);
            }

            return campo;
        }

        private String armarCampo(DebitoAutomatico.EN.Tablas.EstructuraArchivo objEst, String valor)
        {
            String campo = String.Empty;
            switch (objEst.pTipoDato)
            {
                case "AN":
                    campo = valor;
                    break;
                case "DE":
                    campo = convertirNumero(valor, objEst.pCantidadDecimales.Value);
                    break;
                case "FE":
                    if (!String.IsNullOrEmpty(valor))
                    {
                        DateTime fechaContrato;
                        string[] fecha = objEst.pFormatoFecha.Split('/');
                        fechaContrato = Convert.ToDateTime(valor);
                        campo = fechaContrato.ToString(fecha[0].ToString()) +
                                fechaContrato.ToString(fecha[1].ToString()) +
                                fechaContrato.ToString(fecha[2].ToString());
                    }
                    else
                    {
                        string[] fecha = objEst.pFormatoFecha.Split('/');
                        campo = DateTime.Today.ToString(fecha[0].ToString()) +
                                DateTime.Today.ToString(fecha[1].ToString()) +
                                DateTime.Today.ToString(fecha[2].ToString());
                    }
                    break;
                case "HH":
                    campo = writeMilitaryTime(DateTime.Now);
                    break;
            }
            return campo;
        }

        private String convertirNumero(String numero, int decimales)
        {

            String[] numeros = numero.Split(',');
            if (!numero.Contains(","))
                return (Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales)).ToString();
            else
                return Convert.ToInt64((Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales - numeros[1].Length))).ToString();

        }

        //***********************************************************************************************************
        //***********************************************************************************************************

        //EN ESTE METODO SE VERIFICA LOS DEBITOS, SE CAMBIA EL ESTADO DEL CLIENTE SEGUN LA RESPUESTA
        //DEL BANCO. TAMBIEN SE LLENA UNA TABLA DE TODOS LOS DEBITOS ACEPTADOS PARA GENERAR EL ARCHIVO
        //SICO
        private void InterpreteRespuesta(DataTable tablaEstados, Int32[,] camposBanco, Banco objB, String Usuario,
                                          String NombreArchivoCargado, String FechaProcesoArchivo, bool procesar,
                                          DataSet ArchivoRespuesta, String Proceso, ref Int32 resultadoOK, ref Int32 resultadoERROR,
                                          Int32 Fiducia, DataTable HistorialTransacciones, bool notificacion)
        {
            try
            {
                #region "ERRORES EN EL ARCHIVO"
                //EN ESTE CICLO SE OBTIENE LOS ERRORES EN EL ARCHIVO
                for (int i = 0; i < ArchivoRespuesta.Tables[0].Rows.Count; i++)
                {
                    String Respuesta = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[0, 0], camposBanco[0, 1]), Convert.ToInt32(camposBanco[0, 2]));
                    String Identificacion = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[1, 0], camposBanco[1, 1]), Convert.ToInt32(camposBanco[1, 2])).TrimStart('0');
                    String Ncuenta = valorCampo(objB.pCodigo == "013" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]).TrimStart('0', ' ') == "" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[3, 0], camposBanco[3, 1]) : ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]) : ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[3, 0], camposBanco[3, 1]), objB.pCodigo == "013" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]).TrimStart('0') == "" ? Convert.ToInt32(camposBanco[3, 2]) : Convert.ToInt32(camposBanco[8, 2]) : Convert.ToInt32(camposBanco[3, 2])).TrimStart('0');
                    String Contrato = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[5, 0], camposBanco[5, 1]), Convert.ToInt32(camposBanco[5, 2])).TrimStart('0');


                    DataRow[] objPC = tablaEstados.Select("CONTRATO='" + Contrato.TrimStart('0') + "'");


                    if (objPC.Length == 0)
                    {
                        ArchivoRespuesta.Tables[0].Rows[i][1] = "- CONTRATO NO SE ENCUENTRA EN ESTADO " + (Proceso.Equals("DEBITO") ? "DEBITO EN PROCESO" : "PRENOTA EN PROCESO");
                        lineasErroneas += 1;
                        continue;
                    }
                    else if (objPC.Length > 0)
                    {
                        // Ncuenta = objB.pCodigo == "013" && objPC[0].ItemArray[7].ToString().Length == 9 ?  : Ncuenta;                   

                        if (objB.pCodigo == "013" && objPC[0].ItemArray[7].ToString().Length == 9)
                        {


                            if (Ncuenta.Length == 15)
                            {
                                Ncuenta = Ncuenta.Substring(0, 3) + Ncuenta.Substring(9, 6);
                            }
                            else
                            {
                                if (Ncuenta.Length == 14)
                                {
                                    Ncuenta = Ncuenta.Substring(0, 2) + Ncuenta.Substring(8, 6);
                                }
                                else
                                {
                                    if (Ncuenta.Length == 13)
                                    {
                                        Ncuenta = Ncuenta.Substring(0, 1) + Ncuenta.Substring(7, 6);
                                    }
                                    
                                }
                            }


                        }

                        if (!objPC[0].ItemArray[4].ToString().Equals(Identificacion.TrimStart('0')))
                        {
                            ArchivoRespuesta.Tables[0].Rows[i][1] = "- CONTRATO CON IDENTIFICACION ERRONEA ";
                            lineasErroneas += 1;
                        }
                        if (!objPC[0].ItemArray[7].ToString().TrimStart('0').Equals(Ncuenta.TrimStart('0')))
                        {
                            ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- CONTRATO CON NUMERO DE CUENTA ERRONEA";
                            lineasErroneas += 1;
                        }
                    }

                    if (objPC[0].ItemArray[4].ToString().Length == 0)
                    {
                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- INFORMACION DEL TITULAR DE LA CUENTA NO ENCONTRADA";
                        lineasErroneas += 1;
                    }
                }
                #endregion

                if (procesar == false)
                    return;

                //INCLUIR LINEA DE CONSULTA DE HISTORIAL Y NO DEJAR QUE ACTUALIZE NI INSERTE CAMPOS Y TAMPOCO LLENE LA TABLA DS3
                for (int i = 0; i < ArchivoRespuesta.Tables[0].Rows.Count; i++)
                {
                    String Respuesta = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[0, 0], camposBanco[0, 1]), Convert.ToInt32(camposBanco[0, 2]));
                    String Identificacion = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[1, 0], camposBanco[1, 1]), Convert.ToInt32(camposBanco[1, 2])).TrimStart('0');
                    String Ncuenta = valorCampo(objB.pCodigo == "013" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]).TrimStart('0', ' ') == "" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[3, 0], camposBanco[3, 1]) : ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]) : ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[3, 0], camposBanco[3, 1]), objB.pCodigo == "013" ? ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[8, 0], camposBanco[8, 1]).TrimStart('0') == "" ? Convert.ToInt32(camposBanco[3, 2]) : Convert.ToInt32(camposBanco[8, 2]) : Convert.ToInt32(camposBanco[3, 2])).TrimStart('0');
                    String Contrato = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[5, 0], camposBanco[5, 1]), Convert.ToInt32(camposBanco[5, 2])).TrimStart('0');
                    String FechaDebito = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[4, 0], camposBanco[4, 1]), Convert.ToInt32(camposBanco[4, 2]));
                    String MensajeRespuesta = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[7, 0], camposBanco[7, 1]), Convert.ToInt32(camposBanco[7, 2]));
                    DataRow[] objPC = tablaEstados.Select("CONTRATO='" + Contrato.TrimStart('0') + "'");
                    if (objB.pCodigo == "013" && MensajeRespuesta == "ORIZADO")
                    {
                        MensajeRespuesta = "AUTORIZADO";
                    }

                    if (objPC.Length > 0)
                    {
                        //    Ncuenta = objB.pCodigo == "013" && objPC[0].ItemArray[7].ToString().Length == 9 ? Ncuenta.Substring(0, 4) + Ncuenta.Substring(10, 5) : Ncuenta;
                        if (objB.pCodigo == "013" && objPC[0].ItemArray[7].ToString().Length == 9)
                        {

                            if (Ncuenta.Length == 15)
                            {
                                Ncuenta = Ncuenta.Substring(0, 3) + Ncuenta.Substring(9, 6);
                            }
                            else
                            {
                                if (Ncuenta.Length == 14)
                                {
                                    Ncuenta = Ncuenta.Substring(0, 2) + Ncuenta.Substring(8, 6);
                                }
                                else
                                {
                                    if (Ncuenta.Length == 13)
                                    {
                                        Ncuenta = Ncuenta.Substring(0, 1) + Ncuenta.Substring(7, 6);
                                    }

                                }
                            }

                        }
                    }
                    String ValorDebitar = String.Empty;
                    if (Proceso.Equals("DEBITO"))
                        ValorDebitar = valorCampo(ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString().Substring(camposBanco[2, 0], camposBanco[2, 1]), Convert.ToInt32(camposBanco[2, 2]));
                    DataRow[] objP = tablaEstados.Select("NUMERO_IDENTIFICACION='" + Identificacion.TrimStart('0') +
                              "' AND NUMERO_CUENTA like'*" + Ncuenta.TrimStart('0') +
                              "' AND CONTRATO='" + Contrato.TrimStart('0') + "'");
                    DataRow[] objTransacciones = HistorialTransacciones.Select("RESPUESTA='" + Respuesta +
                              "' AND CONTRATO='" + Contrato.TrimStart('0') + "' AND VALOR='" + ValorDebitar + "'");
                    if (objTransacciones.Length > 0)
                    {
                        lineasErroneas += 1;
                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- LINEA PROCESADA ANTERIORMENTE";
                        continue;
                    }

                    RespuestaTransaccion objresp = new RespuestaTransaccion();
                    objresp.pBanco = Convert.ToInt32(objB.pId);
                    objresp.pCodigo = Respuesta;
                    RespuestaTransaccionLN respuestabanco = new RespuestaTransaccionLN();
                    List<RespuestaTransaccion> ListaR = new List<RespuestaTransaccion>();
                    ListaR = respuestabanco.consultarRespuestas(objresp);
                    if (ListaR.Count > 0)
                    {
                        objresp = ListaR[0];
                    }
                    else
                    {
                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- CODIGO DE CAUSAL DESCONOCIDO";
                        lineasErroneas += 1;
                        continue;
                    }

                    if (objP.Length > 0)
                    {
                        if (objP[0].ItemArray[4].ToString().Length != 0)
                        {
                            TitularCuenta objTC = new TitularCuenta();
                            DatosDebito objTD = new DatosDebito();
                            objTD.pContrato = Contrato;
                            objTC.pId = Convert.ToInt32(objP[0].ItemArray[2].ToString());
                            List<EN.Tablas.TitularCuenta> listaT = new TitularCuentaLN().consultarTerceros(objTC);
                            if (listaT.Count > 0)
                                objTC = listaT[0];
                            else
                            {
                                ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- INFORMACION DEL TITULAR DE LA CUENTA NO ENCONTRADA";
                                lineasErroneas += 1;
                                continue;
                            }

                            //Consulta la fecha de débito que tiene en ese momento
                            DatosDebito ObjFecDe = new DatosDebito();
                            ObjFecDe.pContrato = Contrato;
                            List<DatosDebito> listfec = new DatosDebitoLN().consultarFecha(ObjFecDe);

                            DatosDebito ResulF = new DatosDebito();
                            if (listfec.Count > 0)
                            {
                                ResulF = listfec[0];
                            }

                            //SI ESTA EN EL WEB CONFIG TOMA LA CAUSAL PARAMETRIZADA SI NO TOMA LA CAUSAL DEL ARCHIVO
                            if (ConfigurationManager.AppSettings["CodBanco"].ToString().Contains("," + Convert.ToInt32(objB.pId) + ","))
                                objHPU.pCausal = objresp.pRespuesta;
                            else
                                objHPU.pCausal = MensajeRespuesta;

                            if (objresp.pEstadoRespuesta.Equals("ACEPTADO"))
                            //PRENOTA O DEBITO ACEPTADO
                            {
                                if (Proceso.Equals("PRENOTA") && objresp.pEstadoPrenota != 0)
                                {
                                    int val = 0;
                                    if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 2 || Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 3 ||
                                    Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 4)
                                    {
                                        if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 4)
                                        {
                                            val = 1;
                                        }
                                        else
                                        {
                                            objTD.pEstado = objresp.pEstadoPrenota;
                                            val = new DatosDebitoLN().actualizarEstado(objTD);
                                        }
                                    }
                                    else if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 3)
                                    {
                                        val = 1;
                                    }
                                    if (val <= 0)
                                    {
                                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- ERROR EN LA ACTUALIZACION DEL ESTADO DEL CLIENTE";
                                        lineasErroneas += 1;
                                        continue;
                                    }
                                }
                                else if (Proceso.Equals("DEBITO") && objresp.pEstadoDebito != 0)
                                {
                                    PagoParcial objPP = new PagoParcial();
                                    objPP.pContrato = Contrato;
                                    List<EN.Tablas.PagoParcial> listaP = new PagoParcialLN().consultar(objPP); //Consulta si tiene pago parcial
                                    int val = 0;
                                    int borrar = 0;
                                    if (listaP.Count > 0)
                                    {
                                        objTD.pEstado = 4; //Actualiza el estado a debito
                                        val = new DatosDebitoLN().actualizarEstado(objTD);

                                        borrar = new PagoParcialLN().borrar(objPP); //Lo borra de pagos parciales
                                    }
                                    else
                                    {
                                        objTD.pEstado = objresp.pEstadoDebito;
                                        val = new DatosDebitoLN().actualizarEstado(objTD);
                                    }

                                    if (val <= 0)
                                    {

                                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- ERROR EN LA ACTUALIZACION DEL ESTADO DEL CLIENTE";
                                        lineasErroneas += 1;
                                        continue;
                                    }
                                }
                                resultadoOK += 1;

                                if (Proceso.Equals("DEBITO"))
                                {
                                    DataRow dr3 = dt3.NewRow();
                                    dr3["fecha"] = Convert.ToString(Convert.ToDateTime(objB.pCodigo == "013" ? FechaDebito.Substring(6, 2) + "-" + FechaDebito.Substring(4, 2) + "-" + FechaDebito.Substring(0, 4) : FechaDebito));
                                    dr3["linea"] = ArchivoRespuesta.Tables[0].Rows[i].ItemArray[0].ToString();
                                    dt3.Rows.Add(dr3);
                                }

                                ArchivoRespuesta.Tables[0].Rows[i].Delete();
                                i--;

                                objHPU.pNumeroIdentificacion = Identificacion.TrimStart('0');
                                objHPU.pTipoCuenta = objP[0].ItemArray[6].ToString();
                                objHPU.pNumeroCuenta = objP[0].ItemArray[7].ToString();
                                objHPU.pNombreBanco = objP[0].ItemArray[8].ToString();
                                objHPU.pTipoTransferencia = Proceso;
                                objHPU.pFecha = Convert.ToString(Convert.ToDateTime(objB.pCodigo == "013" ? FechaDebito.Substring(6, 2) + "-" + FechaDebito.Substring(4, 2) + "-" + FechaDebito.Substring(0, 4) : FechaDebito).ToString("dd/MM/yyyy")) + " " + DateTime.Now.ToString("H:mm:ss");
                                objHPU.pValor = ValorDebitar.TrimStart('0');
                                objHPU.pRespuesta = Respuesta;
                                objHPU.pNombreBancoDebita = objB.pNombre;
                                objHPU.pFechaProceso = Convert.ToString(Convert.ToDateTime(FechaProcesoArchivo).ToString("dd/MM/yyyy")) + " " + DateTime.Now.ToString("H:mm:ss");
                                objHPU.pUsuario = Usuario;
                                objHPU.pContrato = Contrato.TrimStart('0');
                                objHPU.pNombreCliente = objP[0].ItemArray[5].ToString();
                                objHPU.pNombreArchivo = NombreArchivoCargado;
                                objHPU.pEstado = objresp.pEstadoRespuesta;
                                objHPU.pFechaDebito = ResulF.pFechaDebito;
                                new HistorialProcesoUsuarioLN().insertar(objHPU);
                            }
                            else
                            //PRENOTA O DEBITO RECHAZADO
                            {
                                if (Proceso.Equals("PRENOTA") && objresp.pEstadoPrenota != 0)
                                {
                                    int val = 0;
                                    String Mensaje = String.Empty;
                                    if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 2 || Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 3 ||
                                    Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 4)
                                    {
                                        if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 4)
                                        {
                                            val = 1;
                                        }
                                        else
                                        {
                                            objTD.pEstado = objresp.pEstadoPrenota;
                                            val = new DatosDebitoLN().actualizarEstado(objTD);
                                        }
                                    }
                                    else if (Convert.ToInt32(objP[0].ItemArray[11].ToString()) == 3)
                                    {
                                        val = 1;
                                    }
                                    if (val <= 0)
                                    {
                                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- ERROR EN LA ACTUALIZACION DEL ESTADO DEL CLIENTE";
                                        lineasErroneas += 1;
                                    }
                                }
                                else if (Proceso.Equals("DEBITO") && objresp.pEstadoDebito != 0)
                                {

                                    PagoParcial objPP = new PagoParcial();
                                    objPP.pContrato = Contrato;
                                    List<EN.Tablas.PagoParcial> listaP = new PagoParcialLN().consultar(objPP); //Consulta si tiene pago parcial
                                    int val = 0;
                                    int borrar = 0;
                                    if (listaP.Count > 0)
                                    {
                                        objTD.pEstado = 4; //Actualiza el estado a debito
                                        val = new DatosDebitoLN().actualizarEstado(objTD);

                                        borrar = new PagoParcialLN().borrar(objPP); //Lo borra de pagos parciales
                                    }
                                    else
                                    {
                                        objTD.pEstado = objresp.pEstadoDebito;
                                        val = new DatosDebitoLN().actualizarEstado(objTD);
                                    }

                                    if (val <= 0)
                                    {
                                        ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- ERROR EN LA ACTUALIZACION DEL ESTADO DEL CLIENTE";
                                        lineasErroneas += 1;
                                    }
                                }
                                resultadoERROR += 1;
                                ArchivoRespuesta.Tables[0].Rows[i].Delete();
                                i--;


                                objHPU.pNumeroIdentificacion = Identificacion.TrimStart('0');
                                objHPU.pTipoCuenta = objP[0].ItemArray[6].ToString();
                                objHPU.pNumeroCuenta = objP[0].ItemArray[7].ToString();
                                objHPU.pNombreBanco = objP[0].ItemArray[8].ToString();
                                objHPU.pTipoTransferencia = Proceso;
                                objHPU.pFecha = Convert.ToString(Convert.ToDateTime(objB.pCodigo == "013" ? FechaDebito.Substring(6, 2) + "-" + FechaDebito.Substring(4, 2) + "-" + FechaDebito.Substring(0, 4) : FechaDebito).ToString("dd/MM/yyyy")) + " " + DateTime.Now.ToString("H:mm:ss");
                                objHPU.pValor = ValorDebitar.TrimStart('0');
                                objHPU.pRespuesta = Respuesta;
                                objHPU.pNombreBancoDebita = objB.pNombre;
                                objHPU.pFechaProceso = Convert.ToString(Convert.ToDateTime(FechaProcesoArchivo).ToString("dd/MM/yyyy")) + " " + DateTime.Now.ToString("H:mm:ss");
                                objHPU.pUsuario = Usuario;
                                objHPU.pContrato = Contrato.TrimStart('0');
                                objHPU.pNombreCliente = objP[0].ItemArray[5].ToString();
                                objHPU.pNombreArchivo = NombreArchivoCargado;
                                objHPU.pEstado = objresp.pEstadoRespuesta;
                                objHPU.pFechaDebito = ResulF.pFechaDebito;
                                new HistorialProcesoUsuarioLN().insertar(objHPU);
                            }

                            //ENVIO DEL CORREO AL CLIENTE PARA NOTIFICARLE LA RESPUESTA DEL BANCO

                            DataSet dsSico = new DataSet();
                            ClienteSico objS = new ClienteSico();
                            objS.pContrato = Convert.ToInt32(Contrato);
                            dsSico = new ClienteSICOLN().consultarClienteLectura(objS);

                            // if (notificacion == true)
                            //{

                            //    if (objresp.pEnvioCorreo == 1)
                            //    {
                            //        //si en la tabla consultamdo pór causal es igual a verdadero
                            //        String CorreoCliente = String.Empty;
                            //        String CiudadCliente = String.Empty;

                            //        if (dsSico.Tables["ClienteSICO"].Rows.Count > 0)
                            //        {
                            //            CorreoCliente = dsSico.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();
                            //            CiudadCliente = dsSico.Tables["ClienteSICO"].Rows[0]["CIUDAD"].ToString();
                            //        }

                            //        String Correo = String.Empty;
                            //        String MensajeFinal = String.Empty;

                            //        Mensajes objM = new Mensajes();
                            //        double valor = 0;
                            //        string resultvalor = String.Empty;
                            //        objM.pTipoContrato = "S";

                            //        switch (Proceso)
                            //        {
                            //            case "PRENOTA":
                            //                objM.pEstadoDebito = 1;
                            //                break;
                            //            case "DEBITO":
                            //                objM.pEstadoDebito = 2;
                            //                valor = Convert.ToDouble(ValorDebitar.TrimStart('0'));
                            //                resultvalor = Convert.ToString(ValorDebitar.TrimStart('0'));
                            //                resultvalor = valor.ToString("0,0", CultureInfo.InvariantCulture);
                            //                resultvalor = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);
                            //                break;
                            //        }

                            //        switch (objresp.pEstadoRespuesta)
                            //        {
                            //            case "ACEPTADO":
                            //                objM.pMotivo = 3;
                            //                break;
                            //            case "RECHAZADO":
                            //                objM.pMotivo = 4;
                            //                break;
                            //        }

                            //        List<EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                            //        if (listaM.Count > 0)
                            //        {
                            //            if (CorreoCliente != String.Empty)
                            //            {

                            //                if (dsSico.Tables["ClienteSICO"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                            //                {
                            //                    objM = listaM[0];
                            //                    MensajeFinal = objM.pMensaje
                            //                    .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                            //                    .Replace("@Nombrecliente_", objHPU.pNombreCliente)
                            //                    .Replace("@NroContrato_", Contrato.TrimStart('0'))
                            //                    .Replace("@Ciudad_", CiudadCliente)
                            //                    .Replace("@causalrechazo_", objHPU.pCausal)
                            //                    .Replace("@fechadébito_", objHPU.pFecha)
                            //                    .Replace("@Valordébito_", resultvalor);

                            //                    //Linea de pruebas
                            //                    Correo = enviar.EnvioMail("", objM.pAsunto, MensajeFinal, objB.pCorreoEnvio, objB.pRemitente, objB.pCorreoControl);

                            //                    ////Linea de produccion
                            //              //      Correo = enviar.EnvioMail("", objM.pAsunto, MensajeFinal, CorreoCliente, objB.pRemitente, objB.pCorreoControl);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}



                            /////////--------------------------------------------------------------correo
                            //ACTUALIZA LA INFORMACION EN LA TABLA FINAL SI EL CLIENTE ACTUALIZO DESDE EL APP O LA WEB EN UN ESTADO NO PERMITIDO
                            int elim = 0;
                            int valorD = 0;
                            int valorT = 0;
                            ActualizaCliente objAc = new ActualizaCliente();
                            DatosDebito objD = new DatosDebito();
                            TitularCuenta objT = new TitularCuenta();
                            objAc.pContrato = Contrato.TrimStart('0');
                            objAc.pIdTitularCuenta = objTC.pId;
                            List<EN.Tablas.ActualizaCliente> listaB = new ActualizaClienteLN().consultarDatos(objAc);

                            if (listaB.Count > 0)
                            {
                                objAc = listaB[0];
                                objD.pContrato = objAc.pContrato;
                                objD.pIdTitularCuenta = objAc.pIdTitularCuenta;
                                objD.pNumeroCuenta = objAc.pNumeroCuenta;
                                objD.pTipoCuenta = objAc.pIdTipoCuenta;
                                objD.pIdBanco = objAc.pIdBanco;
                                objD.pSuspendido = false;
                                objD.pFechaInicioSus = String.Empty;
                                objD.pFechaFinSus = String.Empty;
                                objD.pEstado = 1;
                                objD.pTercero = false;

                                valorD = new DatosDebitoLN().actualizarCuentas(objD);

                                objT.pId = objAc.pIdTitularCuenta;
                                objT.pNombre = dsSico.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString();
                                objT.pNumeroIdentificacion = dsSico.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                                objT.pTipoIdentificacion = UtilidadesWeb.homologarDocumento(dsSico.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString());
                                valorT = new TitularCuentaLN().actualizar(objT);

                                if (valorD > 0 && valorT > 0)
                                {
                                    elim = new ActualizaClienteLN().eliminar(objAc);

                                    if (elim == 0)
                                    {
                                        string campos = String.Empty;

                                        #region (INFORMACION PARA LOG ELMINACIÓN)
                                        campos = string.Concat(objD.pContrato);
                                        Log(1, objD.pContrato, campos);
                                        #endregion

                                        Banco objLogB = new Banco();
                                        objLogB.pId = objD.pIdBanco;
                                        List<EN.Tablas.Banco> listLogB = new BancoLN().consultarBanco(objLogB);
                                        Banco objBanL = new Banco();

                                        if (listLogB.Count > 0)
                                        {
                                            objBanL = listLogB[0];

                                            #region (INFORMACION PARA LOG ACTUALIZACIÓN)
                                            campos = string.Concat(objD.pContrato,
                                                " con NOMBRE CLIENTE:", objT.pNombre,
                                                ", TIPO DE INDENTIFICACIÓN:", objT.pTipoIdentificacion,
                                                ", NÚMERO DE DOCUMENTO:", objT.pNumeroIdentificacion,
                                                ", TIPO DE CUENTA:", objD.pTipoCuenta,
                                                ", NÚMERO DE CUENTA:", objD.pNumeroCuenta,
                                                ", BANCO:", objBanL.pNombre);
                                            Log(2, objD.pContrato, campos);
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ArchivoRespuesta.Tables[0].Rows[i][1] = ArchivoRespuesta.Tables[0].Rows[i][1] + "- INFORMACION DEL TITULAR DE LA CUENTA NO ENCONTRADA";
                            lineasErroneas += 1;
                        }
                    }
                }
                if (Proceso.Equals("DEBITO"))
                {
                    DataView dv = dt3.DefaultView;
                    dv.Sort = "fecha";
                    dt3 = dv.ToTable();
                    ds3.Tables.Add(dt3);
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Ocurrio un error al actualizar " + Proceso.ToLower() + ex.Message);
            }

        }
        /// <summary>
        /// Nuevo Envio de Correo
        /// </summary>
        /// <param name="list"></param>
        public void EnviarCorreo(List<HistorialArchivos> list)
        {

            DataSet dsSico = new DataSet();
            ClienteSico objS = new ClienteSico();

            foreach (var lista in list)
            {
                Banco objB = new Banco();
                objB.pNombre = lista.pbanco;
                List<EN.Tablas.Banco> listaB = new BancoLN().consultarBancoid(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                }
                objS.pContrato = Convert.ToInt32(lista.pContrato);
                dsSico = new ClienteSICOLN().consultarClienteCorreo(objS);
                if (lista.pMarca == true)
                {
                    //si en la tabla consultamdo pór causal es igual a verdadero
                    String CorreoCliente = String.Empty;
                    String CiudadCliente = String.Empty;

                    if (dsSico.Tables["ClienteSICO"].Rows.Count > 0)
                    {
                        CorreoCliente = dsSico.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();
                        CiudadCliente = dsSico.Tables["ClienteSICO"].Rows[0]["CIUDAD"].ToString();
                    }

                    String Correo = String.Empty;
                    String MensajeFinal = String.Empty;

                    Mensajes objM = new Mensajes();
                    double valor = 0;
                    string resultvalor = String.Empty;
                    objM.pTipoContrato = "S";

                    switch (lista.pTipo_transferencia)
                    {
                        case "PRENOTA":
                            objM.pEstadoDebito = 1;
                            break;
                        case "DEBITO":
                            objM.pEstadoDebito = 2;
                            valor = Convert.ToDouble(lista.pValor.TrimStart('0'));
                            resultvalor = Convert.ToString(lista.pValor.TrimStart('0'));
                            resultvalor = valor.ToString("0,0", CultureInfo.InvariantCulture);
                            resultvalor = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);
                            break;
                    }

                    switch (lista.pEstado)
                    {
                        case "ACEPTADO":
                            objM.pMotivo = 3;
                            break;
                        case "RECHAZADO":
                            objM.pMotivo = 4;
                            break;
                    }

                    List<EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                    if (listaM.Count > 0)
                    {
                        bool mensaje = true;
                        string Observaciones = String.Empty;

                        if (CorreoCliente != String.Empty)
                        {

                            if (dsSico.Tables["ClienteSICO"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                            {
                                objM = listaM[0];
                                MensajeFinal = objM.pMensaje
                                .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                .Replace("@Nombrecliente_", lista.pNombreCliente)
                                .Replace("@NroContrato_", lista.pContrato.TrimStart('0'))
                                .Replace("@Ciudad_", CiudadCliente)
                                .Replace("@causalrechazo_", lista.pCausal)
                                .Replace("@fechadébito_", lista.pFecha)
                                .Replace("@Valordébito_", resultvalor);

                                //Linea de pruebas
                                // Correo = enviar.EnvioMail("", objM.pAsunto, MensajeFinal, objB.pCorreoEnvio, objB.pRemitente, objB.pCorreoControl);

                                ////Linea de produccion
                                Correo = enviar.EnvioMail("", objM.pAsunto, MensajeFinal, CorreoCliente, objB.pRemitente, objB.pCorreoControl);

                                mensaje = true;
                                Observaciones = "Correo exitoso";
                            }
                            else
                            {
                                mensaje = false;
                                Observaciones = "No autoriza envio de correo";
                            }
                        }
                        else
                        {
                            mensaje = false;
                            Observaciones = "Correo electronico vacio";
                        }

                        //Guardar tabla de correos
                        int correo = 0;
                        Correos ObjCorreos = new Correos();
                        ObjCorreos.NombreArchivo = lista.pNombreArchivo;
                        ObjCorreos.Contrato = objS.pContrato.ToString();
                        ObjCorreos.Envio = mensaje;
                        ObjCorreos.Obervaciones = Observaciones;
                        correo = new CorreosLN().insertar(ObjCorreos);
                    }


                }
            }
        }

        /// <summary>
        /// Meotodo Log que guarda la actualizacion del cliente en la tabla tb_DEB_LOGS_USUARIO
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="ContratoCliente"></param>
        private void Log(int opcion, String ContratoCliente, string campos)
        {
            string fecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
            LogsUsuario objL = new LogsUsuario();
            switch (opcion)
            {
                case 1:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = "AUTOMATICO";
                    objL.pDetalle = "Se elimino de la temporal el contrato N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ELIMINACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
                case 2:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = "AUTOMATICO";
                    objL.pDetalle = "Se actualizó el contrato N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ACTUALIZACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
            }
        }

        //***********************************************************************************************************
        //***********************************************************************************************************

        private ArrayList obtenerLimites(DataSet tablaArchivo)
        {
            int registros = 0, tot = 1;
            int veces = 0;
            DateTime fechas = new DateTime();
            ArrayList limite = new ArrayList();
            foreach (DataRow renglon in tablaArchivo.Tables[0].Rows)
            {
                if (registros == 0)
                    fechas = Convert.ToDateTime(renglon[0].ToString());
                if (fechas.ToShortDateString() == Convert.ToDateTime(renglon[0].ToString()).ToShortDateString())
                {
                    registros += 1;
                }
                else
                {
                    limite.Add(registros);
                    registros = 1;
                    veces += 1;
                }
                if (tot == tablaArchivo.Tables[0].Rows.Count)
                {
                    limite.Add(registros);
                    break;
                }
                fechas = Convert.ToDateTime(renglon[0].ToString());
                tot += 1;
            }
            return limite;
        }
        // SE CARGA LOS PARAMETROS DE LA ESTRUCTURA (RESPUESTA BANCO Y SICO)
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> consultarEstructura(String tipoLinea, String tipoProceso, Int32 Idbanco)
        {
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraArchivo(tipoLinea, tipoProceso, Idbanco);
            List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> lista = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
            DebitoAutomatico.EN.Tablas.EstructuraArchivo Entidad;

            foreach (DataRow fila in tabla.Rows)
            {

                Entidad = new DebitoAutomatico.EN.Tablas.EstructuraArchivo();

                Entidad.pId = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Id]);
                Entidad.pConfiguracion = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Configuracion]);
                Entidad.pTipoDato = Convertidor.aCadena(fila[EstructuraArchivoDEF.TipoDato]);
                Entidad.pNombre = Convertidor.aCadena(fila[EstructuraArchivoDEF.Nombre]);
                Entidad.pColumna = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Columna]);
                Entidad.pLongitud = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Longitud]);
                Entidad.pCaracterRelleno = Convertidor.aCadena(fila[EstructuraArchivoDEF.CaracterRelleno]);
                Entidad.pAlineacion = Convertidor.aCadena(fila[EstructuraArchivoDEF.Alineacion]);
                Entidad.pCantidadDecimales = Convertidor.aEntero32(fila[EstructuraArchivoDEF.CantidadDecimales]);
                Entidad.pFormatoFecha = Convertidor.aCadena(fila[EstructuraArchivoDEF.FormatoFecha]);
                Entidad.pEsContador = Convertidor.aBooleano(fila[EstructuraArchivoDEF.EsContador]);
                Entidad.pSumaCampo = Convertidor.aEntero32(fila[EstructuraArchivoDEF.SumaCampo]);
                Entidad.pRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                Entidad.pValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);
                Entidad.pValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);

                lista.Add(Entidad);

            }

            return lista;
        }

        //EJ: 
        //fecha='2014-03-06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='2014/03/06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='20140306' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='2014 03 06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //FUNCIONA SOLO PARA FECHAS QUE NO CONTENGAN MAS DE 10 CARACTERES, POR LO TANTO DEBEN
        //VENIR SIN NUMERO
        private String fechaTransformado(String fecha)
        {
            String formato = String.Empty;
            foreach (DebitoAutomatico.EN.Tablas.EstructuraArchivo objBan in BListaLinea3DT)
            {
                if (objBan.pNombre.Equals("Fecha del debito"))
                {
                    formato = objBan.pFormatoFecha;
                    break;
                }
            }

            int x = 0, dia = 0, mes = 0, ano = 0;
            DateTime fecha1 = DateTime.Now;
            string[] format = formato.Split('/');
            if (fecha.Length == 10)
                x = 1;

            string[] f = {  fecha.Substring(0,format[0].Length),
            fecha.Substring(format[0].Length + x,format[1].Length),
            fecha.Substring(format[0].Length + format[1].Length + 2 * x ,format[2].Length)};

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i].ToString().Equals("yyyy"))
                    ano = Convert.ToInt32(f[i].ToString());
                else if (format[i].ToString().Equals("MM"))
                    mes = Convert.ToInt32(f[i]);
                else if (format[i].ToString().Equals("dd"))
                    dia = Convert.ToInt32(f[i]);
            }

            fecha1 = new DateTime(ano, mes, dia, 0, 0, 0);

            return fecha1.ToString();

        }

        //SE CONVIERTE UNA HORA NORMAL A HORA MILITAR
        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
        }

        //SE CALCULA EL CONSECUTIVO DE UN ARCHIVO SICO QUE FUE GENEREADO MAS DE UNA VEZ EN EL MISMO DIA
        private String consecutivo(int columna)
        {
            string[] caracter = {"A","B","C","D","E","F","G","H","I","J","K",
                                "L","M","N","O","P","Q","R","S","T","U","W",
                                "X","Y","Z","0","1","2","3","4","5","6","7","8","9"};

            if (columna > 34)
                return caracter[0];
            else
                return caracter[columna];
        }

        private String evaluarCampo(DebitoAutomatico.EN.Tablas.EstructuraArchivo objEA, String valor)
        {
            //SE LLENA UN OBJETO TIPO DICTIONARY CON ID'S Y SU VALOR TIPO DOUBLE
            if (idASumar.Contains(Convert.ToString(objEA.pId.Value)) == true)
            {
                if (contadores.ContainsKey(objEA.pId.Value) == true)
                {
                    double value = contadores[objEA.pId.Value] + Convert.ToDouble(valor);
                    contadores[objEA.pId.Value] = value;
                }
                else
                {
                    contadores.Add(objEA.pId.Value, Convert.ToDouble(valor));
                }
            }
            //CUANDO EL OBJETO REQUIERE TRANSFORMACION BUSCA UN CODIGO EN UNA TABLA PRECARGADA
            if (objEA.pRequiereCambio == true)
            {
                return obtenerEquivalencia(objEA, Convert.ToString(valor));//RETORNA
            }
            //CUANDO EL CAMPO ES CONTADOR RETORNA LA SUMATORIA DE UN CAMPO PREESTABLECIDO
            else if (objEA.pEsContador == true)
            {
                return Convert.ToString(contadores[objEA.pSumaCampo.Value]);
            }
            return valor;
        }

        //EN ESTA RUTINA BUSCA LA EQUIVALENCIA DEL OBJETO objEA. SI TIENE UNA EQUIVALENCIA ASIGNADA A UN CAMPO DIRECTAMENTE
        //OBTIENE EL CODIGO DEL CAMPO, PERO SI LA EQUIVALENCIA ESTA ASIGNADA A UNA TABLA ENTONCES DEL OBJETO objEA SACA EL
        //VALOR Y LO COMPARA CON TODOS LOS VALORES DE LOS CAMPOS DE LA TABLA Y CUANDO ENCUENTRE COINCIDENCIA DE ESE CAMPO
        //ENTONCES OBTIENE EL CODIGO DEL CAMPO. SI NO SE CUMPLE NINGUNA DE LAS ANTERIORES RETORNA VACIO.        
        private String obtenerEquivalencia(DebitoAutomatico.EN.Tablas.EstructuraArchivo objEA, String valor)
        {

            DataRow[] objE = tablaE.Select("IDEA=" + objEA.pId.Value);

            if (objE.Length == 0)
                return String.Empty;

            if (objE[0].ItemArray[3].ToString().Length != 0)
            {
                DataRow[] objCE = tablaCE.Select("ID=" + objE[0].ItemArray[3].ToString());
                return objCE[0].ItemArray[2].ToString();
            }
            else
            {
                DataRow[] objCE = tablaCE.Select("TABLAS_EQUIVALENCIAS=" + objE[0].ItemArray[2].ToString());

                foreach (DataRow CE in objCE)
                {
                    if (CE.ItemArray[4].ToString().Equals(valor) || (CE.ItemArray[4].ToString().Equals(objEA.pValor) && (objEA.pValorPorDefecto == true)))
                    {
                        return CE.ItemArray[2].ToString();
                    }
                }
            }
            return valor;

        }

        //SE OBTIENE EL VALOR DEL CAMPO CON RELACION A LA ESTRUCTURA DEL ARCHIVO
        private String valorCampo(String campo, Int32 IdCampo)
        {
            String valor = String.Empty;

            foreach (DebitoAutomatico.EN.Tablas.EstructuraArchivo objEst in BListaLinea3DT)
            {
                if (objEst.pId.Value == IdCampo)
                {
                    if (campo.Length == 8)
                    {
                        //campo = campo.Substring(6, 2) + "-" + campo.Substring(4, 2) + "-" + campo.Substring(0, 4);
                        //valor = armarCampoBanco(objEst, campo.Trim());
                        //return valor;

                        valor = armarCampoBanco(objEst, campo.Trim());
                        return valor;
                    }
                    else
                    {
                        valor = armarCampoBanco(objEst, campo.Trim());
                        return valor;
                    }
                }
            }
            return campo;
        }

        private String armarCampoBanco(DebitoAutomatico.EN.Tablas.EstructuraArchivo objBan, String valor)
        {
            String campo = String.Empty;
            switch (objBan.pTipoDato)
            {
                case "AN":
                    campo = valor;
                    break;
                case "DE":
                    campo = numeroTransformado(valor, objBan.pCantidadDecimales.Value);
                    break;
                case "FE":
                    if (!String.IsNullOrEmpty(valor) || valor.Length < 8)
                    {
                        campo = fechaTransformado(valor, objBan.pFormatoFecha);
                    }
                    else
                    {
                        campo = "0";
                    }
                    break;
                case "HH":
                    campo = writeMilitaryTime(DateTime.Now);
                    break;
            }
            return campo;
        }

        private String numeroTransformado(string numero, int decimales)
        {

            int divisor = Convert.ToInt32(Math.Pow(10, decimales));
            decimal valor = Convert.ToDecimal((numero)) / divisor;
            return Convert.ToString(decimal.Round(valor, 2));

        }

        private String fechaTransformado(String fecha, String formato)
        {

            int x = 0, dia = 0, mes = 0, ano = 0;
            DateTime fecha1 = DateTime.Now;
            string[] format = formato.Split('/');
            if (fecha.Length == 10)
                x = 1;

            string[] f = {  fecha.Substring(0,format[0].Length),
            fecha.Substring(format[0].Length + x,format[1].Length),
            fecha.Substring(format[0].Length + format[1].Length + 2 * x ,format[2].Length)};

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i].ToString().Equals("yyyy"))
                    ano = Convert.ToInt32(f[i].ToString());
                else if (format[i].ToString().Equals("MM"))
                    mes = Convert.ToInt32(f[i]);
                else if (format[i].ToString().Equals("dd"))
                    dia = Convert.ToInt32(f[i]);
            }

            fecha1 = new DateTime(ano, mes, dia, 0, 0, 0);

            return fecha1.ToString();

        }

    }
}