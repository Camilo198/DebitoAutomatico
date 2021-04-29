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

using System.Data.Odbc;
using System.Configuration;
using System.Text;


namespace DebitoAutomatico.PS.Codigo.Debito
{
    public class GenerarDebito
    {
        #region DEFINICION DE VARIABLES

        //ESTRUCTURA DEL ARCHIVO DEBITO
        //*************************************************************************************
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> ListaLinea1EA { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> ListaLinea2EL { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> ListaLinea3DT { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> ListaLinea4CL { get; set; }
        private List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> ListaLinea5CA { get; set; }
        String LineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String LineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String LineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE
        String LineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE       
        String LineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO       
        //*************************************************************************************

        DataTable tablaCE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA DE LOS CAMPOS_EQUIVALENCIAS
        DataTable tablaE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA EQUIVALENCIAS

        int registrosLote = 0;
        int ConsecutivoLog = 0;
        int sumaletra = 0;
        String ConsecutivoBanc = String.Empty;
        //double valorServicio = 0;
        ArrayList idASumar = new ArrayList(); //CONTIENE LOS ID'S DE LOS CAMPOS QUE SE VAN A SUMAR      
        Dictionary<int, double> contadores = new Dictionary<int, double>();
        static StreamWriter sw;


        String ConsecutivoArchivo; // CONTIENE UN CONSECUTIVO QUE SE USA PARA CONTROLAR CUANTAS VECES SE HA GENERADO EL ARCHIVO EN EL DIA
        String FechaArchivo, FechaDebito;
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA EL ARCHIVO               
        String tipoArchivo = String.Empty;
        String CuentaPrincipal = String.Empty;//Fiducia Principal Chevyplan
        ArrayList retornoMetodo = new ArrayList();

        //PARAMETROS CHEVYPLAN
        //*************************************************************************************
        String nombreEmpresa = String.Empty;
        String IdentificacionEmpresa = String.Empty;
        String MoraEmpresa = String.Empty;
        String MontoMinimoGan = String.Empty;
        String MontoMinimoAdj = String.Empty;
        String MontoMinimoSus = String.Empty;
        //*************************************************************************************

        //PARAMETROS JOB
        //*************************************************************************************
        DataSet Job = new DataSet();
        DataSet EjecucionJob = new DataSet();
        DataSet ConsultaJob = new DataSet();
        long FecHorEje =0;
        long FecHorDesEje = 0;
        int EstadoDesEje = 0;
        //*************************************************************************************


        #endregion

        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion.

        public ArrayList ServicioDebito(Int32 IdBanco, String BancoDebita, String Usuario, ArrayList CodBancos, bool suscriptores,
                                     bool adjudicados, bool ganadores, bool CuotasxDevolver, string AnoDebito, String MesaDebitar, String fechaDelDebito, bool esManual,
                                     Dictionary<String, String> contratosDeb, String FechaProceso)
        {

            //Se comentarea para pruebas
            #region EJECUCION JOB DEBITO

            //if (esManual == false)
            //{
            //    if (ConsultaJob.Tables.Contains("ConsultaJob"))
            //        ConsultaJob.Tables["ConsultaJob"].Rows.Clear();

            //    ConsultaJob = new JobLN().JobConsultaTiempo();

            //    if (EjecucionJob.Tables.Contains("EjecucionJob"))
            //        EjecucionJob.Tables["EjecucionJob"].Rows.Clear();


            //    EjecucionJob = new JobLN().JobConsulta();
            //    FecHorEje = Convert.ToInt64(EjecucionJob.Tables["EjecucionJob"].Rows[0]["FechaHora"].ToString());
            //    FecHorDesEje = 0;

            //    Job = new JobLN().TareaJob();

            //    while (FecHorDesEje <= FecHorEje)
            //    {
            //        System.Threading.Thread.Sleep(Convert.ToInt32(ConsultaJob.Tables["ConsultaJob"].Rows[0]["Promedio"]));

            //        if (EjecucionJob.Tables.Contains("EjecucionJob"))
            //            EjecucionJob.Tables["EjecucionJob"].Rows.Clear();

            //        EjecucionJob = new JobLN().JobConsulta();

            //        FecHorDesEje = Convert.ToInt64(EjecucionJob.Tables["EjecucionJob"].Rows[0]["FechaHora"].ToString());
            //        EstadoDesEje = Convert.ToInt32(EjecucionJob.Tables["EjecucionJob"].Rows[0]["run_status"]);
            //    }

            //    if (EstadoDesEje != 1)
            //    {
            //        retornoMetodo.Add("Ocurrio un error al actualizar la tabla debito");
            //        return retornoMetodo;
            //    }
            //}

            #endregion

            #region CALCULO CONSECUTIVO
            HistorialArchivos objH = new HistorialArchivos();
            objH.pCodigoBanco = Convert.ToString(IdBanco);

            List<EN.Tablas.HistorialArchivos> listaH = new HistorialArchivosLN().consultarConsecutivo(objH);

            sumaletra = 0;
            ConsecutivoLog = 0;
            ConsecutivoBanc = "0";

            if (listaH.Count > 0)
            {
                objH = listaH[0];

                if ((IdBanco == 2 || IdBanco == 51) && objH.pConsecutivo != String.Empty) //Bancolombia y Bancolombia ACH (Letras)
                {
                    sumaletra = Convert.ToInt32(objH.pConsecutivo) + 1;

                    if (sumaletra == 27)
                    {
                        sumaletra = 1;
                    }

                    ConsecutivoBanc = UtilidadesWeb.homologarletra(Convert.ToString(sumaletra));
                    ConsecutivoLog = Convert.ToInt32(objH.pConsecutivo);
                    ConsecutivoLog++;

                    if (ConsecutivoLog == 27)
                    {
                        ConsecutivoLog = 1;
                        ConsecutivoBanc = "A";
                    }
                }
            }
            else
            {
                retornoMetodo.Add("Ocurrio un error al consultar el consecutivo");
                return retornoMetodo;
            }

            if (IdBanco == 51)
            {
                BancoDebita = "BANCOLOMBIA";
            }

            #endregion

            int diahoyhabil = new LeerCalendario().diahabil();

            ChevyplanLN objChevyC = new ChevyplanLN();
            Chevyplan objChevy = new Chevyplan();

            List<EN.Tablas.Chevyplan> listaC = objChevyC.consultarChevyplan(objChevy);
            if (listaC.Count > 0)
            {
                objChevy = listaC[0];
                nombreEmpresa = objChevy.pEmpresa.ToString();
                IdentificacionEmpresa = objChevy.pIdentificacion.ToString();
            }

            Banco objB = new Banco();
            objB.pId = IdBanco;
            List<EN.Tablas.Banco> listaB = new BancoLN().consultarBanco(objB);

            if (listaB.Count > 0)
            {
                objB = listaB[0];
            }

            FiduciasLN objFiduciasLN = new FiduciasLN();
            Fiducias objFiducas = new Fiducias();
            Fiducias objFiducas1 = new Fiducias();

            objFiducas1 = objFiduciasLN.consultarFiducias(objFiducas)[0];

            if (objFiducas1.pFiducia == 1)
                CuentaPrincipal = objFiducas1.pCuentaFiducia1;
            else
                CuentaPrincipal = objFiducas1.pCuentaFiducia2;

            Logs objL = new Logs();
            EnviarCorreo Correo = new EnviarCorreo();

            DataTable tablaDebitos = new DataTable();
            DataTable copiatablaDebitos = new DataTable();
            DataSet ConsultaDebitos = new DataSet();
            copiatablaDebitos = tablaDebitos.Clone();

            if (esManual == true)
                foreach (Int32 codigo in CodBancos)
                {
                    ConsultaDebitos = new ArchivoManualLN().ConsultarArchivoManual("4,8", Convert.ToString(codigo), Convert.ToDateTime(fechaDelDebito).ToString("yyMMdd"), BancoDebita);
                    copiatablaDebitos = ConsultaDebitos.Tables[0];
                    tablaDebitos.Merge(copiatablaDebitos);
                }
            else
                foreach (Int32 codigo in CodBancos)
                {
                    ConsultaDebitos = new DatosDebitoLN().ConsultaDebito("4", Convert.ToString(codigo), BancoDebita, Convert.ToDateTime(fechaDelDebito).ToString("yyMMdd"), AnoDebito + MesaDebitar, diahoyhabil, suscriptores, adjudicados, ganadores, CuotasxDevolver);
                    copiatablaDebitos = ConsultaDebitos.Tables[0];
                    tablaDebitos.Merge(copiatablaDebitos);
                }

            if (tablaDebitos.Rows.Count == 0)
            {
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = objB.pNombre + ", " + tipoArchivo + " : No se encontraron registros para procesar";
                objL.pTipoArchivo = "GDA";
                objL.pTipoProceso = "GEN";
                objL.pIdBanco = objB.pId.Value;
                new LogsLN().insertar(objL);
                retornoMetodo.Add("No se encontraron registros para procesar");
                return retornoMetodo;
            }

            PagoParcial objPag = new PagoParcial();

            foreach (DataRow row in tablaDebitos.Rows)
            {
                if (Convert.ToInt32(row["VALOR_PARCIAL"].ToString()) > 0)
                {
                    if (Convert.ToInt32(row["VALOR_CUOTA"].ToString()) >= Convert.ToInt32(row["VALOR_PARCIAL"].ToString()))
                    {
                        int insertpago = 0;

                        objPag.pContrato = row["CONTRATO"].ToString();

                        List<EN.Tablas.PagoParcial> listPP = new PagoParcialLN().consultar(objPag);

                        if (listPP.Count > 0)
                        {
                            int borrar = new PagoParcialLN().borrar(objPag);
                        }

                        objPag.pContrato = row["CONTRATO"].ToString();
                        objPag.pValorCobrado = Convert.ToInt32(row["VALOR_PARCIAL"].ToString());
                        objPag.pValorSico = Convert.ToInt32(row["VALOR_CUOTA"].ToString());
                        insertpago = new PagoParcialLN().insertar(objPag);

                        row["VALOR_CUOTA"] = row["VALOR_PARCIAL"];

                    }
                }
            }

            foreach (DataRow row in tablaDebitos.Rows)
            {
                if (row["RESPUESTA"].ToString() == "RECHAZADO")
                {
                    retornoMetodo.Add("Prenota del contrato " + row["CONTRATO"].ToString() + " fue rechazado por la entidad bancaria que esta generando: " + BancoDebita);
                    if (esManual == false)
                    {
                        row.Delete();
                    }
                }
            }

            tablaDebitos.AcceptChanges();
            tipoArchivo = "Debitos";

            String mes = DateTime.Now.ToString("MMMM").ToUpper();
            String año = DateTime.Now.Year.ToString();
            RutasLN objRutaLN = new RutasLN();
            Rutas objRutaDebito = new Rutas();
            Rutas objRutaDebitoManual = new Rutas();
            objRutaDebito.pId = objB.pIdDebito;
            objRutaDebitoManual.pId = objB.pIdDebitoManual;

            if (esManual == false)
            {
                String path = objRutaLN.consultarRuta(objRutaDebito)[0].pRuta + "\\" + año + "\\" + mes + "\\";
                Directorio = path;
            }
            else
            {
                String path = objRutaLN.consultarRuta(objRutaDebitoManual)[0].pRuta + "\\" + año + "\\" + mes + "\\";
                Directorio = path;
            }

            //OBTENER LOS CORREOS DE CONTROL DEL PROCESO Y LOS DESTINATARIOS
            string[] CorreoControl = objB.pCorreoControl.Split(';');
            ArrayList correoC = new ArrayList();
            correoC.AddRange(CorreoControl);
            string[] CorreoEnvio = objB.pCorreoEnvio.Split(';');
            ArrayList correoE = new ArrayList();
            correoE.AddRange(CorreoEnvio);

            //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE ASOBANCARIA SI NO EXISTE
            if (!Directory.Exists(Directorio))
            {
                System.IO.Directory.CreateDirectory(Directorio);
            }

            #region CREAR COLUMNAS DE LA TABLA CON LOS DATOS PARA GUARDAR LAS LINEAS EN UNA BD
            LineasArmadasdt.Columns.Add("fecha");
            LineasArmadasdt.Columns.Add("codigoBanco");
            LineasArmadasdt.Columns.Add("consecutivo");
            LineasArmadasdt.Columns.Add("lineaArmada");
            #endregion

            try
            {
                escribirArchivo(objB, tablaDebitos, Convert.ToDateTime(fechaDelDebito), esManual);


                foreach (DataRow row in tablaDebitos.Rows)
                {
                    guardarLineas(fechaDelDebito, Convert.ToString(IdBanco), row["CONTRATO"].ToString(), row["VALOR_CUOTA"].ToString(), Convert.ToInt32(row["IDCLIENTE"].ToString()), nombreArchivo,
                                    ConsecutivoLog, esManual);
                }

                LineasArmadasdt.Clear();

                int actualizaciondebitos = 0;
                ArchivoManual objEntidad = new ArchivoManual();
                int manualA = 0;

                foreach (DataRow renglon in tablaDebitos.Rows)
                {
                    DatosDebito objTC = new DatosDebito();
                    objTC.pContrato = renglon[1].ToString();//CONTRATO
                    objTC.pEstado = 5;
                    int val = new DatosDebitoLN().actualizarEstado(objTC);
                    if (val <= 0)
                        actualizaciondebitos += 1;

                    if (esManual == true)
                    {
                        objEntidad.pContrato = Convert.ToInt32(renglon[1].ToString());
                        objEntidad.pAutorizar = true;
                        manualA = new ArchivoManualLN().actualizar(objEntidad);
                    }

                }

                if (actualizaciondebitos > 0)
                    throw new System.Exception("Ocurrio un error al actualizar el estado del cliente de debito a debito en proceso");

                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = objB.pNombre + ", " + tipoArchivo + " : Archivo fue generado correctamente";
                objL.pTipoArchivo = "GDA";
                objL.pTipoProceso = "GEN";
                objL.pIdBanco = objB.pId.Value;
                new LogsLN().insertar(objL);
                retornoMetodo.Add("Archivo de " + tipoArchivo + " fue generado correctamente con un total de " + tablaDebitos.Rows.Count + " registros.");
                return retornoMetodo;
            }
            catch (Exception ex)
            {
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = objB.pNombre + ", " + tipoArchivo + " : " + ex.Message;
                objL.pTipoArchivo = "GDA";
                objL.pTipoProceso = "GEN";
                objL.pIdBanco = objB.pId.Value;
                new LogsLN().insertar(objL);
                retornoMetodo.Add(ex.Message);
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

        //Metodo retornar formato documentos para el BBVA
        public string convertir_tipo_documento_BBVA(string codigo)
        {

            if (codigo.Equals("CC"))
            {
                return "01";
            }
            else
            {
                if (codigo.Equals("CE"))
                {
                    return "02";
                }
                else
                {
                    if (codigo.Equals("TI"))
                    {
                        return "04";
                    }
                    else
                    {
                        if (codigo.Equals("NIT"))
                        {
                            return "03";
                        }
                        else
                        {
                            if (codigo.Equals("PA"))
                            {
                                return "05";
                            }
                            else
                            {

                                return "00";
                            }
                        }
                    }
                }
            }

        }

        //Metodo Renornar formato BBVA
        public string digito_verificacion_BBVA(string digito, string identificacion)
        {
            string newidentificacion;

            if (digito.Equals("03"))
            {

                string digito_verificacion = "";
                LectorArchivos obj = new LectorArchivos();
                digito_verificacion = obj.Digito_Verificacion(identificacion);
                newidentificacion = identificacion + digito_verificacion;
               // newidentificacion = identificacion + "0";
            }
            else
            {
                newidentificacion = identificacion + "0";
            }

            return newidentificacion;
        }
        //Crea la cuenta de 16 digitos del BBVA y valida que sea del BBVA
        public string num_cuenta_BBVA(string banco, string ncuenta, string tipocuenta)
        {
            string newcuenta;

            if (banco.Equals("013"))
            {
                if (ncuenta.Length == 9)
                {
                    if (tipocuenta.Equals("AHORROS"))
                    {
                        newcuenta = "0" + ncuenta.Substring(0, 3) + "000200" + ncuenta.Substring(3, 6);
                    }
                    else
                    {
                        newcuenta = "0" + ncuenta.Substring(0, 3) + "000100" + ncuenta.Substring(3, 6);
                    }
                }
                else
                {
                    newcuenta = ncuenta;
                }
            }
            else
            {
                newcuenta = "";
            }

            return newcuenta;
        }

        //valida que sea de diferente a BBVA en prenota
        public string num_cuenta_client_BBVA(string banco, string ncuenta)
        {
            string newcuenta;
            if (banco.Equals("013"))
            {
                newcuenta = "";
            }
            else
            {
                newcuenta = ncuenta;
            }

            return newcuenta;
        }

        //Valida si el tipo de cuenta es del BBVA y cambia el codigo
        public string tipo_cuenta(string tipocuenta, string banco)
        {
            string newtipocuenta;
            if (banco.Equals("013"))
            {
                newtipocuenta = "00";
            }
            else
            {
                if (tipocuenta.Equals("AHORROS"))
                {

                    newtipocuenta = "02";
                }
                else
                {
                    newtipocuenta = "01";
                }
            }
            return newtipocuenta;

        }



        //SE CREA EL ARCHIVO ASOBANCARIA
        private void escribirArchivo(Banco objBanco, DataTable tabla, DateTime fechaDelDebito, bool Manual)
        {
            try
            {
                ListaLinea1EA = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
                ListaLinea2EL = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
                ListaLinea3DT = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
                ListaLinea4CL = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();
                ListaLinea5CA = new List<DebitoAutomatico.EN.Tablas.EstructuraArchivo>();

                ListaLinea1EA.AddRange(consultarEstructura("1EA", "GDA", Convert.ToInt32(objBanco.pId)));
                ListaLinea2EL.AddRange(consultarEstructura("2EL", "GDA", Convert.ToInt32(objBanco.pId)));
                ListaLinea3DT.AddRange(consultarEstructura("3DT", "GDA", Convert.ToInt32(objBanco.pId)));
                ListaLinea4CL.AddRange(consultarEstructura("4CL", "GDA", Convert.ToInt32(objBanco.pId)));
                ListaLinea5CA.AddRange(consultarEstructura("5CA", "GDA", Convert.ToInt32(objBanco.pId)));

                tablaCE = new CamposEquivalenciasLN().consultarCampoEquivalencias("GDA", objBanco.pId.ToString());
                tablaE = new EquivalenciasLN().consultarEquivalenciasXTipoArchivo("GDA");

                //EN ESTA DATATABLE SE CARGAN TODOS LOS CAMPOS DE LA ESTRUCTURA DEL ARCHIVO QUE SON DE EXTRACTO_ENCABEZADO
                DataTable tablaEA = new EstructuraArchivoLN().consultarEstructuraArchivo("GDA", objBanco.pId.Value);
                //EN ESTE DATAROW SE CARGAN TODOS LOS CAMPOS QUE VAN A HACER SUMADOS.
                DataRow[] IdSeSuma = tablaEA.Select("ES_CONTADOR = 1");

                if (IdSeSuma.Length != 0)
                {
                    //SE LLENA UN ARRAYLIST CON TODOS LOS ID'S DE LOS CAMPOS QUE SE VAN A CONTAR
                    foreach (DataRow fila in IdSeSuma)
                    {
                        idASumar.Add(fila[11].ToString());
                    }
                }

                //SE CREA EL NOMBRE DEL ARCHIVO SEGUN LOS PARAMETROS 
                if (Manual)
                    nombreArchivo = String.Concat(objBanco.pCodigo, "_GDA_MANUAL_", fechaDelDebito.ToString("ddMMyyyy"),
                                                     "_", writeMilitaryTime(DateTime.Now), ".txt");
                else
                    nombreArchivo = String.Concat(objBanco.pCodigo, "_GDA_", fechaDelDebito.ToString("ddMMyyyy"),
                                                 "_", writeMilitaryTime(DateTime.Now), ".txt");

                sw = new StreamWriter(Directorio + nombreArchivo, false, Encoding.GetEncoding(1252));
                
                FechaArchivo = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                FechaDebito = Convert.ToString(fechaDelDebito.ToString("dd/MM/yyyy"));
                //FechaDiaDespues = Convert.ToString(fechaDelDebito.AddDays(1).ToString("dd/MM/yyyy"));

                //SE ADICIONA UN CONSECUTIVO SEGUN EL NUMERO DE ARCHIVO QUE EXISTAN DEL MISMO DIA                
                int numColumnas = 0;
                HistorialArchivosLN columnas = new HistorialArchivosLN();
                numColumnas = columnas.consultarConsecutivoXBanco(objBanco.pCodigo, "GDA", FechaArchivo).Rows.Count;


                //if (numColumnas == 0)
                //{
                //    if (objBanco.pCodigo == "007" || objBanco.pCodigo == "07")//ESTE CODIGO QUEDA QUEMADO PORQUE BANCOLOMBIA UTILIZA CONSECUTIVO CON LETRAS
                //        ConsecutivoArchivo = "A";
                //    else if (objBanco.pCodigo == "023")//ESTE CODIGO QUEDA QUEMADO PORQUE OCCIDENTE UTILIZA CONSECUTIVO CON NUMEROS
                //        ConsecutivoArchivo = "1";
                //}
                //else
                //{
                //    if (objBanco.pCodigo == "007" || objBanco.pCodigo == "07")//ESTE CODIGO QUEDA QUEMADO PORQUE BANCOLOMBIA UTILIZA CONSECUTIVO CON LETRAS
                //        ConsecutivoArchivo = consecutivo(numColumnas);
                //    else if (objBanco.pCodigo == "023")//ESTE CODIGO QUEDA QUEMADO PORQUE OCCIDENTE UTILIZA CONSECUTIVO CON NUMEROS
                //        ConsecutivoArchivo = (numColumnas + 1).ToString();
                //}

                ArrayList line1EA = new ArrayList() { "1EA", FechaDebito, CuentaPrincipal };
                //ArrayList line2EL = new ArrayList() { "2EL" };
                ArrayList line3DT = new ArrayList();
                //ArrayList line4CL = new ArrayList() { "4CL" };
                ArrayList line5CA = new ArrayList() { "5CA" };

                ArrayList LineaDetalle = new ArrayList();
                #region Armar Linea Detalle
                if (ListaLinea3DT.Count > 0)
                {
                    foreach (DataRow renglon in tabla.Rows)
                    {
                        line3DT.Add("3DT");
                        line3DT.Add(CuentaPrincipal);//Cuenta Fiducia
                        line3DT.Add(FechaDebito);//Fecha Debito
                        line3DT.Add(objBanco.pCodigo == "023" ? renglon[5].ToString() == "NIT" ? "A" : renglon[5].ToString() : renglon[5].ToString());//Tipo de Identificacion
                        line3DT.Add(renglon[6].ToString());//N° Identificacion
                        line3DT.Add(renglon[7].ToString());//Nombre
                        line3DT.Add(renglon[10].ToString());//Codigo Banco
                        line3DT.Add(renglon[2].ToString());//Tipo Cuenta
                        line3DT.Add(renglon[3].ToString());//N° Cuenta Cliente
                        line3DT.Add(renglon[1].ToString());//Contrato
                        line3DT.Add(renglon[13].ToString());//Valor
                        line3DT.Add(FechaArchivo);//Fecha Prenota
                        if (objBanco.pCodigo == "013")
                        {
                            line3DT.Add(renglon[14].ToString());//Dirección BBVA
                            line3DT.Add(renglon[15].ToString());//Email BBVA
                            line3DT.Add(FechaDebito.Substring(6, 4));//año BBVA
                            line3DT.Add(FechaDebito.Substring(3, 2));//mes BBVA
                            line3DT.Add(FechaDebito.Substring(0, 2));//dia BBVA
                            line3DT.Add(convertir_tipo_documento_BBVA(renglon[5].ToString()));//Tipo Cuenta numero BBVA
                            line3DT.Add(digito_verificacion_BBVA(convertir_tipo_documento_BBVA(renglon[5].ToString()), renglon[6].ToString()));//identificacion con digito BBVA
                            line3DT.Add(num_cuenta_BBVA(renglon[10].ToString(), renglon[3].ToString(), renglon[2].ToString()));//Número de Cuenta BBVA Prenota
                            line3DT.Add(num_cuenta_client_BBVA(renglon[10].ToString(), renglon[3].ToString()));//Número de cuenta Nacham BBVA Prenota
                            line3DT.Add(tipo_cuenta(renglon[2].ToString(), renglon[10].ToString()));//Tipo de cuenta BBVA Prenota
                        }
                        line3DT.Add(FechaArchivo);

                        LineaArmada3DT = armarLineas(line3DT, ListaLinea3DT);
                        LineaDetalle.Add(LineaArmada3DT);
                        //sw.WriteLine(LineaArmada3DT);
                        //llenarTablaParaActualizados(Fecha, objBanco.pCodigo, ConsecutivoArchivo, LineaArmada3DT);
                        line3DT.Clear();
                        registrosLote += 1;
                        LineaArmada3DT = String.Empty;

                    }
                }
                #endregion

                ArrayList LineaControlArchivo = new ArrayList();
                #region Armar Linea Control de Archivo
                if (ListaLinea5CA.Count > 0)
                {
                    line5CA.Add(registrosLote + 2);
                    line5CA.Add(registrosLote);
                    LineaArmada5CA = armarLineas(line5CA, ListaLinea5CA);
                    LineaControlArchivo.Add(LineaArmada5CA);
                    //sw.WriteLine(LineaArmada5CA);
                    //llenarTablaParaActualizados(Fecha, objBanco.pCodigo, ConsecutivoArchivo, LineaArmada5CA);
                    registrosLote += 1;
                }
                #endregion

                ArrayList LineaEncabezadoArchivo = new ArrayList();
                #region Armar Linea Encabezado de Archivo
                if (ListaLinea1EA.Count > 0)
                {
                    line1EA.Add(IdentificacionEmpresa);
                    line1EA.Add(nombreEmpresa);
                    line1EA.Add(FechaArchivo);
                    line1EA.Add(registrosLote);
                    line1EA.Add(ConsecutivoBanc);
                    LineaArmada1EA = armarLineas(line1EA, ListaLinea1EA);
                    LineaEncabezadoArchivo.Add(LineaArmada1EA);
                    //sw.WriteLine(LineaArmada1EA);
                    //llenarTablaParaActualizados(Fecha, objBanco.pCodigo, ConsecutivoArchivo, LineaArmada1EA);
                    registrosLote += 1;
                }
                #endregion

                #region GUARDAR LINEAS EN BD Y ARCHIVO PLANO
                if (ListaLinea1EA.Count > 0)
                {
                    sw.WriteLine(LineaEncabezadoArchivo[0].ToString());
                    llenarTablaParaActualizados(FechaArchivo, objBanco.pCodigo, ConsecutivoArchivo, LineaEncabezadoArchivo[0].ToString());
                }
                if (ListaLinea3DT.Count > 0)
                {
                    foreach (String linea in LineaDetalle)
                    {
                        sw.WriteLine(linea);
                        llenarTablaParaActualizados(FechaArchivo, objBanco.pCodigo, ConsecutivoArchivo, linea);
                    }
                }
                if (ListaLinea5CA.Count > 0)
                {
                    sw.WriteLine(LineaControlArchivo[0].ToString());
                    llenarTablaParaActualizados(FechaArchivo, objBanco.pCodigo, ConsecutivoArchivo, LineaControlArchivo[0].ToString());
                }
                sw.Close();
                #endregion

            }
            catch
            {
                sw.Close();
                File.Delete(Directorio + nombreArchivo);
                throw new System.Exception("Ocurrio un error al crear archivo plano");
            }

        }

        private void guardarLineas(String fecha, String CodigoBanco, String Contrato, String Valor, int IdCliente, String NombreArchivo, int Consecutivo, bool Manual)
        {

            int valor = 0;
            HistorialArchivos objEntidad = new HistorialArchivos();
            objEntidad.pFecha = fecha;
            objEntidad.pCodigoBanco = CodigoBanco;
            objEntidad.pTipoArchivo = "GDA";
            objEntidad.pContrato = Contrato;
            objEntidad.pIdCliente = Convert.ToString(IdCliente);
            objEntidad.pValor = Valor;
            objEntidad.pNombreArchivo = NombreArchivo;
            objEntidad.pManual = Manual;
            objEntidad.pEstado = "V";
            objEntidad.pUsuarioModifico = String.Empty;
            objEntidad.pConsecutivo = Convert.ToString(Consecutivo);

            valor = new HistorialArchivosLN().insertar(objEntidad);
            if (valor <= 0)
            {
                throw new System.Exception("Ocurrio un error al guardar archivo plano en la base de datos");
            }
        }

        // SE CARGA LOS PARAMETROS DE LA ESTRUCTURA DE LA LINEAS PARA GENERAR EL ARCHIVO PRENOTA
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

        //SE ARMA CADA LINEA DE PRENOTA SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineas(ArrayList lineaDatos, List<DebitoAutomatico.EN.Tablas.EstructuraArchivo> lineasAsobancaria)
        {

            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;
            foreach (DebitoAutomatico.EN.Tablas.EstructuraArchivo objEst in lineasAsobancaria)
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
                            if (objEst.pNombre.Equals("Fecha"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[1])));
                            else if (objEst.pNombre.Equals("Cta principal"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[2])));
                            else if (objEst.pNombre.Equals("NIT Entidad Recaudadora"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[3])));
                            else if (objEst.pNombre.Equals("Nombre de Entidad Recaudadora"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[4])));
                            else if (objEst.pNombre.Equals("Fecha de Aplicación o Vencimiento"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[1])));
                            else if (objEst.pNombre.Equals("Fecha de Transmision"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[5])));
                            else if (objEst.pNombre.Equals("Número de Registros"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[6])));
                            else if (objEst.pNombre.Equals("Valor total de las transacciones"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            else if (objEst.pNombre.Equals("Secuencia de Envío"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[7])));
                            break;
                        case "2EL":
                            break;
                        case "3DT":
                            if (objEst.pNombre.Equals("Cuenta Recaudadora"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[1])));
                            else if (objEst.pNombre.Equals("Fecha"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[2])));
                            else if (objEst.pNombre.Equals("Tipo Identificación"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[3])));
                            else if (objEst.pNombre.Equals("NIT"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[4])));
                            else if (objEst.pNombre.Equals("Nombre del Usuario Pagador"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[5])));
                            else if (objEst.pNombre.Equals("Banco"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[6])));
                            else if (objEst.pNombre.Equals("Tipo de Cuenta del Usuario Pagador"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[7])));
                            else if (objEst.pNombre.Equals("Cuenta Usuario Pagador"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[8])));
                            else if (objEst.pNombre.Equals("Comprobante"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[9])));
                            else if (objEst.pNombre.Equals("Código del Servicio"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[9])));
                            else if (objEst.pNombre.Equals("Valor"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[10])));
                            else if (objEst.pNombre.Equals("Banco Cuenta del Pagador"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[6])));
                            else if (objEst.pNombre.Equals("Tipo de Transacción"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[7])));
                            else if (objEst.pNombre.Equals("Fecha de Aplicación o Vencimiento"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[2])));
                            else if (objEst.pNombre.Equals("Dirección No 1"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[12])));
                            else if (objEst.pNombre.Equals("E-mail"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[13])));
                            else if (objEst.pNombre.Equals("Tipo_identificacion"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[17])));
                            else if (objEst.pNombre.Equals("NIT_Digito"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[18])));
                            else if (objEst.pNombre.Equals("Número de Cuenta BBVA"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[19])));
                            else if (objEst.pNombre.Equals("Número de cuenta Nacham"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[20])));
                            else if (objEst.pNombre.Equals("Tipo de cuenta Nacham"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[21])));
                            else if (objEst.pNombre.Equals("Concepto 1"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[9])));
                            else if (objEst.pNombre.Equals("Vr operación parte entera"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[10]))); 
                            break;
                        case "4CL":
                            break;
                        case "5CA":
                            if (objEst.pNombre.Equals("Nro de registros"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[1])));
                            else if (objEst.pNombre.Equals("Nro débitos"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, Convert.ToString(lineaDatos[2])));
                            else if (objEst.pNombre.Equals("Valor Débitos"))
                                valor = armarCampo(objEst, evaluarCampo(objEst, String.Empty));
                            break;
                    }
                    linea += rellenarCampo(valor, objEst.pAlineacion, Convert.ToInt32(objEst.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE RELLENAN LOS CAMPOS DEL ARCHIVO ASOBANCARIA CON CEROS O ESPACIOS SEGUN EL PARAMETRO 
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

        //SE CONVIERTE UNA HORA NORMAL A HORA MILITAR
        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
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
        //SE CALCULA EL CONSECUTIVO DE UN ARCHIVO ASOBANCARIA QUE FUE GENEREADO MAS DE UNA VEZ EN EL MISMO DIA
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

        private String convertirNumero(String numero, int decimales)
        {

            String[] numeros = numero.Split(',');
            if (!numero.Contains(","))
                return (Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales)).ToString();
            else
                return Convert.ToInt64((Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales - numeros[1].Length))).ToString();

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
            return String.Empty;

        }



    }
}
