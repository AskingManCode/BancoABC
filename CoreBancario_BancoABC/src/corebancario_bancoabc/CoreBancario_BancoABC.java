package corebancario_bancoabc; // package

// HolaXDD
public class CoreBancario_BancoABC { // Inicia Clase

    public static void main(String[] args) { // Inicia Main
        
        Conexiones conexion = new Conexiones();
        
        // Abre la conexión y se mantiene esperando 
        // para realizar acciones
        conexion.AbrirConexion();
        
    } // Termina Main

} // Termina Clase
