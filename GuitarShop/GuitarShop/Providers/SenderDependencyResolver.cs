using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

namespace GuitarShop.Providers
{
    public class SenderDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public SenderDependencyResolver()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ISender>().To<EmailSender>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}