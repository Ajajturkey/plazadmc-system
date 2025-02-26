using Line.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Line.Models;
using System.IO;

namespace Line.Services
{
    public partial interface IUploadService
    {

        int UploadImage(HttpPostedFileBase httpPostedFile, URLType type, int dbId = 0);

        Data.Picture UploadImage(byte[] fileBinary, string fileName, string fileExtension, string contentType, URLType upload, int pictureId = 0);

        int InsertPicture(Data.Picture picture);

        void DeletePicture(int id);

        string GetPicture(int id);

        void DeleteImage(string path);

       
    }
}
