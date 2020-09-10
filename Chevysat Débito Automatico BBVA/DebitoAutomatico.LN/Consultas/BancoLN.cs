using System;
using System.Collections.Generic;
using System.Text;
using System.Data; 

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class BancoLN
    {
        public String Error { get; set; }
      
        public List<Banco> consultarBanco(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            BancoAD objConsultor = new BancoAD();
            List<Banco> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<Banco> consultarBancoid(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            BancoAD objConsultor = new BancoAD();
            List<Banco> lista = objConsultor.consultaridbanco(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            BancoAD objConsultor = new BancoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            BancoAD objConsultor = new BancoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            BancoAD objConsultor = new BancoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultar()
        {
            return new BancoAD().consultar();
        }

        public DataTable consultarBancoFiduciaAct()
        {
            return new BancoAD().consultarBancoFiduciaAct();
        }

        public DataSet consultarLupa(Banco objEntidad)
        {
            return new BancoAD().consultarLugarPago(objEntidad);
        }

        /// <summary>
        /// Permite la consulta de los bancos existentes en la base de datos, que esten configurados para realizar debito automatico.
        /// </summary>
        /// <returns>Lista de datos</returns>
        //public List<Banco> consultarBancosDebitadores()
        //{
        //    Banco objBanco = new Banco();
        //    objBanco.pOperacion = TiposConsultas.CONSULTA_COMPUESTA;
        //    BancoAD objConsultor = new BancoAD();
        //    List<Banco> lista = objConsultor.consultarBancos(objBanco);
        //    Error = objConsultor.Error;
        //    return lista;
        //}

        //public List<Banco> consultarBanco(String banco)
        //{
        //    Banco objBanco = new Banco();
        //    objBanco.pCodigo = banco;
        //    objBanco.pOperacion = TiposConsultas.CONSULTAR;
        //    BancoAD objConsultor = new BancoAD();
        //    List<Banco> lista = objConsultor.consultarBancos(objBanco);
        //    Error = objConsultor.Error;
        //    return lista;
        //}


    }
}
