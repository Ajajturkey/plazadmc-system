using Autofac;
using AutoMapper;
using Line.Data;
using Line.Services;
using Line;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Line.Models
{
    public static class Date {
        public static string DateStyle = "dd/MM/yyyy";
        public static string DefaultStyle = "MM/dd/yyyy";
        public static string DotsStyle = "dd.MM.yyyy";
        public static string ShortDate = "dd MMMM, yyyy";
        public static string TimeStyle = "dd/MM/yyyy";

        public static string ToFString(this DateTime source)
        {
            return source.ToString(DateStyle);
        }


        public static DateTime GetDateTime(string date)
        {
            DateTime returndate;
            if (DateTime.TryParseExact(date, new string[]{ Date.DateStyle, Date.DefaultStyle,Date.DotsStyle } , CultureInfo.InvariantCulture, DateTimeStyles.None, out returndate))
            {
                return returndate.AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).AddHours(DateTime.Now.Hour);
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
    public static class CommonExtending
    {
        public static string GetCustomers(this Reservations item)
        {
            string result = "";

            foreach (var hotel in item.Rhotel)
            {
                var b = hotel.RHRoom.FirstOrDefault(d => !string.IsNullOrEmpty(d.guset));
                if (b != null)
                {
                    return b.guset;
                }
            }
           
            var y = item.Transfers.FirstOrDefault(d => !string.IsNullOrEmpty(d.customer));
            if (y != null)
            { return y.customer; }
            var z = item.Tour.FirstOrDefault(d => !string.IsNullOrEmpty(d.customer));
            if (z != null)
            { return z.customer; }
            var m = item.ExtraService.FirstOrDefault(d => !string.IsNullOrEmpty(d.customer));
            if (m != null)
            { return m.customer; }

            var c = item.Flight.FirstOrDefault(d => !string.IsNullOrEmpty(d.customer));
            if (c != null)
            { return c.customer; }

            return result;
        }

        public static SettingViewModel ToModel(this Settings item)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Settings, SettingViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<Settings, SettingViewModel>(item);
            return dest;
        }
        public static Settings ToDBObject(this SettingViewModel item)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SettingViewModel, Settings>();
            });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<SettingViewModel, Settings>(item);
            return dest;
        }
        //public static Webshipment ToModel(this Shipment item)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Shipment, Webshipment>();
        //    });
        //    IMapper mapper = config.CreateMapper();
        //    var dest = mapper.Map<Shipment, Webshipment>(item);
        //    return dest;
        //}
        //public static Shipment ToDBObject(this Webshipment item)
        //{
        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<Webshipment, Shipment>();
        //    });
        //    IMapper mapper = config.CreateMapper();
        //    var dest = mapper.Map<Webshipment, Shipment>(item);
        //    return dest;
        //}

        public static MiniProfileModel ToMiniModel(this Members source)
        {
            var _service = DependencyManager.Scope.Resolve<IUploadService>();
            var model = new MiniProfileModel();

            model.Fullname = source.FirstName + " " + source.LastName;
            return model;

        }
    }
}