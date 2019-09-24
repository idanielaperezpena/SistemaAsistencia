using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para Personalizacion.xaml
    /// </summary>
    public partial class Personalizacion : Window
    {
        MainWindow mainWindow;
        Enlace enlace;
        public Personalizacion( Enlace enlace,MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.enlace = enlace;
            InitializeComponent();
            TamanoPantalla(this, mainWindow);
        }
        private void TamanoPantalla(Window receiver, Window giver)
        {
            receiver.WindowState = giver.WindowState;
        }
        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            TamanoPantalla(mainWindow, this);
            mainWindow.Show();
            this.Close();
        }

        

        private void btnAplicar_Click(object sender, RoutedEventArgs e)
        {
            bool tipo=false; //false update true insert
            
            enlace.conectar();
            String[] s = new string[2];
            s=enlace.consultaPersonalizacion();
           
            if (!string.IsNullOrEmpty(s[1]))
            {
                tipo = false;
            }
            else
            {
                tipo = true;
            }
            if (rdioAmarillo.IsChecked == true)
            {
                enlace.color("Amber", tipo);
                
            }
            if (rdioAzul.IsChecked == true)
            {
                enlace.color("Indigo", tipo);
            }
            if (rdioGris.IsChecked == true)
            {
                enlace.color("Grey", tipo);
            }
            if (rdioMorado.IsChecked == true)
            {
                enlace.color("DeepPurple", tipo);
            }
            if (rdioNaranja.IsChecked == true)
            {
                enlace.color("DeepOrange", tipo);
            }
            if (rdioRojo.IsChecked == true)
            {
                enlace.color("Red", tipo);
            }
            if (rdioRosa.IsChecked == true)
            {
                enlace.color("Pink", tipo);
            }
            if (rdioTurquesa.IsChecked == true)
            {
                enlace.color("Teal", tipo);
            }
            if (rdioVerde.IsChecked == true)
            {
                enlace.color("Green", tipo);
            }
            enlace.cerrar();
            
            if (imgFondo.Source != null)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgFondo.Source));
                using (FileStream stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory+"Fondo.jpeg", FileMode.Create))
                    encoder.Save(stream);
            }
                     
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TamanoPantalla(mainWindow, this);
            mainWindow.Show();
            
        }

        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtBlckImagen.Text = filename;
                //guardando la imagen
                
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(filename);
                
               
                b.EndInit();
                imgFondo.Source = b;
              

               // byte[] img = Convert.FromBase64String();
                //System.IO.File.WriteAllBytes("",img);
            }

           
            
            

        }
    }
}
