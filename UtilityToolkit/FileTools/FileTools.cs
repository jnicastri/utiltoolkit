using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;


namespace UtilityToolkit.FileTools
{
    public static class FileTools
    {
        public static bool CompressSingleFile(DirectoryInfo directorySelected, string filenameToCompress, string outputPath)
        {
            bool compressed = false;

            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                if (fileToCompress.Name == (filenameToCompress))
                {
                    using (FileStream originalFileStream = fileToCompress.OpenRead())
                    {
                        if ((File.GetAttributes(fileToCompress.FullName) &
                           FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                        {
                            using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                            {
                                using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                                   CompressionMode.Compress))
                                {
                                    originalFileStream.CopyTo(compressionStream);
                                    compressed = true;
                                }
                            }
                            //FileInfo info = new FileInfo(outputPath + fileToCompress.Name + ".gz");
                        }
                    }
                }
            }

            return compressed;
        }

        public static bool CompressEntireFolder(DirectoryInfo directorySelected, string outputPath)
        {
            // To be implemented

            return false;
        }


        public static void DeleteAllFromDirectory(string dirPath)
        {
            // Delete Files
            Array.ForEach(Directory.GetFiles(dirPath), File.Delete);

            // Delete Directories and their children
            foreach (DirectoryInfo dir in new DirectoryInfo(dirPath).GetDirectories())
                dir.Delete(true);
        }


        public static bool Decompress(FileInfo fileToDecompress)
        {
            bool decompressed = false;

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        decompressed = true;
                    }
                }
            }
            return decompressed;
        }

        public static void UnZip(string path, string outputDirectory)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);

                using (ZipInputStream s = new ZipInputStream(fs))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outputDirectory + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                if (fs != null) { fs.Close(); }
            }
        }

        public static string[] FindFileInDirectoryByPartialFileName(string directoryPath, string partialFileName, string extension)
        {
            //string fileName = String.Empty;
            List<string> filesList = new List<string>();

            foreach (string fl in Directory.GetFiles(directoryPath))
            {
                if (fl.ToLower().Contains(partialFileName.ToLower()))
                {
                    filesList.Add(fl);
                }
            }

            return filesList.ToArray();
        }
    }
}
