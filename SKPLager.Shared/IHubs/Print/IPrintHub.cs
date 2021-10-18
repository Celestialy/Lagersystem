using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.IHubs
{
    public interface IPrintHub
    {
        Task PrintRequest(string barcode); //Måske os en stregkode
    }
}
