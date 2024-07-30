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
using System.Net;
using System.IO;
namespace ZebraPrint
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.txtZPLCode.Text = ZebraPrinter.ZPL2_code;
            this.tBX.Value = ZebraPrinter.xoffset;
            this.tBY.Value = ZebraPrinter.yoffset;
            this.nudCantidad.Value = ZebraPrinter.quantity;
            this.numericUpDown1.Value = ZebraPrinter.height;
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            refreshImage();
        }

        private void refreshImage()
        {
            byte[] zpl = Encoding.UTF8.GetBytes(ZebraPrinter.GetZPL2Code(txtZPLCode.Text,"ARM000", "TEST",tBX.Value,tBY.Value,(int)numericUpDown1.Value,(int)nudCantidad.Value));

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
            request.Method = "POST";
            //request.Accept = "application/pdf"; // omit this line to get PNG images back
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                var fileStream = File.Create("label.png"); // change file name for PNG images
                responseStream.CopyTo(fileStream);
                pictureBox1.Image = Image.FromStream(fileStream);
                responseStream.Close();
                fileStream.Close();
                
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string csv = txtZPLCode.Text.Trim() + ";" + tBX.Value.ToString() + ";" + tBY.Value.ToString() + ";" + ((int)nudCantidad.Value).ToString() + ";" + ((int)numericUpDown1.Value).ToString();
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "data");
                //Write a line of text
                sw.Write(csv);
                //Close the file
                sw.Close();
            }
            catch (Exception _e)
            {
                Console.WriteLine("Exception: " + _e.Message);
            }
            finally
            {
                Close();
            }
        }
    }
}
