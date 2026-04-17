package DataLayer; // package

import java.time.LocalDate;
import java.util.HashMap;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;


/* META:
    2026-02-18: {
        "Tarjeta": xxxxxx******xxxx,
        "Cuenta": x,
        "CodigoDeAutorizacion": xxxxxxxx,
        "TipoTransaccion": "xxxx",
        "Monto": 0000000000
    }
*/

// Hilo
public class Bitacora extends Thread { // Inicia Clase
    
    private static final String APERTURA = "{";
    private static final String CIERRE = "}";
    private HashMap<String, String> llaveValor;
    private String Respuesta;
    private static final Object LOCK = new Object();
    
    public Bitacora(HashMap<String, String> llaveValor, String Respuesta){
        
        this.llaveValor = llaveValor;
        this.Respuesta = Respuesta;
    }
    
    public String convertirAJson(){
        
        StringBuilder json = new StringBuilder();
        LocalDate hoy = LocalDate.now();
        
        json.append("\"").append(hoy.toString()).append("\"");
        json.append(":");
        json.append(APERTURA);
        json.append("\n");
        json.append(
                crearFormatoJson("NumeroTarjeta", this.llaveValor.get("NumeroTarjeta"))
        );
        json.append(
                crearFormatoJson("Cuenta", this.llaveValor.get("NumeroCuenta"))
        );
        json.append(
                crearFormatoJson("TipoDeTransaccion", this.llaveValor.get("TipoDeTransaccion"))
        );
        json.append(
                crearFormatoJson("Resultado", this.Respuesta)
        );
        
        String TipoTransaccion = this.llaveValor.get("TipoDeTransaccion");
        if(TipoTransaccion.equals("Retiro") || TipoTransaccion.equals("Confirmacion")){
            json.append(
                crearFormatoJson("Monto", this.llaveValor.get("Monto"))
            );
        }
        
        // eliminar última coma y salto de línea
        int length = json.length();
        if (length >= 2) {
            json.delete(length - 2, length);
            json.append("\n");
        }

        json.append(CIERRE);
        json.append("\n");
        
        return json.toString();
    }
    
    private String crearFormatoJson(String nombre, String valor){
        
        return String.format("\"%s\": \"%s\",\n", nombre, valor);
    }
    
    private void escribirEnArchivo(String contenido) {

        Path ruta = Path.of("Bitacora.json");

        synchronized (LOCK) {
            try {
                
                Files.writeString(
                        ruta,
                        contenido,
                        StandardOpenOption.CREATE,
                        StandardOpenOption.APPEND
                );
                
            } catch (IOException e) {
                
                e.printStackTrace();
            }
        }
    }
    
    @Override
    public void run(){
        
        escribirEnArchivo(convertirAJson());
    }
    
} // Termina Clase
