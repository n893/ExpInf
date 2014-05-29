using System;
using System.Web.Mvc;
using DAL;
using Ninject;

namespace ExperienceInfo.ControllerFactories
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;
        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<ISkillRepository>().To<SkillRepository>();
            _ninjectKernel.Bind<ICategoryRepository>().To<CategoryRepository>();
        }

	    protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
		    Type controllerType)
	    {
		    return controllerType == null
			    ? null
			    : (IController) _ninjectKernel.Get(controllerType);
	    }
    }
}