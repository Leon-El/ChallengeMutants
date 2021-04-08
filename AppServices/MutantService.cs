using Infraestructura.Exceptions;
using Infraestructura.IRepository;
using Infraestructura.IService;
using Infraestructura.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppServices
{
    public class MutantService : IMutantService
    {
        private readonly IPersonRepository personR;

        private readonly string VALID_DNA = ConfigurationManager.AppSettings["ValidDna"];

        private readonly int MAX_DNA_ARRAY = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDnaArray"]);

        private readonly int MAX_SEQUENCE = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSequence"]);

        public MutantService(IPersonRepository pRepo)
        {
            this.personR = pRepo;
        }

        public bool IsMutant(IEnumerable<string> dna)
        {
            bool isMutant = false;
            StringBuilder builder = new StringBuilder();

            //Regex Validator = new Regex(@"^"+ VALID_DNA+ "+$");
            //queda más lindo con linq: (dna.All(s => s.All(c => VALID_DNA.Contains(c)))

            if (dna.All(s => s.All(c => VALID_DNA.Contains(c))) &&
                dna.All(p => p.Count() == MAX_DNA_ARRAY &&
                dna.Count() == MAX_DNA_ARRAY))
            {

                #region find mutant gen
                for (int row = 0; row <= (MAX_DNA_ARRAY - MAX_SEQUENCE); row++)
                {
                    for (int col = 0; col <= (MAX_DNA_ARRAY - MAX_SEQUENCE); col++)
                    {
                        var elemento = dna.ElementAt(row).ElementAt(col);

                        if (validateRows(dna, elemento, MAX_SEQUENCE - 1, row, col + 1) ||
                            validateColumns(dna, elemento, MAX_SEQUENCE - 1, row + 1, col) ||
                            validateDiagonals(dna, elemento, MAX_SEQUENCE - 1, row + 1, col + 1))
                        {
                            isMutant = true;
                            break;
                        }
                    }

                    if (isMutant)
                        break;
                }
                #endregion

                #region save PersonOfInterest
                foreach (var fila in dna)
                {
                    builder.Append(fila);
                }

                var personOfInterest = new Person()
                {
                    isMutant = isMutant,
                    dna = builder.ToString()
                };

                personR.Guardar(personOfInterest);
                #endregion

                return isMutant;
            }
            else
                throw new ValidationException("Error en la composición de ADN");

        }

        private bool validateRows(IEnumerable<string> dna, char c, int repeats, int row, int col)
        {
            if (dna.ElementAt(row).ElementAt(col) == c)
            {
                if (repeats == 1)
                    return true;
                else
                    return validateRows(dna, c, repeats - 1, row, col + 1);
            }
            else
                return false;
        }

        private bool validateColumns(IEnumerable<string> dna, char c, int repeats, int row, int col)
        {
            if (dna.ElementAt(row).ElementAt(col) == c)
            {
                if (repeats == 1)
                    return true;
                else
                    return validateColumns(dna, c, repeats - 1, row + 1, col);
            }
            else
                return false;
        }

        private bool validateDiagonals(IEnumerable<string> dna, char c, int repeats, int row, int col)
        {
            if (dna.ElementAt(row).ElementAt(col) == c)
            {
                if (repeats == 1)
                    return true;
                else
                    return validateDiagonals(dna, c, repeats - 1, row + 1, col + 1);
            }
            else
                return false;
        }
    }
}
