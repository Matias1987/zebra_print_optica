using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ZebraPrint.lib.print
{
    public class ZebraPrinter
    {
        /*public static string ZPL2_code = @"^XA
        ^FO350,10^BY3 ^BAN,30,N,N ^FD{0}^FS ^CFA,20 
        ^FO400,45^FD{1}^FS
        ^FO30,75^GB700,3,3^FS
        ^FO350,90^BY3 ^BAN,30,N,N ^FD{0}^FS ^CFA,20 
        ^FO400,125^FD{1}^FS
        ^FO30,150^GB700,3,3^FS
        ^FO350,165^BY3 ^BAN,30,N,N ^FD{0}^FS ^CFA,20 
        ^FO400,205^FD{1}^FS
        ^XZ";*/
        public static string ZPL2_code = @"^FO{2},{3}^BY3 ^BAN,30,N,N ^FD{0}^FS ^CFA,20^FO{2},{4}^FD{1}^FS^FO0,{5}^GB700,3,3^FS";

        public static int xoffset = 400;

        public static int yoffset = 10;

        public static int quantity = 3;

        public static int height = 90;

        public static string GetZPL2Code(string code,  string barcode, string text, int xoffet, int yoffset, int height, int repeat = 1)
        {
            string _barcode = "^XA";
            for(int i = 0; i < repeat; i++)
            {
                _barcode += string.Format(code, barcode, text, xoffet , yoffset + height * i, yoffset + height * i + 35, yoffset + height * i + 60);
            }
            _barcode += "^XZ";

            return _barcode;
        }
        
        public static string GetZPL2Code2(string code,  string barcode, string text, int xoffet, int yoffset, int height, int index)
        {
            //string _barcode = "^XA";

            return string.Format(code, barcode, text, xoffet, yoffset + height * index, yoffset + height * index + 35, yoffset + height * index + 60);

            //_barcode += "^XZ";

        }

        public static bool useTwoCodesPerTicket = true;

        private static ZebraPrinter instance = null;

        public static ZebraPrinter Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ZebraPrinter();
                }
 
                return ZebraPrinter.instance; 
            }

            set { ZebraPrinter.instance = value; }
        }

        public void PrintCode(string _code)
        {
            
        }

        public void PrintCodes(List<Code> codesToPrint, Control parent=null)
        {
             PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            if (DialogResult.OK == pd.ShowDialog(parent))
            {
                if (!useTwoCodesPerTicket)
                {
                    foreach (Code code in codesToPrint)
                    {
                        for (int i = 0; i < code.Cantidad; i++)
                        {
                            string _code = code.Barcode; //UnionStrings(code.Idcodigo.ToString());
                            string _zplIICode = GetZPL2Code(ZPL2_code, code.Barcode, code.Codigo, xoffset, yoffset, height, quantity);  //string.Format(ZPL2_code, _code, " ", code.Barcode);
                            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, _zplIICode);
                        }
                    }
                }
                else
                {
                    List<Code> __codestoprint = new List<Code>();
                    foreach (Code code in codesToPrint)
                    {
                        for (int i = 0; i < code.Cantidad; i++)
                        {
                            Code _s = new Code();

                            _s.Codigo = code.Codigo;
                            _s.Barcode = code.Barcode;
                            __codestoprint.Add(_s);
                        }
                    }
                    Console.WriteLine(__codestoprint.Count.ToString());

                    while (__codestoprint.Count>0)
                    {
                        /*
                         *  check wether there are two codes at least...
                         * */

                        /*if (__codestoprint.Count > 1)
                        {
                            string _zplIICode = GetZPL2Code(ZPL2_code, __codestoprint[0].Barcode, __codestoprint[0].Codigo, xoffset, yoffset, height, quantity);

                            __codestoprint.RemoveAt(0);
                            __codestoprint.RemoveAt(0);

                            Console.WriteLine(_zplIICode);

                            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, _zplIICode);

                        }
                        else
                        {
                            string _zplIICode = GetZPL2Code(ZPL2_code, __codestoprint[0].Barcode, __codestoprint[0].Codigo, xoffset, yoffset, height, quantity);

                            __codestoprint.RemoveAt(0);

                            Console.WriteLine(_zplIICode);

                            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, _zplIICode);

                        }*/

                        string _zplIICode = "^XA";

                        for (int i=0;i<quantity;i++)
                        {
                            if(__codestoprint.Count > 0)
                            {
                                _zplIICode += GetZPL2Code2(ZPL2_code, __codestoprint[0].Barcode, __codestoprint[0].Codigo, xoffset, yoffset, height, i);
                                __codestoprint.RemoveAt(0);
                                
                                
                            }

                        }
                        _zplIICode += "^XZ";

                        RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, _zplIICode);
                    }
                }
            }

        }

        /*private string UnionStrings(string str, string mask = "40000000000")
        {
            string output = "";
            int _dif = mask.Length - str.Length;
            for (int i = mask.Length - 1; i > -1; i--)
            {
                char _current = (i - _dif < 0) ? mask[i] : str[i - _dif];
                output = _current + output;
            }
            return output;
        }*/



    }

    
}
