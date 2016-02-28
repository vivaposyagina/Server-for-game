using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    public enum direction { N, E, S, W };
    [XmlRootAttribute("player_move")]
    public class PlayerMove : BaseCommand
    {
        private direction direction;

        [XmlElement("direction")]
        public direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public PlayerMove(direction d)
        {
            this.direction = d;
            id = 10;
        }
        public PlayerMove()
        {            
            id = 10;
        }
        
    }
}
