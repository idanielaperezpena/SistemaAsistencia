using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

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

        public void insertar(string id, string nombre, string grupo, string observaciones, string foto)
        {
            string sql = "INSERT INTO ALUMNOS VALUES ('"+id+"','"+nombre+"','"+grupo+"','"+observaciones+"','"+foto+"');";
            comandos(sql);
            
        }

        public void actualizar(string nombre, string grupo, string observaciones, string foto, string idactual) {
            string sql = "UPDATE ALUMNOS SET NOMBRE ='"+nombre+"', GRADO_GRUPO ='"+grupo+"', OBSERVACIONES='"+observaciones+"', FOTO='"+foto+"' WHERE ID = '"+idactual+"';";
            comandos(sql);
        }

        public int consulta_existencia(string id)
        {
           
            SQLiteCommand command = new SQLiteCommand("SELECT ID FROM ALUMNOS WHERE ID = '"+id+"' ;", m_dbConnection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();
            
            return count;
        }

        public void color(string color, bool tipo)
        {
            string sql="";
            
            
            if(!tipo)
            {
                sql= "UPDATE PERSONALIZACION SET COLOR='" + color + "' WHERE ID=1;";
            }
            else
            {
                sql = "INSERT INTO PERSONALIZACION VALUES(1,'','" + color + "');";
            }
                 
            comandos(sql);
            Uri uri = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor."+color+".xaml");
            Application.Current.Resources.MergedDictionaries[0].Source = uri;
           
        }

        public void comandos(string sql)
        {
            try
            {
                
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                
                command.ExecuteNonQuery();
               
                      
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }

         
        }

        public SQLiteDataAdapter consulta()
        {
            
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM ALUMNOS;", m_dbConnection);
            command.ExecuteNonQuery();
            SQLiteDataAdapter dataadp = new SQLiteDataAdapter(command); 
            
            return dataadp;
        }
        public string[] consultaPersonalizacion()
        {
            
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Personalizacion;", m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string [] res= new string[2];
            if (reader.Read())
            {
                res[0] = reader[1].ToString();
                res[1] = reader[2].ToString();
            }
            
           
            return res;
        }
    }
}
