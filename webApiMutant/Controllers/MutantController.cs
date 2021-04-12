using Infraestructura.Exceptions;
using Infraestructura.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webApiMutant.Filter;
using webApiMutant.Models;

namespace webApiMutant.Controllers
{
    [ValidacionesExceptionFilter]
    public class MutantController : ApiController
    {
        private readonly IMutantService mutantServ;

        public MutantController(IMutantService mServ)
        {
            mutantServ = mServ;
        }

        [HttpPost]
        public HttpResponseMessage Mutant([FromBody] PersonDTO person)
        {
            if (mutantServ.IsMutant(person.dna))
                return Request.CreateResponse(HttpStatusCode.OK, "Mutante");
            else
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Humano");
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            throw new ValidationException("Sólo soporta POST");

        }

        /// <summary>
        /// Borra los datos del repositorio
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete()
        {
            mutantServ.DeleteAll();
            return Ok("Borrado con exito");
        }
    }
}
