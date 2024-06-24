using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetria;
using System.IO;
using System.Reflection;

namespace Telemetria
{
    public class SerializerCSV : ISerializer
    {
        private List<string> _gameProperties;

        /// <summary>
        /// Build a CSV serializar that has the default columns for a tracker event plus the columns specified
        /// </summary>
        /// <param name="properties">The assembly in which the events from the game are defined</param>
        public SerializerCSV(Assembly eventsAssembly) {
            _gameProperties = new List<string>();
            foreach (var property in typeof(Telemetria.Event).GetProperties()) {
                _gameProperties.Add(property.Name);
            }
            
           foreach(var type in eventsAssembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Telemetria.Event)))
                {
                    foreach(var property in type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public|BindingFlags.Instance))
                    {
                        if(!_gameProperties.Contains(property.Name))
                            _gameProperties.Add(property.Name);
                    }
                }
            }
        }
        public object Serialize(Event e)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type type = e.GetType();
                
                foreach (var property in _gameProperties)
                {
                    var propertyInfo = type.GetProperty(property);
                    if (propertyInfo != null)
                    {
                        sw.Write($"{propertyInfo.GetValue(e)};");
                    }
                    else
                    {
                        sw.Write(";");
                    }
                }
                return sw.ToString().TrimEnd(';');
            }
        }

        public string Setup()
        {
            string final = "";
           for(int i = 0; i < _gameProperties.Count; i++)
           {
                final += _gameProperties[i];
                final += ";";
           }
            return final;
        }
    }
}
