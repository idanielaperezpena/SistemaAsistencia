using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para ListadoES.xaml
    /// </summary>
    public partial class ListadoES : Window
    {

        Enlace enlace;
        DataTable table;
        MainWindow mainWindow;
        public ListadoES(Enlace enlace, MainWindow mainWindow)
        {
            this.enlace = enlace;
            this.mainWindow = mainWindow;
            InitializeComponent();
            
            //combo grupo
            enlace = new Enlace();
            FillCombo();
            FillDG();
            TamanoPantalla(this, mainWindow);
           
        }
        private void TamanoPantalla(Window receiver, Window giver)
        {
            receiver.WindowState = giver.WindowState;
        }
        private void FillDG()
        {
           
            enlace.conectar();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.consulta_lista_asistencia();
            
            table = new DataTable("LISTADO");
            adapter.Fill(table);
            
            dgListado.ItemsSource = table.DefaultView;
            adapter.Update(table);
            enlace.cerrar();
        }


        private void FillCombo()
        {
           
            enlace.conectar();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.combo();
            enlace.cerrar();
            DataTable table = new DataTable();

            adapter.Fill(table);
            
           
            cmbGrado.DisplayMemberPath = "GRADO_GRUPO";
            cmbGrado.SelectedValuePath = "GRADO_GRUPO";
            cmbGrado.ItemsSource = table.DefaultView;

        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {


            TamanoPantalla(mainWindow, this);
            mainWindow.Show();

            this.Close();

        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TamanoPantalla(mainWindow, this);
            mainWindow.Show();
           
        
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
            //algo muy amorfo
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            // Create a Section  
            doc.ColumnGap = 0;
            doc.ColumnWidth = 999999;
            Section section = new Section();
            section.BreakColumnBefore = false;

            Table table1 = new Table();
            table1.CellSpacing = 0;

            table1.Background = Brushes.White;
            table1.FontFamily = new FontFamily("Arial");

            int n = dgListado.Columns.Count;
            table1.Columns.Add(new TableColumn() { Width = new GridLength(1,GridUnitType.Star)});
            table1.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });
            table1.Columns.Add(new TableColumn() { Width = new GridLength(5, GridUnitType.Star) });
            for (int i = 3; i < n; i++)
            {
                table1.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
            }
            

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table1.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table1.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Listado de Asistencia"))));
            // and set the row to span all  columns.
            currentRow.Cells[0].ColumnSpan = n;
            //Segunda fila, grupo e intervalo de fecha

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[1];
            currentRow.FontSize = 14;
            currentRow.FontWeight = FontWeights.Bold;
            string grado;
            if (cmbGrado.SelectedItem!=null)
            {
                grado = cmbGrado.SelectedValue.ToString();
            }
            else
            {
                grado = "No Aplica";
            }
            String fechaI="";
            String fechaF = "";
            if (datepkInicio.SelectedDate != null)
            {
                fechaI = datepkInicio.SelectedDate.Value.ToShortDateString();
            }
            if (datepkFin.SelectedDate != null)
            {
                fechaF = datepkFin.SelectedDate.Value.ToShortDateString();
            }

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Grupo: "+grado))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Fecha: "+fechaI+" - "+fechaF))));
            currentRow.Cells[0].ColumnSpan = n/2;
            currentRow.Cells[1].ColumnSpan = n/2;
            currentRow.Cells[0].Padding = new Thickness(4, 4, 4, 4);
            currentRow.Cells[1].Padding = new Thickness(4, 4, 4, 4);
            // tercera fila, encabezados
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[2];

            // Global formatting for the header row.
            currentRow.FontSize = 13;
            currentRow.FontWeight = FontWeights.Bold;


            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("ID Alumno"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Nombre"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Grupo"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Fecha"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Entrada"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Salida"))));

            for (int a = 0; a < currentRow.Cells.Count; a++)
            {
                
                currentRow.Cells[a].Padding = new Thickness(4, 4, 4, 4);
            }

            int filas = 2;
            //contenido
            foreach (DataRowView r in dgListado.ItemsSource)
            {
                //new row
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[++filas];
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



            section.Blocks.Add(table1);
            doc.Blocks.Add(table1);
            return doc;
        }

        private void cmbGrado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filtrar();
            
        }

        private void cmbIntervalo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime fecha = DateTime.Today;
 

            switch (cmbIntervalo.SelectedIndex)
            {
                case 0:

                    datepkInicio.SelectedDate = fecha;
                    datepkFin.SelectedDate = fecha;
                    break;
                case 1:
                    double quitar;
                    quitar = Convert.ToDouble(fecha.DayOfWeek);
                    datepkInicio.SelectedDate = fecha.AddDays(-quitar+1);
                    datepkFin.SelectedDate = fecha.AddDays(7 - quitar);
                    break;

                case 2:
                    string fec = "01" +"/"+ DateTime.Now.Month.ToString("00") + "/"+ DateTime.Now.Year.ToString();
                    fecha = DateTime.Parse(fec);
                    datepkInicio.SelectedDate = fecha;
                    fecha= fecha.AddMonths(1);
                    fecha= fecha.AddDays(-1);
                    datepkFin.SelectedDate = fecha;
                    break;
                default:
                    datepkInicio.SelectedDate = fecha;
                    datepkFin.SelectedDate = fecha;
                    break;
                    
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            

            filtrar();
        }
        private void filtrar()
        {
            table.DefaultView.RowFilter = null;
            if (datepkInicio.SelectedDate == null && datepkFin.SelectedDate == null && cmbGrado.SelectedValue != null)
            {
                table.DefaultView.RowFilter = $"GRADO_GRUPO LIKE'{cmbGrado.SelectedValue.ToString()}%'";
               
            }
            if(datepkInicio.SelectedDate != null && datepkFin.SelectedDate != null && cmbGrado.SelectedValue != null)
            {
                
                   table.DefaultView.RowFilter = $"GRADO_GRUPO LIKE'{cmbGrado.SelectedValue.ToString()}%' AND CONVERT(FECHA,'System.DateTime') >='#{datepkInicio.SelectedDate}#' AND CONVERT(FECHA,'System.DateTime') <= '#{datepkFin.SelectedDate}#'";
               
            }
            if (datepkInicio.SelectedDate != null && datepkFin.SelectedDate != null && cmbGrado.SelectedValue == null)
            {

               
                    table.DefaultView.RowFilter = $"CONVERT(FECHA,'System.DateTime') >='#{datepkInicio.SelectedDate}#' AND CONVERT(FECHA,'System.DateTime') <= '#{datepkFin.SelectedDate}#'";
                

            }

        }

        private void btnBorrarfiltros_Click(object sender, RoutedEventArgs e)
        {
            table.DefaultView.RowFilter = null;
            cmbGrado.SelectedIndex = -1;
            cmbIntervalo.SelectedIndex = -1;
        }
    }
}
