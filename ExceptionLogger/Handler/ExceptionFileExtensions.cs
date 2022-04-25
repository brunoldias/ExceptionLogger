using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Handler
{
    public static class ExceptionFileExtensions
    {
        private static string _path = $@"C:\log";
        private static string _archiveName = $"\\Application_{DateTime.Now.Date.ToString("ddMMyyyy")}";

        public static void Write(this Exception exception, string directoryDestination = null)
        {
            FileCreate(exception, directoryDestination);
        }
        private static void FileCreate<T>(T data, string directoryDestination = null) where T : class
        {
            if (!string.IsNullOrWhiteSpace(directoryDestination))
                _path = directoryDestination;

            if (!File.Exists(string.Format("{0}{1}", _path, _archiveName)))
            {
                CreateDirectory(_path);
            }
            File.AppendAllText(string.Format("{0}{1}", _path, _archiveName), $"Error : {JsonConvert.SerializeObject(data)} \n Datetime: {DateTime.Now} \n");
        }

        private static void CreateDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Create();
        }
    }
}
