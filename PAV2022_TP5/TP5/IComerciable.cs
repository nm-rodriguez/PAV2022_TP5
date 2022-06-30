using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP5
{
    interface IComerciable: IImpuesto
    {
        double CostoSinIva { get; set; }
        double CostoConIVA { get;}
        double MargenGanancia { get; set; }
        double PrecioFinalVenta { get;}

        double GetCostoConIVA();
        double GetPrecioFinal();
        double GetMargenGanancia();
    }
}
