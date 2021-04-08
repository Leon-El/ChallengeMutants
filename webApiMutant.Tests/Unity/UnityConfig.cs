using AppRepository;
using AppServices;
using Infraestructura.IRepository;
using Infraestructura.IService;
using Microsoft.Practices.Unity;
using System;


namespace webApiMutant.Tests.Unity
{
    internal static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterComponents(IUnityContainer container)
        {


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

        }
    }
}
