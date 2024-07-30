using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZebraPrint.lib.print;
using System.IO;
namespace ZebraPrint
{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
          
            loadCSV(openFileDialog.FileName);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<Code> codigos = new List<Code>();

            for (int r = 0; r < dataGridView1.Rows.Count; r++) {

                if (((bool)dataGridView1.Rows[r].Cells[2].Value))
                {
                    Code temp = (Code)dataGridView1.Rows[r].Tag;
                    int _cantidad = (int)dataGridView1.Rows[r].Cells[1].Value;

                    Code c = new Code
                    {
                        Barcode = temp.Barcode,//dataGridView1.Rows[r].Cells[0].Value.ToString(),
                        Codigo = temp.Codigo,//dataGridView1.Rows[r].Cells[1].Value.ToString(),
                        Cantidad = _cantidad
                    };
                    codigos.Add(c);

                }

            }

            if(codigos.Count<1)
            {
                MessageBox.Show("No hay códigos para imprimir");
                return;
            }

            ZebraPrinter.Instance.PrintCodes(codigos);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*List<Code> codigos = new List<Code>();
            
            for (int i=0;i<4;i++)
            {
                Code c = new Code
                {
                    Barcode = string.Format("LARM{0}", i),
                    Cantidad = 1
                };
                codigos.Add(c);
            }*/


            LoadConfig();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {

                dataGridView1.Rows[e.RowIndex].Cells[2].Value = !((bool)dataGridView1.Rows[e.RowIndex].Cells[2].Value);

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int.TryParse((string)dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out int temporary_i);

                dataGridView1.Rows[e.RowIndex].Cells[1].Value = temporary_i;

            }
        }

        private void loadCSV(string filename)
        {
            try
            {
                List<Code> codigos = new List<Code>();
                using (var reader = new StreamReader(filename))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        Code code = new Code();
                        code.Barcode = values[0].Trim('\"');
                        int.TryParse(values[2].Trim('\"'), out int temp_int);
                        code.Codigo = values[1].Trim('\"');
                        code.Cantidad = temp_int;
                        codigos.Add(code);
                    }

                    loadDataGrid(codigos);
                }
            }catch(Exception _e)
            {
                Console.WriteLine(_e.Message);
            }

        }

        private void loadDataGrid(List<Code> codigos)
        {
            codigos.ForEach(delegate (Code c) {

                int idx = dataGridView1.Rows.Add(new object[] { c.Codigo, c.Cantidad, true });
                dataGridView1.Rows[idx].Tag = c;
            });
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog(this);
            LoadConfig();
        }

        private void LoadConfig()
        {
            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            String line;
            string text = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "data");
                
                //Read the first line of text
                line = sr.ReadLine();
                
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window

                    //Read the next line
                    text += line;
                    line = sr.ReadLine();
                    
                   

                }

                
                //close the file
                sr.Close();
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
               
                
                string[] parts = text.Split(';');
                ZebraPrinter.ZPL2_code = parts[0];
                int.TryParse(parts[1], out int _xoffset);
                int.TryParse(parts[2], out int _yoffset);
                int.TryParse(parts[3], out int _cant);
                int.TryParse(parts[4], out int _h);
                ZebraPrinter.xoffset = _xoffset;
                ZebraPrinter.yoffset = _yoffset;
                ZebraPrinter.quantity = _cant;
                ZebraPrinter.height = _h;

            }
        }
    }
}
