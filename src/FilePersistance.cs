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
            stream = new StreamWriter(file);
            stream.WriteLine(ser.Setup());
        }

        protected override void Save(object data)
        {
            
            stream.WriteLine(data);
            
        }

        public override void Close()
        {
            stream.Close();
        }
    }
}
