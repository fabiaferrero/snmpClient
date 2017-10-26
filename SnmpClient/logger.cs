using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnmpClient
{
    class logger
    {

        public static void Main(String[] args)
        {
            StreamWriter w = File.AppendText("log.txt");
            Log("Test1", w);
            Log("Test2", w);
            // Close the writer and underlying file.
            w.Close();
            // Open and read the file.
            StreamReader r = File.OpenText("log.txt");
            DumpLog(r);
        }
        public static void Log(String logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }
        public static void DumpLog(StreamReader r)
        {
            // While not at the end of the file, read and write lines.
            String line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            r.Close();
        }



    }
}
