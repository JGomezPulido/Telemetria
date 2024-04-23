using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    public class SerializerCSV : ISerializer
    {
        public object Serialize(Event e)
        {
            using (StringWriter sw = new StringWriter())
            {
                var properties = e.GetType().GetProperties();
                foreach (var property in properties)
                    sw.Write($"{property.Name},{property.GetValue(e)},");

                return sw.ToString().TrimEnd(',');
            }
        }
    }
}
