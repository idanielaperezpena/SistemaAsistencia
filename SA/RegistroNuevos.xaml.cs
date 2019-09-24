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
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para RegistroNuevos.xaml
    /// </summary>
    public partial class RegistroNuevos : Window
    {
        Enlace enlace;
        MainWindow mainWindow;
        public RegistroNuevos(Enlace enlace, MainWindow mainWindow)
        {
            this.enlace = enlace;
            this.mainWindow = mainWindow;
            InitializeComponent();
            carga_dg();
            TamanoPantalla(this, mainWindow);
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
        private void TamanoPantalla(Window receiver, Window giver)
        {
            receiver.WindowState = giver.WindowState;
        }

        private void carga_dg()
        {
            enlace.conectar();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.consulta();
            DataTable table = new DataTable("ALUMNOS");
            adapter.Fill(table);
            dgAlumnos.ItemsSource = table.DefaultView;
            adapter.Update(table);
            enlace.cerrar();
        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            TamanoPantalla(mainWindow, this);

            mainWindow.Show();
            
            this.Close();
        }

        private void btnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // enlace = new Enlace(); ya tenias tu enlace global e inicializada en el constructor
            
            enlace.conectar();
            DataRowView row = dgAlumnos.SelectedItem as DataRowView;
            if (!String.IsNullOrWhiteSpace(txtID.Text) && !String.IsNullOrWhiteSpace(txtNombre.Text) && !String.IsNullOrWhiteSpace(txtGrupo.Text))
            {
                if (row == null)
                {
                    int contador = enlace.consulta_existencia(txtID.Text);
                    if (contador == 0)
                    {

                        enlace.insertar(txtID.Text, txtNombre.Text, txtGrupo.Text,txtTutor.Text,txtTelefono.Text, txtObservaciones.Text);
                        imagen(txtID.Text);
                        MessageBox.Show("Alumno registrado con éxito.");
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("El ID de alumno ya existe.");
                    }
                }

                else
                {
                    if (txtID.Text == row["ID"].ToString())
                    {
                        enlace.actualizar(txtNombre.Text, txtGrupo.Text, txtTutor.Text, txtTelefono.Text, txtObservaciones.Text, txtID.Text);
                        imagen(txtID.Text);
                        MessageBox.Show("Datos de alumno actualizados.");
                        limpiar();
                    }
                    else
                    {
                        if (txtNombre.Text == row["NOMBRE"].ToString())
                        {
                            MessageBoxResult result = MessageBox.Show("Ya hay un alumno registrado con este nombre, ¿Desea continuar?", "Registro", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.OK:
                                    enlace.insertar(txtID.Text, txtNombre.Text, txtGrupo.Text, txtTutor.Text, txtTelefono.Text, txtObservaciones.Text);
                                    imagen(txtID.Text);
                                    MessageBox.Show("Alumno registrado con éxito.");
                                    limpiar();
                                    break;

                                case MessageBoxResult.Cancel:
                                    break;
                            }
                        }
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Hay campos necesarios vacíos");
            }

            dgAlumnos.ItemsSource = null;
            carga_dg();
            enlace.cerrar();
        }
        private void imagen(string id)
        {
            if (imgFoto.Source != null )
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgFoto.Source));
                using (FileStream stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + id +".jpeg", FileMode.Create))
                    encoder.Save(stream);
            }
        }
        private void carga_imagen(string id)
        {
            
            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory + id+".jpeg", FileMode.Open))
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
        public void limpiar()
        {
            txtID.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtGrupo.Text = String.Empty;
            txtTutor.Text = String.Empty;
            txtTelefono.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            try
            {
                using (FileStream streams = new FileStream(AppDomain.CurrentDomain.BaseDirectory +"users/defaultUser.png", FileMode.Open))
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

        private void dgAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            limpiar();
            DataRowView row = dgAlumnos.SelectedItem as DataRowView;
            if (row != null)
            {
                txtID.Text = row["ID"].ToString();
                txtNombre.Text = row["NOMBRE"].ToString();
                txtGrupo.Text = row["GRADO_GRUPO"].ToString();
                txtTutor.Text = row["TUTOR"].ToString();
                txtTelefono.Text = row["TELEFONO"].ToString();
                txtObservaciones.Text = row["OBSERVACIONES"].ToString();
                carga_imagen(row["ID"].ToString());
                
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
                txBlckRutaImagen.Text = filename;
                //guardando la imagen
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(filename);
                b.EndInit();
                imgFoto.Source = b;
                
            }
        }

       

        private void txtTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {
            //char[] c = txtTelefono.Text.ToCharArray();
            //foreach( char element in c)
            //{
            //    if (((element) < 48 && element != 8 && element != 46) || element > 57)
            //    {
            //        txtTelefono.BorderBrush = Brushes.Red;
            //        txtTelefono.BorderThickness= new Thickness(3, 3, 3, 3);
            //        break;
                
            //    e.Handled = true;
            //    }
            //}

        }
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            // Creando el pintdialog
            PrintDialog printDlg = new PrintDialog();

            if ((bool)printDlg.ShowDialog().GetValueOrDefault())
            {
                // Create a FlowDocument dynamically. 

                FlowDocument doc = CreateFlowDocument();
                doc.Name = "Listado";
                // Create IDocumentPaginatorSource from FlowDocument  
                IDocumentPaginatorSource idpSource = doc;
                doc.ColumnWidth = printDlg.PrintableAreaWidth;
                // Call PrintDocument method to send document to printer  
                printDlg.PrintDocument(idpSource.DocumentPaginator, "Listado de asistencia");
            }
        }
        private FlowDocument CreateFlowDocument()
        {
            //alfo muy amorfo
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            // Create a Section  
            doc.ColumnGap = 0;
            Section section = new Section();
            section.BreakColumnBefore = false;

            Table table = new Table();
            table.CellSpacing = 0;

            table.Background = Brushes.White;

            int n = dgAlumnos.Columns.Count;
            for (int i = 0; i < n; i++)
            {
                table.Columns.Add(new TableColumn());
            }
            // Create and add an empty TableRowGroup to hold the table's Rows.
            table.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 18;
            currentRow.FontWeight = System.Windows.FontWeights.Bold;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Listado de Asistencia"))));
            // and set the row to span all  columns.
            currentRow.Cells[0].ColumnSpan = n;
            // Add the second (header) row.
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Bold;


            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Product"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 1"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 2"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 3"))));

            int filas = 1;
            //contenido
            foreach (DataRowView r in dgAlumnos.Items)
            {
                //new row
                table.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table.RowGroups[0].Rows[++filas];
                // Global formatting for the row.
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Normal;

                // Add cells with content to the third row.
                for (int i = 0; i < n; i++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(r[i].ToString()))));

                }



                // Bold the first cell.
                currentRow.Cells[0].FontWeight = FontWeights.Bold;
                for (int a = 0; a < currentRow.Cells.Count; a++)
                {
                    currentRow.Cells[a].BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                    currentRow.Cells[a].BorderBrush = Brushes.Gray;
                    currentRow.Cells[a].Padding = new Thickness(3, 3, 3, 3);
                }


            }

            // Add Section to FlowDocument  



            section.Blocks.Add(table);
            doc.Blocks.Add(table);
            return doc;
        }

        private void btnCargarExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel excel = new Excel(enlace);
            excel.ShowDialog();
            carga_dg();
            
            

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            enlace.conectar();
            DataRowView row = dgAlumnos.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("No ha seleccionado un registro.");
            }
            else
            {
                MessageBoxResult resulta = MessageBox.Show("¿Desea eliminar el registro?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                switch (resulta)
                {
                    case MessageBoxResult.OK:
                        enlace.eliminar(row["ID"].ToString());
                        MessageBox.Show("Registro eliminado");
                        limpiar();
                        break;

                    case MessageBoxResult.Cancel:
                        limpiar();
                        break;

                }

            }
            carga_dg();
            enlace.cerrar();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            carga_dg();

        }
    }
}
