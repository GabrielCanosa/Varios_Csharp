using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    class Program
    {
        #region Delegados

        delegate int DelegadoSumar(int num1, int num2);
        delegate int DelegadoMultiplicar(int num1, int num2);
        delegate Persona DelegadoPersona(int id, string nombre, string apellido, int edad, Telefono telf);
        #endregion

        static void Main(string[] args)
        {
            int num1 = 4, num2 = 5;
            var delegado = new DelegadoSumar(Sumar);
            Console.WriteLine(delegado(num1, num2));

            delegado = new DelegadoSumar(Multiplicar);
            Console.WriteLine(delegado(num1, num2));

            /////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////

            Telefono telf = new Telefono();
            telf.CodigoArea = 11;
            telf.Numero = "1111-2222";

            var dPersona = new DelegadoPersona(CrearPersona);
            Persona p = dPersona(1, "Jose", "Perez", 32, telf);

            Console.WriteLine("Id: {0}\nNombre: {1},\nApellido: {2},\nEdad: {3},\nCodigo de área: {4},\nNumero de telefono: {5}"
                , p.Id, p.Nombre, p.Apellido, p.Edad, telf.CodigoArea, telf.Numero);

            /////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////

            // Func exige que la funcion tenga valores de salida
            Func<string, string> delegadoFunc = FuncConvierteMayusculas;
            var resultado = delegadoFunc("esto es un simple string");
            Console.WriteLine(resultado);

            // Func exige que la funcion NO tenga valores de salida
            Action<string, int> delegadoAction = NoHaceNada;

            /*
             * Func y Action son dos formas de utilizar delegados sin crear variables globales
             * */

            /////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////

            // Predicados
            // Siempre retorna un bool y recibe un único parametro

            Predicate<int> predicado = EsMayorDeEdad;
            int Edad = 17;
            if (predicado(Edad))
                Console.WriteLine("es mayor de edad");
            else
                Console.WriteLine("no es mayor de edad");

            Console.ReadKey();
        }

        #region Funciones

        public static int Sumar(int num1, int num2) { return num1 + num2; }
        public static int Multiplicar(int num1, int num2) { return num1 * num2; }
        public static Persona CrearPersona(int id, string nombre, string apellido, int edad, Telefono telf)
        {
            Persona persona = new Persona()
            {
                Id = id,
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                numTelefono = telf
            };
            return persona;
        }
        public static Telefono CrearTelefono(int codArea, string numero)
        {
            Telefono telefono = new Telefono()
            {
                CodigoArea = codArea,
                Numero = numero
            };
            return telefono;
        }
        public static string FuncConvierteMayusculas(string s1)
        {
            return s1.ToUpper();
        }
        public static void NoHaceNada(string s1, int num1)
        {

        }
        private static bool EsMayorDeEdad(int edad)
        {
            if (edad >= 18)
                return true;
            else
                return false;
        }

        #endregion
    }

    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public Telefono numTelefono { get; set; }
    }

    public class Telefono
    {
        public int CodigoArea { get; set; }
        public string Numero { get; set; }
    }
}
