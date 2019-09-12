using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SA
{
    class Enlace
    {
        SQLiteConnection m_dbConnection;
        public Enlace()
        {
            if (!System.IO.File.Exists("dbAsistencia.sqlite"))
            {
                SQLiteConnection.CreateFile("dbAsistencia.sqlite");
                Console.WriteLine("BD creada");
            }

                
            
        }
        public void conectar()
        {
            m_dbConnection = new SQLiteConnection("Data Source=dbAsistencia.sqlite; Version=3;");
            m_dbConnection.Open();
        }
        public void cerrar()
        {
            m_dbConnection.Close();
        }
        public void tablas()
        {
            string sqlAlumnos = "CREATE TABLE IF NOT EXISTS ALUMNOS(ID VARCHAR(10), NOMBRE VARCHAR(100), GRADO_GRUPO VARCHAR(10), OBSERVACIONES VARCHAR(255), FOTO VARCHAR(255));";
            comandos(sqlAlumnos);
            Console.WriteLine("Tabla alumnos existe");
            string sqlAsistencia = "CREATE TABLE IF NOT EXISTS Asistencia(ID INT, ID_ALUMNO VARCHAR(10), FECHA varchar(50), ENTRADA VARCHAR(255), SALIDA VARCHAR(255));";
            comandos(sqlAsistencia);
            Console.WriteLine("Tabla asistencia existe");
            string sqlPersonalizacion = "CREATE TABLE IF NOT EXISTS PERSONALIZACION(ID INTEGER , RUTA_FONDO VARCHAR(255), COLOR VARCHAR(255));";
            comandos(sqlPersonalizacion);
            Console.WriteLine("Tabla  personalizacion existe ");
        }
        public void insertar()
        {
            string sql = "INSERT INTO ALUMNOS VALUES ('1PRUEBA','DANIELA PP','1A','NEL','hshs');";
            comandos(sql);
        }
        public void comandos(string sql)
        {
            try
            {
                conectar();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                cerrar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }
            

        }
        public void consulta()
        {
            conectar();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM ALUMNOS;", m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine(reader["ID"].ToString()+ reader["Nombre"].ToString()+reader[2].ToString()+reader[3].ToString());

            cerrar();
        }
    }
}
