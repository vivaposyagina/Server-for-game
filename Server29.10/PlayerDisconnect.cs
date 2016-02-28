using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("player_disconnect")]
    public class PlayerDisconnect : BaseCommand
    {
        public PlayerDisconnect()
        {
            id = 12;
        }
    }
}
