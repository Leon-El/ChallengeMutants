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

            PersonDTO persona2 = new PersonDTO()
            {
                dna = new List<string>() {
                    "ATGCGA",
                    "CAGTGC",
                    "TTATTT",
                    "AGACGG",
                    "GCGTCA",
                    "AAAAAA" }
            };

            var resp = controller.Mutant(persona);
            var resp2 = controller.Mutant(persona2);

            Assert.AreEqual(HttpStatusCode.Forbidden, resp.StatusCode);
            Assert.AreEqual(HttpStatusCode.Forbidden, resp2.StatusCode);
        }

        [TestMethod]
        public void TestIsMutan()
        {
            MutantController controller = new MutantController(mutantServ);

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey,
                                              new HttpConfiguration());

            //ejemplo
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

            //dos columnas
            PersonDTO mutante2Col = new PersonDTO()
            {
                dna = new List<string>() {
                    "ATGCGA",
                    "ACGTGC",
                    "ATATGT",
                    "AGAAGG",
                    "GCCCTA",
                    "ACACTG"}
            };

            //dos filas
            PersonDTO mutante2Row = new PersonDTO()
            {
                dna = new List<string>() {
                    "TTTTGA",
                    "ACGTTC",
                    "ATATGT",
                    "AGAAGG",
                    "GCCCCC",
                    "ACACTG"}
            };

            //2 diagonales
            PersonDTO mutante2Diag = new PersonDTO()
            {
                dna = new List<string>() {
                    "TATTGA",
                    "ACGTTC",
                    "ATATGT",
                    "AGTAGG",
                    "GCCTAC",
                    "ACACTA"}
            };

            var resp = controller.Mutant(mutante);
            var resp2col = controller.Mutant(mutante2Col);
            var resp2row = controller.Mutant(mutante2Row);
            var resp2diag = controller.Mutant(mutante2Diag);

            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, resp2col.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, resp2row.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, resp2diag.StatusCode);
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
