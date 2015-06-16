using System;
using System.Reflection;
using Ninject;
using TaxRay.Contracts;
using TaxRay.Data;
using TaxRay.Data.Helpers;

namespace TaxRay.Web
{
    public static class NinjectConfig
    {
        public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            RegisterServices(kernel);

            return kernel;
        });

        private static void RegisterServices(KernelBase kernel)
        {
            kernel.Bind<ITaxRayUow>()
             .To<TaxRayUow>();
            kernel.Bind<IRepositoryProvider>()
             .To<RepositoryProvider>();
            //kernel.Bind<>()
            // .To<>();
        }
    }
}