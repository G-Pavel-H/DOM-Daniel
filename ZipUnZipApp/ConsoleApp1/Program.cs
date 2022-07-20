using System.Globalization;
using System.IO.Compression;
using Aspose.Zip;
using Aspose.Zip.Gzip;
using Aspose.Zip.Saving;
using Aspose.Zip.Tar;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace ConsoleApp1
{
    public class SharpTest
    {
        public static void Main(string[] args)
        {
           start:
            
                try
                {
                    Console.WriteLine("Please choose: \n 1.Compress \n 2.Extract \n (Enter 1 or 2)");
                    var inputStart = Console.ReadLine();
                    var inputChoice = Convert.ToInt64(inputStart);

              
                    if (inputChoice == 1)
                    {
                        choice1:
                        
                            Console.WriteLine("Please specify type of compression, source (with double backslash) and target paths in the format of: type source target.");

                            var inputSecond = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputSecond)) goto choice1;
                            var keyWords = inputSecond.Split(' ');
                            var compressionType = keyWords[0].ToLower();
                            var source = keyWords[1];
                            var target = keyWords[2];

                            if (Directory.Exists(source) || File.Exists(source))
                            {
                                switch (compressionType)
                                {
                                    case "tar": Tarring(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                    case "zip": Zipping(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                    case "targz": TarringGzip(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                }

                                
                            }
                            else
                            {
                                Console.WriteLine("Source invalid, try again");
                                goto choice1;
                            }
                            

                        

                        

                    }
                    else if (inputChoice == 2)
                    {
                        choice2:
                        
                            Console.WriteLine("Please specify type of Extraction, source (with double backslash) and target paths in the format of: type source target.");

                            var inputSecond = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputSecond)) goto choice2;
                            var keyWords = inputSecond.Split(' ');
                            var compressionType = keyWords[0].ToLower();
                            var source = keyWords[1];
                            var target = keyWords[2];

                            if (Directory.Exists(source) || File.Exists(source))
                            {
                                switch (compressionType)
                                {
                                    case "untar": UnTar(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                    case "unzip": Unzipping(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                    case "untargz": UnTarGzip(source, target);
                                        Console.WriteLine("Done!");
                                        break;
                                }

                                
                            }
                            else
                            {
                                Console.WriteLine("Source invalid, try again");
                                goto choice2;
                            }
                        
                        
                        
                        
                    }
                    else
                    {
                        Console.WriteLine("Try Again, incorrect input");
                        goto start;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Something went WRONG: " + e.ToString());
                    goto start;
                }

               
            

        }

        public static void UnTarGzip(string path, string path2)
        {
            //using var archive = new GzipArchive(path);
            //// create a file
            //using (var extracted = File.Create(@"data.tar"))
            //{
            //    // open archive
            //    var unpacked = archive.Open();
            //    byte[] b = new byte[8192];
            //    int bytesRead;
        
            //    // write to file
            //    while (0 < (bytesRead = unpacked.Read(b, 0, b.Length)))
            //        extracted.Write(b, 0, bytesRead);

            //}
            //UnTar(@"data.tar", path2);
            //FileInfo currentFile = new FileInfo(path);
            //using FileStream originalFileStream = File.Open(path, FileMode.Open);
            //using FileStream compressedFileStream = File.Create(Path.ChangeExtension(path, ".tar"));
            //using var compressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            //originalFileStream.CopyTo(compressor);
            //originalFileStream.Dispose();
            //UnTar(Path.ChangeExtension(path, ".tar"), path2);
            //File.Delete(Path.ChangeExtension(path, ".tar"));

            using FileStream compressedFileStream = File.Open(path, FileMode.Open);
            var midPath = Path.ChangeExtension(path, null);
            using FileStream outputFileStream = File.Create(midPath);
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            decompressor.CopyTo(outputFileStream);
            outputFileStream.Close();
            compressedFileStream.Close();
            UnTar(midPath, path2);
            
            
            
        }
 
        public static void TarringGzip(string path, string path2)
        {
            var pathToTar = path + ".tar";
            Tarring(path, pathToTar);
            using FileStream originalFileStream = File.Open(pathToTar, FileMode.Open);
            using FileStream compressedFileStream = File.Create(path2);
            using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
            originalFileStream.CopyTo(compressor);
            originalFileStream.Dispose();
            File.Delete(pathToTar);
        }

        public static void Tarring(string path, string path2)
        {
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {

                using (FileStream tarFile = File.Open("archive.tar", FileMode.Create))
                {
                    using (var archive = new TarArchive())
                    {
                        archive.CreateEntries(new DirectoryInfo(path), false);
                        archive.Save(path2);
                    }
                }

            }

            else
            {
                using (var archive = new TarArchive())
                {
                    archive.CreateEntry(path, File.OpenRead(path));
                    archive.Save(path2);
                }
            }

        }
        
        public static void UnTar(string path, string path2)
        {
            FileAttributes attr = File.GetAttributes(path);


            using (var archive = new TarArchive(path))
            {
                if (attr.HasFlag(FileAttributes.Directory) || archive.Entries.Count > 1)
                { 
                    archive.ExtractToDirectory(path2);
                }
                else
                {   
                    var fileName = archive.Entries[0].Name;
                    var indexOfDot = fileName.LastIndexOf('.');
                    var extension = fileName.Substring(indexOfDot, fileName.Length-indexOfDot);

                    Console.WriteLine(path2 + extension);

                    var indexOfDotPath2 = path2.LastIndexOf('.');
                    var whichToCheck = path2.Length - indexOfDotPath2;
                    
                    var result = path2.Substring(path2.Length - whichToCheck);

                    if (result.Equals(extension))
                    {
                        archive.Entries[0].Extract(path2);

                    }
                    else
                    {
                        archive.Entries[0].Extract(path2 + extension);
                    }
                }

            }

        }

        public static void Zipping(string path, string path2)
        {
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                ZipFile.CreateFromDirectory(path, path2);
            }
            else
            {
                var newFolderPath = Path.ChangeExtension(path, null); 
                Console.WriteLine(newFolderPath);
                Directory.CreateDirectory(newFolderPath);
                File.Copy(path, Path.Combine(newFolderPath, Path.GetFileName(path)));
                ZipFile.CreateFromDirectory(newFolderPath, path2);

                Directory.Delete(newFolderPath, true);
            }

        }
        public static void Unzipping(string path1, string path2)
        {
            ZipFile.ExtractToDirectory(path1, path2);
        }

    }

}