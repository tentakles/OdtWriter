using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using OdtWriter;

namespace OdtWriterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string templatefilename = @"c:\temp\odt\mall.odt";
            string resultfilename = @"c:\temp\odt\result.odt";


            string[] list = Enumerable.Range(1, 2000).Select(x => "Min fina rad " + x).ToArray();

            var data = new List<OdtData>
            {
                new OdtDataSimple {Name = "RUBRIK1", Data = "Hejsan hoppsan"},
                new OdtDataSimple {Name = "TEXT1", Data = "Hejsan 1\nhoppsan\n"},
                new OdtDataSimple {Name = "TEXT2", Data = "Hejsan2 hoppsan"},
                new OdtDataArray {Name = "TEXT3", Data = list}
            };
            try
            {
                var writer = new OdtWriter.OdtWriter();
                writer.SetTemplate(templatefilename);
                writer.SetData(data);
                writer.WriteToFile(resultfilename,true);

                Console.WriteLine("Fil skriven: " + resultfilename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Det blev fel: " + e.ToString());
            }

            Console.WriteLine("Tryck valfri tangent för att avsluta");
            Console.ReadKey();

        }
    }
}
