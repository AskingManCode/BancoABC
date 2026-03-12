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
