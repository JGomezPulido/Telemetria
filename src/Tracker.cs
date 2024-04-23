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
        CancellationTokenSource _cancellationTokenSource;

        private ConcurrentQueue<Event> eventsQueue;
        private Thread persistThread;

        private string userId;
        private string sessionId;
        private int gameId;

        private const UInt32 SAVING_FREQ = 10000;

        private Tracker(string userId)
        {
            eventsQueue = new ConcurrentQueue<Event>();
            this.userId = userId;
            this.sessionId = Guid.NewGuid().ToString();
            gameId = -1;
            _cancellationTokenSource = new CancellationTokenSource();
            persistThread = new Thread(() => ThreadLoop(_cancellationTokenSource.Token));
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
            TrackEvent(new EndSession());
            _cancellationTokenSource.Cancel();
            persistThread.Join();
            Save();
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
            evt.id_game = HashCode.Combine(gameId,userId,sessionId).ToString();
            eventsQueue.Enqueue(evt);
        }
        private void ThreadLoop(CancellationToken tk)
        {
            while (true)
            {
                int result = WaitHandle.WaitAny(new WaitHandle[] { tk.WaitHandle }, TimeSpan.FromMilliseconds(SAVING_FREQ));

                if (result == WaitHandle.WaitTimeout)
                    break;
                Save();
            }
        }
        private void Save()
        {
            while(eventsQueue.TryDequeue(out var evt)) {
                persister.Persist();
            }
        }
    }
}
