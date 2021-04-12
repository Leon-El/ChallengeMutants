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

        private readonly int MIN_MATCHES = Convert.ToInt32(ConfigurationManager.AppSettings["MinDnaMatches"]);

        public MutantService(IPersonRepository pRepo)
        {
            this.personR = pRepo;
        }


        public bool IsMutant(IEnumerable<string> dna)
        {
            int isMutant = 0;


            StringBuilder builder = new StringBuilder();

            //Regex Validator = new Regex(@"^"+ VALID_DNA+ "+$");
            //queda más lindo con linq: (dna.All(s => s.All(c => VALID_DNA.Contains(c)))


            ///valida caracteres, logitud de filas y columnas
            if (dna.All(s => s.All(c => VALID_DNA.Contains(c))) &&
                dna.All(p => p.Count() == MAX_DNA_ARRAY &&
                dna.Count() == MAX_DNA_ARRAY))
            {

                #region find mutant gen

                isMutant = searchInRows(dna, isMutant);
                isMutant = searchInColumns(dna, isMutant);
                isMutant = searchInDiagonals(dna, isMutant);


                #endregion

                #region save PersonOfInterest
                foreach (var fila in dna)
                {
                    builder.Append(fila);
                }

                var personOfInterest = new Person()
                {
                    isMutant = isMutant >= MIN_MATCHES,
                    dna = builder.ToString()
                };

                personR.Guardar(personOfInterest);
                #endregion

                return isMutant >= MIN_MATCHES;
            }
            else
                throw new ValidationException("Error en la composición de ADN");

        }

        private int searchInRows(IEnumerable<string> dna, int matches)
        {

            for (int row = 0; row < MAX_DNA_ARRAY; row++)
            {
                for (int col = 0; col <= (MAX_DNA_ARRAY - MAX_SEQUENCE); col++)
                {
                    var dnaElement = dna.ElementAt(row).ElementAt(col);

                    if (validateRows(dna, dnaElement, MAX_SEQUENCE - 1, row, col + 1))
                    {
                        matches++;

                        //seteo para que no siga buscando dentro de esa secuencia
                        col = col + (MAX_SEQUENCE - 1);
                    }

                    if (matches >= MIN_MATCHES)
                        break;
                }
            }
            return matches;
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

        private int searchInColumns(IEnumerable<string> dna, int matches)
        {

            for (int col = 0; col < MAX_DNA_ARRAY; col++)
            {
                for (int row = 0; row <= (MAX_DNA_ARRAY - MAX_SEQUENCE); row++)
                {
                    var dnaElement = dna.ElementAt(row).ElementAt(col);

                    if (validateColumns(dna, dnaElement, MAX_SEQUENCE - 1, row + 1, col))
                    {
                        matches++;

                        //seteo para que no siga buscando dentro de esa secuencia
                        row = row + (MAX_SEQUENCE - 1);
                    }

                    if (matches >= MIN_MATCHES)
                        break;
                }
            }
            return matches;
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

        private int searchInDiagonals(IEnumerable<string> dna, int matches)
        {

            List<int[]> diagonalException = new List<int[]>();

            for (int row = 0; row <= (MAX_DNA_ARRAY - MAX_SEQUENCE); row++)
            {
                if (matches >= MIN_MATCHES)
                    break;

                for (int col = 0; col <= (MAX_DNA_ARRAY - MAX_SEQUENCE); col++)
                {
                    var dnaElement = dna.ElementAt(col).ElementAt(row);

                    //valido que exista dentro de la lista exceptuados
                    var exceptionRowCol = diagonalException.Count() > 0 &&
                        diagonalException.Any(e => e.ElementAt(0) == row && e.ElementAt(1) == col);

                    if (exceptionRowCol == false && validateDiagonals(dna, dnaElement, MAX_SEQUENCE - 1, col + 1, row + 1))
                    {
                        matches++;
                        //agrego filas y col para que no repita la busqueda dentro de esa secuencia

                        for (int size = 1; size < MAX_SEQUENCE; size++)
                            diagonalException.Add(new int[] { row + size, col + size });
                    }

                    if (matches >= MIN_MATCHES)
                        break;
                }

            }
            return matches;
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


        public void DeleteAll()
        {
            personR.DeleteAll();
        }
    }
}
