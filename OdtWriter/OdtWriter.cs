using System;
using System.Collections.Generic;
using System.IO;

namespace OdtWriter
{
    public class OdtWriter
    {
        private string _templateFile;
        private List<OdtData> _data;
        private const string InnerFilename = "content.xml";
        private const string TemplateMatchFormatString = "${0}$";

        public OdtWriter()
        {

        }

        public OdtWriter(string templateFile, List<OdtData> data)
        {
            SetTemplate(templateFile);
            SetData(data);
        }

        public void SetTemplate(string templateFile)
        {
            _templateFile = templateFile;
        }

        public void SetData(List<OdtData> data)
        {
            _data = data;
        }

        public void WriteToFile(string filename,bool replaceIfExists)
        {
            string tempdir = OdtWriterFileHandling.GetTempDirectory();
            OdtWriterFileHandling.UnzipFileToDirectory(_templateFile, tempdir);
            var completepath = Path.Combine(tempdir, InnerFilename);
            string text = File.ReadAllText(completepath);

            foreach (var i in _data)
            {
                string match = String.Format(TemplateMatchFormatString, i.Name);
                text = text.Replace(match, i.Data);
            }

            File.WriteAllText(completepath, text);

            if(replaceIfExists)
                OdtWriterFileHandling.RemoveIfExists(filename);

            OdtWriterFileHandling.WriteDirectoryToZip(tempdir, filename);
            OdtWriterFileHandling.RemoveDirectory(tempdir);
        }

        public byte[] Download()
        {
            throw new NotImplementedException();
            return null;
        }
    }
}
