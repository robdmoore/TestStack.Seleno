using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace TestStack.Seleno.AcceptanceTests.Web.Scripts
{
    public class jquery_delayed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.Sleep(2000);

            var assembly = typeof (MvcApplication).Assembly;
            var resourceNames = assembly.GetManifestResourceNames();
            var resourceName = resourceNames.First(x => x.EndsWith("jquery-2.0.3.min.js", true, CultureInfo.InvariantCulture));
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(resourceStream))
            {
                Response.Write(reader.ReadToEnd());
            }
            Response.ContentType = "text/javascript";
        }
    }
}