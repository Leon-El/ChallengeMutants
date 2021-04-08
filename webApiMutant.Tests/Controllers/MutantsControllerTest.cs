using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Infraestructura.IService;
using System.Web;
using webApiMutant.Controllers;
using webApiMutant.Models;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Infraestructura.Exceptions;

namespace webApiMutant.Tests.Controllers
{
    [TestClass]
    public class MutantsControllerTest
    {
        private IUnityContainer container;

        private IMutantService mutantServ;

        [TestInitialize]
        public void TestInitialize()
        {
            HttpContext.Current = new HttpContext(
                       new HttpRequest("", "http://tempuri.org", ""),
                       new HttpResponse(new System.IO.StringWriter())
                   );

            container = Unity.UnityConfig.GetConfiguredContainer();
            mutantServ = container.Resolve<IMutantService>();

        }
        [TestMethod]
        public void TestIsHuman()
        {
            MutantController controller = new MutantController(mutantServ);

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey,
                                              new HttpConfiguration());

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

            var resp = controller.Mutant(persona);

            Assert.AreEqual(HttpStatusCode.Forbidden, resp.StatusCode);
        }

        [TestMethod]
        public void TestIsMutan()
        {
            MutantController controller = new MutantController(mutantServ);

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey,
                                              new HttpConfiguration());

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

            var resp = controller.Mutant(mutante);

            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }


        [TestMethod]
        public void InvalidDna()
        {
            MutantController controller = new MutantController(mutantServ);

            string mensaje = null;

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey,
                                              new HttpConfiguration());

            PersonDTO invalid = new PersonDTO()
            {
                dna = new List<string>() {
                    "AtGCGA",
                    "CAGTGC",
                    "TTATGT",
                    "AGAAGG",
                    "CCCCTA",
                    "TCACTG"}
            };

            try
            {
                var resp = controller.Mutant(invalid);

                Assert.AreEqual(HttpStatusCode.BadRequest, resp.StatusCode);
            }
            catch (ValidationException ex)
            {
                mensaje = ex.Message;

            }

            Assert.IsNotNull(mensaje);

        }
    }
}
