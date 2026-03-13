package DataLayer; // package

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.CallableStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;
import java.util.ArrayList;

public class SQLConnection { // Inicia Clase
    
    private final String SQLString = config.getSQLString();
    
    private Connection getConnection() throws SQLException {
        
        return DriverManager.getConnection(SQLString);
    }
    
    public boolean executeSP(String spName, Object... params) {

        StringBuilder call = new StringBuilder();
        call.append("{CALL ").append(spName).append("(");

        for (int i = 0; i < params.length; i++) {
            call.append("?");
            if (i < params.length - 1) {
                call.append(",");
            }
        }

        call.append(")}");

        try (Connection connection = getConnection();
             CallableStatement cs = connection.prepareCall(call.toString())) {

            for (int i = 0; i < params.length; i++) {
                cs.setObject(i + 1, params[i]);
            }

            cs.execute(); 

            return true;

        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }
    
    // Solamente un dato
    public Object selectData(String sql, Object... parametros) {

        try (Connection connection = getConnection();
                
            PreparedStatement ps = connection.prepareStatement(sql)) {

            setParametros(ps, parametros);

            try (ResultSet resultado = ps.executeQuery()) {
                
                if (resultado.next()) {
                    return resultado.getObject(1); // primera columna
                }
            }

        } catch (SQLException e) {
            
            System.out.println("Error en consulta");
            e.printStackTrace();
        }

        return null;
    }
    
    // Todos los datos
    public List<Object[]> selectAll(String sql, Object... parametros) {

        List<Object[]> lista = new ArrayList<>();

        try (Connection connection = getConnection();
            
            PreparedStatement ps = connection.prepareStatement(sql)) {

            setParametros(ps, parametros);

            try (ResultSet resultado = ps.executeQuery()) {

                int columnas = resultado.getMetaData().getColumnCount();

                while (resultado.next()) {

                    Object[] fila = new Object[columnas];

                    for (int i = 0; i < columnas; i++) {
                        fila[i] = resultado.getObject(i + 1);
                    }

                    lista.add(fila);
                }
            }

        } catch (SQLException e) {
            
            System.out.println("Error en consulta general");
            e.printStackTrace();
        }

        return lista;
    }
    
    private void setParametros(PreparedStatement ps, Object... parametros) throws SQLException {

        for (int i = 0; i < parametros.length; i++) {
            ps.setObject(i + 1, parametros[i]);
        }
    }
    
} // Termina Clase
