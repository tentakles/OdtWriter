using System;
using System.Collections.Generic;
using OdtWriter;

namespace OdtWriterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string templatefilename = @"c:\temp\odt\mall.odt";
            string resultfilename = @"c:\temp\odt\result.odt";

            var data = new List<OdtData>
            {
                new OdtData {Name = "RUBRIK1", Data = "Hejsan hoppsan"},
                new OdtData {Name = "TEXT1", Data = "Hejsan 1hoppsan"},
                new OdtData {Name = "TEXT2", Data = "Hejsan2 hoppsan"},
                new OdtData {Name = "TEXT3", Data = "Hejsan3 hoppsan"}
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
