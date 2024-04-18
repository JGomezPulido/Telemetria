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


        private ConcurrentQueue<Event> eventsQueue;
        private List<Event> pendingEvents;
        private Thread persistThread;

        private string userId;
        private string sessionId;
        private int gameId;

        private Tracker(string userId)
        {
            eventsQueue = new ConcurrentQueue<Event>();
            pendingEvents = new List<Event>();
            this.userId = userId;
            this.sessionId = Guid.NewGuid().ToString();
            gameId = -1;
            persistThread = new Thread(() => ThreadLoop());
        }

        public static bool Init(string userId)
        {
            _instance = new Tracker(userId);
            _instance.TrackEvent(new StartSession());
            try
            {
                _instance.StartThread();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        private void StartThread()
        {
            persistThread.Start();
        }
        public static void Close()
        {
            if (_instance == null)
            {
                return;
            }
            _instance?.End();

        }
        private void End()
        {
            persistThread.Join();
        }
        public void TrackEvent(in Event evt)
        {
            //Preparar el evento con los datos de la sesion
            if( ((StartSession) evt) !=null)
            {
                gameId++;
            }
            evt.id_session = sessionId;
            evt.id_user = userId;
            evt.id_game = gameId.ToString();
            eventsQueue.Enqueue(evt);
        }
        private void ThreadLoop()
        {

        }
        private void Save()
        {

        }
    }
}
