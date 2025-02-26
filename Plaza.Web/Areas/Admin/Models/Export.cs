using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Line.Models
{
    public class Export
    {
        public void ToExcel(HttpResponseBase Response, object clientsList,string filename)
        {
            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = clientsList;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename="+ filename + "_"+DateTime.Now.ToString("ddMMyyyy")+".xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}