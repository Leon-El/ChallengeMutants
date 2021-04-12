using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Model
{
    public class Stats
    {
        public int count_mutant_dna { get; set; }

        public int count_human_dna { get; set; }

        public decimal ratio
        {
            get
            {
                if (count_human_dna > 0 && count_mutant_dna > 0)
                    return (decimal) count_mutant_dna / (decimal) count_human_dna;
                else
                    return 0;
            }
        }
    }
}
