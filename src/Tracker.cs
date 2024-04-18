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
        private Thread? persistThread; 
        private Tracker()
        {
            persistThread = new Thread(()=>threadLoop());
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
            //Preparar el evento para con los datos de la sesion
            eventsQueue.Enqueue(evt);
        }
        public void 
    }
}
