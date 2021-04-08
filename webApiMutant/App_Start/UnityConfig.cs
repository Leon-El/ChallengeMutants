using AppRepository;
using AppServices;
using Infraestructura.IRepository;
using Infraestructura.IService;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace webApiMutant
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();


            //Repositorios
            container.RegisterType<IPersonRepository, PersonRepository>();


            // Servicios

            container.RegisterType<IMutantService, MutantService>(
               new InjectionConstructor(
               new ResolvedParameter<IPersonRepository>()
            ));


            container.RegisterType<IStatsService, StatsService>(
               new InjectionConstructor(
               new ResolvedParameter<IPersonRepository>()
            ));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}