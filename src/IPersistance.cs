using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    public interface IPersistance
    {
        void Save(Event e);
        abstract void Close();
    }
}
