using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Telemetria
{

    public class SerializerCSV
    {

        string csvFilePath = "eventos.csv";
        public void exportar(List<Event> events)
        {
            // Abrir el archivo en modo escritura
            using (StreamWriter sw = new StreamWriter(csvFilePath))
            {

                sw.WriteLine("UserId, SessionID, EventType, Timestamp, ItemName, Position");

                // Escribir cada evento como una fila en el archivo CSV
                foreach (Event ev in events)
                {

                    // Escribir la línea en el archivo
                    string csvLine = ev.serialize();

                    sw.WriteLine(csvLine);
                }

            }
        }
    }
}
