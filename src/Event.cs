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
        public string event_type { get; internal set; }
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
        public StartSession()
        {
            event_type = "StartSession";
        }
    }

    public class EndSession : Event
    {
        public EndSession()
        {
            event_type = "EndSession";

        }
    }

    public class StartGame : Event
    {
        public StartGame()
        {
            event_type = "StartGame";

        }
    }

    public class EndGame : Event
    {
        public EndGame()
        {
            event_type = "EndGame";

        }
    }

    public class GameAborted : Event
    {
        public GameAborted()
        {
            event_type = "GameAborted";

        }
    }

    // Eventos de Dinosouls
    public class GetItem : Event
    {
        string item_name { get; set; }
        float position_x { get; set; }
        float position_y { get; set; }
        public GetItem(string itemN, float x, float y)
        {
            event_type = "GetItem";
            position_x = x;
            position_y = y;
            item_name = itemN;

        }

        public override string serialize()
        {
            return base.serialize() + $"{item_name},{position_x},{position_y}";
        }
    }

    public class UseItem : Event
    {
        string item_name { get; set; }
        public UseItem(string itemN, float x, float y)
        {
            event_type = "UseItem";
            item_name = itemN;
        }

        public override string serialize()
        {
            return base.serialize() + $"{item_name}";
        }
    }

}


