using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;

namespace Line.Models
{
    public class WebEventModel 
    {
        public WebEventModel()
        {
            EventSliders = new List<SlideModel>();
            Gallary = new List<GallaryItemModel>();
        }

        public int Id { get; set; }
        public bool IsLogoEnabled { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public string LogoText { get; set; }

        public bool isHtmlEnabled { get; set; }
        [AllowHtml]
        public string HtmlBody { get; set; }
        public string bodyHeader { get; set; }
        public string bodyDescription { get; set; }

        public bool isExtraHtmlEnabled { get; set; }
        [AllowHtml]
        public string ExtraHtmlBody { get; set; }

        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string twitter { get; set; }
        public bool AddLocationPart { get; set; }
        [AllowHtml]
        public string LocationSideHTML { get; set; }


        public string ThemeTemplate { get; set; }

        public string phone { get; set; }

        public string adress { get; set; }
        public string Email { get; set; }

        public int EventId { get; set; }

        public List<SlideModel> EventSliders { get; set; }

        public List<GallaryItemModel> Gallary { get; set; }
    }

    public enum GallaryType
    {
        Sideslider = 1,
        Gallary = 2
    }

    public class GallaryItemModel
    {
        public int id { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public GallaryType Type { get; set; }
        public int eventId { get; set; }
    }

    public class SlideModel
    {
        public int id { get; set; }
        public string textheader { get; set; }
        public string textDescription { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public int WebEventId { get; set; }
    }


    public class ContactModel 
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string adress { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public int WebEventId { get; set; }
    }

    public class SponsorModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public int WebEventId { get; set; }
        public string Description { get; set; }
    }


    public class DataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }

        public DataTableSearch Search { get; set; }
    }
    [System.Web.Http.ModelBinding.ModelBinder(typeof(DataTableModelBinder))]
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public DataTableOrder[] Order { get; set; }
        public DataTableColumn[] Columns { get; set; }
        public DataTableSearch Search { get; set; }
    }

    public class DataTableResponse
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object[] data { get; set; }
        public string error { get; set; }
    }

    public class DataTableModelBinder : System.Web.Http.ModelBinding.IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(DataTableRequest))
            {
                return false;
            }

            var model = (DataTableRequest)bindingContext.Model ?? new DataTableRequest();

            model.Draw = Convert.ToInt32(GetValue(bindingContext, "draw"));
            model.Start = Convert.ToInt32(GetValue(bindingContext, "start"));
            model.Length = Convert.ToInt32(GetValue(bindingContext, "length"));

            // Search
            model.Search = new DataTableSearch
            {
                Value = GetValue(bindingContext, "search.value"),
                Regex = Convert.ToBoolean(GetValue(bindingContext, "search.regex"))
            };


            // Order
            var o = 0;
            var order = new List<DataTableOrder>();
            while (GetValue(bindingContext, "order[" + o + "].column") != null)
            {
                order.Add(new DataTableOrder
                {
                    Column = Convert.ToInt32(GetValue(bindingContext, "order[" + o + "].column")),
                    Dir = GetValue(bindingContext, "order[" + o + "].dir")
                });
                o++;
            }
            model.Order = order.ToArray();

            // Columns
            var c = 0;
            var columns = new List<DataTableColumn>();
            while (GetValue(bindingContext, "columns[" + c + "].data") != null)
            {
                columns.Add(new DataTableColumn
                {
                    Data = GetValue(bindingContext, "columns[" + c + "].data"),
                    Name = GetValue(bindingContext, "columns[" + c + "].name"),
                    Orderable = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "].orderable")),
                    Searchable = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "].searchable")),
                    Search = new DataTableSearch
                    {
                        Value = GetValue(bindingContext, "columns[" + c + "][search].value"),
                        Regex = Convert.ToBoolean(GetValue(bindingContext, "columns[" + c + "].search.regex"))
                    }
                });
                c++;
            }
            model.Columns = columns.ToArray();

            bindingContext.Model = model;

            return true;
        }

        private string GetValue(System.Web.Http.ModelBinding.ModelBindingContext context, string key)
        {
            var result = context.ValueProvider.GetValue(key);
            return result == null ? null : result.AttemptedValue;
        }
    }

}