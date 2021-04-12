using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.IRepository
{
    public interface IPersonRepository
    {
        void Guardar(Person persona);

        bool VerificarExistente(Person persona);

        Stats GetStats();

        void DeleteAll();

    }
}
