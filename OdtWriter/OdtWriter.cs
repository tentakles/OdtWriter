﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

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

        public void WriteToFile(string filename, bool replaceIfExists)
        {
            string tempdir = OdtWriterFileHandling.GetTempDirectory();
            OdtWriterFileHandling.UnzipFileToDirectory(_templateFile, tempdir);
            var completepath = Path.Combine(tempdir, InnerFilename);

            ReplaceXml(completepath);

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


        /*

        foreach (var i in _data)
        {
            string match = String.Format(TemplateMatchFormatString, i.Name);
            var simple = i as OdtDataSimple;
            var arr = i as OdtDataArray;
        }

        //
      

        //).Elements(XName.Get("office", "body"));

        //Descendants("body");//.Elements("office:text");

        //    "office:body").Elements("office:text");

        /*
        List<Student> students = (from student in doc.Element("Students").Elements("student")
                                  select new Student
                                  {
                                      Name = student.Attribute("name"),
                                      Class = student.Attribute("class")
                                  }
                      ).ToList();
         */



        private void ReplaceText(string completepath)
        {
            string text = File.ReadAllText(completepath);

            foreach (var i in _data)
            {
                string match = String.Format(TemplateMatchFormatString, i.Name);
                var simple = i as OdtDataSimple;
                var arr = i as OdtDataArray;

                if (simple != null)
                {
                    text = text.Replace(match, simple.Data);
                }
                else if (arr != null)
                {

                }
            }

            File.WriteAllText(completepath, text);
        }

        //public byte[] Download()
        //{
        //    throw new NotImplementedException();
        //    return null;
        //}
    }
}
