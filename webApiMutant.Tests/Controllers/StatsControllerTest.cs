using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using webApiMutant;
using webApiMutant.Controllers;
using Microsoft.Practices.Unity;
using Infraestructura.IService;
using Infraestructura.Model;
using webApiMutant.Models;
using System.Web;
using System.Web.Http.Hosting;

namespace webApiMutant.Tests.Controllers
{
    [TestClass]
    public class StatsControllerTest
    {
        private IUnityContainer container;

        private IStatsService statServ;

        private IMutantService mutantServ;

        [TestInitialize]
        public void TestInitialize()
        {
            HttpContext.Current = new HttpContext(
                       new HttpRequest("", "http://tempuri.org", ""),
                       new HttpResponse(new System.IO.StringWriter())
                   );

            container = Unity.UnityConfig.GetConfiguredContainer();
            statServ = container.Resolve<IStatsService>();
            mutantServ = container.Resolve<IMutantService>();
        }



        [TestMethod]
        public void GetWithoutData()
        {

            StatsController controller = new StatsController(statServ);
            MutantController controllerM = new MutantController(mutantServ);
            controllerM.Delete();

            Stats nulas = controller.Get();

            Assert.AreEqual(0, nulas.count_human_dna);
            Assert.AreEqual(0, nulas.count_mutant_dna);
            Assert.AreEqual(0.0m, nulas.ratio);
        }

        [TestMethod]
        public void GetData()
        {

            MutantController controller = new MutantController(mutantServ);

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey,
                                              new HttpConfiguration());

            controller.Delete();

            PersonDTO persona = new PersonDTO()
            {
                dna = new List<string>() {
                    "ATGCGA",
                    "CAGTGC",
                    "TTATTT",
                    "AGACGG",
                    "GCGTCA",
                    "TCACTG" }
            };

            PersonDTO mutante = new PersonDTO()
            {
                dna = new List<string>() {
                    "ATGCGA",
                    "CAGTGC",
                    "TTATGT",
                    "AGAAGG",
                    "CCCCTA",
                    "TCACTG"}
            };

            PersonDTO mutante2 = new PersonDTO()
            {
                dna = new List<string>() {
                    "ATGCGA",
                    "CAGTGG",
                    "TCCCCT",
                    "ATAGGG",
                    "CCTAAA",
                    "TCATTG"}
            };
            controller.Mutant(persona);
            controller.Mutant(mutante);
            controller.Mutant(mutante2);

            StatsController controllerS = new StatsController(statServ);

            Stats stat = controllerS.Get();

            Assert.AreEqual(1, stat.count_human_dna);
            Assert.AreEqual(2, stat.count_mutant_dna);
            Assert.AreEqual(2.0m, stat.ratio);

        }

    }
}
