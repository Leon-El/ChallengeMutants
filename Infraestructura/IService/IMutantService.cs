using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.IService
{
    public interface IMutantService
    {
        bool IsMutant(IEnumerable<string> dna);

        void DeleteAll();
    }
}
