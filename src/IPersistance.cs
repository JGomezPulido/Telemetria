using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    internal interface IPersistance
    {
        void Save(Event e);

    }
}
