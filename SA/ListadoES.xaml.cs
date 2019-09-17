using System;
using System.Collections.Generic;
using System.Data;
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

namespace SA
{
    /// <summary>
    /// Lógica de interacción para ListadoES.xaml
    /// </summary>
    public partial class ListadoES : Window
    {
        public ListadoES()
        {
            InitializeComponent();
        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Environment.Exit(1);
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

            int n = dgListado.Columns.Count;
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
            foreach (DataRowView r in dgListado.Items)
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
    }
}
