using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    public abstract class PersistanceSystem : IPersistance
    {
        private readonly ISerializer serializer;
        
        public PersistanceSystem(ISerializer ser)
        {
            this.serializer = ser;
        }

        protected abstract void Save(object data);

        public void Save(Event e)
        {
            Save(serializer.Serialize(e));
        }
    }
}
