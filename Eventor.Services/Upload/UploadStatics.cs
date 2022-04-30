using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Line.Services
{

    public static class MimeTypes
    {
        #region application/*

        public const string ApplicationForceDownload = "application/force-download";

        public const string ApplicationJson = "application/json";

        public const string ApplicationOctetStream = "application/octet-stream";

        public const string ApplicationPdf = "application/pdf";

        public const string ApplicationRssXml = "application/rss+xml";

        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

        public const string ApplicationXZipCo = "application/x-zip-co";

        #endregion application/*


        #region image/*

        public const string ImageBmp = "image/bmp";

        public const string ImageGif = "image/gif";

        public const string ImageJpeg = "image/jpeg";

        public const string ImagePJpeg = "image/pjpeg";

        public const string ImagePng = "image/png";

        public const string ImageTiff = "image/tiff";

        #endregion image/*


        #region text/*

        public const string TextCss = "text/css";

        public const string TextCsv = "text/csv";

        public const string TextJavascript = "text/javascript";

        public const string TextPlain = "text/plain";

        public const string TextXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        #endregion text/*
    }
    public static class UploadStatics
    {
        /// <summary>        
        /// Events/{eventID}/Particepent/{ID}
        /// </summary>
        public static string GetURLByType(URLType type) {
            string path = "";

            

            if (!Directory.Exists(HttpRuntime.AppDomainAppPath + "images"))
            {
                Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "images");
            }

            switch (type)
            {
                case URLType.logo:
                    path += "/images/logo/";
                    if (!Directory.Exists(HttpRuntime.AppDomainAppPath + "/images/sliders/"))
                    {
                        Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "/images/sliders/");
                    }
                    break;
                case URLType.slider:
                    path += "/images/sliders/";
                    if (!Directory.Exists(HttpRuntime.AppDomainAppPath + "/images/sliders/"))
                    {
                        Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "/images/sliders/");
                    }
                    break;
                

                default:
                    path += "/images/upload/";
                    if (!Directory.Exists(HttpRuntime.AppDomainAppPath + "/images/upload/"))
                    {
                        Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "/images/upload/");
                    }
                    break;
            }
            return path;
        }

    }

    public enum URLType
    {
        slider=0,
        logo=3,
        shipments=5,
        upload = 6
    }
}
