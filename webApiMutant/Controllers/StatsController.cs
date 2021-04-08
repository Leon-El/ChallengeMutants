using Infraestructura.IService;
using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webApiMutant.Controllers
{
    public class StatsController : ApiController
    {
        private readonly IStatsService statServ;

        public StatsController(IStatsService sServ)
        {
            statServ = sServ;
        }

        /// <summary>
        /// Obtiene las estadisticas de todo los adn analizados con exito
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Stats Get()
        {
            return statServ.getStat();
        }
    }
}
