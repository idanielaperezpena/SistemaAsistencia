using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para RegistroAsistencia.xaml
    /// </summary>
    public partial class RegistroAsistencia : Window
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        Enlace enlace;
        MainWindow mainWindow;
        public RegistroAsistencia( Enlace enlace, MainWindow mainWindow)
        {
            this.enlace = enlace;
            this.mainWindow = mainWindow;
            InitializeComponent();
            


            Image image = new Image();
            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Fondo.jpeg", FileMode.Open))
                {
                    image.Source = BitmapFrame.Create(streams,
                                                      BitmapCreateOptions.None,
                                                      BitmapCacheOption.OnLoad);
                }
                ImageBrush brush = new ImageBrush(image.Source);
                winRegAsist.Background = brush;
            }
            catch (Exception)
            {

            }
            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "users/defaultUser.png", FileMode.Open))
                {
                    imgFoto.Source = BitmapFrame.Create(streams,
                                                      BitmapCreateOptions.None,
                                                      BitmapCacheOption.OnLoad);

                }

            }
            catch (Exception)
            {

            }

            txtID.Focus();
            txtFechaHora.Text= DateTime.Now.ToLongDateString() + "\n" + DateTime.Now.ToLongTimeString();
            Timer.Tick += new EventHandler(reloj);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            

        }
       

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
         
            
            mainWindow.Show();
            Timer.Stop();
            this.Close();

        }
        private void reloj(object sender, EventArgs e)
        {
            
            txtFechaHora.Text = DateTime.Now.ToLongDateString() + "\n"+ DateTime.Now.ToLongTimeString();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            mainWindow.Show();
          
        }

        


        private void limpiar()
        {
            
            txbID.Text = "ID";
            txbNombre.Text ="Nombre";
            txbGradoGrupo.Text = "Grado y Grupo";
            txbObservaciones.Text = String.Empty;
            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "users/defaultUser.png", FileMode.Open))
                {
                    imgFoto.Source = BitmapFrame.Create(streams,
                                                      BitmapCreateOptions.None,
                                                      BitmapCacheOption.OnLoad);

                }

            }
            catch (Exception)
            {

            }

        }
        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
                      
            if (e.Key == Key.Enter)
            {
                enlace.conectar();
                String[] resultado = new string[6];
                resultado = enlace.consulta_alumno(txtID.Text);
                if (resultado[0] != null)
                {
                    bool contador = Convert.ToBoolean(enlace.consultar_asistencia(txtID.Text));
                    enlace.registro_asistencia(txtID.Text, contador);
                    txbID.Text = resultado[0];
                    txbNombre.Text = resultado[1];
                    txbGradoGrupo.Text = resultado[2];
                    txbObservaciones.Text = resultado[3];
                    carga_imagen(resultado[0]);

                    
                   
                }
                else
                {
                    limpiar();
                    MessageBox.Show("Alumno no encontrado", "No encontrado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                txtID.Clear();
                enlace.cerrar();
            }
        }
        private void carga_imagen(string id)
        {

            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory + id + ".jpeg", FileMode.Open))
                {
                    imgFoto.Source = BitmapFrame.Create(streams,
                                                      BitmapCreateOptions.None,
                                                      BitmapCacheOption.OnLoad);

                }

            }
            catch (Exception)
            {

            }
        }
    }
}
