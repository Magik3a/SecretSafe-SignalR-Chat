[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SecretSafe.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SecretSafe.App_Start.NinjectConfig), "Stop")]

namespace SecretSafe.App_Start
{
    using System;
    using System.Web;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using System.Linq;
    using Ninject;
    using Data;
    using Infrastructure;
    using System.Web.Mvc;
    using Ninject.Web.Mvc;
    using global::Common.Constants;
    using global::Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public static class NinjectConfig
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                ObjectFactory.Initialize(kernel);
                RegisterServices(kernel);

                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel
          .Bind<IDbContext>()
          .To<SecretSafeDbContext>()
          .InRequestScope();

            kernel.Bind(typeof(IRepository<>)).To(typeof(EfGenericRepository<>));


            kernel.Bind(b => b.From(Assemblies.DataServices)
            .SelectAllClasses()
            .BindDefaultInterface());

            kernel.Bind<IUserStore<SecretSafeUser>>().To<UserStore<SecretSafeUser>>().WithConstructorArgument("context", kernel.Get<SecretSafeDbContext>());
            kernel.Bind<UserManager<SecretSafeUser>>().ToSelf();
        }
    }
}
