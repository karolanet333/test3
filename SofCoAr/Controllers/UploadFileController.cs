using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using System;
using System.Web.Http.Cors;
using System.Reflection;

namespace SofCoAr.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadFileController : ApiController
    {
        [HttpGet]
        public FileData[] LoadJsonFiles(string customer, string service, string project, string hito)
        {
            string folder = "cust_" + customer + "_serv_" + service + "_proj_" + project + "_hito_" + hito;
            string path = "";
            List<FileData> rpta = new List<FileData>();

            //path = HttpContext.Current.Server.MapPath("~/ImageStorage/" + folder); //Path
            path = Assembly.GetExecutingAssembly().Location;
            path = new FileInfo(path).Directory.FullName;
            path = path + "\\ImageStorage";

            string[] files = new string[0];
            bool thereAreFiles = true;

            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception e)
            {
                thereAreFiles = false;
            }

            //String AsBase64String;

            if (thereAreFiles)
            {

                foreach (string f in files)
                {
                    /*using (StreamReader sr = new StreamReader(f))
                    {
                        String AsString = sr.ReadToEnd();
                        byte[] AsBytes = new byte[AsString.Length];
                        Buffer.BlockCopy(AsString.ToCharArray(), 0, AsBytes, 0, AsBytes.Length);
                        AsBase64String = Convert.ToBase64String(AsBytes);
                    }*/

                    Byte[] bytes = File.ReadAllBytes(f);
                    String file = Convert.ToBase64String(bytes);

                    //var file8 = Encoding.UTF8.GetBytes(f);
                    //string file64 = Convert.ToBase64String(file8);

                    string fileName = f.Split('\\').Last();

                    FileData tmp = new FileData();
                    tmp.FileName = fileName;
                    tmp.File = file;

                    tmp.Customer = int.Parse(customer);
                    tmp.Service = int.Parse(service);
                    tmp.Project = int.Parse(project);
                    tmp.Hito = int.Parse(hito);

                    rpta.Add(tmp);
                }
            }

            return rpta.ToArray<FileData>();

        }

        //TODO: Copiar los archivos a una carpeta TMP antes de eliminarlos.
        //Eliminarlos y luego copiar los nuevos
        //Si todo eso no dio error, eliminar los TMP, si dio error, rollbackear todo
        [HttpPost]
        public HttpResponseMessage UploadJsonFiles([FromBody] FileData[] filesData)
        {
            string folder = "";

            //HttpContext.Current solo se puede usar dentro de un sitio web
            //string path = HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path
            string path = Assembly.GetExecutingAssembly().Location;
            path = new FileInfo(path).Directory.FullName;
            path = path + "\\ImageStorage";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //limpiar directorio
            if (filesData.Length > 0)
            {
                folder = "cust_" + filesData[0].Customer + "_serv_" + filesData[0].Service + "_proj_" + filesData[0].Project + "_hito_" + filesData[0].Hito;

                //string fullDirectoryPath = Assembly.GetExecutingAssembly().Location + "\\ImageStorage\\" + folder;
                //string fullDirectoryPath = HttpContext.Current.Server.MapPath("~/ImageStorage/" + folder);
                string fullDirectoryPath = Assembly.GetExecutingAssembly().Location;
                fullDirectoryPath = new FileInfo(fullDirectoryPath).Directory.FullName;
                fullDirectoryPath = fullDirectoryPath + "\\ImageStorage" + folder;

                if (!System.IO.Directory.Exists(fullDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(fullDirectoryPath); //Create directory if it doesn't exist
                }

                System.IO.DirectoryInfo di = new DirectoryInfo(fullDirectoryPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
            }


            //grabar los archivos subidos
            foreach (var fd in filesData)
            {
                int startAt = 0;
                string onlyBase64 = fd.File;
                if (fd.File.IndexOf(";base64,") > 0)
                {
                    startAt = fd.File.IndexOf(";base64,") + ";base64,".Length;
                    onlyBase64 = fd.File.Substring(startAt);
                }

                SaveFile(onlyBase64, fd.FileName, folder);
            }

            return new HttpResponseMessage();
        }

        /*[HttpPost]
        private HttpResponseMessage UploadJsonFile([FromBody] FileData fileData)
        {
            int startAt = 0;
            string onlyBase64 = fileData.File;
            if (fileData.File.IndexOf(";base64,") > 0)
            {
                startAt = fileData.File.IndexOf(";base64,") + ";base64,".Length;
                onlyBase64 = fileData.File.Substring(startAt);
            }
            
            string folder = "cust_" + fileData.Customer + "_serv_" + fileData.Service + "_proj_" + fileData.Project + "_hito_" + fileData.Hito;

            SaveFile(onlyBase64, fileData.FileName, folder);

            return new HttpResponseMessage();
        }*/

        private bool SaveFile(string base64File, string fileName, string folder)
        {
            //string path = HttpContext.Current.Server.MapPath("~/ImageStorage/" + folder); //Path

            string path = Assembly.GetExecutingAssembly().Location;
            path = new FileInfo(path).Directory.FullName;
            path = path + "\\ImageStorage";

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //set the image path
            string filePath = Path.Combine(path, fileName);

            byte[] imageBytes = Convert.FromBase64String(base64File);

            File.WriteAllBytes(filePath, imageBytes);

            return true;
        }

    }

    public class FileData
    {
        public string FileName { get; set; }
        public string File { get; set; }
        public int Customer { get; set; }
        public int Service { get; set; }
        public int Project { get; set; }
        public int Hito { get; set; }
    }


}
