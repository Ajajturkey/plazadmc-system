using Line.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Models
{

    public class Result
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
    }

    public class LocaleStringResourcesModel
    {
        public LocaleStringResourcesModel()
        {
            Resources = new List<LocaleStringResource>();
        }
        public int LanguageId { get; set; }
        public int RecourceId { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Value { get; set; }

        public List<LocaleStringResource> Resources { get; set; }
    }

    public class MiniProfileModel
    {
        public string Fullname { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public string LogoUrl { get; set; }
    }
   

}