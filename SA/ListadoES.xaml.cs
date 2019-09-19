﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

        Enlace enlace;
        DataTable table;
        public ListadoES()
        {
            InitializeComponent();
            //combo grupo
            enlace = new Enlace();
            FillCombo();
            FillDG();
            
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
            if(cmbGrado.SelectedValue == null)
            {
                MessageBox.Show("Vacio");
           
            }
            else
            {
                MessageBox.Show(cmbGrado.SelectedValue.ToString());
            }
            
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
            doc.ColumnWidth = 999999;
            Section section = new Section();
            section.BreakColumnBefore = false;

            Table table1 = new Table();
            table1.CellSpacing = 0;

            table1.Background = Brushes.White;
            table1.FontFamily = new FontFamily("Arial");

            int n = dgListado.Columns.Count;
            table1.Columns.Add(new TableColumn() { Width = new GridLength(0.5,GridUnitType.Star)});
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
            // Add the second (header) row.
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 14;
            currentRow.FontWeight = FontWeights.Bold;


            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("ID Alumno"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Nombre"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Grado y Grupo"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Fecha"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Hora de Entrada"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Hora de Salida"))));

            int filas = 1;
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
            
            table.DefaultView.RowFilter = $"GRADO_GRUPO LIKE'{cmbGrado.SelectedValue.ToString()}%'";
        }
    }
}
