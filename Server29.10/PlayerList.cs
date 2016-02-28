using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server29._10
{
    [XmlRootAttribute("player_list")]
    public class PlayerList : BaseCommand
    {
        public class Player
        {
            private string name;
            [XmlElement("name")]
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            [XmlIgnore()]
            private Color color;
            [XmlElement]
            public int ColorCode
            {
                get { return color.ToArgb(); }
                set { color = Color.FromArgb(value); }
            }
            public Player(string name, Color color)
            {
                this.name = name;
                this.color = color;
            }
            public Player()
            {
                this.name = "";
                this.color = Color.FromArgb(0, 0, 0);
            }
        }
        private List<Player> players;
        [XmlArray("players")]
        [XmlArrayItem("player")]
        public List<Player> Players
        {
            get {return players;}
        }

        public PlayerList()
        {
            id = 3;
            players = new List<Player>();
        }
        public PlayerList(List<Player> list)
        {
            id = 3;
            players = list;
        }   
    }
}
