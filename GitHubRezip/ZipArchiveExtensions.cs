using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using Ionic.Zip;

namespace GitHubRezip
{
    public static class ZipArchiveExtensions
    {
        public static void AddDirectory(this ZipArchive zipArchive, string directoryPath, string directoryNameInArchive = "")
        {
            var directoryInfo = new DirectoryInfoWrapper(new DirectoryInfo(directoryPath));
            zipArchive.AddDirectory(directoryInfo, directoryNameInArchive);
        }

        public static void AddDirectory(this ZipArchive zipArchive, DirectoryInfoBase directory, string directoryNameInArchive)
        {
            bool any = false;
            foreach (var info in directory.GetFileSystemInfos())
            {
                any = true;
                var subDirectoryInfo = info as DirectoryInfoBase;
                if (subDirectoryInfo != null)
                {
                    string childName = ForwardSlashCombine(directoryNameInArchive, subDirectoryInfo.Name);
                    zipArchive.AddDirectory(subDirectoryInfo, childName);
                }
                else
                {
                    zipArchive.AddFile((FileInfoBase)info, directoryNameInArchive);
                }
            }

            if (!any)
            {
                // If the directory did not have any files or folders, add a entry for it
                zipArchive.CreateEntry(EnsureTrailingSlash(directoryNameInArchive));
            }
        }

        private static string ForwardSlashCombine(string part1, string part2)
        {
            return Path.Combine(part1, part2).Replace('\\', '/');
        }

        public static void AddFile(this ZipArchive zipArchive, string filePath, string directoryNameInArchive = "")
        {
            var fileInfo = new FileInfoWrapper(new FileInfo(filePath));
            zipArchive.AddFile(fileInfo, directoryNameInArchive);
        }

        public static void MoveDirectoriesOneLevelUp(this ZipArchive zipArchive)
        {
            foreach (var zipArchiveEntry in zipArchive.Entries)
            {
                zipArchiveEntry.Delete();
            }
       }

        public static void AddFile(this ZipArchive zipArchive, FileInfoBase file,  string directoryNameInArchive)
        {
            Stream fileStream = null;
            try
            {
                fileStream = file.OpenRead();
            }
            catch (Exception ex)
            {
                // tolerate if file in use.
                // for simplicity, any exception.
                Console.WriteLine(String.Format("{0}, {1}", file.FullName, ex));
                return;
            }

            try
            {
                string fileName = ForwardSlashCombine(directoryNameInArchive, file.Name);
                ZipArchiveEntry entry = zipArchive.CreateEntry(fileName, CompressionLevel.Fastest);
                entry.LastWriteTime = file.LastWriteTime;

                using (Stream zipStream = entry.Open())
                {
                    fileStream.CopyTo(zipStream);
                }
            }
            finally
            {
                fileStream.Dispose();
            }
        }

        public static void AddFileContent(this ZipArchive zip, string fileName, string fileContent)
        {
            ZipArchiveEntry entry = zip.CreateEntry(fileName, CompressionLevel.Fastest);
            using (var writer = new StreamWriter(entry.Open()))
            {
                writer.Write(fileContent);
            }
        }
        public static void AddFileContent(this ZipArchive zip, string fileName, Stream fileContent)
        {
            ZipArchiveEntry entry = zip.CreateEntry(fileName, CompressionLevel.Fastest);
            using (Stream zipEntryStream = entry.Open())
            {
                //Copy the attachment stream to the zip entry stream
                fileContent.CopyTo(zipEntryStream);
            }

        }
        private static string EnsureTrailingSlash(string input)
        {
            return input.EndsWith("/", StringComparison.Ordinal) ? input : input + "/";
        }
    }
}