using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OdtWriter
{
    public class OdtWriter
    {
        private string _templateFile;
        private List<OdtData> _data;
        private const string InnerFilenameContent = "content.xml";
        private const string InnerFilenameStyles = "styles.xml";
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

        public void WriteToFile(string filename, bool replaceIfExists)
        {
            string tempdir = OdtWriterFileHandling.GetTempDirectory();
            OdtWriterFileHandling.UnzipFileToDirectory(_templateFile, tempdir);
            var completepathcontent = Path.Combine(tempdir, InnerFilenameContent);
            var completepathstyles = Path.Combine(tempdir, InnerFilenameStyles);

            ReplaceXml(completepathcontent);
            ReplaceXml(completepathstyles);

            if (replaceIfExists)
                OdtWriterFileHandling.RemoveIfExists(filename);

            OdtWriterFileHandling.WriteDirectoryToZip(tempdir, filename);
            OdtWriterFileHandling.RemoveDirectory(tempdir);
        }

        private void ReplaceXml(string completepath)
        {
            var xDoc = XDocument.Load(completepath);

            foreach (var idata in _data)
            {
                string match = String.Format(TemplateMatchFormatString, idata.Name);
                var simple = idata as OdtDataSimple;
                var arr = idata as OdtDataArray;

                XElement ie = GetNode(xDoc, match);

                if (ie != null)
                {
                    if (simple != null)
                    {
                        ie.Value = simple.Data;
                    }
                    else if (arr != null)
                    {
                        var lastelement = ie;
                        foreach (var iarr in arr.Data)
                        {
                            var clone = new XElement(lastelement) { Value = iarr };

                            if (lastelement == ie)
                            {
                                lastelement.ReplaceWith(clone);

                            }
                            else
                            {
                                lastelement.AddAfterSelf(clone);
                            }
                            lastelement = clone;
                        }

                    }

                }

            }

            xDoc.Save(completepath);
        }

        private XElement GetNode(XDocument xDoc, string match)
        {
            foreach (var i in xDoc.DescendantNodes())
            {
                var ie = i as XElement;

                if (ie != null && ie.Value == match)
                {
                    return ie;
                }
            }
            return null;
        }

        public byte[] Download()
        {
            string tempfile = Path.GetRandomFileName();
            string temppath = Path.Combine(Path.GetTempPath(), tempfile);
            WriteToFile(temppath, false);
            var result = File.ReadAllBytes(temppath);
            File.Delete(temppath);
            return result;
        }


        //private void ReplaceText(string completepath)
        //{
        //    string text = File.ReadAllText(completepath);

        //    foreach (var i in _data)
        //    {
        //        string match = String.Format(TemplateMatchFormatString, i.Name);
        //        var simple = i as OdtDataSimple;
        //        var arr = i as OdtDataArray;

        //        if (simple != null)
        //        {
        //            text = text.Replace(match, simple.Data);
        //        }
        //        else if (arr != null)
        //        {

        //        }
        //    }

        //    File.WriteAllText(completepath, text);
        //}


    }
}
