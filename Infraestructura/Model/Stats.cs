using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Model
{
    public class Stats
    {
        public int human { get; set; }

        public int mutant { get; set; }

        public decimal ratio
        {
            get
            {
                if (human > 0 && mutant > 0)
                    return (decimal) mutant / (decimal) human;
                else
                    return 0;
            }
        }
    }
}
