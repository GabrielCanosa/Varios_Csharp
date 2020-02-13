using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfacesAndAbstractClass
{
    class Program
    {
        static void Main(string[] args)
        {
            // Diferencia entre clases abstractas e interfaces
            // 1. Las clases abstractas pueden definir variables
            // 2. Las clases abstractas puede definir miembros protected (solo accesibles para la misma clase o derivadas)

            ClaseQueHeredaDeLaClaseAbstracta siHereda = new ClaseQueHeredaDeLaClaseAbstracta();
            siHereda.MiPropiedad = "hola, como estas?";
            // sale el siguiente error: Program.ClaseAbstracta.MiPropiedad is innaccesible due to its protection level
        }

        abstract class ClaseAbstracta
        {
            protected string MiPropiedad { get; set; }
            // protected: el miembro solo puede ser accedido desde la misma clase o una clase derivada

            public abstract void multiplicarNumero(int numero);
            public virtual void implementacionPorDefecto(int numero) => multiplicarNumero(numero * 2);

            /* => multiplicarNumero(numero * 2);
             * esto es una implementacion por defecto
             * y estan permitidas en las clases abstractas
             */
        }

        interface IInterfaceAbstracta
        {
            // La interfaz SI obliga a implementar los dos metodos. a diferencia de la clase abstracta

            void multiplicarNumero(int numero);
            void implementacionPorDefecto(int numero) => multiplicarNumero(numero * 2);
            /*
             * Aparece el siguiente error:
             ... interface members cannot have a definition
             Pero esto cambió a partir de C# 8 (.NET Core 3)
             Ahora se permiten las implementaciones por defecto en las interfaces...
             no me crees? pruébalo!
            */
        }

        // Las clases abstractas nos permiten tener un nivel de encapsulamiento que las interfaces no
        // lo que??
        // mejor lo vemos con un ejemplo:

        class ClaseQueHeredaDeLaClaseAbstracta : ClaseAbstracta
        {
            public override void multiplicarNumero(int numero)
            {
                MiPropiedad = "Hola, como estas?";
            }

            // En esta clase no estamos sobreescribiendo el metodo implementacionPorDefecto
            // ya que tenemos la opcion de usar su implementacion por defecto, valga la redundancia.
        }
    }
}
