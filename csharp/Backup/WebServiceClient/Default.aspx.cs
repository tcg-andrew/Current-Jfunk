using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebServiceClient.ProductionServiceReference;

namespace WebServiceClient
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductionServiceReference.ProductionServiceClient client = new ProductionServiceReference.ProductionServiceClient("BasicHttpBinding_IProductionService1");
            client.Open();
            mesdrawinggetresult result = client.getmesdrawing("CRD", "583");
            client.Close();

            Response.ContentType = "application/acad";
            Response.AddHeader("Content-Disposition", "attachment;filename=download.dwg");
            Response.BinaryWrite(result.epicor[0].filedata);
        }
    }
}