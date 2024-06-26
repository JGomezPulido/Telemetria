using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Telemetria
{
    public class FilePersistance : PersistanceSystem
    {
        protected string path;
        protected StreamWriter stream;
        public FilePersistance(ISerializer ser, string path) : base(ser)
        {
            this.path = path;
            FileStream file = File.Create(path);
            try
            {
                stream = new StreamWriter(file);
                stream.WriteLine(ser.Setup());
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Error: No se ha encontrado el archivo {file}");
                Console.WriteLine(e.Message);
            }
        }

        protected override void Save(object data)
        {
            try
            {
                stream.WriteLine(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fallo al intentar persistir el archivo \n");
                Console.WriteLine(e.Message + "\n");
            }
        }

        public override void Close()
        {
            try
            {
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Fallo al intentar cerrar \n");
                Console.WriteLine(e.Message);
            }
        }
    }
}
