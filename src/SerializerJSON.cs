using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetria
{
    // Clase que se encarga de serializar los eventos en formato JSON
    public class SerializerJSON
    {
        string jsonFilePath = "eventos.json";
        public void exportar(List<Event> events)
        {
            // Abrir el archivo en modo escritura
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(jsonFilePath))
            {
                // Escribir el encabezado del archivo JSON
                file.WriteLine("[");
                // Escribir cada evento como un objeto JSON en el archivo
                for (int i = 0; i < events.Count; i++)
                {
                    // Serializar el evento a JSON
                    string json = events[i].serialize();
                    //Separar el JSON en sus partes
                    string[] parts = json.Split(',');
                    // Escribir el objeto JSON en el archivo
                    file.WriteLine("{");
                    file.WriteLine($"\"UserId\": \"{parts[0]}\",");
                    file.WriteLine($"\"SessionID\": \"{parts[1]}\",");
                    file.WriteLine($"\"EventType\": \"{parts[2]}\",");
                    if(parts.Length < 5)
                    {
                        file.WriteLine($"\"Timestamp\": {parts[3]}");
                    }
                    else
                    {
                        file.WriteLine($"\"Timestamp\": {parts[3]},");
                        if (parts.Length < 6)
                        {

                            file.WriteLine($"\"ItemName\": {parts[4]}");
                        }
                        else
                        {
                            file.WriteLine($"\"ItemName\": {parts[4]},");
                            file.WriteLine($"\"Position\": {parts[5]}, {parts[6]}");
                        }
                    }
                    file.WriteLine("}");
                    // Si no es el último evento, escribir una coma
                    if (i < events.Count - 1)
                    {
                        file.WriteLine(",");
                    }
                }
                // Escribir el pie del archivo JSON
                file.WriteLine("]");
            }
        }
    }
}
