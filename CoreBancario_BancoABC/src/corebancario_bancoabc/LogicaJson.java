package corebancario_bancoabc; // package

import java.util.ArrayList;
import java.util.HashMap;

public class LogicaJson { // Inicia Clase
    
    public HashMap<String, String> logicaJSON(String MensajeEntrada){

        return this.ProcesarJSON(MensajeEntrada);
    }
    
    private HashMap<String, String> ProcesarJSON(String MensajeEntrada) {
        
        HashMap<String, String> DatosFormatoJson = new HashMap<>();
        String mensajeJson = MensajeEntrada.substring(1, MensajeEntrada.length() - 1);

        String[] mensajeSeparado = mensajeJson.split(",");

        for (String parDatos : mensajeSeparado) {

            String[] llaveValor = parDatos.split(":");

            String llave = llaveValor[0].trim().replace("\"", "");
            String valor = llaveValor[1].trim().replace("\"", "");

            DatosFormatoJson.put(llave, valor);
        }

        return DatosFormatoJson;
    }
    
} // Termina Clase
