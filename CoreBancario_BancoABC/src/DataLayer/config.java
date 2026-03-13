package DataLayer; // package

public class config { // Inicia Clase
    
    public static String getHost(){
    
        return "127.0.0.1";
    }
    
    public static int getPort(){
        
        return 6000;
    }
    
    public static String getSQLString(){
    
        return "jdbc:sqlserver://localhost:1433;" +
        "databaseName=CORE_BANCOABC_DB;" +
        "user=sa;" + 
        "password=Meg@mancero666;" +
        "encrypt=true;" +
        "trustServerCertificate=true;";
    }
    
} // Termina Clase
