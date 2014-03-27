using System.IO;
using System.IO.Compression;

namespace OdtWriter
{
    class OdtWriterFileHandling
    {
        
        public static void UnzipFileToDirectory(string filename, string directory)
        {
            using (ZipArchive archive = ZipFile.Open(filename, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(directory);
            }
        }

        public static string GetTempDirectory()
        {
            string path = Path.GetRandomFileName();
            string completepath = Path.Combine(Path.GetTempPath(), path);
            Directory.CreateDirectory(completepath);
            return completepath;
        }

        public static void RemoveDirectory(string dir)
        {
            DirectoryInfo directory = new DirectoryInfo(dir);

            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);

            Directory.Delete(dir);
        }


        internal static void WriteDirectoryToZip(string tempdir, string filename)
        {
            ZipFile.CreateFromDirectory(tempdir, filename);
        }

        internal static void RemoveIfExists(string filename)
        {
            if(File.Exists(filename))
                File.Delete(filename);         
        }
    }
}
