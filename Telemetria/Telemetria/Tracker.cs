using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Telemetria
{
    public class Tracker
    {
        private static Tracker? _instance = null;

        public static Tracker? Instance { get { return _instance; } }

        List<Event> eventsList = new List<Event>();
        List<Event> pendingEvents = new List<Event>();
        private Tracker()
        {

        }

        public static void Init()
        {
            _instance = new Tracker();
        }
    }
}
