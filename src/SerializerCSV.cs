using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    public class SerializerCSV
    {
        public object Serialize(Event e)
        {
            using (StringWriter sb = new StringWriter())
            {
                var properties = e.GetType().GetProperties();
                foreach (var property in properties)
                    sb.Write($"{property.Name},{property.GetValue(e)},");

                return sb.ToString().TrimEnd(',');
            }
        }
    }
}
