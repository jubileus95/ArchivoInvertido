using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Createtxt
    {

        private void print(string v)
        {
            throw new NotImplementedException();
        }
        static void main()
        {
            // Compose a string that consists of three lines.
            string lines = "First line.\r\nSecond line.\r\nThird line.";

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
            file.WriteLine(lines);
            //print("creado");
            file.Close();
         }
    }
}
