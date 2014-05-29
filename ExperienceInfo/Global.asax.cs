using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DAL;
using DataContract;
using ExperienceInfo.ControllerFactories;
using WebMatrix.WebData;

namespace ExperienceInfo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
			AreaRegistration.RegisterAllAreas();
			
			
			

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
			
			System.Data.Entity.Database.SetInitializer<SkillInfoContext>(new SkillsContextInitializer());
	        try
	        {
		        new SkillInfoContext().UserProfiles.Find(1);
	        }
	        catch
	        {
		        
	        }

			if (!WebSecurity.Initialized)
				WebSecurity.InitializeDatabaseConnection("DataContract.SkillInfoContext",
					"UserProfile", "UserId", "UserName", autoCreateTables: true);
            
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
    }
}