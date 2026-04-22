package corebancario_bancoabc; // package
import DataLayer.Bitacora;
import DataLayer.SQLConnection; 

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.List;

public class LogicaCore { // Inicia Clase
    
    SQLConnection sqlConnection = new SQLConnection();
    Bitacora bitacora;
    
    public String logicaCore(HashMap<String, String> listaFinal){
        
        String Respuesta = "";
            
        Respuesta = ElegirPorTipoTransaccion(listaFinal);
        
        return Respuesta;
    }
    
    private String ElegirPorTipoTransaccion(HashMap<String, String> llaveValor){ 
        
        // Validar, guardar, y llenar bitacora al mismo tiempo,  responder 
        
        String Respuesta = "";
        
        try{
        
            if("Retiro".equals(llaveValor.get("TipoDeTransaccion"))){
                    
                // Este select ya hace validación por si solo
                List<Object[]> datos = sqlConnection.selectAll(
                    "SELECT CUENTA_MontoDisponible "
                    + "FROM CUENTAS_TB "
                    + "WHERE CUENTA_ID = ? "
                    + "AND CUENTA_Tarjeta = ? "
                    + "AND CUENTA_Estado = 1;",
                    Integer.parseInt(llaveValor.get("NumeroCuenta")),
                    llaveValor.get("NumeroTarjeta")
                );

                if (datos.isEmpty()){

                    throw new Exception();

                } else {

                    // Validación de Dinero suficiente
                    BigDecimal montoSolicitado = new BigDecimal(llaveValor.get("Monto"));
                    BigDecimal saldoCuenta = (BigDecimal) datos.get(0)[0];
                    boolean montoValido = ValidarMonto(montoSolicitado, saldoCuenta);

                    if(montoValido){

                        Respuesta = "{\"status\": \"OK\"}";
                        bitacora = new Bitacora(llaveValor, "OK");
                    } else {

                        Respuesta = "{\"status\": \"INSUF\"}";
                        bitacora = new Bitacora(llaveValor, "INSUF");
                    }
                    
                    bitacora.start();
                }

            } else if ("Consulta".equals(llaveValor.get("TipoDeTransaccion"))){
                
                // Este select ya hace validación por si solo
                List<Object[]> datos = sqlConnection.selectAll(
                    "SELECT CUENTA_MontoDisponible "
                    + "FROM CUENTAS_TB "
                    + "WHERE CUENTA_ID = ? "
                    + "AND CUENTA_Tarjeta = ? "
                    + "AND CUENTA_Estado = 1;",
                    Integer.parseInt(llaveValor.get("NumeroCuenta")),
                    llaveValor.get("NumeroTarjeta")
                );

                if (datos.isEmpty()){

                    throw new Exception();

                } else { 

                    BigDecimal saldoCuenta = (BigDecimal) datos.get(0)[0];

                    // Asegura 2 decimales exactos sin redondeos inesperados
                    saldoCuenta = saldoCuenta.setScale(2);

                    // Formatea: 16 enteros + '.' + 2 decimales = 19 caracteres totales
                    String saldoFormateado = String.format("%019.2f", saldoCuenta);

                    Respuesta = "{\"status\": \"OK\", \"saldo\": \"" + saldoFormateado + "\"}";
                    
                    bitacora = new Bitacora(llaveValor, "OK");
                    bitacora.start();
                }
                
            }  else if ("Confirmacion".equals(llaveValor.get("TipoDeTransaccion"))){
                
                boolean ok = sqlConnection.executeSP(
                    "SP_CONFIRMACION_DE_RETIRO",
                    llaveValor.get("TipoDeTransaccion"),
                    Integer.parseInt(llaveValor.get("NumeroCuenta")),
                    llaveValor.get("NumeroTarjeta"),
                    new BigDecimal(llaveValor.get("Monto")),
                    llaveValor.get("CodigoAutorizacion")
                ); 

                if (!ok) {
                    
                    throw new Exception();
                    
                } else {

                    Respuesta = "{\"status\": \"OK\"}";
                    bitacora = new Bitacora(llaveValor, "OK");
                    bitacora.start();
                }
                
            //-------------------------------------------------------------------------------------------
            
            } else if ("ObtenerCuentas".equals(llaveValor.get("TipoDeTransaccion"))) {

                String identificacion = llaveValor.get("Identificacion");
                List<Object[]> cuentas = sqlConnection.selectAll(
                    "SELECT CUENTA_ID, CUENTA_MontoDisponible " +
                    "FROM CUENTAS_TB " +
                    "WHERE CLIENTE_ID = (SELECT CLIENTE_ID FROM CLIENTES_TB WHERE CLIENTE_Identificacion = ?)",
                    identificacion
                );

                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.append("{\"status\":\"OK\", \"cuentas\":[");
                for (int i = 0; i < cuentas.size(); i++) {
                    Object[] row = cuentas.get(i);
                    jsonBuilder.append("{")
                               .append("\"NumeroCuenta\":\"").append(row[0].toString()).append("\",")
                               .append("\"Saldo\":").append(row[1])
                               .append("}");
                    if (i < cuentas.size() - 1) jsonBuilder.append(",");
                }
                jsonBuilder.append("]}");
                Respuesta = jsonBuilder.toString();

            } else if ("ObtenerMovimientosCuenta".equals(llaveValor.get("TipoDeTransaccion"))) {

                String identificacion = llaveValor.get("Identificacion");
                String numeroCuenta = llaveValor.get("NumeroCuenta");
                List<Object[]> movimientos = sqlConnection.selectAll(
                    "SELECT t.TRAN_FechaHora, " +
                    "CASE tt.TIPO_TRAN_Nombre " +
                    "   WHEN 'Retiro' THEN 'Retiro en cajero' " +
                    "   ELSE tt.TIPO_TRAN_Nombre " +
                    "END AS Descripcion, " +
                    "t.TRAN_MontoRetiro AS Monto " +
                    "FROM TRANSACCIONES_TB t " +
                    "INNER JOIN CUENTAS_TB c ON t.TRAN_CUENTA_ID = c.CUENTA_ID " +
                    "INNER JOIN CLIENTES_TB cl ON c.CLIENTE_ID = cl.CLIENTE_ID " +
                    "INNER JOIN TIPOS_TRANSACCIONES_TB tt ON t.TRAN_TIPO_TRAN_ID = tt.TIPO_TRAN_ID " +
                    "WHERE cl.CLIENTE_Identificacion = ? AND c.CUENTA_ID = ? " +
                    "ORDER BY t.TRAN_FechaHora DESC",
                    identificacion, numeroCuenta
                );

                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.append("{\"status\":\"OK\", \"movimientos\":[");
                for (int i = 0; i < movimientos.size(); i++) {
                    Object[] row = movimientos.get(i);
                    jsonBuilder.append("{")
                               .append("\"Fecha\":\"").append(row[0].toString()).append("\",")
                               .append("\"Descripcion\":\"").append(row[1].toString()).append("\",")
                               .append("\"Monto\":").append(row[2])
                               .append("}");
                    if (i < movimientos.size() - 1) jsonBuilder.append(",");
                }
                
                jsonBuilder.append("]}");
                Respuesta = jsonBuilder.toString();
            
            //-------------------------------------------------------------------------------------------
            
            } else{

                throw new Exception();
            }

        } catch (Exception e){
            
            Respuesta = "{\"status\": \"ERROR\"}";
            bitacora = new Bitacora(llaveValor, "ERROR");
            bitacora.start();
        }
        
        return Respuesta;
    }
    
    private boolean ValidarMonto(BigDecimal AutDato, BigDecimal sqlDato){
        
        return AutDato.compareTo(sqlDato) <= 0;
    }
    
} // Termina Clase
