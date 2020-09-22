using RestSharp;
using System;
using System.IO;


namespace LoggerClass
{
    public abstract class LogBase
    {

        public abstract void Log(RestClient client, IRestRequest request, IRestResponse response);
    }
    public class Logger : LogBase
    {

        private string CurrentDirectory { get; set; }

        private string FileName { get; set; }

        private string FilePath { get; set; }

        private string File { get; set; }


        public Logger()
        {
            this.CurrentDirectory = Directory.GetCurrentDirectory();
            //this.FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_log.txt";
            this.FileName = "log.txt";
            this.FilePath = this.CurrentDirectory + "/logs/";
            this.File = this.CurrentDirectory + "/logs/" + this.FileName;
        }

        private void CreateIfFolderIsMisisng(string FilePath)
        {

            bool folderExists = Directory.Exists(FilePath);
            if (!folderExists)
            {
                Directory.CreateDirectory(FilePath);

            }

        }

        public override void Log(RestClient client, IRestRequest request, IRestResponse response)
        {

            CreateIfFolderIsMisisng(this.FilePath);

            using System.IO.StreamWriter w = System.IO.File.AppendText(this.File);
            var requestToLog = new
            {
                url = client.BaseUrl,
                method = request.Method,

            };

            string requestParameter = null;


            foreach (var parameter in request.Parameters)
            {
                requestParameter = parameter.ToString();
            }


            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,

            };

            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("Request message is :{0}", requestToLog);
            w.WriteLine("---------------------------------------------------");
            w.WriteLine("Request parameters is :{0}", requestParameter);
            w.WriteLine("---------------------------------------------------");
            w.WriteLine("Response message is :{0}", responseToLog);
            w.WriteLine("---------------------------------------------------");
        }
    }
}
