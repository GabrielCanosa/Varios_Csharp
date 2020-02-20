using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InyeccionDeDependencias
{
    public class DecirUnaFrase : IDecirUnaFrase
    {
        string IDecirUnaFrase.DecirUnaFrase()
        {
            return "Tengo la intención de vivir para siempre, o morir en el intento";
        }
    }
}
