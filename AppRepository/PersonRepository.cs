using Infraestructura.Exceptions;
using Infraestructura.IRepository;
using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository
{
    /// <summary>
    /// Simular una BD en Memoria de Persona
    /// CREATE TABLE Persons (
    ///    DNA      VARCHAR(36) NOT NULL,
    ///    Mutant   TINYINT,
    ///    PRIMARY KEY(DNA)
    ///);
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private static IList<Person> datos = new List<Person>();


        /// <summary>
        /// INSERT
        /// </summary>
        /// <param name="person"></param>
        public void Guardar(Person person)
        {
            if (!VerificarExistente(person))
                datos.Add(person);
            else
                throw new ValidationException("PK violated");
        }

        /// <summary>
        /// Simular la PrImary Key
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        public bool VerificarExistente(Person persona)
        {
            return datos.Any(p => persona.dna.Equals(p.dna));
        }

        /// <summary>
        /// Simula consultas con funciones de agregación 
        /// que se harían sobre la BD
        /// </summary>
        /// <returns></returns>
        public Stats GetStats()
        {
            var stats = new Stats();

            stats.human = datos.Count(p => p.isMutant == false);

            stats.mutant = datos.Count(p => p.isMutant == true);

            return stats;
        }
    }
}
