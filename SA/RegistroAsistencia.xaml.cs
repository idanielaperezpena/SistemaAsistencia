using System;
using System.Collections.Generic;
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

namespace SA
{
    /// <summary>
    /// Lógica de interacción para RegistroAsistencia.xaml
    /// </summary>
    public partial class RegistroAsistencia : Window
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public RegistroAsistencia()
        {
            InitializeComponent();
            txtFechaHora.Text= DateTime.Now.ToLongDateString() + "\n" + DateTime.Now.ToLongTimeString();
            Timer.Tick += new EventHandler(reloj);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            

        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Timer.Stop();
            this.Close();

        }
        private void reloj(object sender, EventArgs e)
        {
            
            txtFechaHora.Text = DateTime.Now.ToLongDateString() + "\n"+ DateTime.Now.ToLongTimeString();
            
        }
        
     

        

        
    }
}
