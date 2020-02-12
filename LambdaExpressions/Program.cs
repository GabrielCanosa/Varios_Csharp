using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            // Funciones anonimas

            Func<int, int> duplicar = x => { return x * 2; };
            Console.WriteLine(duplicar(35)); //OUTPUT = 70

            Func<int, int, int> multiplicar = (x, y) => { return x * y; };
            Console.WriteLine(multiplicar(3, 6)); //OUTPUT = 18

            // LINQ
            // Where()

            List<int> numeros = new List<int>()
            {
                10, -5, 46, 0, 8, -18, 27, -7, -33, -21, 40, 21
            };
            List<int> listaVacia = new List<int>();

            var numerosMayoresDeCero = numeros.Where(x => x > 0).ToList();
            var numerosMayoresDeCeroPares = numeros.Where(x => x > 0 && (x % 2 == 0)).ToList();
            var numerosDeIndicePar = numeros.Where((x, indice) => indice % 2 == 0).ToList();

            List<Persona> personas = new List<Persona>()
            {
                new Persona() { Nombre = "juan", Edad = 20},
                new Persona() { Nombre = "maria", Edad = 7},
                new Persona() { Nombre = "cristian", Edad = 51},
                new Persona() { Nombre = "claudia", Edad = 16},
                new Persona() { Nombre = "jose", Edad = 24},
                new Persona() { Nombre = "victoria", Edad = 24},
                new Persona() { Nombre = "tomas", Edad = 24}
            };

            var personasMayoresDeEdad = personas.Where(persona => persona.Edad >= 18).ToList();

            // OrderBy() y ThenBy() -> ordena de forma ascendente
            // OrderByDescending() y ThenByDescending() -> ordena de forma descendente

            var personasOrdenadasPorEdadAsc = personas.OrderBy(persona => persona.Edad);
            var personasOrdenadasPorEdadAscyAlfabeticamente = personas.OrderBy(persona => persona.Edad).ThenBy(persona => persona.Nombre).ToList();

            // First() y firstOrDefault()

            var primerElemento = numeros.FirstOrDefault(); // return 10
            var primerElemento2 = listaVacia.FirstOrDefault(); // return 0

            var primElemento = numeros.First(); // return 10
            //var primElemento2 = listaVacia.First(); // excepcion

            var primerElementoImparMayorDeCero = numeros.FirstOrDefault(x => x % 2 == 1); // return 27

            var primerPersonaMenorDeEdad = personas.Where(persona => persona.Edad < 18).Select(persona => persona.Nombre).FirstOrDefault(); //maria

            // Crea una nueva persona con los datos filtrados
            var primerPersonaMenorDeEdad2 = personas.Where(persona => persona.Edad < 18).Select(persona =>
            new Persona()
            {
                Nombre = persona.Nombre
            }).FirstOrDefault();

            var listadoPersonasMenoresDeEdad = personas.Where(persona => persona.Edad < 18).Select(persona =>
            new
            {
                Nombre = persona.Nombre
            }).ToList();

            // Skip() y Take()
            int n = 3;
            var primerosNElementos = numeros.Take(n).ToList(); // 10, -5, 46

            int r = 1;
            var primerosNElementosSaltandoRElementos = numeros.Skip(r).Take(n).ToList(); // -5, 46, 0

            // SkipWhile() TakeWhile()
            var numerosMenoresDeCuarenta = numeros.TakeWhile(num => num < 40).ToList(); // 10, -5

            var ignorarElementosHastaNumMayorDeCuarenta = numeros.SkipWhile(num => num < 40).ToList(); // 46, 0, 8, -18, 27, -7, -33, -21, 40, 21

            // GroupBy()

            var grupoNumeros = numeros.GroupBy(x => Math.Abs(x % 2));
            // Separa en grupos de pares e impares
            foreach (var grupo in grupoNumeros)
            {
                Console.WriteLine();
                foreach (var item in grupo)
                {
                    Console.Write(item + " ");
                }
            }

            Console.WriteLine();

            var gruposDePersonasSegunEdad = personas.GroupBy(x =>
            {
                if (x.Edad <= 10)
                {
                    return "menor de 10";
                }
                else if (x.Edad >= 11 && x.Edad <= 30)
                {
                    return "entre 11 y 30";
                }
                else
                {
                    return "mayores de 31";
                }
            });

            foreach (var grupo in gruposDePersonasSegunEdad)
            {
                Console.WriteLine("------------" + grupo.Key + "------------");
                foreach (var item in grupo)
                {
                    Console.WriteLine(item.Nombre + " " + item.Edad);
                }
            }

            // Any() y All()
            var todosLosNumerosSonPares = numeros.All(x => x % 2 == 0); // false (no todos los numeros son pares)
            var hayAlgunNumeroPar = numeros.Any(x => x % 2 == 0); // true (existe algun numero par)

            // Sum(), Min(), Max(), Average()
            var sumaDeTodosLosNumeros = numeros.Sum();
            var MinDeTodosLosNumeros = numeros.Min();
            var MaxDeTodosLosNumeros = numeros.Max();
            var PromedioDeTodosLosNumeros = numeros.Average();

            // Aggregate()
            List<int> numeros2 = new List<int>() { 2, 3, 4, 5 };
            var funcionAggregate = numeros2.Aggregate((anterior, actual) => anterior + actual);
            /*
             * Esta funcion hace lo siguiente:
             * 2 + 3 = 5
             * 5 + 4 = 9
             * 9 + 5 = 14
             * funcionAggregate = 14
             * */

            Console.Read();
        }
    }
    public class Persona
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }
}
