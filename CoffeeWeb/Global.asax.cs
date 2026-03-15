using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CoffeeWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["Today"] = 0L;
            Application["Yesterday"] = 0L;
            Application["ThisWeek"] = 0L;
            Application["LastWeek"] = 0L;
            Application["ThisMonth"] = 0L;
            Application["LastMonth"] = 0L;
            Application["AllTime"] = 0L;
            Application["VisitorsOnline"] = 0;
           
        }
        void Session_Start(object sender, EventArgs e)
        {

            Session.Timeout = 1;
            Application.Lock();
            Application["visitors_online"] = Convert.ToInt32(Application["visitors_online"]) + 1;
            Application.UnLock();

            try
            {
                var item = CoffeeWeb.Models.Common.StatisticalUser.StaProc();
                if (item != null)
                {
                    Application["Today"] = long.Parse("0" + item.Today);
                    Application["Yesterday"] = long.Parse("0" + item.Yesterday);
                    Application["ThisWeek"] = long.Parse("0" + item.ThisWeek);
                    Application["LastWeek"] = long.Parse("0" + item.LastWeek);
                    Application["ThisMonth"] = long.Parse("0" + item.ThisMonth);
                    Application["LastMonth"] = long.Parse("0" + item.LastMonth);
                    Application["AllTime"] = long.Parse("0" + item.AllTime);

                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("Error " + ex.Message);
            }
            
        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["visitors_online"] = Convert.ToUInt32(Application["visitors_online"]) - 1;
            Application.UnLock();
        }
        protected void Application_BeginRequest()
        {
            var culture = new System.Globalization.CultureInfo("vi-VN");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
