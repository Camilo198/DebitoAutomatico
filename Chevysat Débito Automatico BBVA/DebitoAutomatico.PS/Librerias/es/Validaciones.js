var _formatoMoneda = "#,##0.##";
var _formatoEntero = "#,##0";
var _formatoFecha = 'dd/mm/yyyy';
var _contenedor = '#Form1';

 
var tiempoInactividad = 600;//Tiempo inactividad sesion en segundos
_fechaActual = new Date();
//Calcula inatividad y muestra mensaje
function inactividadR() {
    if (parseInt((new Date() - _fechaActual) / 1000) > tiempoInactividad) {
        $.ajax({
            type: 'POST',
            async: true,
            url: "../Servicios/Service.svc/CerrarSession",
            success: function (data) {
                if (data) {
                    console.log(data);                    
                    $('.wrapper').css({ 'display': 'block', opacity: 0 });
                    $.alerts.alert("No hemos detectado actividad en los &uacuteltimos " + tiempoInactividad / 60 + " minutos.<br/>Por su seguridad su sesi&oacuten se ha cerrado.<br/>Si desea ingresar de nuevo, por favor haga clic en aceptar e ingrese nuevamente su usuario y contrase&ntildea.", "Sico Web", function () { location.href = "https://www.chevynet.com.co:8004/Login.aspx" })

                }
            }
        });
    }
}

