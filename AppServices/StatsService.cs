using Infraestructura.IRepository;
using Infraestructura.IService;
using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServices
{
    public class StatsService : IStatsService
    {
        private readonly IPersonRepository statR;


        public StatsService(IPersonRepository sRepo)
        {
            this.statR = sRepo;
        }

        public Stats getStat()
        {
            return statR.GetStats();
           
        }
    }
}
