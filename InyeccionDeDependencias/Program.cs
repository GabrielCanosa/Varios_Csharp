using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InyeccionDeDependencias
{
    class Program
    {
        static void Main(string[] args) { }
    }

    public class Frase
    {
        private IDecirUnaFrase decirUnaFrase;

        public Frase(IDecirUnaFrase _decirUnaFrase)
        {
            _decirUnaFrase = decirUnaFrase;
        }

        public void LlamadoAlMetodoDecirUnaFrase()
        {
            decirUnaFrase.DecirUnaFrase();
            // Ya podemos acceder al metodo DecirUnaFrase() gracias a la inyeccion de dependencias
            // que realizamos en el constructor
        }
    }
}
