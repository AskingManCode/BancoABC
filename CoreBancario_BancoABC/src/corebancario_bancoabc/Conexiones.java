package corebancario_bancoabc; // package

import DataLayer.config;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;

class Conexiones { // Inicia Clase
    
    private final String HOST = config.getHost();
    private final int PORT = config.getPort();

    private final LogicaJson logicaJson = new LogicaJson();
    private final LogicaCore logicaCore = new LogicaCore();
    
    public void AbrirConexion() {

        try (ServerSocket serverSocket = new ServerSocket(PORT)) {

            System.out.println("Servidor Core escuchando en: " + HOST + ":" + PORT);

            do {
                Socket cliente = serverSocket.accept();
                
                new Thread(() -> {
                    try (
                            cliente;

                            BufferedReader reader = new BufferedReader(
                                new InputStreamReader(cliente.getInputStream(), StandardCharsets.UTF_8));

                            BufferedWriter writer = new BufferedWriter(
                                new OutputStreamWriter(cliente.getOutputStream(), StandardCharsets.UTF_8));
                        ){
                        
                        String linea;
                        while((linea = reader.readLine()) != null){
                            
                            System.out.println("Texto Recibido: " + linea);
                            
                            HashMap<String, String> JsonFinal =
                                    logicaJson.logicaJSON(linea);
                            
                            String resultado = 
                                    logicaCore.logicaCore(JsonFinal);
                            
                            writer.write(resultado);
                            writer.newLine();
                            writer.flush();
                        }
                        
                        cliente.close();
                        
                    } catch (SocketException e){
                        
                        System.out.println("Cliente desconectado.");
                        
                    } catch (IOException e) {
                        
                        e.printStackTrace();
                    }
                    
                }).start();
                
            } while (true);

        } catch (IOException e) {
            System.out.println("Servidor desconectado.");
            e.printStackTrace();
        }
    }
} // Termina Clase
