using Line.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Line.Services
{
    public class UploadService : IUploadService
    {
        private readonly DBEntities _Repository;
        private readonly IWorkContext _workcontext;

        public UploadService(IWorkContext workcontext)
        {
            _workcontext = workcontext;
            _Repository = new DBEntities();
        }
       
        public int UploadImage(HttpPostedFileBase httpPostedFile, URLType type, int pictureId = 0)
        {
            //    //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";

            if (httpPostedFile == null)
                throw new ArgumentException("No file uploaded");


            stream = httpPostedFile.InputStream;
            fileName = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
            contentType = httpPostedFile.ContentType;

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(httpPostedFile.FileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    default:
                        break;
                }
            }


            string filename = "";
             filename = type.ToString()  + fileExtension;
            

            var Websubpath = UploadStatics.GetURLByType(type);
            string webPath = Path.Combine(Websubpath, filename);

            var Drivefullpath = HttpRuntime.AppDomainAppPath + webPath.Substring(1).Replace("/","\\");

            if (File.Exists(Drivefullpath))
            {
                for (int i = 0; i < 500; i++)
                {
                    filename = type.ToString() + i.ToString()  + fileExtension;
                    Drivefullpath = HttpRuntime.AppDomainAppPath + Path.Combine(Websubpath, filename).Replace("/", "\\");
                    webPath = Path.Combine(Websubpath, filename);
                    if (!File.Exists(Drivefullpath))
                    {
                        break;
                    }
                }
              
            }
  
            File.WriteAllBytes(Drivefullpath, fileBinary);
            
            int id;

            if (pictureId == 0)
            {
              
                   id = InsertPicture(new Picture { url = webPath, title= filename, alt= fileName });
            }
            else
            {
                id = pictureId;
                UpdatePicture(new Picture { Id = pictureId, url = webPath, title = filename, alt = fileName, data = "" });
            }

            return id;
            
        }

        public Picture UploadImage( byte[] fileBinary, string fileName, string fileExtension, string contentType, URLType upload,int pictureId = 0)
        {
      
            

        
            string filename = "";
            filename = fileName.Split('.')[0] + fileExtension;


            var Websubpath = UploadStatics.GetURLByType(upload);

            string webPath = Path.Combine(Websubpath, filename);

            var Drivefullpath = HttpRuntime.AppDomainAppPath + webPath.Substring(1).Replace("/", "\\");

            if (File.Exists(Drivefullpath))
            {
                for (int i = 0; i < 500; i++)
                {
                    filename = fileName + i.ToString() + fileExtension;
                    Drivefullpath = HttpRuntime.AppDomainAppPath + Path.Combine(Websubpath, filename).Replace("/", "\\");
                    webPath = Path.Combine(Websubpath, filename);
                    if (!File.Exists(Drivefullpath))
                    {
                        break;
                    }
                }

            }

            File.WriteAllBytes(Drivefullpath, fileBinary);

            var model = new Picture { url = webPath, title = filename, alt = fileName };

            int id;

            if (pictureId == 0)
            {
                id = InsertPicture(model);
                model.Id = id;
            }
            else
            {
                model.Id = pictureId;
                UpdatePicture(model);
            }

            return model;

        }


        public int InsertPicture(Picture picture )
        {
            if (picture != null)
            {
                var pic = _Repository.PictureSet.Add(new Picture {
                    url = picture.url,
                    alt = picture.alt,
                    data = "",
                    thumbUrl = "",
                    title = picture.title
                });
                _Repository.EntityAdded(pic);

                return pic.Id;
            }
            return 0;
        }

        public void UpdatePicture(Picture item)
        { 
            if (item.Id != 0)
            {
                var pic = _Repository.PictureSet.Find(item.Id);

                if (pic.url != item.url)
                {
                    DeleteImage(pic.url);
                }
                pic.url = item.url;
                pic.alt = item.alt;
                pic.data = "";
                pic.thumbUrl = item.thumbUrl;
                pic.title = item.title;


                _Repository.EntityEdited(pic);
            }
        }


        public void DeletePicture(int id) {

            var pic = _Repository.PictureSet.Find(id);
            if (pic != null)
            {

                DeleteImage(pic.url);

               var  dbitem =_Repository.PictureSet.Remove(pic);
                _Repository.EntityDeleted(dbitem);
            }
        }

        public string GetPicture(int id)
        {
            var host = HttpContext.Current.Request.Url.Host;
            if (host == "localhost")
            {
                host = HttpContext.Current.Request.Url.Authority;
            }

            host = "http://" + host;

            var pic = _Repository.PictureSet.Find(id);
            if (pic != null)
            {
                return host + pic.url;
            }
            else
            {
                return host + "/images/no-image-available.png";
            }
        }

        public void DeleteImage(string path) {

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          

        }


    }
}
