using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace AgroExpressAPI.FileHandling
{
    public class FileHandlingClass
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileHandlingClass(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public FileHandlingClass()
        {
            
        }
        public string FileHandlingMethod(IFormCollection files = null)
        {
            string fileName = null;
            if (files != null && files.Count > 0)
            {
                string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (!Directory.Exists(imageDirectory)) Directory.CreateDirectory(imageDirectory);
                foreach (var file in files.Files)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    var extension = info.Extension;
                    string[] extensions = new string[] { ".png", ".jpeg", ".jpg", ".gif", ".tif" };
                    bool check = false;
                    foreach (var ext in extensions)
                    {
                        if (extension.Equals(ext)) check = true;
                    }
                    if (check == false) return null; ;
                    if (file.Length > 20480) return null;
                    string image = Guid.NewGuid().ToString() + info.Extension;
                    string path = Path.Combine(imageDirectory, image);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    fileName = (image);
                }
            }
            return fileName;
        }
    }
}
