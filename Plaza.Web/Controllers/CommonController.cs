using Line.Data;
using Line.Models;
using Line.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Controllers
{
    

    public class CommonController : Controller
    {

        public ILanguageService _LanguageService;
        public ILocalizationService _LocalizationService;
        public IWorkContext _workContext;
        public IConfiqurationService _configurationService;
        public IVisitorService _VisitorService;
        public IUploadService _uploadService;


        public CommonController(ILanguageService languageService, ILocalizationService localizationService,
         IWorkContext workContext, IConfiqurationService configurationService, IVisitorService VisitorService,  IUploadService uploadService)
        {
            this._LanguageService = languageService;
            this._LocalizationService = localizationService;
            this._workContext = workContext;
            this._configurationService = configurationService;
            this._VisitorService = VisitorService;
            this._uploadService = uploadService;
        }

        public ActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _LanguageService.GetLanguageById(langid);
            if (language != null && language.Published)
            {
                _workContext.WorkingLanguage = language;
            }

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

        public ActionResult LanguageSelector()
        {

            var availableLanguages = _LanguageService
                    .GetAllLanguages()
                    .ToList();


            var model = new LanguageSelectorModel
            {
                CurrentLanguageId = _workContext.WorkingLanguage.Id,
                AvailableLanguages = availableLanguages,
            };

            if (model.AvailableLanguages.Count == 1)
                Content("");

            return PartialView(model);
        }

        public ActionResult navheader()
        {
            var visitor = _workContext.CurrentVisitor;
           
            var profile = visitor.ToMiniModel();

            return PartialView(profile);
        }

        [HttpPost]
        //do not validate request token(XSRF)
        public ActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");
            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            MemoryStream ms = new MemoryStream(fileBinary);
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(ms);

            if (originalImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                            // de-rotate:
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }


            var fileExtension = Path.GetExtension(fileName);
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

            Line.Data.Picture picture = _uploadService.UploadImage(CopyImageToByteArray(originalImage), Path.GetFileNameWithoutExtension(fileName), fileExtension, contentType, URLType.upload);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = picture.url
            },
                MimeTypes.TextPlain);
        }

        public static byte[] CopyImageToByteArray(System.Drawing.Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}

