using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Telemetria
{
    public sealed class FilePersistance : PersistanceSystem
    {
        private string path;

        public FilePersistance(ISerializer ser, string path) : base(ser)
        {
            this.path = path;
        }

        protected override void Save(object data)
        {
            using (StreamWriter file = File.AppendText(path))   
            {
                file.WriteLine(data);
            }
        }
    }
}
