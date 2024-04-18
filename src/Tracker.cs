using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;

namespace Telemetria
{
    public class Tracker
    {
        private static Tracker? _instance;
        public static Tracker? Instance { get { return _instance; } }


        private ConcurrentQueue<Event> eventsQueue = new ConcurrentQueue<Event>();
        private List<Event> pendingEvents = new List<Event>();
        private Thread? savingThread; 
        private Tracker()
        {

        }

        public static void Init()
        {
            _instance = new Tracker();
        }
        public static  void Close()
        {

        }
        public void trackEvent(in Event evt)
        {
            eventsQueue.Enqueue(evt);


        }

    }
}
