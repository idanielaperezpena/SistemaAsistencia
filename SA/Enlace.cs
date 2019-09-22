using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace SA
{
   public class Enlace
    {

        //codigo de conexion y creación de la base de datos

        SQLiteConnection m_dbConnection;

        public Enlace()
        {
            if (!System.IO.File.Exists("dbAsistencia.sqlite"))
            {
                SQLiteConnection.CreateFile("dbAsistencia.sqlite");
                Console.WriteLine("BD creada");
            }       
        }
        //EJECUTA COMANDOS QUE NO RETORNAN NINGUN VALOR
        public int comandos(string sql)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //error
                return 1;
            }
        }
        //CONECTA
        public void conectar()
        {
            m_dbConnection = new SQLiteConnection("Data Source=dbAsistencia.sqlite; Version=3;");
            m_dbConnection.Open();
        }
        //CIERRA
        public void cerrar()
        {
            m_dbConnection.Close();
        }
        //CREA LAS TABLAS
        public void tablas()
        {
            string sqlAlumnos = "CREATE TABLE IF NOT EXISTS ALUMNOS(ID VARCHAR(10), NOMBRE VARCHAR(100), GRADO_GRUPO VARCHAR(10),TUTOR VARCHAR(100), TELEFONO VARCHAR(10), OBSERVACIONES VARCHAR(255));";
            comandos(sqlAlumnos);
            Console.WriteLine("Tabla alumnos existe");
            string sqlAsistencia = "CREATE TABLE IF NOT EXISTS Asistencia(ID INTEGER PRIMARY KEY AUTOINCREMENT, ID_ALUMNO VARCHAR(10), FECHA varchar(50), ENTRADA VARCHAR(255), SALIDA VARCHAR(255));";
            comandos(sqlAsistencia);
            Console.WriteLine("Tabla asistencia existe");
            string sqlPersonalizacion = "CREATE TABLE IF NOT EXISTS PERSONALIZACION(ID INTEGER , RUTA_FONDO VARCHAR(255), COLOR VARCHAR(255));";
            comandos(sqlPersonalizacion);
            Console.WriteLine("Tabla  personalizacion existe ");
        }

        //operaciones para el registro de alumnos

        public int insertar(string id, string nombre, string grupo, string tutor, string telefono, string observaciones)
        {
            string sql = "INSERT INTO ALUMNOS VALUES ('"+id+"','"+nombre+"','"+grupo+"','"+tutor+"','"+telefono+"','"+observaciones+"');";
            return comandos(sql);

            
        }

        public void actualizar(string nombre, string grupo, string tutor, string telefono, string observaciones, string idactual) {
            string sql = "UPDATE ALUMNOS SET NOMBRE ='"+nombre+"', GRADO_GRUPO ='"+grupo+"',TUTOR='"+tutor+"', TELEFONO='"+telefono+"', OBSERVACIONES='"+observaciones+"' WHERE ID = '"+idactual+"';";
            comandos(sql);
        }

        public void eliminar(string id)
        {
            string sql = "DELETE FROM ALUMNOS WHERE ID ='" + id + "';";
            comandos(sql);
        }

        public int consulta_existencia(string id)
        {         
            SQLiteCommand command = new SQLiteCommand("SELECT COUNT(ID) FROM ALUMNOS WHERE ID = '"+id+"' ;", m_dbConnection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();          
            return count;
        }

        public SQLiteDataAdapter consulta()
        {

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM ALUMNOS;", m_dbConnection);
            command.ExecuteNonQuery();
            SQLiteDataAdapter dataadp = new SQLiteDataAdapter(command);
            return dataadp;
        }

        //operaciones para personalización

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

        //operaciones de registro de asistencia de alumnos

        public void registro_asistencia(string id_alumno, bool tipo)
        {
            DateTime fechahora = DateTime.Now;
            string fecha = fechahora.ToString("dd-MM-yyyy");
            string hora = fechahora.ToString("HH:mm"); //con segundos?
            string sql = "";
            if (tipo==false)
            {
                sql = "INSERT INTO Asistencia VALUES (NULL,'" + id_alumno + "','" + fecha + "','" + hora + "','');";
               
            }
            else
            {
                sql = "UPDATE Asistencia SET SALIDA='" + hora + "' where id_alumno = '" + id_alumno + "';";
                
            }
            comandos(sql);

        }
               
        public int consultar_asistencia(string id_alumno)
        {
            DateTime fechahora = DateTime.Now;
            string fecha = fechahora.ToString("dd-MM-yyyy");
            SQLiteCommand command = new SQLiteCommand("SELECT COUNT(ID_ALUMNO) FROM Asistencia WHERE ID_ALUMNO = '" + id_alumno + "' AND FECHA = '"+fecha+"' ;", m_dbConnection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();
            return count;
        }

        public String[] consulta_alumno(string ID)
        {
            string[] resultado = new String[6]; //6campos
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM ALUMNOS where ID = '"+ID+"';", m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                for (int i=0; i<6; i++)
                {
                    resultado[i] = reader[i].ToString();
                }
            }
            
            return resultado;   
        }

        //operaciones para la lista de asistencias

        public SQLiteDataAdapter combo()
        {
            SQLiteCommand comando = new SQLiteCommand("SELECT DISTINCT GRADO_GRUPO FROM ALUMNOS", m_dbConnection);
            comando.ExecuteNonQuery();
            SQLiteDataAdapter dataadp = new SQLiteDataAdapter(comando);
            
            return dataadp;
        }

        public SQLiteDataAdapter consulta_lista_asistencia()
        {

            SQLiteCommand command = new SQLiteCommand("select Asistencia.ID, ID_ALUMNO, Alumnos.NOMBRE,Alumnos.GRADO_GRUPO, FECHA, ENTRADA, SALIDA from Asistencia inner join alumnos on ALUMNOS.ID = Asistencia.ID_ALUMNO; ", m_dbConnection);
            command.ExecuteNonQuery();
            SQLiteDataAdapter dataadp = new SQLiteDataAdapter(command);
            return dataadp;
        }

        //borrando toda la tabla
        public int borrado()
        {
            string sql = "Delete from Alumnos; Delete from Asistencia;";
            return comandos(sql);
        }



    }
}
