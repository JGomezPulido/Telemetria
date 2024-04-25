using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Telemetria;

namespace Telemetria
{
    public abstract class Event
    {
        public string id_user { get; internal set; }
        public string id_session { get; internal set; }
        public string event_type { get; protected set; }
        public string id_game { get; internal set; }
        public long send_time { get; protected set; }


        public Event()
        {
            id_user = "0";
            id_session = "";
            id_game = "";
            event_type = "";
            send_time = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public virtual string serialize()
        {
            return $"{id_user},{id_session},{event_type},{send_time}";
        }
    }

    // Eventos basicos
    public class StartSession : Event
    {
        public StartSession() : base()
        {
            
            event_type = "StartSession";
        }
    }

    public class EndSession : Event
    {
        public EndSession() : base()
        {
            event_type = "EndSession";

        }
    }

    public class StartGame : Event
    {
        public StartGame() : base()
        {
            event_type = "StartGame";

        }
    }

    public class EndGame : Event
    {
        public EndGame() : base()
        {
            event_type = "EndGame";

        }
    }

    public class GameAborted : Event
    {
        public GameAborted() : base()
        {
            event_type = "GameAborted";

        }
    }


}


